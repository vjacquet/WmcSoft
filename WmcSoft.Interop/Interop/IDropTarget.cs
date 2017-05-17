using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

using HRESULT = System.Int32;

namespace WmcSoft.Interop
{
    [Flags]
    public enum DropEffect
    {
        None = 0,
        Copy = 1,
        Move = 2,
        Link = 4,
        Scroll = -2147483648, //0x80000000U,
    }

    [ComImport, Guid("00000122-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDropTarget
    {
        [PreserveSig]
        HRESULT DragEnter(
            [In, MarshalAs(UnmanagedType.Interface)] IDataObject pDataObj,
            [In, MarshalAs(UnmanagedType.U4)] int grfKeyState,
            [In, MarshalAs(UnmanagedType.U8)] POINTL pt,
            [In, Out] ref int pdwEffect);

        [PreserveSig]
        HRESULT DragOver(
            [In, MarshalAs(UnmanagedType.U4)] int grfKeyState,
            [In, MarshalAs(UnmanagedType.U8)] POINTL pt,
            [In, Out, MarshalAs(UnmanagedType.I4)] ref DropEffect pdwEffect);

        [PreserveSig]
        HRESULT DragLeave();

        [PreserveSig]
        HRESULT Drop(
            [In, MarshalAs(UnmanagedType.Interface)] IDataObject pDataObj,
            [In, MarshalAs(UnmanagedType.U4)] int grfKeyState,
            [In, MarshalAs(UnmanagedType.U8)] POINTL pt,
            [In, Out, MarshalAs(UnmanagedType.I4)] ref DropEffect pdwEffect);
    }


}
