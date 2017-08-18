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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class ToolStripStatusKeyLockLabel : ToolStripStatusLabel, IObserver<Keys>
    {
        #region KeyLockMessageFilter class

        class KeyLockMessageFilter : IMessageFilter
            , IObservable<Keys>
        {
            static readonly int[] _keys;
            static KeyLockMessageFilter() {
                _keys = new[] { (int)Keys.CapsLock, (int)Keys.NumLock, (int)Keys.Insert, (int)Keys.Scroll };
                Array.Sort(_keys);
            }

            List<IObserver<Keys>> _observers;

            public KeyLockMessageFilter() {
            }

            #region IMessageFilter Membres

            public bool PreFilterMessage(ref Message m) {
                if ((_observers != null) && (m.Msg == NativeMethods.WM_KEYDOWN || m.Msg == NativeMethods.WM_KEYUP)) {
                    int w = m.WParam.ToInt32();
                    if (Array.BinarySearch(_keys, w) >= 0) {
                        Keys key = (Keys)w;
                        foreach (var observer in _observers) {
                            observer.OnNext(key);
                        }
                    }
                }
                return false;
            }

            #endregion

            #region IObservable<Keys> Membres

            private void Unsubscribe(IObserver<Keys> observer) {
                if (_observers.Remove(observer) && _observers.Count == 0)
                    _observers = null;
            }

            public IDisposable Subscribe(IObserver<Keys> observer) {
                if (observer == null)
                    throw new ArgumentNullException();

                if (_observers == null)
                    _observers = new List<IObserver<Keys>>();

                _observers.Add(observer);
                return new Subscription(() => Unsubscribe(observer));
            }

            #endregion
        }

        #endregion

        static KeyLockMessageFilter messageFilter = new KeyLockMessageFilter();
        static int messageFilterRefCount = 0;
        IDisposable _unsubscriber;

        public ToolStripStatusKeyLockLabel() {
            AddFilter();
            InitializeKeyLock();
        }

        #region Properties & Associated events

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ReadOnly(true)]
        public override string Text {
            get { return base.Text; }
            set { throw new InvalidOperationException(); }
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
            add { this.Events.AddHandler(KeyLockChangedEvent, value); }
            remove { this.Events.RemoveHandler(KeyLockChangedEvent, value); }
        }
        static object KeyLockChangedEvent = new object();

        protected virtual void OnKeyLockChanged(EventArgs e) {
            RefreshDisplay();

            EventHandler handler = (EventHandler)this.Events[KeyLockChangedEvent];
            if (handler != null) {
                handler(this, e);
            }
        }

        #endregion

        #region Private members

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
        }
        string text;

        private void RefreshDisplay() {
            if (this.DesignMode) {
                base.Text = text;
            } else if (Control.IsKeyLocked((Keys)keyLock)) {
                base.Text = text;
            } else {
                base.Text = String.Empty;
            }
        }

        public override ISite Site {
            get {
                return base.Site;
            }
            set {
                base.Site = value;
                RefreshDisplay();
            }
        }

        #endregion

        #region Overridables

        public override Size GetPreferredSize(Size constrainingSize) {
            var size = base.GetPreferredSize(constrainingSize);
            size.Width = Math.Max(40, TextRenderer.MeasureText(text, Font).Width + Padding.Horizontal);
            return size;
        }

        protected override void Dispose(bool disposing) {
            RemoveFilter();
            base.Dispose(disposing);
        }

        #endregion

        #region Event filtering

        private void AddFilter() {
            _unsubscriber = messageFilter.Subscribe(this);
            if (System.Threading.Interlocked.Increment(ref messageFilterRefCount) == 1) {
                Application.AddMessageFilter(messageFilter);
            }
        }

        private void RemoveFilter() {
            if (System.Threading.Interlocked.Decrement(ref messageFilterRefCount) == 0) {
                Application.RemoveMessageFilter(messageFilter);
            }
            _unsubscriber.Dispose();
        }

        #endregion

        #region IObserver<Keys> Membres

        void IObserver<Keys>.OnCompleted() {
        }

        void IObserver<Keys>.OnError(Exception error) {
        }

        void IObserver<Keys>.OnNext(Keys value) {
            if ((int)value == (int)this.KeyLock) {
                RefreshDisplay();
            }
        }

        #endregion
    }

    public enum KeyLock
    {
        CapsLock = Keys.CapsLock,
        NumLock = Keys.NumLock,
        Insert = Keys.Insert,
        ScrollLock = Keys.Scroll,
    }
}
