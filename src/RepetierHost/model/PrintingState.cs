using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using RepetierHost.view.utils;

namespace RepetierHost.model
{
    /// <summary>
    /// This class represents an instantaneous state while printing.
    /// </summary>
    public abstract class PrintingState
    {
        public static double RestoreStateZMargin = RegMemory.GetDouble("restoreStateZMargin", 5.0);   //5 mm by default

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

        protected virtual void CaptureState(PrinterConnection conn)
        {
            GCodeAnalyzer analyzer = conn.analyzer;

            this.x = analyzer.RealX;
            this.y = analyzer.RealY;
            this.z = analyzer.RealZ;
            this.fanVoltage = analyzer.fanVoltage;
            this.fanOn = analyzer.fanOn;
            this.speed = analyzer.f;
            this.layer = analyzer.layer;
            this.extrudersTemp = new float[conn.extruderTemp.Count];
            for (int extr = 0; extr < conn.extruderTemp.Count; extr++)
            {
                // Use the configured temperature, not the measured
                // temperature.
                this.extrudersTemp[extr] = conn.analyzer.getTemperature(extr);
                //this.extrudersTemp[extr] = conn.extruderTemp[extr];
            }
            // Use the configured temperature, not the measured temperature.
            this.bedTemp = conn.analyzer.bedTemp;
            //this.bedTemp = conn.bedTemp;
            this.activeExtruderId = analyzer.activeExtruderId;
            this.relative = analyzer.relative;
            this.activeExtruderValue = analyzer.activeExtruder.e - analyzer.activeExtruder.eOffset;

        }


        /// <summary>
        /// Generates a GCode script that sets the current state.
        /// </summary>
        /// <returns></returns>
        protected virtual string GenerateGCodeToRestoreState()
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
            if (z < Main.printerSettings.PrintAreaHeight && z + RestoreStateZMargin > Main.printerSettings.PrintAreaHeight)
            {
                // The object is too tall, we must restrict the z to the
                // printer height.
                g.MoveZ(Main.printerSettings.PrintAreaHeight, layer);
            }
            else
            {
                g.MoveZ(z + RestoreStateZMargin, layer);
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


        public virtual void RestoreState(GCodeExecutor executor)
        {
            // Generate code to restore state.
            string gCodeToRestoreState = GenerateGCodeToRestoreState();
            executor.queueGCodeScript(gCodeToRestoreState + "\r\n" + GetRemainingCode());
        }

        protected abstract string GetRemainingCode();



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
}
