using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Reflection;

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