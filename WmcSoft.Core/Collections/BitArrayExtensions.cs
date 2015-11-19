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
using WmcSoft.Collections.Generic;

namespace WmcSoft.Collections
{
    public static class BitArrayExtensions
    {
        public static IEnumerable<TSource> Mask<TSource>(this BitArray mask, IEnumerable<TSource> x, IEnumerable<TSource> y) {
            using (var enumerator1 = x.GetEnumerator())
            using (var enumerator2 = y.GetEnumerator()) {
                for (int i = 0; i < mask.Count; i++) {
                    if (!enumerator1.MoveNext())
                        throw new ArgumentException("x");
                    if (!enumerator2.MoveNext())
                        throw new ArgumentException("y");
                    if (mask.Get(i))
                        yield return enumerator2.Current;
                    else
                        yield return enumerator1.Current;
                }
            }
        }

        public static TSource[] Mask<TSource>(this BitArray mask, IReadOnlyList<TSource> x, IReadOnlyList<TSource> y) {
            if (x == null) throw new ArgumentNullException("x");
            if (mask.Count != x.Count) throw new ArgumentException("x");
            if (y == null) throw new ArgumentNullException("y");
            if (mask.Count != y.Count) throw new ArgumentException("y");

            var result = new TSource[mask.Count];
            for (int i = 0; i < mask.Count; i++) {
                result[i] = mask.Get(i) ? y[i] : x[i];
            }
            return result;
        }

        public static TSource[] Mask<TSource>(this BitArray mask, IList<TSource> x, IList<TSource> y) {
            return Mask(mask, x.AsReadOnly(), y.AsReadOnly());
        }

        public static string Mask(this BitArray mask, string x, string y) {
            return new String(Mask(mask, x.AsReadOnlyList(), y.AsReadOnlyList()));
        }

        public static bool All(this BitArray bits) {
            foreach (bool b in bits) {
                if (!b)
                    return false;
            }
            return true;
        }

        public static bool Any(this BitArray bits) {
            foreach (bool b in bits) {
                if (b)
                    return true;
            }
            return false;
        }

        public static bool None(this BitArray bits) {
            foreach (bool b in bits) {
                if (b)
                    return false;
            }
            return true;
        }

        public static BitArray Concat(this BitArray x, BitArray y) {
            var z = new BitArray(x.Length + y.Length);
            for (var i=0; i < x.Length; i++) {
                z.Set(i, x.Get(i));
            }
            var j = x.Length;
            for (int i = 0; i < y.Length; i++) {
                z.Set(j + i, x.Get(i));
            }
            return z;
        }

        /// <summary>
        /// Resizes the <see cref="BitArray"/>, padding it with the <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="bits">The <see cref="BitArray"/></param>
        /// <param name="length">The new length.</param>
        /// <param name="defaultValue">The value to use for padding.</param>
        /// <returns></returns>
        public static BitArray Resize(this BitArray bits, int length, bool defaultValue = false) {
            if (defaultValue && bits.Length < length) {
                int first = bits.Length;
                bits.Length = length;
                for (; first < length; first++)
                    bits.Set(first, true);
            } else {
                bits.Length = length;
            }
            return bits;
        }
    }
}
