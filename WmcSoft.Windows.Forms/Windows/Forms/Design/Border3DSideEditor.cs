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
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Resources;
using System.Drawing.Design;

namespace WmcSoft.Windows.Forms.Design
{
    /// <summary>
    /// 
    /// </summary>
    public class Border3DSideEditor : UITypeEditor
    {
        private class Border3DSideUI : Control
        {
            public Border3DSideUI(Border3DSideEditor editor) {
                this.editor = editor;
                this.InitializeComponent();
                this.tabOrder = new Border3DSideEditor.Border3DSideUI.SpringControl[5] { this.left, this.top, this.right, this.bottom, this.middle };
            }

            public void End() {
                this.edSvc = null;
                this.value = null;
            }

            public virtual Border3DSide GetSelectedBorder3DSide() {
                Border3DSide sides = Border3DSide.All;
                if (!this.left.GetSolid()) {
                    sides &= ~Border3DSide.Left;
                }
                if (!this.top.GetSolid()) {
                    sides &= ~Border3DSide.Top;
                }
                if (!this.bottom.GetSolid()) {
                    sides &= ~Border3DSide.Bottom;
                }
                if (!this.right.GetSolid()) {
                    sides &= ~Border3DSide.Right;
                }
                if (!this.middle.GetSolid()) {
                    sides &= ~Border3DSide.Middle;
                }
                //if (((int)sides) == 0) {
                //    sides = Border3DSide.All;
                //    this.left.SetSolid(true);
                //    this.top.SetSolid(true);
                //    this.bottom.SetSolid(true);
                //    this.right.SetSolid(true);
                //    this.middle.SetSolid(true);
                //}
                return sides;
            }

            void InitializeComponent() {

                int width = 2 * SystemInformation.Border3DSize.Width;
                int height = 2 * SystemInformation.Border3DSize.Height;

                this.left = new Border3DSideEditor.Border3DSideUI.SpringControl(this);
                this.right = new Border3DSideEditor.Border3DSideUI.SpringControl(this);
                this.top = new Border3DSideEditor.Border3DSideUI.SpringControl(this);
                this.bottom = new Border3DSideEditor.Border3DSideUI.SpringControl(this);
                this.middle = new Border3DSideEditor.Border3DSideUI.SpringControl(this);
                base.SetBounds(0, 0, 90, 90);
                //
                // middle
                //
                this.middle.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
                this.middle.Location = new Point(30, 30);
                this.middle.Size = new Size(30, 30);
                this.middle.TabIndex = 4;
                this.middle.TabStop = true;
                //
                // right
                //
                this.right.Anchor = AnchorStyles.Right;
                this.right.Location = new Point(80 - width, 10 + height);
                this.right.Size = new Size(10, 70 - 2 * height);
                this.right.TabIndex = 2;
                this.right.TabStop = true;
                //
                // left
                //
                this.left.Anchor = AnchorStyles.Left;
                this.left.Location = new Point(width, 10 + height);
                this.left.Size = new Size(10, 70 - 2 * height);
                this.left.TabIndex = 0;
                this.left.TabStop = true;
                //
                // top
                //
                this.top.Anchor = AnchorStyles.Top;
                this.top.Location = new Point(10 + width, height);
                this.top.Size = new Size(70 - 2 * width, 10);
                this.top.TabIndex = 1;
                this.top.TabStop = true;
                //
                // bottom
                //
                this.bottom.Anchor = AnchorStyles.Bottom;
                this.bottom.Location = new Point(10 + width, 80 - height);
                this.bottom.Size = new Size(70 - 2 * width, 10);
                this.bottom.TabIndex = 3;
                this.bottom.TabStop = true;

                this.BackColor = SystemColors.Window;
                this.ForeColor = SystemColors.WindowText;
                base.TabStop = false;
                //base.Controls.Add(this.container);
                base.Controls.AddRange(new Control[5] { this.top, this.left, this.bottom, this.right, this.middle });

                ResourceManager rm = new ResourceManager(typeof(Border3DSideEditor));
                this.middle.AccessibleName = rm.GetString("Border3DSide.Middle.AccessibleName");
                this.right.AccessibleName = rm.GetString("Border3DSide.Right.AccessibleName");
                this.left.AccessibleName = rm.GetString("Border3DSide.Left.AccessibleName");
                this.top.AccessibleName = rm.GetString("Border3DSide.Top.AccessibleName");
                this.bottom.AccessibleName = rm.GetString("Border3DSide.Bottom.AccessibleName");
                rm.ReleaseAllResources();
            }

            protected override void OnPaint(PaintEventArgs e) {
                Rectangle rectangle = base.ClientRectangle;
                ControlPaint.DrawBorder3D(e.Graphics, rectangle, Border3DStyle.Sunken);
            }

            protected override void OnGotFocus(EventArgs e) {
                base.OnGotFocus(e);
                this.top.Focus();
            }

            private void SetValue() {
                this.value = this.GetSelectedBorder3DSide();
            }

