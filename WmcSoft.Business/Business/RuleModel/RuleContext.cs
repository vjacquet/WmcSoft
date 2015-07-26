using System;
using System.Collections;
using System.Xml.Serialization;

namespace WmcSoft.Business.RuleModel 
{
	/// <remarks/>
	[XmlRootAttribute("ruleContext", Namespace="", IsNullable=false)]
	public class RuleContext
	{
		private IDictionary dictionary = new Hashtable();

		/// <remarks/>
		[XmlElement("ruleOverride", typeof(RuleOverride))]
		[XmlElement("proposition", typeof(Proposition))]
		[XmlElement("variable", typeof(Variable))]
		public RuleElement[] Items
		{
			get
			{
				return this.items;
			}
			set
			{
				if(this.items != value)
				{
					IDictionary dictionary = new Hashtable(value.Length);
					foreach(RuleElement element in value)
					{
						dictionary.Add(element.Name, element);
					}

					this.dictionary = dictionary;
					this.items = value;
				}
			}
		} RuleElement[] items;

		/// <remarks/>
		[XmlAttribute("version")]
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		} string version;

		public RuleElement this[string name]
		{
			get
			{
				return (RuleElement)this.dictionary[name];
			}
		}

	}

}
