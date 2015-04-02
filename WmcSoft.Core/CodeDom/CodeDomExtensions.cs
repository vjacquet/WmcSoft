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
using System.Collections.Generic;

namespace WmcSoft.CodeDom
{
    public static class CodeDomExtensions
    {
        public static IEnumerable<T> FindCode<T>(this CodeStatementCollection statements, Predicate<T> matchesFilter) where T : CodeObject {
            foreach (CodeStatement statement in statements) {
                T expression = statement as T;
                var expressionStatement = statement as CodeExpressionStatement;
                if ((expression == null) && (expressionStatement != null)) {
                    expression = expressionStatement.Expression as T;
                }
                if ((expression != null) && matchesFilter(expression)) {
                    yield return expression;
                }
            }
        }

        public static T FindFirstCode<T>(this CodeStatementCollection statements, Predicate<T> matchesFilter) where T : CodeObject {
            foreach (CodeStatement statement in statements) {
                T expression = statement as T;
                var expressionStatement = statement as CodeExpressionStatement;
                if ((expression == null) && (expressionStatement != null)) {
                    expression = expressionStatement.Expression as T;
                }
                if ((expression != null) && matchesFilter(expression)) {
                    return expression;
                }
            }
            return default(T);
        }

        public static T FindMember<T>(this CodeTypeDeclaration declaration, string memberName) where T : CodeObject {
            foreach (CodeTypeMember member in declaration.Members) {
                T local = member as T;
                if ((local != null) && (member.Name == memberName)) {
                    return local;
                }
            }
            return default(T);
        }

        public static void RemoveFromStatements<T>(this CodeStatementCollection statements, Predicate<T> shouldRemove) where T : CodeObject {
            var list = new List<CodeStatement>();
            foreach (CodeStatement statement in statements) {
                T expression = statement as T;
                var expressionStatement = statement as CodeExpressionStatement;
                if ((expression == null) && (expressionStatement != null)) {
                    expression = expressionStatement.Expression as T;
                }
                if ((expression != null) && shouldRemove(expression)) {
                    list.Add(statement);
                }
            }
            foreach (CodeStatement statement in list) {
                statements.Remove(statement);
            }
        }
    }

}
