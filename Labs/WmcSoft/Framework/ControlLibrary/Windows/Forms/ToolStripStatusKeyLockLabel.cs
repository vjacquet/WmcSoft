using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    public class ToolStripStatusKeyLockLabel : ToolStripStatusLabel, IObserver<Keys>
    {
        class KeyLockMessageFilter : IMessageFilter
            , IObservable<Keys>
        {

            List<IObserver<Keys>> observers;

            public KeyLockMessageFilter() {
            }

            #region IMessageFilter Membres

            public bool PreFilterMessage(ref Message m) {
                if ((observers != null)
                    && (m.Msg == NativeMethods.WM_KEYDOWN || m.Msg == NativeMethods.WM_KEYUP)) {
                    int w = m.WParam.ToInt32();
                    if (w == (int)Keys.CapsLock
                        || w == (int)Keys.NumLock
                        || w == (int)Keys.Insert
                        || w == (int)Keys.Scroll) {
                        Keys key = (Keys)w;

                        foreach (var observer in observers) {
                            observer.OnNext(key);
                        }
                    }
                }
                return false;
            }

            #endregion

            #region IObservable<Keys> Membres

            public IDisposable Subscribe(IObserver<Keys> observer) {
                if (observer == null)
                    throw new ArgumentNullException();

                if (observers == null)
                    observers = new List<IObserver<Keys>>();

                observers.Add(observer);
                return new Unsubscriber(() => observers.Remove(observer));
            }

            #endregion
        }
        static KeyLockMessageFilter messageFilter = new KeyLockMessageFilter();
        static int messageFilterRefCount = 0;
        IDisposable unsubscriber;

        public ToolStripStatusKeyLockLabel() {
            AddFilter();
            InitializeKeyLock();
        }

        #region Properties & Associated events

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ReadOnly(true)]
        public override string Text {
            get {
                return base.Text;
            }
            set {
                throw new InvalidOperationException();
            }
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

        public override Size GetPreferredSize(System.Drawing.Size constrainingSize) {
            Size size = base.GetPreferredSize(constrainingSize);
            size.Width = TextRenderer.MeasureText(text, this.Font).Width + this.Padding.Left + this.Padding.Right;
            size.Width = System.Math.Min(size.Width, 40);
            return size;
        }

        protected override void Dispose(bool disposing) {
            RemoveFilter();
            base.Dispose(disposing);
        }

        #endregion

        #region Event filtering

        private void AddFilter() {
            unsubscriber = messageFilter.Subscribe(this);
            if (System.Threading.Interlocked.Increment(ref messageFilterRefCount) == 1) {
                Application.AddMessageFilter(messageFilter);
            }
        }

        private void RemoveFilter() {
            if (System.Threading.Interlocked.Decrement(ref messageFilterRefCount) == 0) {
                Application.RemoveMessageFilter(messageFilter);
            }
            unsubscriber.Dispose();
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
