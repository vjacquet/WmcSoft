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

using System.Xml;
using System.Xml.XPath;

namespace WmcSoft.Xml
{
    public static class XmlWriterExtensions
    {
        public static void WriteNodeChildren(this XmlWriter writer, XPathNavigator navigator) {
            if (navigator.MoveToFirstChild()) {
                do {
                    writer.WriteNode(navigator.CreateNavigator(), true);
                } while (navigator.MoveToNext());
            }
        }

        /// <remarks>Adapted frop Mark Russel's code. See http://blogs.msdn.com/b/mfussell/archive/2005/02/12/371546.aspx .</remarks>
        public static XmlWriter WriteShallowNode(this XmlWriter writer, XmlReader reader) {
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
    }
}
