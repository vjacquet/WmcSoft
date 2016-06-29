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
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WmcSoft
{
    public struct Suid : IFormattable, IComparable, IComparable<Suid>, IEquatable<Suid>
    {
        static readonly char[] _encoding = new char[]{
            '0','1','2','3','4','5','6','7', '8','9',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            '-','_'
        };
        static Regex _validator = new Regex("^[A-Za-z0-9_-]{21}[0GWm]$");
        public static readonly Suid Empty = new Suid(Guid.Empty);

        readonly byte[] _storage;

        private Suid(byte[] g, PiecewiseConstruct tag) {
            _storage = g;
        }

        public Suid(string g) {
            if (g == null) throw new ArgumentNullException("g");

            _storage = (g.Length == 22 && _validator.IsMatch(g))
                ? Decode(g)
                : new Guid(g).ToByteArray();
        }

        public Suid(Guid guid) {
            _storage = guid.ToByteArray();
        }

        public Suid(params byte[] bytes) {
            if (bytes == null) throw new ArgumentNullException("bytes");
            if (bytes.Length > 16) throw new ArgumentException("bytes");
            _storage = new byte[16];
            Array.Copy(bytes, _storage, bytes.Length);
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
            return input != null
                && input.Length == 22
                && _validator.IsMatch(input);
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
                result = new Suid(guid);
            } else {
                result = new Suid(Decode(input), PiecewiseConstruct.Tag);
            }
            return true;
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            if (format == null || format == "G" || format == "g")
                return Encode(_storage ?? Empty._storage);

            var guid = new Guid(_storage);
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
            return CompareTo(other) == 0;
        }

        public int CompareTo(Suid other) {
            var x = _storage ?? Empty._storage;
            var y = other._storage ?? Empty._storage;
            int i = 0;
            var result = x[0].CompareTo(y[0]);
            while (result == 0 && ++i < 16)
                result = x[i].CompareTo(y[i]);
            return result;
        }

        public int CompareTo(object obj) {
            if (obj == null)
                return 1;
            if (obj.GetType() != typeof(Suid))
                throw new ArgumentException();
            return CompareTo((Suid)obj);
        }

        #region Helpers

        static byte Decode(char c) {
            unchecked {
                if (c <= '9')
                    return (c == '-') ? (byte)62 : (byte)(c - '0');
                if (c <= 'Z')
                    return (byte)(c - 'A' + 10);
                if (c == '_')
                    return (byte)63;
                return (byte)(c - 'a' + 36);
            }
        }

        static byte[] Decode(string value) {
            unchecked {
                var bytes = new byte[16];
                Debug.Assert(value.Length == 22);
                byte a, b, c, d;
                for (int i = 0, j = 0; i < 20; i += 4, j += 3) {
                    a = Decode(value[i]);
                    b = Decode(value[i + 1]);
                    c = Decode(value[i + 2]);
                    d = Decode(value[i + 3]);
                    bytes[j] = (byte)((a << 2) | (b >> 4));
                    bytes[j + 1] = (byte)(b << 4 | c >> 2);
                    bytes[j + 2] = (byte)(c << 6 | d);
                }
                a = Decode(value[20]);
                b = Decode(value[21]);
                bytes[15] = (byte)((a << 2) | (b >> 4));
                return bytes;
            }
        }

        static string Encode(byte[] bytes) {
            var chars = new char[22];

            for (int i = 0, j = 0; i < 15; i += 3, j += 4) {
                chars[j] = _encoding[(bytes[i] & 0xfc) >> 2];
                chars[j + 1] = _encoding[((bytes[i] & 0x03) << 4) | ((bytes[i + 1] & 0xf0) >> 4)];
                chars[j + 2] = _encoding[((bytes[i + 1] & 0x0f) << 2) | ((bytes[i + 2] & 0xc0) >> 6)];
                chars[j + 3] = _encoding[(bytes[i + 2] & 0x3f)];
            }
            chars[20] = _encoding[(bytes[15] & 0xfc) >> 2];
            chars[21] = _encoding[(bytes[15] & 0x03) << 4];
            return new string(chars);
        }

        #endregion
    }
}
