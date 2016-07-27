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
using System.Linq;
using System.Threading;

namespace WmcSoft.Diagnostics.Sentries
{
    /// <summary>
    /// Represents a sentry that observe and aggregate the status of other sentries.
    /// </summary>
    public class AggregateSentry : SentryBase, IObserver<SentryStatus>, IDisposable
    {
        #region Utility classes

        sealed class SentryStatusAggregator
        {
            private int _aggregated;

            public SentryStatusAggregator Add(ISentry sentry) {
                return Add(sentry.Status);
            }

            public SentryStatusAggregator Add(SentryStatus value) {
                switch (value) {
                case SentryStatus.Success:
                    _aggregated |= 1;
                    break;
                case SentryStatus.Warning:
                    _aggregated |= 3;
                    break;
                case SentryStatus.Error:
                    _aggregated |= 2;
                    break;
                }
                return this;
            }

            public SentryStatus GetResult() {
                switch (_aggregated) {
                case 1:
                    return SentryStatus.Success;
                case 2:
                    return SentryStatus.Error;
                case 3:
                    return SentryStatus.Warning;
                case 0:
                default:
                    return SentryStatus.None;
                }
            }
        }

        #endregion

        private readonly ISentry[] _sentries;
        private readonly IDisposable[] _unsubscribers;

        public AggregateSentry(string name, params ISentry[] sentries) : base(name) {
            _sentries = sentries;
            _unsubscribers = new IDisposable[_sentries.Length];
        }

        protected override void OnObserving() {
            var length = _sentries.Length;
            for (int i = 0; i < length; i++) {
                _unsubscribers[i] = _sentries[i].Subscribe(this);
            }
        }

        protected override void OnObserved() {
            UnsubscribeAll();
        }

        void IObserver<SentryStatus>.OnNext(SentryStatus value) {
            var aggregator = new SentryStatusAggregator();
            var aggregate = _sentries.Aggregate(aggregator, (a, s) => a.Add(s)).GetResult();
            OnNext(aggregate);
        }

        void IObserver<SentryStatus>.OnError(Exception error) {
        }

        void IObserver<SentryStatus>.OnCompleted() {
        }

        ~AggregateSentry() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            UnsubscribeAll();
        }

        private void UnsubscribeAll() {
            var length = _sentries.Length;
            for (int i = 0; i < length; i++) {
                var disposer = Interlocked.Exchange(ref _unsubscribers[i], null);
                disposer?.Dispose();
            }
        }
    }
}