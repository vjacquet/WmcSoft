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
using System.Threading;

namespace WmcSoft.Monitoring.Instruments
{
    /// <summary>
    /// Represents an instrument observing an observable and computing a metric from it.
    /// </summary>
    /// <typeparam name="T">The type of value observed.</typeparam>
    public abstract class ObservingInstrument<T> : InstrumentBase, IObserver<T>
    {
        private readonly IObservable<T> underlying;
        private IDisposable subscription;

        public ObservingInstrument(string name, IObservable<T> observable)
            : base(name)
        {
            if (observable == null) throw new ArgumentNullException(nameof(observable));
            underlying = observable;
        }

        protected override void OnObserving()
        {
            subscription = underlying.Subscribe(this);
            base.OnObserving();
        }

        protected override void OnObserved()
        {
            base.OnObserved();
            var disposable = Interlocked.Exchange(ref subscription, null);
            if (disposable != null) {
                disposable.Dispose();
            }
        }

        protected abstract decimal Measure(T value);

        void IObserver<T>.OnNext(T value)
        {
            var measure = Measure(value);
            OnNext(new Timestamped<decimal>(measure, DateTime.UtcNow));
        }

        void IObserver<T>.OnError(Exception error)
        {
            OnError(error);
        }

        void IObserver<T>.OnCompleted()
        {
            OnCompleted();
        }

    }

    /// <summary>
    /// Represents an instrument observing an observable producing decimals.
    /// </summary>
    public class ObservingInstrument : ObservingInstrument<decimal>
    {
        public ObservingInstrument(string name, IObservable<decimal> observable)
            : base(name, observable)
        {
        }

        protected override decimal Measure(decimal value)
        {
            return value;
        }
    }
}
