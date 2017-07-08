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
using System.IO;

namespace WmcSoft.IO
{
    /// <summary>
    /// A stream decorator to report reading progress on a <see cref="IProgress{int}"/>.
    /// </summary>
    public class ReportProgressStream : StreamDecorator
    {
        #region Fields

        readonly IProgress<int> _progress;
        readonly long _length;
        long _read;

        #endregion

        #region Lifecycle

        public ReportProgressStream(Stream stream, IProgress<int> progress)
            : this(stream, stream.Length, progress)
        {
        }

        public ReportProgressStream(Stream stream, long length, IProgress<int> progress)
            : base(stream)
        {
            _progress = progress;
            _length = length;
        }

        #endregion

        #region Overrides

        public override int Read(byte[] buffer, int offset, int count)
        {
            var read = base.Read(buffer, offset, count);

            _read += read;
            _progress.Report((int)((100 * _read) / _length));

            return read;
        }

        #endregion
    }
}
