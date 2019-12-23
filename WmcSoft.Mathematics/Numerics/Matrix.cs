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
using System.Diagnostics;
using WmcSoft.Properties;

using static System.Math;
using static WmcSoft.Helpers;

namespace WmcSoft.Numerics
{
    /// <summary>
    /// Represents a matrix of <see cref="double"/>s.
    /// </summary>
    /// <remarks>See [http://en.wikipedia.org/wiki/Matrix_(mathematics)]</remarks>
    [Serializable]
    public struct Matrix : IEquatable<Matrix>
    {
        public static Matrix Empty;

        [DebuggerTypeProxy(typeof(DebugView))]
        class Storage
        {
            class DebugView
            {
                private readonly Storage _storage;

                DebugView(Storage storage)
                {
                    _storage = storage;
                }

                [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
                public double[] Items => _storage.data;
            }

            public readonly double[] data;
            public readonly int m;
            public readonly int n;

            public Storage(Storage x)
            {
                data = (double[])x.data.Clone();
                m = x.m;
                n = x.n;

            }
            public Storage(int m, int n)
            {
                this.m = m;
                this.n = n;
                data = new double[m * n];
            }

            public double this[int i, int j] {
                get => data[i * n + j];
                set => data[i * n + j] = value;
            }

            public int Length => data.Length;
            public Dimensions Size => new Dimensions(m, n);

            public T MapReduce<T>(int first, int last, int stride, Func<double, T> map, Func<T, T, T> reduce, T seed = default)
            {
                while (first != last) {
                    seed = reduce(seed, map(data[first]));
                    first += stride;
                }
                return seed;
            }

            public void SwapRows(int i, int k)
            {
                double temp;
                var countdown = n;
                while (countdown-- != 0) {
                    temp = data[i];
                    data[i] = data[k];
                    data[k] = temp;

                    i += n;
                    k += n;
                }
            }

            public void SwapColumns(int j, int k)
            {
                double temp;
                j *= n;
                k *= n;
                var countdown = n;
                while (countdown-- != 0) {
                    temp = data[j];
                    data[j] = data[k];
                    data[k] = temp;

                    j++;
                    k++;
                }
            }

            public T MapReduce<T>(int first, int last, int stride, Func<double, int, T> map, Func<T, T, T> reduce, T seed = default)
            {
                int i = 0;
                while (first != last) {
                    seed = reduce(seed, map(data[first], i++));
                    first += stride;
                }
                return seed;
            }

            public (int k, int offset) Element<T>(int first, int last, int stride, Func<double, int, T> map, Relation<T> relation)
            {
                if (first == last)
                    return (0, first);
                T seed = map(data[first], 0);
                var offset = first;
                var k = 0;
                int i = 0;
                do {
                    first += stride;
                    var x = map(data[first], ++i);
                    if (relation(seed, x)) {
                        seed = x;
                        k = i;
                        offset = first;
                    }
                } while (first != last);
                return (k, offset);
            }
        }

        #region Fields

        private readonly Storage _storage;

        #endregion

        #region Lifecycle

        public Matrix(int n)
            : this(n, n)
        {
        }

        public Matrix(int m, int n)
        {
            _storage = new Storage(m, n);
        }

        public Matrix(int m, int n, Func<int, int, double> generator)
        {
            _storage = new Storage(m, n);
            var index = 0;
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    _storage.data[index++] = generator(i, j);
                }
            }
        }

        public Matrix(int m, int n, double value)
            : this(m, n)
        {
            Fill(_storage.data, value);
        }

        public Matrix(double[,] values)
            : this(values.GetLength(0), values.GetLength(1))
        {
            var k = 0;
            var data = _storage.data;
            foreach (var value in values) {
                data[k++] = value;
            }
        }

        private Matrix(Matrix m)
        {
            _storage = new Storage(m._storage);
        }

