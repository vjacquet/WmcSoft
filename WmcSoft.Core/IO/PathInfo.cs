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
    public struct PathInfo
    {
        struct UninitializedTag { }
        static readonly UninitializedTag Uninitialized = default(UninitializedTag);


        #region Fields

        private readonly string _path;

        #endregion

        #region Lifecycle


        private PathInfo(string value, UninitializedTag tag) {
            _path = value;
        }

        public PathInfo(string value) {
            _path = value;
        }

        public PathInfo(string path1, string path2)
            : this(Path.Combine(path1, path2)) {
        }

        public PathInfo(string path1, string path2, string path3)
            : this(Path.Combine(path1, path2, path3)) {
        }

        public PathInfo(string path1, string path2, string path3, string path4)
            : this(Path.Combine(path1, path2, path3, path4)) {
        }

        public PathInfo(params string[] paths)
            : this(Path.Combine(paths)) {
        }

        #endregion

        #region Properties

        public PathInfo Root
        {
            get {
                var root = Path.GetPathRoot(_path);
                return new PathInfo(root, Uninitialized);
            }
        }

        public string FileName
        {
            get {
                return Path.GetFileName(_path);
            }
        }

        public string FileNameWithoutExtension
        {
            get {
                return Path.GetFileNameWithoutExtension(_path);
            }
        }

        public string Extension
        {
            get {
                return Path.GetExtension(_path);
            }
        }

        #endregion

        #region Methods

        public PathInfo GetFullPath() {
            var path = Path.GetFullPath(_path);
            return new PathInfo(path, Uninitialized);
        }

        #endregion
    }
}