            public void Start(System.Windows.Forms.Design.IWindowsFormsEditorService edSvc, object value) {
                this.edSvc = edSvc;
                this.value = value;
                if (value is Border3DSide) {
                    this.left.SetSolid((((Border3DSide)value) & Border3DSide.Left) == Border3DSide.Left);
                    this.top.SetSolid((((Border3DSide)value) & Border3DSide.Top) == Border3DSide.Top);
                    this.bottom.SetSolid((((Border3DSide)value) & Border3DSide.Bottom) == Border3DSide.Bottom);
                    this.right.SetSolid((((Border3DSide)value) & Border3DSide.Right) == Border3DSide.Right);
                    this.middle.SetSolid((((Border3DSide)value) & Border3DSide.Middle) == Border3DSide.Middle);
                    this.oldBorders = (Border3DSide)value;
                } else {
                    this.oldBorders = Border3DSide.All;
                }
            }

            private void Teardown(bool save) {
                if (!save) {
                    this.value = this.oldBorders;
                }
                this.edSvc.CloseDropDown();
            }

            public object Value {
                get { return this.value; }
            }

            private SpringControl bottom;
            //private ContainerPlaceholder container;
            private Border3DSideEditor editor;
            private System.Windows.Forms.Design.IWindowsFormsEditorService edSvc;
            private SpringControl left;
            private Border3DSide oldBorders;
            private SpringControl right;
            private SpringControl top;
            private SpringControl middle;
            private SpringControl[] tabOrder;
            private object value;

            // Nested Types
            private class SpringControl : Control
            {
                // Methods
                public SpringControl(Border3DSideEditor.Border3DSideUI picker) {
                    if (picker == null) {
                        throw new ArgumentNullException("picker");
                    }
                    this.picker = picker;
                    base.TabStop = true;
                }

                protected override AccessibleObject CreateAccessibilityInstance() {
                    return new Border3DSideEditor.Border3DSideUI.SpringControl.SpringControlAccessibleObject(this);
                }

                public virtual bool GetSolid() {
                    return this.solid;
                }

                protected override void OnGotFocus(EventArgs e) {
                    if (!this.focused) {
                        this.focused = true;
                        base.Invalidate();
                    }
                    base.OnGotFocus(e);
                }

                protected override void OnLostFocus(EventArgs e) {
                    if (this.focused) {
                        this.focused = false;
                        base.Invalidate();
                    }
                    base.OnLostFocus(e);
                }

                protected override void OnMouseDown(MouseEventArgs e) {
                    this.SetSolid(!this.solid);
                    base.Focus();
                }

                protected override void OnPaint(PaintEventArgs e) {
                    Rectangle rectangle = base.ClientRectangle;
                    if (this.solid) {
                        e.Graphics.FillRectangle(SystemBrushes.ControlDark, rectangle);
                        e.Graphics.DrawRectangle(SystemPens.WindowFrame, rectangle.X, rectangle.Y, (int)(rectangle.Width - 1), (int)(rectangle.Height - 1));
                    } else {
                        ControlPaint.DrawFocusRectangle(e.Graphics, rectangle);
                    }
                    if (this.focused) {
                        rectangle.Inflate(-2, -2);
                        ControlPaint.DrawFocusRectangle(e.Graphics, rectangle);
                    }
                }

                protected override bool ProcessDialogChar(char charCode) {
                    if (charCode == ' ') {
                        this.SetSolid(!this.solid);
                        return true;
                    }
                    return base.ProcessDialogChar(charCode);
                }

                protected override bool ProcessDialogKey(Keys keyData) {
                    if (((keyData & Keys.KeyCode) == Keys.Return) && ((keyData & (Keys.Alt | Keys.Control)) == Keys.None)) {
                        this.picker.Teardown(true);
                        return true;
                    }
                    if (((keyData & Keys.KeyCode) == Keys.Escape) && ((keyData & (Keys.Alt | Keys.Control)) == Keys.None)) {
                        this.picker.Teardown(false);
                        return true;
                    }
                    if (((keyData & Keys.KeyCode) != Keys.Tab) || ((keyData & (Keys.Alt | Keys.Control)) != Keys.None)) {
                        return base.ProcessDialogKey(keyData);
                    }
                    for (int i = 0; i < this.picker.tabOrder.Length; ++i) {
                        if (this.picker.tabOrder[i] == this) {
                            i += (((keyData & Keys.Shift) == Keys.None) ? 1 : -1);
                            i = (i < 0) ? (i + this.picker.tabOrder.Length) : (i % this.picker.tabOrder.Length);
                            this.picker.tabOrder[i].Focus();
                            break;
                        }
                    }
                    return true;
                }

                public virtual void SetSolid(bool value) {
                    if (this.solid != value) {
                        this.solid = value;
                        this.picker.SetValue();
                        base.Invalidate();
                    }
                }

                // Fields
                internal bool focused;
                internal bool solid;
                private Border3DSideEditor.Border3DSideUI picker;

                // Nested Types
                private class SpringControlAccessibleObject : Control.ControlAccessibleObject
                {
                    // Methods
                    public SpringControlAccessibleObject(Border3DSideEditor.Border3DSideUI.SpringControl owner)
                        : base(owner) {
                    }

                    // Properties
                    public override AccessibleStates State {
                        get {
                            AccessibleStates states = base.State;
                            if (((Border3DSideEditor.Border3DSideUI.SpringControl)base.Owner).GetSolid()) {
                                states |= AccessibleStates.Selected;
                            }
                            return states;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Border3DSideEditor() {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
            if (provider != null) {
                var service = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));
                if (service == null) {
                    return value;
                }
                using (var ui = new Border3DSideEditor.Border3DSideUI(this)) {
                    ui.Start(service, value);
                    service.DropDownControl(ui);
                    value = ui.Value;
                    ui.End();
                }
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
        }

    }
}

