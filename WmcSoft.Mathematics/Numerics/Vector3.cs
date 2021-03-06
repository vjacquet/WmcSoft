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
using System.Collections.Generic;

namespace WmcSoft.Numerics
{
    /// <summary>
    /// Represents a vector of 3 <see cref="double"/>s.
    /// </summary>
    [Serializable]
    public struct Vector3 : IEquatable<Vector3>, IReadOnlyList<double>, IFormattable
    {
        const int N = 3;

        public readonly static Vector3 Zero = new Vector3(NumericsUtilities.Uninitialized);
        public readonly static Vector3 One = new Vector3(1, 1, 1);

        #region Fields

        internal readonly double[] _data;

        #endregion

        #region Lifecycle

        private Vector3(NumericsUtilities.UninitializedTag tag)
        {
            _data = new double[N];
        }

        public Vector3(Func<int, double> generator)
            : this(generator(0), generator(1), generator(2))
        {
        }

        public Vector3(double x = 0d, double y = 0d, double z = 0d)
        {
            _data = new[] { x, y, z };
        }

        #endregion

        #region Properties

        public int Rank { get { return 1; } }
        public int Cardinality { get { return N; } }
        public double this[int index] { get { return _data[index]; } }

        #endregion

        #region Operators

        public static explicit operator double[] (Vector3 x)
        {
            var data = x._data ?? Zero._data;
            return (double[])data.Clone();
        }
        public static double[] ToArray(Vector3 x)
        {
            return (double[])x;
        }

        public static Vector3 operator +(Vector3 x, Vector3 y)
        {
            if (x._data == null)
                return y;
            if (y._data == null)
                return x;

            var result = new Vector3(x._data[0] + y._data[0], x._data[1] + y._data[1], x._data[2] + y._data[2]);
            return result;
        }
        public static Vector3 Add(Vector3 x, Vector3 y)
        {
            return x + y;
        }

        public static Vector3 operator -(Vector3 x, Vector3 y)
        {
            if (x._data == null)
                return -y;
            if (y._data == null)
                return x;

            var result = new Vector3(x._data[0] - y._data[0], x._data[1] - y._data[1], x._data[2] - y._data[2]);
            return result;
        }
        public static Vector3 Subtract(Vector3 x, Vector3 y)
        {
            return x - y;
        }

        public static Vector3 operator *(double alpha, Vector3 x)
        {
            if (x._data == null)
                return x;

            var result = new Vector3(alpha * x._data[0], alpha * x._data[1], alpha * x._data[2]);
            return result;
        }
        public static Vector3 Multiply(double alpha, Vector3 x)
        {
            return alpha * x;
        }
        public static Vector3 operator *(Vector3 x, double alpha)
        {
            return alpha * x;
        }
        public static Vector3 Multiply(Vector3 x, double alpha)
        {
            return alpha * x;
        }

        public static Vector3 operator /(Vector3 x, double alpha)
        {
            if (x._data == null)
                return x;

            var result = new Vector3(x._data[0] / alpha, x._data[1] / alpha, x._data[2] / alpha);
            return result;
        }
        public static Vector3 Divide(Vector3 x, double alpha)
        {
            return x / alpha;
        }

        public static Vector3 operator -(Vector3 x)
        {
            if (x._data == null)
                return x;

            var result = new Vector3(-x._data[0], -x._data[1], -x._data[2]);
            return result;
        }
        public static Vector3 Negate(Vector3 x)
        {
            return -x;
        }

        public static Vector3 operator +(Vector3 x)
        {
            return x;
        }
        public static Vector3 Plus(Vector3 x)
        {
            return x;
        }

        public static double DotProduct(Vector3 x, Vector3 y)
        {
            if (x._data == null)
                return 0d;
            if (y._data == null)
                return 0d;

            var result = 0d;
            for (int i = 0; i < N; i++) {
                result += x._data[i] * y._data[i];
            }
            return result;
        }

        public static bool operator ==(Vector3 x, Vector3 y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Vector3 x, Vector3 y)
        {
            return !x.Equals(y);
        }

        #endregion

        #region IEquatable<Vector3> Membres

        public bool Equals(Vector3 other)
        {
            var x = _data;
            var y = _data;
            if (_data == null) {
                if (other._data == null)
                    return true;
                x = Zero._data;
            } else if (other._data == null) {
                y = Zero._data;
            }
            if (ReferenceEquals(x, y))
                return true;
            return x[0].Equals(y[0]) && x[1].Equals(y[1]) && x[2].Equals(y[2]);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Vector3)obj);
        }

        public override int GetHashCode()
        {
            if (_data == null)
                return 0;
            return _data.GetHashCode();
        }

        #endregion

        #region IReadOnlyCollection<double> Members

        int IReadOnlyCollection<double>.Count {
            get { return Cardinality; }
        }

        #endregion

        #region IEnumerable<double> Members

        public IEnumerator<double> GetEnumerator()
        {
            return new StrideEnumerator<double>(_data ?? Zero._data);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        #endregion

        #region IFormattable Membres

        public override string ToString()
        {
            return ToString(null, null);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(null, formatProvider);
        }

        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            return NumericsUtilities.FormatVector(_data ?? Zero._data, format, formatProvider);
        }

        #endregion
    }
}
