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
using System.Linq;
using System.Collections.Generic;

namespace WmcSoft.CodeDom
{
    public static class CodeDomExtensions
    {
        #region FindCode

        public static IEnumerable<T> FindCode<T>(this CodeStatementCollection statements, Predicate<T> matchesFilter)
            where T : CodeObject {
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

        public static T FindFirstCode<T>(this CodeStatementCollection statements, Predicate<T> matchesFilter)
            where T : CodeObject {
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

        public static T FindMember<T>(this CodeTypeDeclaration declaration, string memberName)
            where T : CodeObject {
            foreach (CodeTypeMember member in declaration.Members) {
                T local = member as T;
                if ((local != null) && (member.Name == memberName)) {
                    return local;
                }
            }
            return default(T);
        }

        #endregion

        #region RemoveFromStatements

        public static void RemoveFromStatements<T>(this CodeStatementCollection statements, Predicate<T> shouldRemove)
            where T : CodeObject {
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

        #endregion

        #region Comments

        public static T Comment<T>(this T member, params CodeCommentStatement[] comments)
            where T : CodeTypeMember {
            member.Comments.AddRange(comments);
            return member;
        }

        public static T Comment<T>(this T member, params string[] comments)
            where T : CodeTypeMember {
            member.Comments.AddRange(comments.Select((c) => new CodeCommentStatement(c)).ToArray());
            return member;
        }

        public static T Document<T>(this T member, params string[] comments)
            where T : CodeTypeMember {
            member.Comments.AddRange(comments.Select((c) => new CodeCommentStatement(c, true)).ToArray());
            return member;
        }

        #endregion

        #region CodeStatement

        public static T StartRegion<T>(this T self, string title)
            where T : CodeStatement {
            self.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, title));
            return self;
        }

        public static T EndRegion<T>(this T self)
            where T : CodeStatement {
            self.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));
            return self;
        }

