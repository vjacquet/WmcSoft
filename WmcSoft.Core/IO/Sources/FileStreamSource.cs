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
using System.IO;

namespace WmcSoft.IO.Sources
{
    /// <summary>
    /// Returns a <see cref="Stream"/> from the specified <see cref="Path"/>.
    /// </summary>
    /// <remarks>The base implementaion returns a read-only stream.</remarks>
    public class FileStreamSource : ITimestampStreamSource
    {
        public FileStreamSource(string path)
        {
            // let FileInfo check the argument
            var info = new FileInfo(path);
            Path = info.FullName;
        }

        /// <summary>
        /// Returns <c>true</c> only when the <see cref="Timestamp"/> property may return a non null value.
        /// </summary>
        public bool SupportTimestamp => true;

        /// <summary>
        /// The UTC datetime at which the stream source was last written.
        /// </summary>
        /// <remarks>Returns <c>null</c> if the stream is not available.</remarks>
        public DateTime? Timestamp {
            get {
                if (File.Exists(Path))
                    return File.GetLastWriteTimeUtc(Path);
                return null;
            }
        }

        /// <summary>
        /// The full path to the file.
        /// </summary>
        public string Path { get; }

        public virtual Stream OpenSource()
        {
            return File.OpenRead(Path);
        }
    }
}
