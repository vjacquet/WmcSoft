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
using System.Collections;
using System.Collections.Generic;
using WmcSoft.Collections.Generic;

using static WmcSoft.BitArithmetics;

namespace WmcSoft.Collections
{
    public static class BitArrayExtensions
    {
        static IEnumerable<TSource> DoMask<TSource>(this BitArray mask, IEnumerable<TSource> x, IEnumerable<TSource> y)
        {
            using (var enumerator1 = x.GetEnumerator())
            using (var enumerator2 = y.GetEnumerator()) {
                for (int i = 0; i < mask.Count; i++) {
                    if (!enumerator1.MoveNext()) throw new ArgumentException(nameof(x));
                    if (!enumerator2.MoveNext()) throw new ArgumentException(nameof(y));

                    if (mask.Get(i))
                        yield return enumerator2.Current;
                    else
                        yield return enumerator1.Current;
                }
            }
        }

        static TSource[] DoMask<TSource>(this BitArray mask, IReadOnlyList<TSource> x, IReadOnlyList<TSource> y)
        {
            if (mask.Count != x.Count) throw new ArgumentException(nameof(x));
            if (mask.Count != y.Count) throw new ArgumentException(nameof(y));

            var result = new TSource[mask.Count];
            for (int i = 0; i < mask.Count; i++) {
                result[i] = mask.Get(i) ? y[i] : x[i];
            }
            return result;
        }

        static IReadOnlyList<TSource> ReadOnly<TSource>(IEnumerable<TSource> x)
        {
            var rx = x as IReadOnlyList<TSource>;
            if (rx != null) return rx;
            var wx = x as IList<TSource>;
            if (wx != null) return wx.AsReadOnly();
            return null;
        }

        /// <summary>
        /// Takes values from the enumerables depending on the value of the bit in the mask.
        /// </summary>
        /// <typeparam name="TSource">The type of elements</typeparam>
        /// <param name="mask">The mask</param>
        /// <param name="x">The source to get elements from when the bit is 0.</param>
        /// <param name="y">The source to get elements from when the bit is 1.</param>
        /// <returns>The items selected by the mask.</returns>
        public static IEnumerable<TSource> Mask<TSource>(this BitArray mask, IEnumerable<TSource> x, IEnumerable<TSource> y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));

            var rx = ReadOnly(x);
            var ry = ReadOnly(y);
            if (rx != null && ry != null)
                return DoMask(mask, rx, ry);
            return DoMask(mask, x, y);
        }

        /// <summary>
        /// Takes values from the lists depending on the value of the bit in the mask.
        /// </summary>
        /// <typeparam name="TSource">The type of elements</typeparam>
        /// <param name="mask">The mask</param>
        /// <param name="x">The source to get elements from when the bit is 0.</param>
        /// <param name="y">The source to get elements from when the bit is 1.</param>
        /// <returns>The items selected by the mask.</returns>
        public static TSource[] Mask<TSource>(this BitArray mask, IList<TSource> x, IList<TSource> y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));

            return DoMask(mask, x.AsReadOnly(), y.AsReadOnly());
        }

        /// <summary>
        /// Takes chars from the strings depending on the value of the bit in the mask.
        /// </summary>
        /// <typeparam name="TSource">The type of elements</typeparam>
        /// <param name="mask">The mask</param>
        /// <param name="x">The source to get elements from when the bit is 0.</param>
        /// <param name="y">The source to get elements from when the bit is 1.</param>
        /// <returns>The string whose chars were selected by the mask.</returns>
        public static string Mask(this BitArray mask, string x, string y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));

            return new string(DoMask(mask, x.AsReadOnlyList(), y.AsReadOnlyList()));
        }

        /// <summary>
        /// Determines whether all bits are true.
        /// </summary>
        /// <param name="bits">The bits</param>
        /// <returns>Returns true when all bits are true.</returns>
        public static bool All(this BitArray bits)
        {
            foreach (bool b in bits) {
                if (!b)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether any bit is true.
        /// </summary>
        /// <param name="bits">The bits</param>
        /// <returns>Returns true when at least one bit is true.</returns>
        public static bool Any(this BitArray bits)
        {
            var data = DataOf(bits);
            foreach (var i in data) {
                if (i != 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether all bits are false.
        /// </summary>
        /// <param name="bits">The bits</param>
        /// <returns>Returns true when all bits are false.</returns>
        public static bool None(this BitArray bits)
        {
            var data = DataOf(bits);
            foreach (var i in data) {
                if (i != 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Concatenate two bit array.
        /// </summary>
        /// <param name="x">The first bit array</param>
        /// <param name="y">The second bit array</param>
        /// <returns>The bit array produced by the concatenation</returns>
        public static BitArray Concat(this BitArray x, BitArray y)
        {
            var z = new BitArray(x.Length + y.Length);
            for (var i = 0; i < x.Length; i++) {
                z.Set(i, x.Get(i));
            }
            var j = x.Length;
            for (int i = 0; i < y.Length; i++) {
                z.Set(j + i, y.Get(i));
            }
            return z;
        }

        /// <summary>
        /// Copies bits into another bit array
        /// </summary>
        /// <param name="source">The source bit array</param>
        /// <param name="array">The destination bit array</param>
        /// <param name="index">The index where to start the copy in the destination array</param>
        public static void CopyTo(this BitArray source, BitArray array, int index)
        {
            var length = source.Length;
            for (int i = 0; i < length; i++, index++) {
                array.Set(index, source.Get(i));
            }
        }

        /// <summary>
        /// Copies bits into another bit array
        /// </summary>
        /// <param name="source">The source bit array</param>
        /// <param name="sourceIndex">The index where to start the copy in the source array</param>
        /// <param name="array">The destination bit array</param>
        /// <param name="destinationIndex">The index where to start the copy in the destination array</param>
        /// <param name="length">The number of bit to copy.</param>
        public static void CopyTo(this BitArray source, int sourceIndex, BitArray array, int destinationIndex, int length)
        {
            while (length-- != 0) {
                array.Set(destinationIndex++, source.Get(sourceIndex++));
            }
        }

        /// <summary>
        /// Resizes the <see cref="BitArray"/>, padding it with the <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="bits">The <see cref="BitArray"/></param>
        /// <param name="length">The new length.</param>
        /// <param name="defaultValue">The value to use for padding.</param>
        /// <returns></returns>
        public static BitArray Resize(this BitArray bits, int length, bool defaultValue = false)
        {
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

        internal static int[] DataOf(BitArray bits)
        {
            var ints = new int[(bits.Count >> 5) + 1];
            bits.CopyTo(ints, 0);
            // fix for not truncated bits in last integer that may have been set to true with SetAll()
            ints[ints.Length - 1] &= ~(-1 << (bits.Count % 32));
            return ints;
        }

        /// <summary>
        /// Computes the number of bits set to one.
        /// </summary>
        /// <param name="bits">The bits</param>
        /// <returns>The number of bits set to one.</returns>
        public static int Cardinality(this BitArray bits)
        {
            // see <http://stackoverflow.com/questions/5063178/counting-bits-set-in-a-net-bitarray-class/14354311#14354311>
            var data = DataOf(bits);

            var count = 0;
            for (int i = 0; i < data.Length; i++) {
                count += FastCountBits((uint)data[i]);
            }
            return count;
        }
    }
}