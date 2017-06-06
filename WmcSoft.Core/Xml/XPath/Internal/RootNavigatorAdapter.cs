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
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;

namespace WmcSoft.Xml.XPath.Internal
{
    internal class RootNavigationAdapter : NavigationAdapter
    {
        NavigationAdapter documentElement;

        public RootNavigationAdapter(NavigationAdapter documentElement)
            : base(null, 0) {
            this.documentElement = documentElement;
        }

        public override XPathNodeType NodeType {
            [DebuggerStepThrough]
            get {
                return XPathNodeType.Root;
            }
        }

        public override int GetAttributeCount() {
            return 0;
        }

        public override int GetChildCount() {
            return 1;
        }

        public override NavigationAdapter GetChild(int index) {
            if (index == 0)
                return documentElement;
            return null;
        }

        public override bool IsEmptyElement() {
            return false;
        }

        public override string GetValue(XmlNameTable nameTable) {
            return String.Empty;
        }
    }
}