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
    /// Implements a <see cref="BulkAdapter{T}"/> calling a function to get the values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class FuncBulkAdapter<T> : BulkAdapter<T>
    {
        private readonly int _fieldCount;
        private readonly Func<T, int, object> _interpreter;

        /// <summary>
        /// Creates a new instance of the <see cref="FuncBulkAdapter{T}"/>.
        /// </summary>
        /// <param name="fieldCount">The field count.</param>
        /// <param name="interpreter">The interpreter function.</param>
        /// <param name="values">The values to adapt as a <see cref="System.Data.IDataReader"/>.</param>
        public FuncBulkAdapter(int fieldCount, Func<T, int, object> interpreter, IEnumerable<T> values)
            : base(values)
        {
            _fieldCount = fieldCount;
            _interpreter = interpreter;
        }

        /// <inheritdoc/>
        public override object GetValue(int i) => _interpreter(Current, i);

        /// <inheritdoc/>
        public override int FieldCount => _fieldCount;
    }
}
