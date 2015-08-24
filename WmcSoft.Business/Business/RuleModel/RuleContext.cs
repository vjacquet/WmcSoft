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

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using WmcSoft.Runtime.Serialization;

namespace WmcSoft.Business.RuleModel
{
    /// <summary>
    /// Contains the informational context for the execution of a <see cref="Rule"/>. 
    /// It represents this information as a collection of a <see cref="RuleEment"/> that may
    /// be <see cref="Proposition"/> or <see cref="Variable"/>, but not <see cref="Operator"/>.
    /// </summary>
    [XmlRootAttribute("ruleContext", Namespace = @"http://www.wmcsoft.fr/schemas/2015/business/RuleModel.xsd", IsNullable = false)]
    public class RuleContext
    {
        private Dictionary<string, RuleElement> _dictionary = new Dictionary<string, RuleElement>();

        /// <remarks/>
        [XmlElement("ruleOverride", typeof(RuleOverride))]
        [XmlElement("proposition", typeof(Proposition))]
        [XmlElement("variable", typeof(Variable))]
        public RuleElement[] Items
        {
            get
            {
                if (_items == null)
                {
                    _items = _dictionary.Values.ToArray();
                }
                return _items;
            }
            set
            {
                var dictionary = new Dictionary<string, RuleElement>(value.Length);
                foreach (RuleElement element in value)
                {
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

        public void Add(string name, RuleElement value)
        {
            _dictionary.Add(name, value);
            _items = null;
        }

        public bool Contains(string name)
        {
            return _dictionary.ContainsKey(name);
        }

        public bool Remove(string name)
        {
            if (_dictionary.Remove(name))
            {
                _items = null;
                return true;
            }
            return false;
        }

        public RuleElement this[string name]
        {
            get
            {
                RuleElement value;
                _dictionary.TryGetValue(name, out value);
                return value;
            }
            set
            {
                _dictionary[name] = value;
                _items = null;
            }
        }

        public void Clear()
        {
            _dictionary.Clear();
            _items = null;
        }

        [XmlIgnore]
        public int Count
        {
            get { return _dictionary.Count; }
        }

        #endregion
    }
}
