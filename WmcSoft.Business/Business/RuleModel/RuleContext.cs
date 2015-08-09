using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace WmcSoft.Business.RuleModel
{
    /// <remarks/>
    [XmlRootAttribute("ruleContext", Namespace = "", IsNullable = false)]
    public class RuleContext
    {
        private Dictionary<string, RuleElement> _dictionary = new Dictionary<string, RuleElement>();

        /// <remarks/>
        [XmlElement("ruleOverride", typeof(RuleOverride))]
        [XmlElement("proposition", typeof(Proposition))]
        [XmlElement("variable", typeof(Variable))]
        public RuleElement[] Items {
            get {
                if (_items == null) {
                    _items = _dictionary.Values.ToArray();
                }
                return _items;
            }
            set {
                var dictionary = new Dictionary<string, RuleElement>(value.Length);
                foreach (RuleElement element in value) {
                    dictionary.Add(element.Name, element);
                }

                _dictionary = dictionary;
                _items = value;
            }
        }
        RuleElement[] _items;

        /// <remarks/>
        [XmlAttribute("version")]
        public string Version { get; set; }

        #region Members

        public void Add(string name, RuleElement value) {
            _dictionary.Add(name, value);
            _items = null;
        }

        public bool Contains(string name) {
            return _dictionary.ContainsKey(name);
        }

        public bool Remove(string name) {
            if (_dictionary.Remove(name)) {
                _items = null;
                return true;
            }
            return false;
        }

        public RuleElement this[string name] {
            get {
                RuleElement value;
                _dictionary.TryGetValue(name, out value);
                return value;
            }
            set {
                _dictionary[name] = value;
                _items = null;
            }
        }

        public void Clear() {
            _dictionary.Clear();
            _items = null;
        }

        [XmlIgnore]
        public int Count {
            get { return _dictionary.Count; }
        }

        #endregion
    }
}
