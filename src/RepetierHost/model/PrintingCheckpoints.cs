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

    public class PrintingCheckpoint
    {
        private string name;

        public float x, y, z;
        public float speed; //speed
        public float fanVoltage;    //fan speed
        public bool fanOn;
        public bool relative;
        public float[] extrudersTemp;
        public float bedTemp;
        public int layer;
        public int activeExtruderId;
        public float activeExtruderValue;

        public int lineNumber;
        public IEnumerable<GCodeCompressed> jobGCodes;
        public PrintingCheckpoints checkpoints;


        internal static PrintingCheckpoint GenerateCheckpoint(string name, PrintingCheckpoints checkpoints)
        {
            // Analyze status and save state.
            PrinterConnection conn = checkpoints.conn;
            GCodeAnalyzer analyzer = conn.analyzer;
            PrintingCheckpoint s = new PrintingCheckpoint();

            s.Name = name;

            s.x = analyzer.RealX;
            s.y = analyzer.RealY;
            s.z = analyzer.RealZ;
            s.fanVoltage = analyzer.fanVoltage;
            s.fanOn = analyzer.fanOn;
            s.speed = analyzer.f;
            s.layer = analyzer.layer;
            s.extrudersTemp = new float[conn.extruderTemp.Count];
            for (int extr = 0; extr < conn.extruderTemp.Count; extr++)
            {
                // Use the configured temperature, not the measured
                // temperature.
                s.extrudersTemp[extr] = conn.analyzer.getTemperature(extr);
                //s.extrudersTemp[extr] = conn.extruderTemp[extr];
            }
            // Use the configured temperature, not the measured temperature.
            s.bedTemp = conn.analyzer.bedTemp;
            //s.bedTemp = conn.bedTemp;
            s.activeExtruderId = analyzer.activeExtruderId;
            s.relative = analyzer.relative;
            s.activeExtruderValue = analyzer.activeExtruder.e - analyzer.activeExtruder.eOffset;


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

        public void RestoreState(GCodeExecutor executor)
        {
            // Generate code to restore state.
            string gCodeToRestoreState = GenerateGCodeToRestoreState();
            Console.Out.WriteLine("About to print to restore state:");
            Console.Out.WriteLine(gCodeToRestoreState);

            executor.queueGCodeScript(gCodeToRestoreState + "\r\n" + GetRemainingGcs());
        }

        public void Delete()
        {
            checkpoints.RemoveCheckpoint(this);
        }

        public void Rename(string newCheckpointName)
        {
            this.Name = newCheckpointName;
        }

        private string GetRemainingGcs()
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

        /// <summary>
        /// Generates a GCode script that sets the current state.
        /// </summary>
        /// <returns></returns>
        private string GenerateGCodeToRestoreState()
        {
            GCodeGenerator g = Main.generator;
            g.Reset();
            g.Load();
            g.Add("@continuedScript");
            g.NewLine();
            g.SetPositionMode(true);
            g.HomeAllAxis();
            g.MoveZ(10.0, 0);    //Move up so as to let the plastic flow.
            if (fanOn)
            {
                // Set fan speed
                g.Add("M106 S" + (int)fanVoltage); //Fan
                g.NewLine();
            }
            g.ResetE();

            // First set temperature without blocking.
            // Marlin doesn't have M116, so, we must use M190 and M109 to wait
            // to reach the temperature.
            // Set bed temperature. Force use of "." as decimal separator.
            if (bedTemp != 0.0)
            {
                g.Add("M140 S" + bedTemp.ToString(CultureInfo.InvariantCulture));
                g.NewLine();
            }
            for (int i = 0; i < extrudersTemp.Length; i++)
            {
                if (extrudersTemp[i] != 0.0)
                {
                    // Set extruders temperature. Force use of "." as decimal separator.
                    g.Add("M104 S" + extrudersTemp[i].ToString(CultureInfo.InvariantCulture) + " T" + i);
                    g.NewLine();
                }
            }

            // Then wait until all temperatures have been reached.
            // Marlin doesn't have M116, so, we must use M190 and M109 to wait
            // to reach the temperature.
            // Set bed temperature. Force use of "." as decimal separator.
            if (bedTemp != 0.0)
            {
                g.Add("M190 S" + bedTemp.ToString(CultureInfo.InvariantCulture));
                g.NewLine();
            }
            for (int i = 0; i < extrudersTemp.Length; i++)
            {
                if (extrudersTemp[i] != 0.0)
                {
                    // Set extruders temperature. Force use of "." as decimal separator.
                    g.Add("M109 S" + extrudersTemp[i].ToString(CultureInfo.InvariantCulture) + " T" + i);
                    g.NewLine();
                }
            }
            // Select extruder
            g.Add("T" + activeExtruderId);
            g.NewLine();

            g.SetE(activeExtruderValue);

            g.Add("@pause " + Trans.T("L_EXTRUDE_PLASTIC_PAUSE")); // Let the user extrude some plastic.
            g.NewLine();

            g.Add("G28 X0 Y0"); // Go to home x y in case you moved the bed accidentally.
            g.NewLine();

            // We first must move vertically, so that it doesn't collide with the object.
            if (z < Main.printerSettings.PrintAreaHeight && z + PrintingStateSnapshot.RestoreStateZMargin > Main.printerSettings.PrintAreaHeight)
            {
                // The object is too tall, we must restrict the z to the
                // printer height.
                g.MoveZ(Main.printerSettings.PrintAreaHeight, layer);
            }
            else
            {
                g.MoveZ(z + PrintingStateSnapshot.RestoreStateZMargin, layer);
            }
            g.Move(x, y, g.TravelFeedRate);
            g.MoveZ(z, layer);
            g.Add("G1 F" + speed.ToString(GCode.format)); // Reset old speed
            g.NewLine();

            if (relative)
            {
                // We know the coordinates will be absolute because we set it at first.
                g.SetPositionMode(false);
            }
            return g.Code;
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
