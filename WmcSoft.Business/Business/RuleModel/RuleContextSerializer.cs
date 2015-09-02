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
using WmcSoft.Runtime.Serialization;

namespace WmcSoft.Business.RuleModel
{
    public class RuleContextSerializer : XmlSerializer<RuleContext>
    {
        const string Xmlns = @"http://www.wmcsoft.fr/schemas/2015/business/RuleModel.xsd";

        protected override RuleContext DoDeserialize(XmlReader reader) {
            if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "ruleContext" && reader.NamespaceURI == Xmlns) {
                string version = null;
                var mandatory = new BitArray(1);
                while (reader.MoveToNextAttribute()) {
                    //if (reader.NamespaceURI != Xmlns)
                    //    continue;
                    if (!mandatory[0] && reader.LocalName == "version") {
                        version = reader.Value;
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

                var ruleContext = new RuleContext { Version = version };
                reader.MoveToElement();
                if (reader.IsEmptyElement) {
                    reader.Skip();
                    return ruleContext;
                } else {
                    reader.ReadStartElement();
                    reader.MoveToContent();
                    var items = new List<RuleElement>();
                    while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None) {
                        if (reader.NodeType == XmlNodeType.Element && reader.NamespaceURI == Xmlns) {
                            if (reader.LocalName == "variable") {
                                var ruleElement = Deserialize(reader, (Variable)null);
                                items.Add(ruleElement);
                            } else if (reader.LocalName == "proposition") {
                                var ruleElement = Deserialize(reader, (Proposition)null);
                                items.Add(ruleElement);
                            } else if (reader.LocalName == "ruleOverride") {
                                var ruleElement = Deserialize(reader, (RuleOverride)null);
                                items.Add(ruleElement);
                            }

                        }
                        reader.MoveToContent();
                    }
                    reader.ReadEndElement();
                    ruleContext.Items = items.ToArray();
                }
                return ruleContext;
            }
            throw new InvalidOperationException();
        }

        protected override void DoSerialize(XmlWriter writer, RuleContext value) {
            throw new NotImplementedException();
        }

        Variable Deserialize(XmlReader reader, Variable prototype) {
            string name = null;
            string value = null;
            var mandatory = new BitArray(1);
            while (reader.MoveToNextAttribute()) {
                //if (reader.NamespaceURI != Xmlns)
                //    continue;
                if (!mandatory[0] && reader.LocalName == "name") {
                    name = reader.Value;
                    mandatory[0] = true;
                } else if (reader.LocalName == "value") {
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
                if (!mandatory[0] && reader.LocalName == "name") {
                    name = reader.Value;
                    mandatory[0] = true;
                } else if (reader.LocalName == "value") {
                    value = reader.ReadContentAsBoolean();
                }
            }
            reader.Skip();
            return new Proposition { Name = name, Value = value };
        }
    }
}
