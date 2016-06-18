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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace WmcSoft.Interop
{
    public static class Shell
    {
        [DllImport("shell32.dll")]
        [PreserveSig]
        internal static extern int SHGetMalloc(out IMalloc ppMalloc);

        [DllImport("shell32.dll")]
        [PreserveSig]
        internal static extern int SHGetDesktopFolder(ref IShellFolder ppshf);


        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool CreateHardLink(string lpFileName, string lpExistingFileName,
           IntPtr lpSecurityAttributes);

        public static Image CreateThumbnail(string fileName) {
            return CreateThumbnail(fileName, new Size(96, 96));
        }

        public static Image CreateThumbnail(string fileName, Size size) {
            IMalloc pMalloc;
            IntPtr ppMalloc = IntPtr.Zero;

            Shell.SHGetMalloc(out pMalloc);
            /*System.Type mallocType = typeof(IMalloc);
            object obj = Marshal.GetTypedObjectForIUnknown(ppMalloc, typeof(IMalloc));
            pMalloc = (IMalloc)obj;*/

            //pMalloc.Free(pv);
            IntPtr pidlURL = IntPtr.Zero;
            IntPtr pidlWorkDir = IntPtr.Zero;
            int dwPriority = 0;
            int dwFlags = 0x0004;//IEIFLAG_ASPECT;
            int hr;
            SIZE nsize = new SIZE(size.Width, size.Height);
            StringBuilder szBuffer = new StringBuilder(260);
            IExtractImage peiURL = null;
            IntPtr ieiURL = IntPtr.Zero;
            IntPtr isfWorkDir = IntPtr.Zero;
            IShellFolder psfWorkDir = null;
            IShellFolder psfDesktop = null;
            IntPtr pThumbnail = IntPtr.Zero;

            IntPtr zero = IntPtr.Zero;
            uint attributes = 0;
            Guid IID_IShellFolder = typeof(IShellFolder).GUID;
            Guid IID_IExtractImage = typeof(IExtractImage).GUID;

            uint cchEaten;
            hr = Shell.SHGetDesktopFolder(ref psfDesktop);
            hr = psfDesktop.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, System.IO.Path.GetDirectoryName(fileName), out cchEaten, out pidlWorkDir, ref attributes);
            hr = psfDesktop.BindToObject(pidlWorkDir, IntPtr.Zero, ref IID_IShellFolder, out isfWorkDir);
            psfWorkDir = Marshal.GetObjectForIUnknown(isfWorkDir) as IShellFolder;
            hr = psfWorkDir.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, System.IO.Path.GetFileName(fileName), out cchEaten, out pidlURL, ref attributes);
            hr = psfWorkDir.GetUIObjectOf(IntPtr.Zero, 1, new IntPtr[] { pidlURL }, ref IID_IExtractImage, 0, out ieiURL);
            if (ieiURL != IntPtr.Zero) {
                peiURL = Marshal.GetObjectForIUnknown(ieiURL) as IExtractImage;

                dwFlags = 0x0004;
                hr = peiURL.GetLocation(szBuffer, 260, ref dwPriority, ref nsize, 16, ref dwFlags);
                hr = peiURL.Extract(out pThumbnail);

                Marshal.ReleaseComObject(peiURL);
            }
            pMalloc.Free(pidlWorkDir);
            pMalloc.Free(pidlURL);
            Marshal.ReleaseComObject(psfDesktop);
            Marshal.ReleaseComObject(psfWorkDir);

            if (pThumbnail == IntPtr.Zero)
                return null;

            return Image.FromHbitmap(pThumbnail);
        }
    }

    #region Interfaces

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000002-0000-0000-C000-000000000046")]
    internal interface IMalloc
    {
        /// <summary>
        /// Allocate a block of memory
        /// </summary>
        /// <param name="cb">Size, in bytes, of the memory block to be allocated.</param>
        /// <returns>a pointer to the allocated memory block.</returns>
        [PreserveSig]
        IntPtr Alloc(UInt32 cb);

        /// <summary>
        /// Changes the size of a previously allocated memory block.
        /// </summary>
        /// <param name="pv">Pointer to the memory block to be reallocated</param>
        /// <param name="cb">Size of the memory block, in bytes, to be reallocated.</param>
        /// <returns>reallocated memory block</returns>
        [PreserveSig]
        IntPtr Realloc(IntPtr pv, UInt32 cb);

        // 
        /// <summary>
        /// Free a previously allocated block of memory.
        /// </summary>
        /// <param name="pv"> Pointer to the memory block to be freed.</param>
        [PreserveSig]
        void Free(IntPtr pv);

        /// <summary>
        /// This method returns the size, in bytes, of a memory block
        /// previously allocated with IMalloc::Alloc or IMalloc::Realloc.
        /// </summary>
        /// <param name="pv">Pointer to the memory block for which the size is requested.</param>
        /// <returns>The size of the allocated memory block in bytes.</returns>
        [PreserveSig]
        UInt32 GetSize(IntPtr pv);

        /// <summary>
        /// This method determines whether this allocator was used to allocate the specified block of memory.
        /// </summary>
        /// <param name="pv">Pointer to the memory block</param>
        /// <returns>1 - allocated 0 - not allocated by this IMalloc Instance.</returns>
        [PreserveSig]
        Int16 DidAlloc(IntPtr pv);

        /// <summary>
        /// Minimizes the heap by releasing unused memory to the operating system.
        /// </summary>
        [PreserveSig]
        void HeapMinimize();
    }

    /// <summary>
    ///  managed equivalent of IShellFolder interface
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214E6-0000-0000-C000-000000000046")]
    internal interface IShellFolder
    {
        int ParseDisplayName(IntPtr hwnd,
            IntPtr pbc,
            string pszDisplayName,
            out UInt32 pchEaten,
            out IntPtr ppidl,
            ref UInt32 pdwAttributes);

        int EnumObjects(IntPtr hwnd, ESHCONTF grfFlags, out IntPtr ppenumIDList);

        int BindToObject(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IntPtr ppv);

        int BindToStorage(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IntPtr ppv);

        [PreserveSig]
        Int32 CompareIDs(Int32 lParam, IntPtr pidl1, IntPtr pidl2);

        int CreateViewObject(IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv);

        /* this version is good if cidl is one
         * void GetAttributesOf(UInt32 cidl, ref IntPtr apidl, ref ESFGAO rgfInOut);
         */
        int GetAttributesOf(UInt32 cidl, 
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] apidl,
            ref ESFGAO rgfInOut);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwndOwner"></param>
        /// <param name="cidl">number of IntPtr's in incoming array</param>
        /// <param name="apidl"></param>
        /// <param name="riid"></param>
        /// <param name="rgfReserved"></param>
        /// <param name="ppv"></param>
        /// <returns></returns>
        [PreserveSig]
        int GetUIObjectOf(IntPtr hwndOwner,
            UInt32 cidl,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IntPtr[] apidl,
            [In] ref Guid riid,
            UInt32 rgfReserved,
            out IntPtr ppv);

        /*    this version is good if cidl is one
              void GetUIObjectOf(
              IntPtr hwndOwner,
              UInt32 cidl,
              ref    IntPtr apidl,
              [In] ref Guid riid,
              UInt32 rgfReserved,
              out IntPtr ppv);
           */

        int GetDisplayNameOf(IntPtr pidl, ESHGDN uFlags, out STRRET pName);

        int SetNameOf(IntPtr hwnd,
            IntPtr pidl,
            string pszName,
            ESHCONTF uFlags,
            out IntPtr ppidlOut);
    }

    // from ShObjIdl.h
    internal enum ESFGAO : uint
    {
        SFGAO_CANCOPY = 0x00000001,
        SFGAO_CANMOVE = 0x00000002,
        SFGAO_CANLINK = 0x00000004,
        SFGAO_LINK = 0x00010000,
        SFGAO_SHARE = 0x00020000,
        SFGAO_READONLY = 0x00040000,
        SFGAO_HIDDEN = 0x00080000,
        SFGAO_FOLDER = 0x20000000,
        SFGAO_FILESYSTEM = 0x40000000,
        SFGAO_HASSUBFOLDER = 0x80000000,
    }

    internal enum ESHCONTF
    {
        SHCONTF_FOLDERS = 0x0020,
        SHCONTF_NONFOLDERS = 0x0040,
        SHCONTF_INCLUDEHIDDEN = 0x0080,
        SHCONTF_INIT_ON_FIRST_NEXT = 0x0100,
        SHCONTF_NETPRINTERSRCH = 0x0200,
        SHCONTF_SHAREABLE = 0x0400,
        SHCONTF_STORAGE = 0x0800
    }
    // from shlobj.h
    internal enum ESHGDN
    {
        SHGDN_NORMAL = 0x0000,
        SHGDN_INFOLDER = 0x0001,
        SHGDN_FOREDITING = 0x1000,
        SHGDN_FORADDRESSBAR = 0x4000,
        SHGDN_FORPARSING = 0x8000,
    }

    internal enum ESTRRET : int
    {
        eeRRET_WSTR = 0x0000,      // Use STRRET.pOleStr
        STRRET_OFFSET = 0x0001,    // Use STRRET.uOffset to Ansi
        STRRET_CSTR = 0x0002       // Use STRRET.cStr
    }

    // this works too...from Unions.cs
    [StructLayout(LayoutKind.Explicit, Size = 520)]
    internal struct STRRETinternal
    {
        [FieldOffset(0)]
        public IntPtr pOleStr;

        [FieldOffset(0)]
        public IntPtr pStr;  // LPSTR pStr;   NOT USED

        [FieldOffset(0)]
        public uint uOffset;

    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct STRRET
    {
        public uint uType;
        public STRRETinternal data;
    }

    internal class Guid_IShellFolder
    {
        public static Guid IID_IShellFolder = new Guid("{000214E6-0000-0000-C000-000000000046}");
    }

    [ComImport]
    [Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IExtractImage
    {
        [PreserveSig]
        int GetLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPathBuffer,
            int cch,
            ref int pdwPriority,
            ref SIZE prgSize,
            int dwRecClrDepth,
            ref int pdwFlags);

        [PreserveSig]
        int Extract(out IntPtr phBmpThumbnail);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SIZE
    {
        public int cx;
        public int cy;

        public SIZE(int cx, int cy) {
            this.cx = cx;
            this.cy = cy;
        }
    }

    #endregion
}
