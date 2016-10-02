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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WmcSoft.Benchmark;
using Microsoft.Extensions.Primitives;
using WmcSoft.Text;

namespace ImplBench
{
    /// <summary>
    /// Compares Strings implementation with https://raw.githubusercontent.com/aspnet/Common/dev/src/Microsoft.Extensions.Primitives/StringValues.cs
    /// </summary>
    [Benchmark(Iterations = 2000)]
    public class StringsBench
    {
        const int Count = 8192 * 4;
        static readonly string[] samples = new[] { "a", "b", "c", "e", "f", "g", "h", "i", "j", "k", "l", "m", "lorem", "ipsum", "dolor", "sit", "amet" };
        static StringValues[] stringValues;
        static Strings[] strings;

        static StringValues[] stringValues2;
        static Strings[] strings2;

        static int Values;
        static string LatestString;
        static string[] LatestStringArray;

        static string Sample(Random random) {
            return samples[random.Next(0, samples.Length)];
        }

        static IEnumerable<string> Samples(Random random, int count) {
            while (count-- > 0)
                yield return Sample(random);
        }

        public static void Init(string[] args) {
            stringValues = new StringValues[Count];
            strings = new Strings[Count];

            var random = new Random(1664);
            for (int i = 0; i < Count; i++) {
                var count = random.Next(90, 100);
                if (count <= 91) {
                    var s = Sample(random);
                    stringValues[i] = new StringValues(s);
                    strings[i] = new Strings(s);
                } else {
                    var s = Samples(random, count - 90).ToArray();
                    stringValues[i] = new StringValues(s);
                    strings[i] = new Strings(s);
                }
            }
        }

        public static void Reset() {
            Values = 0;
        }

        public static void Check() {
        }

        [Measure]
        public static void MeasureCtorOnStringValues() {
            var result = new StringValues[Count];
            for (int n = 0; n < Count; n++) {
                var reference = strings[n].ToArray();
                if (reference.Length == 1)
                    result[n] = new StringValues(reference[0]);
                else
                    result[n] = new StringValues(reference);
            }
            stringValues2 = result;
        }

        [Measure]
        public static void MeasureCtorOnStrings() {
            var result = new Strings[Count];
            for (int n = 0; n < Count; n++) {
                var reference = strings[n].ToArray();
                if (reference.Length == 1)
                    result[n] = new Strings(reference[0]);
                else
                    result[n] = new Strings(reference);
            }
            strings2 = result;
        }

        [Measure]
        public static void MeasureForEachOnStringValues() {
            int count = 0;
            for (int n = 0; n < Count; n++) {
                foreach (var value in stringValues[n]) {
                    count++;
                }
            }
            Values = count;
        }

        [Measure]
        public static void MeasureForEachOnStrings() {
            int count = 0;
            for (int n = 0; n < Count; n++) {
                foreach (var value in strings[n]) {
                    count++;
                }
            }
            Values = count;
        }

        [Measure]
        public static void MeasureForOnStringValues() {
            int count = 0;
            for (int n = 0; n < Count; n++) {
                var candidate = stringValues[n];
                for (int i = 0; i < candidate.Count; i++) {
                    count++;
                }
            }
            Values = count;
        }

        [Measure]
        public static void MeasureForOnStrings() {
            int count = 0;
            for (int n = 0; n < Count; n++) {
                var candidate = strings[n];
                for (int i = 0; i < candidate.Count; i++) {
                    count++;
                }
            }
            Values = count;
        }

        [Measure]
        public static void MeasureGetAsStringOnStringValues() {
            int count = 0;
            for (int n = 0; n < Count; n++) {
                var candidate = stringValues[n];
                for (int i = 0; i < candidate.Count; i++) {
                    LatestString = (string)candidate;
                }
            }
            Values = count;
        }

        [Measure]
        public static void MeasureGetAsStringOnStrings() {
            int count = 0;
            for (int n = 0; n < Count; n++) {
                var candidate = strings[n];
                for (int i = 0; i < candidate.Count; i++) {
                    LatestString = (string)candidate;
                }
            }
            Values = count;
        }

        [Measure]
        public static void MeasureGetAsArrayOnStringValues() {
            int count = 0;
            for (int n = 0; n < Count; n++) {
                var candidate = stringValues[n];
                for (int i = 0; i < candidate.Count; i++) {
                    LatestStringArray = (string[])candidate;
                }
            }
            Values = count;
        }

