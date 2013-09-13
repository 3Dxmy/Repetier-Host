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

        public int? FindElementAtZWithIndexGreaterThan(float z, int minIndex)
        {
            return FindElementAtZWithIndexGreaterOrLowerThan(z, minIndex, 0, false);
        }

        public int? FindElementAtZWithIndexLowerThan(float z, int maxIndex)
        {
            return FindElementAtZWithIndexGreaterOrLowerThan(z, maxIndex, 0, true);
        }

        public int? FindElementAboveZWithIndexGreaterThan(float z, int minIndex)
        {
            return FindElementAtZWithIndexGreaterOrLowerThan(z, minIndex, 1, false);
        }

        public int? FindElementBelowZWithIndexLowerThan(float z, int maxIndex)
        {
            return FindElementAtZWithIndexGreaterOrLowerThan(z, maxIndex, -1, true);
        }

        /// <summary>
        /// Returns the index of the position where the element with specified
        /// z was found, or the index of the first value with greater z if not
        /// found.
        /// This method verifies the element has an index not lower or greater
        /// than the one given as boundaryIndex parameter (this allows making a
        /// search next if needed). In order to decide whether it must be
        /// greater or lower, it uses the parameter reverse.
        /// If delta is different from 0, instead of returning the elment with
        /// the specified z, it returns "index + delta" if that index exists in
        /// "the same heap".
        /// </summary>
        /// <param name="z"></param>
        /// <param name="boundaryIndex"></param>
        /// <param name="delta"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public int? FindElementAtZWithIndexGreaterOrLowerThan(float z, int boundaryIndex, int delta, bool reverse)
        {
            float maxLayerDistance = (float)RegMemory.GetDouble("indexingMaxLayerHeight", 1f);

            IndexElement dummyElement = new IndexElement();
            dummyElement.z = z;
            IComparer<IndexElement> comparer = new IndexElementComparer();
            foreach (List<IndexElement> list in reverse ? indexes.Reverse() : indexes)
            {
                if (list.Count > 0)
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
                        // element found. We must verify it's not much greater
                        // than searched z.
                        if (Math.Abs(((IndexElement)list[index]).z - z) <= maxLayerDistance)
                        {
                            // shift the index based on the delta
                            int shiftedIndex = index + delta;
                            if (!(shiftedIndex >= list.Count || shiftedIndex < 0) && (Math.Abs(((IndexElement)list[shiftedIndex]).z - z) <= maxLayerDistance))
                            {
                                IndexElement element = list[shiftedIndex];
                                if ((!reverse && element.listIndex >= boundaryIndex) || (reverse && element.listIndex <= boundaryIndex))
                                {
                                    return element.listIndex;
                                }
                            }
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
    /// It provides some additional methods, like moving between layers and
    /// searching coordinates.
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
        /// <summary>
        /// Returns the current element or null if none.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Returns the current index position.
        /// </summary>
        /// <returns></returns>
        public int GetCurrentPosition()
        {
            return index;
        }
        /// <summary>
        /// Searchs for a layer with the same z value, or if none, it searchs the nearest with a
        /// higher value.
        /// If one was found, it returns true. Otherwise, false.
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Searchs for the checkpoint with the nearest distance in the current
        /// layer (in order to decide in which layer it's working it uses the
        /// same criteria GoToPositionWithZ uses).
        /// If more than one has the same distance, it picks the first one.
        /// If any was found, it returns true. Otherwise, false.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        internal bool GoToPositionWithNearestCoords(float x, float y, float z)
        {
            // FIXME If you have more than one "column", it should check in all columns available for the nearest coords
            int minIndex = index;
            int? foundIndex;
            bool foundOne = false;
            int globalIndexMin = index;
            double minDist = double.MaxValue;

            // There may be many stacks. We must verify in all of them
            while (((foundIndex = chks.checkpointIndex.FindElementAtZWithIndexGreaterThan(z, minIndex)) != null) && (minDist > 0))
            {
                foundOne = true;

                int workingIndex = (int)foundIndex;
                PrintingCheckpoint chk = chks.checkpoints.ElementAt(workingIndex);
                float currentZ = chk.z;
                double sqDist = vectorSquareDistance(chk.x, chk.y, chk.z, x, y, z);
                if (sqDist < minDist)
                {
                    minDist = sqDist;
                    globalIndexMin = workingIndex;
                }

                while ((chk.z == currentZ) && (index + 1 < chks.checkpoints.Count) && (minDist > 0))
                {
                    index++;
                    chk = chks.checkpoints.ElementAt(index);
                    sqDist = vectorSquareDistance(chk.x, chk.y, chk.z, x, y, z);
                    if (sqDist < minDist)
                    {
                        minDist = sqDist;
                        globalIndexMin = workingIndex;
                    }
                }
                // we set the new index after the end of current index, which
                // is the end of current layer.
                minIndex = workingIndex + 1;
            }

            if (foundOne)
            {
                index = globalIndexMin;

                if (onCurrentCheckpointChanged != null)
                {
                    onCurrentCheckpointChanged();
                }
            }
            return foundOne;
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

        /// <summary>
        /// Goes to the last checkpoint.
        /// </summary>
        public void GoToLast()
        {
            index = chks.CheckPoints.Count - 1;
            if (onCurrentCheckpointChanged != null)
            {
                onCurrentCheckpointChanged();
            }
        }
        /// <summary>
        /// Goes to the first checkpoint.
        /// </summary>
        public void GoToFirst()
        {
            index = 0;
            if (onCurrentCheckpointChanged != null)
            {
                onCurrentCheckpointChanged();
            }
        }
        /// <summary>
        /// Returns true if there's a next checkpoint.
        /// </summary>
        /// <returns></returns>
        public bool HasNext()
        {
            return index < chks.CheckPoints.Count - 1;
        }
        /// <summary>
        /// Returns true if there's a previous checkpoint.
        /// </summary>
        /// <returns></returns>
        public bool HasPrevious()
        {
            return index > 0;
        }
        /// <summary>
        /// Moves to the next position, and returns it.
        /// If there are no more, it stays in the same position, and returns
        /// the same element.
        /// If there are no elements, it returns null.
        /// </summary>
        /// <returns></returns>
        public PrintingCheckpoint MoveToNext(bool lockZ)
        {
            if (HasNext())
            {
                float lockedZ = GetCurrent().z; //only used if lockz=true

                // increment index
                index++;

                if (lockZ && GetCurrent().z != lockedZ)
                {
                    // if z is locked, we must ensure z doesn't change
                    int? candidateIndex = chks.checkpointIndex.FindElementAtZWithIndexGreaterThan(lockedZ, index);
                    if (candidateIndex != null)
                    {
                        index = (int)candidateIndex;
                    }
                    else
                    {
                        // Restore last position with z=currentZ.
                        index--;
                    }
                }

                if (onCurrentCheckpointChanged != null)
                {
                    onCurrentCheckpointChanged();
                }
            }
            return GetCurrent();
        }
        /// <summary>
        /// Moves to the previous position, and returns it.
        /// If there are no more, it stays in the same position, and returns
        /// the same element.
        /// If there are no elements, it returns null.
        /// </summary>
        /// <returns></returns>
        public PrintingCheckpoint MoveToPrevious(bool lockZ)
        {
            if (HasPrevious())
            {
                float lockedZ = GetCurrent().z; //only used if lockz=true
                
                index--;

                if (lockZ && GetCurrent().z != lockedZ)
                {
                    // if z is locked, we must ensure z doesn't change
                    int? candidateIndex = chks.checkpointIndex.FindElementAtZWithIndexLowerThan(lockedZ, index);
                    if (candidateIndex != null)
                    {
                        // Go to the end of the layer
                        int candidateIndexInt = (int)candidateIndex;
                        while (chks.checkpoints.Count > candidateIndexInt && chks.checkpoints.ElementAt(candidateIndexInt).z == lockedZ)
                        {
                            candidateIndexInt++;
                        }

                        index = candidateIndexInt - 1;
                    }
                    else
                    {
                        // Restore last position with z=currentZ.
                        index++;
                    }
                }

                if (onCurrentCheckpointChanged != null)
                {
                    onCurrentCheckpointChanged();
                }
            }
            return GetCurrent();
        }
        /// <summary>
        /// Moves to the first checkpoint in the next layer, and returns it.
        /// If there are no more, it stays in the same position, and returns
        /// the same element.
        /// If there are no elements, it returns null.
        /// </summary>
        /// <returns></returns>
        public PrintingCheckpoint MoveToNextLayer()
        {
            if (index < chks.checkpoints.Count && chks.checkpoints.Count > 0)
            {
                int minIndex = index;
                if (minIndex < 0)
                {
                    minIndex = 0;
                }
                PrintingCheckpoint chk = chks.checkpoints.ElementAt(index);
                float currentZ = chk.z;

                int? newIndex = chks.checkpointIndex.FindElementAboveZWithIndexGreaterThan(currentZ, minIndex);
                if (newIndex != null)
                {
                    index = (int)newIndex;
                }

                if (onCurrentCheckpointChanged != null)
                {
                    onCurrentCheckpointChanged();
                }
            }
            return GetCurrent();
        }
        /// <summary>
        /// Moves to the first checkpoint in the previous layer, and returns it.
        /// If there are no more, it stays in the same position, and returns
        /// the same element.
        /// If there are no elements, it returns null.
        /// </summary>
        /// <returns></returns>
        public PrintingCheckpoint MoveToPreviousLayer()
        {
            if (index > 0 && chks.checkpoints.Count > 0)
            {
                int minIndex = index;
                PrintingCheckpoint chk = chks.checkpoints.ElementAt(minIndex);
                float currentZ = chk.z;
                int? newIndex = chks.checkpointIndex.FindElementBelowZWithIndexLowerThan(currentZ, minIndex);
                if (newIndex != null)
                {
                    index = (int)newIndex;
                }

                if (onCurrentCheckpointChanged != null)
                {
                    onCurrentCheckpointChanged();
                }
            }
            return GetCurrent();
        }
    }

}
