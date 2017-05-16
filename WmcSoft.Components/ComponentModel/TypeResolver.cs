using System;
using System.ComponentModel.Design;
using System.Reflection;

namespace WmcSoft.ComponentModel
{
    public sealed class TypeResolver : IDisposable
    {
        #region Private fields

        private readonly ITypeResolutionService _resolutionService;

        #endregion

        #region Lifecycle

        public TypeResolver(ITypeResolutionService resolutionService)
        {
            _resolutionService = resolutionService;
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(OnAssemblyResolve);
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(OnAssemblyResolve);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly;
            AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(this.OnAssemblyResolve);
            try {
                var name = new AssemblyName(args.Name);
                assembly = _resolutionService.GetAssembly(name, false);
            } finally {
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(this.OnAssemblyResolve);
            }
            return assembly;
        }

        #endregion
    }
}
