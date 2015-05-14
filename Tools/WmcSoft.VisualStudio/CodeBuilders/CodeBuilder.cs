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
using System.Xml.XPath;
using System.CodeDom;
using System.Xml;
using System.Diagnostics;

namespace WmcSoft.CodeBuilders
{
    public abstract class CodeBuilder
    {
        class IgnoreCodeBuilder : CodeBuilder
        {
            public IgnoreCodeBuilder() {
            }

            public override void Parse(XmlReader reader, CodeBuilderContext context) {
                reader.Skip();
            }
        }
        public static readonly CodeBuilder Ignore = new IgnoreCodeBuilder();

        public abstract void Parse(XmlReader reader, CodeBuilderContext context);

        internal static IDictionary<string, string> ReadAttributes(XmlReader reader) {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            if (reader.MoveToFirstAttribute()) {
                do {
                    string name = reader.LocalName;
                    if (name == "xmlns")
                        continue;

                    attributes.Add(name, reader.Value);
                } while (reader.MoveToNextAttribute());
                reader.MoveToElement();
            }
            return attributes;
        }

        internal static MemberAttributes InterpretAccessType(string access) {
            switch (access) {
            case "private":
                return MemberAttributes.Private;
            case "public":
                return MemberAttributes.Public;
            case "protected":
                return MemberAttributes.Family;
            case "internal":
                return MemberAttributes.FamilyAndAssembly;
            case "protected internal":
                return MemberAttributes.FamilyOrAssembly;
            default:
                throw new CodeBuilderException(String.Format("Undetermined access type \"{0}\".", access));
            }
        }

        internal static bool InterpretReadOnly(string readOnly) {
            return (readOnly != "false");
        }
    }


    /*class NamespaceBuilder : CodeBuilder
    {
        public override void Handle(CodeBuilderContext context, XmlNode node) {
            Debug.Assert(node.Name == "namespace");

            CodeNamespace codeNamespace = context.AddNamespace(node.Attributes["name"].Value);

            foreach (XmlNode child in node.SelectNodes("*")) {
                switch (child.Name) {
                case "import":
                    new ImportBuilder().Handle(context, child);
                    break;
                case "class":
                    new ClassBuilder().Handle(context, child);
                    break;
                default:
                    Debug.Assert(false, "Unknown child.");
                    break;
                }
            }

            context.codeNamespace.Pop();
        }
    }*/

    /*class ClassBuilder : CodeBuilder
    {
        public override void Handle(CodeBuilderContext context, XmlNode node) {
            Debug.Assert(node.Name == "class");

            CodeTypeDeclaration codeTypeDeclaration = context.AddTypeDeclaration(node.Attributes["name"].Value);
            codeTypeDeclaration.IsClass = true;
            codeTypeDeclaration.IsPartial = true;

            foreach (XmlNode child in node.SelectNodes("*")) {
                switch (child.Name) {
                case "inherit":
                    new InheritBuilder().Handle(context, child);
                    break;
                case "implement":
                    new ImplementBuilder().Handle(context, child);
                    break;
                case "rules":
                    new RulesBuilder().Handle(context, child);
                    break;
                case "properties":
                    new PropertiesBuilder().Handle(context, child);
                    break;
                case "fields":
                    new FieldsBuilder().Handle(context, child);
                    break;
                default:
                    Debug.Assert(false, "Unknown child.");
                    break;
                }
            }
        }
    }*/

