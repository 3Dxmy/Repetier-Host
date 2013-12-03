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
            this.groupScanArea = new System.Windows.Forms.GroupBox();
            this.buttonMeasureHeights = new System.Windows.Forms.Button();
            this.textYPoints = new System.Windows.Forms.TextBox();
            this.labelYPoints = new System.Windows.Forms.Label();
            this.textXPoints = new System.Windows.Forms.TextBox();
            this.labelXPoints = new System.Windows.Forms.Label();
            this.textYMax = new System.Windows.Forms.TextBox();
            this.labelYMax = new System.Windows.Forms.Label();
            this.textXMax = new System.Windows.Forms.TextBox();
            this.labelXMax = new System.Windows.Forms.Label();
            this.textYMin = new System.Windows.Forms.TextBox();
            this.labelYMin = new System.Windows.Forms.Label();
            this.textXMin = new System.Windows.Forms.TextBox();
            this.labelXMin = new System.Windows.Forms.Label();
            this.buttonResultToClipboard = new System.Windows.Forms.Button();
            this.map = new System.Windows.Forms.PictureBox();
            this.groupScanResults = new System.Windows.Forms.GroupBox();
            this.labelZCenterValue = new System.Windows.Forms.Label();
            this.labelZAvgValue = new System.Windows.Forms.Label();
            this.labelZMaxValue = new System.Windows.Forms.Label();
            this.labelZCenter = new System.Windows.Forms.Label();
            this.labelZAvg = new System.Windows.Forms.Label();
            this.labelZMax = new System.Windows.Forms.Label();
            this.labelZMinValue = new System.Windows.Forms.Label();
            this.labelZMin = new System.Windows.Forms.Label();
            this.groupScanArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.map)).BeginInit();
            this.groupScanResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupScanArea
            // 
            this.groupScanArea.Controls.Add(this.buttonMeasureHeights);
            this.groupScanArea.Controls.Add(this.textYPoints);
            this.groupScanArea.Controls.Add(this.labelYPoints);
            this.groupScanArea.Controls.Add(this.textXPoints);
            this.groupScanArea.Controls.Add(this.labelXPoints);
            this.groupScanArea.Controls.Add(this.textYMax);
            this.groupScanArea.Controls.Add(this.labelYMax);
            this.groupScanArea.Controls.Add(this.textXMax);
            this.groupScanArea.Controls.Add(this.labelXMax);
            this.groupScanArea.Controls.Add(this.textYMin);
            this.groupScanArea.Controls.Add(this.labelYMin);
            this.groupScanArea.Controls.Add(this.textXMin);
            this.groupScanArea.Controls.Add(this.labelXMin);
            this.groupScanArea.Location = new System.Drawing.Point(13, 13);
            this.groupScanArea.Name = "groupScanArea";
            this.groupScanArea.Size = new System.Drawing.Size(194, 217);
            this.groupScanArea.TabIndex = 0;
            this.groupScanArea.TabStop = false;
            this.groupScanArea.Text = "Scan Area";
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
            // labelYPoints
            // 
            this.labelYPoints.AutoSize = true;
            this.labelYPoints.Location = new System.Drawing.Point(7, 153);
            this.labelYPoints.Name = "labelYPoints";
            this.labelYPoints.Size = new System.Drawing.Size(49, 13);
            this.labelYPoints.TabIndex = 0;
            this.labelYPoints.Text = "Y-Points:";
            // 
            // textXPoints
            // 
            this.textXPoints.Location = new System.Drawing.Point(60, 124);
            this.textXPoints.Name = "textXPoints";
            this.textXPoints.Size = new System.Drawing.Size(100, 20);
            this.textXPoints.TabIndex = 4;
            this.textXPoints.Text = "8";
            // 
            // labelXPoints
            // 
            this.labelXPoints.AutoSize = true;
            this.labelXPoints.Location = new System.Drawing.Point(7, 127);
            this.labelXPoints.Name = "labelXPoints";
            this.labelXPoints.Size = new System.Drawing.Size(49, 13);
            this.labelXPoints.TabIndex = 0;
            this.labelXPoints.Text = "X-Points:";
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
            // labelYMax
            // 
            this.labelYMax.AutoSize = true;
            this.labelYMax.Location = new System.Drawing.Point(7, 101);
            this.labelYMax.Name = "labelYMax";
            this.labelYMax.Size = new System.Drawing.Size(39, 13);
            this.labelYMax.TabIndex = 0;
            this.labelYMax.Text = "Y-max:";
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
            // labelXMax
            // 
            this.labelXMax.AutoSize = true;
            this.labelXMax.Location = new System.Drawing.Point(7, 75);
            this.labelXMax.Name = "labelXMax";
            this.labelXMax.Size = new System.Drawing.Size(39, 13);
            this.labelXMax.TabIndex = 0;
            this.labelXMax.Text = "X-max:";
            // 
            // textYMin
            // 
            this.textYMin.Location = new System.Drawing.Point(60, 46);
            this.textYMin.Name = "textYMin";
            this.textYMin.Size = new System.Drawing.Size(100, 20);
            this.textYMin.TabIndex = 1;
            this.textYMin.Text = "-80";
            // 
            // labelYMin
            // 
            this.labelYMin.AutoSize = true;
            this.labelYMin.Location = new System.Drawing.Point(7, 49);
            this.labelYMin.Name = "labelYMin";
            this.labelYMin.Size = new System.Drawing.Size(36, 13);
            this.labelYMin.TabIndex = 0;
            this.labelYMin.Text = "Y-min:";
            // 
            // textXMin
            // 
            this.textXMin.Location = new System.Drawing.Point(60, 20);
            this.textXMin.Name = "textXMin";
            this.textXMin.Size = new System.Drawing.Size(100, 20);
            this.textXMin.TabIndex = 0;
            this.textXMin.Text = "-80";
            // 
            // labelXMin
            // 
            this.labelXMin.AutoSize = true;
            this.labelXMin.Location = new System.Drawing.Point(7, 23);
            this.labelXMin.Name = "labelXMin";
            this.labelXMin.Size = new System.Drawing.Size(36, 13);
            this.labelXMin.TabIndex = 0;
            this.labelXMin.Text = "X-min:";
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
            // groupScanResults
            // 
            this.groupScanResults.Controls.Add(this.labelZCenterValue);
            this.groupScanResults.Controls.Add(this.labelZAvgValue);
            this.groupScanResults.Controls.Add(this.labelZMaxValue);
            this.groupScanResults.Controls.Add(this.labelZCenter);
            this.groupScanResults.Controls.Add(this.labelZAvg);
            this.groupScanResults.Controls.Add(this.labelZMax);
            this.groupScanResults.Controls.Add(this.labelZMinValue);
            this.groupScanResults.Controls.Add(this.labelZMin);
            this.groupScanResults.Controls.Add(this.buttonResultToClipboard);
            this.groupScanResults.Location = new System.Drawing.Point(12, 236);
            this.groupScanResults.Name = "groupScanResults";
            this.groupScanResults.Size = new System.Drawing.Size(194, 174);
            this.groupScanResults.TabIndex = 2;
            this.groupScanResults.TabStop = false;
            this.groupScanResults.Text = "Scan results";
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
            this.Controls.Add(this.groupScanResults);
            this.Controls.Add(this.map);
            this.Controls.Add(this.groupScanArea);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BedHeightMap";
            this.Text = "Create height map";
            this.groupScanArea.ResumeLayout(false);
            this.groupScanArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.map)).EndInit();
            this.groupScanResults.ResumeLayout(false);
            this.groupScanResults.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupScanArea;
        private System.Windows.Forms.Button buttonResultToClipboard;
        private System.Windows.Forms.Button buttonMeasureHeights;
        private System.Windows.Forms.TextBox textYPoints;
        private System.Windows.Forms.Label labelYPoints;
        private System.Windows.Forms.TextBox textXPoints;
        private System.Windows.Forms.Label labelXPoints;
        private System.Windows.Forms.TextBox textYMax;
        private System.Windows.Forms.Label labelYMax;
        private System.Windows.Forms.TextBox textXMax;
        private System.Windows.Forms.Label labelXMax;
        private System.Windows.Forms.TextBox textYMin;
        private System.Windows.Forms.Label labelYMin;
        private System.Windows.Forms.TextBox textXMin;
        private System.Windows.Forms.Label labelXMin;
        private System.Windows.Forms.PictureBox map;
        private System.Windows.Forms.GroupBox groupScanResults;
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