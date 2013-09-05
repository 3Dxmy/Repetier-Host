using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace RepetierHost.model
{
    public class PrintingCheckpoints
    {
        internal LinkedList<GCodeCompressed> jobGCodes;
        internal LinkedList<PrintingCheckpoint> checkpoints;
        internal PrinterConnection conn;
        internal bool jobActive;

        public PrintingCheckpoints(PrinterConnection conn)
        {
            checkpoints = new LinkedList<PrintingCheckpoint>();
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
}
