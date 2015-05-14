using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WmcSoft.IO
{
    class LZStream : Stream
    {
        #region Interop

        [Flags]
        enum OFEnum : int
        {
            Read = 0x00000000,
            Write = 0x00000001,
            ReadWrite = 0x00000002,
            ShareCompat = 0x00000000,
            ShareExclusive = 0x00000010,
            ShareDenyWrite = 0x00000020,
            ShareDenyRead = 0x00000030,
            ShareDenyNone = 0x00000040,
            Parse = 0x00000100,
            Delete = 0x00000200,
            Verify = 0x00000400,
            Cancel = 0x00000800,
            Create = 0x00001000,
            Prompt = 0x00002000,
            Exist = 0x00004000,
            Reopen = 0x00008000,
        }
        const int OFS_MAXPATHNAME = 128;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        unsafe struct OFSTRUCT
        {
            byte cBytes;
            byte fFixedDisk;
            UInt16 nErrCode;
            UInt16 Reserved1;
            UInt16 Reserved2;
            fixed char szPathName[OFS_MAXPATHNAME];
        };

        [DllImport("lz32.dll", EntryPoint = "LZOpenFileA", CharSet = CharSet.Ansi)]
        static extern int LZOpenFile(string lpszFile, ref OFSTRUCT lpOf, OFEnum style);
        [DllImport("lz32.dll")]
        static extern void LZClose(int hfFile);
        [DllImport("lz32.dll", CharSet = CharSet.Ansi)]
        static extern int LZRead(int hfFile, [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpvBuf, int cbread);
        [DllImport("lz32.dll")]
        static extern int LZSeek(int hfFile, int lOffset, int nOrigin);

        #endregion

        #region Private Fields

        int hfFile = -1;
        OFSTRUCT ofstruct;

        #endregion

        public LZStream(string fileName) {
            hfFile = LZOpenFile(fileName, ref ofstruct, OFEnum.Read | OFEnum.ShareDenyWrite);
        }

        public override void Close() {
            if (hfFile >= 0) {
                LZClose(hfFile);
                hfFile = -1;
            }
        }

        public override bool CanRead {
            get {
                return true;
            }
        }

        public override bool CanSeek {
            get {
                return false;
            }
        }

        public override bool CanWrite {
            get {
                return false;
            }
        }

        public override void Flush() {
        }

        public override long Length {
            get {
                throw new NotSupportedException();
            }
        }

        public override long Position {
            get {
                throw new NotSupportedException();
            }
            set {
                throw new NotSupportedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count) {
            if (hfFile < 0)
                throw new InvalidOperationException();

            StringBuilder sb = new StringBuilder(count);
            int readCount = LZRead(hfFile, sb, count);
            byte[] temp = Encoding.ASCII.GetBytes(sb.ToString());
            temp.CopyTo(buffer, offset);
            return readCount;
        }

        public override long Seek(long offset, SeekOrigin origin) {
            throw new NotSupportedException();
        }

        public override void SetLength(long value) {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count) {
            throw new NotSupportedException();
        }
    }
}
