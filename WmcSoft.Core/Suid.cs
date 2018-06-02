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
    /// <summary>
    /// Represents the short form of an globally unique identifier.The string version takes 22 chars representable in urls.
    /// </summary>
    /// <remarks>The string format is adapted from Base64, so 0 is encoded as '0' instead of 'A'</remarks>
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

        private Suid(byte[] g, PiecewiseConstruct tag)
        {
            _storage = g;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Suid"/> structure by using the value represented by the specified string.
        /// </summary>
        /// <param name="g">A string that contains a GUID.</param>
        /// <exception cref="ArgumentNullException"><paramref name="g"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException">The format of <paramref name="g"/> is invalid.</exception>
        public Suid(string g)
        {
            if (g == null) throw new ArgumentNullException("g");

            _storage = (g.Length == 22 && _validator.IsMatch(g))
                ? Decode(g)
                : new Guid(g).ToByteArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Suid"/> structure by a <see cref="Guid"/>.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        public Suid(Guid guid)
        {
            _storage = guid.ToByteArray();
        }

        /// <summary>
        /// Initializes a new instance of the Guid structure by using the specified array of bytes.
        /// </summary>
        /// <param name="b">A 16-element byte array containing values with which to initialize the GUID.</param>
        /// <exception cref="ArgumentNullException"><paramref name="b"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="b"/> is not 16 bytes long.</exception>
        public Suid(params byte[] b)
        {
            if (b == null) throw new ArgumentNullException("b");
            if (b.Length != 16) throw new ArgumentException("b");
            _storage = new byte[16];
            Array.Copy(b, _storage, b.Length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Suid"/> structure.
        /// </summary>
        /// <returns>A new GUID object.</returns>
        public static Suid NewSuid()
        {
            return new Suid(Guid.NewGuid());
        }

        /// <summary>
        /// Returns a 16-element byte array that contains the value of this instance.
        /// </summary>
        /// <returns>A 16-element byte array.</returns>
        public byte[] ToByteArray()
        {
            return (byte[])(_storage ?? Empty._storage).Clone();
        }

        /// <summary>
        /// Converts <see cref="Guid"/> to a <see cref="Suid"/>.
        /// </summary>
        /// <param name="g">The GUID.</param>
        public static implicit operator Suid(Guid g)
        {
            return new Suid(g);
        }

        /// <summary>
        /// Converts <see cref="Suid"/> to a <see cref="Guid"/>.
        /// </summary>
        /// <param name="g">The GUID.</param>
        public static implicit operator Guid(Suid g)
        {
            if (g._storage == null)
                return Guid.Empty;
            return new Guid(g._storage);
        }

        /// <summary>
        /// Indicates whether the values of two specified <see cref="Suid"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare. </param>
        /// <param name="y">The second object to compare. </param>
        /// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Suid x, Suid y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Indicates whether the values of two specified <see cref="Suid"/> objects are not equal.
        /// </summary>
        /// <param name="x">The first object to compare. </param>
        /// <param name="y">The second object to compare. </param>
        /// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Suid x, Suid y)
        {
            return !x.Equals(y);
        }

        /// <summary>
        /// Converts the string representation of a GUID to the equivalent <see cref="Suid"/> structure.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A structure that contains the value that was parsed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException"><paramref name="input"/> is not in a recognized format.</exception>
        public static Suid Parse(string input)
        {
            if (input == null) throw new ArgumentNullException("input");

            Suid suid;
            if (!TryParse(input, out suid))
                throw new FormatException();
            return suid;
        }

        /// <summary>
        /// Converts the string representation of a GUID to the equivalent <see cref="Suid"/> structure.
        /// </summary>
        /// <param name="input">The GUID to convert.</param>
        /// <param name="result">
        ///   The structure that will contain the parsed value. 
        ///   If the method returns <c>true</c>, result contains a valid Guid. 
        ///   If the method returns <c>false</c>, result equals <see cref="Suid.Empty"/>.
        /// </param>
        /// <returns>true if the parse operation was successful; otherwise, false.</returns>
        public static bool TryParse(string input, out Suid result)
        {
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

        /// <summary>
        /// Converts the string representation of a GUID to the equivalent <see cref="Suid"/> structure, 
        /// provided that the string is in the specified format.
        /// </summary>
        /// <param name="input">The GUID to convert.</param>
        /// <param name="format">One of the following specifiers that indicates 
        /// the exact format to use when interpreting <paramref name="input"/>: "S", "N", "D", "B", "P", or "X".</param>
        /// <param name="result">
        ///   The structure that will contain the parsed value. 
        ///   If the method returns <c>true</c>, result contains a valid Guid. 
        ///   If the method returns <c>false</c>, result equals <see cref="Suid.Empty"/>.
        /// </param>
        /// <returns>true if the parse operation was successful; otherwise, false.</returns>
        public static bool TryParseExact(string input, string format, out Suid result)
        {
            if (!string.IsNullOrEmpty(input)) {
                switch (format) {
                case "S":
                case "s":
                    if (input.Length == 22 && IsValid(input)) {
                        result = new Suid(Decode(input), PiecewiseConstruct.Tag);
                        return true;
                    }
                    break;
                default:
                    Guid guid;
                    if (Guid.TryParseExact(input, format, out guid)) {
                        result = new Suid(guid);
                        return true;
                    }
                    break;
                }
            }
            result = Suid.Empty;
            return false;
        }


        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            if (_storage == null || _storage == Empty._storage)
                return 0;
            return _storage.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Suid))
                return false;
            return Equals((Suid)obj);
        }

        #region IEquatable<Suid> members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Suid other)
        {
            return CompareTo(other) == 0;
        }

#endregion

        #region IComparable<Suid> members

        /// <summary>
        ///   Compares the current instance with another object of the same type and returns
        ///   an integer that indicates whether the current instance precedes, follows, or
        ///   occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other"> An object to compare with this instance.</param>
        /// <returns>
        ///   A value that indicates the relative order of the objects being compared. The
        ///   return value has these meanings: Value Meaning Less than zero This instance precedes
        ///   other in the sort order. Zero This instance occurs in the same position in the
        ///   sort order as other. Greater than zero This instance follows other in the sort
        ///   order.
        /// </returns>
        /// <remarks><see cref="Empty"/> is smaller than any other value.</remarks>
        public int CompareTo(Suid other)
        {
            var x = _storage ?? Empty._storage;
            var y = other._storage ?? Empty._storage;
            int i = 0;
            var result = x[0].CompareTo(y[0]);
            while (result == 0 && ++i < 16)
                result = x[i].CompareTo(y[i]);
            return result;
        }

#endregion

        #region IComparable members

        /// <summary>
        ///  Compares the current instance with another object of the same type and returns
        ///  an integer that indicates whether the current instance precedes, follows, or
        ///  occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <exception cref="ArgumentException">obj is not the same type as this instance.</exception>
        /// <returns>
        ///   A value that indicates the relative order of the objects being compared. The
        ///   return value has these meanings: Value Meaning Less than zero This instance precedes
        ///   obj in the sort order. Zero This instance occurs in the same position in the
        ///   sort order as obj. Greater than zero This instance follows obj in the sort order.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj.GetType() != typeof(Suid))
                throw new ArgumentException();
            return CompareTo((Suid)obj);
        }

        #endregion

        #region IFormattable members

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <param name="format">
        ///   The format to use.
        ///   -or- 
        ///   A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="IFormattable"/> implementation.
        /// </param>
        /// <returns>The value of the current instance in the specified format.</returns>
        /// <remarks>Supports <see cref="Guid"/> five format in addition to "S" for the 22 chars short version.</remarks>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <param name="format">
        ///   The format to use.
        ///   -or- 
        ///   A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="IFormattable"/> implementation.
        /// </param>
        /// <param name="formatProvider">
        ///   The provider to use to format the value.
        ///   -or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting
        ///   of the operating system.
        /// </param>
        /// <returns>The value of the current instance in the specified format.</returns>
        /// <remarks>Supports <see cref="Guid"/> five format in addition to "S" for the 22 chars short version.</remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null || format == "S" || format == "s")
                return Encode(_storage ?? Empty._storage);

            var guid = new Guid(_storage);
            return guid.ToString(format, formatProvider);
        }

        #endregion

        #region Helpers

        static bool IsValid(string input)
        {
            return input != null && input.Length == 22 && _validator.IsMatch(input);
        }

        static byte Decode(char c)
        {
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

        static byte[] Decode(string value)
        {
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

        static string Encode(byte[] bytes)
        {
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
