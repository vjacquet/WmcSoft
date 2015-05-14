using System;
using System.CodeDom;
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
using System.ComponentModel.Design;
using System.Globalization;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using WmcSoft.Reflection;
using WmcSoft.Xml.XPath;

namespace WmcSoft.CodeBuilders
{
    public class ReflectBuilder : CodeBuilder
    {

        class MemberInfoListBuilder
        {
            readonly string metadataTokenAttributeName;
            readonly string nameAttributeName;
            readonly string propertyElementName;

            ReflectNavigator navigator;
            IList<MemberInfo> members;
            Type type;

            public MemberInfoListBuilder(Type type, IList<MemberInfo> members) {
                this.members = members;
                this.type = type;
                navigator = new ReflectNavigator(type);
                navigator.MoveToFirstChild();
                metadataTokenAttributeName = navigator.NameTable.Get("metadataToken");
                nameAttributeName = navigator.NameTable.Get("name");
                propertyElementName = navigator.NameTable.Get("property");
            }

            public void Add(string match) {
                int metadataToken;
                MemberInfo memberInfo;
                foreach (XPathNavigator member in navigator.Select(match)) {
                    if (!Int32.TryParse(member.GetAttribute(metadataTokenAttributeName, String.Empty), NumberStyles.HexNumber, null, out metadataToken))
                        continue;
                    if (member.Name == propertyElementName) {
                        memberInfo = type.GetProperty(member.GetAttribute(nameAttributeName, String.Empty));
                    } else {
                        memberInfo = type.Module.ResolveMember(metadataToken);
                    }
                    if (memberInfo != null && !members.Contains(memberInfo))
                        members.Add(memberInfo);
                }
            }

            public void Remove(string match) {
                int metadataToken;
                MemberInfo memberInfo;
                foreach (XPathNavigator member in navigator.Select(match)) {
                    if (!Int32.TryParse(member.GetAttribute(metadataTokenAttributeName, String.Empty), NumberStyles.HexNumber, null, out metadataToken))
                        continue;
                    if (member.Name == propertyElementName) {
                        memberInfo = type.GetProperty(member.GetAttribute(nameAttributeName, String.Empty));
                    } else {
                        memberInfo = type.Module.ResolveMember(metadataToken);
                    }
                    members.Remove(memberInfo);
                }
            }
        }

        List<MemberInfo> members = new List<MemberInfo>();

