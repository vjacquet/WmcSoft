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
using System.Text;
using System.Xml;

namespace WmcSoft.Xml.XPath
{
    static class XPathLocator
    {
        public static string GetXPathTo(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            switch (node.NodeType) {
            case XmlNodeType.None:
            case XmlNodeType.EntityReference:
            case XmlNodeType.Entity:
            case XmlNodeType.DocumentType:
            case XmlNodeType.DocumentFragment:
            case XmlNodeType.Notation:
            case XmlNodeType.EndElement:
            case XmlNodeType.EndEntity:
            case XmlNodeType.XmlDeclaration:
                return null;
            case XmlNodeType.Attribute:
                return GetXPathTo((XmlAttribute)node);
            case XmlNodeType.Document:
                return "/";
            }

            var sb = new StringBuilder();
            NodeToXPath(node, sb);
            return sb.ToString();
        }

        public static string GetXPathTo(XmlAttribute node)
        {
            var sb = new StringBuilder();
            NodeToXPath(node.SelectSingleNode(".."), sb);

            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
                sb.Append("/@").Append(node.Name);
            else if (String.IsNullOrEmpty(node.Prefix) && node.LocalName == "xmlns")
                sb.Append("/namespace::*[local-name()='']");
            else
                sb.Append("/namespace::").Append(node.LocalName);
            return sb.ToString();
        }

        private static void NodeToXPath(XmlNode node, StringBuilder sb)
        {
            if (node != null) {
                var parentNode = node.ParentNode;
                if (parentNode == null) {
                    // for attributes for instance, the parent might be null
                    parentNode = node.SelectSingleNode("..");
                }
                NodeToXPath(parentNode, sb);
                AppendPathFromParent(node, sb);
            }
        }

        private static int PositionOfElement(XmlNode node)
        {
            var position = 1;
            foreach (XmlNode child in node.ParentNode.ChildNodes) {
                if (child == node)
                    return position;
                if ((child.NodeType == node.NodeType) && (child.Name == node.Name))
                    position++;
            }
            throw new InvalidOperationException();
        }

        private static int PositionOfTextNode(XmlNode node)
        {
            var position = 1;
            // consecutive text nodes should be merged.
            var flag = false;
            foreach (XmlNode child in node.ParentNode.ChildNodes) {
                if (child == node)
                    return position;
                var nodeType = child.NodeType;
                if (!IsTextNode(nodeType)) {
                    flag = false;
                } else if (!flag) {
                    flag = true;
                    position++;
                }
            }
            throw new InvalidOperationException();
        }

        private static void AppendPathFromParent(XmlNode node, StringBuilder sb)
        {
            switch (node.NodeType) {
            case XmlNodeType.Element:
                sb.Append('/').Append(node.Name);
                break;
            case XmlNodeType.Text:
            case XmlNodeType.CDATA:
            case XmlNodeType.Whitespace:
            case XmlNodeType.SignificantWhitespace:
                sb.Append("/text()");
                break;
            case XmlNodeType.ProcessingInstruction:
                sb.Append("/processing-instruction('").Append(node.Name).Append("')");
                break;
            case XmlNodeType.Comment:
                sb.Append("/comment()");
                break;
            case XmlNodeType.Document:
                return;
            }

            if (node.ParentNode == null)
                return;

            var position = IsTextNode(node.NodeType)
                ? PositionOfTextNode(node)
                : PositionOfElement(node);
            if (position > 1)
                sb.Append("[").Append(position).Append("]");
        }

        private static bool IsTextNode(XmlNodeType nt)
        {
            if (nt != XmlNodeType.SignificantWhitespace
                && nt != XmlNodeType.Whitespace
                && nt != XmlNodeType.Text
                && nt != XmlNodeType.CDATA)
                return nt == XmlNodeType.EntityReference;
            return true;
        }
    }
}
