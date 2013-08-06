using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace RepetierHost.model
{
    public class PrintingStateSnapshot
    {
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
        public string remainingCode;

        internal static PrintingStateSnapshot GeneratePrintingStateSnapshot(PrinterConnection conn)
        {
            // Analyze status and save state.
            GCodeAnalyzer analyzer = conn.analyzer;
            PrintingStateSnapshot s = new PrintingStateSnapshot();

            s.x = analyzer.RealX;
            s.y = analyzer.RealY;
            s.z = analyzer.RealZ;
            s.fanVoltage = analyzer.fanVoltage;
            s.fanOn = analyzer.fanOn;
            s.speed = analyzer.f;
            s.layer = analyzer.layer;
            s.extrudersTemp = new float[conn.extruderTemp.Count];
            for (int extr = 0; extr < conn.extruderTemp.Count; extr++ )
            {
                s.extrudersTemp[extr] = conn.extruderTemp[extr];
            }
            s.bedTemp = conn.bedTemp;
            s.activeExtruderId = analyzer.activeExtruderId;
            s.relative = analyzer.relative;
            s.activeExtruderValue = analyzer.activeExtruder.e - analyzer.activeExtruder.eOffset;
            s.remainingCode = GetRemainingGcode(conn); 
            
            return s;
        }

        internal static string GetRemainingGcode(PrinterConnection conn)
        {
            StringBuilder remainingCodeSb = new StringBuilder();
            foreach (GCodeCompressed gc in conn.connector.Job.GetPendingJobCommands())
            {
                remainingCodeSb.AppendLine(gc.getCode().orig);
            }
            return remainingCodeSb.ToString();
        }

        public void RestoreState(GCodeExecutor executor)
        {
            // Generate code to restore state.
            string gCodeToRestoreState = GenerateGCodeToRestoreState();

            Console.Out.WriteLine("Loaded State:");
            Console.Out.WriteLine(this.ToString());
            Console.Out.WriteLine("About to print to restore state:");
            Console.Out.WriteLine(gCodeToRestoreState);

            Console.Out.WriteLine("Restoring state");
            executor.queueGCodeScript(gCodeToRestoreState + "\r\n" + remainingCode);
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
            // Based on ContinueJob from PauseInfo.
            // Start code, generic
            // XXX FIXME uso las de Repetier???????)
            g.SetPositionMode(true);
            g.HomeAllAxis();
            g.MoveZ(10.0, 0);    //Move up so as to let the plastic flow.
            // XXXX FIXME GET FAN SPEED FROM DEVICE
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
            g.Add("M140 S" + bedTemp.ToString(CultureInfo.InvariantCulture));
            g.NewLine();
            for (int i = 0; i < extrudersTemp.Length; i++)
            {
                // Set extruders temperature. Force use of "." as decimal separator.
                g.Add("M104 S" + extrudersTemp[i].ToString(CultureInfo.InvariantCulture) + " T" + i);
                g.NewLine();
            }

            // Then wait until all temperatures have been reached.
            // Marlin doesn't have M116, so, we must use M190 and M109 to wait
            // to reach the temperature.
            // Set bed temperature. Force use of "." as decimal separator.
            g.Add("M190 S" + bedTemp.ToString(CultureInfo.InvariantCulture));
            g.NewLine();
            for (int i = 0; i < extrudersTemp.Length; i++)
            {
                // Set extruders temperature. Force use of "." as decimal separator.
                g.Add("M109 S" + extrudersTemp[i].ToString(CultureInfo.InvariantCulture) + " T" + i);
                g.NewLine();
            }
            // Select extruder
            g.Add("T" + activeExtruderId);
            g.NewLine();

            g.SetE(activeExtruderValue);

            g.Add("@pause Please extrude some plastic to have a better flow and resume after removing the exceeding plastic"); // Let the user extrude some plastic.
            g.NewLine();

            g.Add("G28 X0 Y0"); // Go to home x y in case you moved the bed accidentally.
            g.NewLine();

            // We first must move vertically, so that it doesn't collide with the object.
            g.MoveZ(z + 5, layer);
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
            StringBuilder sb = new StringBuilder("Layer: " + layer + "\nBed Temp: " + bedTemp + "\nX: " + x + "\nY: " + y + "\nZ: " + z + "\nActive Extruder: " + activeExtruderId);
            for (int i = 0; i < extrudersTemp.Count(); i++)
            {
                sb.Append("\nTemp Extruder #" + i + ": " + extrudersTemp[i]);
            }
            return sb.ToString();
        }
    }

    public class SnapshotFactory
    {
        public static PrintingStateSnapshot TakeSnapshot(PrinterConnection conn)
        {
            return PrintingStateSnapshot.GeneratePrintingStateSnapshot(conn);
        }
    }
    
}
