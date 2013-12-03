/*
   Copyright 2013 Hot-World GmbH & Co. KG

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
   Code spend by Tomáš Chládek
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace RepetierHost.view.utils
{
    public class SlidePanelControler
    {
        private static SlidePanelControler instance;
        private List<SlidePanel> panels;
        private System.Timers.Timer timer;

        public static SlidePanelControler GetInstance()
        {
            if (instance == null)
                instance = new SlidePanelControler();
            return instance;
        }

        private SlidePanelControler()
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += timer_Tick;
            timer.Interval = 15;
            panels = new List<SlidePanel>();
        }

        public int GetPanelCount()
        {
            return panels.Count;
        }

        public bool IsRunning()
        {
            return timer.Enabled;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public SlidePanel Add(Control control, int direction, double speed, long delay, bool hideOnClick)
        {
            foreach (SlidePanel sl in panels)
                if (sl.panel == control)
                {
                    sl.time = 0;
                    if (sl.delay != delay)
                    {
                        sl.delay = delay;
                        return sl;
                    }
                    else if (sl.delay != -1)
                        sl.time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    return null;
                }
            if (hideOnClick)
                control.MouseClick += panel_MouseClick;
            SlidePanel slideLabel = new SlidePanel(control, direction, speed, delay);
            panels.Add(slideLabel);
            if (!timer.Enabled)
                timer.Enabled = true;
            timer.Start();
            return slideLabel;
        }

        public SlidePanel Add(Control control, int direction, double speed, bool hideOnClick)
        {
            return Add(control, direction, speed, -1, hideOnClick);
        }

        private void panel_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (SlidePanel obj in panels)
                if (obj.panel == (Control)sender)
                {
                    obj.show = false;
                    break;
                }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < panels.Count; i++)
                    if (panels[i].Update())
                        panels.RemoveAt(i);
                if (panels.Count == 0)
                    timer.Enabled = false;
            }
            catch
            {
            }
        }
    }
}
