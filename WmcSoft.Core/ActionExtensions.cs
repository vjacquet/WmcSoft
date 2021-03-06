﻿#region Licence

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
using System.Diagnostics;
using WmcSoft.Diagnostics;

namespace WmcSoft
{
    /// <summary>
    /// Defines the extension methods to the <see cref="Action"/> delegate.
    /// This is a static class.
    /// </summary>
    public static class ActionExtensions
    {
        #region Helpers

        internal static void Noop()
        {
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
        public static void ApplyEach<T>(this Action<T> action, params T[] args)
        {
            Debug.Assert(action != null);

            int i = 0;
            try {
                for (; i < args.Length; i++) {
                    action(args[i]);
                }
            } catch (Exception e) {
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
        public static Exception[] TryEach<T>(this Action<T> action, params T[] args)
        {
            Debug.Assert(action != null);

            var results = new Exception[args.Length];
            for (int i = 0; i < args.Length; i++) {
                try {
                    action(args[i]);
                } catch (Exception e) {
                    results[i] = e;
                }
            }
            return results;
        }

        #endregion

        #region Then

        /// <summary>
        /// Executes an action then another using the same argument.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="action">The first action to execute.</param>
        /// <param name="then">The second action to execute.</param>
        /// <returns>An action performing both actions.</returns>
        public static Action<T> Then<T>(this Action<T> action, Action<T> then)
        {
            Debug.Assert(action != null);
            if (then == null) throw new ArgumentNullException(nameof(then));

            return delegate (T obj) {
                action(obj);
                then(obj);
            };
        }

        #endregion

        #region Pack/Unpack

        public static Action<Tuple<T1, T2>> Pack<T1, T2>(this Action<T1, T2> action)
        {
            Debug.Assert(action != null);
            return t => action(t.Item1, t.Item2);
        }

        public static Action<T1, T2> Unpack<T1, T2>(this Action<Tuple<T1, T2>> action)
        {
            Debug.Assert(action != null);
            return (t1, t2) => action(Tuple.Create(t1, t2));
        }

        public static Action<Tuple<T1, T2, T3>> Pack<T1, T2, T3>(this Action<T1, T2, T3> action)
        {
            Debug.Assert(action != null);
            return t => action(t.Item1, t.Item2, t.Item3);
        }

        public static Action<T1, T2, T3> Unpack<T1, T2, T3>(this Action<Tuple<T1, T2, T3>> action)
        {
            Debug.Assert(action != null);
            return (t1, t2, t3) => action(Tuple.Create(t1, t2, t3));
        }

        public static Action<Tuple<T1, T2, T3, T4>> Pack<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action)
        {
            Debug.Assert(action != null);
            return t => action(t.Item1, t.Item2, t.Item3, t.Item4);
        }

        public static Action<T1, T2, T3, T4> Unpack<T1, T2, T3, T4>(this Action<Tuple<T1, T2, T3, T4>> action)
        {
            Debug.Assert(action != null);
            return (t1, t2, t3, t4) => action(Tuple.Create(t1, t2, t3, t4));
        }

        public static Action<Tuple<T1, T2, T3, T4, T5>> Pack<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action)
        {
            Debug.Assert(action != null);
            return t => action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5);
        }

        public static Action<T1, T2, T3, T4, T5> Unpack<T1, T2, T3, T4, T5>(this Action<Tuple<T1, T2, T3, T4, T5>> action)
        {
            Debug.Assert(action != null);
            return (t1, t2, t3, t4, t5) => action(Tuple.Create(t1, t2, t3, t4, t5));
        }

