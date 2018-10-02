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
    /// Represents the outcome of an operation that does not return a value.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq}")]
    [Serializable]
    public struct OperationResult : ISerializable
    {
        internal static OperationError[] None = new OperationError[0];
        internal static OperationError[] Undefined = null;

        internal readonly OperationError[] errors;

        private OperationResult(OperationError[] errors)
        {
            this.errors = errors;
        }

        /// <summary>
        /// Returns a <see cref="OperationResult"/> indicating a successful operation.
        /// </summary>
        public static OperationResult Success = new OperationResult(None);

        /// <summary>
        /// Indicates whether the operation succeeded or not.
        /// </summary>
        public bool Succeeded => ReferenceEquals(errors, None);

        /// <summary>
        /// Returns an <see cref="OperationResult{T}"/> indicating a successful operation.
        /// </summary>
        /// <typeparam name="T">The type of the value in the operation result.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The operation result.</returns>
        public static OperationResult<T> Returns<T>(T value)
        {
            return new OperationResult<T>(value);
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the result contains errors.
        /// </summary>
        /// <exception cref="InvalidOperationException">The operation result contains errors.</exception>
        public void ThrowIfFailed()
        {
            if (Succeeded)
                return;
            if (ReferenceEquals(errors, Undefined))
                throw new InvalidOperationException();

            var message = string.Join("\r\n", Array.ConvertAll(errors, e => e.Description ?? e.Code));
            var exception = new InvalidOperationException(message);
            exception.Data.Add("Errors", errors);
            throw exception;
        }

        /// <summary>
        /// Errors that occured during the operation.
        /// </summary>
        public ReadOnlyCollection<OperationError> Errors => new ReadOnlyCollection<OperationError>(errors ?? None);

        /// <summary>
        /// Creates a failed <see cref="OperationResult"/> with a list of errors.
        /// </summary>
        /// <param name="errors">The list of errors.</param>
        /// <returns>The result.</returns>
        public static OperationResult Failed(params OperationError[] errors)
        {
            if (errors == null || errors.Length == 0)
                return new OperationResult(null);
            return new OperationResult((OperationError[])errors.Clone());
        }

        public override string ToString()
        {
            if (Succeeded)
                return "Succeeded";
            if (ReferenceEquals(errors, Undefined))
                return "Failed";
            return "Failed: " + string.Join(", ", Array.ConvertAll(errors, e => e.Code));
        }

        #region Serialization

        private OperationResult(SerializationInfo info, StreamingContext context)
        {
            var succeeded = info.GetBoolean("Succeeded");
            errors = succeeded ? None : info.GetValue<OperationError[]>("Errors");
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (Succeeded) {
                info.AddValue("Succeeded", true);
            } else {
                info.AddValue("Succeeded", false);
                info.AddValue("Errors", errors);
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents the outcome of an operation that returns a value.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq}")]
    [Serializable]
    public struct OperationResult<T> : ISerializable
    {
        private OperationResult result;
        private readonly T value;

        public OperationResult(T value)
        {
            result = OperationResult.Success;
            this.value = value;
        }

        private OperationResult(OperationError[] errors)
        {
            result = OperationResult.Failed(errors);
            value = default;
        }

        /// <summary>
        /// Retrieves the value of the current <see cref="OperationResult{T}"/> object, or the object's default value.
        /// </summary>
        /// <returns>The value of the Value property if the <see cref="HasValue"/> property is <c>true</c>; otherwise,
        /// the default value of the current <see cref="OperationResult{T}"/> object.
        /// The type of the default value is the type argument of the current <see cref="OperationResult{T}"/> object,
        /// and the value of the default value consists solely of binary zeroes.</returns>
        /// <remarks>The <see cref="GetValueOrDefault"/> method returns a value even if the <see cref="HasValue"/> property is <c>false</c>
        /// (unlike the <see cref="Value"/> property, which throws an exception).</remarks>
        public T GetValueOrDefault()
        {
            return value;
        }

        /// <summary>
        /// Retrieves the value of the current <see cref="OperationResult{T}"/> object, or the specified default value.
        /// </summary>
        /// <returns>The value of the Value property if the <see cref="HasValue"/> property is <c>true</c>; otherwise,
        /// the <paramref name="defaultValue"/> parameter.
        /// The type of the default value is the type argument of the current <see cref="OperationResult{T}"/> object,
        /// and the value of the default value consists solely of binary zeroes.</returns>
        /// <remarks>The <see cref="GetValueOrDefault"/> method returns a value even if the <see cref="HasValue"/> property is <c>false</c>
        /// (unlike the <see cref="Value"/> property, which throws an exception).</remarks>
        public T GetValueOrDefault(T defaultValue)
        {
            return HasValue ? value : defaultValue;
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="OperationResult{T}"/> object has a valid value of its underlying type.
        /// </summary>
        /// <value><c>true</c> if the current <see cref="OperationResult{T}"/> object has a value; <c>false</c> if the current<see cref="OperationResult{T}"/> object has no value.</value>
        public bool HasValue => Succeeded;

        /// <summary>
        /// Gets the value of the current <see cref="OperationResult{T}"/> object if it has been assigned a valid underlying value.
        /// </summary>
        /// <value></value>
        /// <exception cref="InvalidOperationException">The <see cref="HasValue"/> property is <c>false</c>.</exception>
        public T Value {
            get {
                result.ThrowIfFailed();
                return value;
            }
        }

        /// <summary>
        /// Indicates whether the operation succeeded or not.
        /// </summary>
        public bool Succeeded => result.Succeeded;

        public static explicit operator T(OperationResult<T> result)
        {
            return result.Value;
        }

        public static implicit operator OperationResult<T>(T value)
        {
            return new OperationResult<T>(value);
        }

        /// <summary>
        /// Converts a <see cref="OperationResult"/> to a <see cref="OperationResult{T}"/>. If the <paramref name="result"/> result was successfull,
        /// the returned value contains the type's default value.
        /// </summary>
        /// <param name="result">The <see cref="OperationResult"/> to convert.</param>
        /// <remarks>This conversion is useful to convert failed results.</remarks>
        public static implicit operator OperationResult<T>(OperationResult result)
        {
            return new OperationResult<T>(result.errors);
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the result contains errors.
        /// </summary>
        /// <exception cref="InvalidOperationException">The operation result contains errors.</exception>

        public void ThrowIfFailed()
        {
            result.ThrowIfFailed();
        }

        /// <summary>
        /// Errors that occured during the operation.
        /// </summary>
        public ReadOnlyCollection<OperationError> Errors => result.Errors;

        /// <summary>
        /// Creates a failed <see cref="OperationResult"/> with a list of errors.
        /// </summary>
        /// <param name="errors">The list of errors.</param>
        /// <returns>The result.</returns>
        public static OperationResult<T> Failed(params OperationError[] errors)
        {
            if (errors == null || errors.Length == 0)
                return new OperationResult<T>(OperationResult.Undefined);
            return new OperationResult<T>((OperationError[])errors.Clone());
        }

        public override string ToString()
        {
            if (Succeeded)
                return value.ToString();
            return result.ToString();
        }

        #region Serialization

        private OperationResult(SerializationInfo info, StreamingContext context)
        {
            var succeeded = info.GetBoolean("Succeeded");
            if (succeeded) {
                value = info.GetValue<T>("Value");
                result = OperationResult.Success;
            } else {
                result = OperationResult.Failed(info.GetValue<OperationError[]>("Errors"));
                value = default;
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (Succeeded) {
                info.AddValue("Succeeded", true);
                info.AddValue("Value", value);
            } else {
                ((ISerializable)result).GetObjectData(info, context);
            }
        }

        #endregion
    }
}
