using System;
using System.Linq;
using System.Text;

namespace WmcSoft.Numerics
{
    static class NumericsUtilities
    {
        #region Tags

        public struct UninitializedTag { }
        public static readonly UninitializedTag Uninitialized = default;

        #endregion

        public static string FormatVector<T>(T[] v, string format, IFormatProvider formatProvider)
             where T : IFormattable
        {
            var length = v.Length;
            var values = Array.ConvertAll(v, x => x.ToString(format, formatProvider));
            var capacity = values.Sum(x => x.Length + 2);
            var sb = new StringBuilder(capacity);
            sb.Append('[');
            sb.Append(values[0]);
            for (int i = 1; i < length; i++) {
                sb.Append("  ");
                sb.Append(values[i]);
            }
            sb.Append(']');
            return sb.ToString();
        }
    }
}
