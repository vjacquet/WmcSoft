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
        static readonly UninitializedTag Uninitialized = default;

        #region Fields

        private readonly string path;

        #endregion

        #region Lifecycle

        private PathInfo(UninitializedTag tag, string value)
        {
            path = value;
        }

        public PathInfo(string path)
        {
            CheckInvalidPathChars(path);
            this.path = path;
        }

        public PathInfo(string path1, string path2)
            : this(Uninitialized, Path.Combine(path1, path2))
        {
        }

        public PathInfo(string path1, string path2, string path3)
            : this(Uninitialized, Path.Combine(path1, path2, path3))
        {
        }

        public PathInfo(string path1, string path2, string path3, string path4)
            : this(Uninitialized, Path.Combine(path1, path2, path3, path4))
        {
        }

        public PathInfo(params string[] paths)
            : this(Uninitialized, Path.Combine(paths))
        {
        }

        #endregion

        #region Operators

        /// <summary>
        /// Creates a path.
        /// </summary>
        /// <param name="path">The path to create the path info to.</param>
        public static implicit operator PathInfo(string path)
        {
            return new PathInfo(path);
        }

        /// <summary>
        /// Converts a <see cref="path"/> to a string.
        /// </summary>
        /// <param name="path">The path info.</param>
        public static explicit operator string(PathInfo path)
        {
            return path.ToString();
        }

        /// <summary>
        /// Combines the two paths.
        /// </summary>
        /// <param name="path1">The first path.</param>
        /// <param name="path2">The second path.</param>
        /// <returns>The combined path.</returns>
        public static PathInfo operator /(PathInfo path1, string path2)
        {
            return new PathInfo(Uninitialized, Path.Combine(path1.path, path2));
        }

        /// <summary>
        /// Combines the two paths.
        /// </summary>
        /// <param name="path1">The first path.</param>
        /// <param name="path2">The second path.</param>
        /// <returns>The combined path.</returns>
        public static PathInfo operator /(PathInfo path1, PathInfo path2)
        {
            return new PathInfo(Uninitialized, Path.Combine(path1.path, path2.path));
        }

        #endregion

        #region Properties

        public PathInfo? Root => Maybe(Path.GetPathRoot(path));

        public string DirectoryName => Path.GetDirectoryName(path);

        public string FileName => Path.GetFileName(path);

        public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(path);

        public string Extension => NullifyEmpty(Path.GetExtension(path));

        #endregion

        #region Methods

        public PathInfo AppendBeforeExtension(string suffix)
        {
            if (string.IsNullOrEmpty(suffix))
                return this;
            var extension = Extension;
            return new PathInfo(Uninitialized, Path.ChangeExtension(path, null) + suffix + extension);
        }

        public PathInfo ChangeExtension(string extension)
        {
            return new PathInfo(Uninitialized, Path.ChangeExtension(path, extension));
        }

        public PathInfo ReplaceExtension(string oldExtension, string newExtension, StringComparer comparer = null)
        {
            comparer = comparer ?? StringComparer.CurrentCultureIgnoreCase;
            if (comparer.Equals(Extension, oldExtension))
                return ChangeExtension(newExtension);
            return this;
        }

        public PathInfo GetFullPath()
        {
            var path = Path.GetFullPath(this.path);
            return new PathInfo(Uninitialized, path);
        }

        public override string ToString()
        {
            return path;
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format) {
            case "f":
            case "F":
                return FileName;
            case "p":
            case "P":
                return Path.GetFullPath(path);
            case "x":
            case "X":
                return Extension;
            case "n":
            case "N":
                return FileNameWithoutExtension;
            case "u":
            case "U":
                // TODO: uses plateform specific values.
                return path.Replace('\\', '/');
            default:
                return path;
            }
        }

        #endregion

        #region Helpers

        static PathInfo? Maybe(string value)
        {
            return !string.IsNullOrEmpty(value) ? new PathInfo(Uninitialized, value) : default(PathInfo?);
        }

        static string NullifyEmpty(string s)
        {
            return !string.IsNullOrEmpty(s) ? s : null;
        }

        static void CheckInvalidPathChars(string path, string name = null)
        {
            if (path == null) throw new ArgumentNullException(name ?? nameof(path));

            if (path.IndexOfAny(InvalidPathChars) >= 0)
                throw new ArgumentException(name ?? nameof(path));
        }

        #endregion
    }
}
