using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Small utility to operate on ordered enumerable or enumerable on ordered sets.
    /// </summary>
    static class SetUtilities
    {
        internal static IEnumerable<T> Unique<T>(IEnumerable<T> enumerable, IEqualityComparer<T> comparer)
        {
            using (IEnumerator<T> enumerator = enumerable.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    T current = enumerator.Current;
                    while (enumerator.MoveNext()) {
                        if (!comparer.Equals(current, enumerator.Current)) {
                            yield return current;
                            current = enumerator.Current;
                        }
                    }
                    yield return current;
                }
            }
        }

        internal static IEnumerable<T> Unique<T>(IEnumerable<T> enumerable)
        {
            return Unique(enumerable, EqualityComparer<T>.Default);
        }

        internal static IEnumerable<T> Union<T>(IEnumerable<T> x, IEnumerable<T> y, IComparer<T> comparer)
        {
            int compare = 0;
            using (var enumerator1 = x.GetEnumerator())
            using (var enumerator2 = y.GetEnumerator()) {
                var hasValue1 = enumerator1.MoveNext();
                var hasValue2 = enumerator2.MoveNext();

                do {
                    if (!hasValue1) {
                        while (hasValue2) {
                            yield return enumerator2.Current;
                            hasValue2 = enumerator2.MoveNext();
                        }
                        yield break;
                    } else if (!hasValue2) {
                        while (hasValue1) {
                            yield return enumerator1.Current;
                            hasValue1 = enumerator1.MoveNext();
                        }
                        yield break;
                    } else {
                        compare = comparer.Compare(enumerator1.Current, enumerator2.Current);
                        if (compare < 0) {
                            yield return enumerator1.Current;
                            hasValue1 = enumerator1.MoveNext();
                        } else if (compare > 0) {
                            yield return enumerator2.Current;
                            hasValue2 = enumerator2.MoveNext();
                        } else {
                            yield return enumerator1.Current;
                            hasValue1 = enumerator1.MoveNext();
                            hasValue2 = enumerator2.MoveNext();
                        }
                    }
                } while (true);
            }
        }

        internal static IEnumerable<T> Union<T>(IEnumerable<T> x, IEnumerable<T> y)
        {
            return Union(x, y, Comparer<T>.Default);
        }

        internal static IEnumerable<T> Intersection<T>(IEnumerable<T> x, IEnumerable<T> y, IComparer<T> comparer)
        {
            int compare = 0;
            using (var enumerator1 = x.GetEnumerator())
            using (var enumerator2 = y.GetEnumerator()) {
                var hasValue1 = enumerator1.MoveNext();
                var hasValue2 = enumerator2.MoveNext();

                do {
                    if (!hasValue1 || !hasValue2) {
                        yield break;
                    } else {
                        compare = comparer.Compare(enumerator1.Current, enumerator2.Current);
                        if (compare < 0) {
                            hasValue1 = enumerator1.MoveNext();
                        } else if (compare > 0) {
                            hasValue2 = enumerator2.MoveNext();
                        } else {
                            yield return enumerator1.Current;
                            hasValue1 = enumerator1.MoveNext();
                            hasValue2 = enumerator2.MoveNext();
                        }
                    }
                } while (true);
            }
        }

        internal static IEnumerable<T> Intersection<T>(IEnumerable<T> x, IEnumerable<T> y)
        {
            return Intersection(x, y, Comparer<T>.Default);
        }

        internal static IEnumerable<T> Difference<T>(IEnumerable<T> x, IEnumerable<T> y, IComparer<T> comparer)
        {
            int compare = 0;
            using (var enumerator1 = x.GetEnumerator())
            using (var enumerator2 = y.GetEnumerator()) {
                var hasValue1 = enumerator1.MoveNext();
                var hasValue2 = enumerator2.MoveNext();

                do {
                    if (!hasValue1) {
                        yield break;
                    } else if (!hasValue2) {
                        while (hasValue1) {
                            yield return enumerator1.Current;
                            hasValue1 = enumerator1.MoveNext();
                        }
                        yield break;
                    } else {
                        compare = comparer.Compare(enumerator1.Current, enumerator2.Current);
                        if (compare < 0) {
                            yield return enumerator1.Current;
                            hasValue1 = enumerator1.MoveNext();
                        } else if (compare > 0) {
                            hasValue2 = enumerator2.MoveNext();
                        } else {
                            hasValue1 = enumerator1.MoveNext();
                            hasValue2 = enumerator2.MoveNext();
                        }
                    }
                } while (true);
            }
        }

        internal static IEnumerable<T> Difference<T>(IEnumerable<T> x, IEnumerable<T> y)
        {
            return Difference(x, y, Comparer<T>.Default);
        }

        internal static IEnumerable<T> SymmetricDifference<T>(IEnumerable<T> x, IEnumerable<T> y, IComparer<T> comparer)
        {
            int compare = 0;
            using (var enumerator1 = x.GetEnumerator())
            using (var enumerator2 = y.GetEnumerator()) {
                var hasValue1 = enumerator1.MoveNext();
                var hasValue2 = enumerator2.MoveNext();

                do {
                    if (!hasValue1) {
                        while (hasValue2) {
                            yield return enumerator2.Current;
                            hasValue2 = enumerator2.MoveNext();
                        }
                        yield break;
                    } else if (!hasValue2) {
                        while (hasValue1) {
                            yield return enumerator1.Current;
                            hasValue1 = enumerator1.MoveNext();
                        }
                        yield break;
                    } else {
                        compare = comparer.Compare(enumerator1.Current, enumerator2.Current);
                        if (compare < 0) {
                            yield return enumerator1.Current;
                            hasValue1 = enumerator1.MoveNext();
                        } else if (compare > 0) {
                            yield return enumerator2.Current;
                            hasValue2 = enumerator2.MoveNext();
                        } else {
                            hasValue1 = enumerator1.MoveNext();
                            hasValue2 = enumerator2.MoveNext();
                        }
                    }
                } while (true);
            }
        }

        internal static IEnumerable<T> SymmetricDifference<T>(IEnumerable<T> x, IEnumerable<T> y)
        {
            return SymmetricDifference(x, y, Comparer<T>.Default);
        }
    }
}
