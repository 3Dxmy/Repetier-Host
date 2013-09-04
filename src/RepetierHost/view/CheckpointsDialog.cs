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
        private PrintingCheckpointsControl checkpointsListbox;

        public CheckpointsDialog()
        {
            InitializeComponent();

            translate();
            Main.main.languageChanged += translate;
        }

        private void translate()
        {
            Text = Trans.T("W_CHECKPOINTS");
            toolStripButtonSelectCheckpoint.ToolTipText = Trans.T("M_CHECKPOINT_RESTORE_CHECKPOINT");
            toolStripButtonGo.ToolTipText = Trans.T("M_CHECKPOINT_GO_TO_POSITION");
            toolStripButtonRefresh.ToolTipText = Trans.T("M_CHECKPOINT_REFRESH");
            toolStripButtonSelectCurrentLayer.ToolTipText = Trans.T("M_CHECKPOINT_GO_TO_CURRENT_LAYER");
            toolStripButtonNext.ToolTipText = Trans.T("M_CHECKPOINT_NEXT");
            toolStripButtonLast.ToolTipText = Trans.T("M_CHECKPOINT_PREVIOUS");
            checkBoxUpdate3dView.Text = Trans.T("L_CHECKPOINT_SHOW_CHECKPOINT_IN_3D_VIEW");
            checkBoxMoveExtruder.Text = Trans.T("L_CHECKPOINT_MOVE_EXTRUDER_TO_CHECKPOINT");
            checkBoxPreviewCheckpoint.Text = Trans.T("L_CHECKPOINT_PREVIEW_CHECKPOINT");
            buttonCancel.Text = Trans.T("B_CANCEL");
        }

        private static CheckpointsDialog dialog;
        public static void Execute()
        {
            if (dialog == null)
            {
                dialog = new CheckpointsDialog();
            }
            dialog.Show();
            dialog.LoadCheckpoints();
        }
        internal static bool CheckpointViewMode()
        {
            return dialog != null && dialog.Visible && dialog.checkBoxPreviewCheckpoint.Checked;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Main.main.languageChanged -= translate;
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
            Main.main.checkpointsView = null;
            Main.main.assign3DView();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case (Keys.Escape):
                    Cancel();
                break;
                case (Keys.Return):
                    RestoreCheckpoint();
                break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LoadCheckpoints()
        {
            lock (this)
            {
                LinkedList<PrintingCheckpoint> list = Main.main.checkpoints.GetCheckPoints();
                checkpointsListbox = new PrintingCheckpointsControl(list);
                checkpointsListbox.onCurrentCheckpointChanged += RefreshCheckpointDescription;
                checkpointsListbox.GoToLast();

                RedrawCurrentCheckpoint();
            }
        }
        private void RedrawCurrentCheckpoint()
        {
            if (checkpointsListbox.GetCurrent() != null && checkBoxPreviewCheckpoint.Checked)
            {
                GCodeVisual gcodeVisual = new GCodeVisual();
                gcodeVisual.showSelection = true;
                gcodeVisual.minLayer = 0;
                gcodeVisual.maxLayer = 999999;
                Main.main.checkpointsView = new ThreeDView();
                Main.main.checkpointsView.editor = false;
                Main.main.checkpointsView.models.AddLast(gcodeVisual);
                Main.main.assign3DView();

                gcodeVisual.parseGCodeShortArray(ToGCodeShortArray(checkpointsListbox.GetCurrent().GetCodeAlreadyExecuted()), false, 0);
            }
        }

        private List<GCodeShort> ToGCodeShortArray(IEnumerable<GCodeCompressed> lCompressed)
        {
            List<GCodeShort> transfList = new List<GCodeShort>();
            foreach (GCodeCompressed g in lCompressed)
            {
                transfList.Add(new GCodeShort(g.getCommand()));
            }
            return transfList;
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

        private void toolStripButtonGo_Click(object sender, EventArgs e)
        {
            GoToCheckpointPosition();
        }
        
        private void RestoreCheckpoint()
        {
            if (Main.conn.connector.IsJobRunning())
            {
                MessageBox.Show(Trans.T("L_CHECKPOINT_STOP_JOB_BEFORE_RESTORING_POINT"));
            }
            else
            {
                this.chk = (PrintingCheckpoint)checkpointsListbox.GetCurrent();
                if (this.chk != null)
                {
                    chk.RestoreState(new PrinterConnectionGCodeExecutor(Main.conn, false));
                }
                this.Close();
            }
        }

        private void GoToCheckpointPosition()
        {
            this.chk = (PrintingCheckpoint)checkpointsListbox.GetCurrent();
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

        private void Cancel()
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private static string ReadCheckpointName(string currentSnapshotName)
        {
            return StringInput.GetString(Trans.T("L_POSTPONED_CHECKPOINT_NAME"), Trans.T("L_NAME_POSTPONED_CHECKPOINT"), currentSnapshotName, true);
        }

        private void toolStripButtonSelectCurrentLayer_Click(object sender, EventArgs e)
        {
            lock (this)
            {
                if (checkBoxMoveExtruder.Checked && Main.conn.connector.IsJobRunning())
                {
                    MessageBox.Show(Trans.T("L_CHECKPOINT_STOP_JOB_BEFORE_GOING_TO_POSITION"));
                }
                else
                {
                    MoveToCurrentLayerCheckpoint();

                    if (checkBoxMoveExtruder.Checked)
                    {
                        GoToCheckpointPosition();
                    }
                    if (checkBoxUpdate3dView.Checked)
                    {
                        RedrawCurrentCheckpoint();
                    }
                }
            }
        }

        private void MoveToCurrentLayerCheckpoint()
        {
            checkpointsListbox.Reset();
            while (checkpointsListbox.HasNext())
            {
                PrintingCheckpoint o = checkpointsListbox.MoveToNext();
                if (o.z >= Main.conn.analyzer.z)
                {
                    return;
                }
            }
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            lock (this)
            {
                LoadCheckpoints();
            }
        }

        private void toolStripButtonNext_Click(object sender, EventArgs e)
        {
            lock (this)
            {
                if (checkBoxMoveExtruder.Checked && Main.conn.connector.IsJobRunning())
                {
                    MessageBox.Show(Trans.T("L_CHECKPOINT_STOP_JOB_BEFORE_GOING_TO_POSITION"));
                }
                else
                {
                    checkpointsListbox.MoveToNext();
                    if (checkBoxMoveExtruder.Checked)
                    {
                        GoToCheckpointPosition();
                    }
                    if (checkBoxUpdate3dView.Checked)
                    {
                        RedrawCurrentCheckpoint();
                    }
                }
            }
        }

        private void toolStripButtonLast_Click(object sender, EventArgs e)
        {
            lock (this)
            {
                if (checkBoxMoveExtruder.Checked && Main.conn.connector.IsJobRunning())
                {
                    MessageBox.Show(Trans.T("L_CHECKPOINT_STOP_JOB_BEFORE_GOING_TO_POSITION"));
                }
                else
                {
                    checkpointsListbox.MoveToPrevious();
                    if (checkBoxMoveExtruder.Checked)
                    {
                        GoToCheckpointPosition();
                    }
                    if (checkBoxUpdate3dView.Checked)
                    {
                        RedrawCurrentCheckpoint();
                    }
                }
            }
        }

        private void checkBoxPreviewCheckpoint_CheckedChanged(object sender, EventArgs e)
        {
            Main.main.assign3DView();
        }

        private void checkBoxUpdate3dView_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxUpdate3dView.Checked)
            {
                RedrawCurrentCheckpoint();
            }
        }

        private void CheckpointsDialog_Load(object sender, EventArgs e)
        {

        }

        public void RefreshCheckpointDescription()
        {
            PrintingCheckpoint chk = checkpointsListbox.GetCurrent();
            if (chk != null)
            {
                labelCheckpointData.Text = chk.Name;
            }
            else
            {
                labelCheckpointData.Text = "";
            }
        }
    }


    public delegate void OnCurrentCheckpointChanged();
    public class PrintingCheckpointsControl
    {
        private LinkedList<PrintingCheckpoint> list;
        private int index;
        public OnCurrentCheckpointChanged onCurrentCheckpointChanged;
        public PrintingCheckpointsControl(LinkedList<PrintingCheckpoint> list)
        {
            this.list = list;
            this.index = -1;
            if (onCurrentCheckpointChanged != null)
            {
                onCurrentCheckpointChanged();
            }
        }
        public PrintingCheckpoint GetCurrent()
        {
            if (index >= 0 && index < list.Count)
            {
                return list.ElementAt(index);
            }
            else
            {
                return null;
            }
        }
        public int GetCurrentPosition()
        {
            return index;
        }
        public void GoToLast()
        {
            index = list.Count - 1;
            if (onCurrentCheckpointChanged != null)
            {
                onCurrentCheckpointChanged();
            }
        }
        public void Reset()
        {
            index = 0;
            if (onCurrentCheckpointChanged != null)
            {
                onCurrentCheckpointChanged();
            }
        }
        public bool HasNext()
        {
            return index < list.Count - 1;
        }
        public bool HasPrevious()
        {
            return index > 0;
        }
        public PrintingCheckpoint MoveToNext()
        {
            if (HasNext())
            {
                index++;
                if (onCurrentCheckpointChanged != null)
                {
                    onCurrentCheckpointChanged();
                }
            }
            return GetCurrent();
        }
        public PrintingCheckpoint MoveToPrevious()
        {
            if (HasPrevious())
            {
                index--;
                if (onCurrentCheckpointChanged != null)
                {
                    onCurrentCheckpointChanged();
                }
            }
            return GetCurrent();
        }

    }

}
