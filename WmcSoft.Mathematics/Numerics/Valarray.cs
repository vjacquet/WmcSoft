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

namespace WmcSoft.Numerics
{
    [Serializable]
    public sealed class Valarray : IEnumerable<double>, ICloneable<Valarray>, IEquatable<Valarray>
    {
        class Writer
        {
            int _i;
            double[] _data;
            Func<double, double, double> _op;

            public Writer(double[] data, Func<double, double, double> op) {
                _data = data;
                _op = op;
            }

            public void Write(double value, IEnumerator<double> enumerator) {
                while (enumerator.MoveNext()) {
                    _data[_i++] = _op(value, enumerator.Current);
                }
            }

            public void Write(IEnumerator<double> enumerator, double value) {
                while (enumerator.MoveNext()) {
                    _data[_i++] = _op(enumerator.Current, value);
                }
            }

            public void Write(IEnumerator<double> x, IEnumerator<double> y) {
                while (x.MoveNext()) {
                    if (!y.MoveNext()) {
                        do {
                            _data[_i++] = _op(x.Current, 0d);
                        }
                        while (x.MoveNext());
                        break;
                    }

                    _data[_i++] = _op(x.Current, y.Current);
                }
                Write(0d, y);
            }
        }

        class SegmentEnumerator : IEnumerator<double>
        {
            int _length;
            double[] _data;

            int _begin;
            int _end;

            public SegmentEnumerator(Valarray valarray)
                : this(valarray._dimensions.GetDimension(-1), valarray._data) {
            }
            public SegmentEnumerator(int length, double[] data) {
                _length = length;
                _data = data;
            }

            public bool NextSegment() {
                _begin = _end - 1;
                _end += _length;
                return _end <= _data.Length;
            }

            #region IEnumerator Membres

            public double Current {
                get { return _data[_begin]; }
            }

            public bool MoveNext() {
                _begin++;
                return _begin < _end;
            }

            object System.Collections.IEnumerator.Current {
                get { return Current; }
            }

            public void Reset() {
                _begin = 0;
                _end = 0;
            }

            #endregion

            #region IDisposable Membres

            public void Dispose() {
            }

            #endregion
        }

        #region Fields

        private Dimensions _dimensions;
        private double[] _data;

        #endregion

        #region Lifecycle

        private Valarray(Dimensions dimensions) {
            _dimensions = dimensions;
            var length = dimensions.Aggregate(1, (x, y) => x * y);
            _data = new double[length];
        }

        private Valarray(int[] dimensions)
            : this(new Dimensions(dimensions)) {
        }

        private Valarray(double value, int[] dimensions)
            : this(new Dimensions(dimensions)) {
            var length = _data.Length;
            for (int i = 0; i < length; i++) {
                _data[i] = value;
            }
        }

        public Valarray(int n) {
            _dimensions = new Dimensions(n);
            _data = new double[n];
        }

        public Valarray(int n, Func<int, double> generator) {
            _dimensions = new Dimensions(n);
            _data = new double[n];
            for (int i = 0; i < n; i++) {
                _data[i] = generator(i);
            }
        }

        public Valarray(params double[] values) {
            _dimensions = new Dimensions(values.Length);
            _data = values;
        }

        /// <summary>
        /// Construct a Valarray filled with zeros.
        /// </summary>
        /// <param name="values"></param>
        public static Valarray Zeros(params int[] dimensions) {
            return new Valarray(dimensions);
        }

        /// <summary>
        /// Construct a Valarray filled with ones.
        /// </summary>
        /// <param name="values"></param>
        public static Valarray Ones(params int[] dimensions) {
            return new Valarray(1d, dimensions);
        }

        public static Valarray Range(int start, int end, int step = 1) {
            if (step == 0)
                throw new ArgumentOutOfRangeException("step");

            var length = Math.Abs((end - start) / step);
            var result = new Valarray(length);
            double value = start;
            for (int i = 0; i < length; i++) {
                result._data[i] = value;
                value += step;
            }
            return result;
        }

        #endregion

        #region Properties

