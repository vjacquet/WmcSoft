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
using System.Linq;
using System.Xml.XPath;

namespace WmcSoft.Xml.XPath
{
    public sealed class RulesXPathNodeIterator : FilterXPathNodeIterator
    {
        static readonly char[] Delimiters = new char[] { ' ' };

        RulesXPathNodeIterator(XPathNodeIterator iterator, Predicate<XPathNavigator> predicate)
            : base(iterator, predicate)
        {
        }

        public RulesXPathNodeIterator(XPathNodeIterator iterator, XPathNavigator context, IDictionary<string, XPathExpression> rules)
            : base(iterator, BuildPredicate(context, rules))
        {
        }

        private static Predicate<XPathNavigator> BuildPredicate(XPathNavigator context, IDictionary<string, XPathExpression> rules)
        {
            return delegate (XPathNavigator navigator) {
                XPathNavigator clone = navigator.Clone();
                if (!clone.MoveToFirstAttribute())
                    return false;

                bool triedSomeRules = false;
                while (true) {
                    string name = clone.LocalName;
                    if (rules.TryGetValue(name, out XPathExpression expression)) {
                        string value = clone.Value;
                        if (!String.IsNullOrEmpty(value)) {
                            var tokens = value.Split(Delimiters);
                            triedSomeRules = true;
                            bool found = false;
                            switch (expression.ReturnType) {
                            case XPathResultType.Number:
                            case XPathResultType.String:
                            case XPathResultType.Boolean:
                                value = (context.Evaluate(expression) ?? "").ToString();
                                found = tokens.Any(t => t == value);
                                break;
                            case XPathResultType.NodeSet: {
                                    var iterator = (XPathNodeIterator)context.Evaluate(expression);
                                    while (iterator.MoveNext() && !found) {
                                        value = iterator.Current.Value;
                                        found = tokens.Any((string t) => t == value);
                                    }
                                    break;
                                }
                            }
                            if (!found) {
                                return false;
                            }
                        }
                    }
                    if (!clone.MoveToNextAttribute())
                        return triedSomeRules;
                }
            };
        }

        public override XPathNodeIterator Clone()
        {
            return new RulesXPathNodeIterator(_iterator, _predicate) {
                _position = _position
            };
        }
    }
}
