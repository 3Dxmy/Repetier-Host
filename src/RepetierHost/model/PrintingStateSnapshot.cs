using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;

namespace RepetierHost.model
{
    public class PrintingStateSnapshot
    {
        public float x, y, z;
        public float speed; //speed
        public Boolean relative;
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
            s.speed = analyzer.f;
            s.layer = analyzer.layer;
            s.extrudersTemp = new float[analyzer.extruder.Count];
            foreach (int extr in analyzer.extruder.Keys) {
                s.extrudersTemp[extr] = analyzer.getTemperature(extr);
            }
            s.bedTemp = analyzer.bedTemp;
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
            g.SetPositionMode(true);
            g.HomeAllAxis();
            g.ResetE();
            // We first must move vertically, so that it doesn't collide with the object.
            g.MoveZ(z + 5, layer);
            g.Move(x, y, 0.0);
            g.MoveZ(z, layer);
            g.Move(x, y, speed); // Reset old speed

            // XXX FIXME: Debo setear la temperatura? O eso debe hacerlo el usuario?
            //g.SetTemperatureFast((int)extrudersTemp[activeExtruderId]);
            // XXX FIXME: Como seteo el bed temperature????
            // XXX FIXME: Como seteo el active extruder????
            
            g.SetE(activeExtruderValue);
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
