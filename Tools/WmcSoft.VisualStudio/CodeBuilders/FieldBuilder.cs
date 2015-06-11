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

namespace WmcSoft.CodeBuilders
{
    public class FieldBuilder : TerminalCodeBuilder
    {
        protected override void DoParse(IDictionary<string, string> attributes, CodeBuilderContext context) {
            string name = attributes["name"];
            string typeName = attributes["type"];
            string access = attributes["access"];

            CodeTypeReference codeTypeReference = context.GetTypeReference(typeName);
            CodeMemberField field = new CodeMemberField(codeTypeReference, name);
            field.Attributes = CodeBuilder.InterpretAccessType(access);
            context.CurrentTypeDeclaration.Members.Add(field);

            context.ApplyCurrentPolicyRules(field);

            attributes.Remove("access");
            attributes.Remove("type");
            attributes.Remove("name");
        }
    }
}