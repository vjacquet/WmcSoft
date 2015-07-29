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
using System.Globalization;
using System.Runtime.InteropServices;

namespace WmcSoft.AI.FuzzyLogic
{
    /// <summary>
    /// Description résumée de FuzzyVar.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct FuzzyVar : IComparable, IFormattable, IConvertible, IComparable<FuzzyVar>, IEquatable<FuzzyVar>
    {
        public const double MinValue = 0;
        public const double Epsilon = Double.Epsilon;
        public const double MaxValue = 1;
        public const double NaN = Double.NaN;

        internal double _value;

        public FuzzyVar(double value) {
            if (value < 0.0 || value > 1.0) {
                throw new ArgumentOutOfRangeException("value");
            }
            this._value = value;
        }

        public static bool IsNaN(FuzzyVar x) {
            return Double.IsNaN(x._value);
        }

        public int CompareTo(object obj) {
            if (obj == null) {
                return 1;
            }
            if (obj is FuzzyVar) {
                FuzzyVar fuzzyVar = (FuzzyVar)obj;
                return CompareTo(fuzzyVar);
            }
            throw new InvalidCastException();
        }

        public override bool Equals(object obj) {
            if (obj is FuzzyVar) {
                FuzzyVar fuzzyVar = (FuzzyVar)obj;
                if (fuzzyVar._value == this._value) {
                    return true;
                }
                if (FuzzyVar.IsNaN(fuzzyVar)) {
                    return FuzzyVar.IsNaN(this);
                }
            }
            return false;
        }

        public override int GetHashCode() {
            return this._value.GetHashCode();
        }

        public override string ToString() {
            return this.ToString(null, null);
        }

        public string ToString(string format) {
            return this.ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            return this._value.ToString(format, NumberFormatInfo.GetInstance(formatProvider));
        }

        public static FuzzyVar Parse(string text) {
            double value = Double.Parse(text, NumberStyles.Float | NumberStyles.AllowThousands, null);
            if (value < 0.0f && value > 1.0f) {
                throw new ArgumentOutOfRangeException("text");
            }
            return new FuzzyVar(value);
        }

        public static FuzzyVar Parse(string text, NumberStyles style) {
            double value = Double.Parse(text, style, null);
            if (value < 0.0f && value > 1.0f) {
                throw new ArgumentOutOfRangeException("text");
            }
            return new FuzzyVar(value);
        }

        public static FuzzyVar Parse(string text, IFormatProvider provider) {
            double value = Double.Parse(text, NumberStyles.Float | NumberStyles.AllowThousands, provider);
            if (value < 0.0f && value > 1.0f) {
                throw new ArgumentOutOfRangeException("text");
            }
            return new FuzzyVar(value);
        }

        public static FuzzyVar Parse(string text, NumberStyles style, IFormatProvider provider) {
            double value = Double.Parse(text, style, provider);
            if (value < 0.0f && value > 1.0f) {
                throw new ArgumentOutOfRangeException("text");
            }
            return new FuzzyVar(value);
        }

        public string ToString(IFormatProvider provider) {
            return this.ToString(null, provider);
        }

        public TypeCode GetTypeCode() {
            return TypeCode.Double;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider) {
            return Convert.ToBoolean(this._value);
        }

        char IConvertible.ToChar(IFormatProvider provider) {
            throw new InvalidCastException();
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider) {
            return Convert.ToSByte(this._value);
        }
        byte IConvertible.ToByte(IFormatProvider provider) {
            return Convert.ToByte(this._value);
        }

        short IConvertible.ToInt16(IFormatProvider provider) {
            return Convert.ToInt16(this._value);
        }

        //[CLSCompliant(false)]
        ushort IConvertible.ToUInt16(IFormatProvider provider) {
            return Convert.ToUInt16(this._value);
        }

        int IConvertible.ToInt32(IFormatProvider provider) {
            return Convert.ToInt32(this._value);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider) {
            return Convert.ToUInt32(this._value);
        }

        long IConvertible.ToInt64(IFormatProvider provider) {
            return Convert.ToInt64(this._value);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider) {
            return Convert.ToUInt64(this._value);
        }

        float IConvertible.ToSingle(IFormatProvider provider) {
            return Convert.ToSingle(this._value);
        }

        double IConvertible.ToDouble(IFormatProvider provider) {
            return this._value;
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider) {
            return Convert.ToDecimal(this._value);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) {
            throw new InvalidCastException();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) {
            return ((IConvertible)_value).ToType(conversionType, provider);
        }

        public static FuzzyVar operator &(FuzzyVar x, FuzzyVar y) {
            return new FuzzyVar(System.Math.Min(x, y));
        }

        public FuzzyVar BitwiseAnd(FuzzyVar value) {
            return this & value;
        }

        public static FuzzyVar operator |(FuzzyVar x, FuzzyVar y) {
            return new FuzzyVar(System.Math.Max(x, y));
        }

        public FuzzyVar BitwiseOr(FuzzyVar value) {
            return this | value;
        }

        public static explicit operator FuzzyVar(double value) {
            return new FuzzyVar(value);
        }

        public static implicit operator double(FuzzyVar value) {
            return value._value;
        }

        public static bool operator ==(FuzzyVar x, FuzzyVar y) {
            return x._value.CompareTo(y._value) == 0;
        }

        public static bool operator !=(FuzzyVar x, FuzzyVar y) {
            return x._value.CompareTo(y._value) != 0;
        }

        public static bool operator <(FuzzyVar x, FuzzyVar y) {
            return x._value.CompareTo(y._value) < 0;
        }

        public static bool operator >(FuzzyVar x, FuzzyVar y) {
            return x._value.CompareTo(y._value) > 0;
        }

        #region IComparable<FuzzyVar> Members

        public int CompareTo(FuzzyVar other) {
            if (this._value < other._value) {
                return -1;
            }
            if (this._value > other._value) {
                return 1;
            }
            if (this._value != other._value) {
                if (!FuzzyVar.IsNaN(this)) {
                    return 1;
                }
                if (!FuzzyVar.IsNaN(other)) {
                    return -1;
                }
            }
            return 0;
        }

        #endregion

        #region IEquatable<FuzzyVar> Members

        public bool Equals(FuzzyVar other) {
            return (CompareTo(other) == 0);
        }

        #endregion
    }

}
