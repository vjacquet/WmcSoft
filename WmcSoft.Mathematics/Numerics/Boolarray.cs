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
    /// Represents an array of bool.
    /// </summary>
    [Serializable]
    public sealed class Boolarray : IEnumerable<bool>, ICloneable<Boolarray>, IEquatable<Boolarray>
    {
        public static Boolarray Empty = new Boolarray(0);

        #region Private utilities

        sealed class Writer
        {
            int _i;
            bool[] _data;
            Func<bool, bool, bool> _op;

            public Writer(bool[] data, Func<bool, bool, bool> op)
            {
                _data = data;
                _op = op;
            }

            public void Write(bool value, IEnumerator<bool> enumerator)
            {
                while (enumerator.MoveNext()) {
                    _data[_i++] = _op(value, enumerator.Current);
                }
            }

            public void Write(IEnumerator<bool> enumerator, bool value)
            {
                while (enumerator.MoveNext()) {
                    _data[_i++] = _op(enumerator.Current, value);
                }
            }

            public void Write(IEnumerator<bool> x, IEnumerator<bool> y)
            {
                while (x.MoveNext()) {
                    if (!y.MoveNext()) {
                        do {
                            _data[_i++] = _op(x.Current, false);
                        }
                        while (x.MoveNext());
                        break;
                    }

                    _data[_i++] = _op(x.Current, y.Current);
                }
                Write(false, y);
            }
        }

        sealed class SegmentEnumerator : IEnumerator<bool>
        {
            int _length;
            bool[] _data;

            int _begin;
            int _end;

            public SegmentEnumerator(Boolarray valarray)
                : this(valarray._dimensions.GetDimension(-1), valarray._data)
            {
            }
            public SegmentEnumerator(int length, bool[] data)
            {
                _length = length;
                _data = data;
            }

            public bool NextSegment()
            {
                _begin = _end - 1;
                _end += _length;
                return _end <= _data.Length;
            }

            #region IEnumerator Membres

            public bool Current {
                get { return _data[_begin]; }
            }

            public bool MoveNext()
            {
                _begin++;
                return _begin < _end;
            }

            object System.Collections.IEnumerator.Current {
                get { return Current; }
            }

            public void Reset()
            {
                _begin = 0;
                _end = 0;
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

        private Dimensions _dimensions;
        internal bool[] _data;

        #endregion

        #region Lifecycle

        internal Boolarray(Dimensions dimensions, bool[] data)
        {
            _dimensions = dimensions;
            _data = data;
        }
        private Boolarray(Dimensions dimensions)
            : this(dimensions, new bool[dimensions.GetCardinality()])
        {
        }

        private Boolarray(int[] dimensions)
            : this(new Dimensions(dimensions))
        {
        }

        private Boolarray(bool value, int[] dimensions)
            : this(value, new Dimensions(dimensions))
        {
        }

        private Boolarray(bool value, Dimensions dimensions)
            : this(dimensions)
        {
            var length = _data.Length;
            for (int i = 0; i < length; i++) {
                _data[i] = value;
            }
        }
        public Boolarray(int n)
        {
            if (n < 0) throw new ArgumentNullException("n");

            _dimensions = new Dimensions(n);
            _data = new bool[n];
        }

        public Boolarray(int n, Func<int, bool> generator)
        {
            _dimensions = new Dimensions(n);
            _data = new bool[n];
            for (int i = 0; i < n; i++) {
                _data[i] = generator(i);
            }
        }

        /// <summary>
        /// Creates a new one-dimensional array <see cref="Boolarray"/> with the given values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <remarks>The <see cref="Boolarray"/> takes ownership of the values, it does not copy them.</remarks>
        public Boolarray(params bool[] values)
        {
            if (values == null) throw new ArgumentNullException("values");
            if (values.Length == 0) {
                _dimensions = Empty._dimensions;
                _data = Empty._data;
            } else {
                _dimensions = new Dimensions(values.Length);
                _data = values;
            }
        }

        /// <summary>
        /// Construct a Boolarray filled with <c>false</c> values.
        /// </summary>
        /// <param name="dimensions">The dimensions of the <see cref="Boolarray"/>.</param>
        public static Boolarray Falsities(params int[] dimensions)
        {
            return new Boolarray(dimensions);
        }

        /// <summary>
        /// Construct a Boolarray filled with <c>false</c> values.
        /// </summary>
        /// <param name="dimensions">The dimensions of the <see cref="Boolarray"/>.</param>
        public static Boolarray Falsities(Dimensions dimensions)
        {
            return new Boolarray(dimensions);
        }

        /// <summary>
        /// Construct a Boolarray filled with <c>true</c> values.
        /// </summary>
        /// <param name="dimensions">The dimensions of the <see cref="Boolarray"/>.</param>
        public static Boolarray Truths(params int[] dimensions)
        {
            return new Boolarray(true, dimensions);
        }

        /// <summary>
        /// Construct a Boolarray filled with <c>true</c> values.
        /// </summary>
        /// <param name="dimensions">The dimensions of the <see cref="Boolarray"/>.</param>
        public static Boolarray Truths(Dimensions dimensions)
        {
            return new Boolarray(true, dimensions);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The number of dimensions of the <see cref="Boolarray"/>.
        /// </summary>
        public int Rank { get { return _dimensions.Count; } }

        /// <summary>
        /// The dimensions of the <see cref="Boolarray"/>.
        /// </summary>
        public Dimensions Size { get { return _dimensions; } }

        /// <summary>
        /// The number of values in the <see cref="Boolarray"/>.
        /// </summary>
        public int Cardinality { get { return _data == null ? 0 : _data.Length; } }

        /// <summary>
        /// The value at the specified coordinates.
        /// </summary>
        /// <param name="indices">The indice of the target value, in every dimension</param>
        /// <returns>The value</returns>
        /// <exception cref="ArgumentOutOfRangeException">The rank of the <paramref name="indices"/> does not match the rank of the <see cref="Boolarray"/>.</exception>
        /// <remarks>Negative and out of range indices are handled throw modulo arithmetics.</remarks>
        public bool this[params int[] indices] {
            get {
                var index = _dimensions.GetIndex(indices);
                return _data[index];
            }
            set {
                var index = _dimensions.GetIndex(indices);
                _data[index] = value;
            }
        }

        public IEnumerable<bool> this[Boolarray mask] {
            get {
                var length = Math.Min(mask._data.Length, _data.Length);
                for (int i = 0; i < length; i++) {
                    if (mask._data[i])
                        yield return _data[i];
                }
            }
            set {
                if (value == null)
                    return;
                using (var enumerator = value.GetEnumerator()) {
                    var length = Math.Min(mask._data.Length, _data.Length);
                    var i = 0;
                    for (i = 0; i < length; i++) {
                        if (mask._data[i]) {
                            if (enumerator.MoveNext()) {
                                _data[i] = enumerator.Current;
                            } else {
                                return;
                            }
                        }
                    }
                    for (; i < length; i++) {
                        if (mask._data[i]) {
                            _data[i] = false;
                        }
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Change the shape of the Boolarray.
        /// </summary>
        /// <param name="dimensions">The new dimensions</param>
        /// <returns>The Boolarray</returns>
        /// <remarks>The internal storage is grown or shrinked when required.</remarks>
        public Boolarray Reshape(params int[] dimensions)
        {
            int length = dimensions.Aggregate(1, (x, y) => x * y);
            Array.Resize(ref _data, length);
            _dimensions = new Dimensions(dimensions);
            return this;
        }

        /// <summary>
        /// Ravel the Boolarray, making it unidimensional.
        /// </summary>
        /// <returns>The Boolarray</returns>
        /// <remarks>The internal storage is preserved.</remarks>
        public Boolarray Ravel()
        {
            if (_data != null) {
                _dimensions = new Dimensions(_data.Length);
            }
            return this;
        }

        static void Combine(SegmentEnumerator x, SegmentEnumerator y, Writer writer)
        {
            while (x.NextSegment()) {
                if (!y.NextSegment()) {
                    do {
                        writer.Write(x, false);
                    }
                    while (x.NextSegment());
                    break;
                }

                writer.Write(x, y);
            }
            while (y.NextSegment()) {
                writer.Write(false, y);
            }
        }

        static Boolarray Combine(Boolarray x, Boolarray y, Func<bool, bool, bool> op)
        {
            if (x.Rank == 0)
                return y.Clone();
            if (y.Rank == 0)
                return x.Clone();

            var dimensions = Dimensions.Combine(x._dimensions, y._dimensions);
            var result = new Boolarray(dimensions);
            var writer = new Writer(result._data, op);
            Combine(new SegmentEnumerator(x), new SegmentEnumerator(y), writer);
            return result;
        }

        /// <summary>
        /// Applies the unary operation to each elements and puts the result in a new <see cref="Boolarray"/>
        /// with the same shape.
        /// </summary>
        /// <param name="op">The unary operation.</param>
        /// <returns>A new <see cref="Boolarray"/>.</returns>
        public Boolarray Map(Func<bool, bool> op)
        {
            if (Rank == 0)
                return Clone();

            var result = new Boolarray(_dimensions);
            var length = result._data.Length;
            for (int i = 0; i < length; i++) {
                result._data[i] = op(_data[i]);
            }
            return result;
        }

        /// <summary>
        /// Applies inplace the unary operation to each elements
        /// </summary>
        /// <param name="op">The unary operation.</param>
        /// <returns>The current <see cref="Boolarray"/>.</returns>
        public Boolarray Transform(Func<bool, bool> op)
        {
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

        public static Boolarray operator &(Boolarray x, Boolarray y)
        {
            return Combine(x, y, (a, b) => a && b);
        }
        public static Boolarray And(Boolarray x, Boolarray y)
        {
            return x & y;
        }

        public static Boolarray operator |(Boolarray x, Boolarray y)
        {
            return Combine(x, y, (a, b) => a | b);
        }
        public static Boolarray Or(Boolarray x, Boolarray y)
        {
            return x | y;
        }

        public static Boolarray operator ^(Boolarray x, Boolarray y)
        {
            return Combine(x, y, (a, b) => a ^ b);
        }
        public static Boolarray Xor(Boolarray x, Boolarray y)
        {
            return x ^ y;
        }

        public static Boolarray operator ~(Boolarray x)
        {
            return x.Map(v => !v);
        }
        public static Boolarray Not(Boolarray x)
        {
            return ~x;
        }

        public static bool operator ==(Boolarray x, Boolarray y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Boolarray x, Boolarray y)
        {
            return !x.Equals(y);
        }

        public static Boolarray operator !(Boolarray x)
        {
            return x.All(v => v)
                ? Falsities(x._dimensions)
                : Truths(x._dimensions);
        }

        public static bool operator true(Boolarray x)
        {
            return x.All(v => v);
        }

        public static bool operator false(Boolarray x)
        {
            return !x.Any(v => !v);
        }

        #endregion

        #region IEnumerable<bool> Membres

        public IEnumerator<bool> GetEnumerator()
        {
            var data = _data ?? Empty._data;
            return data.AsEnumerable().GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            var data = _data ?? Empty._data;
            return data.GetEnumerator();
        }

        #endregion

        #region ICloneable<Boolarray> Membres

        public Boolarray Clone()
        {
            return new Boolarray(_dimensions, (bool[])_data.Clone());
        }

        #endregion

        #region ICloneable Membres

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region IEquatable<Boolarray> Membres

        public bool Equals(Boolarray other)
        {
            if (_dimensions != other._dimensions)
                return false;
            var length = _data.Length;
            for (int i = 0; i < length; i++) {
                if (!_data[i].Equals(other._data[i]))
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Boolarray)obj);
        }

        public override int GetHashCode()
        {
            if (_data == null)
                return 0;
            return _data.GetHashCode();
        }

        #endregion
    }
}