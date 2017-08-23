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

 ****************************************************************************
 * Adapted Eric Evans, Domain-Driven Design, Page 224
 ****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace WmcSoft
{
    /// <summary>
    /// Defines the extension methods about <see cref="ISpecification{T}"/>.
    /// This is a static class.
    /// </summary>
    public static class SpecificationExtensions
    {
        #region Selection

        public static IEnumerable<T> Satisfying<T, TSpecification>(this IEnumerable<T> source, TSpecification specification)
            where TSpecification : ISpecification<T> {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            return source.Where(specification.IsSatisfiedBy);
        }

        #endregion
    }
}
