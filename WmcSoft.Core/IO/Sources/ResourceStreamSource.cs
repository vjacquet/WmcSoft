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
using System.Globalization;
using System.IO;
using System.Resources;

namespace WmcSoft.IO.Sources
{
    /// <summary>
    /// Returns <see cref="Stream"/> from the specified resource, using the specified culture.
    /// </summary>
    public sealed class ResourceStreamSource : IStreamSource
    {
        public ResourceStreamSource(Type resourceSource, string name, CultureInfo culture = null)
        {
            ResourceSource = resourceSource;
            Name = name;
            Culture = culture;
        }

        /// <summary>
        /// The type from which the resource manager derives all information for finding .resources files. 
        /// </summary>
        public Type ResourceSource { get; }

        /// <summary>
        /// The name of the resource.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// An object that specifies the culture to use for the resource lookup.
        /// If <see cref="Culture"/> is null, the culture for the current thread is used.
        /// </summary>
        public CultureInfo Culture { get; }

        public Stream OpenSource()
        {
            var rm = new ResourceManager(ResourceSource);
            return rm.GetStream(Name, Culture);
        }
    }
}
