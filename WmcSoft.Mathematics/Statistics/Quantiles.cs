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

namespace WmcSoft.Statistics
{
    public struct Quantiles<T> : IEnumerable<T>
    {
        readonly List<T> _values;

        #region Lifecycle

        public Quantiles(IEnumerable<T> values) {
            _values = new List<T>(values);
            _values.Sort();
        }

        public Quantiles(IComparer<T> comparer, IEnumerable<T> values) {
            _values = new List<T>(values);
            _values.Sort(comparer);
        }

        public Quantiles(params T[] values) {
            _values = new List<T>(values);
            _values.Sort();
        }

        public Quantiles(IComparer<T> comparer, params T[] values) {
            _values = new List<T>(values);
            _values.Sort(comparer);
        }

        #endregion

        #region Properties

        public T this[double quantile] {
            get {
                if (quantile < 0d || quantile >= 1d)
                    throw new ArgumentOutOfRangeException("q");

                var n = _values.Count;
                // nearest policy
                var q = quantile * (n - 1) + 1d;
                var j = (int)Math.Floor(q);
                var g = q - j;
                if (g > 0.5d)
                    return _values[j];
                return _values[j - 1];
            }
        }

        #endregion

        #region IEnumerable<T> Membres

        public IEnumerator<T> GetEnumerator() {
            return _values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((System.Collections.IEnumerable)_values).GetEnumerator();
        }

        #endregion
    }
}
