using System.Collections.Generic;

namespace WmcSoft.Collections.Generic.Algorithms
{
    public class KnuthMorrisPratt<T>
    {
        readonly IReadOnlyList<T> _pattern;
        readonly int[] _next;
        readonly IEqualityComparer<T> _comparer;

        public KnuthMorrisPratt(IReadOnlyList<T> pattern, IEqualityComparer<T> comparer) {
            _pattern = pattern;
            _comparer = comparer;
            var length = pattern.Count;
            _next = new int[length+1];
            _next[0] = -1;
            for (int i = 0, j = -1; i < length; _next[++i] = ++j) {
                while (j >= 0 && !_comparer.Equals(pattern[i], pattern[j]))
                    j = _next[j];
            }
        }

        public int Find(IReadOnlyList<T> t) {
            int i = 0, j = 0, m = _pattern.Count, n = t.Count;
            for (; j < m && i < n; i++, j++) {
                while (j >= 0 && !_comparer.Equals(t[i], _pattern[j]))
                    j = _next[j];
            }
            if (j == m)
                return i - m;
            return -1;
        }
    }
}
