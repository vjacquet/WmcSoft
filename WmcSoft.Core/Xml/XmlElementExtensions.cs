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
using System.Xml;

namespace WmcSoft.Xml
{
    /// <summary>
    /// Provides a set of static methods to extend the <see cref="XmlElement"/> derived class. This is a static class.
    /// </summary>
    public static class XmlElementExtensions
    {
        /// <summary>
        /// Returns the value for the attribute with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="element">The element</param>
        /// <param name="name">The name of the attribute to retrieve. This is a qualified name. It is matched against the <see cref="XmlNode.Name"/> property of the matching node. </param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted value of the specified attribute. <see cref="defaultValue"/> is returned if a matching attribute is not found or if the attribute does not have a specified or default value.</returns>
        public static T GetAttribute<T>(this XmlElement element, string name, T defaultValue = default(T))
            where T : IConvertible
        {
            var value = element.GetAttribute(name);
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Returns the value for the attribute with the specified local name and namespace URI.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="localName">The local name of the attribute to retrieve. </param>
        /// <param name="namespaceURI">The namespace URI of the attribute to retrieve. </param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted value of the specified attribute. <see cref="defaultValue"/> is returned if a matching attribute is not found or if the attribute does not have a specified or default value.</returns>
        public static T GetAttribute<T>(this XmlElement element, string localName, string namespaceURI, T defaultValue = default(T))
            where T : IConvertible
        {
            var value = element.GetAttribute(localName, namespaceURI);
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
