﻿#region Licence

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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using WmcSoft.CodeDom;

namespace WmcSoft.ComponentModel.Design.Serialization
{
    public static class DesignerSerializationManagerExtensions
    {
        public static IEnumerable<string> GetComponentsOnDesignerSurface(IDesignerSerializationManager manager)
        {
            var designerHost = manager.GetService<IDesignerHost>();
            var rootComponent = designerHost.RootComponent;
            foreach (IComponent component in designerHost.Container.Components) {
                if (component != rootComponent) {
                    yield return component.Site.Name;
                }
            }
        }

        public static CodeMemberMethod FindInitializeComponent(this CodeTypeDeclaration declaration)
        {
            return declaration.FindMember<CodeMemberMethod>("InitializeComponent");
        }

        public static void AddStatementToInitializeComponent(this CodeTypeDeclaration declaration, CodeStatement statement)
        {
            var method = FindInitializeComponent(declaration);
            if (method != null) {
                method.Statements.Add(statement);
            }
        }

        public static void RemoveFromInitializeComponent<T>(this CodeTypeDeclaration declaration, Predicate<T> shouldRemove) where T : CodeObject
        {
            var method = FindInitializeComponent(declaration);
            if (method != null) {
                method.Statements.RemoveFromStatements<T>(shouldRemove);
            }
        }
    }
}
