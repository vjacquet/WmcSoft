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

namespace WmcSoft.Diagnostics
{
    /// <summary>
    /// Utility class useful for testing exceptions and combining them, possibly using short circuit.
    /// </summary>
    /// <remarks>This class is interresting for demonstrating short circuit with real use cases.</remarks>
    public struct Fault : IEquatable<Fault>
    {
        #region Fields

        public static readonly Fault Empty = default;

        readonly Exception exception;

        #endregion

        #region Lifecycle

        public Fault(Exception exception)
        {
            this.exception = exception;
        }

        public Fault(Exception exception1, Exception exception2)
        {
            exception = new AggregateException(exception1, exception2);
        }

        public Fault(params Exception[] exceptions)
        {
            exception = new AggregateException(exceptions);
        }

        #endregion

        #region Properties

        public Exception Exception => exception;

        #endregion

        #region Operators

        public static bool operator ==(Fault x, Fault y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Fault x, Fault y)
        {
            return !Equals(x, y);
        }

        public static implicit operator Fault(Exception x)
        {
            return new Fault(x);
        }

        public static bool operator !(Fault x)
        {
            return x.exception == null;
        }

        public static bool operator true(Fault x)
        {
            return IsTrue(x);
        }

        public static bool operator false(Fault x)
        {
            return !IsTrue(x);
        }

        public static bool IsTrue(Fault x)
        {
            return x.exception != null;
        }

        public static Fault operator &(Fault x, Fault y)
        {
            if (x.exception != null && y.exception != null)
                return new Fault(x.exception, y.exception);
            return default;
        }

        public static Fault operator |(Fault x, Fault y)
        {
            if (x.exception != null) {
                if (y.exception != null)
                    return new Fault(x.exception, y.exception);
                return x;
            }
            if (y.exception != null)
                return y;
            return default;
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Exception exception)
                return exception.Equals(this.exception);

            return obj is Fault && Equals((Fault)obj);
        }

        public override int GetHashCode()
        {
            if (exception != null)
                return exception.GetHashCode();
            return 0;
        }

        #endregion

        #region IEquatable<T> Members

        public bool Equals(Fault other)
        {
            return Equals(exception, other.exception);
        }

        #endregion
    }
}
