#region Licence

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
using System.Linq;
using WmcSoft.Collections.Generic.Internals;

namespace WmcSoft.Text
{
    /// <summary>
    /// Implementation of trie data structure.
    /// </summary>
    public class Trie<TLetter, TValue> : ITrie<TLetter, TValue>
        where TLetter : struct, IEquatable<TLetter>
    {
        #region Node

        class Node : IEnumerable<TLetter>
        {
            bool _hasValue;
            TValue _value;
            readonly IDictionary<TLetter, Node> _nodes;

            public Node()
            {
                _nodes = new SortedList<TLetter, Node>();
            }

            public bool HasValue {
                get { return _hasValue; }
            }

            public TValue Value {
                get { return _value; }
                set {
                    _hasValue = true;
                    _value = value;
                }
            }

            public bool Reset()
            {
                if (_hasValue) {
                    _hasValue = false;
                    _value = default(TValue);
                    return true;
                }
                return false;
            }

            public bool ResetIf(TValue value)
            {
                if (_hasValue && EqualityComparer<TValue>.Default.Equals(_value, value)) {
                    _hasValue = false;
                    _value = default(TValue);
                    return true;
                }
                return false;
            }

            public bool IsEmpty {
                get { return !_hasValue && _nodes.Count == 0; }
            }

            public Node this[TLetter index] {
                get {
                    if (_nodes.TryGetValue(index, out Node node))
                        return node;
                    return null;
                }
                set {
                    _nodes[index] = value;
                }
            }

            public IEnumerator<TLetter> GetEnumerator()
            {
                return _nodes.Keys.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        #endregion

        private Node _root;
        private int _count;
        private int _version;

        Node Locate(Node x, IEnumerator<TLetter> key)
        {
            if (x == null || !key.MoveNext()) return x;
            var c = key.Current;
            return Locate(x[c], key);
        }

        Node Put(Node x, IEnumerator<TLetter> key, TValue value, bool add)
        {
            if (x == null)
                x = new Node();
            if (!key.MoveNext()) {
                if (!x.HasValue)
                    _count++;
                else if (add)
                    throw new ArgumentException(nameof(key));
                _version++;
                x.Value = value;
                return x;
            }
            var c = key.Current;
            x[c] = Put(x[c], key, value, add);
            return x;
        }

        void Collect(Node x, List<TLetter> prefix, Queue<KeyValuePair<IEnumerable<TLetter>, TValue>> results)
        {
            if (x == null) return;
            if (x.HasValue) results.Enqueue(new KeyValuePair<IEnumerable<TLetter>, TValue>(prefix.ToArray(), x.Value));

            var last = prefix.Count;
            prefix.Add(default(TLetter));
            foreach (var c in x) {
                prefix[last] = c;
                Collect(x[c], prefix, results);
            }
            prefix.RemoveAt(last);
        }

        void Collect(Node x, List<TLetter> prefix, Queue<IEnumerable<TLetter>> results)
        {
            if (x == null) return;
            if (x.HasValue) results.Enqueue(prefix.ToArray());

            var last = prefix.Count;
            prefix.Add(default(TLetter));
            foreach (var c in x) {
                prefix[last] = c;
                Collect(x[c], prefix, results);
            }
            prefix.RemoveAt(last);
        }

        void Collect(Node x, List<TLetter> prefix, List<TLetter?> pattern, Queue<IEnumerable<TLetter>> results)
        {
            if (x == null) return;
            var d = prefix.Count;
            if (d == pattern.Count) {
                if (x.HasValue)
                    results.Enqueue(prefix.ToList());
                return;
            }
            var p = pattern[d];
            var last = prefix.Count;
            prefix.Add(default(TLetter));
            if (!p.HasValue) {
                foreach (var c in x) {
                    prefix[last] = c;
                    Collect(x[c], prefix, pattern, results);
                }
            } else {
                var c = p.GetValueOrDefault();
                prefix[last] = c;
                Collect(x[c], prefix, pattern, results);
            }
            prefix.RemoveAt(last);
        }

        public TValue this[IEnumerable<TLetter> key] {
            get {
                if (TryGetValue(key, out TValue value))
                    return value;
                throw new KeyNotFoundException();
            }
            set {
                _root = Put(_root, key.GetEnumerator(), value, add: false);
            }
        }

        public int Count { get { return _count; } }

        public bool IsReadOnly { get { return false; } }

        public ICollection<IEnumerable<TLetter>> Keys {
            get {
                return new ReadOnlyCollectionToCollectionAdapter<IEnumerable<TLetter>>(_count, this.Select(p => p.Key));
            }
        }

        public ICollection<TValue> Values {
            get {
                return new ReadOnlyCollectionToCollectionAdapter<TValue>(_count, this.Select(p => p.Value));
            }
        }

        public void Add(KeyValuePair<IEnumerable<TLetter>, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(IEnumerable<TLetter> key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            _root = Put(_root, key.GetEnumerator(), value, add: true);
        }

        public void Clear()
        {
            _count = 0;
            _root = null;
        }

        public bool Contains(KeyValuePair<IEnumerable<TLetter>, TValue> item)
        {
            TValue value;
            return TryGetValue(item.Key, out value) == true && EqualityComparer<TValue>.Default.Equals(value, item.Value);
        }

        public bool ContainsKey(IEnumerable<TLetter> key)
        {
            TValue value;
            return TryGetValue(key, out value) == true;
        }

        public void CopyTo(KeyValuePair<IEnumerable<TLetter>, TValue>[] array, int arrayIndex)
        {
            foreach (var item in this)
                array[arrayIndex++] = item;
        }

        public IEnumerator<KeyValuePair<IEnumerable<TLetter>, TValue>> GetEnumerator()
        {
            var results = new Queue<KeyValuePair<IEnumerable<TLetter>, TValue>>();
            Collect(_root, new List<TLetter>(), results);
            return results.GetEnumerator();
        }

        public IEnumerable<IEnumerable<TLetter>> GetKeysWithPrefix(IEnumerable<TLetter> prefix)
        {
            var results = new Queue<IEnumerable<TLetter>>();
            var node = Locate(_root, prefix.GetEnumerator());
            Collect(node, prefix.ToList(), results);
            return results;
        }

        public int GetLengthLongestPrefixOf(IEnumerable<TLetter> query)
        {
            return GetLengthLongestPrefixOf(_root, query.GetEnumerator(), 0, -1);
        }

        int GetLengthLongestPrefixOf(Node x, IEnumerator<TLetter> query, int d, int length)
        {
            if (x == null)
                return length;
            if (x.HasValue)
                length = d;
            if (!query.MoveNext())
                return length;
            var c = query.Current;
            return GetLengthLongestPrefixOf(x[c], query, d + 1, length);
        }

        public IEnumerable<IEnumerable<TLetter>> Match(IEnumerable<TLetter?> pattern)
        {
            var results = new Queue<IEnumerable<TLetter>>();
            Collect(_root, new List<TLetter>(), pattern.ToList(), results);
            return results;
        }

        public bool Remove(KeyValuePair<IEnumerable<TLetter>, TValue> item)
        {
            var comparer = EqualityComparer<TValue>.Default;
            var count = _count;
            _root = Remove(_root, item.Key.GetEnumerator(), x => x.ResetIf(item.Value));
            return count != _count;
        }

        public bool Remove(IEnumerable<TLetter> key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            var count = _count;
            _root = Remove(_root, key.GetEnumerator(), x => x.Reset());
            return count != _count;
        }

        Node Remove(Node x, IEnumerator<TLetter> key, Func<Node, bool> disposer)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (x == null) return null;
            if (!key.MoveNext()) {
                if (disposer(x))
                    _count--;
            } else {
                var c = key.Current;
                x[c] = Remove(x[c], key, disposer);
            }

            // remove subtrie rooted at x if it is completely empty
            if (!x.IsEmpty)
                return x;
            return null;
        }

        public bool TryGetValue(IEnumerable<TLetter> key, out TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var x = Locate(_root, key.GetEnumerator());
            if (x != null && x.HasValue) {
                value = x.Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}