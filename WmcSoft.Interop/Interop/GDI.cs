using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using LONG = System.Int32;

namespace WmcSoft.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public class POINTL
    {
        public int x;
        public int y;

        public POINTL(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class SIZEL
    {
        public int cx;
        public int cy;

        public SIZEL(int cx, int cy)
        {
            this.cx = cx;
            this.cy = cy;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class RECTL
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public RECTL(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public sealed class DVTARGETDEVICE
    {
        [MarshalAs(UnmanagedType.U4)]
        public int tdSize;
        [MarshalAs(UnmanagedType.U2)]
        public short tdDriverNameOffset;
        [MarshalAs(UnmanagedType.U2)]
        public short tdDeviceNameOffset;
        [MarshalAs(UnmanagedType.U2)]
        public short tdPortNameOffset;
        [MarshalAs(UnmanagedType.U2)]
        public short tdExtDevmodeOffset;

        public DVTARGETDEVICE()
        {
        }
    }

}