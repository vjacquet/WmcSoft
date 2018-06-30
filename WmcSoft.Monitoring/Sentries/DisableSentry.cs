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

namespace WmcSoft.Diagnostics.Sentries
{
    /// <summary>
    /// Decorates a sentry to support enable and disable state.
    /// </summary>
    public sealed class DisableSentry : SentryBase, IObserver<SentryStatus>
    {
        private readonly ISentry underlying;
        private IDisposable subscription;
        private SentryStatus underlyingStatus;
        private bool enabled;

        public DisableSentry(ISentry sentry, bool enabled = false) : base(sentry.Name)
        {
            this.enabled = enabled;
            underlying = sentry;
            subscription = sentry.Subscribe(this);
        }

        public bool Enabled {
            get {
                return enabled;
            }
            set {
                if (enabled == value) {
                    // nothing to do
                } else if (value) {
                    enabled = true;
                    OnNext(underlyingStatus);
                } else {
                    enabled = false;
                    OnNext(SentryStatus.None);
                }
            }
        }

        #region IObserver<SentryStatus> members

        void IObserver<SentryStatus>.OnCompleted()
        {
            OnCompleted();
            subscription = null;
        }

        void IObserver<SentryStatus>.OnError(Exception error)
        {
            OnError(error);
            subscription = null;
        }

        void IObserver<SentryStatus>.OnNext(SentryStatus value)
        {
            underlyingStatus = value;
            if (enabled)
                OnNext(value);
        }

        #endregion
    }
}
