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

namespace WmcSoft.IO
{
    /// <summary>
    /// Represents the registration of stream sources to be included in a batch but defers the actual processing to the commit phase.
    /// </summary>
    public interface IBatch : IDisposable
    {
        /// <summary>
        /// Adds the specified data source.
        /// </summary>
        /// <param name="soure">The data source.</param>
        /// <param name="name">Name of the entry.</param>
        void Add(string name, IStreamSource source);

        /// <summary>
        /// Commits this batch.
        /// </summary>
        void Commit();
    }
}
