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
using System.Runtime.Serialization;
using System.Xml;

namespace WmcSoft
{
    /// <summary>
    /// Represents a family of symbols.
    /// </summary>
    /// <typeparam name="T">The type of the family, used as a marker.</typeparam>
    [Serializable]
    public struct Symbol<T> : IEquatable<Symbol<T>>, ISerializable
    {
        static readonly NameTable nt = new NameTable();

        readonly string value;

        public Symbol(string symbol)
        {
            value = nt.Add(symbol);
        }

        public static implicit operator Symbol<T>(string symbol)
        {
            return new Symbol<T>(symbol);
        }

        public static explicit operator string(Symbol<T> symbol)
        {
            return symbol.value;
        }

        public override int GetHashCode()
        {
            if (value == null)
                return 0;
            return value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Symbol<T>))
                return false;
            return Equals((Symbol<T>)obj);
        }

        public bool Equals(Symbol<T> other)
        {
            return ReferenceEquals(value, other.value);
        }

        public static bool operator ==(Symbol<T> x, Symbol<T> y)
        {
            return x.Equals(y);
        }
        public static bool operator !=(Symbol<T> x, Symbol<T> y)
        {
            return !x.Equals(y);
        }

        public override string ToString()
        {
            return value ?? "";
        }

        private Symbol(SerializationInfo info, StreamingContext context)
            : this((string)info.GetValue("symbol", typeof(string)))
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("symbol", value, typeof(string));
        }
    }
}
