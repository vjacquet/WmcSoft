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

        static internal NavigationAdapter CreateAdapter(Assembly assembly)
        {
            return CreateAdapter(assembly, null, 0);
        }

        static internal NavigationAdapter CreateAdapter(Assembly assembly, NavigationAdapter parent, int indexInParent)
        {
            return null;
        }

        static internal NavigationAdapter CreateAdapter(Type type)
        {
            return CreateAdapter(type, null, 0);
        }

        static internal NavigationAdapter CreateAdapter(Type type, NavigationAdapter parent, int indexInParent)
        {
            if (type.IsEnum)
                return new EnumAdapter(type, parent, indexInParent);
            else if (type.IsInterface)
                return new InterfaceAdapter(type, parent, indexInParent);
            else if (type.IsValueType)
                return new StructAdapter(type, parent, indexInParent);
            return new ClassAdapter(type, parent, indexInParent);
        }

        static internal NavigationAdapter CreateAdapter(MemberInfo memberInfo, NavigationAdapter parent, int indexInParent)
        {
            if (memberInfo is FieldInfo) {
                return new FieldAdapter((FieldInfo)memberInfo, parent, indexInParent);
            } else if (memberInfo is ConstructorInfo) {
                return new ConstructorAdapter((MethodBase)memberInfo, parent, indexInParent);
            } else if (memberInfo is MethodInfo) {
                var methodInfo = (MethodInfo)memberInfo;
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

        NavigationAdapter _adapter;
        XmlNameTable _nameTable;

        #endregion

        #region Lifecycle

        private ReflectNavigator(XmlNameTable nameTable)
        {
            _nameTable = nameTable;
        }

        public ReflectNavigator(Type type)
            : this(type, new NameTable())
        {
        }

        public ReflectNavigator(Type type, XmlNameTable nameTable)
        {
            // initialize nametable
            nameTable.Add(string.Empty);
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

            _nameTable = nameTable;

            // set the current adapter
            _adapter = new RootNavigationAdapter(CreateAdapter(type));
        }

        public override XPathNavigator Clone()
        {
            return new ReflectNavigator(_nameTable) {
                _adapter = (NavigationAdapter)_adapter.Clone()
            };
        }

        #endregion

        #region Global properties

        public override XmlNameTable NameTable {
            [DebuggerStepThrough]
            get => _nameTable;
        }

        public override string BaseURI {
            [DebuggerStepThrough]
            get => string.Empty;//this.nameTable.Get(baseURI.ToString());
        }

        #endregion

        #region Current state properties

        public override XPathNodeType NodeType {
            [DebuggerStepThrough]
            get {
                return _adapter.NodeType;
            }
        }

        public override string NamespaceURI {
            [DebuggerStepThrough]
            get => string.Empty;
        }

        public override string Prefix {
            [DebuggerStepThrough]
            get => string.Empty;
        }

        public override string Name {
            [DebuggerStepThrough]
            get => LocalName;
        }

        public override string LocalName {
            get {
                var nodeType = NodeType;
                switch (nodeType) {
                case XPathNodeType.Element:
                case XPathNodeType.Attribute:
                    return _adapter.GetLocalName(_nameTable);
                case XPathNodeType.Root:
                case XPathNodeType.Namespace:
                case XPathNodeType.ProcessingInstruction:
                default:
                    return string.Empty;
                }
            }
        }

        public override string Value {
            get {
                var nodeType = NodeType;
                switch (nodeType) {
                case XPathNodeType.Attribute:
                    return _adapter.GetValue(_nameTable);
                case XPathNodeType.Root:
                case XPathNodeType.Element:
                default:
                    return string.Empty;
                }
            }
        }

        public override bool IsEmptyElement {
            get {
                var nodeType = NodeType;
                if ((nodeType == XPathNodeType.Element)
                    || (nodeType == XPathNodeType.Root)) {
                    return (_adapter.IsEmptyElement());
                }
                return false;
            }
        }

        public override bool IsSamePosition(XPathNavigator other)
        {
            if (GetType() != other.GetType())
                return false;

            var navigator = (ReflectNavigator)other;
            return _adapter.Equals(navigator._adapter);
        }

        #endregion

        #region Move methods

        public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
        {
            return false;
        }

        public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
        {
            return false;
        }

        public override bool MoveToId(string id)
        {
            return false;
        }

        public override bool MoveTo(XPathNavigator other)
        {
            var that = other as ReflectNavigator;
            if (that == null)
                return false;
            _adapter = (NavigationAdapter)that._adapter.Clone();
            return true;
        }

        public override bool MoveToNext()
        {
            var nodeType = NodeType;
            if (nodeType == XPathNodeType.Root || nodeType == XPathNodeType.Attribute)
                return false;

            if (nodeType == XPathNodeType.Element) {
                var adapter = _adapter.GetNextSibbling();
                if (adapter != null) {
                    _adapter = adapter;
                    return true;
                }
            }

            return false;
        }

        public override bool MoveToPrevious()
        {
            var nodeType = NodeType;
            if (nodeType == XPathNodeType.Root
                || nodeType == XPathNodeType.Attribute)
                return false;

            if (nodeType == XPathNodeType.Element) {
                var adapter = _adapter.GetPreviousSibbling();
                if (adapter != null) {
                    _adapter = adapter;
                    return true;
                }
            }

            return false;
        }

        public override bool MoveToFirstChild()
        {
            var nodeType = NodeType;
            if (nodeType != XPathNodeType.Root && nodeType != XPathNodeType.Element)
                return false;

            var adapter = _adapter.GetChild(0);
            if (adapter != null) {
                _adapter = adapter;
                return true;
            }

            return false;
        }

        public override bool MoveToFirstAttribute()
        {
            var nodeType = NodeType;
            if (nodeType != XPathNodeType.Element)
                return false;

            return _adapter.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute()
        {
            var nodeType = NodeType;
            if (nodeType != XPathNodeType.Attribute
                && nodeType != XPathNodeType.Element)
                return false;

            return _adapter.MoveToNextAttribute();
        }

        public override bool MoveToParent()
        {
            var nodeType = NodeType;
            if (nodeType == XPathNodeType.Root) {
                return false;
            } else if (nodeType == XPathNodeType.Attribute) {
                return _adapter.MoveToElement();
            } else if (nodeType == XPathNodeType.Element) {
                var adapter = _adapter.GetParent();
                if (adapter != null) {
                    _adapter = adapter;
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
