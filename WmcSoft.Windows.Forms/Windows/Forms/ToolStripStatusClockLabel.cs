#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(ToolStripStatusClockLabel), "ToolStripStatusClockLabel.png")]
    [System.ComponentModel.DesignerCategory("Code")]
    public class ToolStripStatusClockLabel : ToolStripStatusLabel
    {
        Timer timer;

        public ToolStripStatusClockLabel() {
            timer = new Timer();
            timer.Interval = 500;
            timer.Tick += new EventHandler(timer_Tick);
            Text = DateTime.Now.ToString(dateTimeFormat);
            Paint += new PaintEventHandler(ToolStripStatusClockLabel_Paint);
        }

        void ToolStripStatusClockLabel_Paint(object sender, PaintEventArgs e) {
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e) {
            this.Text = DateTime.Now.ToString(dateTimeFormat);
        }

        [DefaultValue("g")]
        public string DateTimeFormat {
            get { return dateTimeFormat; }
            set { dateTimeFormat = value; }
        }
        string dateTimeFormat = "g";

        protected override void OnVisibleChanged(EventArgs e) {
            if (Visible) {
                Paint -= ToolStripStatusClockLabel_Paint;
                Paint += ToolStripStatusClockLabel_Paint;
            } else {
                timer.Stop();
            }
            base.OnVisibleChanged(e);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text {
            get { return base.Text; }
            set { base.Text = value; }
        }
    }
}
