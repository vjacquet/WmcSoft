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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Properties;

namespace WmcSoft.Numerics
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>See [http://en.wikipedia.org/wiki/Matrix_(mathematics)]</remarks>
    [Serializable]
    public struct Matrix : IEquatable<Matrix>
    {
        class Storage
        {
            public readonly double[] data;
            public readonly int m;
            public readonly int n;

            public Storage(int m, int n) {
                this.m = m;
                this.n = n;
                data = new double[m * n];
            }

            public Dimensions Size { get { return data == null ? Dimensions.Empty : new Dimensions(m, n); } }
        }

        #region Fields

        private readonly Storage _storage;

        #endregion

        #region Lifecycle

        public Matrix(int n)
            : this(n, n) {
        }

        public Matrix(int m, int n) {
            _storage = new Storage(m, n);
        }

        public Matrix(int m, int n, Func<int, int, double> generator) {
            _storage = new Storage(m, n);
            var index = 0;
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    _storage.data[index++] = generator(i, j);
                }
            }
        }

        public Matrix(int m, int n, double value)
            : this(m, n) {
            var length = _storage.data.Length;
            for (int i = 0; i < length; i++) {
                _storage.data[i] = value;
            }
        }

        public static Matrix Identity(int n) {
            var matrix = new Matrix(n);
            var length = matrix._storage.data.Length;
            for (int i = 0; i < length; i += n) {
                matrix._storage.data[i] = 1d;
            }
            return matrix;
        }

        #endregion

        #region Properties

        public int Rank { get { return _storage.data == null ? 0 : 2; } }
        public int Cardinality { get { return _storage.data == null ? 0 : _storage.data.Length; } }
        public Dimensions Size { get { return _storage.Size; } }
        public double this[int i, int j] { get { return _storage.data[i * _storage.n + j]; } }

        #endregion

        #region Operators

        public static Matrix operator +(Matrix x, Matrix y) {
            if (x.Size != y.Size)
                throw new ArgumentException(Resources.MatricesMustHaveTheSameSizeError);

            var result = new Matrix(x._storage.m, x._storage.n);
            var length = result._storage.data.Length;
            for (int i = 0; i < length; i++) {
                result._storage.data[i] = y._storage.data[i] + y._storage.data[i];
            }
            return result;
        }
        public static Matrix Add(Matrix x, Matrix y) {
            return x + y;
        }

        public static Matrix operator -(Matrix x, Matrix y) {
            if (x.Size != y.Size)
                throw new ArgumentException(Resources.MatricesMustHaveTheSameSizeError);

            var result = new Matrix(x._storage.m, x._storage.n);
            var length = result._storage.data.Length;
            for (int i = 0; i < length; i++) {
                result._storage.data[i] = y._storage.data[i] - y._storage.data[i];
            }
            return result;
        }
        public static Matrix Subtract(Matrix x, Matrix y) {
            return x - y;
        }

        public static Matrix operator -(Matrix x) {
            var length = x.Cardinality;
            var result = new Matrix(length);
            for (int i = 0; i < length; i++) {
                result._storage.data[i] = -x._storage.data[i];
            }
            return result;
        }
        public static Matrix Negate(Matrix x) {
            return -x;
        }

        public static Matrix operator +(Matrix x) {
            return x;
        }
        public static Matrix Plus(Matrix x) {
            return x;
        }

        #endregion

        #region IEquatable<Matrix> Membres

        public bool Equals(Matrix other) {
            if (Size != other.Size)
                return false;
            var length = _storage.data.Length;
            for (int i = 0; i < length; i++) {
                if (!_storage.data[i].Equals(other._storage.data[i]))
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
            if (_storage.data == null)
                return 0;
            return _storage.data.GetHashCode();
        }

        #endregion
    }
}
