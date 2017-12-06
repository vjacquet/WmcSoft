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

namespace WmcSoft
{
    /// <summary>
    /// Defines the extension methods to the <see cref="ICloneable"/> interface.
    /// This is a static class.
    /// </summary>
    public static class CloneableExtensions
    {
        #region Clone

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="obj">The instance</param>
        /// <returns>A new object that is a copy of this instance.</returns>
        public static T Clone<T>(this T obj)
            where T : ICloneable
        {
            return (T)obj.Clone();
        }

        #endregion

        #region Duplicate

        /// <summary>
        /// Duplicates an object by cloning and adapting an instance.
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="obj">The instance</param>
        /// <param name="adapter">The adapter to apply on the clone.</param>
        /// <returns>A new object that is a duplicate of this instance.</returns>
        public static T Duplicate<T>(this T obj, Action<T> adapter)
            where T : class, ICloneable
        {
            var clone = (T)obj.Clone();
            adapter(clone);
            return clone;
        }

        #endregion
    }
}
