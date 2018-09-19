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
using System.IO;

namespace WmcSoft.IO
{
    /// <summary>
    /// Notifies when a stream is about to be closed, potentially cancelling it.
    /// </summary>
    [Serializable]
    public class ClosableStream : StreamDecorator
    {
        public ClosableStream(Stream stream, CancelEventHandler onClosing = null, EventHandler onClosed = null)
             : base(stream)
        {
            if (onClosing != null)
                Closing += onClosing;
            if (onClosed != null)
                Closed += onClosed;
        }

        public sealed override void Close()
        {
            if (!IsClosed) {
                if (OnClosing()) {
                    base.Close();
                    IsClosed = true;
                    OnClosed();
                }
            }
        }

        public bool IsClosed { get; private set; }

        public event CancelEventHandler Closing;
        public event EventHandler Closed;

        protected bool OnClosing()
        {
            var handler = Closing;
            if (handler != null) {
                var e = new CancelEventArgs(false);
                handler(this, e);
                return !e.Cancel;
            }
            return true;
        }

        protected void OnClosed()
        {
            var handler = Closed;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
