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
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    public static class FuncExtensions
    {
        #region Shield

        /// <summary>
        /// A function adaptor to transform exception thrown into another one.
        /// </summary>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <typeparam name="TException">The exception type.</typeparam>
        /// <param name="func">The function to adapt.</param>
        /// <param name="rethrow">The transformation function for the exception</param>
        /// <returns>The result of the function.</returns>
        public static Func<TResult> Shield<TResult, TException>(this Func<TResult> func, Func<Exception, TException> rethrow) where TException : Exception {
            return () => {
                try {
                    return func();
                }
                catch (Exception e) {
                    throw rethrow(e);
                }
            };
        }

        /// <summary>
        /// A function adaptor to transform exception thrown into another one.
        /// </summary>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <typeparam name="TException">The exception type.</typeparam>
        /// <param name="func">The function to adapt.</param>
        /// <param name="rethrow">The transformation function for the exception</param>
        /// <returns>The result of the function.</returns>
        public static Func<T, TResult> Shield<T, TResult, TException>(this Func<T, TResult> func, Func<Exception, TException> rethrow) where TException : Exception {
            return (T t) => {
                try {
                    return func(t);
                }
                catch (Exception e) {
                    throw rethrow(e);
                }
            };
        }

        /// <summary>
        /// A function adaptor to transform exception thrown into another one.
        /// </summary>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <typeparam name="TException">The exception type.</typeparam>
        /// <param name="func">The function to adapt.</param>
        /// <param name="rethrow">The transformation function for the exception</param>
        /// <returns>The result of the function.</returns>
        public static Func<T, TResult> Shield<T, TResult, TException>(this Func<T, TResult> func, Func<T, Exception, TException> rethrow) where TException : Exception {
            return (T t) => {
                try {
                    return func(t);
                }
                catch (Exception e) {
                    throw rethrow(t, e);
                }
            };
        }

        /// <summary>
        /// A function adaptor to transform exception thrown into another one.
        /// </summary>
        /// <typeparam name="TException">The exception type.</typeparam>
        /// <param name="action">The function to adapt.</param>
        /// <param name="rethrow">The transformation function for the exception</param>
        /// <returns>The result of the function.</returns>
        public static Action Shield<TResult, TException>(this Action action, Func<Exception, TException> rethrow) where TException : Exception {
            return () => {
                try {
                    action();
                }
                catch (Exception e) {
                    throw rethrow(e);
                }
            };
        }

        /// <summary>
        /// A function adaptor to transform exception thrown into another one.
        /// </summary>
        /// <typeparam name="TException">The exception type.</typeparam>
        /// <param name="action">The function to adapt.</param>
        /// <param name="rethrow">The transformation function for the exception</param>
        public static Action<T> Shield<T, TException>(this Action<T> action, Func<Exception, TException> rethrow) where TException : Exception {
            return (T t) => {
                try {
                    action(t);
                }
                catch (Exception e) {
                    throw rethrow(e);
                }
            };
        }

        /// <summary>
        /// A function adaptor to transform exception thrown into another one.
        /// </summary>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <typeparam name="TException">The exception type.</typeparam>
        /// <param name="action">The function to adapt.</param>
        /// <param name="rethrow">The transformation function for the exception</param>
        /// <returns>The result of the function.</returns>
        public static Action<T> Shield<T, TException>(this Action<T> action, Func<T, Exception, TException> rethrow) where TException : Exception {
            return (T t) => {
                try {
                    action(t);
                }
                catch (Exception e) {
                    throw rethrow(t, e);
                }
            };
        }

        #endregion
    }
}
