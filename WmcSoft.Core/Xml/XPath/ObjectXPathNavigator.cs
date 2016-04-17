using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using System.Collections;
using System.Reflection;
using System.Globalization;

namespace WmcSoft.Xml.XPath
{
    public class ObjectXPathNavigator : XPathNavigator
    {
        #region class ObjectXPathProxy

        class ObjectXPathProxy
        {
            private static readonly object[] m_empty = new object[0];

            private object _binding;
            private string _name;
            private bool _activated = false;
            private Hashtable _attributes = null;
            private string[] _attributeKeys = null;
            private ArrayList _elements = null;
            private Hashtable _elemDict = null;
            private ObjectXPathProxy _parent = null;
            private readonly XmlNameTable _nt;

            public ObjectXPathProxy(object binding, XmlNameTable nt)
                : this(binding, GetName(binding), null, nt) {
            }

            private ObjectXPathProxy(object binding, string name, ObjectXPathProxy parent, XmlNameTable nt) {
                _binding = binding;
                _parent = parent;
                _nt = nt;
                _name = GetAtomicString(name);
            }

            static string GetName(object binding) {
                if (binding == null)
                    throw new ArgumentNullException("binding");
                return binding.GetType().Name;
            }

            public string Name {
                get { return _name; }
            }

            public ObjectXPathProxy Parent {
                get { return _parent; }
            }

            public string Value {
                get {
                    if (HasText) {
                        return CultureSafeToString(_binding);
                    }

                    return string.Empty;
                }
            }

            public bool HasAttributes {
                get {
                    Activate();

                    return (_attributes != null);
                }
            }

            public bool HasChildren {
                get {
                    Activate();

                    return (_elements != null) || HasText;
                }
            }

            public bool HasText {
                get {
                    var t = _binding.GetType();
                    return t.IsValueType || t == typeof(string);
                }
            }

            public IList AttributeKeys {
                get {
                    Activate();

                    if (_attributeKeys != null)
                        return _attributeKeys;
                    return m_empty;
                }
            }

            public string GetAttributeValue(string name) {
                string v = null;

                Activate();

                if (_attributes != null) {
                    v = (string)_attributes[name];
                }

                return (v != null) ? v : string.Empty;
            }

            public IList Elements {
                get {
                    Activate();

                    if (_elements != null) {
                        return _elements;
                    } else {
                        return m_empty;
                    }
                }
            }

            public IDictionary ElementDictionary {
                get {
                    Activate();

                    return _elemDict;
                }
            }

            public void AddSpecialName(string key, string val) {
                Activate();

                if (_attributes == null) {
                    _attributes = new Hashtable();
                }

                _attributes["*" + key] = val;

                _attributeKeys = new string[_attributes.Count];
                _attributes.Keys.CopyTo(_attributeKeys, 0);
            }

            private void Activate() {
                if (_activated)
                    return;

                lock (this) {
                    if (_activated)
                        return;

                    if (_binding is ValueType || _binding is string) {
                        // no attributes or children
                    } else if (_binding is IDictionary) {
                        ActivateDictionary();
                    } else if (_binding is ICollection) {
                        ActivateCollection();
                    } else {
                        ActivateSimple();
                    }

                    _activated = true;
                }
            }

            private void ActivateCollection() {
                var elements = new ArrayList();

                foreach (object val in (ICollection)_binding) {
                    if (val == null)
                        continue;
                    elements.Add(new ObjectXPathProxy(val, val.GetType().Name, this, _nt));
                }

                _elements = (elements.Count != 0) ? elements : null;
            }

            private void ActivateDictionary() {
                ArrayList elements = new ArrayList();

                _elemDict = new Hashtable();

                foreach (DictionaryEntry entry in (IDictionary)_binding) {
                    if (entry.Value == null) {
                        continue;
                    }

                    ObjectXPathProxy item = new ObjectXPathProxy(entry.Value, entry.Value.GetType().Name, this, _nt);

                    elements.Add(item);

                    item.AddSpecialName("key", entry.Key.ToString());

                    _elemDict[entry.Key.ToString()] = item;
                }

                _elements = (elements.Count != 0) ? elements : null;
            }

