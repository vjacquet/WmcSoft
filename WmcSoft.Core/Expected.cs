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
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using WmcSoft.Collections.Generic;
using WmcSoft.Diagnostics;

namespace WmcSoft
{
    /// <summary>
    /// Contains a value or an exception explaining why the value is missing.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <remarks>This type can simplify code when combining results from several functions, or 
    /// for giving a cheap way for the user to handle errors.</remarks>
    [DebuggerDisplay("{ToString(),nq}")]
    public struct Expected<T> : IEquatable<Expected<T>>
    {
        #region Fields

        readonly T _value;
        readonly Exception _exception;

        #endregion

        #region Lifecycle

        /// <summary>
        /// Constructs the expected from a value.
        /// </summary>
        /// <param name="value">The value</param>
        public Expected(T value) {
            _value = value;
            _exception = null;
        }

        /// <summary>
        /// Constructs the expected from an exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public Expected(Exception exception) {
            _value = default(T);
            _exception = exception;
        }

        #endregion

        #region Operators

        public static bool operator ==(Expected<T> x, Expected<T> y) {
            return Equals(x, y);
        }

        public static bool operator !=(Expected<T> x, Expected<T> y) {
            return Equals(x, y);
        }

        public static bool operator !(Expected<T> x) {
            return x.IsFaulted;
        }

        public static bool operator true(Expected<T> x) {
            return x.HasValue;
        }

        public static bool operator false(Expected<T> x) {
            return x.IsFaulted;
        }

        public static implicit operator Expected<T>(T value) {
            return new Expected<T>(value);
        }

        /// <summary>
        /// Converts the <see cref="Expected{T}"/> to its value.
        /// </summary>
        /// <exception cref="Exception">Throws the stored exception when the value is missing.</exception>

        public static explicit operator T(Expected<T> expected) {
            return expected.Value;
        }

        public static implicit operator Expected<T>(Exception exception) {
            return new Expected<T>(exception);
        }

        public static implicit operator Fault(Expected<T> x) {
            return new Fault(x._exception);
        }

        #endregion

        #region Properties

        /// <summary>
        /// True is the Expected contains a value; otherwise, false.
        /// </summary>
        public bool HasValue {
            get { return _exception == null; }
        }
        /// <summary>
        /// True is the Expected does not contain a value; otherwise, false.
        /// </summary>
        public bool IsFaulted {
            get { return _exception != null; }
        }

