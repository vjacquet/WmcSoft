using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
