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
    public static class Pair
    {
        public static Pair<T> Create<T>(T item1, T item2) {
            return new Pair<T>(item1, item2);
        }

        public static Pair<T1, T2> Create<T1, T2>(T1 item1, T2 item2) {
            return new Pair<T1, T2>(item1, item2);
        }
    }

    [Serializable]
    public struct Pair<T> : IStructuralEquatable, IStructuralComparable, IComparable, IEquatable<Pair<T>>, IComparable<Pair<T>>
    {
        public T Item1 { get; }
        public T Item2 { get; }

        public Pair(T item1, T item2) {
            Item1 = item1;
            Item2 = item2;
        }

        public static implicit operator Tuple<T, T>(Pair<T> pair) {
            return Tuple.Create(pair.Item1, pair.Item2);
        }

        public static implicit operator Pair<T>(Tuple<T, T> tuple) {
            return new Pair<T>(tuple.Item1, tuple.Item2);
        }

        public static bool operator ==(Pair<T> a, Pair<T> b) {
            return a.Equals(b);
        }
        public static bool operator !=(Pair<T> a, Pair<T> b) {
            return !a.Equals(b);
        }

        public static bool operator <(Pair<T> x, Pair<T> y) {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(Pair<T> x, Pair<T> y) {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(Pair<T> x, Pair<T> y) {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(Pair<T> x, Pair<T> y) {
            return x.CompareTo(y) >= 0;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append("(");
            sb.Append(Item1);
            sb.Append(", ");
            sb.Append(Item2);
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

            var that = (Pair<T>)other;
            return comparer.Equals(Item1, that.Item1) && comparer.Equals(Item2, that.Item2);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer) {
            return CombineHashCodes(comparer.GetHashCode(Item1), comparer.GetHashCode(Item2));
        }

        int IStructuralComparable.CompareTo(object other, IComparer comparer) {
            if (other == null)
                return 1;
            if (other.GetType() != GetType())
                throw new ArgumentException(string.Format(Resources.ComparisonIncorrectTypeFormat, GetType(), other.GetType()), "other");

            var that = (Pair<T>)other;
            var result = comparer.Compare(Item1, that.Item1);
            if (result != 0) return result;
            return comparer.Compare(Item2, that.Item2);
        }

        int IComparable.CompareTo(object obj) {
            return ((IStructuralComparable)this).CompareTo(obj, Comparer<T>.Default);
        }

        public bool Equals(Pair<T> other) {
            var comparer = EqualityComparer<T>.Default;
            return comparer.Equals(Item1, other.Item1) && comparer.Equals(Item2, other.Item2);
        }

        public int CompareTo(Pair<T> other) {
            var comparer = Comparer<T>.Default;
            var result = comparer.Compare(Item1, other.Item1);
            if (result != 0) return result;
            return comparer.Compare(Item2, other.Item2);
        }
    }

    public struct Pair<T1, T2> : IStructuralEquatable, IStructuralComparable, IComparable, IEquatable<Pair<T1, T2>>, IComparable<Pair<T1, T2>>
    {
        public T1 Item1 { get; }
        public T2 Item2 { get; }

        public Pair(T1 item1, T2 item2) {
            Item1 = item1;
            Item2 = item2;
        }

        public static implicit operator Tuple<T1, T2>(Pair<T1, T2> pair) {
            return Tuple.Create(pair.Item1, pair.Item2);
        }

        public static implicit operator Pair<T1, T2>(Tuple<T1, T2> tuple) {
            return new Pair<T1, T2>(tuple.Item1, tuple.Item2);
        }

        public static bool operator ==(Pair<T1, T2> a, Pair<T1, T2> b) {
            return a.Equals(b);
        }
        public static bool operator !=(Pair<T1, T2> a, Pair<T1, T2> b) {
            return !a.Equals(b);
        }

        public static bool operator <(Pair<T1, T2> x, Pair<T1, T2> y) {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(Pair<T1, T2> x, Pair<T1, T2> y) {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator >(Pair<T1, T2> x, Pair<T1, T2> y) {
            return x.CompareTo(y) > 0;
        }
        public static bool operator >=(Pair<T1, T2> x, Pair<T1, T2> y) {
            return x.CompareTo(y) >= 0;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append("(");
            sb.Append(Item1);
            sb.Append(", ");
            sb.Append(Item2);
            sb.Append(")");
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
        }

        public override int GetHashCode() {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer) {
            if (other == null || other.GetType() != GetType())
                return false;

            var that = (Pair<T1, T2>)other;
            return comparer.Equals(Item1, that.Item1) && comparer.Equals(Item2, that.Item2);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer) {
            return CombineHashCodes(comparer.GetHashCode(Item1), comparer.GetHashCode(Item2));
        }

        int IStructuralComparable.CompareTo(object other, IComparer comparer) {
            if (other == null)
                return 1;
            if (other.GetType() != GetType())
                throw new ArgumentException(string.Format(Resources.ComparisonIncorrectTypeFormat, GetType(), other.GetType()), "other");

            var that = (Pair<T1, T2>)other;
            var result = comparer.Compare(Item1, that.Item1);
            if (result != 0) return result;
            return comparer.Compare(Item2, that.Item2);
        }

        int IComparable.CompareTo(object obj) {
            return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
        }

        public bool Equals(Pair<T1, T2> other) {
            var comparer = EqualityComparer<object>.Default;
            return comparer.Equals(Item1, other.Item1) && comparer.Equals(Item2, other.Item2);
        }

        public int CompareTo(Pair<T1, T2> other) {
            var comparer = Comparer<object>.Default;
            var result = comparer.Compare(Item1, other.Item1);
            if (result != 0) return result;
            return comparer.Compare(Item2, other.Item2);
        }
    }
}