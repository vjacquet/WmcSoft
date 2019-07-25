#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Monitoring.Instruments
{
    /// <summary>
    /// Stores a value and the timestamp at which it was produced.
    /// </summary>
    /// <typeparam name="T">The type of value</typeparam>
    [DebuggerDisplay("{Value,nq}@{Timestamp.ToString(\"HH:mm:ss.fff\"), nq}")]
    public struct Timestamped<T> : IEquatable<T>
    {
        public Timestamped(T value, DateTime timestamp)
        {
            Value = value;
            Timestamp = timestamp;
        }

        public void Deconstruct(out T value, out DateTime timestamp)
        {
            value = Value;
            timestamp = Timestamp;
        }

        /// <summary>
        /// The value.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// The date and time when the value was produced.
        /// </summary>
        public DateTime Timestamp { get; }

        #region operators

        public static implicit operator T(Timestamped<T> value)
        {
            return value.Value;
        }

        #endregion

        #region IEquatable<T> members

        bool IEquatable<T>.Equals(T other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other);
        }

        #endregion
    }
}
