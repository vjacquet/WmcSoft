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
using System.ComponentModel;
using System.Collections.Generic;

namespace WmcSoft.CodeBuilders.Policies
{
    public class FlowGroupEntityPropertyRule : CodePolicyRule
    {
        public override void Apply(CodeCompileUnit codeCompileUnit, CodeTypeDeclaration codeTypeDeclaration, CodeTypeMember codeTypeMember) {
            var property = codeTypeMember as CodeMemberProperty;
            if (property != null) {
                property.Attributes &= ~MemberAttributes.Abstract;

                var @this = new CodeThisReferenceExpression();
                var name = new CodePrimitiveExpression(property.Name);

                // get the default value, if any
                CodeExpression defaultValueExpression = null;
                if (property.UserData.Contains("defaultValue")) {
                    defaultValueExpression = (CodeExpression)property.UserData["defaultValue"];
                }

                var method = new CodeMethodInvokeExpression(@this, "GetProperty", name);
                if (defaultValueExpression != null) {
                    method.Parameters.Add(defaultValueExpression);
                }
                property.GetStatements.Clear();
                property.GetStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(property.Type, method)));

                if (property.HasSet) {
                    var @value = new CodePropertySetValueReferenceExpression();
                    var ifChanged = PropertyBuilder.CreateIfPropertyChangedStatement(property);
                    ifChanged.TrueStatements.Add(new CodeMethodInvokeExpression(@this, "SetProperty", name, value));

                    property.SetStatements.Clear();
                    property.SetStatements.Add(ifChanged);
                }
            }
        }
    }
}
