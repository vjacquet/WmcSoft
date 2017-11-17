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
using System.Collections.Generic;
using System.Linq;

namespace WmcSoft
{
    /// <summary>
    /// Defines the extension methods to the <see cref="Exception"/> class.
    /// This is a static class. 
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Composes the exception list.
        /// </summary>
        /// <param name="exceptions">The exception list.</param>
        /// <returns><c>null</c> if the list is empty; the item if the list contains a single element; otherwise, an <see cref="AggregateException"/>.</returns>
        public static Exception Compose(this ICollection<Exception> exceptions)
        {
            switch (exceptions.Count) {
            case 0: return null;
            case 1: return exceptions.First();
            default: return new AggregateException(exceptions);
            }
        }

        /// <summary>
        /// Composes the exceptions.
        /// </summary>
        /// <param name="exceptions">The exceptions.</param>
        /// <returns><c>null</c> if the enumeration is empty; the item if the enumeration contains a single element; otherwise, an <see cref="AggregateException"/>.</returns>
        public static Exception Compose(this IEnumerable<Exception> exceptions)
        {
            switch (exceptions) {
            case ICollection<Exception> collection:
                return Compose(collection);
            case IReadOnlyCollection<Exception> collection:
                switch (collection.Count) {
                case 0: return null;
                case 1: return collection.First();
                default: return new AggregateException(collection);
                }
            default:
                using (var enumerator = exceptions.GetEnumerator()) {
                    if (!enumerator.MoveNext())
                        return null;

                    var first = enumerator.Current;
                    if (!enumerator.MoveNext())
                        return first;

                    var list = new List<Exception>();
                    list.Add(first);
                    do {
                        list.Add(enumerator.Current);
                    } while (enumerator.MoveNext());
                    return new AggregateException(list);
                }
            }
        }
    }
}