        public int Rank { get { return _dimensions.Count; } }
        public Dimensions Size { get { return _dimensions; } }
        public int Cardinality { get { return _data == null ? 0 : _data.Length; } }
        public double this[params int[] indices] {
            get {
                var index = _dimensions.GetIndex(indices);
                return _data[index];
            }
            set {
                var index = _dimensions.GetIndex(indices);
                _data[index] = value;
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
        public Valarray Reshape(params int[] dimensions) {
            int length = dimensions.Aggregate(1, (x, y) => x * y);
            Array.Resize(ref _data, length);
            _dimensions = new Dimensions(dimensions);
            return this;
        }

        /// <summary>
        /// Ravel the Valarray, making it unidimensional.
        /// </summary>
        /// <returns>The Valarray</returns>
        /// <remarks>The internal storage is preserved.</remarks>
        public Valarray Ravel() {
            if (_data != null) {
                _dimensions = new Dimensions(_data.Length);
            }
            return this;
        }

        static void Combine(SegmentEnumerator x, SegmentEnumerator y, Writer writer) {
            while (x.NextSegment()) {
                if (!y.NextSegment()) {
                    do {
                        writer.Write(x, 0d);
                    }
                    while (x.NextSegment());
                    break;
                }

                writer.Write(x, y);
            }
            while (y.NextSegment()) {
                writer.Write(0d, y);
            }
        }

        static Valarray Combine(Valarray x, Valarray y, Func<double, double, double> op) {
            if (x.Rank == 0)
                return y.Clone();
            if (y.Rank == 0)
                return x.Clone();

            var dimensions = Dimensions.Combine(x._dimensions, y._dimensions);
            var result = new Valarray(dimensions);
            var writer = new Writer(result._data, op);
            Combine(new SegmentEnumerator(x), new SegmentEnumerator(y), writer);
            return result;
        }

        public Valarray Map(Func<double, double> op) {
            if (Rank == 0)
                return Clone();

            var result = new Valarray(_dimensions);
            var length = result._data.Length;
            for (int i = 0; i < length; i++) {
                result._data[i] = op(_data[i]);
            }
            return result;
        }

        public Valarray Transform(Func<double, double> op) {
            if (Rank > 0) {
                var length = _data.Length;
                for (int i = 0; i < length; i++) {
                    _data[i] = op(_data[i]);
                }
            }
            return this;
        }

        #endregion

        #region Operators

        public static Valarray operator +(Valarray x, Valarray y) {
            return Combine(x, y, (a, b) => a + b);
        }
        public static Valarray Add(Valarray x, Valarray y) {
            return x + y;
        }

        public static Valarray operator -(Valarray x, Valarray y) {
            return Combine(x, y, (a, b) => a - b);
        }
        public static Valarray Subtract(Valarray x, Valarray y) {
            return x - y;
        }

        public static Valarray operator *(Valarray x, Valarray y) {
            return Combine(x, y, (a, b) => a * b);
        }
        public static Valarray Multiply(Valarray x, Valarray y) {
            return x * y;
        }

        public static Valarray operator /(Valarray x, Valarray y) {
            return Combine(x, y, (a, b) => a / b);
        }
        public static Valarray Divide(Valarray x, Valarray y) {
            return x / y;
        }

        public static Valarray operator +(double scalar, Valarray valarray) {
            return valarray.Map(x => scalar + x);
        }
        public static Valarray Add(double scalar, Valarray valarray) {
            return scalar + valarray;
        }

        public static Valarray operator +(Valarray valarray, double scalar) {
            return valarray.Map(x => x + scalar);
        }
        public static Valarray Add(Valarray valarray, double scalar) {
            return valarray + scalar;
        }

        public static Valarray operator -(double scalar, Valarray valarray) {
            return valarray.Map(x => scalar - x);
        }
        public static Valarray Subtract(double scalar, Valarray valarray) {
            return scalar - valarray;
        }

        public static Valarray operator -(Valarray valarray, double scalar) {
            return valarray.Map(x => x - scalar);
        }
        public static Valarray Subtract(Valarray valarray, double scalar) {
            return valarray - scalar;
        }

        public static Valarray operator *(double scalar, Valarray valarray) {
            return valarray.Map(x => scalar * x);
        }
        public static Valarray Multiply(double scalar, Valarray valarray) {
            return scalar * valarray;
        }

        public static Valarray operator *(Valarray valarray, double scalar) {
            return valarray.Map(x => x * scalar);
        }
        public static Valarray Multiply(Valarray valarray, double scalar) {
            return valarray * scalar;
        }

        public static Valarray operator /(double scalar, Valarray valarray) {
            return valarray.Map(x => scalar / x);
        }
        public static Valarray Divide(double scalar, Valarray valarray) {
            return scalar / valarray;
        }

        public static Valarray operator /(Valarray valarray, double scalar) {
            return valarray.Map(x => x / scalar);
        }
        public static Valarray Divide(Valarray valarray, double scalar) {
            return valarray / scalar;
        }

        public static Valarray operator -(Valarray x) {
            return x.Map(v => -v);
        }
        public static Valarray Negate(Valarray x) {
            return -x;
        }

        public static Valarray operator +(Valarray x) {
            return x.Clone();
        }
        public static Valarray Plus(Valarray x) {
            return x;
        }

        #endregion

        #region IEnumerable<double> Membres

        public IEnumerator<double> GetEnumerator() {
            var data = _data ?? new double[0];
            return data.AsEnumerable().GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            var data = _data ?? new double[0];
            return data.GetEnumerator();
        }

        #endregion

        #region ICloneable<Valarray> Membres

        public Valarray Clone() {
            var clone = new Valarray();
            clone._dimensions = _dimensions;
            if (_dimensions.Count > 0) {
                clone._data = (double[])_data.Clone();
            }
            return clone;
        }

        #endregion

        #region ICloneable Membres

        object ICloneable.Clone() {
            return Clone();
        }

        #endregion

        #region IEquatable<Valarray> Membres

        public bool Equals(Valarray other) {
            if (_dimensions != other._dimensions)
                return false;
            var length = _data.Length;
            for (int i = 0; i < length; i++) {
                if (!_data[i].Equals(other._data[i]))
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Valarray)obj);
        }

        public override int GetHashCode() {
            if (_data == null)
                return 0;
            return _data.GetHashCode();
        }

        #endregion
    }
}