        public static Action<Tuple<T1, T2, T3, T4, T5, T6>> Pack<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action)
        {
            Debug.Assert(action != null);
            return t => action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6);
        }

        public static Action<T1, T2, T3, T4, T5, T6> Unpack<T1, T2, T3, T4, T5, T6>(this Action<Tuple<T1, T2, T3, T4, T5, T6>> action)
        {
            Debug.Assert(action != null);
            return (t1, t2, t3, t4, t5, t6) => action(Tuple.Create(t1, t2, t3, t4, t5, t6));
        }

        public static Action<Tuple<T1, T2, T3, T4, T5, T6, T7>> Pack<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            Debug.Assert(action != null);
            return t => action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7> Unpack<T1, T2, T3, T4, T5, T6, T7>(this Action<Tuple<T1, T2, T3, T4, T5, T6, T7>> action)
        {
            Debug.Assert(action != null);
            return (t1, t2, t3, t4, t5, t6, t7) => action(Tuple.Create(t1, t2, t3, t4, t5, t6, t7));
        }

        #endregion

        #region Unless

        /// <summary>
        /// Executes the action unless the condition is <c>true</c>.
        /// </summary>
        /// <typeparam name="T">The type of argument for the action and the condition.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="condition">The condition.</param>
        /// <returns>An action that combines testing the condition and executing the action.</returns>
        public static Action<T> Unless<T>(this Action<T> action, Predicate<T> condition)
        {
            Debug.Assert(action != null);
            if (condition == null) throw new ArgumentNullException(nameof(condition));

            return delegate (T obj) {
                if (!condition(obj))
                    action(obj);
            };
        }

        #endregion

        #region Using

        /// <summary>
        /// Surrounds the action by a using statement.
        /// </summary>
        /// <typeparam name="T">The type of argument for the action.</typeparam>
        /// <param name="action">The action</param>
        /// <returns>An action</returns>
        public static Action<T> Using<T>(this Action<T> action)
            where T : IDisposable
        {
            Debug.Assert(action != null);

            return delegate (T obj) {
                using (obj) {
                    action(obj);
                }
            };
        }

        /// <summary>
        /// Surrounds the action by a using statement.
        /// </summary>
        /// <typeparam name="T">The type of argument for the action.</typeparam>
        /// <param name="action">The action</param>
        /// <param name="usedBy">The instance of IDisposable</param>
        /// <returns>An action</returns>
        public static Action<T> Using<T>(this Action<T> action, IDisposable usedBy)
        {
            Debug.Assert(action != null);

            return delegate (T obj) {
                using (usedBy) {
                    action(obj);
                }
            };
        }

        /// <summary>
        /// Surrounds the action by a using statement.
        /// </summary>
        /// <typeparam name="T">The type of argument for the action.</typeparam>
        /// <param name="action">The action</param>
        /// <param name="usedBy">A function returning an <see cref="IDisposable"/>.</param>
        /// <returns>An action</returns>
        public static Action<T> Using<T>(this Action<T> action, Func<T, IDisposable> usedBy)
        {
            Debug.Assert(action != null);

            return delegate (T obj) {
                using (usedBy(obj)) {
                    action(obj);
                }
            };
        }

        #endregion

        #region Once / After / Before

        /// <summary>
        /// Creates a version of the <see cref="Action"/> that can only be call one time.
        /// </summary>
        /// <param name="action">The action to run.</param>
        /// <returns>A new action.</returns>
        public static Action Once(this Action action)
        {
            Debug.Assert(action != null);

            bool ran = false;
            return () => {
                if (!ran) {
                    ran = true;
                    action();
                }
            };
        }

        /// <summary>
        /// Creates a version of the <see cref="Action"/> that will only be run after being call <paramref name="count"/> times.
        /// </summary>
        /// <param name="action">The action to run.</param>
        /// <param name="count">The number of times to call the action.</param>
        /// <returns>A new action.</returns>
        public static Action After(this Action action, int count)
        {
            Debug.Assert(action != null);

            count = Math.Max(0, count);
            return () => {
                if (count > 0) {
                    count--;
                } else {
                    action();
                }
            };
        }

        /// <summary>
        /// Creates a version of the <see cref="Action"/> that can be called no more than <paramref name="count"/> times.
        /// </summary>
        /// <param name="action">The action to run.</param>
        /// <param name="count">The number of times to call the action.</param>
        /// <returns>A new action.</returns>
        public static Action Before(this Action action, int count)
        {
            Debug.Assert(action != null);

            return () => {
                if (count > 0) {
                    count--;
                    action();
                }
            };
        }

        #endregion
    }
}
