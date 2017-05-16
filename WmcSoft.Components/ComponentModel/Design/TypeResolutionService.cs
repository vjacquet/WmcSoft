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
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using WmcSoft.Properties;
using WmcSoft.Reflection;

namespace WmcSoft.ComponentModel.Design
{
    public class TypeResolutionService : ITypeResolutionService
    {
        #region Fields

        private readonly Dictionary<string, Assembly> _assemblyCache;

        private readonly string _basePath;
        private readonly ITypeResolutionService _parentService;

        #endregion

        #region Methods

        public TypeResolutionService()
            : this(AppDomain.CurrentDomain.BaseDirectory)
        {
        }

        public TypeResolutionService(string basePath)
        {
            if (String.IsNullOrEmpty(basePath))
                throw new ArgumentNullException("basePath");

            _basePath = Path.GetFullPath(basePath);
            _assemblyCache = new Dictionary<string, Assembly>();
        }

        public TypeResolutionService(string basePath, ITypeResolutionService parentService)
            : this(basePath)
        {
            _parentService = parentService;
        }

        public Assembly GetAssembly(AssemblyName assemblyName)
        {
            return GetAssembly(assemblyName, false);
        }

        public virtual Assembly GetAssembly(AssemblyName assemblyName, bool throwOnError)
        {
            if (assemblyName == null)
                throw new ArgumentNullException("assemblyName");

            Assembly assembly = _assemblyCache[assemblyName.FullName];
            if (assembly == null) {
                try {
                    assembly = Assembly.Load(assemblyName);
                } catch (FileNotFoundException) {
                    string path = Path.Combine(_basePath, assemblyName.Name + ".dll");
                    if (File.Exists(path)) {
                        assembly = Assembly.LoadFrom(path);
                    }
                }
                if ((assembly == null) && (_parentService != null)) {
                    assembly = _parentService.GetAssembly(assemblyName, throwOnError);
                }
                if ((assembly == null) && throwOnError) {
                    throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Resources.AssemblyNotFound, new object[] { assemblyName.FullName }));
                }
                if ((assembly != null) && !_assemblyCache.ContainsKey(assemblyName.FullName)) {
                    _assemblyCache.Add(assemblyName.FullName, assembly);
                }
            }
            return assembly;
        }

        public virtual string GetPathOfAssembly(AssemblyName assemblyName)
        {
            var assembly = GetAssembly(assemblyName);
            if (assembly == null)
                return null;

            var uri = new Uri(assembly.CodeBase);
            if (uri.IsFile) {
                return uri.LocalPath.Replace(@"\\", @"\");
            }
            return assembly.CodeBase;
        }

        public Type GetType(string typeName)
        {
            return GetType(typeName, false, false);
        }

        public Type GetType(string typeName, bool throwOnError)
        {
            return GetType(typeName, throwOnError, false);
        }

        public virtual Type GetType(string typeName, bool throwOnError, bool ignoreCase)
        {
            if (typeName == null)
                throw new ArgumentNullException("typeName");

            Type type = null;
            Exception inner = null;
            if ((typeName != null) && (typeName.Length > 0)) {
                try {
                    type = Type.GetType(typeName.Trim(), throwOnError, false);
                    if (type != null)
                        return type;
                } catch (Exception exception) {
                    inner = exception;
                }
                if (!ReflectionHelper.IsAssemblyQualifiedTypeName(typeName)) {
                    var queue = new Queue<AssemblyName>();
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                        type = assembly.GetType(typeName, false, ignoreCase);
                        if (type != null)
                            return type;
                        foreach (AssemblyName reference in assembly.GetReferencedAssemblies()) {
                            queue.Enqueue(reference);
                        }
                    }
                    while (queue.Count > 0) {
                        var name = queue.Dequeue();
                        var assembly = Assembly.Load(name);
                        if (assembly != null) {
                            type = assembly.GetType(typeName, false, ignoreCase);
                            if (type != null)
                                return type;
                            foreach (AssemblyName reference in assembly.GetReferencedAssemblies()) {
                                queue.Enqueue(reference);
                            }
                        }
                    }
                } else {
                    string assemblyString = ReflectionHelper.GetAssemblyString(typeName);
                    Assembly assembly = null;
                    try {
                        assembly = this.GetAssembly(new AssemblyName(assemblyString), throwOnError);
                    } catch (Exception exception) {
                        inner = exception;
                    }
                    string[] strArray = typeName.Split(new char[] { ',' });
                    if (assembly != null) {
                        type = assembly.GetType(strArray[0].Trim(), throwOnError, ignoreCase);
                        if (type != null)
                            return type;
                    }
                }
            }
            if (_parentService != null) {
                type = _parentService.GetType(typeName, throwOnError, ignoreCase);
            }
            if ((type == null) && throwOnError) {
                throw new TypeLoadException(typeName, inner);
            }
            return type;
        }

        public virtual void ReferenceAssembly(AssemblyName assemblyName)
        {
            _parentService.ReferenceAssembly(assemblyName);
        }

        #endregion

        #region Properties

        public string BasePath {
            get { return _basePath; }
        }

        #endregion
    }
}
