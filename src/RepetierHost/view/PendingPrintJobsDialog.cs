using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RepetierHost.model;
using RepetierHost.view.utils;

namespace RepetierHost.view
{
    public partial class PendingPrintJobsDialog : Form
    {
        public PendingPrintJob selectedJob;

        public PendingPrintJobsDialog()
        {
            InitializeComponent();

            LoadPendingJobs();
        }

        private void LoadPendingJobs()
        {
            List<PendingPrintJob> list = PendingPrintJobs.GetPendingJobs();
            foreach (PendingPrintJob job in list)
            {
                pendingJobsListbox.Items.Add(job);
            }
        }

        public PendingPrintJob GetSelectedJob()
        {
            return selectedJob;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButtonSelectJob_Click(object sender, EventArgs e)
        {
            this.selectedJob = (PendingPrintJob)pendingJobsListbox.SelectedItem;
            this.Close();
        }

        private void toolStripButtonKillJob_Click(object sender, EventArgs e)
        {
            PendingPrintJob job = (PendingPrintJob)pendingJobsListbox.SelectedItem;
            if (job != null)
            {
                if (MessageBox.Show("Are you sure you want to delete job " + job.Name + "?", "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        job.Delete();
                        pendingJobsListbox.Items.Remove(job);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while deleting state file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void toolStripButtonRename_Click(object sender, EventArgs e)
        {
            PendingPrintJob job = (PendingPrintJob)pendingJobsListbox.SelectedItem;
            if (job != null)
            {
                string currentSnapshotName = job.Name;
                string newSnapshotName = ReadSnapshotName(currentSnapshotName);
                if (newSnapshotName == null)
                {
                    // User cancelled
                    return;
                }
                if (!newSnapshotName.Equals(currentSnapshotName))
                {
                    try
                    {
                        job.Rename(newSnapshotName);
                        pendingJobsListbox.Items[pendingJobsListbox.SelectedIndex] = job;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("An error occurred while renaming state file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Show the user a dialog requesting a snapshot name.
        /// If the name is iinvalid, ask him again until he sets a right name.
        /// If the user cancels, returns null.
        /// </summary>
        /// <returns></returns>
        private static string ReadSnapshotName(string currentSnapshotName)
        {
            string snapshotName = currentSnapshotName;
            do
            {
                snapshotName = StringInput.GetString("Snapshot Name", "Please, write a snapshot name:", snapshotName, true);
                if (snapshotName == null)
                {
                    // User cancelled.
                    return null;
                }
            } while (PendingPrintJob.IsInvalidSnapshotName(snapshotName));

            return snapshotName;
        }
     
    }
}
