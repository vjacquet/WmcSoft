﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Properties;

namespace WmcSoft.Numerics
{
    [Serializable]
    public struct Matrix3 : IEquatable<Matrix3>
    {
        struct Tag { };
        static Tag Uninitialized = new Tag();

        const int N = 3;

        public static Matrix3 Empty;
        public static Matrix3 Zero = new Matrix3(Uninitialized);
        public static Matrix3 Identity = new Matrix3(1, 0, 0, 0, 1, 0, 0, 0, 1);

        #region Fields

        private readonly double[] _storage;

        #endregion

        #region Lifecycle

        private Matrix3(Tag tag) {
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

        public IEnumerable<double> Row(int i) {
            // TODO: Change IEnumerable to a IReadOnlyCollection
            if (_storage == null)
                yield break;

            var countdown = N;
            var k = i * N;
            while (countdown-- > 0)
                yield return _storage[k++];
        }

        public IEnumerable<double> Column(int j) {
            // TODO: Change IEnumerable to a IReadOnlyCollection
            if (_storage == null)
                yield break;

            var countdown = N;
            var k = j;
            while (countdown-- > 0) {
                yield return _storage[k];
                k += N;
            }
        }

        #endregion

        #region Operators

        public static explicit operator double[,](Matrix3 x) {
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
            var result = new Matrix3(Uninitialized);
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
            var result = new Matrix3(Uninitialized);
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
            var result = new Matrix3(Uninitialized);
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
            var result = new Matrix3(Uninitialized);
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
            var result = new Matrix3(Uninitialized);
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
            var result = new Matrix3(Uninitialized);
            for (int i = 0; i < length; i++) {
                result._storage[i] = matrix._storage[i] / scalar;
            }
            return result;
        }
        public static Matrix3 Divide(Matrix3 matrix, double scalar) {
            return matrix / scalar;
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