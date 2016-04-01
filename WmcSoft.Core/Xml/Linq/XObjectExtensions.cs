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
    /// <summary>
    /// Provides a set of static methods to extend the <see cref="XObject"/> derived class. This is a static class.
    /// </summary>
    public static class XObjectExtensions
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

        /// <summary>
        /// Sets the value of this attribute, after formating it.
        /// </summary>
        /// <typeparam name="TFormattable">A type that implement <see cref="IFormattable"/>.</typeparam>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value.</param>
        /// <param name="format">
        /// The format to use.-or- A <c>null</c> reference (<c>Nothing</c> in Visual Basic) to use the
        /// default format defined for the type of the <see cref="IFormattable"/> implementation.
        /// </param>
        /// <param name="formatProvider">
        /// The provider to use to format the value.-or- A <c>null</c> reference (<c>Nothing</c> in Visual
        /// Basic) to obtain the numeric format information from the current locale setting
        /// of the operating system.
        /// </param>
        public static void SetValue<TFormattable>(this XAttribute attribute, TFormattable value, string format, IFormatProvider formatProvider = null)
          where TFormattable : IFormattable {
            attribute.SetValue(value != null ? value.ToString(format, formatProvider) : "");
        }

        /// <summary>
        /// Sets the value of this attribute, after formating it.
        /// </summary>
        /// <typeparam name="TFormattable">A type that implement <see cref="IFormattable"/>.</typeparam>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value.</param>
        /// <param name="formatProvider">
        /// The provider to use to format the value.-or- A <c>null</c> reference (<c>Nothing</c> in Visual
        /// Basic) to obtain the numeric format information from the current locale setting
        /// of the operating system.
        /// </param>
        public static void SetValue<TFormattable>(this XAttribute attribute, TFormattable value, IFormatProvider formatProvider)
          where TFormattable : IFormattable {
            SetValue(attribute, value, null, formatProvider);
        }

        /// <summary>
        /// Sets the value of this element, after formating it.
        /// </summary>
        /// <typeparam name="TFormattable">A type that implement <see cref="IFormattable"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        /// <param name="format">
        /// The format to use.-or- A <c>null</c> reference (<c>Nothing</c> in Visual Basic) to use the
        /// default format defined for the type of the <see cref="IFormattable"/> implementation.
        /// </param>
        /// <param name="formatProvider">
        /// The provider to use to format the value.-or- A <c>null</c> reference (<c>Nothing</c> in Visual
        /// Basic) to obtain the numeric format information from the current locale setting
        /// of the operating system.
        /// </param>
        public static void SetValue<TFormattable>(this XElement element, TFormattable value, string format, IFormatProvider formatProvider = null)
          where TFormattable : IFormattable {
            element.SetValue(value != null ? value.ToString(format, formatProvider) : "");
        }

        /// <summary>
        /// Sets the value of this element, after formating it.
        /// </summary>
        /// <typeparam name="TFormattable">A type that implement <see cref="IFormattable"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        /// <param name="formatProvider">
        /// The provider to use to format the value.-or- A <c>null</c> reference (<c>Nothing</c> in Visual
        /// Basic) to obtain the numeric format information from the current locale setting
        /// of the operating system.
        /// </param>
        public static void SetValue<TFormattable>(this XElement element, TFormattable value, IFormatProvider formatProvider)
          where TFormattable : IFormattable {
            SetValue(element, value, null, formatProvider);
        }

        /// <summary>
        /// Sets the value of an attribute, adds an attribute, after formating it.
        /// </summary>
        /// <typeparam name="TFormattable">A type that implement <see cref="IFormattable"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="name">An <seealso cref="XName"/> that contains the name of the attribute to change.</param>
        /// <param name="value">The value.</param>
        /// <param name="format">
        /// The format to use.-or- A <c>null</c> reference (<c>Nothing</c> in Visual Basic) to use the
        /// default format defined for the type of the <see cref="IFormattable"/> implementation.
        /// </param>
        /// <param name="formatProvider">
        /// The provider to use to format the value.-or- A <c>null</c> reference (<c>Nothing</c> in Visual
        /// Basic) to obtain the numeric format information from the current locale setting
        /// of the operating system.
        /// </param>
        public static void SetAttributeValue<TFormattable>(this XElement element, XName name, TFormattable value, string format, IFormatProvider formatProvider = null)
          where TFormattable : IFormattable {
            element.SetAttributeValue(name, value != null ? value.ToString(format, formatProvider) : null);
        }

        /// <summary>
        /// Sets the value of an attribute, adds an attribute, after formating it.
        /// </summary>
        /// <typeparam name="TFormattable">A type that implement <see cref="IFormattable"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="name">An <seealso cref="XName"/> that contains the name of the attribute to change.</param>
        /// <param name="value">The value.</param>
        /// <param name="formatProvider">
        /// The provider to use to format the value.-or- A <c>null</c> reference (<c>Nothing</c> in Visual
        /// Basic) to obtain the numeric format information from the current locale setting
        /// of the operating system.
        /// </param>
        public static void SetAttributeValue<TFormattable>(this XElement element, XName name, TFormattable value, IFormatProvider formatProvider)
          where TFormattable : IFormattable {
            SetAttributeValue(element, name, value, null, formatProvider);
        }

        /// <summary>
        /// Sets the value of a child element, adds a child element, after formating it.
        /// </summary>
        /// <typeparam name="TFormattable">A type that implement <see cref="IFormattable"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="name">An <seealso cref="XName"/> that contains the name of the attribute to change.</param>
        /// <param name="value">The value.</param>
        /// <param name="format">
        /// The format to use.-or- A <c>null</c> reference (<c>Nothing</c> in Visual Basic) to use the
        /// default format defined for the type of the <see cref="IFormattable"/> implementation.
        /// </param>
        /// <param name="formatProvider">
        /// The provider to use to format the value.-or- A <c>null</c> reference (<c>Nothing</c> in Visual
        /// Basic) to obtain the numeric format information from the current locale setting
        /// of the operating system.
        /// </param>
        public static void SetElementValue<TFormattable>(this XElement element, XName name, TFormattable value, string format, IFormatProvider formatProvider = null)
          where TFormattable : IFormattable {
            element.SetElementValue(name, value != null ? value.ToString(format, formatProvider) : null);
        }

        /// <summary>
        /// Sets the value of a child element, adds a child element, after formating it.
        /// </summary>
        /// <typeparam name="TFormattable">A type that implement <see cref="IFormattable"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="name">An <seealso cref="XName"/> that contains the name of the attribute to change.</param>
        /// <param name="value">The value.</param>
        /// <param name="formatProvider">
        /// The provider to use to format the value.-or- A <c>null</c> reference (<c>Nothing</c> in Visual
        /// Basic) to obtain the numeric format information from the current locale setting
        /// of the operating system.
        /// </param>
        public static void SetElementValue<TFormattable>(this XElement element, XName name, TFormattable value, IFormatProvider formatProvider)
          where TFormattable : IFormattable {
            SetElementValue(element, name, value, null, formatProvider);
        }

        #endregion
    }
}
