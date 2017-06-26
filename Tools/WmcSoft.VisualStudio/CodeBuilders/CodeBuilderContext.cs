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
using WmcSoft.Reflection;
using System.Reflection;

namespace WmcSoft.CodeBuilders
{
    public class CodeBuilderContext : IServiceProvider
    {
        const string NamespaceURI = "http://www.wmcsoft.com/schema/CodeBuilder.xsd";

        #region Private fields

        CodeCompileUnit codeCompileUnit;
        Stack<CodeNamespace> codeNamespaces;
        Stack<CodeTypeDeclaration> codeTypeDeclarations;
        Dictionary<string, CodeTypeReference> codeTypeReferences;
        Dictionary<string, PolicyBuilder> policies;
        PolicyBuilder currentPolicy;
        IServiceProvider serviceProvider;
        DocumentationLoader documentationLoader;

        #endregion

        #region Lifecycle

        public CodeBuilderContext(IServiceProvider serviceProvider)
        {
            documentationLoader = null;
            this.serviceProvider = serviceProvider;
            codeNamespaces = new Stack<CodeNamespace>();
            codeTypeDeclarations = new Stack<CodeTypeDeclaration>();
            codeTypeReferences = new Dictionary<string, CodeTypeReference>();
            policies = new Dictionary<string, PolicyBuilder>();
        }

        #endregion

        #region Methods

        public CodeCompileUnit BeginCompileUnit()
        {
            codeCompileUnit = new CodeCompileUnit();
            return codeCompileUnit;
        }

        public void EndCompileUnit()
        {
            while (codeNamespaces.Count > 0 && codeCompileUnit.Namespaces.Contains(codeNamespaces.Peek()))
                EndNamespace();
        }

        public CodeNamespace BeginNamespace(string namespaceName)
        {
            CodeNamespace codeNamespace = new CodeNamespace(namespaceName);
            codeCompileUnit.Namespaces.Add(codeNamespace);
            this.codeNamespaces.Push(codeNamespace);
            return codeNamespace;
        }

        public void EndNamespace()
        {
            if (codeNamespaces.Count == 0)
                return;

            CodeNamespace codeNamespace = codeNamespaces.Peek();
            while (codeTypeDeclarations.Count > 0 && codeNamespace.Types.Contains(codeTypeDeclarations.Peek()))
                EndTypeDeclaration();
            codeNamespaces.Pop();
        }

        public CodeTypeDeclaration BeginTypeDeclaration(string typeName)
        {
            CodeNamespace codeNamespace = CurrentNamespace;
            string fullname = codeNamespace.Name + "." + typeName;

            CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(typeName);
            codeNamespace.Types.Add(codeTypeDeclaration);
            codeTypeReferences.Add(fullname, new CodeTypeReference(fullname));
            this.codeTypeDeclarations.Push(codeTypeDeclaration);
            return codeTypeDeclaration;
        }

        public void EndTypeDeclaration()
        {
            codeTypeDeclarations.Pop();
        }

        public CodeTypeReference GetTypeReference(string typeName)
        {
            CodeTypeReference result;
            if (!codeTypeReferences.TryGetValue(typeName, out result)) {
                result = new CodeTypeReference(typeName);
                codeTypeReferences.Add(typeName, result);
            }
            return result;
        }

        public void RegisterPolicy(string name, PolicyBuilder policy)
        {
            policies.Add(name, policy);
        }

        public void BeginPolicy(string policy)
        {
            currentPolicy = null;
            policies.TryGetValue(policy, out currentPolicy);
        }

        public void EndPolicy()
        {
            currentPolicy = null;
        }

        public void ApplyCurrentPolicyRules(CodeTypeMember codeTypeMember)
        {
            if (currentPolicy != null)
                currentPolicy.ApplyRules(codeTypeMember, this);
        }

        public XmlDocumentation GetTypeDocumentation(Type type)
        {
            return Loader.GetTypeDocumentation(type);
        }

        public XmlDocumentation GetMemberDocumentation(MemberInfo memberInfo)
        {
            return Loader.GetMemberDocumentation(memberInfo);
        }

        protected DocumentationLoader Loader {
            get {
                if (documentationLoader == null)
                    documentationLoader = new DocumentationLoader();
                return documentationLoader;
            }
        }

        #endregion

        #region properties

        public CodeCompileUnit CurrentCompileUnit {
            get {
                return codeCompileUnit;
            }
        }

        public CodeNamespace CurrentNamespace {
            get {
                return codeNamespaces.Peek();
            }
        }

        public CodeTypeDeclaration CurrentTypeDeclaration {
            get {
                return codeTypeDeclarations.Peek();
            }
        }

        public PolicyBuilder CurrentPolicy {
            get {
                return currentPolicy;
            }
        }

        #endregion

        #region IServiceProvider Membres

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(CodeBuilderContext))
                return this;
            return serviceProvider.GetService(serviceType);
        }

        #endregion
    }

}
