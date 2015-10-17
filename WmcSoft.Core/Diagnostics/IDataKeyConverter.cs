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

namespace WmcSoft.Diagnostics
{
    /// <summary>
    /// Defines a generalized strategy to convert value to key to <see cref="System.Exception"/>'s <see cref="System.Exception.Data"/> property keys.
    /// </summary>
    public interface IDataKeyConverter
    {
        /// <summary>
        /// Converts a <paramref name="name"/> to a key.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The key.</returns>
        object ToKey(string name);

        /// <summary>
        /// Converts a <paramref name="key"/> back to its name.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The name.</returns>
        string FromKey(object key);
    }
}
