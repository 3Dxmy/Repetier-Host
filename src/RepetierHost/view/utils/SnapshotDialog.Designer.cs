namespace RepetierHost.view.utils
{
    partial class SnapshotDialog
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
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.buttonForceSnapshot = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(13, 9);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(290, 26);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "The snapshot will be taken once the next layer is reached.\r\nYou can force taking " +
                "the snapshot now or cancel if needed.";
            // 
            // buttonForceSnapshot
            // 
            this.buttonForceSnapshot.Location = new System.Drawing.Point(174, 58);
            this.buttonForceSnapshot.Name = "buttonForceSnapshot";
            this.buttonForceSnapshot.Size = new System.Drawing.Size(75, 23);
            this.buttonForceSnapshot.TabIndex = 1;
            this.buttonForceSnapshot.Text = "&Force";
            this.buttonForceSnapshot.UseVisualStyleBackColor = true;
            this.buttonForceSnapshot.Click += new System.EventHandler(this.buttonForceSnapshot_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(255, 58);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // SnapshotDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 93);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonForceSnapshot);
            this.Controls.Add(this.descriptionLabel);
            this.Name = "SnapshotDialog";
            this.Text = "SnapshotDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Button buttonForceSnapshot;
        private System.Windows.Forms.Button buttonCancel;
    }
}