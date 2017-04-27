using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Checks the traits of an enumerable, to optimize computation on SortedCollection or SortedCollectionSet.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    struct EnumerableTraits<T>
    {
        public int Count;
        public bool HasCount => Count >= 0;
        public bool IsSorted;
        public bool IsSet;

        public EnumerableTraits(IEnumerable<T> enumerable)
        {
            IsSet = false;
            IsSorted = false;
            Count = ResolveCount(enumerable);
        }

        public EnumerableTraits(IEnumerable<T> enumerable, IComparer<T> comparer)
        {
            Count = ResolveCount(enumerable);
            if (Count >= 0) {
                switch (enumerable) {
                case SortedSequenceSet<T> set:
                    IsSet = true;
                    IsSorted = comparer.Equals(set.Comparer);
                    break;
                case SortedSet<T> set:
                    IsSet = true;
                    IsSorted = comparer.Equals(set.Comparer);
                    break;
                case ISet<T> set:
                    IsSet = true;
                    IsSorted = false;
                    break;
                case SortedSequence<T> set:
                    IsSet = false;
                    IsSorted = comparer.Equals(set.Comparer);
                    break;
                default:
                    IsSet = false;
                    IsSorted = false;
                    break;
                }
            } else {
                IsSet = false;
                IsSorted = false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int ResolveCount(IEnumerable<T> enumerable)
        {
            switch (enumerable) {
            case ICollection<T> collection:
                return collection.Count;
            case IReadOnlyCollection<T> collection:
                return collection.Count;
            case ICollection collection:
                return collection.Count;
            default:
                return -1;
            }
        }
    }
}
