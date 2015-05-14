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
using System.Reflection;
using WmcSoft.Reflection;
using System.Xml;

namespace WmcSoft.CodeBuilders.Policies
{
    public class ImplementPropertyThroughMethodsRule : CodePolicyRule
    {
        public string FieldName {
            get {
                return fieldName;
            }
            set {
                fieldName = value;
            }
        }
        string fieldName;

        public string PropertyName {
            get {
                return propertyName;
            }
            set {
                propertyName = value;
            }
        }
        string propertyName;

        public string Getter {
            get {
                return getter;
            }
            set {
                getter = value;
            }
        }
        string getter;

        public string Setter {
            get {
                return setter;
            }
            set {
                setter = value;
            }
        }
        string setter;

        public string Reset {
            get {
                return reset;
            }
            set {
                reset = value;
            }
        }
        string reset;

        public string ShouldSerialize {
            get {
                return shouldSerialize;
            }
            set {
                shouldSerialize = value;
            }
        }
        string shouldSerialize;

        public string TypeName {
            get {
                return typeName;
            }
            set {
                typeName = value;
            }
        }
        string typeName = null;

        public string DefaultValuesTypeName {
            get {
                return defaultValuesTypeName;
            }
            set {
                defaultValuesTypeName = value;
            }
        }
        string defaultValuesTypeName = null;

        public override void Apply(System.CodeDom.CodeCompileUnit codeCompileUnit, System.CodeDom.CodeTypeDeclaration codeTypeDeclaration, System.CodeDom.CodeTypeMember codeTypeMember) {
            CodeExpression targetObject = new CodeThisReferenceExpression();
            if (!String.IsNullOrEmpty(typeName))
                targetObject = new CodeTypeReferenceExpression(typeName);

            CodeExpression delegateObject = targetObject;
            if (!String.IsNullOrEmpty(propertyName))
                delegateObject = new CodePropertyReferenceExpression(targetObject, propertyName);
            else if (!String.IsNullOrEmpty(fieldName))
                delegateObject = new CodeFieldReferenceExpression(targetObject, fieldName);

            CodeExpression defaultValuesTarget = null;
            if (defaultValuesTypeName != null) {
                defaultValuesTarget = new CodeTypeReferenceExpression(defaultValuesTypeName);
            }

            CodeMemberProperty codeMemberProperty = codeTypeMember as CodeMemberProperty;
            if (codeMemberProperty != null) {
                codeMemberProperty.Attributes &= ~MemberAttributes.Abstract;
                codeMemberProperty.Attributes |= MemberAttributes.Override;

                if (codeMemberProperty.HasGet && !String.IsNullOrEmpty(getter)) {
                    CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(delegateObject, getter);
                    invoke.Parameters.Add(new CodePrimitiveExpression(codeMemberProperty.Name));
                    if (defaultValuesTarget != null) {
                        CodePropertyReferenceExpression property =
                            new CodePropertyReferenceExpression(defaultValuesTarget, codeMemberProperty.Name);
                        invoke.Parameters.Add(property);
                    }
                    CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement(new CodeCastExpression(codeMemberProperty.Type, invoke));
                    codeMemberProperty.GetStatements.Add(returnStatement);
                }
                if (codeMemberProperty.HasSet && !String.IsNullOrEmpty(setter)) {
                    CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(delegateObject, setter);
                    invoke.Parameters.Add(new CodePrimitiveExpression(codeMemberProperty.Name));
                    invoke.Parameters.Add(new CodePropertySetValueReferenceExpression());
                    codeMemberProperty.SetStatements.Add(invoke);
                }
                if (codeMemberProperty.HasSet && !String.IsNullOrEmpty(shouldSerialize)) {
                    CodeMemberMethod shouldSerializeMethod = new CodeMemberMethod();
                    shouldSerializeMethod.Name = "ShouldSerialize" + codeMemberProperty.Name;
                    shouldSerializeMethod.Attributes |= MemberAttributes.Public;
                    shouldSerializeMethod.ReturnType = new CodeTypeReference("System.Boolean");

                    CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(delegateObject, shouldSerialize);
                    invoke.Parameters.Add(new CodePrimitiveExpression(codeMemberProperty.Name));
                    shouldSerializeMethod.Statements.Add(new CodeMethodReturnStatement(invoke));

                    int index = codeTypeDeclaration.Members.IndexOf(codeMemberProperty);
                    codeTypeDeclaration.Members.Insert(index + 1, shouldSerializeMethod);
                }
                if (codeMemberProperty.HasSet && !String.IsNullOrEmpty(reset)) {
                    CodeMemberMethod resetMethod = new CodeMemberMethod();
                    resetMethod.Name = "Reset" + codeMemberProperty.Name;
                    resetMethod.Attributes |= MemberAttributes.Public;

                    CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(delegateObject, reset);
                    invoke.Parameters.Add(new CodePrimitiveExpression(codeMemberProperty.Name));
                    resetMethod.Statements.Add(invoke);

                    int index = codeTypeDeclaration.Members.IndexOf(codeMemberProperty);
                    codeTypeDeclaration.Members.Insert(index + 1, resetMethod);
                }
            }
        }
    }
}
