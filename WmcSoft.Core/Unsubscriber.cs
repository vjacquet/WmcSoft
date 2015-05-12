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

namespace WmcSoft
{
    public sealed class Unsubscriber : IDisposable
    {
        #region Private field

        Action _unsubscribe;

        #endregion

        #region Lifecycle

        public Unsubscriber(Action unsubscribe)  {
            if (unsubscribe == null)
                throw new ArgumentNullException("unsubscribe");

            _unsubscribe = unsubscribe;
        }

        public Unsubscriber(IDisposable unsubscribe) {
            if (unsubscribe == null)
                throw new ArgumentNullException("unsubscribe");

            _unsubscribe = () => unsubscribe.Dispose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Explicitly unsubscribe the <see cref="IObserver<T>"/> from the <see cref="IObservable<T>"/>.
        /// </summary>
        public void Unsubscribe() {
            ((IDisposable)this).Dispose();
        }

        #endregion

        #region IDisposable Membres

        ~Unsubscriber() {
            // in the finalizer only when not disposed, so _unsubscribe cannot be null
            _unsubscribe(); 
        }

        void IDisposable.Dispose() {
            if (_unsubscribe != null) {
                _unsubscribe();
                _unsubscribe = null;
            }
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