            private void ActivateSimple() {
                Hashtable attributes = new Hashtable();
                ArrayList elements = new ArrayList();

                foreach (PropertyInfo pi in _binding.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                    // get the value
                    object value = pi.GetValue(_binding, m_empty);

                    if (value == null) {
                        continue;
                    }

                    // get the custom attributes
                    object[] attrs = pi.GetCustomAttributes(true);
                    bool skip = false;

                    if (attrs != null) {
                        foreach (Attribute a in attrs) {
                            if (a is System.Xml.Serialization.XmlIgnoreAttribute) {
                                skip = true;
                                break;
                            }
                        }
                    }

                    if (skip) {
                        break;
                    }

                    // now handle the values
                    string str = CultureSafeToString(value);

                    if (str != null) {
                        attributes.Add(GetAtomicString(pi.Name), str);
                    } else {
                        elements.Add(new ObjectXPathProxy(value, pi.Name, this, _nt));
                    }
                }

                _attributes = (attributes.Count != 0) ? attributes : null;
                _elements = (elements.Count != 0) ? elements : null;

                if (_attributes != null) {
                    _attributeKeys = new string[_attributes.Count];
                    _attributes.Keys.CopyTo(_attributeKeys, 0);
                }
            }

            private string GetAtomicString(string v) {
                return _nt.Add(v);
            }

            private string CultureSafeToString(object obj) {
                if (obj is string) {
                    return (string)obj;
                }

                if (obj is ValueType) {
                    // handle DateTime
                    if (obj is DateTime) {
                        string fmt = "yyyy-MM-dd";
                        DateTime dt = (DateTime)obj;

                        if (dt.TimeOfDay.Ticks > 0) {
                            fmt += " HH:mm:ss";
                        }

                        return dt.ToString(fmt);
                    }

                    // specific handling for floating point types
                    if ((obj is decimal) || (obj is float) || (obj is double)) {
                        return ((IFormattable)obj).ToString(null, CultureInfo.InvariantCulture.NumberFormat);
                    }

                    // generic handling for all other value types
                    return obj.ToString();
                }

                // objects return null
                return null;
            }
        }

        #endregion

        #region Member fields

        private ObjectXPathProxy m_docElem = null;
        private ObjectXPathProxy m_currentElem = null;
        private XPathNodeType m_nodeType = XPathNodeType.Root;
        private IList m_values = null;
        private int m_valueIndex = -1;
        private XmlNameTable m_nt = new NameTable();

        #endregion

        #region Constructor
        public ObjectXPathNavigator(object obj) {
            m_docElem = new ObjectXPathProxy(obj, m_nt);

            //	m_docElem.AddSpecialName( "timestamp", DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ) );
            m_docElem.AddSpecialName("type", obj.GetType().FullName);
        }

        private ObjectXPathNavigator() {
        }
        #endregion

        #region XPathNavigator
        public override string BaseURI {
            // we don't expose a namespace right now
            get { return string.Empty; }
        }

        public override bool HasAttributes {
            get {
                switch (m_nodeType) {
                case XPathNodeType.Element:
                    // does the element have attributes?
                    return m_currentElem.HasAttributes;
                default:
                    // nothing has attributes except elements
                    return false;
                }
            }
        }

        public override bool HasChildren {
            get {
                switch (m_nodeType) {
                case XPathNodeType.Element:
                    // does the element have children?
                    return m_currentElem.HasChildren;
                case XPathNodeType.Root:
                    // the root always has at least one child
                    // (for the object the navigator was built from)
                    return true;
                default:
                    // nothing else has children
                    return false;
                }
            }
        }

        public override bool IsEmptyElement {
            get {
                // we are empty if we don't have children
                return !HasChildren;
            }
        }

        public override string LocalName {
            // we don't use namespaces, so our Name and LocalName are the same
            get { return Name; }
        }

        public override string Name {
            get {
                switch (m_nodeType) {
                case XPathNodeType.Element:
                    return m_currentElem.Name;
                case XPathNodeType.Attribute:
                    if (m_valueIndex >= 0 && m_valueIndex < m_values.Count) {
                        string s = (string)m_values[m_valueIndex];
                        if (s[0] == '*')
                            s = s.Substring(1);
                        return s;
                    }
                    break;
                }
                return string.Empty;
            }
        }

        public override string NamespaceURI {
            get {
                switch (m_nodeType) {
                case XPathNodeType.Attribute:
                    if (m_valueIndex >= 0 && m_valueIndex < m_values.Count) {
                        string s = (string)m_values[m_valueIndex];
                        if (s[0] == '*')
                            return "urn:ObjectXPathNavigator";
                    }
                    break;
                }
                return string.Empty;
            }
        }