        public static Matrix Identity(int n)
        {
            var result = new Matrix(n);
            var data = result._storage.data;
            var length = data.Length;
            for (var i = 0; i < length; i += n + 1) {
                data[i] = 1d;
            }
            return result;
        }

        public Matrix Transpose()
        {
            var size = Size;
            var result = new Matrix(size[1], size[0]);
            var length = _storage.data.Length - _storage.m; // to avoid overflow for huge matrices
            var k = 0;
            for (var j = 0; j < _storage.m; j++, length++) {
                for (var i = j; i <= length; i += _storage.m, k++) {
                    result._storage.data[i] = _storage.data[k];
                }
            }
            return result;
        }

        public Matrix Inverse()
        {
            var size = Size;
            if (size[0] != size[1])
                throw new ArgumentException(Resources.MatrixMustBeSquare);
            var n = size[0];
            var matrix = _storage;

            var result = Identity(n);
            var invert = result._storage;

            // triangulate
            for (int i = 0; i < n; i++) {
                // Normalize
                var norm = matrix[i, i];
                for (int j = 0; j < (i + 1); j++)
                    invert[i, j] /= norm;
                for (int j = i; j < n; j++)
                    matrix[i, j] /= norm;

                // Pivot
                for (int k = (i + 1); k < n; k++) {
                    var pivot = matrix[k, i];
                    for (int j = 0; j <= i; j++)
                        invert[k, j] -= invert[i, j] * pivot;
                    for (int j = i; j < n; j++)
                        matrix[k, j] -= matrix[i, j] * pivot;
                }
            }

            // diagonalize
            for (int i = n - 1; i >= 0; i--) {
                for (int k = (i - 1); k >= 0; k--) {
                    var pivot = matrix[k, i];
                    for (int j = n - 1; j > i; j--)
                        invert[k, j] -= pivot * invert[i, j];
                    for (int j = i; j >= 0; j--) {
                        invert[k, j] -= pivot * invert[i, j];
                        matrix[k, i] -= pivot * matrix[i, j];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a Vandermonde's matrix.
        /// </summary>
        /// <param name="row">The vector used as the first row.</param>
        /// <returns>The Vandermonde's matrix.</returns>
        /// <remarks>See Knuth's TAoCP, Vol 1, Page 37.</remarks>
        public static Matrix Vandermonde(Vector row)
        {
            var n = row.Cardinality;
            var result = new Matrix(n, n);
            var data = result._storage.data;
            var length = data.Length;

            for (int i = 0; i < n; i++)
                data[i] = row[i];

            for (var j = n; j < length; j += n) {
                for (int i = 0; i < n; i++)
                    data[j + i] = data[j - n + i] * row[i];
            }

            return result;
        }

        /// <summary>
        /// Creates a combinatorial matrix.
        /// </summary>
        /// <param name="n">Dimension</param>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <returns>The combinatorial matrix.</returns>
        /// <remarks>See Knuth's TAoCP, Vol 1, Page 37.</remarks>
        public static Matrix Combinatorial(int n, double x, double y)
        {
            var result = new Matrix(n, n, y);
            var data = result._storage.data;
            var length = data.Length;
            for (int i = 0; i < length; i += n + 1) {
                result._storage.data[i] = x + y;
            }
            return result;
        }

        /// <summary>
        /// Creates a combinatorial matrix.
        /// </summary>
        /// <param name="n">Dimension</param>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="op">The combination function.</param>
        /// <returns>The combinatorial matrix.</returns>
        /// <remarks>See Knuth's TAoCP, Vol 1, Page 37.</remarks>
        public static Matrix Combinatorial(int n, double x, double y, Func<double, double, double> op)
        {
            if (op == null) throw new ArgumentNullException(nameof(op));

            var result = new Matrix(n, n, y);
            var data = result._storage.data;
            var length = data.Length;
            for (int i = 0; i < length; i += n + 1) {
                data[i] = op(x, y);
            }
            return result;
        }

        static int MaximizeCardinalities(ref Vector x, ref Vector y)
        {
            var n = x.Cardinality;
            var m = y.Cardinality;

            if (n == m)
                return n;

            if (n < m) {
                x = new Vector(m, (double[])x);
                return m;
            }

            y = new Vector(n, (double[])y);
            return n;
        }

        /// <summary>
        /// Creates a Cauchy's matrix.
        /// </summary>
        /// <param name="x">The first vector</param>
        /// <param name="y">The second vector</param>
        /// <returns>The Cauchy's matrix.</returns>
        /// <remarks>See Knuth's TAoCP, Vol 1, Page 37.</remarks>
        public static Matrix Cauchy(Vector x, Vector y)
        {
            var n = MaximizeCardinalities(ref x, ref y);
            return new Matrix(n, n, (i, j) => 1d / (x[i] + y[j]));
        }

        /// <summary>
        /// Creates a Cauchy's matrix.
        /// </summary>
        /// <param name="x">The first vector</param>
        /// <param name="y">The second vector</param>
        /// <param name="op">The combination function.</param>
        /// <returns>The Cauchy's matrix.</returns>
        /// <remarks>See Knuth's TAoCP, Vol 1, Page 37.</remarks>
        public static Matrix Cauchy(Vector x, Vector y, Func<double, double, double> op)
        {
            if (op == null) throw new ArgumentNullException(nameof(op));

            var n = MaximizeCardinalities(ref x, ref y);
            return new Matrix(n, n, (i, j) => op(x[i], y[j]));
        }

        #endregion

        #region Properties

        public int Rank => _storage == null ? 0 : 2;
        public int Cardinality => _storage == null ? 0 : _storage.Length;
        public Dimensions Size => _storage == null ? Dimensions.Empty : _storage.Size;

        public int Columns => _storage == null ? 0 : _storage.n;
        public int Rows => _storage == null ? 0 : _storage.m;

        public double this[int i, int j] {
            get {
                try {
                    return _storage[i, j];
                } catch (NullReferenceException) {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public Band<double> Row(int i)
        {
            if (_storage == null)
                return Band<double>.Empty;

            var k = i * _storage.n;
            return new Band<double>(_storage.data, k, _storage.n, 1);
        }

        public Band<double> Column(int j)
        {
            if (_storage == null)
                return Band<double>.Empty;

            return new Band<double>(_storage.data, j, _storage.m, _storage.n);
        }

        #endregion

        #region Operators

        public static explicit operator double[,](Matrix x)
        {
            if (x._storage == null)
                return new double[0, 0];
            var result = new double[x._storage.m, x._storage.n];
            var m = x._storage.m;
            var n = x._storage.n;
            var k = 0;
            for (var i = 0; i < m; i++) {
                for (var j = 0; j < n; j++) {
                    result[i, j] = x._storage.data[k++];
                }
            }
            return result;
        }
        public static double[,] ToArray(Matrix x)
        {
            return (double[,])x;
        }

        public static Matrix operator +(Matrix x, Matrix y)
        {
            if (x.Size != y.Size)
                throw new ArgumentException(Resources.MatricesMustHaveTheSameSizeError);

            var result = new Matrix(x._storage.m, x._storage.n);
            var length = result._storage.data.Length;
            for (var i = 0; i < length; i++) {
                result._storage.data[i] = y._storage.data[i] + y._storage.data[i];
            }
            return result;
        }
        public static Matrix Add(Matrix x, Matrix y)
        {
            return x + y;
        }

        public static Matrix operator -(Matrix x, Matrix y)
        {
            if (x.Size != y.Size)
                throw new ArgumentException(Resources.MatricesMustHaveTheSameSizeError);

            var result = new Matrix(x._storage.m, x._storage.n);
            var length = result._storage.data.Length;
            for (var i = 0; i < length; i++) {
                result._storage.data[i] = y._storage.data[i] - y._storage.data[i];
            }
            return result;
        }
        public static Matrix Subtract(Matrix x, Matrix y)
        {
            return x - y;
        }

        static Matrix Power(Matrix x, Matrix y, int n)
        {
            // precondition: n >= 0
            if (n == 0)
                return x;
            for (; ; ) {
                if (Odd(n)) {
                    x = x * y;
                    if (n == 1)
                        return x;
                }
                y = y * y;
                n /= 2;
            }
        }
        public static Matrix Power(Matrix m, int n)
        {
            switch (n) {
            case 0:
                return Identity(n);
            case 1:
                return m;
            case -1:
                return m.Inverse();
            }

            if (n < 0) {
                n = -1;
                m = m.Inverse();
            }

            while (Even(n)) {
                m = m * m;
                n /= 2;
            }
            if (n == 1)
                return m;
            return Power(m, m * m, (n - 1) / 2);
        }

        public static Matrix operator *(Matrix x, Matrix y)
        {
            if (x._storage.m != y._storage.n || x._storage.n != y._storage.m)
                throw new ArgumentException(Resources.MatricesMustHaveTheCompatibleSizeError);

            var m = x._storage.m;
            var n = y._storage.n;
            var k = 0;
            var result = new Matrix(m, n);
            var buffer = new double[y._storage.m];
            for (int j = 0; j < n; j++) {
                y.Column(j).CopyTo(buffer); // copying in a buffer and using array based dot product is 4-5 times faster.
                for (int i = 0; i < m; i++) {
                    result._storage.data[k++] = DotProductNotEmpty(m, x._storage.data, i * x._storage.n, 1, buffer, 0, 1);
                    //result._storage.data[k++] = DotProductNotEmpty(m, x._storage.data, i * x._storage.n, 1, y._storage.data, j, y._storage.n);
                    //result._storage.data[k++] = DotProductNotEmpty(m, x.Row(i), y.Column(j));
                }
            }
            return result;
        }
        public static Matrix Multiply(Matrix x, Matrix y)
        {
            return x * y;
        }

        public static Matrix operator -(Matrix x)
        {
            var length = x.Cardinality;
            var result = new Matrix(length);
            for (var i = 0; i < length; i++) {
                result._storage.data[i] = -x._storage.data[i];
            }
            return result;
        }
        public static Matrix Negate(Matrix x)
        {
            return -x;
        }

        public static Matrix operator +(Matrix x)
        {
            return x;
        }
        public static Matrix Plus(Matrix x)
        {
            return x;
        }

        public static Matrix operator *(double scalar, Matrix matrix)
        {
            var result = new Matrix(matrix._storage.m, matrix._storage.n);
            var length = result._storage.data.Length;
            for (int i = 0; i < length; i++) {
                result._storage.data[i] = scalar * matrix._storage.data[i];
            }
            return result;
        }
        public static Matrix Multiply(double scalar, Matrix matrix)
        {
            return scalar * matrix;
        }

        public static Matrix operator *(Matrix matrix, double scalar)
        {
            return scalar * matrix;
        }
        public static Matrix Multiply(Matrix matrix, double scalar)
        {
            return scalar * matrix;
        }

        public static Matrix operator /(Matrix matrix, double scalar)
        {
            var result = new Matrix(matrix._storage.m, matrix._storage.n);
            var length = result._storage.data.Length;
            for (int i = 0; i < length; i++) {
                result._storage.data[i] = matrix._storage.data[i] / scalar;
            }
            return result;
        }
        public static Matrix Divide(Matrix matrix, double scalar)
        {
            return matrix / scalar;
        }

        public static Vector operator *(Matrix x, Vector y)
        {
            var n = x._storage.n;
            if (y.Cardinality != n)
                throw new ArgumentException(Resources.MatricesMustHaveTheCompatibleSizeError);

            var m = x._storage.m;
            var result = new Vector(n);
            var e = y.GetEnumerator();
            for (int i = 0; i < m; i++, e.Reset()) {
                result._data[i] = DotProductNotEmpty(m, x.Row(i).GetEnumerator(), e);
            }
            return result;
        }
        public static Vector Multiply(Matrix x, Vector y)
        {
            return x * y;
        }

        public Vector MultiplyAndAdd(Vector v, Vector w)
        {
            var n = _storage.n;
            if (v.Cardinality != n) throw new ArgumentException(Resources.MatricesMustHaveTheCompatibleSizeError, "v");
            if (w.Cardinality != n) throw new ArgumentException(Resources.MatricesMustHaveTheCompatibleSizeError, "w");

            var m = _storage.m;
            var result = new Vector(w._data);
            var e = v.GetEnumerator();
            for (int i = 0; i < m; i++, e.Reset()) {
                result._data[i] += DotProductNotEmpty(m, Row(i).GetEnumerator(), e);
            }
            return result;
        }

        #endregion

        #region IEquatable<Matrix> Membres

        public bool Equals(Matrix other)
        {
            if (Size != other.Size)
                return false;
            var length = _storage.data.Length;
            for (int i = 0; i < length; i++) {
                if (!_storage.data[i].Equals(other._storage.data[i]))
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Matrix)obj);
        }

        public override int GetHashCode()
        {
            if (_storage.data == null)
                return 0;
            return _storage.data.GetHashCode();
        }

        #endregion

        #region LUDecomposition

        struct LUDecomposition
        {
            const double Tiny = 1.0e-40d;

            int n;
            Matrix a;
            Storage lu;
            int[] indx;
            double d;

            public LUDecomposition(Matrix a)
            {
                this.a = a;
                n = a.Rows;
                lu = new Storage(a._storage);
                indx = new int[n];
                d = 1d;

                try {
                    var scaling = new double[n];
                    for (int i = 0; i < lu.Length; i += n) {
                        scaling[i] = 1d / lu.MapReduce(i, i + n, 1, Abs, Max);
                    }
                    for (int k = 0; k < n; k++) {
                        var first = k * n;
                        var (imax, _) = lu.Element(first, first + n, 1, (x, i) => scaling[i] * Abs(x), (x, y) => x < y);
                        if (k != imax) {
                            lu.SwapRows(k, imax);
                            d = -d;
                            scaling[imax] = scaling[k];
                        }
                        indx[k] = imax;
                        var pivot = lu[k, k];
                        if (Abs(pivot) < double.Epsilon) lu[k, k] = pivot = Tiny;
                        for (int i = k + 1; i < n; i++) {
                            var temp = lu[i, k] /= pivot;
                            for (int j = k + 1; j < n; j++) {
                                lu[i, j] -= temp * lu[k, j];
                            }
                        }
                    }
                } catch (DivideByZeroException) {
                    throw new DivideByZeroException($"Singular matrix in {nameof(LUDecomposition)}");
                }
            }

            public double Det()
            {
                var result = d;
                for (int i = 0; i < n; i++) {
                    result *= lu[i, i];
                }
                return result;
            }

            public double[] Solve(double[] b)
            {
                var x = (double[])b.Clone();
                var ii = 0;
                for (int i = 0; i < n; i++) {
                    var ip = indx[i];
                    var sum = x[ip];
                    x[ip] = x[i];
                    if (ii != 0) {
                        for (int j = ii - 1; j < i; j++) {
                            sum -= lu[i, j] * x[j];
                        }
                    } else if (Abs(sum) > double.Epsilon) {
                        ii = i + 1;
                    }
                }
                for (int i = n - 1; i >= 0; i--) {
                    var sum = x[i];
                    for (int j = i + 1; j < n; j++) {
                        sum -= lu[i, j] * x[j];
                    }
                    x[i] = sum / lu[i, i];
                }
                return x;
            }
        }

        #endregion
    }
}
