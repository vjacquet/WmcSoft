using System;
using System.ComponentModel.Design;
using System.Reflection;

namespace WmcSoft.ComponentModel
{
    public sealed class TypeResolver : IDisposable
    {
        #region Private fields

        private readonly ITypeResolutionService resolutionService;

        #endregion

        #region Lifecycle

        public TypeResolver(ITypeResolutionService resolutionService)
        {
            this.resolutionService = resolutionService;
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        ~TypeResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
            try {
                var name = new AssemblyName(args.Name);
                return resolutionService.GetAssembly(name, false);
            } finally {
                AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            }
        }

        #endregion
    }
}