        public override XPathNodeType NodeType {
            get { return m_nodeType; }
        }

        public override XmlNameTable NameTable {
            get { return m_nt; }
        }

        public override string Prefix {
            get {
                switch (m_nodeType) {
                case XPathNodeType.Attribute:
                    if (m_valueIndex >= 0 && m_valueIndex < m_values.Count) {
                        string s = (string)m_values[m_valueIndex];
                        if (s[0] == '*') {
                            return "oxp";
                        }
                    }
                    break;
                }
                return string.Empty;
            }
        }

        public override string Value {
            get {
                switch (m_nodeType) {
                case XPathNodeType.Element:
                    return m_currentElem.Value;
                case XPathNodeType.Attribute:
                    if (m_valueIndex >= 0 && m_valueIndex < m_values.Count) {
                        return m_currentElem.GetAttributeValue((string)m_values[m_valueIndex]);
                    }
                    break;
                case XPathNodeType.Text:
                    goto case XPathNodeType.Element;
                }
                return string.Empty;
            }
        }

        public override string XmlLang {
            get { return string.Empty; }
        }

        public override XPathNavigator Clone() {
            ObjectXPathNavigator newNav = new ObjectXPathNavigator();

            newNav.m_docElem = m_docElem;
            newNav.m_currentElem = m_currentElem;
            newNav.m_nodeType = m_nodeType;
            newNav.m_values = m_values;
            newNav.m_valueIndex = m_valueIndex;
            newNav.m_nt = m_nt;

            return newNav;
        }

        public override string GetAttribute(string localName, string namespaceURI) {
            if (m_nodeType == XPathNodeType.Element) {
                if (namespaceURI.Length == 0) {
                    return m_currentElem.GetAttributeValue(localName);
                }
            }
            return string.Empty;
        }

        public override string GetNamespace(string localName) {
            return string.Empty;
        }

        public override bool IsDescendant(XPathNavigator nav) {
            var otherNav = nav as ObjectXPathNavigator;
            if (otherNav != null) {
                // if they're in different graphs, they're not the same
                if (m_docElem != otherNav.m_docElem) {
                    return false;
                }

                if (m_nodeType == XPathNodeType.Root && otherNav.m_nodeType != XPathNodeType.Root) {
                    // its on my root element - its still a descendant
                    return true;
                }

                // if I'm not on an element, it can't be my descendant
                // (attributes and text don't have descendants)
                if (m_nodeType != XPathNodeType.Element) {
                    return false;
                }

                if (this.m_currentElem == otherNav.m_currentElem) {
                    // if its on my attribute or content - its still a descendant
                    return (m_nodeType == XPathNodeType.Element && otherNav.m_nodeType != XPathNodeType.Element);
                }

                // ok, we need to hunt...
                for (ObjectXPathProxy parent = otherNav.m_currentElem.Parent; parent != null; parent = parent.Parent) {
                    if (parent == this.m_currentElem) {
                        return true;
                    }
                }
            }

            return false;
        }

        public override bool IsSamePosition(XPathNavigator other) {
            if (other is ObjectXPathNavigator) {
                ObjectXPathNavigator otherNav = (ObjectXPathNavigator)other;

                // if they're in different graphs, they're not the same
                if (this.m_docElem != otherNav.m_docElem) {
                    return false;
                }

                // if they're different node types, they can't be the same node!
                if (this.m_nodeType != otherNav.m_nodeType) {
                    return false;
                }

                // if they're different elements, they can't be the same node!
                if (this.m_currentElem != otherNav.m_currentElem) {
                    return false;
                }

                // are they on different attributes?
                if (this.m_nodeType == XPathNodeType.Attribute && this.m_valueIndex != otherNav.m_valueIndex) {
                    return false;
                }

                return true;
            }
            return false;
        }

        public override bool MoveTo(XPathNavigator other) {
            if (other is ObjectXPathNavigator) {
                ObjectXPathNavigator otherNav = (ObjectXPathNavigator)other;

                m_docElem = otherNav.m_docElem;
                m_currentElem = otherNav.m_currentElem;
                m_nodeType = otherNav.m_nodeType;
                m_values = otherNav.m_values;
                m_valueIndex = otherNav.m_valueIndex;
                m_nt = otherNav.m_nt;

                return true;
            }

            return false;
        }

