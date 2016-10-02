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
using WmcSoft.Diagnostics;

namespace WmcSoft
{
    public static class FuncExtensions
    {
        #region ApplyEach & TryEach

        /// <summary>
        /// Apply the func for each argument.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">The function</param>
        /// <param name="args">The arguments</param>
        /// <returns>The array of results, in the order of the arguments.</returns>
        /// <remarks>If a call throw, the context {i, arg } is captured in the exception Data property, using the default DataKeyConverter.</remarks>
        public static TResult[] ApplyEach<T, TResult>(this Func<T, TResult> func, params T[] args) {
            int i = 0;
            try {
                var results = new TResult[args.Length];
                for (; i < args.Length; i++) {
                    results[i] = func(args[i]);
                }
                return results;
            }
            catch (Exception e) {
                e.CaptureContext(new { i, arg = args[i] });
                throw;
            }
        }

        /// <summary>
        /// Try to the func for each argument.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">The function</param>
        /// <param name="args">The arguments</param>
        /// <returns>The array of expected results, in the order of the arguments.</returns>
        public static Expected<TResult>[] TryEach<T, TResult>(this Func<T, TResult> func, params T[] args) {
            var results = new Expected<TResult>[args.Length];
            for (int i = 0; i < args.Length; i++) {
                try {
                    results[i] = func(args[i]);
                }
                catch (Exception e) {
                    results[i] = e;
                }
            }
            return results;
        }

        #endregion

        #region Bind

        public static Action<T2> BindFirst<T1, T2>(this Action<T1, T2> action, T1 first) {
            return x => action(first, x);
        }

        public static Action<T1> BindSecond<T1, T2>(this Action<T1, T2> action, T2 second) {
            return x => action(x, second);
        }

        public static Func<T2, TResult> BindFirst<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 first) {
            return x => func(first, x);
        }

        public static Func<T1, TResult> BindSecond<T1, T2, TResult>(this Func<T1, T2, TResult> func, T2 second) {
            return x => func(x, second);
        }

        #endregion

        #region Lift

        public static Func<T?, TResult?> Lift<T, TResult>(this Func<T, TResult> func)
            where T : struct
            where TResult : struct {
            return x => {
                if (x.HasValue)
                    return func(x.GetValueOrDefault());
                return default(TResult);
            };
        }

        #endregion

        #region Pack/Unpack

        public static Func<Tuple<T1, T2>, TResult> Pack<T1, T2, TResult>(this Func<T1, T2, TResult> func) {
            return t => func(t.Item1, t.Item2);
        }

        public static Func<T1, T2, TResult> Unpack<T1, T2, TResult>(this Func<Tuple<T1, T2>, TResult> func) {
            return (t1, t2) => func(Tuple.Create(t1, t2));
        }

        public static Func<Tuple<T1, T2, T3>, TResult> Pack<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func) {
            return t => func(t.Item1, t.Item2, t.Item3);
        }

        public static Func<T1, T2, T3, TResult> Unpack<T1, T2, T3, TResult>(this Func<Tuple<T1, T2, T3>, TResult> func) {
            return (t1, t2, t3) => func(Tuple.Create(t1, t2, t3));
        }

        public static Func<Tuple<T1, T2, T3, T4>, TResult> Pack<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func) {
            return t => func(t.Item1, t.Item2, t.Item3, t.Item4);
        }

        public static Func<T1, T2, T3, T4, TResult> Unpack<T1, T2, T3, T4, TResult>(this Func<Tuple<T1, T2, T3, T4>, TResult> func) {
            return (t1, t2, t3, t4) => func(Tuple.Create(t1, t2, t3, t4));
        }

        public static Func<Tuple<T1, T2, T3, T4, T5>, TResult> Pack<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func) {
            return t => func(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5);
        }

        public static Func<T1, T2, T3, T4, T5, TResult> Unpack<T1, T2, T3, T4, T5, TResult>(this Func<Tuple<T1, T2, T3, T4, T5>, TResult> func) {
            return (t1, t2, t3, t4, t5) => func(Tuple.Create(t1, t2, t3, t4, t5));
        }

        public static Func<Tuple<T1, T2, T3, T4, T5, T6>, TResult> Pack<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func) {
            return t => func(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6);
        }

        public static Func<T1, T2, T3, T4, T5, T6, TResult> Unpack<T1, T2, T3, T4, T5, T6, TResult>(this Func<Tuple<T1, T2, T3, T4, T5, T6>, TResult> func) {
            return (t1, t2, t3, t4, t5, t6) => func(Tuple.Create(t1, t2, t3, t4, t5, t6));
        }

        public static Func<Tuple<T1, T2, T3, T4, T5, T6, T7>, TResult> Pack<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func) {
            return t => func(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> Unpack<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<Tuple<T1, T2, T3, T4, T5, T6, T7>, TResult> func) {
            return (t1, t2, t3, t4, t5, t6, t7) => func(Tuple.Create(t1, t2, t3, t4, t5, t6, t7));
        }

        #endregion

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

        #region Logical operators

        public static Func<T, bool> Negation<T>(this Func<T, bool> predicate) {
            return _ => !predicate(_);
        }

        public static Func<T, bool> Conjunction<T>(this IEnumerable<Func<T, bool>> predicates) {
            var array = predicates.ToArray();
            return _ => array.All(f => f(_));
        }

        public static Func<T, bool> Disjunction<T>(this IEnumerable<Func<T, bool>> predicates) {
            var array = predicates.ToArray();
            return _ => array.Any(f => f(_));
        }

        #endregion
    }
}