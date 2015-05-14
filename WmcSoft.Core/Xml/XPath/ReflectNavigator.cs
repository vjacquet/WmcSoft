using System;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using WmcSoft.Xml.XPath.Internal;

namespace WmcSoft.Xml.XPath
{
    public class ReflectNavigator : XPathNavigator
    {
        #region Factory methods

        static internal NavigationAdapter CreateAdapter(Assembly assembly) {
            return CreateAdapter(assembly, null, 0);
        }

        static internal NavigationAdapter CreateAdapter(Assembly assembly, NavigationAdapter parent, int indexInParent) {
            return null;
        }

        static internal NavigationAdapter CreateAdapter(Type type) {
            return CreateAdapter(type, null, 0);
        }

        static internal NavigationAdapter CreateAdapter(Type type, NavigationAdapter parent, int indexInParent) {
            if (type.IsEnum)
                return new EnumAdapter(type, parent, indexInParent);
            else if (type.IsInterface)
                return new InterfaceAdapter(type, parent, indexInParent);
            else if (type.IsValueType)
                return new StructAdapter(type, parent, indexInParent);
            return new ClassAdapter(type, parent, indexInParent);
        }

        static internal NavigationAdapter CreateAdapter(MemberInfo memberInfo, NavigationAdapter parent, int indexInParent) {
            if (memberInfo is FieldInfo) {
                return new FieldAdapter((FieldInfo)memberInfo, parent, indexInParent);
            } else if (memberInfo is ConstructorInfo) {
                return new ConstructorAdapter((MethodBase)memberInfo, parent, indexInParent);
            } else if (memberInfo is MethodInfo) {
                MethodInfo methodInfo = (MethodInfo)memberInfo;
                if (!methodInfo.IsConstructor
                    && !methodInfo.IsSpecialName) {
                    return new MethodAdapter((MethodBase)memberInfo, parent, indexInParent);
                }
            } else if (memberInfo is PropertyInfo) {
                return new PropertyAdapter((PropertyInfo)memberInfo, parent, indexInParent);
            } else if (memberInfo is EventInfo) {
                return new EventAdapter((EventInfo)memberInfo, parent, indexInParent);
            }
            return null;
        }

        #endregion

        #region Private fields

        NavigationAdapter adapter;
        XmlNameTable nameTable;

        #endregion

        #region Lifecycle

        private ReflectNavigator(XmlNameTable nameTable) {
            this.nameTable = nameTable;
        }

        public ReflectNavigator(Type type)
            : this(type, new NameTable()) {
        }

        public ReflectNavigator(Type type, XmlNameTable nameTable) {
            // initialize nametable
            nameTable.Add(String.Empty);
            nameTable.Add("access");
            nameTable.Add("add");
            nameTable.Add("assembly");
            nameTable.Add("class");
            nameTable.Add("constructor");
            nameTable.Add("entry");
            nameTable.Add("enum");
            nameTable.Add("event");
            nameTable.Add("false");
            nameTable.Add("field");
            nameTable.Add("fullName");
            nameTable.Add("get");
            nameTable.Add("implements");
            nameTable.Add("inherits");
            nameTable.Add("interface");
            nameTable.Add("internal");
            nameTable.Add("isAbstract");
            nameTable.Add("isOverride");
            nameTable.Add("isReadOnly");
            nameTable.Add("isSealed");
            nameTable.Add("isStatic");
            nameTable.Add("isVirtual");
            nameTable.Add("metadataToken");
            nameTable.Add("method");
            nameTable.Add("module");
            nameTable.Add("name");
            nameTable.Add("parameter");
            nameTable.Add("property");
            nameTable.Add("protected");
            nameTable.Add("protected internal");
            nameTable.Add("private");
            nameTable.Add("public");
            nameTable.Add("remove");
            nameTable.Add("set");
            nameTable.Add("struct");
            nameTable.Add("true");
            nameTable.Add("type");

            this.nameTable = nameTable;

            // set the current adapter
            this.adapter = new RootNavigationAdapter(CreateAdapter(type));
        }

        public override XPathNavigator Clone() {
            ReflectNavigator clone = new ReflectNavigator(nameTable);
            clone.adapter = (NavigationAdapter)this.adapter.Clone();
            return clone;
        }

        #endregion

