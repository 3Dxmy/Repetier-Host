using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace RepetierHost.model
{
    /// <summary>
    /// This class works as an entry point to access checkpoints.
    /// </summary>
    public class PrintingCheckpoints
    {
        internal LinkedList<GCodeCompressed> jobGCodes;
        internal LinkedList<PrintingCheckpoint> checkpoints;
        internal ZLayerCheckpointIndex checkpointIndex;
        internal PrinterConnection conn;
        internal bool jobActive;

        public PrintingCheckpoints(PrinterConnection conn)
        {
            checkpoints = new LinkedList<PrintingCheckpoint>();
            checkpointIndex = new ZLayerCheckpointIndex();
            this.conn = conn;
            jobActive = false;
        }

        public void BeginJob()
        {
            // XXX TODO: ACA DEBERIA GUARDAR EL ARCHIVO DE GCODE E INICIALIZAR EL ARCHIVO DE ESTADOS.
            jobGCodes = conn.connector.Job.GetPendingJobCommands();
            checkpoints = new LinkedList<PrintingCheckpoint>();
            jobActive = true;
        }
        public void EndJob()
        {
            // XXX TODO: ESTE METODO CREO QUE NUNCA ES LLAMADO, PODES QUITARLO O DARLE UNA FUNCION
            checkpoints = new LinkedList<PrintingCheckpoint>();
            jobGCodes = new LinkedList<GCodeCompressed>();
            jobActive = false;
        }

        public void CreateCheckpoint(string name)
        {
            if (jobActive)
            {
                PrintingCheckpoint chk = PrintingCheckpoint.GenerateCheckpoint(name, this);
                if (checkpoints.Count == 0 || checkpoints.Last.Value.z != chk.z)
                {
                    // Layer changed, add an index entry.
                    checkpointIndex.AddIndexEntry(chk.z, checkpoints.Count);
                }
                checkpoints.AddLast(chk);
            }
        }

        public LinkedList<PrintingCheckpoint> GetCheckPoints()
        {
            // XXX TODO ESTE METODO DEBERIA CAMBIAR, YA QUE NO VAN A ESTAR EN MEMORIA
            return checkpoints;
        }

        public void RemoveCheckpoint(PrintingCheckpoint chk)
        {
            checkpoints.Remove(chk);
        }
    }

    public class PrintingCheckpoint : PrintingState
    {
        private string name;

        // checkpoint state attributes are inherited from PrintingState

        public int lineNumber;
        public IEnumerable<GCodeCompressed> jobGCodes;
        public PrintingCheckpoints checkpoints;


        internal static PrintingCheckpoint GenerateCheckpoint(string name, PrintingCheckpoints checkpoints)
        {
            // Analyze status and save state.
            PrinterConnection conn = checkpoints.conn;
            PrintingCheckpoint s = new PrintingCheckpoint();
            s.Name = name;
            s.CaptureState(conn);
            s.lineNumber = conn.connector.Job.linesSend;
            s.jobGCodes = checkpoints.jobGCodes;
            return s;
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public void RestorePosition(GCodeExecutor executor)
        {
            executor.queueGCodeScript("G1 Z" + z.ToString(GCode.format) + "\r\nG1 X" + x.ToString(GCode.format) + " Y" + y.ToString(GCode.format) + " F" + Main.conn.travelFeedRate);
        }

        public void Delete()
        {
            checkpoints.RemoveCheckpoint(this);
        }

        public void Rename(string newCheckpointName)
        {
            this.Name = newCheckpointName;
        }

        protected override string GetRemainingCode()
        {
            StringBuilder remainingGcs = new StringBuilder();
            foreach (GCodeCompressed gc in jobGCodes.Skip(lineNumber))
            {
                remainingGcs.AppendLine(gc.getCode().orig);
            }
            return remainingGcs.ToString();
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
        private LinkedList<PrintingCheckpoint> list;
        private PrintingCheckpoints chks;
        private int index;
        public OnCurrentCheckpointChanged onCurrentCheckpointChanged;
        public PrintingCheckpointsIterator(PrintingCheckpoints chks)
        {
            LinkedList<PrintingCheckpoint> list = chks.GetCheckPoints();
            this.chks = chks;

            this.list = list;
            this.index = -1;
            if (onCurrentCheckpointChanged != null)
            {
                onCurrentCheckpointChanged();
            }
        }
        public PrintingCheckpoint GetCurrent()
        {
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
        public void GoToLast()
        {
            index = list.Count - 1;
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
            return index < list.Count - 1;
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
