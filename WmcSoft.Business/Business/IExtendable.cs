#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Business
{
    /// <summary>
    /// Defines a contract to identify extensions on a component.
    /// </summary>
    /// <typeparam name="I">The interface the extensions must implement.</typeparam>
    public interface IExtendable<I>
    {
        /// <summary>
        /// Gets the extensions.
        /// </summary>
        IEnumerable<I> Extensions { get; }

        /// <summary>
        /// Gets the extension of the specified type. Returns <c>null</c> if no extension of the specified type is configured.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension.</typeparam>
        /// <returns>The extension of the given type, or <c>null</c>.</returns>
        TExtension FindExtension<TExtension>() where TExtension : class, I;
    }
}
