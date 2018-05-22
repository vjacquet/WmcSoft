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
using System.Diagnostics;

namespace WmcSoft
{
    /// <summary>
    /// Encapsulates an error from a gateway.
    /// </summary>
    [DebuggerDisplay("Code={Code,nq}")]
    [Serializable]
    public class OperationError
    {
        /// <summary>
        /// Creates an instance of <see cref="OperationError"/> without code or description.
        /// </summary>
        public OperationError()
        {
        }

        /// <summary>
        /// Creates an intance of <see cref="OperationError"/> with the given <paramref name="code"/> and <paramref name="description"/>.
        /// </summary>
        /// <param name="code">The code of the error.</param>
        /// <param name="description">The description of the error.</param>
        public OperationError(string code, string description = null)
        {
            Code = code;
            Description = description;
        }

        /// <summary>
        /// The code for this error.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The description for this error.
        /// </summary>
        public string Description { get; set; }
    }
}
