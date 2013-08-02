using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepetierHost.model
{
    public interface GCodeExecutor
    {
        void queueGCodeScript(string gcodeToExecute);
    }
    public class PrinterConnectionGCodeExecutor : GCodeExecutor
    {
        private PrinterConnection conn;
        private bool startImmediatelly;
        public PrinterConnectionGCodeExecutor(PrinterConnection conn, bool startImmediatelly)
        {
            this.conn = conn;
            this.startImmediatelly = startImmediatelly;
        }

        public void queueGCodeScript(string gcodeToExecute)
        {
            /*conn.connector.Job.BeginJob();
            conn.connector.GetInjectLock();
            foreach (string line in gcodeToExecute.Split(new char[] { '\n', '\r' }))
            {
                if (!String.IsNullOrEmpty(line))
                {
                    conn.connector.InjectManualCommand(line);
                }
            }
            conn.connector.ReturnInjectLock();
            conn.connector.Job.EndJob();*/


            // Load Generated GCode as current job.
            Main.main.editor.Text = gcodeToExecute;

            if (startImmediatelly)
            {
                // And then run it.
                conn.connector.RunJob();
            }
        }

    }
}
