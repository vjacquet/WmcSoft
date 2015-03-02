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
