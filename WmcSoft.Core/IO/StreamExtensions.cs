﻿#region Licence

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
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace WmcSoft.IO
{
    public static class StreamExtensions
    {
        #region Constants

        const int DefaultBufferSize = 0x1000;

        #endregion

        #region Copy methods

        /// <summary>
        /// Copy a stream to another one, using the provided buffer.
        /// </summary>
        /// <param name="source">The source stream</param>
        /// <param name="destination">The target stream</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The number of bytes copied</returns>
        public static void CopyTo(this Stream source, Stream destination, byte[] buffer)
        {
            Debug.Assert(source != null);

            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            int bufferSize = buffer.Length;
            if (bufferSize == 0)
                throw new InsufficientMemoryException(); // the buffer is too small

            int bytesRead = 0;
            while ((bytesRead = source.Read(buffer, 0, bufferSize)) > 0) {
                destination.Write(buffer, 0, bytesRead);
            }
        }

        [HostProtection(ExternalThreading = true)]
        public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize, IProgress<long> progress, CancellationToken cancellationToken)
        {
            Debug.Assert(source != null);

            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (progress == null) throw new ArgumentNullException(nameof(progress));

            byte[] buffer = new byte[bufferSize];
            long length = 0;
            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0) {
                length += bytesRead;
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                progress.Report(length);
            }
        }

        [HostProtection(ExternalThreading = true)]
        public static Task CopyToAsync(this Stream source, Stream destination, int bufferSize, IProgress<long> progress)
        {
            return CopyToAsync(source, destination, bufferSize, progress, CancellationToken.None);
        }

        [HostProtection(ExternalThreading = true)]
        public static Task CopyToAsync(this Stream source, Stream destination, IProgress<long> progress)
        {
            return CopyToAsync(source, destination, DefaultBufferSize, progress, CancellationToken.None);
        }

        [HostProtection(ExternalThreading = true)]
        public static Task CopyToAsync(this Stream source, Stream destination, IProgress<long> progress, CancellationToken cancellationToken)
        {
            return CopyToAsync(source, destination, DefaultBufferSize, progress, cancellationToken);
        }

        #endregion

        #region Consume

        public static long ConsumeAll(this Stream stream)
        {
            Debug.Assert(stream != null);

            byte[] buffer = new byte[DefaultBufferSize];
            long length = 0;
            int bytesRead = 0;
            while ((bytesRead = stream.Read(buffer, 0, DefaultBufferSize)) > 0)
                length += bytesRead;
            return length;
        }

        public static async Task<long> ConsumeAllAsync(this Stream stream, CancellationToken cancellationToken)
        {
            Debug.Assert(stream != null);

            byte[] buffer = new byte[DefaultBufferSize];
            long length = 0;
            int bytesRead = 0;
            while ((bytesRead = await stream.ReadAsync(buffer, 0, DefaultBufferSize, cancellationToken).ConfigureAwait(false)) > 0)
                length += bytesRead;
            return length;
        }

        public static Task<long> ConsumeAllAsync(this Stream stream)
        {
            return ConsumeAllAsync(stream, CancellationToken.None);
        }

        #endregion

        #region End method

        /// <summary>
        /// Go to the end of the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <remarks>Uses <see cref="Stream.Seek"/> when <see cref="Stream.CanSeek"/> returns <c>true</c>; otherwise, consume the data until the end of stream.</remarks>
        public static void End(this Stream stream)
        {
            Debug.Assert(stream != null);

            if (stream.CanSeek) {
                stream.Seek(0L, SeekOrigin.End);
            } else {
                var buffer = new byte[DefaultBufferSize];
                var bytesRead = 0;
                while ((bytesRead = stream.Read(buffer, 0, DefaultBufferSize)) > 0)
                    ;
            }
        }

        #endregion

        #region Rewind method

        /// <summary>
        /// Rewind the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static void Rewind(this Stream stream)
        {
            Debug.Assert(stream != null);

            stream.Seek(0L, SeekOrigin.Begin);
        }

        #endregion

        #region Skip

        /// <summary>
        /// Skip <paramref name="count"/> bytes from the current position.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="count">The number of bytes to skip.</param>
        /// <remarks>Uses <see cref="Stream.Seek"/> when <see cref="Stream.CanSeek"/> returns <c>true</c>; otherwise, consume up to <paramref name="count"/> bytes of data.</remarks>
        public static void Skip(this Stream stream, long count)
        {
            Debug.Assert(stream != null);

            if (stream.CanSeek) {
                stream.Seek(count, SeekOrigin.Current);
            } else {
                var buffer = new byte[DefaultBufferSize];
                while (count > DefaultBufferSize) {
                    var bytesRead = stream.Read(buffer, 0, DefaultBufferSize);
                    if (bytesRead == 0)
                        return; // end of stream reached.
                    count -= bytesRead;
                }
                while (count > 0) {
                    var bytesRead = stream.Read(buffer, 0, unchecked((int)count));
                    if (bytesRead == 0)
                        return; // end of stream reached.
                    count -= bytesRead;
                }
            }
        }

        #endregion
    }
}
