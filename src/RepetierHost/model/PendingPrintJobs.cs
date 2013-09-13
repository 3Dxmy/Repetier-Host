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

   written by eze-eoc at kikai labs (eai@eoconsulting.com.ar)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Xml.Linq;

namespace RepetierHost.model
{
    public class PendingPrintJobsException : Exception
    {
        public PendingPrintJobsException(string message, Exception e) : base(message, e) {}
    }

    /// <summary>
    /// This class provides methods to get the list of pending jobs.
    /// </summary>
    public class PendingPrintJobs
    {
        public const string RepetierExtension = "repstate";
        public const string PendingJobsDirName = "pending";

        public static List<PendingPrintJob> GetPendingJobs()
        {
            List<PendingPrintJob> list = new List<PendingPrintJob>();
            foreach (string file in GetPendingJobsFiles())
            {
                PendingPrintJob job = new PendingPrintJob(file);
                list.Add(job);
            }
            return list;
        }

        public static void Add(PrintingStateSnapshot snapshot, string name)
        {
            PendingPrintJob job = new PendingPrintJob(snapshot, PendingJobsDir + Path.DirectorySeparatorChar + name + "." + RepetierExtension);
            job.Save();
        }

        /// <summary>
        /// Returns the list of files that are inside the pending jobs directory.
        /// </summary>
        /// <returns></returns>
        private static string[] GetPendingJobsFiles()
        {
            try
            {
                if (!Directory.Exists(PendingJobsDir))
                {
                    Directory.CreateDirectory(PendingJobsDir);
                }
                string[] files = Directory.GetFiles(PendingJobsDir, "*." + RepetierExtension);

                return files;
            }
            catch (Exception e)
            {
                throw new PendingPrintJobsException("Can't read or create pending jobs directory.", e);
            }
        }

        public static PendingPrintJob GetPendingJobWithName(string snapshotName)
        {
            try
            {
                if (!Directory.Exists(PendingJobsDir))
                {
                    Directory.CreateDirectory(PendingJobsDir);
                }

                string pendingJobFilePath = PendingJobsDir + Path.DirectorySeparatorChar + snapshotName + "." + RepetierExtension;
                if (File.Exists(pendingJobFilePath))
                {
                    return new PendingPrintJob(pendingJobFilePath);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new PendingPrintJobsException("Can't read or create pending jobs directory.", e);
            }
        }

        public static string PendingJobsDir
        {
            get { return Main.globalSettings.Workdir + Path.DirectorySeparatorChar + PendingJobsDirName; }
        }
    }

    /// <summary>
    /// This classs represents a pending job.
    /// </summary>
    public class PendingPrintJob
    {
        private string path;
        private PrintingStateSnapshot snapshot;

        public PendingPrintJob(string path)
        {
            this.path = path;
        }

        public PendingPrintJob(PrintingStateSnapshot snapshot, string path)
        {
            this.path = path;
            this.snapshot = snapshot;
        }

        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(path); }
        }

        public PrintingStateSnapshot GetSnapshot()
        {
            if (snapshot == null)
            {
                // Lazy loading
                snapshot = PrintingStateSnapshotSerialization.LoadSnapshotFile(path);
            }
            return snapshot;
        }

        public void Save()
        {
            PrintingStateSnapshotSerialization.SaveSnapshotFile(snapshot, path);
        }

        public void Delete()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void Rename(string newName)
        {
            if (IsInvalidSnapshotName(newName))
            {
                throw new IOException("Invalid job name");
            }
            string oldPath = this.path;
            string newPath = Path.GetDirectoryName(oldPath) + Path.DirectorySeparatorChar + newName + "." + PendingPrintJobs.RepetierExtension;
            File.Move(oldPath, newPath);
            this.path = newPath;
        }

        /// <summary>
        /// Returns true if and only if the snapshot name is valid.
        /// This method doesn't accept null values.
        /// </summary>
        /// <param name="snapshotName"></param>
        /// <returns></returns>
        public static bool IsInvalidSnapshotName(string snapshotName)
        {
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                if (snapshotName.Contains(invalidChar))
                {
                    return false;
                }
            }
            return snapshotName.StartsWith(" ") || snapshotName.EndsWith(" ") || snapshotName.Length == 0 || snapshotName.Length >= 128;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// This class provides methods to serialize a pending job.
    /// </summary>
    public class PrintingStateSnapshotSerialization
    {
        public const string SnapshotTypeStateAndRemainingGCode = "state-and-remaining-gcode";
        public const string ContainerVersion = "1.0";


