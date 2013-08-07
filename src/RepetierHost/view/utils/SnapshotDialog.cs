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

namespace RepetierHost.view.utils
{
    public partial class SnapshotDialog : Form
    {
        public SnapshotDialog()
        {
            InitializeComponent();

            Main.main.languageChanged += translate;
            translate();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Main.main.languageChanged -= translate;
            base.OnClosing(e);
        }

        private void translate()
        {
            this.Text = Trans.T("W_SNAPSHOT_DIALOG");
            descriptionLabel.Text = Trans.T("L_SNAPSHOT_DIALOG_DESCRIPTION");
            buttonForceSnapshot.Text = Trans.T("B_FORCE_SNAPSHOT");
            buttonCancel.Text = Trans.T("B_CANCEL");
        }

        private void buttonForceSnapshot_Click(object sender, EventArgs e)
        {
            Main.main.OnReadyToSaveStateCallback();
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Main.main.CancelSaveState();
            this.Close();
        }
    }
}
