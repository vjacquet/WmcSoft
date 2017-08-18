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

namespace WmcSoft
{
    public class AnonymousTransformation<TSource, TTarget> : ITransformation<TSource, TTarget>
    {
        private readonly Func<TSource, TTarget> _converter;

        public AnonymousTransformation(Func<TSource, TTarget> converter)
        {
            _converter = converter;
        }

        /// <summary>
        /// Applies the transformation.
        /// </summary>
        /// <param name="source">The source instance.</param>
        /// <returns>The target instance.</returns>
        public TTarget Apply(TSource source)
        {
            return _converter(source);
        }
    }
}