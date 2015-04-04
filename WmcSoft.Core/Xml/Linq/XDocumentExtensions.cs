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
using System.Linq;
using System.Xml.Linq;

namespace WmcSoft.Xml.Linq
{
    public static class XDocumentExtensions
    {
        #region Navigation

        /// <summary>
        /// Returns the first Descendant <see cref="XElement"/> with the passed in <see cref="XName"/>.
        /// </summary>
        /// <param name="name">The <see cref="XName"/> to match against descendant <see cref="XElement"/>s.</param>
        /// <returns>The first <see cref="XElement"/> descendant that matches the <see cref="XName"/> passed in, or null.</returns>
        public static XElement Descendant(this XContainer container, XName name) {
            return container.Descendants(name).FirstOrDefault();
        }

        #endregion

        #region Set(Element|Attribute)?Value

        public static void SetValue(this XAttribute attribute, IFormattable value, string format, IFormatProvider formatProvider = null) {
            attribute.SetValue(value != null ? value.ToString(format, formatProvider) : "");
        }
        public static void SetValue(this XAttribute attribute, IFormattable value, IFormatProvider formatProvider) {
            attribute.SetValue(value, null, formatProvider);
        }

        public static void SetValue(this XElement element, IFormattable value, string format, IFormatProvider formatProvider = null) {
            element.SetValue(value != null ? value.ToString(format, formatProvider) : "");
        }
        public static void SetValue(this XElement element, IFormattable value, IFormatProvider formatProvider ) {
            element.SetValue(value, null, formatProvider);
        }

        public static void SetAttributeValue(this XElement element, XName name, IFormattable value, string format, IFormatProvider formatProvider = null) {
            element.SetAttributeValue(name, value != null ? value.ToString(format, formatProvider) : null);
        }
        public static void SetAttributeValue(this XElement element, XName name, IFormattable value, IFormatProvider formatProvider) {
            element.SetAttributeValue(name, value, null, formatProvider);
        }

        public static void SetElementValue(this XElement element, XName name, IFormattable value, string format, IFormatProvider formatProvider = null) {
            element.SetElementValue(name, value != null ? value.ToString(format, formatProvider) : null);
        }
        public static void SetElementValue(this XElement element, XName name, IFormattable value, IFormatProvider formatProvider) {
            element.SetElementValue(name, value, null, formatProvider);
        }

        #endregion
    }
}
