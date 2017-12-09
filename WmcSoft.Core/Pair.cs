#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using WmcSoft.Collections.Generic;
using WmcSoft.Properties;

namespace WmcSoft
{
    /// <summary>
    /// Provides static methods for creating pair objects.
    /// </summary>
    public static class Pair
    {
        /// <summary>
        /// Creates a pair.
        /// </summary>
        /// <typeparam name="T">The type of the components of the pair.</typeparam>
        /// <param name="item">The value of the first and second component of the pair.</param>
        /// <returns>A pair whose value is (<paramref name="item"/>, <paramref name="item"/>).</returns>
        public static Pair<T> Create<T>(T item = default)
        {
            return new Pair<T>(item, item);
        }

        /// <summary>
        /// Creates a pair.
        /// </summary>
        /// <typeparam name="T">The type of the components of the pair.</typeparam>
        /// <param name="item1">The value of the first component of the pair.</param>
        /// <param name="item2">The value of the second component of the pair.</param>
        /// <returns>A pair whose value is (<paramref name="item1"/>, <paramref name="item2"/>).</returns>
        public static Pair<T> Create<T>(T item1, T item2)
        {
            return new Pair<T>(item1, item2);
        }
    }

    /// <summary>
    /// Represents a pair of two instances of the same type.
    /// </summary>
    /// <typeparam name="T">The type of the instances.</typeparam>
    /// <remarks>This class mimics <see cref="Tuple{T1, T2}"/> where T1 = T2.</remarks>
    [Serializable]
    [DebuggerDisplay("{ToString(),nq}")]
    public struct Pair<T> : IStructuralEquatable, IStructuralComparable, IComparable, IEquatable<Pair<T>>, IComparable<Pair<T>>
    {
        public T Item1 { get; }
        public T Item2 { get; }

        public Pair(T item1, T item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public void Deconstruct(out T item1, out T item2)
        {
            item1 = Item1;
            item2 = Item2;
        }

        public static implicit operator ValueTuple<T, T>(Pair<T> pair)
        {
            return ValueTuple.Create(pair.Item1, pair.Item2);
        }

        public static implicit operator Pair<T>(ValueTuple<T, T> tuple)
        {
            return new Pair<T>(tuple.Item1, tuple.Item2);
        }

        public static implicit operator Tuple<T, T>(Pair<T> pair)
        {
            return Tuple.Create(pair.Item1, pair.Item2);
        }

        public static implicit operator Pair<T>(Tuple<T, T> tuple)
        {
            return new Pair<T>(tuple.Item1, tuple.Item2);
        }

        public static bool operator ==(Pair<T> a, Pair<T> b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Pair<T> a, Pair<T> b)
        {
            return !a.Equals(b);
        }

        public static bool operator <(Pair<T> x, Pair<T> y)
        {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(Pair<T> x, Pair<T> y)
        {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(Pair<T> x, Pair<T> y)
        {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(Pair<T> x, Pair<T> y)
        {
            return x.CompareTo(y) >= 0;
        }

        public override string ToString()
        {
            return "(" + Item1 + ", " + Item2 + ")";
        }

        public override bool Equals(object obj)
        {
            return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<T>.Default);
        }

        public override int GetHashCode()
        {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<T>.Default);
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            if (other == null || other.GetType() != GetType())
                return false;

            var that = (Pair<T>)other;
            return comparer.Equals(Item1, that.Item1) && comparer.Equals(Item2, that.Item2);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            return comparer.CombineHashCodes(Item1, Item2);
        }

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            if (other == null)
                return 1;
            if (other.GetType() != GetType())
                throw new ArgumentException(string.Format(Resources.ComparisonIncorrectTypeFormat, GetType(), other.GetType()), nameof(other));

            var that = (Pair<T>)other;
            var result = comparer.Compare(Item1, that.Item1);
            if (result != 0) return result;
            return comparer.Compare(Item2, that.Item2);
        }

        int IComparable.CompareTo(object obj)
        {
            return ((IStructuralComparable)this).CompareTo(obj, Comparer<T>.Default);
        }

        public bool Equals(Pair<T> other)
        {
            if (ReferenceEquals(this, other)) return true;

            var comparer = EqualityComparer<T>.Default;
            return comparer.Equals(Item1, other.Item1) && comparer.Equals(Item2, other.Item2);
        }

        public int CompareTo(Pair<T> other)
        {
            var comparer = Comparer<T>.Default;
            var result = comparer.Compare(Item1, other.Item1);
            if (result != 0) return result;
            return comparer.Compare(Item2, other.Item2);
        }
    }
}
