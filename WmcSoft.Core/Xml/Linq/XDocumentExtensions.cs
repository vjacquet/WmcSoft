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

        #region SetValue

        public static void SetValue(this XAttribute attribute, IFormattable value, string format, IFormatProvider formatProvider = null) {
            attribute.SetValue(value != null ? value.ToString(format, formatProvider) : null);
        }
        public static void SetValue<T>(this XAttribute attribute, T? value, string format, IFormatProvider formatProvider = null)
            where T : struct, IFormattable {
            attribute.SetValue(value.HasValue ? value.GetValueOrDefault().ToString(format, formatProvider) : null);
        }

        public static void SetValue(this XElement element, IFormattable value, string format, IFormatProvider formatProvider = null) {
            element.SetValue(value != null ? value.ToString(format, formatProvider) : null);
        }
        public static void SetValue<T>(this XElement element, T? value, string format, IFormatProvider formatProvider = null)
            where T : struct, IFormattable {
            element.SetValue(value.HasValue ? value.GetValueOrDefault().ToString(format, formatProvider) : null);
        }

        public static void SetAttributeValue(this XElement element, XName name, IFormattable value, string format, IFormatProvider formatProvider = null) {
            element.SetAttributeValue(name, value != null ? value.ToString(format, formatProvider) : null);
        }
        public static void SetValue<T>(this XElement element, XName name, T? value, string format, IFormatProvider formatProvider = null)
            where T : struct, IFormattable {
                element.SetAttributeValue(name, value.HasValue ? value.GetValueOrDefault().ToString(format, formatProvider) : null);
        }

        public static void SetElementValue(this XElement element, XName name, IFormattable value, string format, IFormatProvider formatProvider) {
            element.SetElementValue(name, value != null ? value.ToString(format, formatProvider) : null);
        }
        public static void SetElementValue<T>(this XElement element, XName name, T? value, string format, IFormatProvider formatProvider)
            where T : struct, IFormattable {
                element.SetElementValue(name, value.HasValue ? value.GetValueOrDefault().ToString(format, formatProvider) : null);
        }

        #endregion
    }
}
