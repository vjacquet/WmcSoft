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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Properties;

namespace WmcSoft.Numerics
{
    [Serializable]
    public struct Vector : IEquatable<Vector>, IReadOnlyList<double>
    {
        public static Vector Empty;

        #region Fields

        private readonly double[] _data;

        #endregion

        #region Lifecycle

        public Vector(int n) {
            _data = new double[n];
        }

        public Vector(int n, Func<int, double> generator) {
            _data = new double[n];
            for (int i = 0; i < n; i++) {
                _data[i] = generator(i);
            }
        }

        public Vector(params double[] values) {
            _data = (double[])values.Clone();
        }

        #endregion

        #region Properties

        public int Rank { get { return _data == null ? 0 : 1; } }
        public int Cardinality { get { return _data == null ? 0 : _data.Length; } }
        public double this[int index] { get { return _data[index]; } }

        #endregion

        #region Operators

        public static Vector operator +(Vector x, Vector y) {
            var length = x.Cardinality;
            if (y.Cardinality != length)
                throw new ArgumentException(Resources.VectorsMustHaveSameSizeError);

            var result = new Vector(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = x._data[i] + y._data[i];
            }
            return result;
        }
        public static Vector Add(Vector x, Vector y) {
            return x + y;
        }

        public static Vector operator -(Vector x, Vector y) {
            var length = x.Cardinality;
            if (y.Cardinality != length)
                throw new ArgumentException(Resources.VectorsMustHaveSameSizeError);

            var result = new Vector(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = x._data[i] - y._data[i];
            }
            return result;
        }
        public static Vector Subtract(Vector x, Vector y) {
            return x - y;
        }

        public static Vector operator *(double alpha, Vector x) {
            var length = x.Cardinality;
            var result = new Vector(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = alpha * x._data[i];
            }
            return result;
        }
        public static Vector Multiply(double alpha, Vector x) {
            return alpha * x;
        }
        public static Vector operator *(Vector x, double alpha) {
            return alpha * x;
        }
        public static Vector Multiply(Vector x, double alpha) {
            return alpha * x;
        }

        public static Vector operator /(Vector x, double alpha) {
            var length = x.Cardinality;
            var result = new Vector(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = x._data[i] / alpha;
            }
            return result;
        }
        public static Vector Divide(Vector x, double alpha) {
            return x / alpha;
        }

        public static Vector operator -(Vector x) {
            var length = x.Cardinality;
            var result = new Vector(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = -x._data[i];
            }
            return result;
        }
        public static Vector Negate(Vector x) {
            return -x;
        }

        public static Vector operator +(Vector x) {
            return x;
        }
        public static Vector Plus(Vector x) {
            return x;
        }

        public static double DotProduct(Vector x, Vector y) {
            var length = x.Cardinality;
            if (y.Cardinality != length)
                throw new ArgumentException(Resources.VectorsMustHaveSameSizeError);

            var result = 0d;
            for (int i = 0; i < length; i++) {
                result += x._data[i] * y._data[i];
            }
            return result;
        }

        internal static double DotProductNotEmpty(int length, IEnumerator<double> x, IEnumerator<double> y) {
            x.MoveNext();
            y.MoveNext();

            var result = x.Current * y.Current;
            length--;
            while (length-- > 0) {
                x.MoveNext();
                y.MoveNext();
                result += x.Current * y.Current;
            }
            return result;
        }

        #endregion

        #region IEquatable<Vector> Membres

        public bool Equals(Vector other) {
            var length = Cardinality;
            if (other.Cardinality != length)
                return false;
            for (int i = 0; i < length; i++) {
                if (!_data[i].Equals(other._data[i]))
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Vector)obj);
        }

        public override int GetHashCode() {
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

        IEnumerator<double> IEnumerable<double>.GetEnumerator() {
            var length = Cardinality;
            for (int i = 0; i < length; i++) {
                yield return _data[i];
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _data.GetEnumerator();
        }

        #endregion
    }
}
