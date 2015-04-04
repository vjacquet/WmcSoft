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
using System.Reflection;

namespace WmcSoft.Reflection
{
    public static class AssemblyExtensions
    {
        public static string GetAssemblyConfiguration(this Assembly targetAssembly) {
            if (targetAssembly == null)
                throw new ArgumentNullException("targetAssembly");

            var attribute = targetAssembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
            if (attribute != null)
                return attribute.Configuration;
            return String.Empty;
        }

        public static string GetAssemblyFileVersion(this Assembly targetAssembly) {
            if (targetAssembly == null)
                throw new ArgumentNullException("targetAssembly");

            var attribute = targetAssembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            if (attribute != null)
                return attribute.Version;
            return String.Empty;
        }

        public static string GetAssemblyFolder(this Assembly targetAssembly) {
            if (targetAssembly == null)
                throw new ArgumentNullException("targetAssembly");

            string assemblyPath = GetAssemblyPath(targetAssembly);
            if (assemblyPath.IndexOf('\\') != -1) {
                assemblyPath = assemblyPath.Substring(0, assemblyPath.LastIndexOf('\\'));
            }
            return assemblyPath;
        }

        public static string GetAssemblyPath(this Assembly targetAssembly) {
            if (targetAssembly == null)
                throw new ArgumentNullException("targetAssembly");
            if (targetAssembly.CodeBase == null || targetAssembly.CodeBase.Length == 0)
                throw new ArgumentNullException("targetAssembly.CodeBase");

            var uri = new Uri(targetAssembly.CodeBase);
            return uri.LocalPath.Replace(@"\\", @"\");
        }

        public static string GetAssemblyProduct(this Assembly targetAssembly) {
            if (targetAssembly == null)
                throw new ArgumentNullException("targetAssembly");

            var attribute = targetAssembly.GetCustomAttribute<AssemblyProductAttribute>();
            if (attribute != null)
                return attribute.Product;
            return String.Empty;
        }

        public static string GetAssemblyTitle(this Assembly targetAssembly) {
            if (targetAssembly == null)
                throw new ArgumentNullException("targetAssembly");

            var attribute = targetAssembly.GetCustomAttribute<AssemblyTitleAttribute>();
            if (attribute != null)
                return attribute.Title;
            return String.Empty;
        }
    }
}
