using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RepetierHost.view;
using RepetierHost.model;
using RepetierHost.model.geom;

namespace RepetierHost.view.calibration
{
    public partial class BedHeightMap : Form
    {
        static BedHeightMap form = null;
        RHVector3[] points = null;
        int nx, ny,n,missing;
        double dx,dy;

        double minx, miny, maxx, maxy;
        double zmin, zmax, zavg,zcenter;
        Bitmap bmap;

        public static void Execute()
        {
            if (form == null)
                form = new BedHeightMap();
            form.Show(null);
            form.BringToFront();
        }
        public BedHeightMap()
        {
            InitializeComponent();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void buttonMeasureHeights_Click(object sender, EventArgs e)
        {
            minx = double.Parse(textXMin.Text,GCode.format);
            maxx = double.Parse(textXMax.Text, GCode.format);
            miny = double.Parse(textYMin.Text, GCode.format);
            maxy = double.Parse(textYMax.Text, GCode.format);
            nx = int.Parse(textXPoints.Text);
            ny = int.Parse(textYPoints.Text);
            dx = (maxx-minx)/(double)(nx-1);
            dy = (maxy-miny)/(double)(ny-1);
            n = nx * ny;
            points = new RHVector3[n];
            int x, y;
            for (y = 0; y < ny; y++)
            {
                for (x = 0; x < nx; x++)
                {
                    points[y * nx + x] = new RHVector3(minx + x * dx, miny + y * dy, 0);
                }
            }
            missing = n;
            Main.conn.eventResponse += Answer;
            buttonResultToClipboard.Enabled = false;
            for (int i = 0; i < n; i++)
            {
                RHVector3 act = points[i];
                Main.conn.injectManualCommand("G1 X" + act.x.ToString("0.00", GCode.format) + " Y" + act.y.ToString("0.00", GCode.format) + " F" + Main.conn.travelFeedRate.ToString(GCode.format));
                Main.conn.injectManualCommand("G30");
            }
        }
        public RHVector3 findNearest(double x, double y)
        {
            RHVector3 best = null;
            RHVector3 pos = new RHVector3(x,y,0);
            double bestdist = 1e20;
            for(int i=0;i<n;i++) {
                RHVector3 act = points[i];
                double dist = act.Distance(pos);
                if (dist < bestdist)
                {
                    best = act;
                    bestdist = dist;
                }
            }
            return best;
        }
        public MethodInvoker EnableButton = delegate
        {
            form.labelZMinValue.Text = form.zmin.ToString("0.00");
            form.labelZMaxValue.Text = form.zmax.ToString("0.00");
            form.labelZAvgValue.Text = form.zavg.ToString("0.00");
            form.labelZCenterValue.Text = form.zcenter.ToString("0.00");
            form.buttonResultToClipboard.Enabled = true;
            form.UpdateMap();
        };
        public void Answer(string text)
        {
            string sz = Main.conn.extract(text, "Z-probe:");
            if (sz == null) return;
            string sx = Main.conn.extract(text, "X:");
            string sy = Main.conn.extract(text, "Y:");
            if (sx == null || sy == null) return;
            double x = double.Parse(sx, GCode.format);
            double y = double.Parse(sy, GCode.format);
            RHVector3 pnt = findNearest(x, y);
            pnt.z = double.Parse(sz, GCode.format);
            missing--;
            if (missing <= 0)
            {
                Main.conn.eventResponse -= Answer;
                zmin = 1e20;
                zmax = -1e20;
                zavg = 0;
                for (int i = 0; i < n; i++)
                {
                    RHVector3 act = points[i];
                    zavg += act.z;
                    zmin = Math.Min(zmin, act.z);
                    zmax = Math.Max(zmax, act.z);
                }
                zavg /= (double)n;
                zcenter = (zmin + zmax) / 2.0;
                Main.main.Invoke(EnableButton);
            }
        }
        public double interpolatedZ(double x, double y)
        {
            double px = (x - minx) / dx;
            double py = (y - miny) / dy;
            if (px < 0) px = 0;
            if (py < 0) py = 0;
            if (px > nx-1) px = nx-1;
            if (py > ny - 1) py = ny - 1;
            int px1 = (int)Math.Floor(px);
            int px2 = (int)Math.Ceiling(px);
            int py1 = (int)Math.Floor(py);
            int py2 = (int)Math.Ceiling(py);
            RHVector3 r11 = points[py1*nx+px1];
            RHVector3 r12 = points[py1*nx+px2];
            RHVector3 r21 = points[py2*nx+px1];
            RHVector3 r22 = points[py2*nx+px2];
            double sx = (x - r11.x) / dx;
            double sy = (y - r11.y) / dy;
            return r11.z * (1.0 - sx) * (1.0 - sy) + r12.z * sx * (1.0 - sy) + r21.z * (1.0 - sx) * sy + r22.z * sx * sy;
        }
        public Color colorForZ(double z)
        {
            int r,g,b;
            double dz = zmax-zcenter;
            r = Math.Min(z > zcenter ? (int)(255.0 * ((z - zcenter))/dz) : 0,255);
            b = z < zcenter ? Math.Min((int)(255.0 * ((zcenter-z)) / dz),255) : 0;
            g = Math.Max(0,Math.Min((int)(255.0 * (dz-Math.Abs(z - zcenter)) / dz), 255));
            return Color.FromArgb(r, g, b);
        }
        public void UpdateMap()
        {
            bmap = new Bitmap(nx*10,ny*10,PixelFormat.Format32bppArgb);
            for (int x = 0; x < nx*10; x++)
            {
                for (int y = 0; y < ny*10; y++)
                {
                    bmap.SetPixel(x, y, colorForZ(interpolatedZ(minx+(double)x*dx*0.1,maxy-(double)y*dy*0.1)));
                }
            }
            map.Image = bmap;
        }

        private void buttonResultToClipboard_Click(object sender, EventArgs e)
        {
            StringBuilder s = new StringBuilder();
            s.Append("X:\t");
            for (int x = 0; x < nx; x++)
            {
                s.Append((minx+x*dx).ToString("0.00"));
                if (x < nx - 1)
                    s.Append("\t");
                else
                    s.AppendLine();
            }
            for (int y = ny-1; y >=0 ; y--)
            {
                s.Append("y:"+(miny+y*dy).ToString("0.00")+"\t");
                for (int x = 0; x < nx; x++)
                {
                    RHVector3 act = points[y * nx + x];
                    s.Append(act.z.ToString("0.00"));
                    if (x < nx - 1)
                        s.Append("\t");
                    else
                        s.AppendLine();
                }
            }
            Clipboard.SetText(s.ToString());
        }
    }
}
