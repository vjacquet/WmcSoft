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
    /// Represents an array of <see cref="bool"/>.
    /// </summary>
    [Serializable]
    public sealed class Boolarray : IEnumerable<bool>, IEquatable<Boolarray>, ICloneable
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
            int length;
            bool[] data;

            int begin;
            int end;

            public SegmentEnumerator(Boolarray valarray)
                : this(valarray.dimensions.GetDimension(-1), valarray.data)
            {
            }
            public SegmentEnumerator(int length, bool[] data)
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

            public bool Current => data[begin];

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
        internal bool[] data;

        #endregion

        #region Lifecycle

        internal Boolarray(Dimensions dimensions, bool[] data)
        {
            this.dimensions = dimensions;
            this.data = data;
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
            var length = data.Length;
            for (int i = 0; i < length; i++) {
                data[i] = value;
            }
        }
        public Boolarray(int n)
        {
            if (n < 0) throw new ArgumentNullException("n");

            dimensions = new Dimensions(n);
            data = new bool[n];
        }

        public Boolarray(int n, Func<int, bool> generator)
        {
            dimensions = new Dimensions(n);
            data = new bool[n];
            for (int i = 0; i < n; i++) {
                data[i] = generator(i);
            }
        }

        /// <summary>
        /// Creates an empty <see cref="Boolarray"/>.
        /// </summary>
        public Boolarray()
        {
            dimensions = Empty.dimensions;
            data = Empty.data;
        }

        /// <summary>
        /// Creates a new one-dimensional array <see cref="Boolarray"/> with the given values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <remarks>The <see cref="Boolarray"/> takes ownership of the values, it does not copy them.</remarks>
        public Boolarray(params bool[] values)
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
        public int Rank => dimensions.Count;

        /// <summary>
        /// The dimensions of the <see cref="Boolarray"/>.
        /// </summary>
        public Dimensions Size => dimensions;

        /// <summary>
        /// The number of values in the <see cref="Boolarray"/>.
        /// </summary>
        public int Cardinality => data == null ? 0 : data.Length;

        /// <summary>
        /// The value at the specified coordinates.
        /// </summary>
        /// <param name="indices">The indice of the target value, in every dimension</param>
        /// <returns>The value</returns>
        /// <exception cref="ArgumentOutOfRangeException">The rank of the <paramref name="indices"/> does not match the rank of the <see cref="Boolarray"/>.</exception>
        /// <remarks>Negative and out of range indices are handled throw modulo arithmetics.</remarks>
        public bool this[params int[] indices] {
            get {
                var index = dimensions.GetIndex(indices);
                return data[index];
            }
            set {
                var index = dimensions.GetIndex(indices);
                data[index] = value;
            }
        }

        public IEnumerable<bool> this[Boolarray mask] {
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
                                return;
                            }
                        }
                    }
                    for (; i < length; i++) {
                        if (mask.data[i]) {
                            data[i] = false;
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
            Array.Resize(ref data, length);
            this.dimensions = new Dimensions(dimensions);
            return this;
        }

        /// <summary>
        /// Ravel the Boolarray, making it unidimensional.
        /// </summary>
        /// <returns>The Boolarray</returns>
        /// <remarks>The internal storage is preserved.</remarks>
        public Boolarray Ravel()
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

            var dimensions = Dimensions.Combine(x.dimensions, y.dimensions);
            var result = new Boolarray(dimensions);
            var writer = new Writer(result.data, op);
            UnguardedCombine(new SegmentEnumerator(x), new SegmentEnumerator(y), writer);
            return result;
        }

        private Boolarray UnguardedMap(Func<bool, bool> op)
        {
            if (Rank == 0)
                return new Boolarray();

            var length = data.Length;
            var a = new bool[length];
            for (int i = 0; i < length; i++) {
                a[i] = op(data[i]);
            }
            return new Boolarray(dimensions, a);
        }

        /// <summary>
        /// Applies the unary operation to each elements and puts the result in a new <see cref="Boolarray"/>
        /// with the same shape.
        /// </summary>
        /// <param name="op">The unary operation.</param>
        /// <returns>A new <see cref="Boolarray"/>.</returns>
        public Boolarray Map(Func<bool, bool> op)
        {
            if (op == null) throw new ArgumentNullException(nameof(op));

            return UnguardedMap(op);
        }

        /// <summary>
        /// Applies inplace the unary operation to each elements
        /// </summary>
        /// <param name="op">The unary operation.</param>
        /// <returns>The current <see cref="Boolarray"/>.</returns>
        public Boolarray Transform(Func<bool, bool> op)
        {
            if (Rank > 0) {
                var length = data.Length;
                for (int i = 0; i < length; i++) {
                    data[i] = op(data[i]);
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
            return x.UnguardedMap(v => !v);
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
                ? Falsities(x.dimensions)
                : Truths(x.dimensions);
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
            return new Boolarray(dimensions, (bool[])data.Clone());
        }

        #endregion

        #region IEquatable<Boolarray> Membres

        public bool Equals(Boolarray other)
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
            return Equals((Boolarray)obj);
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
