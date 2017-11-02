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

using System.IO;
using System.Reflection;

namespace WmcSoft.IO
{
    /// <summary>
    /// Returns a <see cref="Stream"/> from the specified manifest resource from the specified assembly.
    /// </summary>
    public sealed class AssemblyManifestResourceStreamSource : IStreamSource
    {
        public AssemblyManifestResourceStreamSource(Assembly assembly, string name)
        {
            Assembly = assembly;
            Name = name;
        }

        /// <summary>
        /// The assembly containing the resource.
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>
        /// The case-sensitive name of the manifest resource being requested. 
        /// </summary>
        public string Name { get; }

        public Stream GetStream()
        {
            return Assembly.GetManifestResourceStream(Name);
        }
    }
}
