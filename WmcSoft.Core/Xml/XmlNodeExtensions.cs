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
using System.Collections.Generic;
using System.Xml;
using WmcSoft.Xml.XPath;

namespace WmcSoft.Xml
{
    public static class XmlNodeExtensions
    {
        public static XmlAttribute RemoveAttributeNode(this XmlNode node, string name) {
            return (XmlAttribute)node.Attributes.RemoveNamedItem(name);
        }

        public static string GetValueOrNull(this XmlNode node) {
            if (node != null)
                return node.Value;
            return null;
        }

        public static XmlWriter ReplaceSelf(this XmlNode node) {
            var navigator = node.CreateNavigator();
            return navigator.ReplaceRange(navigator);
        }

        public static XmlWriter ReplaceContent(this XmlNode node) {
            var first = node.FirstChild.CreateNavigator();
            var last = node.LastChild.CreateNavigator();
            return first.ReplaceRange(last);
        }

        public static bool HasValue(this XmlNode self, string xpath) {
            var node = self.SelectSingleNode(xpath);
            if (node == null)
                return false;
            return !String.IsNullOrEmpty(node.InnerText);
        }
        public static bool HasValue(this XmlNode self, string xpath, XmlNamespaceManager nsmgr) {
            var node = self.SelectSingleNode(xpath, nsmgr);
            if (node == null)
                return false;
            return !String.IsNullOrEmpty(node.InnerText);
        }

        public static string GetValue(this XmlNode self, string xpath) {
            var node = self.SelectSingleNode(xpath);
            return (node == null) ? "" : (node.Value ?? node.InnerText);
        }
        public static string GetValue(this XmlNode self, string xpath, XmlNamespaceManager nsmgr) {
            var node = self.SelectSingleNode(xpath, nsmgr);
            return (node == null) ? "" : (node.Value ?? node.InnerText);
        }

        public static IEnumerable<string> GetValues(this XmlNode self, string xpath) {
            foreach (XmlNode node in self.SelectNodes(xpath))
                yield return node.Value ?? node.InnerText;
        }
        public static IEnumerable<string> GetValues(this XmlNode self, string xpath, XmlNamespaceManager nsmgr) {
            foreach (XmlNode node in self.SelectNodes(xpath, nsmgr))
                yield return node.Value ?? node.InnerText;
        }

