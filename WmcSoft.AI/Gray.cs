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
using static WmcSoft.BitArithmetics;

namespace WmcSoft.AI
{
    /// <summary>
    /// Gray representation of byte, which differs only by one bit when incrementing.
    /// </summary>
    public struct Gray8
    {
        byte _value;

        public Gray8(byte n) {
            _value = ToGray(n);
        }

        public static implicit operator Gray8(int n) {
            return new Gray8(checked((byte)n));
        }

        //public static explicit operator double(Gray8 g) {
        //    var n = Array.IndexOf(GrayCodeTable, g._value);
        //    return (n - 127.5d) * 0.0392d;
        //}

        public Gray8 Increment() {
            var x = FromGray(_value);
            return new Gray8(ToGray(++x));
        }

        public Gray8 Decrement() {
            var x = FromGray(_value);
            return new Gray8(ToGray(--x));
        }

        public static Gray8 operator ++(Gray8 x) {
            return x.Increment();
        }

        public static Gray8 operator --(Gray8 x) {
            return x.Decrement();
        }

        public override string ToString() {
            return Convert.ToString(_value, 2).PadLeft(8, '0');
        }

        static byte ToGray(byte x) {
            uint i = x;
            i ^= i >> 1;
            return checked((byte)i);
        }

        static byte FromGray(byte x) {
            uint i = x;
            i ^= i >> 4;
            i ^= i >> 2;
            i ^= i >> 1;
            return checked((byte)i);
        }
    }

    /// <summary>
    /// Gray representation of short, which differs only by one bit when incrementing.
    /// </summary>
    public struct Gray16
    {
        ushort _value;

        public Gray16(ushort n) {
            _value = ToGray(n);
        }

        public static implicit operator Gray16(int n) {
            return new Gray16(checked((ushort)n));
        }

        public Gray16 Increment() {
            var x = FromGray(_value);
            return new Gray16(ToGray(++x));
        }

        public Gray16 Decrement() {
            var x = FromGray(_value);
            return new Gray16(ToGray(--x));
        }

        public static Gray16 operator ++(Gray16 x) {
            return x.Increment();
        }

        public static Gray16 operator --(Gray16 x) {
            return x.Decrement();
        }

        public override string ToString() {
            return Convert.ToString(_value, 2).PadLeft(16, '0');
        }

        static ushort ToGray(ushort x) {
            uint i = x;
            i ^= i >> 1;
            return checked((ushort)i);
        }

        static ushort FromGray(ushort x) {
            uint i = x;
            i ^= i >> 8;
            i ^= i >> 4;
            i ^= i >> 2;
            i ^= i >> 1;
            return checked((ushort)i);
        }
    }

    /// <summary>
    /// Gray representation of integer, which differs only by one bit when incrementing.
    /// </summary>
    public struct Gray32
    {
        uint _value;

        public Gray32(uint n) {
            _value = ToGray(n);
        }

        public static implicit operator Gray32(uint n) {
            return new Gray32(n);
        }

        public Gray32 Increment() {
            var x = FromGray(_value);
            return new Gray32(ToGray(++x));
        }

        public Gray32 Decrement() {
            var x = FromGray(_value);
            return new Gray32(ToGray(--x));
        }

        public static Gray32 operator ++(Gray32 x) {
            return x.Increment();
        }

        public static Gray32 operator --(Gray32 x) {
            return x.Decrement();
        }

        public override string ToString() {
            return Convert.ToString(_value, 2).PadLeft(32, '0');
        }
    }
}
