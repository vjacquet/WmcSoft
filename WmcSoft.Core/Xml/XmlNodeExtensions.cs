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
    }
}