        [Measure]
        public static void MeasureGetAsArrayOnStrings() {
            int count = 0;
            for (int n = 0; n < Count; n++) {
                var candidate = strings[n];
                for (int i = 0; i < candidate.Count; i++) {
                    LatestStringArray = (string[])candidate;
                }
            }
            Values = count;
        }
    }
}

namespace Microsoft.Extensions.Primitives
{
    /// <summary>
    /// Represents zero/null, one, or many strings in an efficient way.
    /// </summary>
    public struct StringValues : IList<string>, IReadOnlyList<string>, IEquatable<StringValues>, IEquatable<string>, IEquatable<string[]>
    {
        private static readonly string[] EmptyArray = new string[0];
        public static readonly StringValues Empty = new StringValues(EmptyArray);

        private readonly string _value;
        private readonly string[] _values;

        public StringValues(string value) {
            _value = value;
            _values = null;
        }

        public StringValues(string[] values) {
            _value = null;
            _values = values;
        }

        public static implicit operator StringValues(string value) {
            return new StringValues(value);
        }

        public static implicit operator StringValues(string[] values) {
            return new StringValues(values);
        }

        public static implicit operator string(StringValues values) {
            return values.GetStringValue();
        }

        public static implicit operator string[] (StringValues value) {
            return value.GetArrayValue();
        }

        public int Count => _value != null ? 1 : (_values?.Length ?? 0);

        bool ICollection<string>.IsReadOnly {
            get { return true; }
        }

        string IList<string>.this[int index] {
            get { return this[index]; }
            set { throw new NotSupportedException(); }
        }

        public string this[int index] {
            get {
                if (_values != null) {
                    return _values[index]; // may throw
                }
                if (index == 0 && _value != null) {
                    return _value;
                }
                return EmptyArray[0]; // throws
            }
        }

        public override string ToString() {
            return GetStringValue() ?? string.Empty;
        }

        private string GetStringValue() {
            if (_values == null) {
                return _value;
            }
            switch (_values.Length) {
            case 0: return null;
            case 1: return _values[0];
            default: return string.Join(",", _values);
            }
        }

        public string[] ToArray() {
            return GetArrayValue() ?? EmptyArray;
        }

        private string[] GetArrayValue() {
            if (_value != null) {
                return new[] { _value };
            }
            return _values;
        }

        int IList<string>.IndexOf(string item) {
            return IndexOf(item);
        }

        private int IndexOf(string item) {
            if (_values != null) {
                var values = _values;
                for (int i = 0; i < values.Length; i++) {
                    if (string.Equals(values[i], item, StringComparison.Ordinal)) {
                        return i;
                    }
                }
                return -1;
            }

            if (_value != null) {
                return string.Equals(_value, item, StringComparison.Ordinal) ? 0 : -1;
            }

            return -1;
        }

        bool ICollection<string>.Contains(string item) {
            return IndexOf(item) >= 0;
        }

        void ICollection<string>.CopyTo(string[] array, int arrayIndex) {
            CopyTo(array, arrayIndex);
        }

        private void CopyTo(string[] array, int arrayIndex) {
            if (_values != null) {
                Array.Copy(_values, 0, array, arrayIndex, _values.Length);
                return;
            }

            if (_value != null) {
                if (array == null) {
                    throw new ArgumentNullException(nameof(array));
                }
                if (arrayIndex < 0) {
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex));
                }
                if (array.Length - arrayIndex < 1) {
                    throw new ArgumentException(
                        $"'{nameof(array)}' is not long enough to copy all the items in the collection. Check '{nameof(arrayIndex)}' and '{nameof(array)}' length.");
                }

