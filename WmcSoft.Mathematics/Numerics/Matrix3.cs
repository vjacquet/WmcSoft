using System;
using System.Collections.Generic;

namespace WmcSoft.Numerics
{
    /// <summary>
    /// Represents a 3x3 matrix of <see cref="double"/>s.
    /// </summary>
    [Serializable]
    public struct Matrix3 : IEquatable<Matrix3>
    {
        const int N = 3;

        public readonly static Matrix3 Zero = new Matrix3(NumericsUtilities.Uninitialized);
        public readonly static Matrix3 Identity = new Matrix3(1, 0, 0, 0, 1, 0, 0, 0, 1);

        #region Fields

        private readonly double[] _storage;

        #endregion

        #region Lifecycle

        private Matrix3(NumericsUtilities.UninitializedTag tag) {
            _storage = new double[N * N];
        }

        private Matrix3(params double[] values) {
            _storage = new double[N * N];
            Array.Copy(values, _storage, Math.Min(N * N, values.Length));
        }

        public Matrix3(double[,] values) {
            if (values.GetLength(0) != N || values.GetLength(1) != N)
                throw new ArgumentException("values");

            _storage = new double[N * N];
            var k = 0;
            foreach (var value in values) {
                _storage[k++] = value;
            }
        }

        public Matrix3(double value) {
            _storage = new double[N * N];
            for (int i = 0; i < _storage.Length; i++) {
                _storage[i] = value;
            }
        }

        public Matrix3(Func<int, int, double> generator) {
            _storage = new double[N * N];
            var index = 0;
            for (int i = 0; i < N; i++) {
                for (int j = 0; j < N; j++) {
                    _storage[index++] = generator(i, j);
                }
            }
        }

        public Matrix3 Transpose() {
            var result = new Matrix3(0d);
            var length = _storage.Length - N; // to avoid overflow for huge matrices
            var k = 0;
            for (var j = 0; j < N; j++, length++) {
                for (var i = j; i <= length; i += N, k++) {
                    result._storage[i] = _storage[k];
                }
            }
            return result;
        }

        #endregion

        #region Properties

        public int Rank { get { return 2; } }
        public int Cardinality { get { return N * N; } }
        public Dimensions Size { get { return new Dimensions(N, N); } }
        public double this[int i, int j] { get { return _storage[i * N + j]; } }

        public IReadOnlyList<double> Row(int i) {
            if (_storage == null)
                return new StrideEnumerable<double>(Vector3.Zero._data);

            var k = i * N;
            return new StrideEnumerable<double>(_storage, k, N, 1);
        }

        public IReadOnlyList<double> Column(int j) {
            if (_storage == null)
                return new StrideEnumerable<double>(Vector3.Zero._data);

            return new StrideEnumerable<double>(_storage, j, N, N);
        }

        #endregion

        #region Methods

        public double Det() {
            return _storage[0] * (_storage[4] * _storage[8] - _storage[5] * _storage[7])
                - _storage[1] * (_storage[3] * _storage[8] - _storage[5] * _storage[6])
                + _storage[2] * (_storage[3] * _storage[7] - _storage[6] * _storage[4]);
        }

        public Matrix3 Inverse() {
            Matrix3 result;
            if (!TryInverse(out result))
                throw new InvalidOperationException("Cannot inverse matrix");
            return result;
        }

        public bool TryInverse(out Matrix3 m) {
            var result = new Matrix3(NumericsUtilities.Uninitialized);
            var storage = result._storage;
            storage[0] = (_storage[4] * _storage[8] - _storage[5] * _storage[7]);
            storage[3] = -(_storage[3] * _storage[8] - _storage[5] * _storage[6]);
            storage[6] = (_storage[3] * _storage[7] - _storage[6] * _storage[4]);

            var det = _storage[0] * storage[0] + _storage[1] * storage[3] + _storage[2] * storage[6];
            if (det == 0d) {
                m = default(Matrix3);
                return false;
            }

            storage[1] = -(_storage[1] * _storage[8] - _storage[7] * _storage[2]);
            storage[2] = (_storage[1] * _storage[5] - _storage[4] * _storage[2]);
            storage[4] = (_storage[0] * _storage[8] - _storage[6] * _storage[2]);
            storage[5] = -(_storage[0] * _storage[5] - _storage[3] * _storage[2]);
            storage[7] = -(_storage[0] * _storage[7] - _storage[6] * _storage[1]);
            storage[8] = (_storage[0] * _storage[4] - _storage[3] * _storage[1]);

            m = result / det;
            return true;
        }

        #endregion

        #region Operators

        public static explicit operator double[,] (Matrix3 x) {
            if (x._storage == null)
                return new double[N, N];
            var result = new double[N, N];
            var k = 0;
            for (var i = 0; i < N; i++) {
                for (var j = 0; j < N; j++) {
                    result[i, j] = x._storage[k++];
                }
            }
            return result;
        }
        public static double[,] ToArray(Matrix3 x) {
            return (double[,])x;
        }

