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
using System.Collections.Generic;
using System.Diagnostics;

namespace WmcSoft.Diagnostics.Sentries
{
    /// <summary>
    /// Represents a sentry.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq}")]
    public abstract class SentryBase : ISentry
    {
        #region Utilities

        class Unsubscriber : IDisposable
        {
            private readonly SentryBase _sentry;
            private readonly IObserver<SentryStatus> _observer;

            public Unsubscriber(SentryBase sentry, IObserver<SentryStatus> observer) {
                _sentry = sentry;
                _observer = observer;
            }

            public void Dispose() {
                var observers = _sentry._observers;
                bool removed = false;
                lock (observers) {
                    if (observers.Remove(_observer)) {
                        removed = true;
                        if (observers.Count == 0)
                            _sentry.OnObserved();
                    }
                }
                if (removed) {
                    _sentry.OnUnsubscribe(_observer);
                }
            }
        }

        #endregion

        private readonly List<IObserver<SentryStatus>> _observers;
        private readonly string _name;
        private volatile SentryStatus _status;

        protected SentryBase(string name) {
            _name = name;
            _observers = new List<IObserver<SentryStatus>>();
        }

        public string Name { get { return _name; } }
        public SentryStatus Status { get { return _status; } }

        /// <summary>
        /// Subscribes an observer on the sentry.
        /// </summary>
        /// <param name="observer">The observer</param>
        /// <returns>An <see cref="IDisposable"/> that unregisters the observer upon dispose.</returns>
        /// <remarks>Subscription and unscribscription are idempotent operations.</remarks>
        public IDisposable Subscribe(IObserver<SentryStatus> observer) {
            lock (_observers) {
                if (Subscribing(observer)) {
                    OnSubscribe(observer);

                    observer.OnNext(Status);
                    _observers.Add(observer);
                }
            }
            return new Unsubscriber(this, observer);
        }

        private bool Subscribing(IObserver<SentryStatus> observer) {
            if (_observers.Count == 0) {
                OnObserving();
                return true;
            }
            return !_observers.Contains(observer);
        }

        #region Overridables methods

        public override string ToString() {
            return Name + ": " + _status + " (" + _observers.Count + ")";
        }

        /// <summary>
        /// Called before the <paramref name="observer"/> is added to the list of observers.
        /// </summary>
        /// <param name="observer">The observer.</param>
        protected virtual void OnSubscribe(IObserver<SentryStatus> observer) {
        }

        /// <summary>
        /// Called after the <paramref name="observer"/> is removed from the list of observers.
        /// </summary>
        /// <param name="observer">The observer.</param>
        protected virtual void OnUnsubscribe(IObserver<SentryStatus> observer) {
        }

        /// <summary>
        /// Called before the subscription of the first observer.
        /// </summary>
        protected virtual void OnObserving() {
        }

        /// <summary>
        /// Called after the unsubscription of the last observer.
        /// </summary>
        protected virtual void OnObserved() {
        }

        #endregion

        #region Observer support

        protected void OnNext(SentryStatus status) {
            lock (_observers) {
                _status = status;
                _observers.ForEach(o => o.OnNext(status));
            }
        }

        protected void OnError(Exception error) {
            lock (_observers) {
                _observers.ForEach(o => o.OnError(error));
            }
        }

        protected void OnCompleted() {
            lock (_observers) {
                _observers.ForEach(o => o.OnCompleted());
            }
        }

        #endregion
    }
}