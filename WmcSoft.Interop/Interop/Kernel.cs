using System;
using System.Runtime.InteropServices;

namespace WmcSoft.Interop
{
    internal class Kernel
    {
        public static readonly IntPtr InvalidHandleValue = new IntPtr(-1);
        public const UInt32 FILE_MAP_WRITE = 2;
        public const UInt32 PAGE_READWRITE = 0x04;

        [DllImport("Kernel32")]
        public static extern IntPtr CreateFileMapping(IntPtr hFile,
            IntPtr pAttributes, UInt32 flProtect,
            UInt32 dwMaximumSizeHigh, UInt32 dwMaximumSizeLow, String pName);

        [DllImport("Kernel32")]
        public static extern IntPtr OpenFileMapping(UInt32 dwDesiredAccess,
            Boolean bInheritHandle, String name);

        [DllImport("Kernel32")]
        public static extern Boolean CloseHandle(IntPtr handle);

        [DllImport("Kernel32")]
        public static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject,
            UInt32 dwDesiredAccess,
            UInt32 dwFileOffsetHigh, UInt32 dwFileOffsetLow,
            IntPtr dwNumberOfBytesToMap);

        [DllImport("Kernel32")]
        public static extern Boolean UnmapViewOfFile(IntPtr address);

        [DllImport("Kernel32")]
        public static extern Boolean DuplicateHandle(IntPtr hSourceProcessHandle,
            IntPtr hSourceHandle,
            IntPtr hTargetProcessHandle, ref IntPtr lpTargetHandle,
            UInt32 dwDesiredAccess, Boolean bInheritHandle, UInt32 dwOptions);
        public const UInt32 DUPLICATE_CLOSE_SOURCE = 0x00000001;
        public const UInt32 DUPLICATE_SAME_ACCESS = 0x00000002;

        [DllImport("Kernel32")]
        public static extern IntPtr GetCurrentProcess();
    }
}
