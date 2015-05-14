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
using WmcSoft.CodeDom;

namespace WmcSoft.CodeBuilders.Policies
{
    public class InjectDefaultValueSupportRule : CodePolicyRule
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

        public string TypeName {
            get {
                return typeName;
            }
            set {
                typeName = value;
            }
        }
        string typeName = null;

        public override void Apply(System.CodeDom.CodeCompileUnit codeCompileUnit, System.CodeDom.CodeTypeDeclaration codeTypeDeclaration, System.CodeDom.CodeTypeMember codeTypeMember) {
            CodeExpression targetObject = new CodeThisReferenceExpression();
            if (!String.IsNullOrEmpty(typeName))
                targetObject = new CodeTypeReferenceExpression(typeName);

            CodeExpression delegateObject = targetObject;
            if (!String.IsNullOrEmpty(propertyName))
                delegateObject = new CodePropertyReferenceExpression(targetObject, propertyName);
            else if (!String.IsNullOrEmpty(fieldName))
                delegateObject = new CodeFieldReferenceExpression(targetObject, fieldName);

            CodeMemberProperty codeMemberProperty = codeTypeMember as CodeMemberProperty;
            if (codeMemberProperty != null) {
                CodeExpression property = codeMemberProperty.UserData["defaultValue"] as CodeExpression;
                if (property == null)
                    property = new CodePropertyReferenceExpression(delegateObject, codeMemberProperty.Name);

                if (codeMemberProperty.HasGet) {
                    foreach (CodeStatement statement in codeMemberProperty.GetStatements) {
                        CodeMethodReturnStatement returnStatement = statement as CodeMethodReturnStatement;
                        if (returnStatement != null) {
                            CodeExpression expression = returnStatement.Expression;
                            if (expression is CodeCastExpression) {
                                expression = ((CodeCastExpression)returnStatement.Expression).Expression;
                            }
                            CodeMethodInvokeExpression invoke = expression as CodeMethodInvokeExpression;

                            invoke.Parameters.Add(property);
                        }
                    }
                }
                if (codeMemberProperty.HasSet) {
                    // add default
                    foreach (CodeStatement statement in codeMemberProperty.SetStatements) {
                        CodeExpressionStatement expression = statement as CodeExpressionStatement;
                        if (expression == null)
                            continue;
                        CodeMethodInvokeExpression invoke = expression.Expression as CodeMethodInvokeExpression;
                        if (invoke != null) {
                            invoke.Parameters.Add(property);
                        }
                    }

                    CodeDomTypeBuilder builder = new CodeDomTypeBuilder(codeTypeDeclaration);
                    string resetMethodName = "Reset" + codeMemberProperty.Name;
                    CodeMemberMethod resetMethod = builder.FindMethod(resetMethodName);
                    if (resetMethod == null) {
                        resetMethod = new CodeMemberMethod();
                        resetMethod.Name = resetMethodName;
                        resetMethod.Attributes |= MemberAttributes.Public;

                        CodeAssignStatement assignStatement = new CodeAssignStatement();
                        assignStatement.Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), codeMemberProperty.Name);
                        assignStatement.Right = property;
                        resetMethod.Statements.Add(assignStatement);

                        int index = codeTypeDeclaration.Members.IndexOf(codeMemberProperty);
                        codeTypeDeclaration.Members.Insert(index + 1, resetMethod);
                    } else {
                        foreach (CodeStatement statement in resetMethod.Statements) {
                            CodeExpressionStatement expression = statement as CodeExpressionStatement;
                            if (expression == null)
                                continue;
                            CodeMethodInvokeExpression invoke = expression.Expression as CodeMethodInvokeExpression;
                            if (invoke != null && invoke.Method.MethodName.StartsWith("Reset")) {
                                invoke.Parameters.Add(property);
                            }
                        }
                    }

                    string shouldSerializeMethodName = "ShouldSerialize" + codeMemberProperty.Name;
                    CodeMemberMethod shouldSerializeMethod = builder.FindMethod(shouldSerializeMethodName);
                    if (shouldSerializeMethod == null) {
                        shouldSerializeMethod = new CodeMemberMethod();
                        shouldSerializeMethod.Name = shouldSerializeMethodName;
                        shouldSerializeMethod.Attributes |= MemberAttributes.Public;
                        shouldSerializeMethod.ReturnType = new CodeTypeReference("System.Boolean");

                        CodeBinaryOperatorExpression operatorExpression = new CodeBinaryOperatorExpression();
                        operatorExpression.Left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), codeMemberProperty.Name);
                        operatorExpression.Operator = CodeBinaryOperatorType.IdentityInequality;
                        operatorExpression.Right = property;

                        shouldSerializeMethod.Statements.Add(new CodeMethodReturnStatement(operatorExpression));

                        int index = codeTypeDeclaration.Members.IndexOf(codeMemberProperty);
                        codeTypeDeclaration.Members.Insert(index + 1, shouldSerializeMethod);
                    } else {
                        foreach (CodeStatement statement in shouldSerializeMethod.Statements) {
                            CodeMethodReturnStatement expression = statement as CodeMethodReturnStatement;
                            if (expression == null)
                                continue;
                            CodeMethodInvokeExpression invoke = expression.Expression as CodeMethodInvokeExpression;
                            if (invoke != null && invoke.Method.MethodName.StartsWith("ShouldSerialize")) {
                                invoke.Parameters.Add(property);
                            }
                        }
                    }
                }
            }
        }
    }
}
