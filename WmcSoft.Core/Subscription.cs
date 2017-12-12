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
using System.Threading;

namespace WmcSoft
{
    /// <summary>
    /// Utility to explicit the nature of the disposable.
    /// For instance received, it could be an <see cref="IDisposable"/> returned when subscribing to an observable. 
    /// </summary>
    public sealed class Subscription : IDisposable
    {
        private Action _unsubscribe;

        /// <summary>
        /// Creates a new <see cref="Subscription"/> from an unsubscribe action.
        /// </summary>
        /// <param name="unsubscribe">The action to execute to unsubscribe.</param>
        /// <exception cref="ArgumentNullException"><paramref name="unsubscribe"/> is <c>null</c>.</exception>
        public Subscription(Action unsubscribe)
        {
            if (unsubscribe == null) throw new ArgumentNullException(nameof(unsubscribe));

            _unsubscribe = unsubscribe;
        }

        /// <summary>
        /// Creates a new <see cref="Subscription"/> from an <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="unsubscriber">The disposable to dispose to unsubscribe.</param>
        /// <exception cref="ArgumentNullException"><paramref name="unsubscriber"/> is <c>null</c>.</exception>
        public Subscription(IDisposable unsubscriber)
        {
            if (unsubscriber == null) throw new ArgumentNullException(nameof(unsubscriber));

            _unsubscribe = unsubscriber.Dispose;
        }

        /// <summary>
        /// Explicitly unsubscribe.
        /// </summary>
        public void Unsubscribe()
        {
            ((IDisposable)this).Dispose();
        }

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);

            var action = Interlocked.Exchange(ref _unsubscribe, null);
            if (action != null) {
                action();
            }
        }
    }
}
