#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Diagnostics.Instruments
{
    public abstract class InstrumentBase : IInstrument,IDisposable
    {
        #region Utilities

        class Unsubscriber : IDisposable
        {
            private readonly InstrumentBase _instrument;
            private readonly IObserver<decimal> _observer;

            public Unsubscriber(InstrumentBase instrument, IObserver<decimal> observer)
            {
                _instrument = instrument;
                _observer = observer;
            }

            public void Dispose()
            {
                var observers = _instrument._observers;
                bool removed = false;
                lock (observers) {
                    if (observers.Remove(_observer)) {
                        removed = true;
                        if (observers.Count == 0) {
                            _instrument.OnObserved();
                        }
                    }
                }
                if (removed) {
                    _instrument.OnUnsubscribe(_observer);
                }
            }
        }

        #endregion

        private readonly List<IObserver<decimal>> _observers = new List<IObserver<decimal>>();

        protected InstrumentBase(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));

            Name = name.Trim();
        }

        public string Name { get; }

        /// <summary>
        /// Subscribes an observer on the sentry.
        /// </summary>
        /// <param name="observer">The observer</param>
        /// <returns>An <see cref="IDisposable"/> that unregisters the observer upon dispose.</returns>
        /// <remarks>Subscription and unscribscription are idempotent operations.</remarks>
        public IDisposable Subscribe(IObserver<decimal> observer)
        {
            lock (_observers) {
                if (Subscribing(observer)) {
                    OnSubscribe(observer);

                    _observers.Add(observer);
                }
            }
            return new Unsubscriber(this, observer);
        }

        private bool Subscribing(IObserver<decimal> observer)
        {
            if (_observers.Count == 0) {
                OnObserving();
                return true;
            }
            return !_observers.Contains(observer);
        }

        #region Overridables methods

        /// <summary>
        /// Called before the <paramref name="observer"/> is added to the list of observers.
        /// </summary>
        /// <param name="observer">The observer.</param>
        protected virtual void OnSubscribe(IObserver<decimal> observer)
        {
        }

        /// <summary>
        /// Called after the <paramref name="observer"/> is removed from the list of observers.
        /// </summary>
        /// <param name="observer">The observer.</param>
        protected virtual void OnUnsubscribe(IObserver<decimal> observer)
        {
        }

        /// <summary>
        /// Called before the subscription of the first observer.
        /// </summary>
        protected virtual void OnObserving()
        {
        }

        /// <summary>
        /// Called after the unsubscription of the last observer.
        /// </summary>
        protected virtual void OnObserved()
        {
        }

        #endregion

        #region Observer support

        protected void OnNext(decimal value)
        {
            lock (_observers) {
                _observers.ForEach(o => o.OnNext(value));
            }
        }

        protected void OnError(Exception error)
        {
            lock (_observers) {
                _observers.ForEach(o => o.OnError(error));
                _observers.Clear();
            }
        }

        protected void OnCompleted()
        {
            lock (_observers) {
                _observers.ForEach(o => o.OnCompleted());
                _observers.Clear();
            }
        }

        #endregion
    }
}
