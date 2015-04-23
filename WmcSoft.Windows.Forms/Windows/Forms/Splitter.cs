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

using System.Drawing;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(System.Windows.Forms.Splitter))]
    [System.ComponentModel.DesignerCategory("Code")]
    public class Splitter : System.Windows.Forms.Splitter
    {
        #region Lifecycle

        public Splitter() {
            this.SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
            ResetBackColor();
        }

        #endregion

        #region Properties

        private bool ShouldSerializeBackColor() {
            return this.BackColor != Color.Transparent;
        }

        public override void ResetBackColor() {
            this.BackColor = Color.Transparent;
        }

        #endregion

        private System.Windows.Forms.Control FindPanel() {
            System.Windows.Forms.Control parent = this.Parent;
            if (parent != null) {
                System.Windows.Forms.Control.ControlCollection controls = parent.Controls;
                int count = controls.Count;
                System.Windows.Forms.DockStyle dock = this.Dock;
                for (int i = 0; i < count; i++) {
                    System.Windows.Forms.Control control = controls[i];
                    if (control != this) {
                        switch (dock) {
                        case System.Windows.Forms.DockStyle.Top:
                            if (control.Bottom == base.Top) {
                                return control;
                            }
                            break;
                        case System.Windows.Forms.DockStyle.Bottom:
                            if (control.Top == base.Bottom) {
                                return control;
                            }
                            break;
                        case System.Windows.Forms.DockStyle.Left:
                            if (control.Right == base.Left) {
                                return control;
                            }
                            break;
                        case System.Windows.Forms.DockStyle.Right:
                            if (control.Left == base.Right) {
                                return control;
                            }
                            break;
                        }
                    }
                }
            }
            return null;
        }
    }
}
