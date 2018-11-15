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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WmcSoft.IO.Sources
{
    /// <summary>
    /// Returns the most recent source from all sources.
    /// </summary>
    public class MostRecentStreamSource : ITimestampStreamSource, IEnumerable<ITimestampStreamSource>
    {
        private static readonly ITimestampStreamSource[] Empty = new ITimestampStreamSource[0];

        private readonly List<ITimestampStreamSource> sources;
        private bool supportTimestamp;

        public MostRecentStreamSource(params ITimestampStreamSource[] sources)
        {
            if (sources == null) {
                this.sources = new List<ITimestampStreamSource>();
            } else if (sources.Any(s => s == null)) {
                throw new ArgumentException(nameof(sources));
            } else {
                this.sources = new List<ITimestampStreamSource>(sources);
            }
            supportTimestamp = sources.Any(s => s.SupportTimestamp);
        }

        public void Add(ITimestampStreamSource source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            supportTimestamp |= source.SupportTimestamp;
            sources.Add(source);
        }

        private int IndexOfMostRecent()
        {
            var length = sources.Count;
            for (int i = 0; i < length; i++) {
                var max = sources[i].Timestamp;
                if (max.HasValue) {
                    int index = i;
                    while (++i < length) {
                        var Timestamp = sources[i].Timestamp;
                        if (Timestamp.GetValueOrDefault() > max.GetValueOrDefault()) {
                            max = Timestamp;
                            index = i;
                        }
                    }
                    return index;
                }
            }
            return -1;
        }

        public bool SupportTimestamp => supportTimestamp;

        public DateTime? Timestamp {
            get {
                var index = IndexOfMostRecent();
                if (index >= 0)
                    return sources[index].Timestamp;
                return null;

            }
        }

        public Stream OpenSource()
        {
            var index = IndexOfMostRecent();
            if (index >= 0)
                return sources[index].OpenSource();
            return null;
        }

        public override string ToString()
        {
            var result = base.ToString();
            var index = IndexOfMostRecent();
            if (index >= 0)
                return result + "->" + sources[index];
            return result;
        }

        #region IEnumerable<ITimestampStreamSource> members

        public IEnumerator<ITimestampStreamSource> GetEnumerator()
        {
            return ((IEnumerable<ITimestampStreamSource>)sources).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
