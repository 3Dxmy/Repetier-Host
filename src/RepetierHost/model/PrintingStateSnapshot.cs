/*
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
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace RepetierHost.model
{
    public class PrintingStateSnapshot : PrintingState
    {
        public string remainingCode;
        // other snapshot attributes are inherited from PrintingState

        internal static PrintingStateSnapshot GeneratePrintingStateSnapshot(PrinterConnection conn)
        {
            // Analyze status and save state.
            PrintingStateSnapshot s = new PrintingStateSnapshot();
            s.CaptureState(conn);
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

        protected override StringBuilder GetRemainingCode(StringBuilder gcodeStringBuilder)
        {
            gcodeStringBuilder.Append(remainingCode);
            return gcodeStringBuilder;
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
