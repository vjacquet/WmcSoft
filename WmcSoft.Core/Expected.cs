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

namespace WmcSoft
{
    /// <summary>
    /// Contains a value or an exception explaining why the value is missing.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public struct Expected<T>
    {
        #region Fields

        readonly T _value;
        readonly Exception _exception;

        #endregion

        #region Lifecycle

        public Expected(T value) {
            _value = value;
            _exception = null;
        }

        public Expected(Exception exception) {
            _value = default(T);
            _exception = exception;
        }

        #endregion

        #region Operators

        public static implicit operator Expected<T>(T value) {
            return new Expected<T>(value);
        }
        public static explicit operator T(Expected<T> expected) {
            return expected.Value;
        }

        public static implicit operator Expected<T>(Exception exception) {
            return new Expected<T>(exception);
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
        /// <exception cref="System.Exception">Throws the stored exception when the value is missing.</exception>
        public T Value {
            get {
                if (_exception != null)
                    throw _exception;
                return _value;
            }
        }

        public T GetValueOrDefault() {
            return _value;
        }
        public T GetValueOrDefault(T defaultValue) {
            return HasValue ? _value : defaultValue;
        }

        /// <summary>
        /// The exception or null.
        /// </summary>
        public Exception Exception {
            get { return _exception; }
        }

        #endregion

        #region Methods

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
                var that = (Expected<T>)other;
                if (_exception != null)
                    return _exception.Equals(that.Exception);
                return Object.Equals(_value, that._value);
            }
            if (_exception != null && _exception.Equals(other))
                return true;
            if (other.Equals(_value))
                return true;
            return false;
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
        public static Expected<TResult> Apply<T1, T2, TResult>(Expected<T1> first, Expected<T2> second, Func<T1, T2, TResult> func) {
            if (first.IsFaulted)
                return first.Exception;
            if (second.IsFaulted)
                return second.Exception;
            try {
                return func(first.GetValueOrDefault(), second.GetValueOrDefault());
            }
            catch (Exception exception) {
                return exception;
            }
        }

        public static Expected<TResult> Apply<T1, T2, T3, TResult>(Expected<T1> first, Expected<T2> second, Expected<T3> third, Func<T1, T2, T3, TResult> func) {
            if (first.IsFaulted)
                return first.Exception;
            if (second.IsFaulted)
                return second.Exception;
            if (third.IsFaulted)
                return third.Exception;
            try {
                return func(first.GetValueOrDefault(), second.GetValueOrDefault(), third.GetValueOrDefault());
            }
            catch (Exception exception) {
                return exception;
            }
        }

        public static Expected<TResult> Apply<T1, T2, T3, T4, TResult>(Expected<T1> first, Expected<T2> second, Expected<T3> third, Expected<T4> fourth, Func<T1, T2, T3, T4, TResult> func) {
            if (first.IsFaulted)
                return first.Exception;
            if (second.IsFaulted)
                return second.Exception;
            if (third.IsFaulted)
                return third.Exception;
            if (fourth.IsFaulted)
                return fourth.Exception;
            try {
                return func(first.GetValueOrDefault(), second.GetValueOrDefault(), third.GetValueOrDefault(), fourth.GetValueOrDefault());
            }
            catch (Exception exception) {
                return exception;
            }
        }
    }
}