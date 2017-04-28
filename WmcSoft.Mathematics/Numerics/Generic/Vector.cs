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
using WmcSoft.Arithmetics;
using WmcSoft.Properties;

namespace WmcSoft.Numerics.Generic
{
    public struct Vector<T, C> : IEquatable<Vector<T, C>>, IReadOnlyList<T>
        where C : IArithmetics<T>, new()
    {
        public static Vector<T, C> Empty;

        #region Fields

        static C Calculator = new C();

        private readonly T[] _data;

        #endregion

        #region Lifecycle

        public Vector(int n)
        {
            _data = new T[n];
        }

        public Vector(int n, Func<int, T> generator)
        {
            _data = new T[n];
            for (int i = 0; i < n; i++) {
                _data[i] = generator(i);
            }
        }

        public Vector(params T[] values)
        {
            _data = values;
        }

        public Vector(int n, T[] values)
        {
            _data = new T[n];
            Array.Copy(values, _data, Math.Min(n, values.Length));
        }

        #endregion

        #region Properties

        public int Rank { get { return _data == null ? 0 : 1; } }
        public int Cardinality { get { return _data == null ? 0 : _data.Length; } }
        public T this[int index] { get { return _data[index]; } }

        #endregion

        #region Operators

        public static explicit operator T[] (Vector<T, C> x)
        {
            return x._data == null
                ? new T[0]
                : (T[])x._data.Clone();
        }
        public static T[] ToArray(Vector<T, C> x)
        {
            return (T[])x;
        }

        public static Vector<T, C> operator +(Vector<T, C> x, Vector<T, C> y)
        {
            var length = x.Cardinality;
            if (y.Cardinality != length)
                throw new ArgumentException(Resources.VectorsMustHaveSameSizeError);

            var result = new Vector<T, C>(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = Calculator.Add(x._data[i], y._data[i]);
            }
            return result;
        }
        public static Vector<T, C> Add(Vector<T, C> x, Vector<T, C> y)
        {
            return x + y;
        }

        public static Vector<T, C> operator -(Vector<T, C> x, Vector<T, C> y)
        {
            var length = x.Cardinality;
            if (y.Cardinality != length)
                throw new ArgumentException(Resources.VectorsMustHaveSameSizeError);

            var result = new Vector<T, C>(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = Calculator.Subtract(x._data[i], y._data[i]);
            }
            return result;
        }
        public static Vector<T, C> Subtract(Vector<T, C> x, Vector<T, C> y)
        {
            return x - y;
        }

        public static Vector<T, C> operator *(T alpha, Vector<T, C> x)
        {
            var length = x.Cardinality;
            var result = new Vector<T, C>(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = Calculator.Multiply(alpha, x._data[i]);
            }
            return result;
        }
        public static Vector<T, C> Multiply(T alpha, Vector<T, C> x)
        {
            return alpha * x;
        }
        public static Vector<T, C> operator *(Vector<T, C> x, T alpha)
        {
            // do not reuse a*v as Multiply might not be commutative
            var length = x.Cardinality;
            var result = new Vector<T, C>(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = Calculator.Multiply(x._data[i], alpha);
            }
            return result;
        }
        public static Vector<T, C> Multiply(Vector<T, C> x, T alpha)
        {
            return x * alpha;
        }

        public static Vector<T, C> operator /(Vector<T, C> x, T alpha)
        {
            var length = x.Cardinality;
            var result = new Vector<T, C>(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = Calculator.Divide(x._data[i], alpha);
            }
            return result;
        }
        public static Vector<T, C> Divide(Vector<T, C> x, T alpha)
        {
            return x / alpha;
        }

        public static Vector<T, C> operator -(Vector<T, C> x)
        {
            var length = x.Cardinality;
            var result = new Vector<T, C>(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = Calculator.Negate(x._data[i]);
            }
            return result;
        }
        public static Vector<T, C> Negate(Vector<T, C> x)
        {
            return -x;
        }

        public static Vector<T, C> operator +(Vector<T, C> x)
        {
            return x;
        }
        public static Vector<T, C> Plus(Vector<T, C> x)
        {
            return x;
        }

        public static T DotProduct(Vector<T, C> x, Vector<T, C> y)
        {
            var length = x.Cardinality;
            if (y.Cardinality != length)
                throw new ArgumentException(Resources.VectorsMustHaveSameSizeError);

            var result = default(T);
            for (int i = 0; i < length; i++) {
                result = Calculator.Add(result, Calculator.Multiply(x._data[i], y._data[i]));
            }
            return result;

        }

        internal static T DotProductNotEmpty(int length, IEnumerator<T> x, IEnumerator<T> y)
        {
            x.MoveNext();
            y.MoveNext();

            var result = Calculator.Multiply(x.Current, y.Current);
            length--;
            while (length-- > 0) {
                x.MoveNext();
                y.MoveNext();
                result = Calculator.Add(result, Calculator.Multiply(x.Current, y.Current));
            }
            return result;
        }

        #endregion

        #region IEquatable<Vector> Membres

        public bool Equals(Vector<T, C> other)
        {
            var length = Cardinality;
            if (other.Cardinality != length)
                return false;
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
            return Equals((Vector<T, C>)obj);
        }

        public override int GetHashCode()
        {
            if (_data == null)
                return 0;
            return _data.GetHashCode();
        }

        #endregion

        #region IReadOnlyCollection<T> Members

        int IReadOnlyCollection<T>.Count {
            get { return Cardinality; }
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            if (_data == null || _data.Length == 0)
                return Enumerable.Empty<T>().GetEnumerator();
            return new StrideEnumerator<T>(_data);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        #endregion
    }
}
