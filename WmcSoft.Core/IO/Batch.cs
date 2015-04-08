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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.IO
{
    /// <summary>
    /// Registers files to be included in a batch but differs the actual processing to the commit phase.
    /// </summary>
    public abstract class Batch : IDisposable
    {
        IList<KeyValuePair<string, IStreamSource>> entries;

        /// <summary>
        /// Adds the specified data source.
        /// </summary>
        /// <param name="soure">The data source.</param>
        /// <param name="name">Name of the entry.</param>
        public void Add(string name, IStreamSource source) {
            if (entries == null) {
                entries = new List<KeyValuePair<string, IStreamSource>>();
            }
            entries.Add(new KeyValuePair<string, IStreamSource>(name, source));
        }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        public void Commit() {
            if (entries != null && entries.Count > 0) {
                using (CreateCommitScope()) {
                    foreach (var entry in entries) {
                        Process(entry.Key, entry.Value);
                    }
                }
            }
            entries = null;
        }

        /// <summary>
        /// Creates the commit scope.
        /// </summary>
        /// <returns>An <see cref="IDisposable"/> instance to release resources once the commit is complete.</returns>
        protected virtual IDisposable CreateCommitScope() {
            return Disposer.Empty;
        }

        /// <summary>
        /// Override this method to actually process the specified entry name.
        /// </summary>
        /// <param name="name">Name of the entry.</param>
        /// <param name="source">The data source.</param>
        protected abstract void Process(string name, IStreamSource dataSource);

        /// <summary>
        /// Releases the unmanaged resources, and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing) {
        }

        /// <summary>
        /// Releases both unmanaged and managed resources.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the resources held by the current instance.
        /// </summary>
        ~Batch() {
            Dispose(false);
        }
    }

}
