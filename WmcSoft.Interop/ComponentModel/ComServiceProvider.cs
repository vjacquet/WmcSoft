using System;
using System.Runtime.InteropServices;

using WmcSoft.Interop;

using IServiceProvider = System.IServiceProvider;
using IOleServiceProvider = WmcSoft.Interop.IServiceProvider;

namespace WmcSoft.ComponentModel
{
    /// <summary>
    /// This wraps the IOleServiceProvider interface and provides an easy COM+ way to get at
    /// services.
    /// </summary>
    public class ComServiceProvider : IServiceProvider, IObjectWithSite, IDisposable
    {
        private IOleServiceProvider serviceProvider;

        /// <summary>
        /// Creates a new ServiceProvider object and uses the given interface to resolve
        /// services.
        /// </summary>
        /// <param name='sp'>The IOleServiceProvider interface to use.</param>
        public ComServiceProvider(object sp)
        {
            serviceProvider = sp as IOleServiceProvider;
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name='serviceType'>An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type serviceType. -or- a null reference if there is no service object of type serviceType.</returns>
        public virtual object GetService(Type serviceType)
        {
            if (serviceType == null) {
                return null;
            }
            return GetService(serviceType.GUID, serviceType);
        }

        /// <summary>
        ///     Retrieves the requested service.
        /// </summary>
        /// <param name='guid'>
        ///     The GUID of the service to retrieve.
        /// </param>
        /// <returns>
        ///     an instance of the service or null if no
        ///     such service exists.
        /// </returns>
        public virtual object GetService(Guid guid)
        {
            return GetService(guid, null);
        }

        /// <summary>
        ///     Retrieves the requested service.  The guid must be specified; the class is only
        ///     used when debugging and it may be null.
        /// </summary>
        private object GetService(Guid guid, Type serviceType)
        {
            // Valid, but wierd for caller to init us with a NULL sp
            if (serviceProvider == null) {
                return null;
            }

            // No valid guid on the passed in class, so there is no service for it.
            if (guid.Equals(Guid.Empty)) {
                return null;
            }

            // We provide a couple of services of our own.
            if (guid.Equals(typeof(IOleServiceProvider).GUID)) {
                return serviceProvider;
            }
            if (guid.Equals(typeof(IObjectWithSite).GUID)) {
                return (IObjectWithSite)this;
            }

            IntPtr pUnk;
            Guid iid = Helpers.IID_IUnknown;
            int hr = serviceProvider.QueryService(ref guid, ref iid, out pUnk);

            if (Helpers.Succeeded(hr) && (pUnk != IntPtr.Zero)) {
                object service = Marshal.GetObjectForIUnknown(pUnk);
                Marshal.Release(pUnk);
                return service;
            }

            return null;
        }

        /// <summary>
        /// Retrieves the current site object we're using to resolve services.
        /// </summary>
        /// <param name='riid'>Must be IServiceProvider.class.GUID</param>
        /// <param name='ppvSite'>Outparam that will contain the site object.</param>
        /// <seealso cref='IObjectWithSite'/>
        void IObjectWithSite.GetSite(ref Guid riid, object[] ppvSite)
        {
            ppvSite[0] = GetService(riid);
        }

        /// <summary>
        /// Sets the site object we will be using to resolve services.
        /// </summary>
        /// <param name='pUnkSite'>
        /// The site we will use.  This site will only be used if it also 
        /// implements IOleServiceProvider.
        /// </param>
        /// <seealso cref='IObjectWithSite'/>
        void IObjectWithSite.SetSite(object pUnkSite)
        {
            if (pUnkSite is IOleServiceProvider) {
                serviceProvider = (IOleServiceProvider)pUnkSite;
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            serviceProvider = null;
        }

        ~ComServiceProvider()
        {
            Dispose(false);
        }
        #endregion
    }
}
