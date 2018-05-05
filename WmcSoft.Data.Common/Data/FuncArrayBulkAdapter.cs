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

namespace WmcSoft.Data
{
    /// <summary>
    /// Implements a <see cref="BulkAdapter{T}"/> calling a function per value to get.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class FuncArrayBulkAdapter<T> : BulkAdapter<T>
    {
        private readonly Func<T, object>[] _interpreters;

        /// <summary>
        /// Creates a new instance of the <see cref="FuncBulkAdapter{T}"/>.
        /// </summary>
        /// <param name="values">The values to adapt as a <see cref="System.Data.IDataReader"/>.</param>
        /// <param name="interpreters">The interpreter functions.</param>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.-or-<paramref name="interpreters"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="interpreters"/> is empty.</exception>
        public FuncArrayBulkAdapter(IEnumerable<T> values, Func<T, object>[] interpreters)
            : base(values)
        {
            if (interpreters == null) throw new ArgumentNullException(nameof(interpreters));
            if (interpreters.Length < 1) throw new ArgumentException(nameof(interpreters));

            _interpreters = interpreters;
        }

        /// <inheritdoc/>
        public override object GetValue(int i) => _interpreters[i](Current);

        /// <inheritdoc/>
        public override int FieldCount => _interpreters.Length;
    }
}
