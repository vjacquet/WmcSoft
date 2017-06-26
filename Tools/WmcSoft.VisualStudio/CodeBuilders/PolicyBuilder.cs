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
using System.Collections.Generic;
using System.CodeDom;
using System.ComponentModel;
using System.Xml;

namespace WmcSoft.CodeBuilders
{
    public class PolicyBuilder : CodeBuilder
    {
        static readonly TypeConverter FallbackConverter = TypeDescriptor.GetConverter(typeof(String));

        public override void Parse(XmlReader reader, CodeBuilderContext context)
        {
            var attributes = CodeBuilder.ReadAttributes(reader);

            int depth = (reader.NodeType == XmlNodeType.None) ? -1 : reader.Depth;
            while (reader.Read() && (depth < reader.Depth)) {
                switch (reader.NodeType) {
                case XmlNodeType.Element:
                    CodePolicyRule rule = ParseRule(reader, context);
                    rules.Add(rule);
                    break;
                case XmlNodeType.EndElement:
                    break;
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                case XmlNodeType.EntityReference:
                case XmlNodeType.ProcessingInstruction:
                case XmlNodeType.XmlDeclaration:
                case XmlNodeType.Comment:
                case XmlNodeType.DocumentType:
                case XmlNodeType.Whitespace:
                case XmlNodeType.SignificantWhitespace:
                    break;
                }
            }
            if ((depth == reader.Depth) && (reader.NodeType == XmlNodeType.EndElement)) {
                reader.Read();
            }

            context.RegisterPolicy(attributes["name"], this);
            attributes.Remove("name");
            if (attributes.Count > 0)
                throw new CodeBuilderException("Unrecognize attribute.");
        }

        CodePolicyRule ParseRule(System.Xml.XmlReader reader, CodeBuilderContext context)
        {
            var attributes = CodeBuilder.ReadAttributes(reader);
            string typeName = attributes["type"];


            var type = Type.GetType(typeName);
            var rule = (CodePolicyRule)Activator.CreateInstance(type);
            attributes.Remove("type");

            var supportInitialize = rule as ISupportInitialize;
            if (supportInitialize != null) {
                supportInitialize.BeginInit();
            }

            var properties = TypeDescriptor.GetProperties(rule);
            if (properties.Count > 0) {
                foreach (string name in attributes.Keys) {
                    var propertyDescriptor = properties.Find(name, true);
                    if (propertyDescriptor == null)
                        continue;

                    string value = attributes[name];
                    var typeConverter = propertyDescriptor.Converter;
                    if (typeConverter != null && typeConverter.CanConvertFrom(typeof(String))) {
                        object convertedValue = typeConverter.ConvertFromInvariantString(value);
                        propertyDescriptor.SetValue(rule, convertedValue);
                    } else if (FallbackConverter.CanConvertTo(propertyDescriptor.PropertyType)) {
                        object convertedValue = FallbackConverter.ConvertTo(value, propertyDescriptor.PropertyType);
                        propertyDescriptor.SetValue(rule, convertedValue);
                    }
                }
            }

            if (supportInitialize != null) {
                supportInitialize.EndInit();
            }


            return rule;
        }

        List<CodePolicyRule> rules = new List<CodePolicyRule>();

        public void ApplyRules(CodeTypeMember codeTypeMember, CodeBuilderContext context)
        {
            foreach (var rule in rules) {
                rule.Apply(context.CurrentCompileUnit, context.CurrentTypeDeclaration, codeTypeMember);
            }
        }
    }
}