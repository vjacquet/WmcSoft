#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
using System.Xml.XPath;
using System.Xml.Xsl;

namespace WmcSoft.Xml.Xsl
{
    public class XsltContextVariable : IXsltContextVariable
    {
        private readonly XPathResultType variableType;
        private readonly Func<XsltContext, object> evaluate;
        private readonly bool isParam;
        private readonly bool isLocal;

        public XsltContextVariable(Func<XsltContext, long> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(c), XPathResultType.Number, isParam, isLocal) {
        }
        public XsltContextVariable(Func<XsltContext, int> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(c), XPathResultType.Number, isParam, isLocal) {
        }
        public XsltContextVariable(Func<XsltContext, short> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(c), XPathResultType.Number, isParam, isLocal) {
        }
        public XsltContextVariable(Func<XsltContext, byte> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(c), XPathResultType.Number, isParam, isLocal) {
        }
        public XsltContextVariable(Func<XsltContext, double> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(c), XPathResultType.Number, isParam, isLocal) {
        }
        public XsltContextVariable(Func<XsltContext, float> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(c), XPathResultType.Number, isParam, isLocal) {
        }
        public XsltContextVariable(Func<XsltContext, decimal> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(c), XPathResultType.Number, isParam, isLocal) {
        }
        public XsltContextVariable(Func<XsltContext, string> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(c), XPathResultType.String, isParam, isLocal) {
        }
        public XsltContextVariable(Func<XsltContext, bool> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(c), XPathResultType.Boolean, isParam, isLocal) {
        }
        public XsltContextVariable(Func<XsltContext, XPathNodeIterator> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(c), XPathResultType.NodeSet, isParam, isLocal) {
        }

        public XsltContextVariable(Func<long> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(), XPathResultType.Number, isParam, isLocal) {
        }
        public XsltContextVariable(Func<int> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(), XPathResultType.Number, isParam, isLocal) {
        }
        public XsltContextVariable(Func<short> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(), XPathResultType.Number, isParam, isLocal) {
        }
        public XsltContextVariable(Func<byte> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(), XPathResultType.Number, isParam, isLocal) {
        }
        public XsltContextVariable(Func<double> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(), XPathResultType.Number, isParam, isLocal) {
        }
        public XsltContextVariable(Func<float> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(), XPathResultType.Number, isParam, isLocal) {
        }
        public XsltContextVariable(Func<decimal> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(), XPathResultType.Number, isParam, isLocal) {
        }
        public XsltContextVariable(Func<string> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(), XPathResultType.String, isParam, isLocal) {
        }
        public XsltContextVariable(Func<bool> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(), XPathResultType.Boolean, isParam, isLocal) {
        }
        public XsltContextVariable(Func<XPathNodeIterator> evaluate, bool isParam = false, bool isLocal = true)
            : this((c) => evaluate(), XPathResultType.NodeSet, isParam, isLocal) {
        }

        public XsltContextVariable(Func<XsltContext, object> evaluate, XPathResultType variableType, bool isParam = false, bool isLocal = true) {
            this.variableType = variableType;
            this.evaluate = evaluate;
            this.isParam = isParam;
            this.isLocal = isLocal;
        }

        #region IXsltContextVariable Members

        public object Evaluate(XsltContext xsltContext) {
            return evaluate(xsltContext);
        }

        public bool IsLocal {
            get { return isParam; }
        }

        public bool IsParam {
            get { return isLocal; }
        }

        public XPathResultType VariableType {
            get { return variableType; }
        }

        #endregion
    }
}
