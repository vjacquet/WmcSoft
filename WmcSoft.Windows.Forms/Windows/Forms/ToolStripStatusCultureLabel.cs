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
using System.Globalization;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    public class ToolStripStatusCultureLabel : ToolStripStatusLabel
    {
        public ToolStripStatusCultureLabel()
            : base() {
        }

        public override Size GetPreferredSize(Size constrainingSize) {
            return new Size(16, 16);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public override string Text {
            get { return base.Text; }
            set { base.Text = value; }
        }

        protected override void OnPaint(PaintEventArgs e) {
            if (Owner == null)
                return;

            var cultureInfo = CultureInfo.CurrentUICulture;
            string culture = cultureInfo.TwoLetterISOLanguageName.ToUpper();
            if (!DesignMode && String.IsNullOrEmpty(ToolTipText)) {
                ToolTipText = cultureInfo.DisplayName;
            }
            using (var backgound = new SolidBrush(SystemColors.ActiveCaption))
            using (var foregound = new SolidBrush(SystemColors.ActiveCaptionText))
            using (var font = new Font(SystemInformation.MenuFont, FontStyle.Regular)) {
                e.Graphics.FillRectangle(backgound, 0, 0, 16, 16);
                e.Graphics.DrawString(culture, font, foregound, -1, 1);
            }
        }
    }
}
