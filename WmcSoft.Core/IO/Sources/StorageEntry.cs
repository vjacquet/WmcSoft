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
    /// Represents an entry in the stream store, for inventory or history.
    /// </summary>
    public abstract class StorageEntry : ITimestampStreamSource
    {
        protected StorageEntry(string name, int length, byte[] hash, DateTime since, DateTime? until = null)
        {
            Name = name;
            Length = length;
            Hash = hash;
            ValidSinceUtc = since;
            ValidUntilUtc = until;
        }

        /// <summary>
        /// The name of the stream in the store.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The length of the stream in the store.
        /// </summary>
        public int Length { get; }
        /// <summary>
        /// The hash of the stream.
        /// </summary>
        public byte[] Hash { get; }
        /// <summary>
        /// The date when storage entry has been created.
        /// </summary>
        public DateTime ValidSinceUtc { get; }
        /// <summary>
        /// The date when the storage entry was replaced by a newer one.
        /// </summary>
        public DateTime? ValidUntilUtc { get; private set; }

        /// <summary>
        /// Gets the stream from the source.
        /// </summary>
        /// <returns>A stream.</returns>
        /// <remarks>The caller owns the stream and, therefore, should dispose it.</remarks>
        public abstract Stream GetStream();

        /// <summary>
        /// Sets the ValidUntilUtc date.
        /// </summary>
        /// <param name="asOf">The ValidUntilUtc date.</param>
        protected virtual void CloseEntry(DateTime asOf)
        {
            if (asOf.Kind != DateTimeKind.Utc) throw new ArgumentException(nameof(asOf));
            if (asOf < ValidSinceUtc) throw new ArgumentOutOfRangeException(nameof(asOf));

            ValidUntilUtc = asOf;
        }

        bool ITimestampStreamSource.SupportTimestamp => true;
        DateTime? ITimestampStreamSource.Timestamp => ValidSinceUtc;
    }
}
