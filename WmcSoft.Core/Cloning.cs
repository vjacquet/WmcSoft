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
using System.Runtime.Serialization;
using WmcSoft.IO;

using Cloner = System.Runtime.Serialization.Formatters.Binary.BinaryFormatter;

namespace WmcSoft
{
    /// <summary>
    /// Defines markers and extensions to help cloing items.
    /// This is a static class.
    /// </summary>
    public static class Cloning
    {
        #region Markers

        /// <summary>
        /// Marker to indicate the cloning should be deep.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "This marker struct make senss only in the context of cloning.")]
        public struct Deep { }

        /// <summary>
        /// Suggest the cloning should be deep.
        /// </summary>
        public static readonly Deep SuggestDeep;

        /// <summary>
        /// Marker to indicate the cloning should be shallow.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "This marker struct make senss only in the context of cloning.")]
        public struct Shallow { }

        /// <summary>
        /// Suggest the cloning should be shallow.
        /// </summary>
        public static readonly Shallow SuggestShallow;

        #endregion

        #region Clone

        /// <summary>
        /// Clone the instance. If the type does not implement the <see cref="ICloneable"/> interface, then this method tries to serialize/deserialize the instance.
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="obj">The instance</param>
        /// <returns>A clone of the instance.</returns>
        /// <remarks>This helper works better for classes implementing <see cref="ICloneable"/> explicitly.</remarks>
        public static T Clone<T>(T obj)
            where T : class
        {
            if (obj == null)
                return null;

            if (obj is ICloneable cloneable)
                return (T)cloneable.Clone();

            using (var ms = new MemoryStream()) {
                var f = new Cloner(null, new StreamingContext(StreamingContextStates.Clone));
                f.Serialize(ms, obj);
                ms.Rewind();
                return (T)f.Deserialize(ms);
            }
        }

        #endregion
    }
}
