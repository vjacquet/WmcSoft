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
    /// Represents a file path.
    /// </summary>
    public struct PathInfo : IFormattable
    {
        private static readonly char[] InvalidPathChars = Path.GetInvalidPathChars();

        struct UninitializedTag { }
        static readonly UninitializedTag Uninitialized = default(UninitializedTag);

        #region Fields

        private readonly string _path;

        #endregion

        #region Lifecycle

        private PathInfo(UninitializedTag tag, string value) {
            _path = value;
        }

        public PathInfo(string path) {
            CheckInvalidPathChars(path);
            _path = path;
        }

        public PathInfo(string path1, string path2)
            : this(Uninitialized, Path.Combine(path1, path2)) {
        }

        public PathInfo(string path1, string path2, string path3)
            : this(Uninitialized, Path.Combine(path1, path2, path3)) {
        }

        public PathInfo(string path1, string path2, string path3, string path4)
            : this(Uninitialized, Path.Combine(path1, path2, path3, path4)) {
        }

        public PathInfo(params string[] paths)
            : this(Uninitialized, Path.Combine(paths)) {
        }

        #endregion

        #region Operators

        public static implicit operator PathInfo(string value) {
            return new PathInfo(value);
        }

        public static explicit operator string(PathInfo value) {
            if (value._path == null) throw new InvalidCastException();
            return value._path;
        }

        public static PathInfo operator /(PathInfo path1, string path2) {
            return new PathInfo(Uninitialized, Path.Combine(path1._path, path2));
        }

        public static PathInfo operator /(PathInfo path1, PathInfo path2) {
            return new PathInfo(Uninitialized, Path.Combine(path1._path, path2._path));
        }

        #endregion

        #region Properties

        public PathInfo? Root {
            get {
                var root = Path.GetPathRoot(_path);
                if (root == "")
                    return null;
                return new PathInfo(Uninitialized, root);
            }
        }

        public string FileName {
            get { return Path.GetFileName(_path); }
        }

        public string FileNameWithoutExtension {
            get { return Path.GetFileNameWithoutExtension(_path); }
        }

        public string Extension {
            get {
                var extension = Path.GetExtension(_path);
                if (extension == "")
                    return null;
                return extension;
            }
        }

        #endregion

        #region Methods

        public PathInfo GetFullPath() {
            var path = Path.GetFullPath(_path);
            return new PathInfo(Uninitialized, path);
        }

        public override string ToString() {
            return _path;
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            switch (format) {
            case "f":
            case "F":
                return FileName;
            case "p":
            case "P":
                return Path.GetFullPath(_path);
            case "x":
            case "X":
                return Extension;
            case "n":
            case "N":
                return FileNameWithoutExtension;
            case "u":
            case "U":
                return _path.Replace('\\', '/');
            default:
                return _path;
            }
        }

        #endregion

        #region Helpers

        static void CheckInvalidPathChars(string path, string name = null) {
            if (path == null) throw new ArgumentNullException(name ?? nameof(path));
            if (path.IndexOfAny(InvalidPathChars) >= 0)
                throw new ArgumentException(name ?? nameof(path));
        }

        #endregion
    }
}
