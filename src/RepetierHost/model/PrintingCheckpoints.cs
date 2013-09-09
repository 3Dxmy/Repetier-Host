using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using RepetierHost.view.utils;

namespace RepetierHost.model
{
    /// <summary>
    /// This class works as an entry point to access checkpoints.
    /// </summary>
    public class PrintingCheckpoints
    {
        internal BasicList<GCodeCompressed> jobGCodes;
        internal BasicList<PrintingCheckpoint> checkpoints;
        internal ZLayerCheckpointIndex checkpointIndex;
        internal PrinterConnection conn;
        internal bool jobActive;

        public PrintingCheckpoints(PrinterConnection conn)
        {
            // TODO Give an option to make checkpoints list configurable (in
            // memory or in disk).
            checkpoints = new InMemoryLinkedListBasicList<PrintingCheckpoint>();
            checkpointIndex = new ZLayerCheckpointIndex();
            this.conn = conn;
            jobActive = false;
        }

        public void BeginJob()
        {
            // TODO Give an option to make jobGCode list configurable (in
            // memory or in disk).
            jobGCodes = new InMemoryLinkedListBasicList<GCodeCompressed>(conn.connector.Job.GetPendingJobCommands());
            checkpoints = new InMemoryLinkedListBasicList<PrintingCheckpoint>();
            jobActive = true;
        }

        public void CreateCheckpoint(CheckpointType type)
        {
            if (jobActive)
            {
                PrintingCheckpoint chk = PrintingCheckpoint.GenerateCheckpoint(type, this);
                if (checkpoints.Count == 0 || checkpoints.Last.z != chk.z)
                {
                    // Layer changed, add an index entry.
                    checkpointIndex.AddIndexEntry(chk.z, checkpoints.Count);
                }
                checkpoints.AddLast(chk);
            }
        }

        public BasicList<PrintingCheckpoint> CheckPoints
        {
            get
            {
                return checkpoints;
            }
        }
    }


    /// <summary>
    /// This class represents the type of a checkpoint.
    /// </summary>
    public interface CheckpointType
    {
        string FormatName(PrintingCheckpoint chk);
    }
    public class ZChangeCheckpointType : CheckpointType
    {
        public static ZChangeCheckpointType Instance = new ZChangeCheckpointType();
        public string FormatName(PrintingCheckpoint chk)
        {
            string CheckpointDateFormat = RegMemory.GetString("checkpointDateFormat", "HH:mm:ss");
            string checkpointName = new DateTime(chk.timestamp).ToString(CheckpointDateFormat) + " z=" + chk.z;
            return checkpointName;
        }
    }
    public class MovementCountCheckpointType : CheckpointType
    {
        public static MovementCountCheckpointType Instance = new MovementCountCheckpointType();
        public string FormatName(PrintingCheckpoint chk)
        {
            string CheckpointDateFormat = RegMemory.GetString("checkpointDateFormat", "HH:mm:ss");
            string checkpointName = new DateTime(chk.timestamp).ToString(CheckpointDateFormat) + " line " + chk.lineNumber + " pos=(" + chk.x + ", " + chk.y + ", " + chk.z + ")";
            return checkpointName;
        }
    }
    

    public class PrintingCheckpoint : PrintingState
    {
        private CheckpointType type;

        // checkpoint state attributes are inherited from PrintingState

        public long timestamp;
        public int lineNumber;
        public BasicList<GCodeCompressed> jobGCodes;


        internal static PrintingCheckpoint GenerateCheckpoint(CheckpointType namingStrategy, PrintingCheckpoints checkpoints)
        {
            // Analyze status and save state.
            PrinterConnection conn = checkpoints.conn;
            PrintingCheckpoint s = new PrintingCheckpoint();
            s.type = namingStrategy;
            s.CaptureState(conn);
            s.lineNumber = conn.connector.Job.linesSend;
            s.jobGCodes = checkpoints.jobGCodes;
            s.timestamp = DateTime.Now.Ticks;
            return s;
        }