        public static PrintingStateSnapshot LoadSnapshotFile(string path)
        {
            Stream stream = File.OpenRead(path);
            try
            {
                return LoadSnapshotFile(stream);
            }
            finally
            {
                stream.Close();
            }
        }
        public static PrintingStateSnapshot LoadSnapshotFile(Stream stream)
        {
            XElement containerNode = XElement.Load(new StreamReader(stream));
            SnapshotContainer container = new SnapshotContainer();

            foreach (XElement elem in containerNode.Elements())
            {
                if ("version".Equals(elem.Name.LocalName))
                {
                    container.version = elem.Value;
                }
                else if ("type".Equals(elem.Name.LocalName))
                {
                    container.type = elem.Value;
                }
                else if ("snapshot".Equals(elem.Name.LocalName))
                {
                    container.snapshot = new PrintingStateSnapshot();
                    foreach (XElement elemCh in elem.Elements())
                    {
                        if ("x".Equals(elemCh.Name.LocalName))
                        {
                            container.snapshot.x = float.Parse(elemCh.Value, CultureInfo.InvariantCulture);
                        }
                        else if ("y".Equals(elemCh.Name.LocalName))
                        {
                            container.snapshot.y = float.Parse(elemCh.Value, CultureInfo.InvariantCulture);
                        }
                        else if ("z".Equals(elemCh.Name.LocalName))
                        {
                            container.snapshot.z = float.Parse(elemCh.Value, CultureInfo.InvariantCulture);
                        }
                        else if ("speed".Equals(elemCh.Name.LocalName))
                        {
                            container.snapshot.speed = float.Parse(elemCh.Value, CultureInfo.InvariantCulture);
                        }
                        else if ("fanVoltage".Equals(elemCh.Name.LocalName))
                        {
                            container.snapshot.fanVoltage = float.Parse(elemCh.Value, CultureInfo.InvariantCulture);
                        }
                        else if ("fanOn".Equals(elemCh.Name.LocalName))
                        {
                            container.snapshot.fanOn = bool.Parse(elemCh.Value);
                        }
                        else if ("relative".Equals(elemCh.Name.LocalName))
                        {
                            container.snapshot.relative = bool.Parse(elemCh.Value);
                        }
                        else if ("extrudersTemp".Equals(elemCh.Name.LocalName))
                        {
                            LinkedList<float> extrudersTempList = new LinkedList<float>();
                            foreach (XElement elemChCh in elemCh.Elements())
                            {
                                if ("float".Equals(elemChCh.Name.LocalName))
                                {
                                    extrudersTempList.AddLast(float.Parse(elemCh.Value, CultureInfo.InvariantCulture));
                                }
                            }
                            container.snapshot.extrudersTemp = extrudersTempList.ToArray();
                        }

                        else if ("bedTemp".Equals(elemCh.Name.LocalName))
                        {
                            container.snapshot.bedTemp = float.Parse(elemCh.Value, CultureInfo.InvariantCulture);
                        }
                        else if ("layer".Equals(elemCh.Name.LocalName))
                        {
                            container.snapshot.layer = int.Parse(elemCh.Value, CultureInfo.InvariantCulture);
                        }
                        else if ("activeExtruderId".Equals(elemCh.Name.LocalName))
                        {
                            container.snapshot.activeExtruderId = int.Parse(elemCh.Value, CultureInfo.InvariantCulture);
                        }
                        else if ("activeExtruderValue".Equals(elemCh.Name.LocalName))
                        {
                            container.snapshot.activeExtruderValue = float.Parse(elemCh.Value, CultureInfo.InvariantCulture);
                        }
                        else if ("remainingCode".Equals(elemCh.Name.LocalName))
                        {
                            container.snapshot.remainingCode = elemCh.Value;
                        }
                    }
                }
            }
            try
            {
                ValidateSnapshot(container);
                return container.snapshot;
            }
            catch (InvalidOperationException e)
            {
                throw new IOException("Invalid state file.", e);
            }
        }

        private static void ValidateSnapshot(SnapshotContainer container)
        {
            if (container.version == null || !container.version.Equals(ContainerVersion))
            {
                throw new IOException("Invalid state file. Unsupported version number.");
            }
            else if (!container.type.Equals(SnapshotTypeStateAndRemainingGCode))
            {
                throw new IOException("Invalid state file. Unknown type.");
            }
        }


        public static void SaveSnapshotFile(PrintingStateSnapshot state, string path)
        {
            Stream stream = File.OpenWrite(path);
            try
            {
                SaveSnapshotFile(state, stream);
            }
            finally
            {
                stream.Close();
            }
        }
        public static void SaveSnapshotFile(PrintingStateSnapshot state, Stream fileStream)
        {
            SnapshotContainer container = new SnapshotContainer();

            container.type = SnapshotTypeStateAndRemainingGCode;
            container.version = ContainerVersion;
            container.snapshot = state;

            Serialize(container, fileStream);
        }

        private static void Serialize(SnapshotContainer container, Stream fileStream)
        {
            //System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(container.GetType());
            XElement[] extrudersTempNode = new XElement[container.snapshot.extrudersTemp.Count()];
            for (int i = 0; i < container.snapshot.extrudersTemp.Count(); i++)
            {
                extrudersTempNode[i] = new XElement("float", container.snapshot.extrudersTemp[i]);
            }

            XElement containerNode = new XElement("SnapshotContainer",
                new XElement("version", container.version),
                new XElement("type", container.type),
                new XElement("snapshot",
                    new XElement("x", container.snapshot.x),
                    new XElement("y", container.snapshot.y),
                    new XElement("z", container.snapshot.z),
                    new XElement("speed", container.snapshot.speed),
                    new XElement("fanVoltage", container.snapshot.fanVoltage),
                    new XElement("fanOn", container.snapshot.fanOn),
                    new XElement("relative", container.snapshot.relative),
                    new XElement("extrudersTemp", extrudersTempNode),
                    new XElement("bedTemp", container.snapshot.bedTemp),
                    new XElement("layer", container.snapshot.layer),
                    new XElement("activeExtruderId", container.snapshot.activeExtruderId),
                    new XElement("activeExtruderValue", container.snapshot.activeExtruderValue),
                    new XElement("remainingCode", container.snapshot.remainingCode)
                )
            );
           
            try
            {
                containerNode.Save(new StreamWriter(fileStream));
            }
            catch (InvalidOperationException ex)
            {
                throw new IOException("Failed to write state file.", ex);
            }
        }

        public class SnapshotContainer
        {
            public String version;
            public String type;
            public PrintingStateSnapshot snapshot;
        }
    }

}
