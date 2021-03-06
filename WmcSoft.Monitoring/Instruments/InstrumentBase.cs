﻿#region Licence

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
using System.Runtime.CompilerServices;

namespace WmcSoft.Monitoring.Instruments
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
            private readonly IObserver<Timestamped<decimal>> observer;

            public Unsubscriber(InstrumentBase instrument, IObserver<Timestamped<decimal>> observer)
            {
                this.instrument = instrument;
                this.observer = observer;
            }

            public void Dispose()
            {
                instrument.Unsubscribe(observer);
            }
        }

        #endregion

        private readonly List<IObserver<Timestamped<decimal>>> observers = new List<IObserver<Timestamped<decimal>>>();

        protected InstrumentBase(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));

            Name = name;
        }

        public string Name { get; }

        /// <summary>
        /// Subscribes an observer on the sentry.
        /// </summary>
        /// <param name="observer">The observer</param>
        /// <returns>An <see cref="IDisposable"/> that unregisters the observer upon dispose.</returns>
        /// <remarks>Subscription and unscribscription are idempotent operations.</remarks>
        public IDisposable Subscribe(IObserver<Timestamped<decimal>> observer)
        {
            lock (observers) {
                if (Subscribing(observer)) {
                    OnSubscribe(observer);

                    observers.Add(observer);
                }
            }
            return new Unsubscriber(this, observer);
        }

        private void Unsubscribe(IObserver<Timestamped<decimal>> observer)
        {
            bool removed = false;
            lock (observers) {
                if (observers.Remove(observer)) {
                    removed = true;
                    if (observers.Count == 0) {
                        OnObserved();
                    }
                }
            }
            if (removed) {
                OnUnsubscribe(observer);
            }
        }

        private bool Subscribing(IObserver<Timestamped<decimal>> observer)
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
        protected virtual void OnSubscribe(IObserver<Timestamped<decimal>> observer)
        {
        }

        /// <summary>
        /// Called after the <paramref name="observer"/> is removed from the list of observers.
        /// </summary>
        /// <param name="observer">The observer.</param>
        protected virtual void OnUnsubscribe(IObserver<Timestamped<decimal>> observer)
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

        protected void Remove(List<IObserver<Timestamped<decimal>>> observers)
        {
            observers.ForEach(OnUnsubscribe);
            observers.Clear();

            if (this.observers.Count == 0)
                OnObserved();
        }

        #endregion

        #region Observer support

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void FastForEach(Action<IObserver<Timestamped<decimal>>> action)
        {
            observers.ForEach(action);
        }

        void SafeForEach(Action<IObserver<Timestamped<decimal>>> action)
        {
            var copy = new List<IObserver<Timestamped<decimal>>>(observers);
            copy.ForEach(action);
        }

        protected void OnNext(Timestamped<decimal> value)
        {
            lock (observers) {
                FastForEach(o => o.OnNext(value));
            }
        }

        protected void OnError(Exception error)
        {
            lock (observers) {
                SafeForEach(o => o.OnError(error));
                Remove(observers);
            }
        }

        protected void OnCompleted()
        {
            lock (observers) {
                SafeForEach(o => o.OnCompleted());
                Remove(observers);
            }
        }

        #endregion
    }
}