        public static T SurroundWithRegion<T>(this T self, string title)
            where T : CodeStatement {
            self.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, title));
            self.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));
            return self;
        }

        public static T Line<T>(this T self, string file, int line)
            where T : CodeStatement {
            self.LinePragma = new CodeLinePragma(file, line);
            return self;
        }

        #endregion

        #region CodeTypeMember

        //public static T StartRegion<T>(this T self, string title)
        //    where T : CodeTypeMember {
        //    self.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, title));
        //    return self;
        //}

        //public static T EndRegion<T>(this T self)
        //    where T : CodeTypeMember {
        //    self.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));
        //    return self;
        //}

        //public static T SurroundWithRegion<T>(this T self, string title)
        //    where T : CodeTypeMember {
        //    self.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, title));
        //    self.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));
        //    return self;
        //}

        //public static T Line<T>(this T self, string file, int line)
        //    where T : CodeTypeMember {
        //    self.LinePragma = new CodeLinePragma(file, line);
        //    return self;
        //}

        #endregion

        #region CodeMemberField

        public static CodeFieldReferenceExpression Reference(CodeExpression targetObject, CodeMemberField field) {
            return new CodeFieldReferenceExpression(targetObject, field.Name);
        }

        public static CodeFieldReferenceExpression Reference(CodeMemberField field) {
            return Reference(new CodeThisReferenceExpression(), field);
        }

        #endregion

        #region Property

        public static CodeMemberProperty Implements(this CodeMemberProperty property, CodeTypeReference type) {
            if ((property.Attributes & MemberAttributes.Private) == MemberAttributes.Private)
                property.PrivateImplementationType = type;
            else
                property.ImplementationTypes.Add(type);
            return property;
        }

        public static CodeMemberProperty Implements(this CodeMemberProperty property, Type type) {
            return Implements(property, new CodeTypeReference(type));
        }

        public static CodeMemberProperty Implements<T>(this CodeMemberProperty property) {
            return Implements(property, new CodeTypeReference(typeof(T)));
        }

        public static CodeMemberProperty Implements(this CodeMemberProperty property, string type) {
            return Implements(property, new CodeTypeReference(type));
        }

        public static CodeMemberProperty AddProperty(this CodeTypeDeclaration typeDeclaration, CodeTypeReference type, string name) {
            var property = new CodeMemberProperty();
            property.Name = name;
            property.Type = type;
            typeDeclaration.Members.Add(property);
            return property;
        }

        public static CodeMemberProperty AddProperty(this CodeTypeDeclaration typeDeclaration, Type type, string name) {
            return AddProperty(typeDeclaration, new CodeTypeReference(type), name);
        }

        public static CodeMemberProperty AddProperty<T>(this CodeTypeDeclaration typeDeclaration, string name) {
            return AddProperty(typeDeclaration, new CodeTypeReference(typeof(T)), name);
        }

        public static CodeMemberProperty Encapsulate(this CodeTypeDeclaration typeDeclaration, CodeMemberField field) {
            return Encapsulate(typeDeclaration, field, false);
        }

        public static CodeMemberProperty Encapsulate(this CodeTypeDeclaration typeDeclaration, CodeMemberField field, bool notifyChanges) {
            if (!typeDeclaration.Members.Contains(field)) {
                typeDeclaration.Members.Add(field);
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
                property.Name = Char.ToLower(name[0]) + name.Substring(1);
            property.Type = field.Type;
            property.Attributes = MemberAttributes.Public;

            // Declares a property get statement to return the value of the integer field.
            property.GetStatements.Add(new CodeMethodReturnStatement(Reference(field)));

            // Declares a property set statement to set the value to the integer field.
            // The CodePropertySetValueReferenceExpression represents the value argument 
            // passed to the property set statement.
            property.SetStatements.Add(new CodeAssignStatement(Reference(field), new CodePropertySetValueReferenceExpression()));

            typeDeclaration.Members.Add(property);

            return property;
        }

        public static CodeMemberMethod FindInitializeComponent(this CodeTypeDeclaration declaration) {
            return FindMember<CodeMemberMethod>(declaration, "InitializeComponent");
        }

        public static void AddStatementToInitializeComponent(this CodeTypeDeclaration declaration, CodeStatement statement) {
            var init = FindInitializeComponent(declaration);
            if (init != null) {
                init.Statements.Add(statement);
            }
        }

        public static void Remove<T>(this CodeStatementCollection statements, Predicate<T> shouldRemove)
            where T : CodeObject {
            var toRemove = new List<CodeStatement>();
            foreach (CodeStatement statement in statements) {
                var typedStatement = statement as T;
                var expression = statement as CodeExpressionStatement;
                if (typedStatement == null && expression != null) {
                    typedStatement = expression.Expression as T;
                }
                if (typedStatement != null && shouldRemove(typedStatement)) {
                    toRemove.Add(statement);
                }
            }

            foreach (CodeStatement statement in toRemove) {
                statements.Remove(statement);
            }
        }

        public static void RemoveFromInitializeComponent<T>(this CodeTypeDeclaration declaration, Predicate<T> shouldRemove)
            where T : CodeObject {
            var initialize = FindInitializeComponent(declaration);
            if (initialize != null) {
                Remove(initialize.Statements, shouldRemove);
            }
        }

        #endregion

        #region Methods

        public static CodeMemberMethod Implements(this CodeMemberMethod self, CodeTypeReference type) {
            if ((self.Attributes & MemberAttributes.Private) == MemberAttributes.Private)
                self.PrivateImplementationType = type;
            else
                self.ImplementationTypes.Add(type);
            return self;
        }

        public static CodeMemberMethod Implements(this CodeMemberMethod self, Type type) {
            return Implements(self, new CodeTypeReference(type));
        }

        public static CodeMemberMethod Implements<T>(this CodeMemberMethod self) {
            return Implements(self, new CodeTypeReference(typeof(T)));
        }

        public static CodeMemberMethod Implements(this CodeMemberMethod self, string type) {
            return Implements(self, new CodeTypeReference(type));
        }

        public static CodeMemberMethod Generic(this CodeMemberMethod self, string paramName) {
            self.TypeParameters.Add(paramName);
            return self;
        }

        public static CodeMemberMethod Generic(this CodeMemberMethod self, string paramName, params string[] constraints) {
            return Generic(self, paramName, constraints.Select((t) => new CodeTypeReference(t)).ToArray());
        }

        public static CodeMemberMethod Generic(this CodeMemberMethod self, string paramName, params Type[] constraints) {
            return Generic(self, paramName, constraints.Select((t) => new CodeTypeReference(t)).ToArray());
        }

        public static CodeMemberMethod Generic(this CodeMemberMethod method, string paramName, params CodeTypeReference[] constraints) {
            var p = new CodeTypeParameter(paramName);
            p.Constraints.AddRange(constraints);
            method.TypeParameters.Add(p);
            return method;
        }

        public static CodeMemberMethod Generic(this CodeMemberMethod self, string paramName, bool hasConstructor, params Type[] constraints) {
            return Generic(self, paramName, hasConstructor, constraints.Select((t) => new CodeTypeReference(t)).ToArray());
        }

        public static CodeMemberMethod Generic(this CodeMemberMethod self, string paramName, bool hasConstructor, params string[] constraints) {
            return Generic(self, paramName, hasConstructor, constraints.Select((t) => new CodeTypeReference(t)).ToArray());
        }

        public static CodeMemberMethod Generic(this CodeMemberMethod method, string paramName, bool hasConstructor, params CodeTypeReference[] constraints) {
            var p = new CodeTypeParameter(paramName) { HasConstructorConstraint = hasConstructor };
            p.Constraints.AddRange(constraints);
            method.TypeParameters.Add(p);
            return method;
        }

        #endregion

        #region Events

        public static CodeMemberEvent Implements(this CodeMemberEvent self, CodeTypeReference type) {
            if ((self.Attributes & MemberAttributes.Private) == MemberAttributes.Private)
                self.PrivateImplementationType = type;
            else
                self.ImplementationTypes.Add(type);
            return self;
        }

        public static CodeMemberEvent Implements(this CodeMemberEvent self, Type type) {
            return Implements(self, new CodeTypeReference(type));
        }

        public static CodeMemberEvent Implements<T>(this CodeMemberEvent self) {
            return Implements(self, new CodeTypeReference(typeof(T)));
        }

        public static CodeMemberEvent Implements(this CodeMemberEvent self, string type) {
            return Implements(self, new CodeTypeReference(type));
        }

        #endregion

        #region Exceptions

        public static CodeTryCatchFinallyStatement Try(this CodeTryCatchFinallyStatement self, params CodeStatement[] statements) {
            self.TryStatements.AddRange(statements);
            return self;
        }

        public static CodeTryCatchFinallyStatement Catch(this CodeTryCatchFinallyStatement self, CodeTypeReference exceptionType, string localName, params CodeStatement[] statements) {
            self.CatchClauses.Add(new CodeCatchClause(localName, exceptionType, statements));
            return self;
        }

        public static CodeTryCatchFinallyStatement Catch(this CodeTryCatchFinallyStatement self, Type exceptionType, string localName, params CodeStatement[] statements) {
            self.CatchClauses.Add(new CodeCatchClause(localName, new CodeTypeReference(exceptionType), statements));
            return self;
        }

        public static CodeTryCatchFinallyStatement Catch<T>(this CodeTryCatchFinallyStatement self, string localName, params CodeStatement[] statements) where T : System.Exception {
            self.CatchClauses.Add(new CodeCatchClause(localName, new CodeTypeReference(typeof(T)), statements));
            return self;
        }

        public static CodeTryCatchFinallyStatement Finally(this CodeTryCatchFinallyStatement self, params CodeStatement[] statements) {
            self.FinallyStatements.AddRange(statements);
            return self;
        }

        #endregion
    }
}
