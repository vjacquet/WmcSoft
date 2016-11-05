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
    public class XsltContextFunction : IXsltContextFunction
    {
        private readonly XPathResultType[] _argTypes;
        private readonly XPathResultType _returnType;
        private readonly int _maxargs;
        private readonly int _minargs;
        private readonly Func<XsltContext, object[], XPathNavigator, object> _invoke;

        public XsltContextFunction(Func<XsltContext, object[], XPathNavigator, object> invoke, XPathResultType[] argTypes, int minargs, int maxargs, XPathResultType returnType) {
            _invoke = invoke;
            _argTypes = argTypes;
            _minargs = minargs;
            _maxargs = maxargs;
            _returnType = returnType;
        }

        public XsltContextFunction(Func<XsltContext, object[], XPathNavigator, object> invoke, XPathResultType[] argTypes, XPathResultType returnType)
            : this(invoke, argTypes, argTypes.Length, argTypes.Length, returnType) {
        }

        public XsltContextFunction(Func<object[], object> invoke, XPathResultType[] argTypes, XPathResultType returnType)
            : this((c, args, n) => invoke(args), argTypes, argTypes.Length, argTypes.Length, returnType) {
        }

        #region IXsltContextFunction Members

        public XPathResultType[] ArgTypes {
            get { return _argTypes; }
        }

        public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext) {
            return _invoke(xsltContext, args, docContext);
        }

        public int Maxargs {
            get { return _maxargs; }
        }

        public int Minargs {
            get { return _minargs; }
        }

        public XPathResultType ReturnType {
            get { return _returnType; }
        }

        #endregion
    }
}
