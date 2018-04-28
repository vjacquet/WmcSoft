#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using WmcSoft.Runtime.Serialization;

namespace WmcSoft
{
    /// <summary>
    /// Represents the outcome of a gateway operation that does not return a value.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq}")]
    [Serializable]
    public struct GatewayResult : ISerializable
    {
        internal static GatewayError[] None = new GatewayError[0];
        internal static GatewayError[] Undefined = new GatewayError[0];

        internal readonly GatewayError[] _errors;

        private GatewayResult(GatewayError[] errors)
        {
            _errors = errors;
        }

        /// <summary>
        /// Returns a <see cref="GatewayResult"/> indicating a successful operation.
        /// </summary>
        public static GatewayResult Success = new GatewayResult(None);

        /// <summary>
        /// Indicates whether the operation succeeded or not.
        /// </summary>
        public bool Succeeded {
            get { return ReferenceEquals(_errors, None); }
        }

        /// <summary>
        /// Returns an <see cref="GatewayResult{T}"/> indicating a successful operation.
        /// </summary>
        /// <typeparam name="T">The type of the value in the operation result.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The operation result.</returns>
        public static GatewayResult<T> Returns<T>(T value)
        {
            return new GatewayResult<T>(value);
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the result contains errors.
        /// </summary>
        /// <exception cref="InvalidOperationException">The operation result contains errors.</exception>
        public void ThrowIfFailed()
        {
            if (Succeeded)
                return;
            if (ReferenceEquals(_errors, null))
                throw new InvalidOperationException();

            var message = string.Join("\r\n", Array.ConvertAll(_errors, e => e.Description ?? e.Code));
            var exception = new InvalidOperationException(message);
            exception.Data.Add("Errors", _errors);
            throw exception;
        }

        /// <summary>
        /// Errors that occured during the operation.
        /// </summary>
        public ReadOnlyCollection<GatewayError> Errors {
            get { return new ReadOnlyCollection<GatewayError>(_errors); }
        }

        /// <summary>
        /// Creates a failed <see cref="GatewayResult"/> with a list of errors.
        /// </summary>
        /// <param name="errors">The list of errors.</param>
        /// <returns>The result.</returns>
        public static GatewayResult Failed(params GatewayError[] errors)
        {
            if (errors == null || errors.Length == 0)
                return new GatewayResult(Undefined);
            return new GatewayResult((GatewayError[])errors.Clone());
        }

        public override string ToString()
        {
            if (Succeeded)
                return "Succeeded";
            if (_errors.Length == 0)
                return "Failed";
            return "Failed: " + string.Join(", ", Array.ConvertAll(_errors, e => e.Code));
        }

        #region Serialization

        private GatewayResult(SerializationInfo info, StreamingContext context)
        {
            var succeeded = info.GetBoolean("Succeeded");
            _errors = succeeded ? None : info.GetValue<GatewayError[]>("Errors");
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (Succeeded) {
                info.AddValue("Succeeded", true);
            } else {
                info.AddValue("Succeeded", false);
                info.AddValue("Errors", _errors);
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents the outcome of a gateway operation that returns a value.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq}")]
    [Serializable]
    public struct GatewayResult<T> : ISerializable
    {
        private GatewayResult _result;
        private readonly T _value;

        public GatewayResult(T value)
        {
            _result = GatewayResult.Success;
            _value = value;
        }

        private GatewayResult(GatewayError[] errors)
        {
            _result = GatewayResult.Failed(errors);
            _value = default(T);
        }

        /// <summary>
        /// Retrieves the value of the current <see cref="GatewayResult{T}"/> object, or the object's default value.
        /// </summary>
        /// <returns>The value of the Value property if the <see cref="HasValue"/> property is <c>true</c>; otherwise,
        /// the default value of the current <see cref="GatewayResult{T}"/> object.
        /// The type of the default value is the type argument of the current <see cref="GatewayResult{T}"/> object,
        /// and the value of the default value consists solely of binary zeroes.</returns>
        /// <remarks>The <see cref="GetValueOrDefault"/> method returns a value even if the <see cref="HasValue"/> property is <c>false</c>
        /// (unlike the <see cref="Value"/> property, which throws an exception).</remarks>
        public T GetValueOrDefault()
        {
            return _value;
        }

        /// <summary>
        /// Retrieves the value of the current <see cref="GatewayResult{T}"/> object, or the specified default value.
        /// </summary>
        /// <returns>The value of the Value property if the <see cref="HasValue"/> property is <c>true</c>; otherwise,
        /// the <paramref name="defaultValue"/> parameter.
        /// The type of the default value is the type argument of the current <see cref="GatewayResult{T}"/> object,
        /// and the value of the default value consists solely of binary zeroes.</returns>
        /// <remarks>The <see cref="GetValueOrDefault"/> method returns a value even if the <see cref="HasValue"/> property is <c>false</c>
        /// (unlike the <see cref="Value"/> property, which throws an exception).</remarks>
        public T GetValueOrDefault(T defaultValue)
        {
            return HasValue ? _value : defaultValue;
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="GatewayResult{T}"/> object has a valid value of its underlying type.
        /// </summary>
        /// <value><c>true</c> if the current <see cref="GatewayResult{T}"/> object has a value; <c>false</c> if the current<see cref="GatewayResult{T}"/> object has no value.</value>
        public bool HasValue => Succeeded;

        /// <summary>
        /// Gets the value of the current <see cref="GatewayResult{T}"/> object if it has been assigned a valid underlying value.
        /// </summary>
        /// <value></value>
        /// <exception cref="InvalidOperationException">The <see cref="HasValue"/> property is <c>false</c>.</exception>
        public T Value {
            get {
                _result.ThrowIfFailed();
                return _value;
            }
        }

        /// <summary>
        /// Indicates whether the operation succeeded or not.
        /// </summary>
        public bool Succeeded {
            get { return _result.Succeeded; }
        }

        public static explicit operator T(GatewayResult<T> result)
        {
            return result.Value;
        }

        public static implicit operator GatewayResult<T>(T value)
        {
            return new GatewayResult<T>(value);
        }

        /// <summary>
        /// Converts a <see cref="GatewayResult"/> to a <see cref="GatewayResult{T}"/>. If the <paramref name="result"/> result was successfull,
        /// the returned value contains the type's default value.
        /// </summary>
        /// <param name="result">The <see cref="GatewayResult"/> to convert.</param>
        /// <remarks>This conversion is useful to convert failed results.</remarks>
        public static implicit operator GatewayResult<T>(GatewayResult result)
        {
            return new GatewayResult<T>(result._errors);
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the result contains errors.
        /// </summary>
        /// <exception cref="InvalidOperationException">The operation result contains errors.</exception>

        public void ThrowIfFailed()
        {
            _result.ThrowIfFailed();
        }

        /// <summary>
        /// Errors that occured during the operation.
        /// </summary>
        public ReadOnlyCollection<GatewayError> Errors => _result.Errors;

        /// <summary>
        /// Creates a failed <see cref="GatewayResult"/> with a list of errors.
        /// </summary>
        /// <param name="errors">The list of errors.</param>
        /// <returns>The result.</returns>
        public static GatewayResult<T> Failed(params GatewayError[] errors)
        {
            if (errors == null || errors.Length == 0)
                return new GatewayResult<T>(GatewayResult.Undefined);
            return new GatewayResult<T>((GatewayError[])errors.Clone());
        }

        public override string ToString()
        {
            if (Succeeded)
                return _value.ToString();
            return _result.ToString();
        }

        #region Serialization

        private GatewayResult(SerializationInfo info, StreamingContext context)
        {
            var succeeded = info.GetBoolean("Succeeded");
            if (succeeded) {
                _value = info.GetValue<T>("Value");
                _result = GatewayResult.Success;
            } else {
                _result = GatewayResult.Failed(info.GetValue<GatewayError[]>("Errors"));
                _value = default;
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (Succeeded) {
                info.AddValue("Succeeded", true);
                info.AddValue("Value", _value);
            } else {
                ((ISerializable)_result).GetObjectData(info, context);
            }
        }

        #endregion
    }
}
