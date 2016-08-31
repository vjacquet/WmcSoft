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

            public Node() {
                _nodes = new SortedList<TLetter, Node>();
            }

            public bool HasValue {
                get { return _hasValue; }
            }

            public TValue Value {
                get { return _value; }
                set { _value = value; }
            }

            public bool Reset() {
                if (_hasValue) {
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
                get { return _nodes[index]; }
                set { _nodes[index] = value; }
            }

            public IEnumerator<TLetter> GetEnumerator() {
                return _nodes.Keys.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        #endregion

        private Node _root;
        private int _count;

        Node Locate(Node x, IReadOnlyList<TLetter> key, int d) {
            if (x == null || d == key.Count) return x;
            var c = key[d];
            return Locate(x[c], key, d + 1);
        }

        Node Put(Node x, IReadOnlyList<TLetter> key, int d, TValue value, bool add) {
            if (x == null) return new Node();
            if (d == key.Count) {
                if (!x.HasValue)
                    _count++;
                else if (add)
                    throw new ArgumentException(nameof(key));
                x.Value = value;
                return x;
            }
            var c = key[d];
            x[c] = Put(x[c], key, d + 1, value, add);
            return x;
        }

        void Collect(Node x, List<TLetter> prefix, Queue<IReadOnlyList<TLetter>> results) {
            if (x == null) return;
            if (x.HasValue) results.Enqueue(prefix.ToArray());
            foreach (var c in x) {
                prefix.Add(c);
                Collect(x[c], prefix, results);
                prefix.RemoveAt(prefix.Count - 1);
            }
        }

        public TValue this[IReadOnlyList<TLetter> key] {
            get {
                TValue value;
                if (TryGetValue(key, out value))
                    return value;
                throw new KeyNotFoundException();
            }
            set {
                _root = Put(_root, key, 0, value, add: false);
            }
        }

        public int Count { get { return _count; } }

        public bool IsReadOnly { get { return false; } }

        public ICollection<IReadOnlyList<TLetter>> Keys {
            get {
                return new ReadOnlyCollectionToCollectionAdapter<IReadOnlyList<TLetter>>(_count, this.Select(p => p.Key));
            }
        }

        public ICollection<TValue> Values {
            get {
                return new ReadOnlyCollectionToCollectionAdapter<TValue>(_count, this.Select(p => p.Value));
            }
        }

        public void Add(KeyValuePair<IReadOnlyList<TLetter>, TValue> item) {
            Add(item.Key, item.Value);
        }

        public void Add(IReadOnlyList<TLetter> key, TValue value) {
            _root = Put(_root, key, 0, value, add: true);
        }

        public void Clear() {
            _count = 0;
            _root = null;
        }

        public bool Contains(KeyValuePair<IReadOnlyList<TLetter>, TValue> item) {
            TValue value;
            return TryGetValue(item.Key, out value) == true && EqualityComparer<TValue>.Default.Equals(value, item.Value);
        }

        public bool ContainsKey(IReadOnlyList<TLetter> key) {
            TValue value;
            return TryGetValue(key, out value) == true;
        }

        public void CopyTo(KeyValuePair<IReadOnlyList<TLetter>, TValue>[] array, int arrayIndex) {
            foreach (var item in this)
                array[arrayIndex++] = item;
        }

        public IEnumerator<KeyValuePair<IReadOnlyList<TLetter>, TValue>> GetEnumerator() {
            throw new NotImplementedException();
        }

        public IEnumerable<IReadOnlyList<TLetter>> GetKeysWithPrefix(IReadOnlyList<TLetter> sequence) {
            var results = new Queue<IReadOnlyList<TLetter>>();
            var x = Locate(_root, sequence, 0);
            return results;
        }

        public int GetLengthLongestPrefixOf(IReadOnlyList<TLetter> sequence) {
            return GetLengthLongestPrefixOf(_root, sequence, 0, -1);
        }

        int GetLengthLongestPrefixOf(Node x, IReadOnlyList<TLetter> sequence, int d, int length) {
            if (x == null)
                return length;
            if (x.HasValue)
                length = d;
            if (d == sequence.Count)
                return length;
            var c = sequence[d];
            return GetLengthLongestPrefixOf(x[c], sequence, d + 1, length);
        }

        public IEnumerable<IReadOnlyList<TLetter>> Match(IReadOnlyList<TLetter?> sequence) {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<IReadOnlyList<TLetter>, TValue> item) {
            throw new NotImplementedException();
        }

        public bool Remove(IReadOnlyList<TLetter> key) {
            var count = _count;
            _root = Remove(_root, key, 0);
            return count != _count;
        }

        Node Remove(Node x, IReadOnlyList<TLetter> key, int d) {
            if (x == null) return null;
            if (d == key.Count) {
                if (x.Reset())
                    _count--;
            } else {
                var c = key[d];
                x[c] = Remove(x[c], key, d + 1);
            }

            // remove subtrie rooted at x if it is completely empty
            if (!x.IsEmpty)
                return x;
            return null;
        }

        public bool TryGetValue(IReadOnlyList<TLetter> key, out TValue value) {
            var x = Locate(_root, key, 0);
            if (x != null && x.HasValue) {
                value = x.Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}