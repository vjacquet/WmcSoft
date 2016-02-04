#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
using System.Drawing;
using WmcSoft.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace WmcSoft.Windows.Forms.Layout
{
    internal class HorizontalFormLayoutEngine : LayoutEngine
    {
        internal static readonly HorizontalFormLayoutEngine Instance = new HorizontalFormLayoutEngine();

        public override bool Layout(object container, LayoutEventArgs layoutEventArgs) {
            var panel = container as HorizontalFormLayoutPanel;
            if (panel != null) {
                var spring = (AnchorStyles.Left | AnchorStyles.Right);
                var settings = panel.LayoutSettings;
                var maxSize = settings.MeasureLabels(panel.Font);
                var labelWidth = Math.Max(maxSize.Width, panel.MinLabelWidth) + panel.Padding.Left;
                var displayRectangle = panel.ClientRectangle.PadWith(panel.Padding);

                var displaySize = new Size(displayRectangle.Width - labelWidth, displayRectangle.Height);
                var location = new Point(displayRectangle.Location.X + labelWidth, displayRectangle.Y);

                var height = -panel.Padding.Bottom - 2; // -2 has been found experimentaly !
                foreach (var control in panel.Controls.Where(c => c.Visible)) {
#pragma warning disable CS0642 // Possible mistaken empty statement

                    var size = control.Size;
                    if ((control.Anchor & spring) == spring || control is TextBoxBase)
                        ; // NOOP
                    else if (control.AutoSize)
                        size = control.GetPreferredSize(displaySize);
                    height += size.Height + panel.Padding.Bottom;

#pragma warning restore CS0642 // Possible mistaken empty statement
                }

                if ((height > displayRectangle.Height) && !panel.VerticalScroll.Visible)
                    displaySize.Width -= SystemInformation.VerticalScrollBarWidth;

                foreach (var control in panel.Controls.Where(c => c.Visible)) {
                    control.Location = location;
                    if ((control.Anchor & spring) == spring || control is TextBoxBase)
                        control.Width = displaySize.Width;
                    else if (control.AutoSize)
                        control.Size = control.GetPreferredSize(displaySize);
                    location.Y = location.Y + control.Size.Height + panel.Padding.Bottom;
                }
            }
            return false;
        }
    }
}
