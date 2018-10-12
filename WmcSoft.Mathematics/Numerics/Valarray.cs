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

namespace WmcSoft.Numerics
{
    /// <summary>
    /// Represents an array of values that can be reshaped.
    /// </summary>
    [Serializable]
    public sealed class Valarray : IEnumerable<double>, IEquatable<Valarray>, ICloneable
    {
        public static Valarray Empty = new Valarray(0);

        #region Private utilities

        sealed class Writer
        {
            int i;
            double[] data;
            Func<double, double, double> op;

            public Writer(double[] data, Func<double, double, double> op)
            {
                this.data = data;
                this.op = op;
            }

            public void Write(double value, IEnumerator<double> enumerator)
            {
                while (enumerator.MoveNext()) {
                    data[i++] = op(value, enumerator.Current);
                }
            }

            public void Write(IEnumerator<double> enumerator, double value)
            {
                while (enumerator.MoveNext()) {
                    data[i++] = op(enumerator.Current, value);
                }
            }

            public void Write(IEnumerator<double> x, IEnumerator<double> y)
            {
                while (x.MoveNext()) {
                    if (!y.MoveNext()) {
                        do {
                            data[i++] = op(x.Current, 0d);
                        }
                        while (x.MoveNext());
                        break;
                    }

                    data[i++] = op(x.Current, y.Current);
                }
                Write(0d, y);
            }
        }

        sealed class SegmentEnumerator : IEnumerator<double>
        {
            readonly int length;
            readonly double[] data;

            int begin;
            int end;

            public SegmentEnumerator(Valarray valarray)
                : this(valarray.dimensions.GetDimension(-1), valarray.data)
            {
            }

            public SegmentEnumerator(int length, double[] data)
            {
                this.length = length;
                this.data = data;
            }

            public bool NextSegment()
            {
                begin = end - 1;
                end += length;
                return end <= data.Length;
            }

            #region IEnumerator Membres

            public double Current => data[begin];

            public bool MoveNext()
            {
                begin++;
                return begin < end;
            }

            object System.Collections.IEnumerator.Current => Current;

            public void Reset()
            {
                begin = 0;
                end = 0;
            }

            #endregion

            #region IDisposable Membres

            public void Dispose()
            {
            }

            #endregion
        }

        #endregion

        #region Fields

        private Dimensions dimensions;
        internal double[] data;

        #endregion

        #region Lifecycle

        private Valarray(Dimensions dimensions, double[] data)
        {
            this.dimensions = dimensions;
            this.data = data;
        }

        private Valarray(Dimensions dimensions)
            : this(dimensions, new double[dimensions.GetCardinality()])
        {
        }

        private Valarray(int[] dimensions)
            : this(new Dimensions(dimensions))
        {
        }

        private Valarray(double value, int[] dimensions)
            : this(new Dimensions(dimensions))
        {
            var length = data.Length;
            for (int i = 0; i < length; i++) {
                data[i] = value;
            }
        }

        public Valarray(int n)
        {
            if (n < 0) throw new ArgumentNullException("n");

            dimensions = new Dimensions(n);
            data = new double[n];
        }

        public Valarray(int n, Func<int, double> generator)
        {
            dimensions = new Dimensions(n);
            data = new double[n];
            for (int i = 0; i < n; i++) {
                data[i] = generator(i);
            }
        }

