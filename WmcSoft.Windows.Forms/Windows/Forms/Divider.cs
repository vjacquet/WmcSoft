using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    [System.ComponentModel.DesignerCategory("code")]
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
