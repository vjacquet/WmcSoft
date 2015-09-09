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
using WmcSoft.Diagnostics;

namespace WmcSoft
{
    public static class ActionExtensions
    {
        #region Helpers

        internal static void Noop() {
        }

        #endregion

        #region ApplyEach & TryEach

        /// <summary>
        /// Apply the action for each argument.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="func">The action</param>
        /// <param name="args">The arguments</param>
        /// <remarks>If a call throw, the context {i, arg } is captured in the exception Data property, using the default DataKeyConverter.</remarks>
        public static void ApplyEach<T, TResult>(this Action<T> action, params T[] args) {
            int i = 0;
            try {
                var results = new TResult[args.Length];
                for (; i < args.Length; i++) {
                    action(args[i]);
                }
            }
            catch (Exception e) {
                e.CaptureContext(new { i, arg = args[i] });
                throw;
            }
        }

        /// <summary>
        /// Try to apply the action for each argument, returning the caught exception when the call failed.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="func">The action</param>
        /// <param name="args">The arguments</param>
        /// <remarks>If a call throw, the context {i, arg } is captured in the exception Data property, using the default DataKeyConverter.</remarks>
        /// <returns>The array of exception, in the order of the arguments.</returns>
        public static Exception[] TryEach<T, TResult>(this Action<T> action, params T[] args) {
            var results = new Exception[args.Length];
            for (int i = 0; i < args.Length; i++) {
                try {
                    action(args[i]);
                }
                catch (Exception e) {
                    results[i] = e;
                }
            }
            return results;
        }

        #endregion

        #region Then

        public static Action<T> Then<T>(this Action<T> action, Action<T> then) {
            return delegate (T obj) {
                action(obj);
                then(obj);
            };
        }

        #endregion

        #region Unless

        public static Action<T> Unless<T>(this Action<T> action, Predicate<T> condition) {
            return delegate (T obj) {
                if (!condition(obj))
                    action(obj);
            };
        }

        #endregion

        #region Using

        public static Action<T> Using<T>(this Action<T> action, IDisposable usedBy) {
            return delegate (T obj) {
                using (usedBy) {
                    action(obj);
                }
            };
        }

        public static Action<T> Using<T>(this Action<T> action, Func<T, IDisposable> usedBy) {
            return delegate (T obj) {
                using (usedBy(obj)) {
                    action(obj);
                }
            };
        }

        #endregion
    }
}