        public static T GetValue<T>(this XmlNode self, string xpath) where T : IConvertible {
            var value = GetValue(self, xpath);
            if (String.IsNullOrEmpty(value))
                return default(T);
            return (T)Convert.ChangeType(value, typeof(T));
        }
        public static T GetValue<T>(this XmlNode self, string xpath, XmlNamespaceManager nsmgr) where T : IConvertible {
            var value = GetValue(self, xpath, nsmgr);
            if (String.IsNullOrEmpty(value))
                return default(T);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static IEnumerable<T> GetValues<T>(this XmlNode self, string xpath) {
            foreach (string value in GetValues(self, xpath)) {
                if (String.IsNullOrEmpty(value))
                    yield return default(T);
                yield return (T)Convert.ChangeType(value, typeof(T));
            }
        }
        public static IEnumerable<T> GetValues<T>(this XmlNode self, string xpath, XmlNamespaceManager nsmgr) {
            foreach (string value in GetValues(self, xpath, nsmgr)) {
                if (String.IsNullOrEmpty(value))
                    yield return default(T);
                yield return (T)Convert.ChangeType(value, typeof(T));
            }
        }

        public static string GetInnerXml(this XmlNode self, string xpath) {
            var node = self.SelectSingleNode(xpath);
            return (node == null) ? "" : node.InnerXml;
        }
        public static string GetInnerXml(this XmlNode self, string xpath, XmlNamespaceManager nsmgr) {
            var node = self.SelectSingleNode(xpath, nsmgr);
            return (node == null) ? "" : node.InnerXml;
        }

        public static string GetInnerText(this XmlNode self, string xpath) {
            var node = self.SelectSingleNode(xpath);
            return (node == null) ? "" : node.InnerText;
        }
        public static string GetInnerText(this XmlNode self, string xpath, XmlNamespaceManager nsmgr) {
            var node = self.SelectSingleNode(xpath, nsmgr);
            return (node == null) ? "" : node.InnerText;
        }

        public static string GetXPath(this XmlNode self) {
            return XPathLocator.GetXPathTo(self);
        }

        public static string GetXPath(this XmlAttribute self) {
            return XPathLocator.GetXPathTo(self);
        }

        public static XmlNode InsertBefore(this XmlNode self, XmlNode node) {
            return self.ParentNode.InsertBefore(node, self);
        }

        public static XmlNode InsertAfter(this XmlNode self, XmlNode node) {
            return self.ParentNode.InsertAfter(node, self);
        }

        public static int IndexOf(this XmlNode self) {
            return IndexOf(self.ParentNode.ChildNodes, self);
        }

        public static int IndexOf(this XmlNode self, XmlNode child) {
            return IndexOf(self.ChildNodes, child);
        }

        public static int IndexOf(this XmlNodeList self, XmlNode child) {
            int index = 0;
            foreach (XmlNode node in self) {
                if (child == node)
                    return index;
                index++;
            }
            return -1;
        }

        /// <summary>
        /// Selects a list of nodes matching the XPath expression.
        /// </summary>
        /// <param name="node">The current node.</param>
        /// <param name="xpath">The XPath expression.</param>
        /// <returns>An <see cref="XmlNodeList"/> containing a collection of nodes matching the XPath query, once removed from the document.</returns>
        /// <exception cref="XPathException">The XPath expression contains a prefix.</exception>
        public static XmlNodeList RemoveNodes(this XmlNode node, string xpath) {
            var list = node.SelectNodes(xpath);
            DetachAll(list);
            return list;
        }

        /// <summary>
        /// Selects a list of nodes matching the XPath expression. Any prefixes found in
        /// the XPath expression are resolved using the supplied System.Xml.XmlNamespaceManager.
        /// </summary>
        /// <param name="node">The current node.</param>
        /// <param name="xpath">The XPath expression.</param>
        /// <param name="nsmgr">An <see cref="XmlNamespaceManager"/> to use for resolving namespaces for prefixes in the XPath expression.</param>
        /// <returns>An <see cref="XmlNodeList"/> containing a collection of nodes matching the XPath query, once removed from the document.</returns>
        /// <exception cref="XPathException">The XPath expression contains a prefix which is not defined in the XmlNamespaceManager.</exception>
        public static XmlNodeList RemoveNodes(this XmlNode node, string xpath, XmlNamespaceManager nsmgr) {
            var list = node.SelectNodes(xpath, nsmgr);
            DetachAll(list);
            return list;
        }

        public static bool Detach(this XmlAttribute attr) {
            var owner = attr.OwnerElement;
            if (owner != null) {
                owner.Attributes.Remove(attr);
                return true;
            }
            return false;
        }

        public static bool Detach(this XmlNode node) {
            if (node.ParentNode != null) {
                node.ParentNode.RemoveChild(node);
                return true;
            }

            switch (node.NodeType) {
            case XmlNodeType.None:
                break;
            case XmlNodeType.Element:
                break;
            case XmlNodeType.Attribute:
                return Detach((XmlAttribute)node);
            case XmlNodeType.Text:
                break;
            case XmlNodeType.CDATA:
                break;
            case XmlNodeType.EntityReference:
                break;
            case XmlNodeType.Entity:
                break;
            case XmlNodeType.ProcessingInstruction:
                break;
            case XmlNodeType.Comment:
                break;
            case XmlNodeType.Document:
                break;
            case XmlNodeType.DocumentType:
                break;
            case XmlNodeType.DocumentFragment:
                break;
            case XmlNodeType.Notation:
                break;
            case XmlNodeType.Whitespace:
                break;
            case XmlNodeType.SignificantWhitespace:
                break;
            case XmlNodeType.EndElement:
                break;
            case XmlNodeType.EndEntity:
                break;
            case XmlNodeType.XmlDeclaration:
                break;
            default:
                break;
            }

            return false;
        }

        public static void DetachAt(this XmlNodeList list, int index) {
            var node = list[index];
            Detach(node);
        }

        public static void DetachAll(this XmlNodeList list) {
            var length = list.Count;
            for (int i = 0; i < length; i++) {
                var node = list[i];
                Detach(node);
            }
        }
    }
}