        #region Global properties

        public override XmlNameTable NameTable {
            [DebuggerStepThrough]
            get {
                return this.nameTable;
            }
        }

        public override string BaseURI {
            [DebuggerStepThrough]
            get {
                return String.Empty;//this.nameTable.Get(baseURI.ToString());
            }
        }

        #endregion

        #region Current state properties

        public override XPathNodeType NodeType {
            [DebuggerStepThrough]
            get {
                return adapter.NodeType;
            }
        }

        public override string NamespaceURI {
            [DebuggerStepThrough]
            get {
                return String.Empty;
            }
        }

        public override string Prefix {
            [DebuggerStepThrough]
            get {
                return String.Empty;
            }
        }

        public override string Name {
            [DebuggerStepThrough]
            get {
                return this.LocalName;
            }
        }

        public override string LocalName {
            get {
                XPathNodeType nodeType = this.NodeType;
                switch (nodeType) {
                case XPathNodeType.Element:
                case XPathNodeType.Attribute:
                    return adapter.GetLocalName(this.nameTable);
                case XPathNodeType.Root:
                case XPathNodeType.Namespace:
                case XPathNodeType.ProcessingInstruction:
                default:
                    return String.Empty;
                }
            }
        }

        public override string Value {
            get {
                XPathNodeType nodeType = this.NodeType;
                switch (nodeType) {
                case XPathNodeType.Attribute:
                    return adapter.GetValue(this.nameTable);
                case XPathNodeType.Root:
                case XPathNodeType.Element:
                default:
                    return String.Empty;
                }
            }
        }

        public override bool IsEmptyElement {
            get {
                XPathNodeType nodeType = this.NodeType;
                if ((nodeType == XPathNodeType.Element)
                    || (nodeType == XPathNodeType.Root)) {
                    return (this.adapter.IsEmptyElement());
                }
                return false;
            }
        }

        public override bool IsSamePosition(XPathNavigator other) {
            if (this.GetType() != other.GetType())
                return false;

            ReflectNavigator navigator = (ReflectNavigator)other;
            return this.adapter.Equals(navigator.adapter);
        }

        #endregion

        #region Move methods

        public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope) {
            return false;
        }

        public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope) {
            return false;
        }

        public override bool MoveToId(string id) {
            return false;
        }

        public override bool MoveTo(XPathNavigator other) {
            ReflectNavigator that = other as ReflectNavigator;
            if (that == null)
                return false;
            this.adapter = (NavigationAdapter)that.adapter.Clone();
            return true;
        }

        public override bool MoveToNext() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType == XPathNodeType.Root
                || nodeType == XPathNodeType.Attribute)
                return false;

            if (nodeType == XPathNodeType.Element) {
                NavigationAdapter adapter = this.adapter.GetNextSibbling();
                if (adapter != null) {
                    this.adapter = adapter;
                    return true;
                }
            }

            return false;
        }

        public override bool MoveToPrevious() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType == XPathNodeType.Root
                || nodeType == XPathNodeType.Attribute)
                return false;

            if (nodeType == XPathNodeType.Element) {
                NavigationAdapter adapter = this.adapter.GetPreviousSibbling();
                if (adapter != null) {
                    this.adapter = adapter;
                    return true;
                }
            }

            return false;
        }

        public override bool MoveToFirstChild() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType != XPathNodeType.Root && nodeType != XPathNodeType.Element)
                return false;

            NavigationAdapter adapter = this.adapter.GetChild(0);
            if (adapter != null) {
                this.adapter = adapter;
                return true;
            }

            return false;
        }

        public override bool MoveToFirstAttribute() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType != XPathNodeType.Element)
                return false;

            return adapter.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType != XPathNodeType.Attribute
                && nodeType != XPathNodeType.Element)
                return false;

            return adapter.MoveToNextAttribute();
        }

        public override bool MoveToParent() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType == XPathNodeType.Root) {
                return false;
            } else if (nodeType == XPathNodeType.Attribute) {
                return adapter.MoveToElement();
            } else if (nodeType == XPathNodeType.Element) {
                NavigationAdapter adapter = this.adapter.GetParent();
                if (adapter != null) {
                    this.adapter = adapter;
                    return true;
                }
            }
            return false;
        }

        #endregion

    }
}
