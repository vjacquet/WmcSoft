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
using System.Threading;

namespace WmcSoft.Diagnostics.Sentries
{
    /// <summary>
    /// Represents a sentry observing an observable and evaluating its status through a chain of responsability.
    /// </summary>
    /// <typeparam name="T">The type of value observed.</typeparam>
    public class ChainOfResponsabilitySentry<T> : SentryBase, IObserver<T>
    {
        static readonly Func<T, SentryStatus> Success = x => SentryStatus.Success;
        static readonly Func<T, SentryStatus>[] Default = new[] { Success };

        private readonly Func<T, SentryStatus>[] analyzers;
        private readonly IObservable<T> underlying;
        private IDisposable subscription;

        public ChainOfResponsabilitySentry(string name, IObservable<T> observable, params Func<T, SentryStatus>[] analyzers)
            : base(name)
        {
            if (observable == null) throw new ArgumentNullException(nameof(observable));

            this.analyzers = analyzers != null && analyzers.Length > 0 ? analyzers : Default;
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

        protected virtual SentryStatus Analyze(T value)
        {
            foreach (var analyzer in analyzers) {
                var status = analyzer(value);
                if (status != SentryStatus.None)
                    return status;
            }
            return SentryStatus.Error;
        }

        void IObserver<T>.OnNext(T value)
        {
            var status = Analyze(value);
            OnNext(status);
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
}
