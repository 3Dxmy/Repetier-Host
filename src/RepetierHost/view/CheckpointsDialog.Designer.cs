namespace RepetierHost.view
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
            this.checkpointsListbox = new System.Windows.Forms.ListBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSelectCheckpoint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonKillCheckpoint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRename = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGo = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pendingJobsListbox
            // 
            this.checkpointsListbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkpointsListbox.FormattingEnabled = true;
            this.checkpointsListbox.Location = new System.Drawing.Point(0, 39);
            this.checkpointsListbox.Name = "pendingJobsListbox";
            this.checkpointsListbox.Size = new System.Drawing.Size(550, 329);
            this.checkpointsListbox.TabIndex = 0;
            this.checkpointsListbox.DoubleClick += new System.EventHandler(this.pendingCheckpointsListbox_DoubleClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(443, 374);
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
            this.toolStripButtonKillCheckpoint,
            this.toolStripButtonRename,
            this.toolStripButtonGo});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(552, 39);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonSelectJob
            // 
            this.toolStripButtonSelectCheckpoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelectCheckpoint.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSelectJob.Image")));
            this.toolStripButtonSelectCheckpoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelectCheckpoint.Name = "toolStripButtonSelectJob";
            this.toolStripButtonSelectCheckpoint.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonSelectCheckpoint.Text = "toolStripButton2";
            this.toolStripButtonSelectCheckpoint.ToolTipText = "Restore Job";
            this.toolStripButtonSelectCheckpoint.Click += new System.EventHandler(this.toolStripButtonSelectCheckpoint_Click);
            // 
            // toolStripButtonKillJob
            // 
            this.toolStripButtonKillCheckpoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonKillCheckpoint.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonKillJob.Image")));
            this.toolStripButtonKillCheckpoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonKillCheckpoint.Name = "toolStripButtonKillJob";
            this.toolStripButtonKillCheckpoint.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonKillCheckpoint.ToolTipText = "Kill Job";
            this.toolStripButtonKillCheckpoint.Click += new System.EventHandler(this.toolStripButtonKillCheckpoint_Click);
            // 
            // toolStripButtonRename
            // 
            this.toolStripButtonRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRename.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRename.Image")));
            this.toolStripButtonRename.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRename.Name = "toolStripButtonRename";
            this.toolStripButtonRename.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonRename.Text = "toolStripButtonRename";
            this.toolStripButtonRename.ToolTipText = "Rename";
            this.toolStripButtonRename.Click += new System.EventHandler(this.toolStripButtonRename_Click);
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
            // CheckpointsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 407);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.checkpointsListbox);
            this.Name = "CheckpointsDialog";
            this.Text = "PendingPrintJobsDialog";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox checkpointsListbox;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonKillCheckpoint;
        private System.Windows.Forms.ToolStripButton toolStripButtonSelectCheckpoint;
        private System.Windows.Forms.ToolStripButton toolStripButtonRename;
        private System.Windows.Forms.ToolStripButton toolStripButtonGo;
    }
}