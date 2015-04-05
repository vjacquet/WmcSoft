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
using System.Xml.XPath;

namespace WmcSoft.Xml.XPath
{
    public class FilterXPathNodeIterator : XPathNodeIterator
    {
        protected XPathNodeIterator _iterator;
        protected int _position;
        protected Predicate<XPathNavigator> _predicate;

        public FilterXPathNodeIterator(XPathNodeIterator iterator, Predicate<XPathNavigator> predicate) {
            _iterator = iterator.Clone();
            _position = 0;
            _predicate = predicate;
        }

        public override XPathNavigator Current {
            get { return _iterator.Current; }
        }

        public override int CurrentPosition {
            get { return _position; }
        }

        public override XPathNodeIterator Clone() {
            return new FilterXPathNodeIterator(_iterator, (Predicate<XPathNavigator>)_predicate.Clone()) {
                _position = _position
            };
        }

        public override bool MoveNext() {
            while (_iterator.MoveNext()) {
                if (_predicate(_iterator.Current)) {
                    _position++;
                    return true;
                }
            }
            return false;
        }
    }
}
