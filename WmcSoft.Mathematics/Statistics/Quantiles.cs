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

namespace WmcSoft.Statistics
{
    /// <summary>
    /// Utility to retrieve the closest quantile from a given set.
    /// </summary>
    /// <typeparam name="T">The type of values.</typeparam>
    public struct Quantiles<T> : IEnumerable<T>
    {
        readonly List<T> values;

        #region Lifecycle

        private Quantiles(List<T> values, IComparer<T> comparer)
        {
            this.values = values;
            this.values.Sort(comparer ?? Comparer<T>.Default);
        }

        public Quantiles(IEnumerable<T> values)
            : this(new List<T>(values), Comparer<T>.Default)
        {
        }

        public Quantiles(IComparer<T> comparer, IEnumerable<T> values)
            : this(new List<T>(values), comparer ?? Comparer<T>.Default)
        {
        }

        public Quantiles(params T[] values)
            : this(new List<T>(values), Comparer<T>.Default)
        {
        }

        public Quantiles(IComparer<T> comparer, params T[] values)
            : this(new List<T>(values), comparer ?? Comparer<T>.Default)
        {
        }

        #endregion

        #region Properties

        public T this[double quantile] {
            get {
                if (quantile < 0d || quantile >= 1d) throw new ArgumentOutOfRangeException(nameof(quantile));

                var n = values.Count;
                // nearest policy
                var q = quantile * (n - 1) + 1d;
                var j = (int)Math.Floor(q);
                var g = q - j;
                if (g > 0.5d)
                    return values[j];
                return values[j - 1];
            }
        }

        public T this[int nth, int quantiles] {
            get {
                if (nth < 0 || nth >= quantiles) throw new ArgumentOutOfRangeException(nameof(nth));

                return this[(double)nth / quantiles];
            }
        }

        #endregion

        #region Specialized

        public T Median()
        {
            return this[1, 2];
        }

        public T Tercile(int nth)
        {
            return this[nth, 3];
        }

        public T Quartile(int nth)
        {
            return this[nth, 4];
        }

        public T Quintile(int nth)
        {
            return this[nth, 5];
        }

        public T Sextile(int nth)
        {
            return this[nth, 6];
        }

        public T Septile(int nth)
        {
            return this[nth, 7];
        }

        public T Octile(int nth)
        {
            return this[nth, 8];
        }

        public T Decile(int nth)
        {
            return this[nth, 10];
        }

        public T Duodecile(int nth)
        {
            return this[nth, 12];
        }

        public T Hexadecile(int nth)
        {
            return this[nth, 16];
        }

        public T Vigintile(int nth)
        {
            return this[nth, 20];
        }

        public T Trigintatrecile(int nth)
        {
            return this[nth, 30];
        }

        public T Percentile(int nth)
        {
            return this[nth, 100];
        }

        public T Permille(int nth)
        {
            return this[nth, 1000];
        }

        #endregion

        #region IEnumerable<T> Membres

        public IEnumerator<T> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }

        #endregion
    }
}
