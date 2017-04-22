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
using System.Collections;
using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    public static class Vouch
    {
        public static EnumerableVoucher<T> IsSorted<T>(IEnumerable<T> enumerable, IComparer<T> comparer)
        {
            return new EnumerableVoucher<T>(false, enumerable, comparer);
        }
        public static EnumerableVoucher<T> IsSorted<T>(IEnumerable<T> enumerable)
        {
            return IsSorted(enumerable, Comparer<T>.Default);
        }

        public static EnumerableVoucher<T> IsSortedSet<T>(IEnumerable<T> enumerable, IComparer<T> comparer)
        {
            return new EnumerableVoucher<T>(true, enumerable, comparer);
        }
        public static EnumerableVoucher<T> IsSortedSet<T>(IEnumerable<T> enumerable)
        {
            return IsSortedSet(enumerable, Comparer<T>.Default);
        }

        public static EnumerableVoucher<T> IsSet<T>(IEnumerable<T> enumerable)
        {
            return new EnumerableVoucher<T>(true, enumerable, null);
        }

        public static SelectorVoucher<TSource, TReturn> SupportsNullArgument<TSource, TReturn>(Func<TSource, TReturn> selector)
        {
            return new SelectorVoucher<TSource, TReturn>(selector, true);
        }
    }

    public sealed class EnumerableVoucher<T> : IEnumerable<T>
    {
        #region Fields

        readonly IEnumerable<T> _enumerable;

        #endregion

        internal EnumerableVoucher(bool isSet, IEnumerable<T> enumerable, IComparer<T> comparer)
        {
            _enumerable = enumerable;
            IsSet = isSet;
            Comparer = comparer;
        }

        public bool IsSet { get; }
        public bool IsSorted => Comparer != null;
        public IComparer<T> Comparer { get; }

        public IEnumerator<T> GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public sealed class SelectorVoucher<TSource, TReturn>
    {
        internal SelectorVoucher(Func<TSource, TReturn> selector, bool supportsNullArgument)
        {
            Selector = selector;
            SupportsNullArgument = supportsNullArgument;
        }

        public Func<TSource, TReturn> Selector { get; }
        public bool SupportsNullArgument { get; }

        public static implicit operator Func<TSource, TReturn>(SelectorVoucher<TSource, TReturn> voucher)
        {
            return voucher.Selector;
        }
    }
}