        /// <summary>
        /// The value.
        /// </summary>
        /// <exception cref="Exception">Throws the stored exception when the value is missing.</exception>
        public T Value {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                if (_exception != null)
                    throw _exception;
                return _value;
            }
        }

        /// <summary>
        /// Retrives the value of the current <see cref="Expected{T}"/> object, or the object's default value.
        /// </summary>
        /// <returns>The value of the <see cref="Value"/> property if the <see cref="HasValue "/> property is <c>true</c>; otherwise, the default value of the current <see cref="Expected{T}"/> object.
        /// The type of the default value is the type argument of the current <see cref="Expected{T}"/> object, and the value of the default value consists solely of binary zeroes.</returns>
        /// <remarks>The <see cref="GetValueOrDefault"/> method returns a value even if the <see cref="HasValue"/> property is <c>false</c> (unlike the <see cref="Value"/> property, which throws an exception). </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValueOrDefault() {
            return _value;
        }

        /// <summary>
        /// Retrives the value of the current <see cref="Expected{T}"/> object, or the object's default value.
        /// </summary>
        /// <returns>The value of the <see cref="Value"/> property if the <see cref="HasValue "/> property is <c>true</c>; otherwise, the <param name="defaultValue"/>.</returns>
        /// <remarks>The <see cref="GetValueOrDefault"/> method returns a value even if the <see cref="HasValue"/> property is <c>false</c> (unlike the <see cref="Value"/> property, which throws an exception). </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValueOrDefault(T defaultValue) {
            return HasValue ? _value : defaultValue;
        }

        /// <summary>
        /// The exception or <c>null</c>.
        /// </summary>
        public Exception Exception {
            get { return _exception; }
        }

        public IEnumerable<Exception> GetExceptions() {
            if (IsFaulted) {
                var aggregated = _exception as AggregateException;
                if (aggregated != null)
                    return aggregated.InnerExceptions;
                return new SingleItemReadOnlyList<Exception>(_exception);
            }
            return Enumerable.Empty<Exception>();
        }

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Exception PassThrough(Exception exception, Func<Exception, Exception> translate) {
            if (translate == null)
                return exception;

            try {
                return translate(exception);
            }
            catch (Exception) {
                return exception; // do not let the janitor destroy the crime scene
            }
        }

        public Expected<TResult> Apply<TResult>(Func<T, TResult> func, Func<Exception, Exception> translate) {
            if (IsFaulted)
                return PassThrough(_exception, translate);

            try {
                return func(_value);
            }
            catch (Exception e) {
                return PassThrough(e, translate);
            }
        }

        public Expected<TResult> Apply<TResult>(Func<T, TResult> func) {
            if (IsFaulted)
                return _exception;

            try {
                return func(_value);
            }
            catch (Exception e) {
                return e;
            }
        }

        #endregion

        #region Overrides

        public override bool Equals(object other) {
            if (other == null)
                return false;
            if (other.GetType() == GetType()) {
                return Equals((Expected<T>)other);
            }
            if (_exception != null && _exception.Equals(other))
                return true;
            if (other.Equals(_value))
                return true;
            return false;
        }

        public bool Equals(Expected<T> other) {
            if (_exception != null)
                return _exception.Equals(other._exception);
            return Object.Equals(_value, other._value);
        }


        public override int GetHashCode() {
            if (_exception != null)
                return _exception.GetHashCode();
            if (Object.ReferenceEquals(_value, null))
                return 0;
            return _value.GetHashCode();
        }

        public override string ToString() {
            return HasValue ? _value.ToString() : (@"/!\ " + _exception.Message);
        }

        #endregion
    }

    public static class Expected
    {
        #region Factory functions

        public static Expected<T> Success<T>(T value) {
            return value;
        }
        public static Expected<T> Failed<T>(Exception exception) {
            return exception;
        }

        #endregion

        #region Functional helpers

        static Exception Compose(Exception x, Exception y) {
            if (x == null)
                return y;
            if (y == null)
                return x;
            return new AggregateException(x, y);
        }

        static Exception Compose(params Exception[] exceptions) {
            if (exceptions == null)
                return null;
            var x = exceptions.Where(e => e != null).ToList();
            switch (x.Count) {
            case 0: return null;
            case 1: return x[0];
            default: return new AggregateException(x);
            }
        }

        public static Expected<TResult> Apply<T1, T2, TResult>(Func<T1, T2, TResult> func, Expected<T1> first = default(Expected<T1>), Expected<T2> second = default(Expected<T2>)) {
            var x = Compose(first.Exception, second.Exception);
            if (x != null)
                return x;

            try {
                return func(first.GetValueOrDefault(), second.GetValueOrDefault());
            }
            catch (Exception exception) {
                return exception;
            }
        }

        public static Expected<TResult> Apply<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, Expected<T1> first = default(Expected<T1>), Expected<T2> second = default(Expected<T2>), Expected<T3> third = default(Expected<T3>)) {
            var x = Compose(first.Exception, second.Exception, third.Exception);
            if (x != null)
                return x;

            try {
                return func(first.GetValueOrDefault(), second.GetValueOrDefault(), third.GetValueOrDefault());
            }
            catch (Exception exception) {
                return exception;
            }
        }

        public static Expected<TResult> Apply<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, Expected<T1> first = default(Expected<T1>), Expected<T2> second = default(Expected<T2>), Expected<T3> third = default(Expected<T3>), Expected<T4> fourth = default(Expected<T4>)) {
            var x = Compose(first.Exception, second.Exception, third.Exception, fourth.Exception);
            if (x != null)
                return x;

            try {
                return func(first.GetValueOrDefault(), second.GetValueOrDefault(), third.GetValueOrDefault(), fourth.GetValueOrDefault());
            }
            catch (Exception exception) {
                return exception;
            }
        }

        #endregion
    }
}
