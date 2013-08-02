namespace RepetierHost.view
{
    partial class PendingPrintJobsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PendingPrintJobsDialog));
            this.pendingJobsListbox = new System.Windows.Forms.ListBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonKillJob = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSelectJob = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pendingJobsListbox
            // 
            this.pendingJobsListbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pendingJobsListbox.FormattingEnabled = true;
            this.pendingJobsListbox.Location = new System.Drawing.Point(0, 39);
            this.pendingJobsListbox.Name = "pendingJobsListbox";
            this.pendingJobsListbox.Size = new System.Drawing.Size(550, 329);
            this.pendingJobsListbox.TabIndex = 0;
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
            this.toolStripButtonSelectJob,
            this.toolStripButtonKillJob});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(552, 39);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonKillJob
            // 
            this.toolStripButtonKillJob.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonKillJob.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonKillJob.Image")));
            this.toolStripButtonKillJob.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonKillJob.Name = "toolStripButtonKillJob";
            this.toolStripButtonKillJob.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonKillJob.Click += new System.EventHandler(this.toolStripButtonKillJob_Click);
            // 
            // toolStripButtonSelectJob
            // 
            this.toolStripButtonSelectJob.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelectJob.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSelectJob.Image")));
            this.toolStripButtonSelectJob.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelectJob.Name = "toolStripButtonSelectJob";
            this.toolStripButtonSelectJob.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonSelectJob.Text = "toolStripButton2";
            this.toolStripButtonSelectJob.Click += new System.EventHandler(this.toolStripButtonSelectJob_Click);
            // 
            // PendingPrintJobsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 407);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.pendingJobsListbox);
            this.Name = "PendingPrintJobsDialog";
            this.Text = "PendingPrintJobsDialog";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox pendingJobsListbox;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonKillJob;
        private System.Windows.Forms.ToolStripButton toolStripButtonSelectJob;
    }
}