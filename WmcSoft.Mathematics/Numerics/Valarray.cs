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
        class SegmentEnumerator
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

            public double Current {
                get { return _data[_begin]; }
                set { _data[_begin] = value; }
            }

            public bool MoveNext() {
                _begin++;
                return _begin < _end;
            }

            public bool NextSegment() {
                _begin = _end - 1;
                _end += _length;
                return _end <= _data.Length;
            }
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
        public Dimensions Shape { get { return _dimensions; } }
        public int Size { get { return _data == null ? 0 : _data.Length; } }
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

        SegmentEnumerator GetSegmentEnumerator() {
            return new SegmentEnumerator(_dimensions[_dimensions.Count - 1], _data);
        }

        static void CombineSegments(SegmentEnumerator x, SegmentEnumerator y, Func<double, double, double> op, SegmentEnumerator output) {
            while (x.MoveNext()) {
                if (!y.MoveNext()) {
                    do {
                        output.MoveNext();
                        output.Current = op(x.Current, 0d);
                    }
                    while (x.MoveNext());
                    break;
                }

                output.MoveNext();
                output.Current = op(x.Current, y.Current);
            }
            CombineSegments(0d, y, op, output);
        }

        static void CombineSegments(double value, SegmentEnumerator y, Func<double, double, double> op, SegmentEnumerator output) {
            while (y.MoveNext()) {
                output.MoveNext();
                output.Current = op(value, y.Current);
            }
        }

        static void CombineSegments(SegmentEnumerator y, double value, Func<double, double, double> op, SegmentEnumerator output) {
            while (y.MoveNext()) {
                output.MoveNext();
                output.Current = op(y.Current, value);
            }
        }

        static void Combine(SegmentEnumerator x, SegmentEnumerator y, Func<double, double, double> op, SegmentEnumerator output) {
            while (x.NextSegment()) {
                if (!y.NextSegment()) {
                    do {
                        output.NextSegment();
                        CombineSegments(x, 0d, op, output);
                    }
                    while (x.MoveNext());
                    break;
                }

                output.NextSegment();
                CombineSegments(x, y, op, output);
            }
            while (y.NextSegment()) {
                output.NextSegment();
                CombineSegments(0d, y, op, output);
            }
        }

        static Valarray Combine(Valarray x, Valarray y, Func<double, double, double> op) {
            if (x._dimensions.Count == 0)
                return (Valarray)y.Clone();
            if (y._dimensions.Count == 0)
                return (Valarray)x.Clone();

            var dimensions = Dimensions.Combine(x._dimensions, y._dimensions);
            var result = new Valarray(dimensions);
            Combine(new SegmentEnumerator(x), new SegmentEnumerator(y), op, new SegmentEnumerator(result));
            return result;
        }

        #endregion

        #region Operators

        public static Valarray operator +(Valarray x, Valarray y) {
            return Combine(x, y, (a, b) => a + b);
        }
        public static Valarray Add(Valarray x, Valarray y) {
            return x + y;
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
            return this.Clone<Valarray>();
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
