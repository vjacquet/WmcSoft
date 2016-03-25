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
        private static readonly string[] EmptyArray = new string[0];
        public static readonly Strings Empty = new Strings(EmptyArray);

        private readonly string _value;
        private readonly string[] _values;

        private Strings(string value, string[] values) {
            _value = value;
            _values = values;
        }

        public Strings(string value) {
            _value = value;
            _values = value == null ? EmptyArray : null;
        }

        public Strings(string[] values) {
            _value = null;
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
            get { return _value != null ? 1 : Values.Length; }
        }

        public string this[int index]
        {
            get {
                if (index == 0 && _value != null)
                    return _value;
                return Values[index]; // throws the regular exception.
            }
        }

        public override string ToString() {
            if (_value == null)
                return _value;

            switch (Values.Length) {
            case 0: return string.Empty;
            case 1: return Values[0];
            default: return string.Join(",", Values);
            }
        }

        public string[] ToArray() {
            if (_value != null)
                return new[] { _value };
            return Values;
        }

        private int IndexOf(string item) {
            if (_value != null) {
                return string.Equals(_value, item, StringComparison.Ordinal) ? 0 : -1;
            }

            var values = Values;
            for (int i = 0; i < values.Length; i++) {
                if (string.Equals(values[i], item, StringComparison.Ordinal)) {
                    return i;
                }
            }
            return -1;
        }

        private void CopyTo(string[] array, int arrayIndex) {
            if (_value == null) {
                var values = Values;
                Array.Copy(values, 0, array, arrayIndex, values.Length);
                return;
            }

            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < 1)
                throw new ArgumentException(
                    $"'{nameof(array)}' is not long enough to copy all the items in the collection. Check '{nameof(arrayIndex)}' and '{nameof(array)}' length.");

            array[arrayIndex] = _value;
        }

        public IEnumerator<string> GetEnumerator() {
            if (_value == null) {
                var values = Values;
                var length = values.Length;
                for (int i = 0; i < length; i++) {
                    yield return values[i];

                }
            } else {
                yield return _value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public static bool IsNullOrEmpty(Strings value) {
            var values = value.Values;
            if (values == null)
                return true;
            if (!String.IsNullOrEmpty(value._value))
                return false;

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
            return new Strings(null, values);
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
            var values = Values;
            if (values == null)
                return _value == null ? 0 : _value.GetHashCode();

            var h = values[0].GetHashCode();
            for (var i = 1; i < values.Length; i++) {
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