        public string Name
        {
            get { return this.type.FormatName(this); }
        }

        public CheckpointType Type
        {
            get { return type; }
            set { type = value; }
        }

        public void RestorePosition(GCodeExecutor executor)
        {
            executor.queueGCodeScript("G1 Z" + z.ToString(GCode.format) + "\r\nG1 X" + x.ToString(GCode.format) + " Y" + y.ToString(GCode.format) + " F" + Main.conn.travelFeedRate);
        }

        protected override StringBuilder GetRemainingCode(StringBuilder gcodeStringBuilder)
        {
            foreach (GCodeCompressed gc in jobGCodes.Skip(lineNumber))
            {
                gcodeStringBuilder.AppendLine(gc.getCode().orig);
            }
            return gcodeStringBuilder;
        }

        public IEnumerable<GCodeCompressed> GetCodeAlreadyExecuted()
        {
            return jobGCodes.Take(lineNumber);
        }

        public override string ToString()
        {
            return this.Name;
        }

    }


    public class IndexElement
    {
        public float z;
        public int listIndex;
    }
    public class IndexElementComparer : IComparer<IndexElement>
    {
        public int Compare(IndexElement x, IndexElement y)
        {
            return Math.Sign(x.z - y.z);
        }
    }
    /// <summary>
    /// This class works as an index ("db kind of indexes") in order to speed
    /// up the seek of positions with a specific z value.
    /// This index is prepared for cases when more than one object is printed,
    /// but each object is printed only once the previous was finished.
    /// In this case, there will be more than one position with the same z.
    /// In this case, the search is done in each object at a time. In order to
    /// get the position of the second object, you can provide an apropiate
    /// minIndex value to method FindElementAtZWithIndexGreaterThan.
    /// </summary>
    public class ZLayerCheckpointIndex
    {
        public LinkedList<List<IndexElement>> indexes = new LinkedList<List<IndexElement>>();
        /// <summary>
        /// Returns the index of the position where the element with specified
        /// z was found, or the index of the first value with greater z if not
        /// found.
        /// This method verifies the element has an index not lower than the
        /// one given as parameter (this allows making a search next if
        /// needed).
        /// </summary>
        /// <param name="z"></param>
        /// <param name="minIndex"></param>
        /// <returns></returns>
        public int? FindElementAtZWithIndexGreaterThan(float z, int minIndex)
        {
            IndexElement dummyElement = new IndexElement();
            dummyElement.z = z;
            IComparer<IndexElement> comparer = new IndexElementComparer();
            foreach (List<IndexElement> list in indexes)
            {
                if ((list.Count > 0) && (list.Last().listIndex >= minIndex))
                {
                    int index = list.BinarySearch(dummyElement, comparer);
                    if (index < 0)
                    {
                        // if it's negative, it means it didn't find the
                        // element, and it returns complement of the index with
                        // the next greater value.
                        index = ~index;
                        if (index >= list.Count)
                        {
                            // in this case, there is no element with greater
                            // value.
                            index = -1;
                        }
                    }
                    if (index >= 0)
                    {
                        // element found
                        IndexElement element = list[index];
                        if (element.listIndex >= minIndex)
                        {
                            return element.listIndex;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Adds an element in the index, associating the given z value with
        /// the given index value.
        /// </summary>
        /// <param name="z"></param>
        /// <param name="index"></param>
        public void AddIndexEntry(float z, int index)
        {
            IndexElement element = new IndexElement();
            element.z = z;
            element.listIndex = index;

            if (indexes.Count == 0)
            {
                indexes.AddFirst(new List<IndexElement>());
            }
            List<IndexElement> currentList = indexes.Last.Value;
            if (currentList.Count > 0 && currentList.Last().z > z)
            {
                // in this case we need to add a new list to the indexes list.
                currentList = new List<IndexElement>();
                indexes.AddLast(currentList);
            }
            currentList.Add(element);
        }
    }






    public delegate void OnCurrentCheckpointChanged();
    /// <summary>
    /// This class works as a bidirectional iterator to navigate the
    /// checkpoints.
    /// </summary>
    public class PrintingCheckpointsIterator
    {
        private PrintingCheckpoints chks;
        private int index;
        public OnCurrentCheckpointChanged onCurrentCheckpointChanged;
        public PrintingCheckpointsIterator(PrintingCheckpoints chks)
        {
            this.chks = chks;
            this.index = -1;
            if (onCurrentCheckpointChanged != null)
            {
                onCurrentCheckpointChanged();
            }
        }
        public PrintingCheckpoint GetCurrent()
        {
            BasicList<PrintingCheckpoint> list = chks.CheckPoints;
            if (index >= 0 && index < list.Count)
            {
                return list.ElementAt(index);
            }
            else
            {
                return null;
            }
        }
        public int GetCurrentPosition()
        {
            return index;
        }
        public bool GoToPositionWithZ(float z)
        {
            int minIndex = index;
            int? foundIndex = chks.checkpointIndex.FindElementAtZWithIndexGreaterThan(z, minIndex);
            if (foundIndex == null)
            {
                return false;
            }
            else
            {
                index = (int)foundIndex;
                if (onCurrentCheckpointChanged != null)
                {
                    onCurrentCheckpointChanged();
                }
                return true;
            }
        }
        internal bool GoToPositionWithNearestCoords(float x, float y, float z)
        {
            int minIndex = index;
            int? foundIndex = chks.checkpointIndex.FindElementAtZWithIndexGreaterThan(z, minIndex);
            if (foundIndex == null)
            {
                return false;
            }
            else
            {
                index = (int)foundIndex;
                PrintingCheckpoint chk = chks.checkpoints.ElementAt(index);
                float currentZ = chk.z;
                double sqDist = vectorSquareDistance(chk.x, chk.y, chk.z, x, y, z);
                double minDist = sqDist;
                int indexMin = index;

                while ((chk.z == currentZ) && (index + 1 < chks.checkpoints.Count) && (minDist > 0))
                {
                    index++;
                    chk = chks.checkpoints.ElementAt(index);
                    sqDist = vectorSquareDistance(chk.x, chk.y, chk.z, x, y, z);
                    if (sqDist < minDist)
                    {
                        minDist = sqDist;
                        indexMin = index;
                    }
                }

                index = indexMin;

                if (onCurrentCheckpointChanged != null)
                {
                    onCurrentCheckpointChanged();
                }
                return true;
            }
        }

        /// <summary>
        /// Calculates the square of the distance between two 3d points.
        /// (Square to avoid the square root, we only need to compare
        /// distances)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="z1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="z2"></param>
        /// <returns></returns>
        private double vectorSquareDistance(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2) + (z1 - z2) * (z1 - z2);
        }

        public void GoToLast()
        {
            index = chks.CheckPoints.Count - 1;
            if (onCurrentCheckpointChanged != null)
            {
                onCurrentCheckpointChanged();
            }
        }
        public void GoToFirst()
        {
            index = 0;
            if (onCurrentCheckpointChanged != null)
            {
                onCurrentCheckpointChanged();
            }
        }
        public bool HasNext()
        {
            return index < chks.CheckPoints.Count - 1;
        }
        public bool HasPrevious()
        {
            return index > 0;
        }
        public PrintingCheckpoint MoveToNext()
        {
            if (HasNext())
            {
                index++;
                if (onCurrentCheckpointChanged != null)
                {
                    onCurrentCheckpointChanged();
                }
            }
            return GetCurrent();
        }
        public PrintingCheckpoint MoveToPrevious()
        {
            if (HasPrevious())
            {
                index--;
                if (onCurrentCheckpointChanged != null)
                {
                    onCurrentCheckpointChanged();
                }
            }
            return GetCurrent();
        }
    }

}
