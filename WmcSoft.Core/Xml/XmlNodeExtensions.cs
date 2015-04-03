using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
    }
}
