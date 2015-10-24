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

namespace WmcSoft.CodeDom
{
    public class CodeDomTypeBuilder
    {
        #region Lifecyle

        public CodeDomTypeBuilder(CodeTypeDeclaration typeDeclaration) {
            TypeDeclaration = typeDeclaration;
        }

        #endregion

        #region Properties

        public CodeTypeDeclaration TypeDeclaration { get; set; }

        #endregion

        #region Methods

        public CodeDomTypeBuilder DeclareMember(CodeTypeMember member) {
            if (!TypeDeclaration.Members.Contains(member)) {
                TypeDeclaration.Members.Add(member);
            }
            return this;
        }

        public CodeDomTypeBuilder EncapsulateField(CodeMemberField field) {
            return EncapsulateField(field, false);
        }

        public CodeDomTypeBuilder EncapsulateField(CodeMemberField field, bool notifyChanges) {
            if (!TypeDeclaration.Members.Contains(field)) {
                TypeDeclaration.Members.Add(field);
            }

            var property = new CodeMemberProperty();
            var name = field.Name;
            if (name.StartsWith("_"))
                property.Name = name.Substring(1);
            else if (name.StartsWith("m_"))
                property.Name = name.Substring(2);
            else if (name.StartsWith("m") && Char.IsUpper(name[1]))
                property.Name = name.Substring(1);
            else
                property.Name = String.Concat(Char.ToLower(name[0]), name.Substring(1));
            property.Type = field.Type;
            property.Attributes = MemberAttributes.Public;

            // Declares a property get statement to return the value of the integer field.
            property.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), field.Name)));

            // Declares a property set statement to set the value to the integer field.
            // The CodePropertySetValueReferenceExpression represents the value argument 
            // passed to the property set statement.
            property.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), field.Name),
                            new CodePropertySetValueReferenceExpression()));

            TypeDeclaration.Members.Add(property);

            return this;
        }

        public CodeMemberMethod FindMethod(string name) {
            foreach (CodeTypeMember member in TypeDeclaration.Members) {
                var method = member as CodeMemberMethod;
                if (method != null && method.Name == name) {
                    return method;
                }
            }
            return null;
        }

        #endregion
    }
}
