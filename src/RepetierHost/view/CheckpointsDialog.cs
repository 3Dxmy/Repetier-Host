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
    public partial class CheckpointsDialog : Form
    {
        public PrintingCheckpoint chk;

        public CheckpointsDialog()
        {
            InitializeComponent();

            LoadCheckpoints();

            translate();
            Main.main.languageChanged += translate;
        }

        private void translate()
        {
            Text = Trans.T("W_CHECKPOINTS");
            toolStripButtonSelectCheckpoint.ToolTipText = Trans.T("M_CHECKPOINT_SELECT_JOB");
            toolStripButtonKillCheckpoint.ToolTipText = Trans.T("M_CHECKPOINT_KILL");
            toolStripButtonRename.ToolTipText = Trans.T("M_CHECKPOINT_RENAME");
            toolStripButtonGo.ToolTipText = Trans.T("M_CHECKPOINT_GO_TO_POSITION");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Main.main.languageChanged -= translate;
            base.OnClosing(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case (Keys.Escape):
                    Cancel();
                break;
                case (Keys.Delete):
                    KillCheckpoint();
                break;
                case (Keys.Return):
                    RestoreCheckpoint();
                break;
                case (Keys.F2):
                    RenameCheckpoint();
                break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LoadCheckpoints()
        {
            LinkedList<PrintingCheckpoint> list = Main.main.checkpoints.GetCheckPoints();
            foreach (PrintingCheckpoint chk in list)
            {
                checkpointsListbox.Items.Add(chk);
            }
        }

        public PrintingCheckpoint GetSelectedCheckpoint()
        {
            return chk;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void toolStripButtonSelectCheckpoint_Click(object sender, EventArgs e)
        {
            RestoreCheckpoint();
        }

        private void toolStripButtonKillCheckpoint_Click(object sender, EventArgs e)
        {
            KillCheckpoint();
        }

        private void toolStripButtonRename_Click(object sender, EventArgs e)
        {
            RenameCheckpoint();
        }

        private void toolStripButtonGo_Click(object sender, EventArgs e)
        {
            GoToCheckpointPosition();
        }

        private void pendingCheckpointsListbox_DoubleClick(object sender, EventArgs e)
        {
            if (checkpointsListbox.SelectedIndex > -1)
            {
                RestoreCheckpoint();
            }
        }

        
        private void RestoreCheckpoint()
        {
            if (Main.conn.connector.IsJobRunning())
            {
                MessageBox.Show(Trans.T("L_CHECKPOINT_STOP_JOB_BEFORE_RESTORING_POINT"));
            }
            else
            {
                this.chk = (PrintingCheckpoint)checkpointsListbox.SelectedItem;
                this.Close();
            }
        }

        private void GoToCheckpointPosition()
        {
            this.chk = (PrintingCheckpoint)checkpointsListbox.SelectedItem;
            if (chk != null)
            {
                if (Main.conn.connector.IsJobRunning())
                {
                    MessageBox.Show(Trans.T("L_CHECKPOINT_STOP_JOB_BEFORE_GOING_TO_POSITION"));
                }
                else
                {
                    this.chk.RestorePosition(new RunNowGCodeExecutor(Main.conn));
                }
            }
        }

        private void KillCheckpoint()
        {
            PrintingCheckpoint chk = (PrintingCheckpoint)checkpointsListbox.SelectedItem;
            if (chk != null)
            {
                if (MessageBox.Show(Trans.T1("L_CONFIRM_DELETE_CHECKPOINT", chk.Name), Trans.T("L_SECURITY_QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        chk.Delete();
                        checkpointsListbox.Items.Remove(chk);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), Trans.T("L_ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RenameCheckpoint()
        {
            PrintingCheckpoint chk = (PrintingCheckpoint)checkpointsListbox.SelectedItem;
            if (chk != null)
            {
                string currentCheckpointName = chk.Name;
                string newCheckpointName = ReadCheckpointName(currentCheckpointName);
                if (newCheckpointName == null)
                {
                    // User cancelled
                    return;
                }
                if (!newCheckpointName.Equals(currentCheckpointName))
                {
                    try
                    {
                        chk.Rename(newCheckpointName);
                        checkpointsListbox.Items[checkpointsListbox.SelectedIndex] = chk;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), Trans.T("L_ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Cancel()
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private static string ReadCheckpointName(string currentSnapshotName)
        {
            return StringInput.GetString(Trans.T("L_POSTPONED_CHECKPOINT_NAME"), Trans.T("L_NAME_POSTPONED_CHECKPOINT"), currentSnapshotName, true);
        }

    }

}