        public override void Parse(XmlReader reader, CodeBuilderContext context) {
            IDictionary<string, string> attributes = CodeBuilder.ReadAttributes(reader);

            string typeName = attributes["type"];
            ITypeResolutionService typeResolutionService = context.GetService(typeof(ITypeResolutionService)) as ITypeResolutionService;
            Type type = (typeResolutionService != null) ? typeResolutionService.GetType(typeName) : Type.GetType(typeName);

            MemberInfoListBuilder builder = new MemberInfoListBuilder(type, members);

            int depth = (reader.NodeType == XmlNodeType.None) ? -1 : reader.Depth;
            string match;
            while (reader.Read() && (depth < reader.Depth)) {
                switch (reader.NodeType) {
                case XmlNodeType.Element:
                    match = reader.GetAttribute("match");
                    switch (reader.LocalName) {
                    case "add":
                        builder.Add(match);
                        break;
                    case "remove":
                        builder.Remove(match);
                        break;
                    }
                    reader.Skip();
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

            ProcessMembers(context);

            attributes.Remove("type");
            if (attributes.Count > 0)
                throw new CodeBuilderException("Unrecognize attribute.");
        }

        private CodeTypeReference CreateTypeReference(Type type) {
            CodeTypeReference codeTypeReference = new CodeTypeReference(type.FullName ?? type.Name);
            if (type.IsByRef)
                codeTypeReference.BaseType = codeTypeReference.BaseType.TrimEnd('&');

            if (type.ContainsGenericParameters) {
                foreach (var generic in type.GetGenericArguments()) {
                    codeTypeReference.TypeArguments.Add(CreateTypeReference(generic));
                }
            }
            return codeTypeReference;
        }

        private void ProcessMembers(CodeBuilderContext context) {
            foreach (MemberInfo memberInfo in members) {
                CodeTypeMember codeTypeMember = null;
                if (memberInfo is PropertyInfo) {
                    PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                    CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
                    codeMemberProperty.UserData.Add("memberInfo", memberInfo);
                    codeMemberProperty.Name = propertyInfo.Name;
                    foreach (var parameterInfo in propertyInfo.GetIndexParameters()) {
                        CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(
                          CreateTypeReference(parameterInfo.ParameterType),
                          parameterInfo.Name);
                        if (parameterInfo.IsOut) {
                            if (parameterInfo.IsIn)
                                codeParameterDeclarationExpression.Direction = FieldDirection.Ref;
                            else
                                codeParameterDeclarationExpression.Direction = FieldDirection.Out;
                        }
                        codeMemberProperty.Parameters.Add(codeParameterDeclarationExpression);
                    }
                    codeMemberProperty.Attributes = MemberAttributes.Abstract | MemberAttributes.Public;
                    codeMemberProperty.Type = CreateTypeReference(propertyInfo.PropertyType);
                    codeMemberProperty.HasGet = propertyInfo.CanRead;
                    codeMemberProperty.HasSet = propertyInfo.CanWrite;

                    codeTypeMember = codeMemberProperty;
                } else if (memberInfo is MethodInfo) {
                    MethodInfo methodInfo = (MethodInfo)memberInfo;
                    CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
                    codeMemberMethod.UserData.Add("documentation", context.GetMemberDocumentation(memberInfo));
                    codeMemberMethod.UserData.Add("memberInfo", memberInfo);
                    codeMemberMethod.Name = methodInfo.Name;
                    codeMemberMethod.Attributes = MemberAttributes.Abstract | MemberAttributes.Public;
                    codeMemberMethod.ReturnType = CreateTypeReference(methodInfo.ReturnType);
                    foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
                        CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(
                            CreateTypeReference(parameterInfo.ParameterType),
                            parameterInfo.Name);
                        if (parameterInfo.IsOut) {
                            if (parameterInfo.IsIn)
                                codeParameterDeclarationExpression.Direction = FieldDirection.Ref;
                            else
                                codeParameterDeclarationExpression.Direction = FieldDirection.Out;
                        }
                        codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
                    }

                    codeTypeMember = codeMemberMethod;
                }

                XmlDocumentation documentation = context.GetMemberDocumentation(memberInfo);
                if (documentation.Summary != null)
                    codeTypeMember.Comments.Add(new CodeCommentStatement(documentation.Summary.OuterXml, true));
                foreach (XmlNode node in documentation.Params) {
                    codeTypeMember.Comments.Add(new CodeCommentStatement(node.OuterXml, true));
                }
                if (documentation.Returns != null)
                    codeTypeMember.Comments.Add(new CodeCommentStatement(documentation.Returns.OuterXml, true));
                foreach (XmlNode node in documentation.Exceptions) {
                    codeTypeMember.Comments.Add(new CodeCommentStatement(node.OuterXml, true));
                }
                foreach (XmlNode node in documentation.Permissions) {
                    codeTypeMember.Comments.Add(new CodeCommentStatement(node.OuterXml, true));
                }
                if (documentation.Remarks != null)
                    codeTypeMember.Comments.Add(new CodeCommentStatement(documentation.Remarks.OuterXml, true));
                if (documentation.Example != null)
                    codeTypeMember.Comments.Add(new CodeCommentStatement(documentation.Example.OuterXml, true));

                context.CurrentTypeDeclaration.Members.Add(codeTypeMember);

                context.ApplyCurrentPolicyRules(codeTypeMember);

            }
        }
    }
}
