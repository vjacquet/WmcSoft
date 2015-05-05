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
using System.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{

    [DesignerCategory("Component")]
    [ToolboxItem(false)]
    public class DropDownHolder : Form
    {
        #region Interop

        const int GWL_HWNDPARENT = -8;
        const int WM_ACTIVATE = 6;
        const int WS_EX_TOOLWINDOW = 0x80;
        const int WS_EX_APPWINDOW = 0x40000;
        const int WS_EX_WINDOWEDGE = 0x00000100;
        const int WS_EX_CLIENTEDGE = 0x00000200;
        const int WS_EX_STATICEDGE = 0x00020000;
        const int WS_POPUP = unchecked((int)0x80000000);
        const int WS_BORDER = unchecked((int)0x00800000);
        const int WS_DLGFRAME = 0x00400000;
        const int WS_THICKFRAME = 0x00040000;
        const int WS_MINIMIZEBOX = 0x00020000;
        const int WS_MAXIMIZEBOX = 0x00010000;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern int MsgWaitForMultipleObjects(int nCount, IntPtr pHandles, bool fWaitAll, int dwMilliseconds, int dwWakeMask);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        static extern IntPtr GetWindowLong32(HandleRef hWnd, int nIndex);
        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
        static extern IntPtr GetWindowLongPtr64(HandleRef hWnd, int nIndex);

        static IntPtr GetWindowLong(HandleRef hWnd, int nIndex) {
            if (IntPtr.Size == 4) {
                return GetWindowLong32(hWnd, nIndex);
            }
            return GetWindowLongPtr64(hWnd, nIndex);
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

        static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, HandleRef dwNewLong) {
            if (IntPtr.Size == 4) {
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            }
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }

        #endregion

        public DropDownHolder(Control parent) {
            this.parent = parent;
            //base.ShowInTaskbar = false;
            //base.ControlBox = false;
            //base.MinimizeBox = false;
            //base.MaximizeBox = false;
            //base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Text = "";
            base.StartPosition = FormStartPosition.Manual;
            this.Font = parent.Font;
            base.Visible = false;
            this.BackColor = SystemColors.Window;
        }

        public void DoModalLoop() {
            while (base.Visible) {
                Application.DoEvents();
                MsgWaitForMultipleObjects(0, IntPtr.Zero, true, 250, 0xff);
            }
        }

        public virtual void FocusControl() {
            if ((this.control != null) && base.Visible) {
                this.control.Focus();
            }
        }

        private void OnControlResize(object o, EventArgs e) {
            if (this.control != null) {
                int num1 = base.Width;
                this.UpdateSize();
                this.control.Location = new Point(1, 1);
                base.Left -= base.Width - num1;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                base.Visible = false;
            }
            base.OnMouseDown(e);
        }

        private bool OwnsWindow(IntPtr hWnd) {
            while (hWnd != IntPtr.Zero) {
                hWnd = GetWindowLong(new HandleRef(null, hWnd), GWL_HWNDPARENT);
                if (hWnd == IntPtr.Zero) {
                    return false;
                }
                if (hWnd == base.Handle) {
                    return true;
                }
            }
            return false;
        }

        protected override bool ProcessDialogKey(Keys keyData) {
            if ((keyData & (Keys.Alt | Keys.Control | Keys.Shift)) == Keys.None) {
                Keys keys = keyData & Keys.KeyCode;
                if (keys != Keys.Return) {
                    if (keys != Keys.Escape) {
                        if (keys == Keys.F4) {
                            return true;
                        }
                        goto DefaultHandler;
                    }
                    base.Visible = false;
                }
                return true;
            }
        DefaultHandler:
            return base.ProcessDialogKey(keyData);
        }

        public virtual void SetControl(Control ctl) {
            if (this.control != null) {
                base.Controls.Remove(this.control);
                this.control = null;
            }
            if (ctl != null) {
                base.Controls.Add(ctl);
                ctl.Location = new Point(1, 1);
                ctl.Visible = true;
                this.control = ctl;
                this.UpdateSize();
                this.control.Resize += new EventHandler(this.OnControlResize);
            }
            base.Enabled = this.control != null;
        }

        private void UpdateSize() {
            base.Size = new Size((2 + this.control.Width) + 2, (2 + this.control.Height) + 2);
        }

        protected override void WndProc(ref Message m) {
            if (((m.Msg == WM_ACTIVATE) && base.Visible) && ((((int)m.WParam & 0xffff) == 0) && !this.OwnsWindow(m.LParam))) {
                base.Visible = false;
            } else {
                base.WndProc(ref m);
            }
        }

        protected override System.Windows.Forms.CreateParams CreateParams {
            get {
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= WS_EX_TOOLWINDOW/* | WS_EX_STATICEDGE*/;
                createParams.ExStyle &= ~(WS_EX_APPWINDOW);
                createParams.Style |= WS_POPUP | WS_BORDER;
                createParams.Style &= ~(WS_THICKFRAME | WS_MAXIMIZEBOX | WS_MINIMIZEBOX | WS_DLGFRAME);
                if (this.parent != null) {
                    createParams.Parent = this.parent.Handle;
                }
                return createParams;
            }
        }

        public void Lock() {
            SetWindowLong(new HandleRef(this, this.Handle), GWL_HWNDPARENT, new HandleRef(this, this.parent.Handle));
        }

        public void Unlock() {
            SetWindowLong(new HandleRef(this, this.Handle), GWL_HWNDPARENT, new HandleRef(null, IntPtr.Zero));
        }

        private Control control;
        private Control parent;

    }
}


