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
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Reflection;

namespace WmcSoft.ComponentModel.Design
{
    public class AliasResolutionService : TypeResolutionService
    {
        #region Fields

        private readonly ITypeResolutionService _parentLoader;
        private readonly IDictionary _typeAliases;

        #endregion

        #region Lifecycle

        static string ResolveBasePath(TypeResolutionService typeResolutionService)
        {
            if (typeResolutionService != null)
                return typeResolutionService.BasePath;
            return typeof(AliasResolutionService).Assembly.CodeBase.Replace(@"\\", @"\");
        }

        public AliasResolutionService(IDictionary aliases, ITypeResolutionService parentLoader)
            : base(ResolveBasePath(parentLoader as TypeResolutionService))
        {
            if (parentLoader == null)
                throw new ArgumentNullException("parentLoader");

            _typeAliases = aliases ?? new HybridDictionary();
            _parentLoader = parentLoader;
        }

        #endregion

        #region Overrides

        public override Assembly GetAssembly(AssemblyName assemblyName, bool throwOnError)
        {
            return _parentLoader.GetAssembly(assemblyName, throwOnError);
        }

        public override string GetPathOfAssembly(AssemblyName assemblyName)
        {
            return _parentLoader.GetPathOfAssembly(assemblyName);
        }

        public override Type GetType(string typeName, bool throwOnError, bool ignoreCase)
        {
            if (typeName == null)
                throw new ArgumentNullException("typeName");

            object alias = _typeAliases[typeName];
            if (alias == null)
                // not found
                return _parentLoader.GetType(typeName, throwOnError, ignoreCase);

            var type = alias as Type;
            if (type != null)
                // already resolved
                return type;

            // update dictionary but only if resolved
            type = _parentLoader.GetType(alias.ToString(), throwOnError, ignoreCase);
            if (type != null)
                _typeAliases[typeName] = type;
            return type;
        }

        public override void ReferenceAssembly(AssemblyName assemblyName)
        {
            _parentLoader.ReferenceAssembly(assemblyName);
        }

        #endregion
    }
}
