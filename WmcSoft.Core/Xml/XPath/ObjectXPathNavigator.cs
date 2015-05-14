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
            private object m_binding;
            private string m_name;
            private bool m_activated = false;
            private Hashtable m_attributes = null;
            private string[] m_attributeKeys = null;
            private ArrayList m_elements = null;
            private Hashtable m_elemDict = null;
            private ObjectXPathProxy m_parent = null;
            private XmlNameTable m_nt;
            private static object[] m_empty = new object[0];

            public ObjectXPathProxy(object binding, XmlNameTable nt) {
                m_binding = binding;
                m_nt = nt;
                m_name = GetAtomicString(binding.GetType().Name);
            }

            private ObjectXPathProxy(object binding, string name, ObjectXPathProxy parent, XmlNameTable nt) {
                m_binding = binding;
                m_parent = parent;
                m_nt = nt;
                m_name = GetAtomicString(name);
            }

            public string Name {
                get {
                    return m_name;
                }
            }

            public ObjectXPathProxy Parent {
                get {
                    return m_parent;
                }
            }

            public string Value {
                get {
                    if (HasText) {
                        return CultureSafeToString(m_binding);
                    }

                    return string.Empty;
                }
            }

            public bool HasAttributes {
                get {
                    Activate();

                    return (m_attributes != null);
                }
            }

            public bool HasChildren {
                get {
                    Activate();

                    return (m_elements != null) || HasText;
                }
            }

            public bool HasText {
                get {
                    Type t = m_binding.GetType();

                    return (t.IsValueType || t == typeof(string));
                }
            }

            public IList AttributeKeys {
                get {
                    Activate();

                    if (m_attributeKeys != null) {
                        return m_attributeKeys;
                    } else {
                        return m_empty;
                    }
                }
            }

            public string GetAttributeValue(string name) {
                string v = null;

                Activate();

                if (m_attributes != null) {
                    v = (string)m_attributes[name];
                }

                return (v != null) ? v : string.Empty;
            }

            public IList Elements {
                get {
                    Activate();

                    if (m_elements != null) {
                        return m_elements;
                    } else {
                        return m_empty;
                    }
                }
            }

            public IDictionary ElementDictionary {
                get {
                    Activate();

                    return m_elemDict;
                }
            }

            public void AddSpecialName(string key, string val) {
                Activate();

                if (m_attributes == null) {
                    m_attributes = new Hashtable();
                }

                m_attributes["*" + key] = val;

                m_attributeKeys = new string[m_attributes.Count];
                m_attributes.Keys.CopyTo(m_attributeKeys, 0);
            }

            private void Activate() {
                if (m_activated) {
                    return;
                }

                lock (this) {
                    if (m_activated) {
                        return;
                    }

                    if (m_binding is ValueType || m_binding is string) {
                        // no attributes or children
                    } else if (m_binding is IDictionary) {
                        ActivateDictionary();
                    } else if (m_binding is ICollection) {
                        ActivateCollection();
                    } else {
                        ActivateSimple();
                    }

                    m_activated = true;
                }
            }

            private void ActivateCollection() {
                ArrayList elements = new ArrayList();

                foreach (object val in (ICollection)m_binding) {
                    if (val == null) {
                        continue;
                    }

                    elements.Add(new ObjectXPathProxy(val, val.GetType().Name, this, m_nt));
                }

                m_elements = (elements.Count != 0) ? elements : null;
            }

            private void ActivateDictionary() {
                ArrayList elements = new ArrayList();

                m_elemDict = new Hashtable();

                foreach (DictionaryEntry entry in (IDictionary)m_binding) {
                    if (entry.Value == null) {
                        continue;
                    }

                    ObjectXPathProxy item = new ObjectXPathProxy(entry.Value, entry.Value.GetType().Name, this, m_nt);

                    elements.Add(item);

                    item.AddSpecialName("key", entry.Key.ToString());

                    m_elemDict[entry.Key.ToString()] = item;
                }

                m_elements = (elements.Count != 0) ? elements : null;
            }

            private void ActivateSimple() {
                Hashtable attributes = new Hashtable();
                ArrayList elements = new ArrayList();

                foreach (PropertyInfo pi in m_binding.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                    // get the value
                    object value = pi.GetValue(m_binding, m_empty);

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
                        elements.Add(new ObjectXPathProxy(value, pi.Name, this, m_nt));
                    }
                }

                m_attributes = (attributes.Count != 0) ? attributes : null;
                m_elements = (elements.Count != 0) ? elements : null;

                if (m_attributes != null) {
                    m_attributeKeys = new string[m_attributes.Count];
                    m_attributes.Keys.CopyTo(m_attributeKeys, 0);
                }
            }

            private string GetAtomicString(string v) {
                string s;
                s = m_nt.Get(v);
                if (s == null) {
                    s = m_nt.Add(v);
                }
                return s;
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
                    case XPathNodeType.Element: {
                            // does the element have attributes?
                            return m_currentElem.HasAttributes;
                        }

                    default: {
                            // nothing has attributes except elements
                            return false;
                        }
                }
            }
        }

        public override bool HasChildren {
            get {
                switch (m_nodeType) {
                    case XPathNodeType.Element: {
                            // does the element have children?
                            return m_currentElem.HasChildren;
                        }

                    case XPathNodeType.Root: {
                            // the root always has at least one child
                            // (for the object the navigator was built from)
                            return true;
                        }

                    default: {
                            // nothing else has children
                            return false;
                        }
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
                    case XPathNodeType.Element: {
                            return m_currentElem.Name;
                        }

                    case XPathNodeType.Attribute: {
                            if (m_valueIndex >= 0 && m_valueIndex < m_values.Count) {
                                string s = (string)m_values[m_valueIndex];

                                if (s[0] == '*') {
                                    s = s.Substring(1);
                                }

                                return s;
                            }

                            break;
                        }
                }

                return string.Empty;
            }
        }

        public override string NamespaceURI {
            get {
                switch (m_nodeType) {
                    case XPathNodeType.Attribute: {
                            if (m_valueIndex >= 0 && m_valueIndex < m_values.Count) {
                                string s = (string)m_values[m_valueIndex];

                                if (s[0] == '*') {
                                    return "urn:ObjectXPathNavigator";
                                }
                            }

                            break;
                        }
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
                    case XPathNodeType.Attribute: {
                            if (m_valueIndex >= 0 && m_valueIndex < m_values.Count) {
                                string s = (string)m_values[m_valueIndex];

                                if (s[0] == '*') {
                                    return "oxp";
                                }
                            }

                            break;
                        }
                }

                return string.Empty;
            }
        }

        public override string Value {
            get {
                switch (m_nodeType) {
                    case XPathNodeType.Element: {
                            return m_currentElem.Value;
                        }

                    case XPathNodeType.Attribute: {
                            if (m_valueIndex >= 0 && m_valueIndex < m_values.Count) {
                                return m_currentElem.GetAttributeValue((string)m_values[m_valueIndex]);
                            }
                            break;
                        }

                    case XPathNodeType.Text: {
                            goto case XPathNodeType.Element;
                        }
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
            if (nav is ObjectXPathNavigator) {
                ObjectXPathNavigator otherNav = (ObjectXPathNavigator)nav;

                // if they're in different graphs, they're not the same
                if (this.m_docElem != otherNav.m_docElem) {
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