        public Valarray(int n, Func<int, int, double> generator)
        {
            dimensions = new Dimensions(n, n);
            data = new double[dimensions.GetCardinality()];
            var index = 0;
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    data[index++] = generator(i, j);
                }
            }
        }

        public Valarray(int m, int n, Func<int, int, double> generator)
        {
            dimensions = new Dimensions(m, n);
            data = new double[dimensions.GetCardinality()];
            var index = 0;
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    data[index++] = generator(i, j);
                }
            }
        }

        public Valarray(int n, Func<int, int, int, double> generator)
        {
            dimensions = new Dimensions(n, n, n);
            data = new double[dimensions.GetCardinality()];
            var index = 0;
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    for (int k = 0; k < n; k++) {
                        data[index++] = generator(i, j, k);
                    }
                }
            }
        }

        public Valarray(int m, int n, int o, Func<int, int, int, double> generator)
        {
            dimensions = new Dimensions(m, n, o);
            data = new double[dimensions.GetCardinality()];
            var index = 0;
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    for (int k = 0; k < o; k++) {
                        data[index++] = generator(i, j, k);
                    }
                }
            }
        }

        public Valarray(int n, Func<int, int, int, int, double> generator)
        {
            dimensions = new Dimensions(n, n, n, n);
            data = new double[dimensions.GetCardinality()];
            var index = 0;
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    for (int k = 0; k < n; k++) {
                        for (int l = 0; l < n; l++) {
                            data[index++] = generator(i, j, k, l);
                        }
                    }
                }
            }
        }

        public Valarray(int m, int n, int o, int p, Func<int, int, int, int, double> generator)
        {
            dimensions = new Dimensions(m, n, o, p);
            data = new double[dimensions.GetCardinality()];
            var index = 0;
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    for (int k = 0; k < o; k++) {
                        for (int l = 0; l < p; l++) {
                            data[index++] = generator(i, j, k, l);
                        }
                    }
                }
            }
        }

        public Valarray(int n, Func<int, int, int, int, int, double> generator)
        {
            dimensions = new Dimensions(n, n, n, n, n);
            data = new double[n];
            var index = 0;
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    for (int k = 0; k < n; k++) {
                        for (int l = 0; l < n; l++) {
                            for (int h = 0; h < n; h++) {
                                data[index++] = generator(i, j, k, l, h);
                            }
                        }
                    }
                }
            }
        }

        public Valarray(int m, int n, int o, int p, int q, Func<int, int, int, int, int, double> generator)
        {
            dimensions = new Dimensions(m, n, o, p, q);
            data = new double[n];
            var index = 0;
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    for (int k = 0; k < o; k++) {
                        for (int l = 0; l < p; l++) {
                            for (int h = 0; h < q; h++) {
                                data[index++] = generator(i, j, k, l, h);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates an empty <see cref="Valarray"/>.
        /// </summary>
        public Valarray()
        {
            dimensions = Empty.dimensions;
            data = Empty.data;
        }

        /// <summary>
        /// Creates a new one-dimensional array <see cref="Valarray"/> with the given values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <remarks>The <see cref="Valarray"/> takes ownership of the values, it does not copy them.</remarks>
        public Valarray(params double[] values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            if (values.Length == 0) {
                dimensions = Empty.dimensions;
                data = Empty.data;
            } else {
                dimensions = new Dimensions(values.Length);
                data = values;
            }
        }

        /// <summary>
        /// Construct a Valarray filled with zeros.
        /// </summary>
        /// <param name="values"></param>
        public static Valarray Zeros(params int[] dimensions)
        {
            return new Valarray(dimensions);
        }

        /// <summary>
        /// Construct a Valarray filled with ones.
        /// </summary>
        /// <param name="values"></param>
        public static Valarray Ones(params int[] dimensions)
        {
            return new Valarray(1d, dimensions);
        }

        public static Valarray Range(int start, int end, int step = 1)
        {
            if (step == 0)
                throw new ArgumentOutOfRangeException("step");

            var length = Math.Abs((end - start) / step);
            var result = new Valarray(length);
            double value = start;
            for (int i = 0; i < length; i++) {
                result.data[i] = value;
                value += step;
            }
            return result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The number of dimensions of the <see cref="Valarray"/>.
        /// </summary>
        public int Rank => dimensions.Count;

        /// <summary>
        /// The dimensions of the <see cref="Valarray"/>.
        /// </summary>
        public Dimensions Size => dimensions;

        /// <summary>
        /// The number of values in the <see cref="Valarray"/>.
        /// </summary>
        public int Cardinality => data == null ? 0 : data.Length;

        /// <summary>
        /// The value at the specified coordinates.
        /// </summary>
        /// <param name="indices">The indice of the target value, in every dimension</param>
        /// <returns>The value</returns>
        /// <exception cref="ArgumentOutOfRangeException">The rank of the <paramref name="indices"/> does not match the rank of the <see cref="Valarray"/>.</exception>
        /// <remarks>Negative and out of range indices are handled throw modulo arithmetics.</remarks>
        public double this[params int[] indices] {
            get {
                var index = dimensions.GetIndex(indices);
                return data[index];
            }
            set {
                var index = dimensions.GetIndex(indices);
                data[index] = value;
            }
        }

        public IEnumerable<double> this[Boolarray mask] {
            get {
                var length = Math.Min(mask.data.Length, data.Length);
                for (int i = 0; i < length; i++) {
                    if (mask.data[i])
                        yield return data[i];
                }
            }
            set {
                if (value == null)
                    return;
                using (var enumerator = value.GetEnumerator()) {
                    var length = Math.Min(mask.data.Length, data.Length);
                    var i = 0;
                    for (i = 0; i < length; i++) {
                        if (mask.data[i]) {
                            if (enumerator.MoveNext()) {
                                data[i] = enumerator.Current;
                            } else {
                                break;
                            }
                        }
                    }
                    for (; i < length; i++) {
                        if (mask.data[i]) {
                            data[i] = 0d;
                        }
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Change the shape of the Valarray.
        /// </summary>
        /// <param name="dimensions">The new dimensions</param>
        /// <returns>The Valarray</returns>
        /// <remarks>The internal storage is grown or shrinked when required.</remarks>
        public Valarray Reshape(params int[] dimensions)
        {
            int length = dimensions.Aggregate(1, (x, y) => x * y);
            Array.Resize(ref data, length);
            this.dimensions = new Dimensions(dimensions);
            return this;
        }

        /// <summary>
        /// Ravel the Valarray, making it unidimensional.
        /// </summary>
        /// <returns>The Valarray</returns>
        /// <remarks>The internal storage is preserved.</remarks>
        public Valarray Ravel()
        {
            if (data != null) {
                dimensions = new Dimensions(data.Length);
            }
            return this;
        }

        static void UnguardedCombine(SegmentEnumerator x, SegmentEnumerator y, Writer writer)
        {
            while (x.NextSegment()) {
                if (!y.NextSegment()) {
                    do {
                        writer.Write(x, 0d);
                    }
                    while (x.NextSegment());
                    return;
                }

                writer.Write(x, y);
            }
            while (y.NextSegment()) {
                writer.Write(0d, y);
            }
        }

        static Valarray Combine(Valarray x, Valarray y, Func<double, double, double> op)
        {
            if (x.Rank == 0)
                return y.Clone();
            if (y.Rank == 0)
                return x.Clone();

            var dimensions = Dimensions.Combine(x.dimensions, y.dimensions);
            var result = new Valarray(dimensions);
            var writer = new Writer(result.data, op);
            UnguardedCombine(new SegmentEnumerator(x), new SegmentEnumerator(y), writer);
            return result;
        }

        private Valarray UnguardedMap(Func<double, double> op)
        {
            if (Rank == 0)
                return new Valarray();

            var length = data.Length;
            var a = new double[length];
            for (int i = 0; i < length; i++) {
                a[i] = op(data[i]);
            }
            return new Valarray(dimensions, a);
        }

        /// <summary>
        /// Applies the unary operation to each elements and puts the result in a new <see cref="Valarray"/>
        /// with the same shape.
        /// </summary>
        /// <param name="op">The unary operation.</param>
        /// <returns>A new <see cref="Valarray"/>.</returns>
        public Valarray Map(Func<double, double> op)
        {
            if (op == null) throw new ArgumentNullException(nameof(op));

            return UnguardedMap(op);
        }

        /// <summary>
        /// Applies inplace the unary operation to each elements
        /// </summary>
        /// <param name="op">The unary operation.</param>
        /// <returns>The current <see cref="Valarray"/>.</returns>
        public Valarray Transform(Func<double, double> op)
        {
            if (Rank > 0) {
                for (int i = 0; i < data.Length; i++) {
                    data[i] = op(data[i]);
                }
            }
            return this;
        }

        #endregion

        #region Operators

        public static Valarray operator +(Valarray x, Valarray y)
        {
            return Combine(x, y, (a, b) => a + b);
        }
        public static Valarray Add(Valarray x, Valarray y)
        {
            return x + y;
        }

        public static Valarray operator -(Valarray x, Valarray y)
        {
            return Combine(x, y, (a, b) => a - b);
        }
        public static Valarray Subtract(Valarray x, Valarray y)
        {
            return x - y;
        }

        public static Valarray operator *(Valarray x, Valarray y)
        {
            return Combine(x, y, (a, b) => a * b);
        }
        public static Valarray Multiply(Valarray x, Valarray y)
        {
            return x * y;
        }

        public static Valarray operator /(Valarray x, Valarray y)
        {
            return Combine(x, y, (a, b) => a / b);
        }
        public static Valarray Divide(Valarray x, Valarray y)
        {
            return x / y;
        }

        public static Valarray operator +(double scalar, Valarray valarray)
        {
            return valarray.UnguardedMap(x => scalar + x);
        }
        public static Valarray Add(double scalar, Valarray valarray)
        {
            return scalar + valarray;
        }

        public static Valarray operator +(Valarray valarray, double scalar)
        {
            return valarray.UnguardedMap(x => x + scalar);
        }
        public static Valarray Add(Valarray valarray, double scalar)
        {
            return valarray + scalar;
        }

        public static Valarray operator -(double scalar, Valarray valarray)
        {
            return valarray.Map(x => scalar - x);
        }
        public static Valarray Subtract(double scalar, Valarray valarray)
        {
            return scalar - valarray;
        }

        public static Valarray operator -(Valarray valarray, double scalar)
        {
            return valarray.UnguardedMap(x => x - scalar);
        }
        public static Valarray Subtract(Valarray valarray, double scalar)
        {
            return valarray - scalar;
        }

        public static Valarray operator *(double scalar, Valarray valarray)
        {
            return valarray.UnguardedMap(x => scalar * x);
        }
        public static Valarray Multiply(double scalar, Valarray valarray)
        {
            return scalar * valarray;
        }

        public static Valarray operator *(Valarray valarray, double scalar)
        {
            return valarray.UnguardedMap(x => x * scalar);
        }
        public static Valarray Multiply(Valarray valarray, double scalar)
        {
            return valarray * scalar;
        }

        public static Valarray operator /(double scalar, Valarray valarray)
        {
            return valarray.UnguardedMap(x => scalar / x);
        }
        public static Valarray Divide(double scalar, Valarray valarray)
        {
            return scalar / valarray;
        }

        public static Valarray operator /(Valarray valarray, double scalar)
        {
            return valarray.UnguardedMap(x => x / scalar);
        }
        public static Valarray Divide(Valarray valarray, double scalar)
        {
            return valarray / scalar;
        }

        public static Valarray operator -(Valarray x)
        {
            return x.UnguardedMap(v => -v);
        }
        public static Valarray Negate(Valarray x)
        {
            return -x;
        }

        public static Valarray operator +(Valarray x)
        {
            return x.Clone();
        }
        public static Valarray Plus(Valarray x)
        {
            return x.Clone();
        }

        public static Boolarray operator <(Valarray valarray, double value)
        {
            return new Boolarray(valarray.dimensions, Array.ConvertAll(valarray.data, x => x < value));
        }
        public static Boolarray operator <=(Valarray valarray, double value)
        {
            return new Boolarray(valarray.dimensions, Array.ConvertAll(valarray.data, x => x <= value));
        }
        public static Boolarray operator >(Valarray valarray, double value)
        {
            return new Boolarray(valarray.dimensions, Array.ConvertAll(valarray.data, x => x > value));
        }
        public static Boolarray operator >=(Valarray valarray, double value)
        {
            return new Boolarray(valarray.dimensions, Array.ConvertAll(valarray.data, x => x >= value));
        }

        public static Boolarray operator ==(Valarray valarray, double value)
        {
            return new Boolarray(valarray.dimensions, Array.ConvertAll(valarray.data, x => x.Equals(value)));
        }
        public static Boolarray operator !=(Valarray valarray, double value)
        {
            return new Boolarray(valarray.dimensions, Array.ConvertAll(valarray.data, x => !x.Equals(value)));
        }

        public Boolarray Match(Predicate<double> predicate)
        {
            return new Boolarray(dimensions, Array.ConvertAll(data, x => predicate(x)));
        }

        private void UnguardedAssign(Boolarray mask, double value)
        {
            var length = Math.Min(data.Length, mask.data.Length);
            for (int i = 0; i < length; i++) {
                if (mask.data[i])
                    data[i] = value;
            }
        }

        public static Valarray operator *(Valarray valarray, Boolarray mask)
        {
            if (mask == null) throw new ArgumentNullException(nameof(mask));

            var result = valarray.Clone();
            result.UnguardedAssign(mask, 0d);
            return result;
        }
        public Valarray Assign(Boolarray mask, double value = 0d)
        {
            if (mask == null) throw new ArgumentNullException(nameof(mask));

            UnguardedAssign(mask, 0d);
            return this;
        }
        public Valarray Assign<TValues>(Boolarray mask, TValues values)
            where TValues : IEnumerable<double>
        {
            using (var enumerator = values.GetEnumerator()) {
                var length = Math.Min(mask.data.Length, data.Length);
                var i = 0;
                for (i = 0; i < length; i++) {
                    if (enumerator.MoveNext()) {
                        if (mask.data[i])
                            data[i] = enumerator.Current;
                    } else {
                        break;
                    }
                }
                for (; i < length; i++) {
                    if (mask.data[i]) {
                        data[i] = 0d;
                    }
                }
            }
            return this;
        }

        #endregion

        #region IEnumerable<double> Membres

        public IEnumerator<double> GetEnumerator()
        {
            var data = this.data ?? Empty.data;
            return data.AsEnumerable().GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            var data = this.data ?? Empty.data;
            return data.GetEnumerator();
        }

        #endregion

        #region ICloneable Membres

        object ICloneable.Clone()
        {
            return new Valarray(dimensions, (double[])data.Clone());
        }

        #endregion

        #region IEquatable<Valarray> Membres

        public bool Equals(Valarray other)
        {
            if (dimensions != other.dimensions)
                return false;
            var length = data.Length;
            for (int i = 0; i < length; i++) {
                if (!data[i].Equals(other.data[i]))
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Valarray)obj);
        }

        public override int GetHashCode()
        {
            if (data == null)
                return 0;
            return data.GetHashCode();
        }

        #endregion
    }
}
