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
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace WmcSoft
{
    public static class StreamExtension
    {
        #region Constants

        const int DefaultBufferSize = 0x1000;

        #endregion

        #region Rewind method

        /// <summary>
        /// Rewind the stream.
        /// </summary>
        /// <param name="stream">The stream</param>
        public static void Rewind(this Stream stream) {
            stream.Seek(0, SeekOrigin.Begin);
        }

        #endregion

        #region End method

        public static void End(this Stream stream) {
            if (stream.CanSeek) {
                stream.Seek(0, SeekOrigin.End);
            } else {
                byte[] buffer = new byte[DefaultBufferSize];
                int bytesRead = 0;
                while ((bytesRead = stream.Read(buffer, 0, DefaultBufferSize)) > 0)
                    ;
            }
        }

        #endregion

        #region Copy methods

        public static void Copy(Stream source, Stream target) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");

            byte[] buffer = new byte[DefaultBufferSize];
            InternalCopy(source, target, buffer);
        }

        public static void CopyTo(this Stream source, Stream target) {
            if (target == null)
                throw new ArgumentNullException("target");

            byte[] buffer = new byte[DefaultBufferSize];
            InternalCopy(source, target, buffer);
        }

        public static void CopyTo(this Stream source, Stream target, out long length) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");

            const int bufSize = 0x1000;
            byte[] buffer = new byte[bufSize];
            InternalCopy(source, target, buffer, out length);
        }

        public static void CopyTo(this Stream source, Stream target, byte[] buffer) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (buffer.Length == 0)
                throw new InsufficientMemoryException(); // the buffer is too small

            InternalCopy(source, target, buffer);
        }

        public static void CopyTo(this Stream source, Stream target, object sender, ProgressChangedEventHandler handler) {
            if (target == null)
                throw new ArgumentNullException("target");

            byte[] buffer = new byte[DefaultBufferSize];
            if (source.CanSeek)
                InternalCopy(source, target, buffer, sender, handler, source.Length);
            else
                InternalCopy(source, target, buffer, sender, handler);
        }

        public static void CopyTo(this Stream source, Stream target, object sender, ProgressChangedEventHandler handler, long size) {
            if (target == null)
                throw new ArgumentNullException("target");

            byte[] buffer = new byte[DefaultBufferSize];
            if (size > 0)
                InternalCopy(source, target, buffer, sender, handler, size);
            else if (source.CanSeek)
                InternalCopy(source, target, buffer, sender, handler, source.Length);
            else
                InternalCopy(source, target, buffer, sender, handler);
        }

        public static void CopyTo(this Stream source, Stream target, byte[] buffer, object sender, ProgressChangedEventHandler handler) {
            if (target == null)
                throw new ArgumentNullException("target");
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (buffer.Length == 0)
                throw new InsufficientMemoryException(); // the buffer is too small

            if (source.CanSeek)
                InternalCopy(source, target, buffer, sender, handler, source.Length);
            else
                InternalCopy(source, target, buffer, sender, handler);
        }

        public static void CopyTo(this Stream source, Stream target, byte[] buffer, object sender, ProgressChangedEventHandler handler, long size) {
            if (target == null)
                throw new ArgumentNullException("target");
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (buffer.Length == 0)
                throw new InsufficientMemoryException(); // the buffer is too small

            if (size > 0)
                InternalCopy(source, target, buffer, sender, handler, size);
            else if (source.CanSeek)
                InternalCopy(source, target, buffer, sender, handler, source.Length);
            else
                InternalCopy(source, target, buffer, sender, handler);
        }

        private static void InternalCopy(Stream source, Stream target, byte[] buffer) {
            int bufferSize = buffer.Length;
            int bytesRead = 0;
            while ((bytesRead = source.Read(buffer, 0, bufferSize)) > 0)
                target.Write(buffer, 0, bytesRead);
        }

        private static void InternalCopy(Stream source, Stream target, byte[] buffer, out long length) {
            int bufferSize = buffer.Length;
            int bytesRead = 0;
            length = 0;
            while ((bytesRead = source.Read(buffer, 0, bufferSize)) > 0) {
                length += bytesRead;
                target.Write(buffer, 0, bytesRead);
            }
        }

        private static void InternalCopy(Stream source, Stream target, byte[] buffer, object sender, ProgressChangedEventHandler handler, long size) {
            int bufferSize = buffer.Length;
            int bytesRead = 0;
            long totalRead = 0;
            while ((bytesRead = source.Read(buffer, 0, bufferSize)) > 0) {
                target.Write(buffer, 0, bytesRead);
                totalRead += bytesRead;
                handler(sender, new ProgressChangedEventArgs(unchecked((int)((totalRead * 100) / size)), totalRead));
            }
            handler(sender, new ProgressChangedEventArgs(100, totalRead));
        }

        private static void InternalCopy(Stream source, Stream target, byte[] buffer, object sender, ProgressChangedEventHandler handler) {
            int bufferSize = buffer.Length;
            int bytesRead = 0;
            long totalRead = 0;
            while ((bytesRead = source.Read(buffer, 0, bufferSize)) > 0) {
                target.Write(buffer, 0, bytesRead);
                totalRead += bytesRead;
                handler(sender, new ProgressChangedEventArgs(0, totalRead));
            }
            handler(sender, new ProgressChangedEventArgs(100, totalRead));
        }

        #endregion

        #region Consume

        public static void ConsumeAll(this Stream source) {
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
                ;
        }

        #endregion
    }
}
