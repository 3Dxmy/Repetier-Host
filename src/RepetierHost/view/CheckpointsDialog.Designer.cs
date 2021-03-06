﻿namespace RepetierHost.view
{
    partial class CheckpointsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckpointsDialog));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSelectCheckpoint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHome = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGoToLast = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSelectCurrentLayer = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGoToNearest = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPrevious = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFF = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPreviousLayer = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNextLayer = new System.Windows.Forms.ToolStripButton();
            this.checkBoxUpdate3dView = new System.Windows.Forms.CheckBox();
            this.checkBoxMoveExtruder = new System.Windows.Forms.CheckBox();
            this.checkBoxPreviewCheckpoint = new System.Windows.Forms.CheckBox();
            this.labelCheckpointData = new System.Windows.Forms.Label();
            this.toolStripButtonLockZ = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(465, 139);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSelectCheckpoint,
            this.toolStripButtonGo,
            this.toolStripButtonHome,
            this.toolStripButtonGoToLast,
            this.toolStripButtonSelectCurrentLayer,
            this.toolStripButtonGoToNearest,
            this.toolStripButtonPrevious,
            this.toolStripButtonNext,
            this.toolStripButtonRew,
            this.toolStripButtonFF,
            this.toolStripButtonPreviousLayer,
            this.toolStripButtonNextLayer,
            this.toolStripButtonLockZ});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(552, 39);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonSelectCheckpoint
            // 
            this.toolStripButtonSelectCheckpoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelectCheckpoint.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSelectCheckpoint.Image")));
            this.toolStripButtonSelectCheckpoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelectCheckpoint.Name = "toolStripButtonSelectCheckpoint";
            this.toolStripButtonSelectCheckpoint.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonSelectCheckpoint.Text = "toolStripButton2";
            this.toolStripButtonSelectCheckpoint.ToolTipText = "Restore Job";
            this.toolStripButtonSelectCheckpoint.Click += new System.EventHandler(this.toolStripButtonSelectCheckpoint_Click);
            // 
            // toolStripButtonGo
            // 
            this.toolStripButtonGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGo.Image")));
            this.toolStripButtonGo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGo.Name = "toolStripButtonGo";
            this.toolStripButtonGo.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonGo.Text = "toolStripButtonGo";
            this.toolStripButtonGo.Click += new System.EventHandler(this.toolStripButtonGo_Click);
            // 
            // toolStripButtonHome
            // 
            this.toolStripButtonHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHome.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHome.Image")));
            this.toolStripButtonHome.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHome.Name = "toolStripButtonHome";
            this.toolStripButtonHome.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonHome.Text = "toolStripButton1";
            this.toolStripButtonHome.Click += new System.EventHandler(this.toolStripButtonHome_Click);
            // 
            // toolStripButtonGoToLast
            // 
            this.toolStripButtonGoToLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGoToLast.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGoToLast.Image")));
            this.toolStripButtonGoToLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGoToLast.Name = "toolStripButtonGoToLast";
            this.toolStripButtonGoToLast.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonGoToLast.Text = "toolStripButtonRefresh";
            this.toolStripButtonGoToLast.Click += new System.EventHandler(this.toolStripButtonGoToLast_Click);
            // 
            // toolStripButtonSelectCurrentLayer
            // 
            this.toolStripButtonSelectCurrentLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelectCurrentLayer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSelectCurrentLayer.Image")));
            this.toolStripButtonSelectCurrentLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelectCurrentLayer.Name = "toolStripButtonSelectCurrentLayer";
            this.toolStripButtonSelectCurrentLayer.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonSelectCurrentLayer.Text = "toolStripButtonSelectCurrentLayer";
            this.toolStripButtonSelectCurrentLayer.Click += new System.EventHandler(this.toolStripButtonSelectCurrentLayer_Click);
            // 
            // toolStripButtonGoToNearest
            // 
            this.toolStripButtonGoToNearest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGoToNearest.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGoToNearest.Image")));
            this.toolStripButtonGoToNearest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGoToNearest.Name = "toolStripButtonGoToNearest";
            this.toolStripButtonGoToNearest.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonGoToNearest.Text = "toolStripButton1";
            this.toolStripButtonGoToNearest.Click += new System.EventHandler(this.toolStripButtonGoToNearest_Click);
            // 
            // toolStripButtonPrevious
            // 
            this.toolStripButtonPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPrevious.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPrevious.Image")));
            this.toolStripButtonPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrevious.Name = "toolStripButtonPrevious";
            this.toolStripButtonPrevious.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonPrevious.Text = "toolStripButtonPrevious";
            this.toolStripButtonPrevious.Click += new System.EventHandler(this.toolStripButtonPrevious_Click);
            // 
            // toolStripButtonNext
            // 
            this.toolStripButtonNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNext.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNext.Image")));
            this.toolStripButtonNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNext.Name = "toolStripButtonNext";
            this.toolStripButtonNext.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonNext.Text = "toolStripButtonNext";
            this.toolStripButtonNext.Click += new System.EventHandler(this.toolStripButtonNext_Click);
            // 
            // toolStripButtonRew
            // 
            this.toolStripButtonRew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRew.Image")));
            this.toolStripButtonRew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRew.Name = "toolStripButtonRew";
            this.toolStripButtonRew.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonRew.Text = "toolStripButton1";
            this.toolStripButtonRew.Click += new System.EventHandler(this.toolStripButtonRew_Click);
            // 
            // toolStripButtonFF
            // 
            this.toolStripButtonFF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFF.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFF.Image")));
            this.toolStripButtonFF.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFF.Name = "toolStripButtonFF";
            this.toolStripButtonFF.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonFF.Text = "toolStripButton1";
            this.toolStripButtonFF.Click += new System.EventHandler(this.toolStripButtonFF_Click);
            // 
            // toolStripButtonPreviousLayer
            // 
            this.toolStripButtonPreviousLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPreviousLayer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPreviousLayer.Image")));
            this.toolStripButtonPreviousLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPreviousLayer.Name = "toolStripButtonPreviousLayer";
            this.toolStripButtonPreviousLayer.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonPreviousLayer.Text = "toolStripButton1";
            this.toolStripButtonPreviousLayer.Click += new System.EventHandler(this.toolStripButtonPreviousLayer_Click);
            // 
            // toolStripButtonNextLayer
            // 
            this.toolStripButtonNextLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNextLayer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNextLayer.Image")));
            this.toolStripButtonNextLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNextLayer.Name = "toolStripButtonNextLayer";
            this.toolStripButtonNextLayer.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonNextLayer.Text = "toolStripButton1";
            this.toolStripButtonNextLayer.Click += new System.EventHandler(this.toolStripButtonNextLayer_Click);
            // 
            // checkBoxUpdate3dView
            // 
            this.checkBoxUpdate3dView.AutoSize = true;
            this.checkBoxUpdate3dView.Checked = true;
            this.checkBoxUpdate3dView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUpdate3dView.Location = new System.Drawing.Point(12, 66);
            this.checkBoxUpdate3dView.Name = "checkBoxUpdate3dView";
            this.checkBoxUpdate3dView.Size = new System.Drawing.Size(201, 17);
            this.checkBoxUpdate3dView.TabIndex = 6;
            this.checkBoxUpdate3dView.Text = "Show Checkpoint position in 3d View";
            this.checkBoxUpdate3dView.UseVisualStyleBackColor = true;
            this.checkBoxUpdate3dView.CheckedChanged += new System.EventHandler(this.checkBoxUpdate3dView_CheckedChanged);
            // 
            // checkBoxMoveExtruder
            // 
            this.checkBoxMoveExtruder.AutoSize = true;
            this.checkBoxMoveExtruder.Location = new System.Drawing.Point(12, 89);
            this.checkBoxMoveExtruder.Name = "checkBoxMoveExtruder";
            this.checkBoxMoveExtruder.Size = new System.Drawing.Size(202, 17);
            this.checkBoxMoveExtruder.TabIndex = 7;
            this.checkBoxMoveExtruder.Text = "Move extruder to Checkpoint position";
            this.checkBoxMoveExtruder.UseVisualStyleBackColor = true;
            // 
            // checkBoxPreviewCheckpoint
            // 
            this.checkBoxPreviewCheckpoint.AutoSize = true;
            this.checkBoxPreviewCheckpoint.Checked = true;
            this.checkBoxPreviewCheckpoint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPreviewCheckpoint.Location = new System.Drawing.Point(13, 113);
            this.checkBoxPreviewCheckpoint.Name = "checkBoxPreviewCheckpoint";
            this.checkBoxPreviewCheckpoint.Size = new System.Drawing.Size(121, 17);
            this.checkBoxPreviewCheckpoint.TabIndex = 8;
            this.checkBoxPreviewCheckpoint.Text = "Preview Checkpoint";
            this.checkBoxPreviewCheckpoint.UseVisualStyleBackColor = true;
            this.checkBoxPreviewCheckpoint.CheckedChanged += new System.EventHandler(this.checkBoxPreviewCheckpoint_CheckedChanged);
            // 
            // labelCheckpointData
            // 
            this.labelCheckpointData.Location = new System.Drawing.Point(13, 43);
            this.labelCheckpointData.Name = "labelCheckpointData";
            this.labelCheckpointData.Size = new System.Drawing.Size(527, 20);
            this.labelCheckpointData.TabIndex = 9;
            // 
            // toolStripButtonLockZ
            // 
            this.toolStripButtonLockZ.CheckOnClick = true;
            this.toolStripButtonLockZ.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLockZ.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLockZ.Image")));
            this.toolStripButtonLockZ.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLockZ.Name = "toolStripButtonLockZ";
            this.toolStripButtonLockZ.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonLockZ.Text = "toolStripButton1";
            // 
            // CheckpointsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 174);
            this.Controls.Add(this.labelCheckpointData);
            this.Controls.Add(this.checkBoxPreviewCheckpoint);
            this.Controls.Add(this.checkBoxMoveExtruder);
            this.Controls.Add(this.checkBoxUpdate3dView);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "CheckpointsDialog";
            this.Text = "PendingPrintJobsDialog";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSelectCheckpoint;
        private System.Windows.Forms.ToolStripButton toolStripButtonGo;
        private System.Windows.Forms.ToolStripButton toolStripButtonSelectCurrentLayer;
        private System.Windows.Forms.ToolStripButton toolStripButtonGoToLast;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrevious;
        private System.Windows.Forms.ToolStripButton toolStripButtonNext;
        private System.Windows.Forms.CheckBox checkBoxUpdate3dView;
        private System.Windows.Forms.CheckBox checkBoxMoveExtruder;
        private System.Windows.Forms.CheckBox checkBoxPreviewCheckpoint;
        private System.Windows.Forms.Label labelCheckpointData;
        private System.Windows.Forms.ToolStripButton toolStripButtonHome;
        private System.Windows.Forms.ToolStripButton toolStripButtonRew;
        private System.Windows.Forms.ToolStripButton toolStripButtonFF;
        private System.Windows.Forms.ToolStripButton toolStripButtonGoToNearest;
        private System.Windows.Forms.ToolStripButton toolStripButtonPreviousLayer;
        private System.Windows.Forms.ToolStripButton toolStripButtonNextLayer;
        private System.Windows.Forms.ToolStripButton toolStripButtonLockZ;
    }
}