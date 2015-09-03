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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WmcSoft.Xml;
using WmcSoft.Runtime.Serialization;

namespace WmcSoft.Business.RuleModel
{
    public class RuleContextSerializer : XmlSerializer<RuleContext>
    {
        protected override RuleContext DoDeserialize(XmlReader reader) {
            var s = new RuleContextDeserializer(reader);
            return s.Deserialize();
        }

        protected override void DoSerialize(XmlWriter writer, RuleContext value) {
            const string xmlns = @"http://www.wmcsoft.fr/schemas/2015/business/RuleModel.xsd";
            writer.WriteStartElement("ruleContext", xmlns);
            writer.WriteAttributeString("version", value.Version);
            foreach (var item in value.Items) {
                var type = item.GetType();
                if (type == typeof(Variable)) {
                    var variable = (Variable)item;
                    writer.WriteStartElement("variable");
                    writer.WriteAttributeString("name", variable.Name);
                    writer.WriteAttributeString("value", variable.Value);
                    writer.WriteEndElement();
                } else if (type == typeof(Proposition)) {
                    var proposition = (Proposition)item;
                    writer.WriteStartElement("proposition");
                    writer.WriteAttributeString("name", proposition.Name);
                    writer.WriteAttributeValue("value", proposition.Value);
                    writer.WriteEndElement();
                } else {
                    throw new NotImplementedException();
                }
            }
            writer.WriteEndElement();
        }
    }

    class RuleContextDeserializer
    {
        class Names
        {
            public readonly string xmlns;
            public readonly string ruleContext;
            public readonly string version;
            public readonly string name;
            public readonly string value;

            public Names(XmlNameTable nt) {
                xmlns = nt.Add(@"http://www.wmcsoft.fr/schemas/2015/business/RuleModel.xsd");
                ruleContext = nt.Add("ruleContext");
                version = nt.Add("version");
                name = nt.Add("name");
                value = nt.Add("value");
            }
        }

        readonly XmlReader _reader;
        readonly Names N;

        public RuleContextDeserializer(XmlReader reader) {
            N = new Names(reader.NameTable);
            _reader = reader;
        }

        public RuleContext Deserialize() {
            if (_reader.MoveToContent() == XmlNodeType.Element && _reader.LocalName == N.ruleContext && _reader.NamespaceURI == N.xmlns) {
                string version = null;
                var mandatory = new BitArray(1);
                while (_reader.MoveToNextAttribute()) {
                    //if (reader.NamespaceURI != Xmlns)
                    //    continue;
                    if (!mandatory[0] && _reader.LocalName == "version") {
                        version = _reader.Value;
                        mandatory[0] = true;
                    }

                    //if (!array2[1] && base.Reader.LocalName == this.id69_version && base.Reader.NamespaceURI == this.id2_Item)
                    //{
                    //    ruleSet.Version = base.Reader.Value;
                    //    array2[1] = true;
                    //}
                    //else
                    //{
                    //    if (!array2[2] && base.Reader.LocalName == this.id66_name && base.Reader.NamespaceURI == this.id2_Item)
                    //    {
                    //        ruleSet.Name = base.CollapseWhitespace(base.Reader.Value);
                    //        array2[2] = true;
                    //    }
                    //    else
                    //    {
                    //        if (!base.IsXmlnsAttribute(base.Reader.Name))
                    //        {
                    //            base.UnknownNode(ruleSet, ":version, :name");
                    //        }
                    //    }
                    //}
                }

                var entity = new RuleContext { Version = version };
                _reader.MoveToElement();
                if (_reader.IsEmptyElement) {
                    _reader.Skip();
                    return entity;
                } else {
                    _reader.ReadStartElement();
                    _reader.MoveToContent();
                    var items = new List<RuleElement>();
                    while (_reader.NodeType != XmlNodeType.EndElement && _reader.NodeType != XmlNodeType.None) {
                        if (_reader.NodeType == XmlNodeType.Element && _reader.NamespaceURI == N.xmlns) {
                            if (_reader.LocalName == "variable") {
                                var ruleElement = Deserialize(_reader, (Variable)null);
                                items.Add(ruleElement);
                            } else if (_reader.LocalName == "proposition") {
                                var ruleElement = Deserialize(_reader, (Proposition)null);
                                items.Add(ruleElement);
                            } else if (_reader.LocalName == "ruleOverride") {
                                var ruleElement = Deserialize(_reader, (RuleOverride)null);
                                items.Add(ruleElement);
                            }

                        }
                        _reader.MoveToContent();
                    }
                    _reader.ReadEndElement();
                    entity.Items = items.ToArray();
                }
                return entity;
            }
            throw new InvalidOperationException();
        }

        Variable Deserialize(XmlReader reader, Variable prototype) {
            string name = null;
            string value = null;
            var mandatory = new BitArray(1);
            while (reader.MoveToNextAttribute()) {
                //if (reader.NamespaceURI != Xmlns)
                //    continue;
                if (!mandatory[0] && reader.LocalName == N.name) {
                    name = reader.Value;
                    mandatory[0] = true;
                } else if (reader.LocalName == N.value) {
                    value = reader.Value;
                }
            }
            reader.Skip();
            return new Variable { Name = name, Value = value };
        }

        Proposition Deserialize(XmlReader reader, Proposition prototype) {
            string name = null;
            bool value = false;
            var mandatory = new BitArray(1);
            while (reader.MoveToNextAttribute()) {
                //if (reader.NamespaceURI != Xmlns)
                //    continue;
                if (!mandatory[0] && reader.LocalName == N.name) {
                    name = reader.Value;
                    mandatory[0] = true;
                } else if (reader.LocalName == N.value) {
                    value = reader.ReadContentAsBoolean();
                }
            }
            reader.Skip();
            return new Proposition { Name = name, Value = value };
        }

    }
}
