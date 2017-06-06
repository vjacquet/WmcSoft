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
using System.Xml.XPath;

namespace WmcSoft.Xml
{
    public static class XmlWriterExtensions
    {
        /// <summary>
        /// Copies The matching children to the writer.
        /// </summary>
        /// <param name="writer">The writer to copy to.</param>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to copy from.</param>
        /// <param name="defattr">true to copy the default attributes, otherwise false.</param>
        /// <param name="predicate">The predicate applied to the children to filter which are written to the writer.</param>
        /// <returns>The count of children wrote to the writer.</returns>
        /// <remarks>The position of the <see cref="XPathNavigator"/> remains unchanged.</remarks>
        public static int WriteNodeChildren(this XmlWriter writer, XPathNavigator navigator, bool defattr = true, Predicate<XPathNavigator> predicate = null)
        {
            var nav = navigator.CreateNavigator();

            if (!nav.MoveToFirstChild())
                return 0;

            var count = 0;
            if (predicate == null) {
                do {
                    writer.WriteNode(nav.CreateNavigator(), defattr);
                    count++;
                } while (nav.MoveToNext());
            } else {
                do {
                    // create a copy of the navigator for the predicate too, so it could not alter the
                    // current position;
                    if (predicate(nav.CreateNavigator())) {
                        writer.WriteNode(nav.CreateNavigator(), defattr);
                        count++;
                    }
                } while (nav.MoveToNext());
            }
            return count;
        }

        /// <remarks>Adapted from Mark Russel's code. See http://blogs.msdn.com/b/mfussell/archive/2005/02/12/371546.aspx .</remarks>
        public static XmlWriter WriteShallowNode(this XmlWriter writer, XmlReader reader)
        {
            switch (reader.NodeType) {
            case XmlNodeType.Element:
                writer.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                writer.WriteAttributes(reader, true);
                if (reader.IsEmptyElement)
                    writer.WriteEndElement();
                break;
            case XmlNodeType.Text:
                writer.WriteString(reader.Value);
                break;
            case XmlNodeType.Whitespace:
            case XmlNodeType.SignificantWhitespace:
                writer.WriteWhitespace(reader.Value);
                break;
            case XmlNodeType.CDATA:
                writer.WriteCData(reader.Value);
                break;
            case XmlNodeType.EntityReference:
                writer.WriteEntityRef(reader.Name);
                break;
            case XmlNodeType.XmlDeclaration:
            case XmlNodeType.ProcessingInstruction:
                writer.WriteProcessingInstruction(reader.Name, reader.Value);
                break;
            case XmlNodeType.DocumentType:
                writer.WriteDocType(reader.Name, reader.GetAttribute("PUBLIC"), reader.GetAttribute("SYSTEM"), reader.Value);
                break;
            case XmlNodeType.Comment:
                writer.WriteComment(reader.Value);
                break;
            case XmlNodeType.EndElement:
                writer.WriteFullEndElement();
                break;
            }
            return writer;
        }

        #region WriteAttributeValue

        public static void WriteAttributeValue(this XmlWriter writer, string prefix, string localName, string ns, double value)
        {
            writer.WriteStartAttribute(prefix, localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, string ns, double value)
        {
            writer.WriteStartAttribute(localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, double value)
        {
            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        public static void WriteAttributeValue(this XmlWriter writer, string prefix, string localName, string ns, float value)
        {
            writer.WriteStartAttribute(prefix, localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, string ns, float value)
        {
            writer.WriteStartAttribute(localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, float value)
        {
            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        public static void WriteAttributeValue(this XmlWriter writer, string prefix, string localName, string ns, decimal value)
        {
            writer.WriteStartAttribute(prefix, localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, string ns, decimal value)
        {
            writer.WriteStartAttribute(localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, decimal value)
        {
            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        public static void WriteAttributeValue(this XmlWriter writer, string prefix, string localName, string ns, int value)
        {
            writer.WriteStartAttribute(prefix, localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, string ns, int value)
        {
            writer.WriteStartAttribute(localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, int value)
        {
            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        public static void WriteAttributeValue(this XmlWriter writer, string prefix, string localName, string ns, long value)
        {
            writer.WriteStartAttribute(prefix, localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, string ns, long value)
        {
            writer.WriteStartAttribute(localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, long value)
        {
            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        public static void WriteAttributeValue(this XmlWriter writer, string prefix, string localName, string ns, DateTimeOffset value)
        {
            writer.WriteStartAttribute(prefix, localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, string ns, DateTimeOffset value)
        {
            writer.WriteStartAttribute(localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, DateTimeOffset value)
        {
            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        public static void WriteAttributeValue(this XmlWriter writer, string prefix, string localName, string ns, bool value)
        {
            writer.WriteStartAttribute(prefix, localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, string ns, bool value)
        {
            writer.WriteStartAttribute(localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, bool value)
        {
            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        public static void WriteAttributeValue(this XmlWriter writer, string prefix, string localName, string ns, DateTime value)
        {
            writer.WriteStartAttribute(prefix, localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, string ns, DateTime value)
        {
            writer.WriteStartAttribute(localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, DateTime value)
        {
            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        public static void WriteAttributeValue(this XmlWriter writer, string prefix, string localName, string ns, object value)
        {
            writer.WriteStartAttribute(prefix, localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, string ns, object value)
        {
            writer.WriteStartAttribute(localName, ns);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
        public static void WriteAttributeValue(this XmlWriter writer, string localName, object value)
        {
            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        #endregion
    }
}
