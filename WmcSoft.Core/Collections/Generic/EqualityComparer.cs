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

using System.Collections;
using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Defines static methods to combine hash codes.
    /// This is a static class. 
    /// </summary>
    public static class EqualityComparer
    {
        /// <summary>
        /// Combines the specified hash codes.
        /// </summary>
        /// <param name="h1">The first hash code.</param>
        /// <param name="h2">The second hash code.</param>
        /// <returns>A hash code.</returns>
        public static int CombineHashCodes(int h1, int h2)
        {
            return (((h1 << 5) + h1) ^ h2);
        }

        /// <summary>
        /// Combines the specified hash codes.
        /// </summary>
        /// <param name="h1">The first hash code.</param>
        /// <param name="h2">The second hash code.</param>
        /// <param name="h3">The third hash code.</param>
        /// <returns>A hash code.</returns>
        public static int CombineHashCodes(int h1, int h2, int h3)
        {
            return CombineHashCodes(CombineHashCodes(h1, h2), h3);
        }

        /// <summary>
        /// Combines the specified hash codes.
        /// </summary>
        /// <param name="h1">The first hash code.</param>
        /// <param name="h2">The second hash code.</param>
        /// <param name="h3">The third hash code.</param>
        /// <param name="h4">The fourth hash code.</param>
        /// <returns>A hash code.</returns>
        public static int CombineHashCodes(int h1, int h2, int h3, int h4)
        {
            return CombineHashCodes(CombineHashCodes(h1, h2), CombineHashCodes(h3, h4));
        }

        /// <summary>
        /// Serves as a hash function for the specified objects for hashing algorithms and data structures.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <returns>A hash code for the specified objects.</returns>
        public static int CombineHashCodes(this IEqualityComparer comparer, object obj1, object obj2)
        {
            return CombineHashCodes(comparer.GetHashCode(obj1), comparer.GetHashCode(obj2));
        }

        /// <summary>
        /// Serves as a hash function for the specified objects for hashing algorithms and data structures.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <param name="obj3">The third object.</param>
        /// <returns>A hash code for the specified objects.</returns>
        public static int CombineHashCodes(this IEqualityComparer comparer, object obj1, object obj2, object obj3)
        {
            return CombineHashCodes(comparer.GetHashCode(obj1), comparer.GetHashCode(obj2), comparer.GetHashCode(obj3));
        }

        /// <summary>
        /// Serves as a hash function for the specified objects for hashing algorithms and data structures.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <param name="obj3">The third object.</param>
        /// <param name="obj4">The fourth object.</param>
        /// <returns>A hash code for the specified objects.</returns>
        public static int CombineHashCodes(this IEqualityComparer comparer, object obj1, object obj2, object obj3, object obj4)
        {
            return CombineHashCodes(comparer.GetHashCode(obj1), comparer.GetHashCode(obj2), comparer.GetHashCode(obj3), comparer.GetHashCode(obj4));
        }

        /// <summary>
        /// Serves as a hash function for the specified objects for hashing algorithms and data structures.
        /// </summary>
        /// <typeparam name="T">The type of objects to get hash codes.</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <returns>A hash code for the specified objects.</returns>
        public static int CombineHashCodes<T>(this IEqualityComparer<T> comparer, T obj1, T obj2)
        {
            return CombineHashCodes(comparer.GetHashCode(obj1), comparer.GetHashCode(obj2));
        }

        /// <summary>
        /// Serves as a hash function for the specified objects for hashing algorithms and data structures.
        /// </summary>
        /// <typeparam name="T">The type of objects to get hash codes.</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <param name="obj3">The third object.</param>
        /// <returns>A hash code for the specified objects.</returns>
        public static int CombineHashCodes<T>(this IEqualityComparer<T> comparer, T obj1, T obj2, T obj3)
        {
            return CombineHashCodes(comparer.GetHashCode(obj1), comparer.GetHashCode(obj2), comparer.GetHashCode(obj3));
        }

        /// <summary>
        /// Serves as a hash function for the specified objects for hashing algorithms and data structures.
        /// </summary>
        /// <typeparam name="T">The type of objects to get hash codes.</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <param name="obj3">The third object.</param>
        /// <param name="obj4">The fourth object.</param>
        /// <returns>A hash code for the specified objects.</returns>
        public static int CombineHashCodes<T>(this IEqualityComparer<T> comparer, T obj1, T obj2, T obj3, T obj4)
        {
            return CombineHashCodes(comparer.GetHashCode(obj1), comparer.GetHashCode(obj2), comparer.GetHashCode(obj3), comparer.GetHashCode(obj4));
        }

        /// <summary>
        /// Serves as a hash function for the specified objects for hashing algorithms and data structures.
        /// </summary>
        /// <typeparam name="T">The type of objects to get hash codes.</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="objects">The objects for which to get a hash code.</param>
        /// <returns>A hash code for the specified objects.</returns>
        public static int CombineHashCodes<T>(this IEqualityComparer<T> comparer, IEnumerable<T> objects)
        {
            if (objects == null)
                return 0;
            using (var enumerator = objects.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    return 0;
                var h = comparer.GetHashCode(enumerator.Current);
                while (enumerator.MoveNext()) {
                    h = CombineHashCodes(h, comparer.GetHashCode(enumerator.Current));
                }
                return h;
            }
        }
    }
}
