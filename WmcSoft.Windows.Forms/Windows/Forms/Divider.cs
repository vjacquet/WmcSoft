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

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    /// <summary>
    ///   Control that draws a divider.
    /// </summary>
    [DesignerCategory("code")]
    public partial class Divider : Control
    {
        public Divider() {
        }

        protected override Size DefaultSize {
            get { return new Size(100, 2); }
        }

        protected override void OnPaint(PaintEventArgs pe) {
            var rect = ClientRectangle;
            if (_orientation == Orientation.Horizontal) {
                var line = new Rectangle(0, rect.Height / 2, rect.Width, 2);
                ControlPaint.DrawBorder3D(pe.Graphics, line, Border3DStyle.Etched, Border3DSide.Top);
            } else {
                var line = new Rectangle(rect.Width / 2, 0, 2, rect.Height);
                ControlPaint.DrawBorder3D(pe.Graphics, line, Border3DStyle.Etched, Border3DSide.Left);
            }
            base.OnPaint(pe);
        }

        [DefaultValue(Orientation.Horizontal)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public Orientation Orientation {
            get {
                return _orientation;
            }
            set {
                if (!value.IsValid(0, 1)) {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(Orientation));
                }
                if (_orientation != value) {
                    _orientation = value;
                    Invalidate();
                }
            }
        }
        Orientation _orientation = Orientation.Horizontal;
    }
}
