using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(System.Windows.Forms.Splitter), "Splitter.bmp")]
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
