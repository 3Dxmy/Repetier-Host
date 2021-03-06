﻿/*
   Copyright 2011 repetier repetierdev@gmail.com

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.

*/

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
            // Load Generated GCode as current job.
            //Main.main.editor.Text = gcodeToExecute;
            Main.main.editor.setContent(0, gcodeToExecute);

            if (startImmediatelly)
            {
                // And then run it.
                conn.connector.RunJob();
            }
        }

    }

    public class StartNewJobGCodeExecutor : GCodeExecutor
    {
        private PrinterConnection conn;
        private bool startImmediatelly;
        public StartNewJobGCodeExecutor(PrinterConnection conn, bool startImmediatelly)
        {
            this.conn = conn;
            this.startImmediatelly = startImmediatelly;
        }

        public void queueGCodeScript(string gcodeToExecute)
        {
            conn.connector.KillJob();

            Main.main.editor.setContent(0, gcodeToExecute);

            if (startImmediatelly)
            {
                // And then run it.
                conn.connector.RunJob();
            }
        }

    }

    public class RunNowGCodeExecutor : GCodeExecutor
    {
        private PrinterConnection conn;
        public RunNowGCodeExecutor(PrinterConnection conn)
        {
            this.conn = conn;
        }

        public void queueGCodeScript(string gcodeToExecute)
        {
            conn.connector.GetInjectLock();
            string [] commands = gcodeToExecute.Split('\r','\n');
            foreach (string command in commands)
            {
                if (command.Length > 0)
                {
                    conn.injectManualCommand(command);
                }
            }
            conn.connector.ReturnInjectLock();
        }

    }

}
