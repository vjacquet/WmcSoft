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

namespace WmcSoft.IO.Sources
{
    /// <summary>
    /// Registers stream sources to be included in a batch but defers the actual processing to the commit phase.
    /// </summary>
    public abstract class Batch<TScope> : IBatch
        where TScope : IDisposable
    {
        private readonly List<(string, IStreamSource)> _entries = new List<(string, IStreamSource)>();

        /// <summary>
        /// Adds the specified data source.
        /// </summary>
        /// <param name="soure">The data source.</param>
        /// <param name="name">Name of the entry.</param>
        public void Add(string name, IStreamSource source)
        {
            _entries.Add((name, source));
        }

        /// <summary>
        /// Commits this batch.
        /// </summary>
        public void Commit()
        {
            if (_entries.Count > 0) {
                using (var scope = CreateCommitScope()) {
                    foreach (var entry in _entries) {
                        Process(scope, entry.Item1, entry.Item2);
                    }
                }
                _entries.Clear();
            }
        }

        /// <summary>
        /// Creates the commit scope.
        /// </summary>
        /// <returns>An <see cref="IDisposable"/> instance to release resources once the commit is complete.</returns>
        protected abstract TScope CreateCommitScope();

        /// <summary>
        /// Override this method to actually process the specified entry name.
        /// </summary>
        /// <param name="scope">The scope</param>
        /// <param name="name">Name of the entry.</param>
        /// <param name="source">The data source.</param>
        protected abstract void Process(TScope scope, string name, IStreamSource source);

        /// <summary>
        /// Releases the unmanaged resources, and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Releases both unmanaged and managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the resources held by the current instance.
        /// </summary>
        ~Batch()
        {
            Dispose(false);
        }
    }
}
