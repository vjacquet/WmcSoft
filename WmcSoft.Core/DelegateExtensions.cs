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
using System.Collections.Generic;
using System.Linq;

namespace WmcSoft
{
    /// <summary>
    /// Defines the extension methods to the <see cref="Delegate"/> class. This is a static class.
    /// </summary>
    public static class DelegateExtensions
    {
        static readonly Action<Delegate, object[]> wrapper = new Action<Delegate, object[]>((d, args) => d.DynamicInvoke(args));

        static readonly AsyncCallback callback = new AsyncCallback((ar) => {
            using (ar.AsyncWaitHandle ?? Disposable.Empty) {
                wrapper.EndInvoke(ar);
            }
        });

        /// <summary>
        /// Executes the specified delegate with the specified arguments
        /// asynchronously on a thread pool thread.
        /// </summary>
        public static void InvokeAsyncAndForget(this Delegate d, params object[] args) {
            wrapper.BeginInvoke(d, args, callback, null);
        }

        /// <summary>
        /// Returns the filtered invocation list of the delegate.
        /// </summary>
        /// <param name="self">The delegate.</param>
        /// <param name="predicate">The predicate the filter the invocation list.</param>
        /// <returns>The filtered invocation list of the delegate.</returns>
        public static IEnumerable<Delegate> GetInvocationList(this Delegate self, Predicate<Delegate> predicate) {
            return self.GetInvocationList().Where(d => predicate(d));
        }
    }
}
