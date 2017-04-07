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
    /// Implements a <see cref="Batch"/> to copy files to a target directory.
    /// </summary>
    public class FileCopyBatch : Batch<IDisposable>
    {
        private readonly string _path;

        private Func<IDisposable> _impersonator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCopyBatch"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public FileCopyBatch(string path) {
            if (path == null) throw new ArgumentNullException(nameof(path));

            _path = path;
        }

        /// <summary>
        /// Impersonates the user.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public void Impersonate(string domainName, string userName, string password) {
            _impersonator = () => new Impersonate(domainName, userName, password);
        }

        /// <summary>
        /// Creates the commit scope.
        /// </summary>
        /// <returns>
        /// An <see cref="IDisposable"/> instance to release resources once the commit is complete.
        /// </returns>
        protected override IDisposable CreateCommitScope() {
            if (_impersonator != null) {
                return _impersonator();
            }
            return Disposable.Empty;
        }

        /// <summary>
        /// Copies the file.
        /// </summary>
        /// <param name="name">Name of the entry.</param>
        /// <param name="source">The data source.</param>
        protected override void Process(IDisposable scope, string name, IStreamSource source) {
            var fileName = Path.Combine(_path, name);
            var directoryName = Path.GetDirectoryName(fileName);

            Directory.CreateDirectory(directoryName);

            using (var target = File.Create(fileName))
            using (var stream = source.GetStream()) {
                stream.CopyTo(target);
            }
        }

    }

}
