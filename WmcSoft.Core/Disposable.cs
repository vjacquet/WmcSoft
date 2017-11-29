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
    /// <summary>
    /// Extensions and helpers for <see cref="IDisposable"/>.
    /// This is a static class.
    /// </summary>
    public static class Disposable
    {
        sealed class EmptyDisposable : IDisposable
        {
            void IDisposable.Dispose()
            {
            }
        };

        /// <summary>
        /// Returns a <see cref="IDisposable"/> that does nothing.
        /// </summary>
        public static readonly IDisposable Empty = new EmptyDisposable();

        #region Extensions

        public static void Push(this IDisposableBin bin, Action action)
        {
            bin.Add(new Disposer(action));
        }

        public static TDisposable Push<TDisposable>(this IDisposableBin bin, TDisposable disposable)
            where TDisposable : IDisposable
        {
            bin.Add(disposable);
            return disposable;
        }

        public static TDisposable ThrowIn<TDisposable>(this TDisposable disposable, IDisposableBin bin)
            where TDisposable : IDisposable
        {
            bin.Add(disposable);
            return disposable;
        }

        #endregion
    }
}
