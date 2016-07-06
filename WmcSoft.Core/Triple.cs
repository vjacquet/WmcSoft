﻿#region Licence

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
using System.Text;
using WmcSoft.Properties;
using static WmcSoft.Collections.Generic.Compare;

namespace WmcSoft
{
    public static class Triple
    {
        public static Triple<T> Create<T>(T item1, T item2, T item3) {
            return new Triple<T>(item1, item2, item3);
        }
    }

    /// <summary>
    /// Represents a Triple of two instances of the same type.
    /// </summary>
    /// <typeparam name="T">The type of the instances.</typeparam>
    /// <remarks>This class mimics <see cref="Tuple{T1, T2}"/> where T1 = T2.</remarks>
    [Serializable]
    public sealed class Triple<T> : IStructuralEquatable, IStructuralComparable, IComparable, IEquatable<Triple<T>>, IComparable<Triple<T>>
    {
        public T Item1 { get; }
        public T Item2 { get; }
        public T Item3 { get; }

        public Triple(T item1, T item2, T item3) {
            Item1 = item1;
            Item2 = item2;
            Item2 = item3;
        }

        public static implicit operator Tuple<T, T, T>(Triple<T> triple) {
            return Tuple.Create(triple.Item1, triple.Item2, triple.Item3);
        }

        public static implicit operator Triple<T>(Tuple<T, T, T> tuple) {
            return new Triple<T>(tuple.Item1, tuple.Item2, tuple.Item3);
        }

        public static bool operator ==(Triple<T> a, Triple<T> b) {
            return a.Equals(b);
        }
        public static bool operator !=(Triple<T> a, Triple<T> b) {
            return !a.Equals(b);
        }

        public static bool operator <(Triple<T> x, Triple<T> y) {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(Triple<T> x, Triple<T> y) {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(Triple<T> x, Triple<T> y) {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(Triple<T> x, Triple<T> y) {
            return x.CompareTo(y) >= 0;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append("(");
            sb.Append(Item1);
            sb.Append(", ");
            sb.Append(Item2);
            sb.Append(", ");
            sb.Append(Item3);
            sb.Append(")");
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<T>.Default);
        }

        public override int GetHashCode() {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<T>.Default);
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer) {
            if (other == null || other.GetType() != GetType())
                return false;

            var that = (Triple<T>)other;
            return comparer.Equals(Item1, that.Item1)
                && comparer.Equals(Item2, that.Item2)
                && comparer.Equals(Item3, that.Item3);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer) {
            return CombineHashCodes(comparer, Item1, Item2, Item3);
        }

        int IStructuralComparable.CompareTo(object other, IComparer comparer) {
            if (other == null)
                return 1;
            if (other.GetType() != GetType())
                throw new ArgumentException(string.Format(Resources.ComparisonIncorrectTypeFormat, GetType(), other.GetType()), "other");

            var that = (Triple<T>)other;
            var result = comparer.Compare(Item1, that.Item1);
            if (result != 0) return result;
            result = comparer.Compare(Item2, that.Item2);
            if (result != 0) return result;
            return comparer.Compare(Item3, that.Item3);
        }

        int IComparable.CompareTo(object obj) {
            return ((IStructuralComparable)this).CompareTo(obj, Comparer<T>.Default);
        }

        public bool Equals(Triple<T> other) {
            var comparer = EqualityComparer<T>.Default;
            return comparer.Equals(Item1, other.Item1)
                && comparer.Equals(Item2, other.Item2)
                && comparer.Equals(Item3, other.Item3);
        }

        public int CompareTo(Triple<T> other) {
            var comparer = Comparer<T>.Default;
            var result = comparer.Compare(Item1, other.Item1);
            if (result != 0) return result;
            result =  comparer.Compare(Item2, other.Item2);
            if (result != 0) return result;
            return comparer.Compare(Item3, other.Item3);
        }
    }
}