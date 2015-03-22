using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Checks the traits of an enumerable, to optimize computation on SortedCollection or SortedCollectionSet.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    struct EnumerableTraits<T>
    {
        public int Count;
        public bool HasCount;
        public bool IsSorted;
        public bool IsSet;

        public EnumerableTraits(IEnumerable<T> enumerable) {
            IsSet = false;
            IsSorted = false;

            var count = GetCount(enumerable as ICollection<T>)
                ?? GetCount(enumerable as IReadOnlyCollection<T>)
                ?? GetCount(enumerable as ICollection);
            if (!count.HasValue) {
                Count = 0;
                HasCount = false;
                return;
            }
            Count = count.GetValueOrDefault();
            HasCount = true;
        }

        public EnumerableTraits(IEnumerable<T> enumerable, IComparer<T> comparer) {
            var count = GetCount(enumerable as ICollection<T>)
                ?? GetCount(enumerable as IReadOnlyCollection<T>)
                ?? GetCount(enumerable as ICollection);
            if (!count.HasValue) {
                Count = 0;
                HasCount = false;
                IsSet = false;
                IsSorted = false;
                return;
            }

            Count = count.GetValueOrDefault();
            HasCount = true;
            IsSet = enumerable is ISet<T>;
            if (IsSet) {
                var sortedCollectionSet = enumerable as SortedCollectionSet<T>;
                if (sortedCollectionSet != null) {
                    IsSorted = comparer.Equals(sortedCollectionSet.Comparer);
                    return;
                }

                var sortedSet = enumerable as SortedSet<T>;
                if (sortedSet != null) {
                    IsSorted = comparer.Equals(sortedSet.Comparer);
                    return;
                }

                IsSorted = false;
            } else {
                var sorted = enumerable as SortedCollection<T>;
                IsSorted = (sorted != null) && comparer.Equals(sorted.Comparer);
            }
        }

        #region Collection resolver

        static int? GetCount(ICollection<T> collection) {
            if (collection != null)
                return collection.Count;
            return null;
        }
        static int? GetCount(IReadOnlyCollection<T> collection) {
            if (collection != null)
                return collection.Count;
            return null;
        }
        static int? GetCount(ICollection collection) {
            if (collection != null)
                return collection.Count;
            return null;
        }

        #endregion
    }
}
