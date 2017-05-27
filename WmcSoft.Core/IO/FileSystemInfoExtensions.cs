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

using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;

namespace WmcSoft.IO
{
    public static class FileSystemInfoExtensions
    {
        public static bool DeleteAll(this IEnumerable<FileInfo> files)
        {
            bool failed = false;
            foreach (var file in files) {
                try {
                    file.Delete();
                } catch {
                    failed = true;
                }
            }
            return !failed;
        }

        public static bool DeleteAllFiles(this DirectoryInfo directory)
        {
            return DeleteAll(directory.EnumerateFiles());
        }

        public static bool DeleteAllFiles(this DirectoryInfo directory, string searchPattern)
        {
            return DeleteAll(directory.EnumerateFiles(searchPattern));
        }

        public static bool DeleteAllFiles(this DirectoryInfo directory, string searchPattern, SearchOption searchOption)
        {
            return DeleteAll(directory.EnumerateFiles(searchPattern, searchOption));
        }

        public static string GetContentType(this FileInfo file)
        {
            object value;
            var key = Registry.ClassesRoot.OpenSubKey(file.Extension);
            if (key != null && (value = key.GetValue("Content Type")) != null) {
                return value.ToString();
            }
            return "application/octetstream";
        }
    }
}
