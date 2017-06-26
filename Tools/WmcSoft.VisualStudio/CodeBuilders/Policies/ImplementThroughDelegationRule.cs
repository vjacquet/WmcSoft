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
    public class ImplementThroughDelegationRule : CodePolicyRule
    {
        public string FieldName { get; set; }
        public string PropertyName { get; set; }
        public string TypeName { get; set; }

        public override void Apply(CodeCompileUnit codeCompileUnit, CodeTypeDeclaration codeTypeDeclaration, CodeTypeMember codeTypeMember)
        {
            CodeExpression targetObject = new CodeThisReferenceExpression();
            if (!string.IsNullOrEmpty(TypeName))
                targetObject = new CodeTypeReferenceExpression(TypeName);

            CodeExpression delegateObject = null;
            if (!string.IsNullOrEmpty(PropertyName))
                delegateObject = new CodePropertyReferenceExpression(targetObject, PropertyName);
            else if (!string.IsNullOrEmpty(FieldName))
                delegateObject = new CodeFieldReferenceExpression(targetObject, FieldName);
            else if (string.IsNullOrEmpty(TypeName)) {
                delegateObject = new CodeBaseReferenceExpression();
            } else {
                throw new CodeBuilderException("PropertyName or FieldName must be defined when TypeName is defined.");
            }

            switch (codeTypeMember) {
            case CodeMemberProperty codeMemberProperty:
                codeMemberProperty.Attributes &= ~MemberAttributes.Abstract;
                codeMemberProperty.Attributes |= MemberAttributes.Override;

                var property = new CodePropertyReferenceExpression(delegateObject, codeMemberProperty.Name);
                if (codeMemberProperty.HasGet)
                    codeMemberProperty.GetStatements.Add(new CodeMethodReturnStatement(property));
                if (codeMemberProperty.HasSet)
                    codeMemberProperty.SetStatements.Add(new CodeAssignStatement(property, new CodePropertySetValueReferenceExpression()));
                break;
            case CodeMemberMethod codeMemberMethod:
                codeMemberMethod.Attributes &= ~MemberAttributes.Abstract;
                codeMemberMethod.Attributes |= MemberAttributes.Override;

                var invoke = new CodeMethodInvokeExpression(delegateObject, codeMemberMethod.Name);
                foreach (CodeParameterDeclarationExpression parameter in codeMemberMethod.Parameters) {
                    CodeExpression codeExpression = new CodeArgumentReferenceExpression(parameter.Name);
                    if (parameter.Direction != FieldDirection.In) {
                        codeExpression = new CodeDirectionExpression(parameter.Direction, codeExpression);
                    }
                    invoke.Parameters.Add(codeExpression);
                }
                if (codeMemberMethod.ReturnType.BaseType != "System.Void")
                    codeMemberMethod.Statements.Add(new CodeMethodReturnStatement(invoke));
                else
                    codeMemberMethod.Statements.Add(invoke);
                break;
            }
        }
    }
}
