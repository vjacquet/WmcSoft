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
using System.Xml.XPath;

namespace WmcSoft.Xml.XPath
{
    public class DistinctXPathNodeIterator : XPathNodeIterator
    {
        private XPathNodeIterator _iterator;
        private IDictionary<object, int> _occurences;
        private int _position;
        private XPathExpression _expression;

        public DistinctXPathNodeIterator(XPathNodeIterator iterator, XPathExpression expression)
        {
            _iterator = iterator.Clone();
            _occurences = new Dictionary<object, int>();
            _position = 0;
            _expression = expression;
        }
        public DistinctXPathNodeIterator(XPathNodeIterator iterator, string xpath)
            : this(iterator, XPathExpression.Compile($"string({xpath})"))
        {
        }
        public DistinctXPathNodeIterator(XPathNodeIterator iterator)
            : this(iterator, ".")
        {
        }

        public override XPathNavigator Current {
            get { return _iterator.Current; }
        }

        public override int CurrentPosition {
            get { return _position; }
        }

        public override XPathNodeIterator Clone()
        {
            return new DistinctXPathNodeIterator(_iterator) {
                _occurences = new Dictionary<object, int>(_occurences),
                _position = _position,
                _expression = _expression.Clone()
            };
        }

        public override bool MoveNext()
        {
            while (_iterator.MoveNext()) {
                object value = _iterator.Current.Evaluate(_expression, _iterator);
                int occurence;
                if (_occurences.TryGetValue(value, out occurence)) {
                    _occurences[value] = occurence + 1;
                } else {
                    _occurences.Add(value, 1);
                    _position++;
                    return true;
                }
            }
            return false;
        }
    }
}