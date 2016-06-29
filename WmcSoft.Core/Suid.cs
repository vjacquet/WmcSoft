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
using System.Text.RegularExpressions;

namespace WmcSoft
{
    public struct Suid : IFormattable, IComparable, IComparable<Suid>, IEquatable<Suid>
    {
        static Regex _validator = new Regex("^[A-Za-z0-9_-]{22}$");

        public static readonly Suid Empty = new Suid(Guid.Empty);

        readonly string _storage;

        private Suid(string g, PiecewiseConstruct tag) {
            _storage = g;
        }

        public Suid(string g) {
            if (g == null) throw new ArgumentNullException("g");

            _storage = (g.Length != 22 || !_validator.IsMatch(g))
                ? Encode(new Guid(g))
                : g;
        }

        public Suid(Guid guid) {
            _storage = Encode(guid);
        }

        public static Suid NewSuid() {
            return new Suid(Guid.NewGuid());
        }

        public static bool operator ==(Suid x, Suid y) {
            return x.Equals(y);
        }

        public static bool operator !=(Suid x, Suid y) {
            return !x.Equals(y);
        }

        public static bool IsValid(string input) {
            return input != null && input.Length == 22 && _validator.IsMatch(input);
        }

        public static Suid Parse(string input) {
            if (input == null) throw new ArgumentNullException("input");

            Suid suid;
            if (!TryParse(input, out suid))
                throw new FormatException();
            return suid;
        }

        public static bool TryParse(string input, out Suid result) {
            if (input.Length != 22 || !_validator.IsMatch(input)) {
                Guid guid;
                if (!Guid.TryParse(input, out guid)) {
                    result = Empty;
                    return false;
                }
                input = Encode(guid);
            }
            result = new Suid(input, PiecewiseConstruct.Tag);
            return true;
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            if (format == null || format == "G" || format == "g")
                return _storage ?? Empty._storage;

            var guid = Decode(_storage);
            return guid.ToString(format, formatProvider);
        }

        public override string ToString() {
            return ToString(null, null);
        }

        public override int GetHashCode() {
            if (_storage == null || _storage == Empty._storage)
                return 0;
            return _storage.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(Suid))
                return false;
            return Equals((Suid)obj);
        }

        public bool Equals(Suid other) {
            var x = _storage ?? Empty._storage;
            var y = other._storage ?? Empty._storage;
            return x == y;
        }

        public int CompareTo(Suid other) {
            var x = _storage ?? Empty._storage;
            var y = other._storage ?? Empty._storage;

            return StringComparer.InvariantCulture.Compare(x, y);
        }

        public int CompareTo(object obj) {
            if (obj == null)
                return 1;
            if (obj.GetType() != typeof(Suid))
                throw new ArgumentException();
            return CompareTo((Suid)obj);
        }

        #region Helpers

        static Guid Decode(string value) {
            value = value.Replace('-', '+').Replace('_', '/');
            byte[] buffer = Convert.FromBase64String(value + "==");
            return new Guid(buffer);
        }

        static string Encode(Guid guid) {
            string encoded = Convert.ToBase64String(guid.ToByteArray());
            encoded = encoded.Replace('+', '-').Replace('/', '_');
            return encoded.Substring(0, 22);
        }

        #endregion
    }
}
