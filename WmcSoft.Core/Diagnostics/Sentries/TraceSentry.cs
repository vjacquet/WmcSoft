#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
using System.Diagnostics;

namespace WmcSoft.Diagnostics.Sentries
{
    public class TraceSentry : ISentry
    {
        #region Utility classes

        class Tracer : IObserver<SentryStatus>
        {
            private readonly TraceSentry _owner;

            public Tracer(TraceSentry owner)
            {
                _owner = owner;
            }

            public void OnNext(SentryStatus value)
            {
                _owner.TraceNext(value);
            }

            public void OnCompleted()
            {
                _owner.TraceCompleted();
            }

            public void OnError(Exception error)
            {
                _owner.TraceError(error);
            }
        }

        #endregion

        private readonly TraceSource _traceSource;
        private readonly ISentry _decorated;
        private readonly object _syncRoot;
        private int _refCount;
        private IDisposable _unsubscriber;

        public TraceSentry(TraceSource traceSource, ISentry decorated)
        {
            _syncRoot = new object();
            _traceSource = traceSource;
            _decorated = decorated;
            _refCount = 0;
        }

        public string Name {
            get { return _decorated.Name; }
        }

        public SentryStatus Status {
            get { return _decorated.Status; }
        }

        public IDisposable Subscribe(IObserver<SentryStatus> observer)
        {
            lock (_syncRoot) {
                if (++_refCount == 1) {
                    _unsubscriber = _decorated.Subscribe(new Tracer(this));
                }
            }
            var unsubscriber = _decorated.Subscribe(observer);
            return new Disposer(() => {
                unsubscriber.Dispose();

                var dispose = false;
                var disposer = _unsubscriber;
                lock (_syncRoot) {
                    if (--_refCount == 0) {
                        dispose = true;
                        _unsubscriber = null;
                    }
                }
                if (dispose) {
                    disposer.Dispose();
                }
            });
        }

        private void TraceNext(SentryStatus value)
        {
            _traceSource.TraceInformation($"Sentry {Name} observed {value}.");
        }

        private void TraceCompleted()
        {
            _traceSource.TraceInformation($"Sentry {Name} completed.");
        }

        private void TraceError(Exception error)
        {
            _traceSource.TraceError(0, $"Sentry {Name} failed: {error}.");
        }
    }
}