                array[arrayIndex] = _value;
            }
        }

        void ICollection<string>.Add(string item) {
            throw new NotSupportedException();
        }

        void IList<string>.Insert(int index, string item) {
            throw new NotSupportedException();
        }

        bool ICollection<string>.Remove(string item) {
            throw new NotSupportedException();
        }

        void IList<string>.RemoveAt(int index) {
            throw new NotSupportedException();
        }

        void ICollection<string>.Clear() {
            throw new NotSupportedException();
        }

        public Enumerator GetEnumerator() {
            return new Enumerator(ref this);
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator() {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public static bool IsNullOrEmpty(StringValues value) {
            if (value._values == null) {
                return string.IsNullOrEmpty(value._value);
            }
            switch (value._values.Length) {
            case 0: return true;
            case 1: return string.IsNullOrEmpty(value._values[0]);
            default: return false;
            }
        }

        public static StringValues Concat(StringValues values1, StringValues values2) {
            var count1 = values1.Count;
            var count2 = values2.Count;

            if (count1 == 0) {
                return values2;
            }

            if (count2 == 0) {
                return values1;
            }

            var combined = new string[count1 + count2];
            values1.CopyTo(combined, 0);
            values2.CopyTo(combined, count1);
            return new StringValues(combined);
        }

        public static bool Equals(StringValues left, StringValues right) {
            var count = left.Count;

            if (count != right.Count) {
                return false;
            }

            for (var i = 0; i < count; i++) {
                if (left[i] != right[i]) {
                    return false;
                }
            }

            return true;
        }

        public static bool operator ==(StringValues left, StringValues right) {
            return Equals(left, right);
        }

        public static bool operator !=(StringValues left, StringValues right) {
            return !Equals(left, right);
        }

        public bool Equals(StringValues other) {
            return Equals(this, other);
        }

        public static bool Equals(string left, StringValues right) {
            return Equals(new StringValues(left), right);
        }

        public static bool Equals(StringValues left, string right) {
            return Equals(left, new StringValues(right));
        }

        public bool Equals(string other) {
            return Equals(this, new StringValues(other));
        }

        public static bool Equals(string[] left, StringValues right) {
            return Equals(new StringValues(left), right);
        }

        public static bool Equals(StringValues left, string[] right) {
            return Equals(left, new StringValues(right));
        }

        public bool Equals(string[] other) {
            return Equals(this, new StringValues(other));
        }

        public static bool operator ==(StringValues left, string right) {
            return Equals(left, new StringValues(right));
        }

        public static bool operator !=(StringValues left, string right) {
            return !Equals(left, new StringValues(right));
        }

        public static bool operator ==(string left, StringValues right) {
            return Equals(new StringValues(left), right);
        }

        public static bool operator !=(string left, StringValues right) {
            return !Equals(new StringValues(left), right);
        }

        public static bool operator ==(StringValues left, string[] right) {
            return Equals(left, new StringValues(right));
        }

        public static bool operator !=(StringValues left, string[] right) {
            return !Equals(left, new StringValues(right));
        }

        public static bool operator ==(string[] left, StringValues right) {
            return Equals(new StringValues(left), right);
        }

        public static bool operator !=(string[] left, StringValues right) {
            return !Equals(new StringValues(left), right);
        }

        public static bool operator ==(StringValues left, object right) {
            return left.Equals(right);
        }

        public static bool operator !=(StringValues left, object right) {
            return !left.Equals(right);
        }
        public static bool operator ==(object left, StringValues right) {
            return right.Equals(left);
        }

        public static bool operator !=(object left, StringValues right) {
            return !right.Equals(left);
        }

        public override bool Equals(object obj) {
            if (obj == null) {
                return Equals(this, StringValues.Empty);
            }

            if (obj is string) {
                return Equals(this, (string)obj);
            }

            if (obj is string[]) {
                return Equals(this, (string[])obj);
            }

            if (obj is StringValues) {
                return Equals(this, (StringValues)obj);
            }

            return false;
        }

        public override int GetHashCode() {
            if (_values == null) {
                return _value == null ? 0 : _value.GetHashCode();
            }

            var h = 0;
            for (var i = 0; i < _values.Length; i++) {
                h = ((h << 5) + h) ^ _values[i].GetHashCode();
            }
            return h;
        }

        public struct Enumerator : IEnumerator<string>
        {
            private readonly string[] _values;
            private string _current;
            private int _index;

            public Enumerator(ref StringValues values) {
                _values = values._values;
                _current = values._value;
                _index = 0;
            }

            public bool MoveNext() {
                if (_index < 0) {
                    return false;
                }

                if (_values != null) {
                    if (_index < _values.Length) {
                        _current = _values[_index];
                        _index++;
                        return true;
                    }

                    _index = -1;
                    return false;
                }

                _index = -1; // sentinel value
                return _current != null;
            }

            public string Current => _current;

            object IEnumerator.Current => _current;

            void IEnumerator.Reset() {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose() {
            }
        }
    }
}