        public static Matrix3 operator +(Matrix3 x, Matrix3 y) {
            var result = new Matrix3(NumericsUtilities.Uninitialized);
            var length = result._storage.Length;
            for (int i = 0; i < length; i++) {
                result._storage[i] = y._storage[i] + y._storage[i];
            }
            return result;
        }
        public static Matrix3 Add(Matrix3 x, Matrix3 y) {
            return x + y;
        }

        public static Matrix3 operator -(Matrix3 x, Matrix3 y) {
            var result = new Matrix3(NumericsUtilities.Uninitialized);
            var length = result.Cardinality;
            for (int i = 0; i < length; i++) {
                result._storage[i] = y._storage[i] - y._storage[i];
            }
            return result;
        }
        public static Matrix3 Subtract(Matrix3 x, Matrix3 y) {
            return x - y;
        }

        public static Matrix3 operator *(Matrix3 x, Matrix3 y) {
            var k = 0;
            var result = new Matrix3(NumericsUtilities.Uninitialized);
            for (int j = 0; j < N; j++) {
                for (int i = 0; i < N; i++) {
                    result._storage[k++] = Vector.DotProductNotEmpty(N, x.Row(i).GetEnumerator(), y.Column(j).GetEnumerator());
                }
            }
            return result;
        }
        public static Matrix3 Multiply(Matrix3 x, Matrix3 y) {
            return x * y;
        }

        public static Matrix3 operator -(Matrix3 x) {
            var length = x.Cardinality;
            var result = new Matrix3();
            for (int i = 0; i < length; i++) {
                result._storage[i] = -x._storage[i];
            }
            return result;
        }
        public static Matrix3 Negate(Matrix3 x) {
            return -x;
        }

        public static Matrix3 operator +(Matrix3 x) {
            return x;
        }
        public static Matrix3 Plus(Matrix3 x) {
            return x;
        }

        public static Matrix3 operator *(double scalar, Matrix3 matrix) {
            var length = matrix.Cardinality;
            var result = new Matrix3(NumericsUtilities.Uninitialized);
            for (int i = 0; i < length; i++) {
                result._storage[i] = scalar * matrix._storage[i];
            }
            return result;
        }
        public static Matrix3 Multiply(double scalar, Matrix3 matrix) {
            return scalar * matrix;
        }

        public static Matrix3 operator *(Matrix3 matrix, double scalar) {
            var length = matrix.Cardinality;
            var result = new Matrix3(NumericsUtilities.Uninitialized);
            for (int i = 0; i < length; i++) {
                result._storage[i] = scalar * matrix._storage[i];
            }
            return result;
        }
        public static Matrix3 Multiply(Matrix3 matrix, double scalar) {
            return matrix * scalar;
        }

        public static Matrix3 operator /(Matrix3 matrix, double scalar) {
            var length = matrix.Cardinality;
            var result = new Matrix3(NumericsUtilities.Uninitialized);
            for (int i = 0; i < length; i++) {
                result._storage[i] = matrix._storage[i] / scalar;
            }
            return result;
        }
        public static Matrix3 Divide(Matrix3 matrix, double scalar) {
            return matrix / scalar;
        }

        public static Vector3 operator *(Matrix3 m, Vector3 v) {
            if (v._data == null | m._storage == null)
                return Vector3.Zero;

            var x = m._storage[0] * v._data[0] + m._storage[1] * v._data[1] + m._storage[2] * v._data[2];
            var y = m._storage[3] * v._data[0] + m._storage[4] * v._data[1] + m._storage[5] * v._data[2];
            var z = m._storage[6] * v._data[0] + m._storage[7] * v._data[1] + m._storage[8] * v._data[2];
            return new Vector3(x, y, z);
        }
        public static Vector3 Multiply(Matrix3 x, Vector3 y) {
            return x * y;
        }

        public Vector3 MultiplyAndAdd(Vector3 v, Vector3 w) {
            if (v._data == null | _storage == null)
                return w;
            if (w._data == null)
                return this * v;

            var x = w._data[0] + _storage[0] * v._data[0] + _storage[0] * v._data[1] + _storage[0] * v._data[2];
            var y = w._data[1] + _storage[1] * v._data[0] + _storage[1] * v._data[1] + _storage[1] * v._data[2];
            var z = w._data[2] + _storage[2] * v._data[0] + _storage[2] * v._data[1] + _storage[2] * v._data[2];
            return new Vector3(x, y, z);
        }

        #endregion

        #region IEquatable<Matrix> Membres

        public bool Equals(Matrix3 other) {
            var length = _storage.Length;
            if (_storage == null) {
                if (other._storage == null)
                    return true;
                for (int i = 0; i < length; i++) {
                    if (!other._storage[i].Equals(0d))
                        return false;
                }
            } else if (other._storage == null) {
                for (int i = 0; i < length; i++) {
                    if (!_storage[i].Equals(0d))
                        return false;
                }
            } else {
                for (int i = 0; i < length; i++) {
                    if (!_storage[i].Equals(other._storage[i]))
                        return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Matrix3)obj);
        }

        public override int GetHashCode() {
            if (_storage == null)
                return 0;
            return _storage.GetHashCode();
        }

        #endregion

    }
}
