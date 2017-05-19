using System;
using System.Runtime.InteropServices;

namespace WmcSoft.Interop
{
    [
        ComImport,
        Guid("FC4801A3-2BA9-11CF-A229-00AA003D7352"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)
    ]
    public interface IObjectWithSite
    {
        /// <summary>
        /// Provides the site's IUnknown pointer to the object.
        /// </summary>
        /// <param name="pUnkSite">Address of an interface pointer to the site managing this object. 
        /// If null, the object should call IUnknown::Release to release the existing site.</param>
        /// <remarks>The object should hold onto the IUnknown pointer,
        /// calling AddRef in doing so. If the object already has a site, 
        /// it should first call pUnkSit.AddRef to secure the new site, call 
        /// IUnknown.Release on the existing site, and then save pUnkSite.</remarks>
        void SetSite([MarshalAs(UnmanagedType.Interface)] object pUnkSite);

        /// <summary>
        /// Retrieves the last site set with IObjectWithSite::SetSite. If there's no known site, the object returns a failure code.
        /// </summary>
        /// <param name="riid">The IID of the interface pointer that should be returned in ppvSite.</param>
        /// <param name="ppvSite">The interface pointer.</param>
        void GetSite([In] ref Guid riid, [Out, MarshalAs(UnmanagedType.LPArray)] object[] ppvSite);
    }
}