#if UNDEFINED

// TODO: Move the native function in the native class
// replace the DoMOdal by the MessageFilter

namespace Devcorp.Controls.VisualStyles.Sample
{
	/// <summary>
	/// A Message Loop filter which detect mouse events while the popup form is shown 
	/// and notifies the owner class when a mouse click outside the popup occurs.
	/// http://www.vbaccelerator.com/home/NET/Code/Controls/Popup_Windows/Popup_Windows/Popup_Form_Demonstration.asp
	/// </summary>
	internal class DropDownMessageFilter : IMessageFilter
	{
		/// <summary>
		/// 
		/// </summary>
		public event DropDownCancelEventHandler DropDownCancel;

		#region Fields
		private Form					dropDownForm	= null;
		private DropDownWindowHelper	ownerHelper		= null;

		#endregion

		#region Accessors
		/// <summary>
		/// Gets or sets the dropdown form which is being displayed.
		/// </summary>
		public Form DropDown
		{
			get
			{ 
				return this.dropDownForm;
			}
			set
			{ 
				this.dropDownForm = value;
			}
		}

		#endregion

		#region Constructor(s)
		/// <summary>
		/// Creates a new instance <c>DropDownMessageFilter</c>.
		/// </summary>
		/// <param name="helper">The <see cref="DropDownWindowHelper"/> object which owns this class.</param>
		public DropDownMessageFilter(DropDownWindowHelper helper)
		{
			this.ownerHelper = helper;
		}

		#endregion
		
		#region Methods
		private void OnMouseDown()
		{
			Point cursorPos = Cursor.Position;

			if(!dropDownForm.Bounds.Contains(cursorPos))
			{
				OnDropDownCancel(new DropDownCancelEventArgs(this.dropDownForm, cursorPos));
			}
		}

		protected virtual void OnDropDownCancel(DropDownCancelEventArgs e)
		{
			if(DropDownCancel != null) DropDownCancel(this, e);

			if(!e.Cancel)
			{
				ownerHelper.CloseDropDown();
				dropDownForm = null;
			}
		}

		/// <summary>
		/// Checks the message loop for mouse messages whilst the popup window is displayed.
		/// If one is detected the position is checked to see if it is outside the form, and the owner is notified if so.
		/// </summary>
		/// <param name="m">Windows Message about to be processed by the message loop</param>
		/// <returns><c>true</c> to filter the message, <c>false</c> otherwise. This implementation always returns <c>false</c>.</returns>
		public bool PreFilterMessage(ref Message m)
		{
			if(dropDownForm != null)
			{
				switch(m.Msg)
				{
					case (int)Messages.WM_LBUTTONDOWN:
					case (int)Messages.WM_RBUTTONDOWN:
					case (int)Messages.WM_MBUTTONDOWN:
					case (int)Messages.WM_NCLBUTTONDOWN:
					case (int)Messages.WM_NCRBUTTONDOWN:
					case (int)Messages.WM_NCMBUTTONDOWN:
						OnMouseDown();
						break;
				}
			}

			return false;
		}

		#endregion
	}
}


#endif