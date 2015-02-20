using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Text
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder Prepend(this StringBuilder sb, bool value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, byte value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, char value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, char[] value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, decimal value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, double value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, float value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, int value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, long value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, object value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, sbyte value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, short value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, uint value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, ulong value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, ushort value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, char[] value, int startIndex, int charCount) {
            return sb.Insert(0, value, startIndex, charCount);
        }

        public static StringBuilder Prepend(this StringBuilder sb, string value, int count) {
            return sb.Insert(0, value, count);
        }
    }
}
