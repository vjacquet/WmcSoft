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

        /// <summary>
        /// Pushes in the <paramref name="bin"/> an <paramref name="action"/> to execute on <see cref="IDisposable.Dispose"/>.
        /// </summary>
        /// <param name="bin">The disposable bin.</param>
        /// <param name="action">The action to execute on <see cref="IDisposable.Dispose"/>.</param>
        public static void Push(this IDisposableBin bin, Action action)
        {
            bin.Add(new Disposer(action));
        }

        /// <summary>
        /// Pushes in the <paramref name="bin"/> a <paramref name="disposable"/> and returns it.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the <paramref name="disposable"/>.</typeparam>
        /// <param name="bin">The disposable bin.</param>
        /// <param name="disposable">The instance to dispose.</param>
        /// <returns>The <paramref name="disposable"/>.</returns>
        public static TDisposable Push<TDisposable>(this IDisposableBin bin, TDisposable disposable)
            where TDisposable : IDisposable
        {
            bin.Add(disposable);
            return disposable;
        }

        /// <summary>
        /// Throws the <paramref name="disposable"/> in the <paramref name="bin"/> and returns it.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the <paramref name="disposable"/>.</typeparam>
        /// <param name="bin">The disposable bin.</param>
        /// <param name="disposable">The instance to dispose.</param>
        /// <returns>The <paramref name="disposable"/>.</returns>
        public static TDisposable ThrowIn<TDisposable>(this TDisposable disposable, IDisposableBin bin)
            where TDisposable : IDisposable
        {
            bin.Add(disposable);
            return disposable;
        }

        #endregion
    }
}
