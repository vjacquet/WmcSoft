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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace WmcSoft.IO.Sources
{
    /// <summary>
    /// Implements a <see cref="IStreamSource"/> that returns the first non null stream from a sequence of <see cref="IStreamSource"/>.
    /// </summary>
    /// <remarks>This class implements the chain of responsability pattern.</remarks>
    public class CompositeStreamSource : IStreamSource
    {
        private readonly List<IStreamSource> sources;

        public CompositeStreamSource(params IStreamSource[] sources)
        {
            if (sources == null) throw new ArgumentNullException(nameof(sources));
            if (sources.Length == 0 || sources.Any(s => s == null)) throw new ArgumentException(nameof(sources));

            this.sources = new List<IStreamSource>(sources);
        }

        public Stream OpenSource()
        {
            foreach (var source in sources) {
                var stream = source.OpenSource();
                if (stream != null) {
                    Trace.TraceInformation("Selecting source {0}.", source);
                    return stream;
                }
            }
            return null;
        }
    }
}
