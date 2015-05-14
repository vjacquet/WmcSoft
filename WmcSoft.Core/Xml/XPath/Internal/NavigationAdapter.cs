using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Reflection;

namespace WmcSoft.Xml.XPath.Internal
{
    internal abstract class NavigationAdapter : ICloneable, IEquatable<NavigationAdapter>
    {
        #region Private fields

        protected int attributeIndex;
        protected int indexInParent;
        protected NavigationAdapter parent;

        #endregion

        #region Lifecycle

        protected NavigationAdapter(NavigationAdapter parent, int indexInParent) {
            this.parent = parent;
            this.indexInParent = indexInParent;
            this.attributeIndex = -1;
        }

        #endregion

        #region Current properties

        public virtual XPathNodeType NodeType {
            [DebuggerStepThrough]
            get {
                if (attributeIndex >= 0)
                    return XPathNodeType.Attribute;
                return XPathNodeType.Element;
            }
        }

        public abstract int GetAttributeCount();

        public virtual bool IsEmptyElement() {
            return GetChildCount() == 0;
        }

        public abstract int GetChildCount();

        public string GetLocalName(XmlNameTable nameTable) {
            if (attributeIndex < 0)
                return GetElementName(nameTable);
            return GetAttributeName(nameTable);
        }

        public virtual string GetAttributeName(XmlNameTable nameTable) {
            return String.Empty;
        }

        public virtual string GetElementName(XmlNameTable nameTable) {
            return String.Empty;
        }

        public abstract string GetValue(XmlNameTable nameTable);

        #endregion

        #region Move methods

        public bool MoveToElement() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType == XPathNodeType.Attribute) {
                nodeType = XPathNodeType.Element;
                attributeIndex = -1;
                return true;
            }
            return false;
        }

        public bool MoveToFirstAttribute() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType != XPathNodeType.Element)
                return false;

            if (GetAttributeCount() > 0) {
                nodeType = XPathNodeType.Attribute;
                attributeIndex = 0;
                return true;
            }

            return false;
        }

        public bool MoveToNextAttribute() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType != XPathNodeType.Attribute
                && nodeType != XPathNodeType.Element)
                return false;

            int attributeCount = GetAttributeCount();
            if (attributeIndex + 1 < attributeCount) {
                nodeType = XPathNodeType.Attribute;
                ++attributeIndex;
                return true;
            }

            return false;
        }

        [DebuggerStepThrough]
        public NavigationAdapter GetParent() {
            if (parent != null)
                return parent.Clone() as NavigationAdapter;
            return null;
        }

        public virtual NavigationAdapter GetChild(int index) {
            return null;
        }

        public NavigationAdapter GetNextSibbling() {
            if (parent == null)
                return null;
            return parent.GetChild(indexInParent + 1);
        }

        public NavigationAdapter GetPreviousSibbling() {
            if (parent == null)
                return null;
            return parent.GetChild(indexInParent - 1);
        }

        #endregion

        #region ICloneable Members

        public object Clone() {
            return MemberwiseClone();
        }

        #endregion

        #region IEquatable<NavigationAdapter> Members

        public override bool Equals(object obj) {
            return base.Equals((NavigationAdapter)obj);
        }
        public virtual bool Equals(NavigationAdapter other) {
            return (attributeIndex == other.attributeIndex)
                && (indexInParent == other.indexInParent);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        #endregion
    }
}