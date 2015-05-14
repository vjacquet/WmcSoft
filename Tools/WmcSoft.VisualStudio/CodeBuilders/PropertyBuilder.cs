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
using System.Text;
using System.CodeDom;
using System.ComponentModel;

namespace WmcSoft.CodeBuilders
{
    public class PropertyBuilder : TerminalCodeBuilder
    {
        protected override void DoParse(IDictionary<string, string> attributes, CodeBuilderContext context) {
            string name = attributes["name"];
            string typeName = attributes["type"];
            string access = attributes["access"];
            string readOnly = attributes["readOnly"];
            bool hasDefaultValue = attributes.ContainsKey("default");

            CodeTypeReference codeTypeReference = context.GetTypeReference(typeName);
            CodeMemberProperty property = new CodeMemberProperty();
            property.Name = name;
            property.Attributes = MemberAttributes.Abstract;
            property.Attributes |= CodeBuilder.InterpretAccessType(access);
            property.Type = codeTypeReference;
            property.HasGet = true;
            property.HasSet = !CodeBuilder.InterpretReadOnly(readOnly);

            // get
            CodeExpression defaultValueExpression = null;
            if (hasDefaultValue) {
                string defaultValueString = attributes["default"];
                object defaultValue = defaultValueString;
                Type type = Type.GetType(typeName, false);
                if (type != null) {
                    TypeConverter converter = TypeDescriptor.GetConverter(type);
                    if (converter.CanConvertFrom(typeof(string))) {
                        try {
                            defaultValue = converter.ConvertFromInvariantString(defaultValueString);
                            defaultValueExpression = new CodePrimitiveExpression(defaultValue);
                            CodeAttributeDeclaration defaultAttribute = new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(System.ComponentModel.DefaultValueAttribute)),
                                new CodeAttributeArgument(defaultValueExpression)
                            );
                            property.CustomAttributes.Add(defaultAttribute);
                        }
                        catch (Exception) {
                            // may be it is a static property
                            string defaultTypeName = defaultValueString.Substring(0, defaultValueString.LastIndexOf('.'));
                            string defaultPropertyName = defaultValueString.Substring(defaultValueString.LastIndexOf('.') + 1);
                            defaultValueExpression = new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(new CodeTypeReference(defaultTypeName)), defaultPropertyName);
                        }
                    }
                }

                property.UserData.Add("defaultValue", defaultValueExpression);

            }

            // set

            // add the member
            context.CurrentTypeDeclaration.Members.Add(property);
            context.ApplyCurrentPolicyRules(property);

            attributes.Remove("readOnly");
            attributes.Remove("default");
            attributes.Remove("access");
            attributes.Remove("type");
            attributes.Remove("name");
        }

        public static CodeConditionStatement CreateIfPropertyChangedStatement(CodeMemberProperty property) {
            return new CodeConditionStatement(
                new CodeBinaryOperatorExpression(
                    new CodePropertyReferenceExpression(
                        new CodeThisReferenceExpression(),
                        property.Name),
                    CodeBinaryOperatorType.IdentityInequality,
                    new CodePropertySetValueReferenceExpression()
                ));
        }
    }
}
