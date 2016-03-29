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

namespace WmcSoft.Text
{
    /// <summary>
    /// Represents zero, one, or many strings.
    /// </summary>
    public struct Strings : IReadOnlyList<string>, IEquatable<Strings>, IEquatable<string>, IEquatable<string[]>
    {
        public struct Enumerator : IEnumerator<string>
        {
            readonly string[] _values;
            int index;

            internal Enumerator(string[] values) {
                _values = values;
                index = -1;
            }

            public string Current { get { return _values[index]; } }

            object IEnumerator.Current { get { return Current; } }

            public void Dispose() {
            }

            public bool MoveNext() {
                if (index < _values.Length) {
                    ++index;
                    return index < _values.Length;
                }
                return false;
            }

            public void Reset() {
                index = -1;
            }
        }

        private static readonly string[] EmptyArray = new string[0];
        public static readonly Strings Empty = new Strings(EmptyArray);

        private readonly string[] _values;

        public Strings(params string[] values) {
            _values = values;
        }

        private string[] Values { get { return _values ?? EmptyArray; } }

        public static implicit operator Strings(string value) {
            return new Strings(value);
        }

        public static implicit operator Strings(string[] values) {
            return new Strings(values);
        }

        public static explicit operator string(Strings values) {
            return values.ToString();
        }

        public static explicit operator string[] (Strings value) {
            return value.ToArray();
        }

        public int Count
        {
            get { return _values == null ? 0 : _values.Length; }
        }

        public string this[int index]
        {
            get {
                return Values[index]; // throws the regular exception.
            }
        }

        public override string ToString() {
            var values = Values;
            switch (Values.Length) {
            case 0: return string.Empty;
            case 1: return Values[0];
            default: return string.Join(",", Values);
            }
        }

        public string[] ToArray() {
            return Values;
        }

        public int IndexOf(string item) {
            if (_values == null)
                return -1;
            for (int i = 0; i < _values.Length; i++) {
                if (string.Equals(_values[i], item, StringComparison.Ordinal)) {
                    return i;
                }
            }
            return -1;
        }

        public bool Contains(string item) {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(string[] array, int arrayIndex) {
            if (_values == null)
                return;
            Array.Copy(_values, 0, array, arrayIndex, _values.Length);
        }

        public Enumerator GetEnumerator() {
            return new Enumerator(Values);
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator() {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public static bool IsNullOrEmpty(Strings value) {
            var values = value.Values;
            switch (values.Length) {
            case 0: return true;
            case 1: return string.IsNullOrEmpty(value._values[0]);
            default: return false;
            }
        }

        public static Strings Concat(Strings x, Strings y) {
            var cx = x.Count;
            if (cx == 0)
                return y;

            var cy = y.Count;
            if (cy == 0)
                return x;

            var values = new string[cx + cy];
            x.CopyTo(values, 0);
            y.CopyTo(values, cx);
            return new Strings(values);
        }

        public static bool Equals(Strings x, Strings y) {
            var count = x.Count;
            if (count != y.Count)
                return false;

            for (var i = 0; i < count; i++) {
                if (x[i] != y[i])
                    return false;
            }
            return true;
        }

        public static bool operator ==(Strings x, Strings y) {
            return Equals(x, y);
        }

        public static bool operator !=(Strings x, Strings y) {
            return !Equals(x, y);
        }

        public bool Equals(Strings other) {
            return Equals(this, other);
        }

        public static bool operator ==(Strings left, object right) {
            return left.Equals(right);
        }

        public static bool operator !=(Strings left, object right) {
            return !left.Equals(right);
        }

        public static bool operator ==(object left, Strings right) {
            return right.Equals(left);
        }

        public static bool operator !=(object left, Strings right) {
            return !right.Equals(left);
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return Equals(this, Strings.Empty);

            if (obj is string)
                return Equals(this, (string)obj);

            if (obj is string[])
                return Equals(this, (string[])obj);

            if (obj is Strings)
                return Equals(this, (Strings)obj);

            return false;
        }

        public override int GetHashCode() {
            if (_values == null)
                return 0;
            var values = _values;
            var h = 0;
            for (var i = 0; i < values.Length; i++) {
                h = ((h << 5) + h) ^ values[i].GetHashCode();
            }
            return h;
        }

        public bool Equals(string other) {
            return Equals(new Strings(other));
        }

        public bool Equals(string[] other) {
            return Equals(new Strings(other));
        }
    }
}
