#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
using System.Runtime.Serialization;

namespace WmcSoft.Runtime.Serialization
{
    public static class SerializationInfoExtensions
    {
        /// <summary>
        /// Retrieves a value from the <see cref="SerializationInfo"/> store.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the value to retrieve. If the stored value cannot be converted
        /// to this type, the system will throw a System.InvalidCastException.</typeparam>
        /// <param name="info">The store.</param>
        /// <param name="name">The name associated with the value to retrieve.</param>
        /// <returns>The instance of <typeparamref name="T"/> associated with <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        /// <exception cref="InvalidCastException">The value associated with <paramref name="name"/> cannot be converted to <typeparamref name="T"/>.</exception>
        /// <exception cref="SerializationException">An element with the specified name is not found in the given store.</exception>
        /// <exception cref="NullReferenceException">The store is <c>null</c>.</exception>
        /// <remarks>Because serialization needs performance, a <see cref="NullReferenceException"/> is throw instead of <see cref="ArgumentNullException"/> when the first parameter is null.</remarks>
        public static T GetValue<T>(this SerializationInfo info, string name) {
            return (T)info.GetValue(name, typeof(T));
        }
    }
}
