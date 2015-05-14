using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Reflection;

namespace WmcSoft.Xml.XPath.Internal
{

    internal abstract class ElementNavigationAdapter : NavigationAdapter
    {
        protected List<NavigationAdapter> childNodes;

        public ElementNavigationAdapter(NavigationAdapter parent, int indexInParent)
            : base(parent, indexInParent) {
            childNodes = new List<NavigationAdapter>();
        }

        public override int GetChildCount() {
            return childNodes.Count;
        }

        public override NavigationAdapter GetChild(int index) {
            if (index < 0 || index >= childNodes.Count)
                return null;
            return childNodes[index];
        }

    }

}