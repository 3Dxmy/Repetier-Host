using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
            get { string fileName = Path.GetFileName(path); return fileName.Substring(0, fileName.LastIndexOf(".")); }
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
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(SnapshotContainer));
            try
            {
                SnapshotContainer container = (SnapshotContainer)x.Deserialize(stream);
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

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(container.GetType());
            try
            {
                x.Serialize(fileStream, container);
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
