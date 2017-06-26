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
using System.CodeDom;

namespace WmcSoft.CodeBuilders
{
    public class ClassBuilder : NonTerminalCodeBuilder
    {
        protected override CodeBuilder Create(string name)
        {
            switch (name) {
            case "extends":
                return new ExtendsBuilder();
            case "implements":
                return new ImplementsBuilder();
            case "members":
                return new MembersBuilder();
            case "properties":
                return new PropertiesBuilder();
            case "fields":
                return new FieldsBuilder();
            }
            return null;
        }

        protected override void BeforeParsingChildren(IDictionary<string, string> attributes, CodeBuilderContext context)
        {
            base.BeforeParsingChildren(attributes, context);

            string name = attributes["name"];

            CodeTypeDeclaration codeTypeDeclaration = context.BeginTypeDeclaration(name);
            codeTypeDeclaration.IsClass = true;
            codeTypeDeclaration.IsPartial = true;

            if (attributes.ContainsKey("public")) {
                string isPublic = attributes["public"];
                if (isPublic == "false" || isPublic == "0")
                    codeTypeDeclaration.TypeAttributes |= System.Reflection.TypeAttributes.NotPublic;
            }

            if (attributes.ContainsKey("abstract")) {
                string isAbstract = attributes["abstract"];
                if (isAbstract != "false" && isAbstract != "1")
                    codeTypeDeclaration.TypeAttributes |= System.Reflection.TypeAttributes.Abstract;
            }
        }

        protected override void AfterParsingChildren(IDictionary<string, string> attributes, CodeBuilderContext context)
        {
            context.EndTypeDeclaration();
            attributes.Remove("name");
            attributes.Remove("abstract");
            attributes.Remove("public");

            base.AfterParsingChildren(attributes, context);
        }
    }
}
