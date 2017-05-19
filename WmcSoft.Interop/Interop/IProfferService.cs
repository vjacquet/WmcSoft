using System;
using System.Runtime.InteropServices;

namespace WmcSoft.Interop
{
    /// <remarks/>
    [
        ComImport,
        ComVisible(true),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        Guid("CB728B20-F786-11CE-92AD-00AA00A74CD0")
    ]
    public interface IProfferService
    {
        /// <remarks/>
        int ProfferService(
            [In] ref Guid rguidService,
            [In, MarshalAs(UnmanagedType.Interface)] IServiceProvider psp,
            [Out, MarshalAs(UnmanagedType.U4)] out uint pdwCookie);
        /// <remarks/>
        void RevokeService(
            [In, MarshalAs(UnmanagedType.U4)] uint dwCookie);
    }
}
