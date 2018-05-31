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
using System.Diagnostics;

namespace WmcSoft.Diagnostics.Instruments
{
    /// <summary>
    /// Represents an instrument.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq}")]
    public abstract class InstrumentBase : IInstrument
    {
        #region Utilities

        class Unsubscriber : IDisposable
        {
            private readonly InstrumentBase instrument;
            private readonly IObserver<decimal> observer;

            public Unsubscriber(InstrumentBase instrument, IObserver<decimal> observer)
            {
                this.instrument = instrument;
                this.observer = observer;
            }

            public void Dispose()
            {
                var observers = instrument.observers;
                bool removed = false;
                lock (observers) {
                    if (observers.Remove(observer)) {
                        removed = true;
                        if (observers.Count == 0) {
                            instrument.OnObserved();
                        }
                    }
                }
                if (removed) {
                    instrument.OnUnsubscribe(observer);
                }
            }
        }

        #endregion

        private readonly List<IObserver<decimal>> observers = new List<IObserver<decimal>>();

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
            lock (observers) {
                if (Subscribing(observer)) {
                    OnSubscribe(observer);

                    observers.Add(observer);
                }
            }
            return new Unsubscriber(this, observer);
        }

        private bool Subscribing(IObserver<decimal> observer)
        {
            if (observers.Count == 0) {
                OnObserving();
                return true;
            }
            return !observers.Contains(observer);
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
            lock (observers) {
                observers.ForEach(o => o.OnNext(value));
            }
        }

        protected void OnError(Exception error)
        {
            lock (observers) {
                observers.ForEach(o => o.OnError(error));
                observers.Clear();
            }
        }

        protected void OnCompleted()
        {
            lock (observers) {
                observers.ForEach(o => o.OnCompleted());
                observers.Clear();
            }
        }

        #endregion
    }
}
