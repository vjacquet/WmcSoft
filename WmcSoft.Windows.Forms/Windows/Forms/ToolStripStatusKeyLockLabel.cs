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
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    public class ToolStripStatusKeyLockLabel : ToolStripStatusLabel
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern short GetKeyState(int keyCode);

        class KeyLockMessageFilter : IMessageFilter
        {
            public KeyLockMessageFilter() {
                LockChanged += KeyLockMessageFilter_LockChanged;
            }

            void KeyLockMessageFilter_LockChanged(object sender, EventArgs e) {
                // nothing to do... it just avoid the if statement
            }

            #region IMessageFilter Membres

            public bool PreFilterMessage(ref Message m) {
                if (m.Msg == NativeMethods.WM_KEYDOWN || m.Msg == NativeMethods.WM_KEYUP) {
                    int w = m.WParam.ToInt32();
                    if (w == (int)Keys.CapsLock
                        || w == (int)Keys.NumLock
                        || w == (int)Keys.Insert
                        || w == (int)Keys.Scroll) {
                        OnLockChanged();
                    }
                }
                return false;
            }

            public event EventHandler LockChanged;

            private void OnLockChanged() {
                LockChanged(this, EventArgs.Empty);
            }

            #endregion
        }
        static KeyLockMessageFilter messageFilter = new KeyLockMessageFilter();
        static int messageFilterRefCount = 0;

        public ToolStripStatusKeyLockLabel() {
            AddFilter();
            Text = "";
            InitializeKeyLock();
        }

        [DefaultValue(KeyLock.CapsLock)]
        public KeyLock KeyLock {
            get {
                return keyLock;
            }
            set {
                if (keyLock != value) {
                    keyLock = value;
                    InitializeKeyLock();
                    OnKeyLockChanged(EventArgs.Empty);
                }
            }
        }
        KeyLock keyLock = KeyLock.CapsLock;

        public event EventHandler KeyLockChanged {
            add { Events.AddHandler(KeyLockChangedEvent, value); }
            remove { Events.RemoveHandler(KeyLockChangedEvent, value); }
        }
        static object KeyLockChangedEvent = new object();

        protected virtual void OnKeyLockChanged(EventArgs e) {
            EventHandler handler = (EventHandler)Events[KeyLockChangedEvent];
            if (handler != null) {
                handler(this, e);
            }
        }

        private void InitializeKeyLock() {
            switch (keyLock) {
            case KeyLock.CapsLock:
                text = "CAPS";
                break;
            case KeyLock.NumLock:
                text = "NUM";
                break;
            case KeyLock.ScrollLock:
                text = "SCRL";
                break;
            case KeyLock.Insert:
                text = "INS";
                break;
            }
            messageFilter_LockChanged(null, EventArgs.Empty);
        }
        string text;

        public override System.Drawing.Size GetPreferredSize(Size constrainingSize) {
            var size = base.GetPreferredSize(constrainingSize);
            size.Width = TextRenderer.MeasureText(text, Font).Width + Padding.Left + Padding.Right;
            size.Width = Math.Min(size.Width, 40);
            return size;
        }

        protected override void Dispose(bool disposing) {
            RemoveFilter();
            base.Dispose(disposing);
        }

        private void AddFilter() {
            if (Interlocked.Increment(ref messageFilterRefCount) == 1) {
                Application.AddMessageFilter(messageFilter);
            }
            messageFilter.LockChanged += messageFilter_LockChanged;
        }

        private void RemoveFilter() {
            if (Interlocked.Decrement(ref messageFilterRefCount) == 0) {
                Application.RemoveMessageFilter(messageFilter);
            }
            messageFilter.LockChanged -= messageFilter_LockChanged;
        }

        void messageFilter_LockChanged(object sender, EventArgs e) {
            //if (Control.IsKeyLocked((Keys)keyLock)) {
            if ((GetKeyState((int)keyLock) & 1) != 0) {
                Text = text;
            } else {
                Text = String.Empty;
            }
        }
    }

    public enum KeyLock
    {
        CapsLock = Keys.CapsLock,
        NumLock = Keys.NumLock,
        Insert = Keys.Insert,
        ScrollLock = Keys.Scroll,
    }
}
