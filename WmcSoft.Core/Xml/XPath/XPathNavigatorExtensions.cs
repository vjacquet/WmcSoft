#region Licence

/****************************************************************************
          Copyright 1999-2019 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Xml.XPath
{
    public static class XPathNavigatorExtensions
    {
        public static string SelectSingleNodeValue(this XPathNavigator self, string xpath)
        {
            var result = self.SelectSingleNode(xpath);
            if (result != null)
                return result.Value;
            return null;
        }

        public static string SelectSingleNodeValue(this XPathNavigator self, string xpath, IXmlNamespaceResolver nsResolver)
        {
            var result = self.SelectSingleNode(xpath, nsResolver);
            if (result != null)
                return result.Value;
            return null;
        }

        public static string SelectSingleNodeValue(this XPathNavigator self, XPathExpression expression)
        {
            var result = self.SelectSingleNode(expression);
            if (result != null)
                return result.Value;
            return null;
        }

        public static T SelectSingleNodeValueAs<T>(this XPathNavigator self, string xpath)
        {
            var result = self.SelectSingleNode(xpath);
            if (result != null)
                return result.ValueAs<T>();
            return default;
        }

        public static T ValueAs<T>(this XPathNavigator self)
        {
            return (T)self.ValueAs(typeof(T));
        }

        public static T ValueAs<T>(this XPathNavigator self, IXmlNamespaceResolver nsResolver)
        {
            return (T)self.ValueAs(typeof(T), nsResolver);
        }
    }
}
