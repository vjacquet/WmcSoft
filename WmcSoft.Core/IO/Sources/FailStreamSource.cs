#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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
using System.IO;

namespace WmcSoft.IO.Sources
{
    /// <summary>
    /// Implements a <see cref="IStreamSource"/> that throws instead of returning a stream.
    /// </summary>
    /// <remarks>This source is usefull as a terminator of a chain of responsability.</remarks>
    public class FailStreamSource : ITimestampStreamSource
    {
        private readonly Func<Exception> _factory;

        public FailStreamSource(Func<Exception> factory = null)
        {
            _factory = factory ?? (() => null);
        }

        /// <inheritdoc/>
        public bool SupportTimestamp => false;

        /// <inheritdoc/>
        public DateTime? Timestamp => null;

        /// <inheritdoc/>
        public Stream OpenSource()
        {
            throw _factory() ?? new InvalidOperationException("Stream source not found.");
        }
    }
}