    /*
        class RulesBuilder : CodeBuilder
        {
            public override void Handle(CodeBuilderContext context, XmlNode node) {
                Debug.Assert(node.Name == "rules");

                foreach (XmlNode child in node.SelectNodes("*")) {
                    switch (child.Name) {
                    case "extract":
                        new ExtractRuleBuilder().Handle(context, child);
                        break;
                    case "implement":
                        new ImplementRuleBuilder().Handle(context, child);
                        break;
                    default:
                        Debug.Assert(false, "Unknown child.");
                        break;
                    }
                }
            }
        }

        class ExtractRuleBuilder : CodeBuilder
        {
            public override void Handle(CodeBuilderContext context, XmlNode node) {
                string typeName = node.Attributes["from"].Value;
                Type type = Type.GetType(typeName);

                List<MemberInfo> members = new List<MemberInfo>();
                SelectMembers(type, node.SelectNodes("*"), members);
                foreach (MemberInfo memberInfo in members) {
                    if (memberInfo is PropertyInfo) {
                        PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                        CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
                        codeMemberProperty.Name = propertyInfo.Name;
                        codeMemberProperty.Attributes = MemberAttributes.Abstract | MemberAttributes.Public;
                        codeMemberProperty.Type = new CodeTypeReference(propertyInfo.PropertyType.FullName);
                        codeMemberProperty.HasGet = propertyInfo.CanRead;
                        codeMemberProperty.HasSet = propertyInfo.CanWrite;

                        context.CurrentTypeDeclaration.Members.Add(codeMemberProperty);
                    } else if (memberInfo is MethodInfo) {
                        MethodInfo methodInfo = (MethodInfo)memberInfo;
                        CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
                        codeMemberMethod.Name = methodInfo.Name;
                        codeMemberMethod.Attributes = MemberAttributes.Abstract | MemberAttributes.Public;
                        codeMemberMethod.ReturnType = new CodeTypeReference(methodInfo.ReturnType.FullName);
                        foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
                            string parameterType = parameterInfo.ParameterType.FullName;
                            if (parameterInfo.ParameterType.IsByRef)
                                parameterType = parameterType.TrimEnd('&');
                            CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(
                                new CodeTypeReference(parameterType),
                                parameterInfo.Name);
                            if (parameterInfo.IsOut) {
                                if (parameterInfo.IsIn)
                                    codeParameterDeclarationExpression.Direction = FieldDirection.Ref;
                                else
                                    codeParameterDeclarationExpression.Direction = FieldDirection.Out;
                            }
                            codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
                        }

                        context.CurrentTypeDeclaration.Members.Add(codeMemberMethod);
                    }
                }

    #if OLD
                    Hashtable methods = new Hashtable();
                    foreach (PropertyInfo propertyInfo in type.GetProperties()) {
                        if ((propertyInfo.CanRead && propertyInfo.GetGetMethod().IsAbstract)
                            || (propertyInfo.CanWrite && propertyInfo.GetSetMethod().IsAbstract)) {
                            CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
                            codeMemberProperty.Name = propertyInfo.Name;
                            codeMemberProperty.Attributes = MemberAttributes.Abstract | MemberAttributes.Public;
                            codeMemberProperty.Type = new CodeTypeReference(propertyInfo.PropertyType.FullName);
                            codeMemberProperty.HasGet = propertyInfo.CanRead;
                            codeMemberProperty.HasSet = propertyInfo.CanWrite;

                            if (propertyInfo.CanRead)
                                methods.Add(propertyInfo.GetGetMethod(), null);
                            if (propertyInfo.CanWrite)
                                methods.Add(propertyInfo.GetSetMethod(), null);

                            context.CurrentTypeDeclaration.Members.Add(codeMemberProperty);
                        }
                    }
                    foreach (MethodInfo methodInfo in type.GetMethods()) {
                        if (methodInfo.IsAbstract) {
                            if (methods.ContainsKey(methodInfo))
                                continue;

                            CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
                            codeMemberMethod.Name = methodInfo.Name;
                            codeMemberMethod.Attributes = MemberAttributes.Abstract | MemberAttributes.Public;
                            if (methodInfo.ReturnType != null)
                                codeMemberMethod.ReturnType = new CodeTypeReference(methodInfo.ReturnType.FullName);
                            foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
                                string parameterType = parameterInfo.ParameterType.FullName;
                                if (parameterInfo.ParameterType.IsByRef)
                                    parameterType = parameterType.TrimEnd('&');
                                CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(
                                    new CodeTypeReference(parameterType),
                                    parameterInfo.Name);
                                if (parameterInfo.IsOut) {
                                    if (parameterInfo.IsIn)
                                        codeParameterDeclarationExpression.Direction = FieldDirection.Ref;
                                    else
                                        codeParameterDeclarationExpression.Direction = FieldDirection.Out;
                                }
                                codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
                            }

                            context.CurrentTypeDeclaration.Members.Add(codeMemberMethod);
                        }
                    }
    #endif
                type = null;
            }
        }

        class ImplementRuleBuilder : CodeBuilder
        {
            public override void Handle(CodeBuilderContext context, XmlNode node) {
                string typeName = node.Attributes["from"].Value;
                Type type = Type.GetType(typeName);

                CodeFieldReferenceExpression @using = new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    node.Attributes["using"].Value
                );

                List<MemberInfo> members = new List<MemberInfo>();
                SelectMembers(type, node.SelectNodes("*"), members);
                foreach (MemberInfo memberInfo in members) {
                    if (memberInfo is PropertyInfo) {
                        PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                        CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
                        codeMemberProperty.Name = propertyInfo.Name;
                        codeMemberProperty.Attributes = MemberAttributes.Override | MemberAttributes.Public;
                        codeMemberProperty.Type = new CodeTypeReference(propertyInfo.PropertyType.FullName);
                        codeMemberProperty.HasGet = propertyInfo.CanRead;
                        codeMemberProperty.HasSet = propertyInfo.CanWrite;

                        CodePropertyReferenceExpression property =
                            new CodePropertyReferenceExpression(@using, propertyInfo.Name);

                        if (codeMemberProperty.HasGet)
                            codeMemberProperty.GetStatements.Add(new CodeMethodReturnStatement(property));
                        if (codeMemberProperty.HasSet)
                            codeMemberProperty.SetStatements.Add(new CodeAssignStatement(property, new CodePropertySetValueReferenceExpression()));

                        context.CurrentTypeDeclaration.Members.Add(codeMemberProperty);
                    } else if (memberInfo is MethodInfo) {
                        MethodInfo methodInfo = (MethodInfo)memberInfo;
                        CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
                        codeMemberMethod.Name = methodInfo.Name;
                        codeMemberMethod.Attributes = MemberAttributes.Public;
                        codeMemberMethod.ReturnType = new CodeTypeReference(methodInfo.ReturnType.FullName);
                        foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
                            string parameterType = parameterInfo.ParameterType.FullName;
                            if (parameterInfo.ParameterType.IsByRef)
                                parameterType = parameterType.TrimEnd('&');
                            CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(
                                new CodeTypeReference(parameterType),
                                parameterInfo.Name);
                            if (parameterInfo.IsOut) {
                                if (parameterInfo.IsIn)
                                    codeParameterDeclarationExpression.Direction = FieldDirection.Ref;
                                else
                                    codeParameterDeclarationExpression.Direction = FieldDirection.Out;
                            }
                            codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
                        }

                        context.CurrentTypeDeclaration.Members.Add(codeMemberMethod);
                    }
                }
                type = null;
            }
        }

        class PropertiesBuilder : CodeBuilder
        {
            public override void Handle(CodeBuilderContext context, XmlNode node) {
                Debug.Assert(node.Name == "properties");
                string policy = node.Attributes["policy"].Value;
                PropertyBuilder property = new PropertyBuilder(policy);
                foreach (XmlNode child in node.SelectNodes("*")) {
                    property.Handle(context, child);
                }
            }
        }

        class PropertyBuilder : CodeBuilder
        {
            string policy;
            public PropertyBuilder(string policy) {
                this.policy = policy;
            }

            public override void Handle(CodeBuilderContext context, XmlNode node) {
                Debug.Assert(node.Name == "property");

                switch (policy) {
                case "fg.property/1.1":
                    Handle_FGProperty1_1(context, node);
                    break;
                }
            }

            protected void Handle_FGProperty1_1(CodeBuilderContext context, XmlNode node) {
                CodeTypeReference codeTypeReference = context.GetTypeReference(node.Attributes["type"].Value);
                CodeMemberProperty property = new CodeMemberProperty();
                property.Name = node.Attributes["name"].Value;
                property.Attributes = MemberAttributes.Public;
                property.Type = codeTypeReference;
                property.HasGet = true;
                property.HasSet = false;

                // get
                CodeExpression defaultValueExpression = null;
                CodeExpression get;
                if (node.Attributes["default"] != null) {
                    string defaultValueString = node.Attributes["default"].Value;
                    object defaultValue = defaultValueString;
                    Type type = Type.GetType(codeTypeReference.BaseType, false);
                    if (type != null) {
                        TypeConverter converter = TypeDescriptor.GetConverter(type);
                        if (converter.CanConvertFrom(typeof(string)))
                            defaultValue = converter.ConvertFromInvariantString(defaultValueString);
                    }

                    defaultValueExpression = new CodePrimitiveExpression(defaultValue);

                    CodeAttributeDeclaration defaultAttribute = new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(System.ComponentModel.DefaultValueAttribute)),
                        new CodeAttributeArgument(defaultValueExpression)
                    );
                    property.CustomAttributes.Add(defaultAttribute);

                    get = new CodeMethodInvokeExpression(
                            new CodeThisReferenceExpression(),
                            "GetProperty",
                            new CodePrimitiveExpression(property.Name),
                            defaultValueExpression
                       );

                } else {
                    get = new CodeMethodInvokeExpression(
                            new CodeThisReferenceExpression(),
                            "GetProperty",
                            new CodePrimitiveExpression(property.Name)
                        );
                }
                property.GetStatements.Add(
                    new CodeMethodReturnStatement(
                        new CodeCastExpression(codeTypeReference, get)));

                // set
                CodeConditionStatement changed = new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), property.Name),
                        CodeBinaryOperatorType.IdentityInequality,
                        new CodePropertySetValueReferenceExpression()
                    ),
                    new CodeExpressionStatement(
                        new CodeMethodInvokeExpression(
                            new CodeThisReferenceExpression(),
                            "SetProperty",
                            new CodePrimitiveExpression(property.Name),
                            new CodePropertySetValueReferenceExpression()
                        )
                    )
                );
                property.SetStatements.Add(
                    changed
                );

                context.CurrentTypeDeclaration.Members.Add(property);
            }
        }
        */
}
