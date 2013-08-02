using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RepetierHost.model;

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
                    job.Delete();
                    pendingJobsListbox.Items.Remove(job);
                }
            }
        }
     
    }
}
