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
        public static int CombineHashCodes(int h1, int h2) {
            return (((h1 << 5) + h1) ^ h2);
        }
        public static int CombineHashCodes(int h1, int h2, int h3) {
            return CombineHashCodes(CombineHashCodes(h1, h2), h3);
        }
        public static int CombineHashCodes(int h1, int h2, int h3, int h4) {
            return CombineHashCodes(CombineHashCodes(h1, h2), CombineHashCodes(h3, h4));
        }

        public static int CombineHashCodes(this IEqualityComparer comparer, object obj1, object obj2) {
            return CombineHashCodes(comparer.GetHashCode(obj1), comparer.GetHashCode(obj2));
        }
        public static int CombineHashCodes(this IEqualityComparer comparer, object obj1, object obj2, object obj3) {
            return CombineHashCodes(comparer.GetHashCode(obj1), comparer.GetHashCode(obj2), comparer.GetHashCode(obj3));
        }
        public static int CombineHashCodes(this IEqualityComparer comparer, object obj1, object obj2, object obj3, object obj4) {
            return CombineHashCodes(comparer.GetHashCode(obj1), comparer.GetHashCode(obj2), comparer.GetHashCode(obj3), comparer.GetHashCode(obj4));
        }

        public static int CombineHashCodes<T>(this IEqualityComparer<T> comparer, T obj1, T obj2) {
            return CombineHashCodes(comparer.GetHashCode(obj1), comparer.GetHashCode(obj2));
        }
        public static int CombineHashCodes<T>(this IEqualityComparer<T> comparer, T obj1, T obj2, T obj3) {
            return CombineHashCodes(comparer.GetHashCode(obj1), comparer.GetHashCode(obj2), comparer.GetHashCode(obj3));
        }
        public static int CombineHashCodes<T>(this IEqualityComparer<T> comparer, T obj1, T obj2, T obj3, T obj4) {
            return CombineHashCodes(comparer.GetHashCode(obj1), comparer.GetHashCode(obj2), comparer.GetHashCode(obj3), comparer.GetHashCode(obj4));
        }
    }
}
