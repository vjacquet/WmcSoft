using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace WmcSoft.Xml.XPath
{
    public sealed class RulesXPathNodeIterator : FilterXPathNodeIterator
    {
        static readonly char[] Delimiters = new char[] { ' ' };

        RulesXPathNodeIterator(XPathNodeIterator iterator, Predicate<XPathNavigator> predicate)
            : base(iterator, predicate) {
        }

        public RulesXPathNodeIterator(XPathNodeIterator iterator, XPathNavigator context, IDictionary<string, XPathExpression> rules)
            : base(iterator, RulesXPathNodeIterator.BuildPredicate(context, rules)) {
        }

        private static Predicate<XPathNavigator> BuildPredicate(XPathNavigator context, IDictionary<string, XPathExpression> rules) {
            return delegate(XPathNavigator navigator) {
                XPathNavigator clone = navigator.Clone();
                if (!clone.MoveToFirstAttribute())
                    return false;

                bool triedSomeRules = false;
                while (true) {
                    string name = clone.LocalName;
                    XPathExpression expression;
                    if (rules.TryGetValue(name, out expression)) {
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
                                    XPathNodeIterator iterator = (XPathNodeIterator)context.Evaluate(expression);
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

        public override XPathNodeIterator Clone() {
            return new RulesXPathNodeIterator(_iterator, _predicate) {
                _position = this._position
            };
        }
    }
}
