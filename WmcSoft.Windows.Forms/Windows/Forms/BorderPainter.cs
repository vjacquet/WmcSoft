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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    [DefaultProperty("Border3DStyle")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(BorderPainter), "BorderPainter.png")]
    [ProvideProperty("Border3DSide", typeof(Control))]
    public partial class BorderPainter : Component, IExtenderProvider
    {
        public BorderPainter() {
        }

        public BorderPainter(IContainer container) {
            container.Add(this);
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(System.Windows.Forms.Border3DStyle.Etched)]
        public System.Windows.Forms.Border3DStyle Border3DStyle {
            get {
                return border3DStyle;
            }
            set {
                if (border3DStyle != value) {
                    border3DStyle = value;
                    InvalidatePainting();
                }
            }
        }
        System.Windows.Forms.Border3DStyle border3DStyle = System.Windows.Forms.Border3DStyle.Etched;

        Dictionary<Control, Border3DSide> border3DSides = new Dictionary<Control, Border3DSide>();

        public Color FlatBorderColor {
            get {
                return flatBorderColor;
            }
            set {
                if (flatBorderColor != value) {
                    flatBorderColor = value;
                    InvalidatePainting();
                }
            }
        }
        Color flatBorderColor = System.Drawing.SystemColors.ControlDark;

        public virtual void ResetFlatColor() {
            flatBorderColor = System.Drawing.SystemColors.ControlDark;
        }

        public virtual bool ShouldSerializeFlatColor() {
            return flatBorderColor != System.Drawing.SystemColors.ControlDark;
        }

        [DefaultValue(1)]
        public int FlatBorderWidth {
            get {
                return _flatBorderWidth;
            }
            set {
                if (_flatBorderWidth != value) {
                    _flatBorderWidth = value;
                    InvalidatePainting();
                }
            }
        }
        int _flatBorderWidth = 1;

        [DefaultValue(false)]
        public bool PaintOnParent {
            get {
                return _paintOnParent;
            }
            set {
                if (_paintOnParent != value) {
                    _paintOnParent = value;
                    InvalidatePainting();
                }
            }
        }
        bool _paintOnParent;

        private void InvalidatePainting() {
            if (_paintOnParent) {
                foreach (Control control in border3DSides.Keys) {
                    Control parent = control.Parent;
                    if (parent != null)
                        control.Parent.Invalidate();
                }
            } else {
                foreach (Control control in border3DSides.Keys) {
                    control.Invalidate();
                }
            }
        }

        #region IExtenderProvider Membres

        [DebuggerStepThrough]
        bool IExtenderProvider.CanExtend(object extendee) {
            return extendee is Control && !(extendee is Divider);
        }

        [Editor(typeof(Design.Border3DSideEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(null)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public Border3DSide? GetBorder3DSide(Control control) {
            Border3DSide border3DSide;
            if (border3DSides.TryGetValue(control, out border3DSide)) {
                return border3DSide;
            }
            return null;
        }

        public void SetBorder3DSide(Control control, Border3DSide? border3DSide) {
            if (border3DSide == null || (((int)border3DSide.Value) == 0)) {
                border3DSides.Remove(control);

                control.Paint -= control_Paint;
                control.Invalidate();
            } else {
                border3DSides[control] = border3DSide.Value;

                control.Paint -= control_Paint;
                control.Paint += control_Paint;
                control.Invalidate();
            }
        }

        #endregion

        private void control_Paint(object sender, PaintEventArgs e) {
            Control control = sender as Control;
            if (control != null) {
                Border3DSide border3DSide;
                if (border3DSides.TryGetValue(control, out border3DSide)) {
                    if (border3DStyle == Border3DStyle.Flat) {
                        ButtonBorderStyle leftStyle = ButtonBorderStyle.None;
                        ButtonBorderStyle topStyle = ButtonBorderStyle.None;
                        ButtonBorderStyle rightStyle = ButtonBorderStyle.None;
                        ButtonBorderStyle bottomStyle = ButtonBorderStyle.None;

                        if ((border3DSide & Border3DSide.Left) == Border3DSide.Left)
                            leftStyle = ButtonBorderStyle.Solid;
                        if ((border3DSide & Border3DSide.Top) == Border3DSide.Top)
                            topStyle = ButtonBorderStyle.Solid;
                        if ((border3DSide & Border3DSide.Right) == Border3DSide.Right)
                            rightStyle = ButtonBorderStyle.Solid;
                        if ((border3DSide & Border3DSide.Bottom) == Border3DSide.Bottom)
                            bottomStyle = ButtonBorderStyle.Solid;

                        ControlPaint.DrawBorder(e.Graphics,
                            control.ClientRectangle,
                            flatBorderColor, _flatBorderWidth, leftStyle,
                            flatBorderColor, _flatBorderWidth, topStyle,
                            flatBorderColor, _flatBorderWidth, rightStyle,
                            flatBorderColor, _flatBorderWidth, bottomStyle);
                    } else {
                        ControlPaint.DrawBorder3D(e.Graphics,
                            control.ClientRectangle,
                            border3DStyle,
                            border3DSide);
                    }
                }
            }
        }

    }
}
