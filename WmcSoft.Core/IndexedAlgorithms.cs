using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    public static class IndexedAlgorithms
    {
        #region CopyTo methods

        public static void CopyTo<T>(this IList<T> source, T[] array, int arrayIndex, int length) {
            if (source == null) throw new ArgumentNullException("source");

            if (source.GetType() == typeof(T[])) {
                Array.Copy((T[])source, 0, array, arrayIndex, length);
            } else {
                if (array == null) throw new ArgumentNullException("array");
                if (length < 0) throw new ArgumentOutOfRangeException("length");
                if (array.Length < (arrayIndex + length)) throw new ArgumentException("array");
                if (source.Count < length) throw new ArgumentException("source");

                for (int i = 0; i < length; i++) {
                    array[arrayIndex++] = source[i];
                }
            }
        }

        public static void CopyTo<T>(this IList<T> source, int sourceIndex, T[] array, int arrayIndex, int length) {
            if (source == null) throw new ArgumentNullException("source");

            if (source.GetType() == typeof(T[])) {
                Array.Copy((T[])source, sourceIndex, array, arrayIndex, length);
            } else {
                if (array == null) throw new ArgumentNullException("array");
                if (length < 0) throw new ArgumentOutOfRangeException("length");
                if (array.Length < (arrayIndex + length)) throw new ArgumentException("array");
                if (source.Count < length) throw new ArgumentException("source");
                length += sourceIndex;

                for (int i = sourceIndex; i < length; i++) {
                    array[arrayIndex++] = source[i];
                }
            }
        }

        #endregion

        #region CopyBackwardsTo methods

        public static void CopyBackwardsTo<T>(this IList<T> source, int sourceIndex, T[] array, int arrayIndex, int length) {
            if (array == null) throw new ArgumentNullException("array");
            if (length < 0) throw new ArgumentOutOfRangeException("length");
            if (array.Length < (arrayIndex + length)) throw new ArgumentException("array");
            if (source.Count < (sourceIndex + length)) throw new ArgumentException("source");

            for (int i = sourceIndex + length - 1; i >= sourceIndex; i--) {
                array[arrayIndex++] = source[i];
            }
        }

        public static void CopyBackwardsTo<T>(this IList<T> source, T[] array, int arrayIndex, int length) {
            if (array == null) throw new ArgumentNullException("array");
            if (length < 0) throw new ArgumentOutOfRangeException("length");
            if (array.Length < (arrayIndex + length)) throw new ArgumentException("array");
            if (source.Count < length) throw new ArgumentException("source");

            for (int i = length - 1; i >= 0; i--) {
                array[arrayIndex++] = source[i];
            }
        }

        public static void CopyBackwardsTo<T>(this IList<T> source, T[] array, int arrayIndex) {
            if (source == null) throw new ArgumentNullException("source");
            if (array == null) throw new ArgumentNullException("array");
            if (array.Length < arrayIndex) throw new ArgumentException("array");

            for (int i = source.Count - 1; i >= 0; i--) {
                array[arrayIndex++] = source[i];
            }
        }

        #endregion

        #region SwapItems methods

        /// <summary>
        /// Swaps two items.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="i">The item at the <paramref name="i"/> index.</param>
        /// <param name="j">The item at the <paramref name="j"/> index.</param>
        public static void SwapItems<T>(this IList<T> list, int i, int j) {
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        #endregion
    }
}
