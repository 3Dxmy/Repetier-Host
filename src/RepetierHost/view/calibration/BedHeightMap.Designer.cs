namespace RepetierHost.view.calibration
{
    partial class BedHeightMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BedHeightMap));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonMeasureHeights = new System.Windows.Forms.Button();
            this.textYPoints = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textXPoints = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textYMax = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textXMax = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textYMin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textXMin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonResultToClipboard = new System.Windows.Forms.Button();
            this.map = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelZCenterValue = new System.Windows.Forms.Label();
            this.labelZAvgValue = new System.Windows.Forms.Label();
            this.labelZMaxValue = new System.Windows.Forms.Label();
            this.labelZCenter = new System.Windows.Forms.Label();
            this.labelZAvg = new System.Windows.Forms.Label();
            this.labelZMax = new System.Windows.Forms.Label();
            this.labelZMinValue = new System.Windows.Forms.Label();
            this.labelZMin = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.map)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonMeasureHeights);
            this.groupBox1.Controls.Add(this.textYPoints);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textXPoints);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textYMax);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textXMax);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textYMin);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textXMin);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 217);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scan Area";
            // 
            // buttonMeasureHeights
            // 
            this.buttonMeasureHeights.Location = new System.Drawing.Point(10, 180);
            this.buttonMeasureHeights.Name = "buttonMeasureHeights";
            this.buttonMeasureHeights.Size = new System.Drawing.Size(150, 23);
            this.buttonMeasureHeights.TabIndex = 6;
            this.buttonMeasureHeights.Text = "Measure heights";
            this.buttonMeasureHeights.UseVisualStyleBackColor = true;
            this.buttonMeasureHeights.Click += new System.EventHandler(this.buttonMeasureHeights_Click);
            // 
            // textYPoints
            // 
            this.textYPoints.Location = new System.Drawing.Point(60, 150);
            this.textYPoints.Name = "textYPoints";
            this.textYPoints.Size = new System.Drawing.Size(100, 20);
            this.textYPoints.TabIndex = 5;
            this.textYPoints.Text = "8";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 153);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "X-Points:";
            // 
            // textXPoints
            // 
            this.textXPoints.Location = new System.Drawing.Point(60, 124);
            this.textXPoints.Name = "textXPoints";
            this.textXPoints.Size = new System.Drawing.Size(100, 20);
            this.textXPoints.TabIndex = 4;
            this.textXPoints.Text = "8";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "X-Points:";
            // 
            // textYMax
            // 
            this.textYMax.Location = new System.Drawing.Point(60, 98);
            this.textYMax.Name = "textYMax";
            this.textYMax.Size = new System.Drawing.Size(100, 20);
            this.textYMax.TabIndex = 3;
            this.textYMax.Tag = "";
            this.textYMax.Text = "80";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Y-max:";
            // 
            // textXMax
            // 
            this.textXMax.Location = new System.Drawing.Point(60, 72);
            this.textXMax.Name = "textXMax";
            this.textXMax.Size = new System.Drawing.Size(100, 20);
            this.textXMax.TabIndex = 2;
            this.textXMax.Tag = "";
            this.textXMax.Text = "80";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "X-max:";
            // 
            // textYMin
            // 
            this.textYMin.Location = new System.Drawing.Point(60, 46);
            this.textYMin.Name = "textYMin";
            this.textYMin.Size = new System.Drawing.Size(100, 20);
            this.textYMin.TabIndex = 1;
            this.textYMin.Text = "-80";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Y-min:";
            // 
            // textXMin
            // 
            this.textXMin.Location = new System.Drawing.Point(60, 20);
            this.textXMin.Name = "textXMin";
            this.textXMin.Size = new System.Drawing.Size(100, 20);
            this.textXMin.TabIndex = 0;
            this.textXMin.Text = "-80";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X-min:";
            // 
            // buttonResultToClipboard
            // 
            this.buttonResultToClipboard.Enabled = false;
            this.buttonResultToClipboard.Location = new System.Drawing.Point(11, 123);
            this.buttonResultToClipboard.Name = "buttonResultToClipboard";
            this.buttonResultToClipboard.Size = new System.Drawing.Size(150, 23);
            this.buttonResultToClipboard.TabIndex = 0;
            this.buttonResultToClipboard.Text = "Result to Clipboard";
            this.buttonResultToClipboard.UseVisualStyleBackColor = true;
            this.buttonResultToClipboard.Click += new System.EventHandler(this.buttonResultToClipboard_Click);
            // 
            // map
            // 
            this.map.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.map.Location = new System.Drawing.Point(213, 12);
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(520, 502);
            this.map.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.map.TabIndex = 1;
            this.map.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelZCenterValue);
            this.groupBox2.Controls.Add(this.labelZAvgValue);
            this.groupBox2.Controls.Add(this.labelZMaxValue);
            this.groupBox2.Controls.Add(this.labelZCenter);
            this.groupBox2.Controls.Add(this.labelZAvg);
            this.groupBox2.Controls.Add(this.labelZMax);
            this.groupBox2.Controls.Add(this.labelZMinValue);
            this.groupBox2.Controls.Add(this.labelZMin);
            this.groupBox2.Controls.Add(this.buttonResultToClipboard);
            this.groupBox2.Location = new System.Drawing.Point(12, 236);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(194, 174);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Scan results";
            // 
            // labelZCenterValue
            // 
            this.labelZCenterValue.AutoSize = true;
            this.labelZCenterValue.Location = new System.Drawing.Point(101, 90);
            this.labelZCenterValue.Name = "labelZCenterValue";
            this.labelZCenterValue.Size = new System.Drawing.Size(25, 13);
            this.labelZCenterValue.TabIndex = 5;
            this.labelZCenterValue.Text = "???";
            // 
            // labelZAvgValue
            // 
            this.labelZAvgValue.AutoSize = true;
            this.labelZAvgValue.Location = new System.Drawing.Point(101, 66);
            this.labelZAvgValue.Name = "labelZAvgValue";
            this.labelZAvgValue.Size = new System.Drawing.Size(25, 13);
            this.labelZAvgValue.TabIndex = 5;
            this.labelZAvgValue.Text = "???";
            // 
            // labelZMaxValue
            // 
            this.labelZMaxValue.AutoSize = true;
            this.labelZMaxValue.Location = new System.Drawing.Point(101, 42);
            this.labelZMaxValue.Name = "labelZMaxValue";
            this.labelZMaxValue.Size = new System.Drawing.Size(25, 13);
            this.labelZMaxValue.TabIndex = 5;
            this.labelZMaxValue.Text = "???";
            // 
            // labelZCenter
            // 
            this.labelZCenter.AutoSize = true;
            this.labelZCenter.Location = new System.Drawing.Point(11, 90);
            this.labelZCenter.Name = "labelZCenter";
            this.labelZCenter.Size = new System.Drawing.Size(50, 13);
            this.labelZCenter.TabIndex = 4;
            this.labelZCenter.Text = "Z center:";
            // 
            // labelZAvg
            // 
            this.labelZAvg.AutoSize = true;
            this.labelZAvg.Location = new System.Drawing.Point(11, 66);
            this.labelZAvg.Name = "labelZAvg";
            this.labelZAvg.Size = new System.Drawing.Size(59, 13);
            this.labelZAvg.TabIndex = 4;
            this.labelZAvg.Text = "Z average:";
            // 
            // labelZMax
            // 
            this.labelZMax.AutoSize = true;
            this.labelZMax.Location = new System.Drawing.Point(11, 42);
            this.labelZMax.Name = "labelZMax";
            this.labelZMax.Size = new System.Drawing.Size(39, 13);
            this.labelZMax.TabIndex = 4;
            this.labelZMax.Text = "Z max:";
            // 
            // labelZMinValue
            // 
            this.labelZMinValue.AutoSize = true;
            this.labelZMinValue.Location = new System.Drawing.Point(101, 20);
            this.labelZMinValue.Name = "labelZMinValue";
            this.labelZMinValue.Size = new System.Drawing.Size(25, 13);
            this.labelZMinValue.TabIndex = 5;
            this.labelZMinValue.Text = "???";
            // 
            // labelZMin
            // 
            this.labelZMin.AutoSize = true;
            this.labelZMin.Location = new System.Drawing.Point(11, 20);
            this.labelZMin.Name = "labelZMin";
            this.labelZMin.Size = new System.Drawing.Size(36, 13);
            this.labelZMin.TabIndex = 4;
            this.labelZMin.Text = "Z min:";
            // 
            // BedHeightMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 526);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.map);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BedHeightMap";
            this.Text = "Create height map";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.map)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonResultToClipboard;
        private System.Windows.Forms.Button buttonMeasureHeights;
        private System.Windows.Forms.TextBox textYPoints;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textXPoints;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textYMax;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textXMax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textYMin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textXMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox map;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelZCenterValue;
        private System.Windows.Forms.Label labelZAvgValue;
        private System.Windows.Forms.Label labelZMaxValue;
        private System.Windows.Forms.Label labelZCenter;
        private System.Windows.Forms.Label labelZAvg;
        private System.Windows.Forms.Label labelZMax;
        private System.Windows.Forms.Label labelZMinValue;
        private System.Windows.Forms.Label labelZMin;
    }
}