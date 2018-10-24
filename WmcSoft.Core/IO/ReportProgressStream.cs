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

        private readonly IProgress<int> progress;
        private readonly long length;
        private long read;

        #endregion

        #region Lifecycle

        static Stream GuardAgainstNull(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            return stream;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportProgressStream"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="progress">The progression, in percentage.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> or <paramref name="progress"/> are <c>null</c>.</exception>
        /// <exception cref="NotSupportedException">The stream does not support reading or seeking.</exception>
        public ReportProgressStream(Stream stream, IProgress<int> progress)
            : this(stream, GuardAgainstNull(stream).Length, progress)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportProgressStream"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="length">The length of the stream.</param>
        /// <param name="progress">The progression, in percentage.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> or <paramref name="progress"/> are <c>null</c>.</exception>
        /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
        public ReportProgressStream(Stream stream, long length, IProgress<int> progress)
            : base(stream)
        {
            if (!stream.CanRead) throw new NotSupportedException();
            if (progress == null) throw new ArgumentNullException(nameof(progress));

            this.progress = progress;
            this.length = length;
        }

        #endregion

        #region Overrides

        public override int Read(byte[] buffer, int offset, int count)
        {
            var read = base.Read(buffer, offset, count);

            Report(read);

            return read;
        }

        private void Report(int n)
        {
            read += n;
            progress.Report((int)((100 * read) / length));
        }

        #endregion
    }
}
