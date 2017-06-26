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
using System.CodeDom;

namespace WmcSoft.CodeBuilders.Policies
{
    public class ImplementPropertyThroughMethodsRule : CodePolicyRule
    {
        public string FieldName { get; set; }
        public string PropertyName { get; set; }
        public string Getter { get; set; }
        public string Setter { get; set; }
        public string Reset { get; set; }
        public string ShouldSerialize { get; set; }
        public string TypeName { get; set; }
        public string DefaultValuesTypeName { get; set; }

        public override void Apply(CodeCompileUnit codeCompileUnit, CodeTypeDeclaration codeTypeDeclaration, CodeTypeMember codeTypeMember)
        {
            CodeExpression targetObject = new CodeThisReferenceExpression();
            if (!String.IsNullOrEmpty(TypeName))
                targetObject = new CodeTypeReferenceExpression(TypeName);

            CodeExpression delegateObject = targetObject;
            if (!String.IsNullOrEmpty(PropertyName))
                delegateObject = new CodePropertyReferenceExpression(targetObject, PropertyName);
            else if (!String.IsNullOrEmpty(FieldName))
                delegateObject = new CodeFieldReferenceExpression(targetObject, FieldName);

            CodeExpression defaultValuesTarget = null;
            if (DefaultValuesTypeName != null) {
                defaultValuesTarget = new CodeTypeReferenceExpression(DefaultValuesTypeName);
            }

            var codeMemberProperty = codeTypeMember as CodeMemberProperty;
            if (codeMemberProperty != null) {
                codeMemberProperty.Attributes &= ~MemberAttributes.Abstract;
                codeMemberProperty.Attributes |= MemberAttributes.Override;

                if (codeMemberProperty.HasGet && !String.IsNullOrEmpty(Getter)) {
                    var invoke = new CodeMethodInvokeExpression(delegateObject, Getter);
                    invoke.Parameters.Add(new CodePrimitiveExpression(codeMemberProperty.Name));
                    if (defaultValuesTarget != null) {
                        var property = new CodePropertyReferenceExpression(defaultValuesTarget, codeMemberProperty.Name);
                        invoke.Parameters.Add(property);
                    }
                    var returnStatement = new CodeMethodReturnStatement(new CodeCastExpression(codeMemberProperty.Type, invoke));
                    codeMemberProperty.GetStatements.Add(returnStatement);
                }
                if (codeMemberProperty.HasSet && !String.IsNullOrEmpty(Setter)) {
                    var invoke = new CodeMethodInvokeExpression(delegateObject, Setter);
                    invoke.Parameters.Add(new CodePrimitiveExpression(codeMemberProperty.Name));
                    invoke.Parameters.Add(new CodePropertySetValueReferenceExpression());
                    codeMemberProperty.SetStatements.Add(invoke);
                }
                if (codeMemberProperty.HasSet && !String.IsNullOrEmpty(ShouldSerialize)) {
                    var shouldSerializeMethod = new CodeMemberMethod();
                    shouldSerializeMethod.Name = "ShouldSerialize" + codeMemberProperty.Name;
                    shouldSerializeMethod.Attributes |= MemberAttributes.Public;
                    shouldSerializeMethod.ReturnType = new CodeTypeReference("System.Boolean");

                    var invoke = new CodeMethodInvokeExpression(delegateObject, ShouldSerialize);
                    invoke.Parameters.Add(new CodePrimitiveExpression(codeMemberProperty.Name));
                    shouldSerializeMethod.Statements.Add(new CodeMethodReturnStatement(invoke));

                    int index = codeTypeDeclaration.Members.IndexOf(codeMemberProperty);
                    codeTypeDeclaration.Members.Insert(index + 1, shouldSerializeMethod);
                }
                if (codeMemberProperty.HasSet && !String.IsNullOrEmpty(Reset)) {
                    var resetMethod = new CodeMemberMethod();
                    resetMethod.Name = "Reset" + codeMemberProperty.Name;
                    resetMethod.Attributes |= MemberAttributes.Public;

                    var invoke = new CodeMethodInvokeExpression(delegateObject, Reset);
                    invoke.Parameters.Add(new CodePrimitiveExpression(codeMemberProperty.Name));
                    resetMethod.Statements.Add(invoke);

                    int index = codeTypeDeclaration.Members.IndexOf(codeMemberProperty);
                    codeTypeDeclaration.Members.Insert(index + 1, resetMethod);
                }
            }
        }
    }
}
