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
using System.Text;

namespace WmcSoft.Text
{
    public static class StringBuilderExtensions
    {
        #region Append

        /// <summary>
        /// Appends <paramref name="repeatCount"/> times the <paramref name="value"/> at the end of this string builder.
        /// </summary>
        /// <param name="self">The <see cref="StringBuilder"/>.</param>
        /// <param name="value">The string to repeat.</param>
        /// <param name="repeatCount">The number of times the string is repeated.</param>
        /// <returns>The string builder.</returns>
        public static StringBuilder Append(this StringBuilder self, string value, int repeatCount)
        {
            switch (repeatCount) {
            case 0:
                return self;
            case 1:
                return self.Append(value);
            default:
                if (repeatCount < 0) throw new ArgumentOutOfRangeException(nameof(repeatCount));
                while (repeatCount-- > 0) {
                    self.Append(value);
                }
                return self;
            }
        }

        #endregion

        #region Append

        /// <summary>
        /// Appends a copy of the specified <see cref="Strip"/> to this instance.
        /// </summary>
        /// <param name="self">The <see cref="StringBuilder"/>.</param>
        /// <param name="value">The <see cref="Strip"/> to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Enlarging the value of this instance would exceed <see cref="StringBuilder.MaxCapacity"/>.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Append(this StringBuilder self, Strip value)
        {
            return (value ?? Strip.Null).AppendTo(self);
        }

        /// <summary>
        /// Appends a copy of a specified substring to this instance.
        /// </summary>
        /// <param name="self">The <see cref="StringBuilder"/>.</param>
        /// <param name="value">The <see cref="Strip"/> that contains the substring to append.</param>
        /// <param name="startIndex">The starting position of the substring within <paramref name="value"/>.</param>
        /// <param name="count">The number of characters in <paramref name="value"/> to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="count"/> less than zero.
        ///   -or- <paramref name="startIndex"/> less than zero.
        ///   -or- <paramref name="startIndex"/> + <paramref name="count"/> is
        ///   greater than the length of value.
        ///   -or- Enlarging the value of this instance would exceed <see cref="StringBuilder.MaxCapacity"/>.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Append(this StringBuilder self, Strip value, int startIndex, int count)
        {
            return (value ?? Strip.Null).AppendTo(self, startIndex, count);
        }

        #endregion

        #region AppendJoin

        /// <summary>
        /// Appends all the elements of an object array, using the specified separator between each element.
        /// </summary>
        /// <param name="self">The <see cref="StringBuilder"/>.</param>
        /// <param name="separator">The string to use as a separator. separator is included in the <see cref="StringBuilder"/> only if value has more than one element.</param>
        /// <param name="parts">An array that contains the elements to concatenate.</param>
        /// <exception cref="ArgumentOutOfRangeException">Enlarging the value of this instance would exceed <see cref="StringBuilder.MaxCapacity"/>.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder AppendJoin(this StringBuilder self, string separator, object[] parts)
        {
            if (parts.Length > 0) {
                self.Append(parts[0]);
                for (int i = 1; i < parts.Length; i++) {
                    self.Append(separator).Append(parts[i]);
                }
            }
            return self;
        }

        /// <summary>
        /// Appends all the elements of a string array, using the specified separator between each element.
        /// </summary>
        /// <param name="self">The <see cref="StringBuilder"/>.</param>
        /// <param name="separator">The string to use as a separator. separator is included in the <see cref="StringBuilder"/> only if value has more than one element.</param>
        /// <param name="parts">An array that contains the elements to concatenate.</param>
        /// <exception cref="ArgumentOutOfRangeException">Enlarging the value of this instance would exceed <see cref="StringBuilder.MaxCapacity"/>.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder AppendJoin(this StringBuilder self, string separator, string[] parts)
        {
            if (parts != null && parts.Length > 0) {
                self.Append(parts[0]);
                for (int i = 1; i < parts.Length; i++) {
                    self.Append(separator).Append(parts[i]);
                }
            }
            return self;
        }

        /// <summary>
        /// Appends all the elements of a string array, using the specified separator between each element.
        /// </summary>
        /// <typeparam name="T">The type of element to convert before inserting them.</typeparam>
        /// <param name="self">The <see cref="StringBuilder"/>.</param>
        /// <param name="separator">The string to use as a separator. separator is included in the <see cref="StringBuilder"/> only if value has more than one element.</param>
        /// <param name="parts">An array that contains the elements to concatenate.</param>
        /// <param name="converter">The converter from the type <typeparamref name="T"/> to string.</param>
        /// <exception cref="ArgumentOutOfRangeException">Enlarging the value of this instance would exceed <see cref="StringBuilder.MaxCapacity"/>.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder AppendJoin<T>(this StringBuilder self, string separator, T[] parts, Converter<T, string> converter)
        {
            if (parts != null && parts.Length > 0) {
                if (converter == null) {
                    self.Append(parts[0]);
                    for (int i = 1; i < parts.Length; i++) {
                        self.Append(separator).Append(parts[i]);
                    }
                } else {
                    self.Append(converter(parts[0]));
                    for (int i = 1; i < parts.Length; i++) {
                        self.Append(separator).Append(converter(parts[i]));
                    }
                }
            }
            return self;
        }

        #endregion

        #region Insert

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Insert(this StringBuilder self, int index, string value, int startIndex, int count)
        {
            return self.Insert(index, value.Substring(startIndex, count));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Insert(this StringBuilder self, int index, Strip value)
        {
            return value.InsertInto(self, index);
        }

        #endregion

        #region Prepend 

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, bool value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, byte value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, char value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, char[] value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, decimal value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, double value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, float value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, int value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, long value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, object value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, sbyte value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, short value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, uint value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, ulong value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, ushort value)
        {
            return self.Insert(0, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, char[] value, int startIndex, int charCount)
        {
            return self.Insert(0, value, startIndex, charCount);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, string value, int count)
        {
            return self.Insert(0, value, count);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, string value, int startIndex, int count)
        {
            return Insert(self, 0, value, startIndex, count);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Prepend(this StringBuilder self, Strip value)
        {
            return value.PrependTo(self);
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes the specified substrings from the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="self">The string.</param>
        /// <param name="args">The substrings to remove.</param>
        /// <returns>The string without the specified substrings.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "NullReferenceException is acceptable for non Linq-like extensions.", MessageId = "0")]
        public static StringBuilder Remove(this StringBuilder self, params string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            foreach (var arg in args) {
                self.Replace(arg, "");
            }
            return self;
        }

        #endregion

        #region SurroundWith

        /// <summary>
        /// Concatenates the prefix, the string and the suffix.
        /// </summary>
        /// <param name="self">The string builder</param>
        /// <param name="prefix">The prefix</param>
        /// <param name="suffix">The suffix</param>
        /// <returns>null if the string is null; otherwise, the concatenated string.</returns>
        public static StringBuilder SurroundWith(this StringBuilder self, string prefix = null, string suffix = null)
        {
            if (self == null)
                return null;
            self.Prepend(prefix);
            self.Append(suffix);
            return self;
        }

        #endregion
    }
}