        public override bool MoveToAttribute(string localName, string namespaceURI) {
            int pos = 0;

            if (m_nodeType != XPathNodeType.Element) {
                return false;
            }

            foreach (string name in m_currentElem.AttributeKeys) {
                if ((string)name == localName) {
                    m_nodeType = XPathNodeType.Attribute;
                    m_valueIndex = pos;

                    return true;
                }

                pos++;
            }

            return false;
        }

        public override bool MoveToFirst() {
            switch (m_nodeType) {
            case XPathNodeType.Element: {
                    m_valueIndex = 0;
                    return true;
                }

            case XPathNodeType.Attribute: {
                    m_valueIndex = 0;
                    return true;
                }

            case XPathNodeType.Text: {
                    return true;
                }
            }

            return false;
        }

        public override bool MoveToFirstAttribute() {
            if (m_nodeType != XPathNodeType.Element) {
                return false;
            }

            m_values = m_currentElem.AttributeKeys;
            m_valueIndex = 0;
            m_nodeType = XPathNodeType.Attribute;

            return true;
        }

        public override bool MoveToFirstChild() {
            if (m_nodeType == XPathNodeType.Root) {
                // move to the document element
                this.MoveNavigator(m_docElem);
                return true;
            }

            if (m_nodeType != XPathNodeType.Element) {
                return false;
            }

            // drop down to the text value (if any)
            if (m_currentElem.HasText) {
                m_nodeType = XPathNodeType.Text;
                m_values = null;
                m_valueIndex = -1;

                return true;
            }

            // drop down to the first element (if any)
            IList coll = m_currentElem.Elements;

            if (coll.Count > 0) {
                MoveNavigator((ObjectXPathProxy)coll[0]);

                return true;
            }

            return false;
        }

        public override bool MoveToFirstNamespace(XPathNamespaceScope scope) {
            return false;
        }

        public override bool MoveToId(string id) {
            return false;
        }

        public override bool MoveToNamespace(string name) {
            return false;
        }

        public override bool MoveToNext() {
            if (m_nodeType != XPathNodeType.Element) {
                return false;
            }

            ObjectXPathProxy parent = m_currentElem.Parent;

            if (parent == null) {
                return false;
            }

            bool foundIt = false;

            foreach (ObjectXPathProxy sib in parent.Elements) {
                if (foundIt) {
                    MoveNavigator(sib);

                    return true;
                }

                if (m_currentElem == sib) {
                    foundIt = true;
                }
            }

            return false;
        }

        public override bool MoveToNextAttribute() {
            if (m_nodeType != XPathNodeType.Attribute) {
                return false;
            }

            if (m_valueIndex + 1 >= m_values.Count) {
                return false;
            }

            m_valueIndex++;

            return true;
        }

        public override bool MoveToNextNamespace(XPathNamespaceScope scope) {
            return false;
        }

        public override bool MoveToParent() {
            if (m_nodeType == XPathNodeType.Root) {
                return false;
            }

            if (m_nodeType != XPathNodeType.Element) {
                m_nodeType = XPathNodeType.Element;

                return true;
            }

            ObjectXPathProxy parent = m_currentElem.Parent;

            if (parent == null) {
                return false;
            }

            MoveNavigator(parent);
            return true;
        }

        public override bool MoveToPrevious() {
            if (m_nodeType != XPathNodeType.Element) {
                return false;
            }

            ObjectXPathProxy parent = m_currentElem.Parent;

            if (parent == null) {
                return false;
            }

            ObjectXPathProxy previous = null;

            foreach (ObjectXPathProxy sib in parent.Elements) {
                if (sib == m_currentElem) {
                    if (previous == null) {
                        break;
                    }

                    MoveNavigator(previous);

                    return true;
                }

                previous = sib;
            }

            return false;
        }

        public override void MoveToRoot() {
            m_nodeType = XPathNodeType.Root;
            m_currentElem = null;
            m_values = null;
            m_valueIndex = -1;
        }
        #endregion

        #region Private methods
        // ---------------------------------------------------------------------
        // private methods
        // ---------------------------------------------------------------------

        private void MoveNavigator(ObjectXPathProxy nav) {
            m_nodeType = XPathNodeType.Element;
            m_currentElem = nav;
            m_values = nav.Elements;
            m_valueIndex = -1;
        }

        #endregion
    }
}
