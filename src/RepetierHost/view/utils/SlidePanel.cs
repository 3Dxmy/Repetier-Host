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
using System.Text;
using System.Windows.Forms;

namespace RepetierHost.view.utils
{

    public delegate void UpdateHandler(Control panel, double phase);

    public class SlidePanel {
        public const int DIRECTION_UP = 0;
        public const int DIRECTION_DOWN = 1;
        public const int DIRECTION_RIGHT = 2;

        protected internal Control panel;
        protected internal bool show;
        private double phase;
        private Point position;
        private int direction;
        protected internal long time;
        private double speed;
        protected internal long delay;

        public event UpdateHandler OnUpdate;

        public SlidePanel(Control panel, int direction, double speed, long delay) {
            this.panel = panel;
            this.direction = direction;
            this.speed = speed;
            this.delay = delay;
            time = 0;
            position = new Point(panel.Location.X, panel.Location.Y);
            show = true;
        }

        public bool Update() {
            if (time != 0 && show && delay != -1)
                if (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - delay > time)
                    show = false;

            if (show) {
                if (phase < 1) {
                    phase += speed;
                    Move();
                    if (OnUpdate != null)
                        OnUpdate(panel, phase);
                } else if (time == 0) {
                    Point loc;
                    switch (direction) {
                        case DIRECTION_DOWN:
                            loc = new Point(panel.Location.X, position.Y + panel.Size.Height);
                            break;
                        case DIRECTION_RIGHT:
                            loc = new Point(position.X + panel.Size.Width, panel.Location.Y);
                            break;
                        default:
                            loc = new Point(panel.Location.X, position.Y - panel.Size.Height);
                            break;
                    }
                    panel.Invoke((MethodInvoker)(() => panel.Location = loc));
                    time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    phase = 1.0;
                }
            } else {
                phase -= speed;
                if (phase > 0) {
                    if (OnUpdate != null)
                        OnUpdate(panel, phase);
                    Move();
                } else {
                    panel.Invoke((MethodInvoker)(() => panel.Location = position));
                    return true;
                }
            }
            return false;
        }

        private void Move() {
            Point loc;
            switch (direction) {
                case DIRECTION_DOWN:
                    loc = new Point(panel.Location.X, (int)(position.Y + panel.Size.Height * getPos(phase)));
                    break;
                case DIRECTION_RIGHT:
                    loc = new Point((int)(position.X + panel.Size.Width * getPos(phase)), panel.Location.Y);
                    break;
                default:
                    loc = new Point(panel.Location.X, (int)(position.Y - panel.Size.Height * getPos(phase)));
                    break;
            }
            panel.Invoke((MethodInvoker)(() => panel.Location = loc));
        }

        private double getPos(double phase) {
            phase -= 1;
            return -phase * phase + 1;
        }
    }
}
