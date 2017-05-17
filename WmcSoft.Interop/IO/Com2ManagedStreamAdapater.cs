using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;

using WmcSoft.Interop;

using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

namespace WmcSoft.IO
{
    public class Com2ManagedStreamAdapater : IStream
    {
        private readonly Stream _stream;
        private long _position;

        // Methods
        protected Com2ManagedStreamAdapater()
        {
            _position = -1;
        }

        public Com2ManagedStreamAdapater(Stream stream)
            : this()
        {
            if (stream == null) {
                throw new ArgumentNullException(nameof(stream));
            }
            _stream = stream;
        }

        protected Stream BaseStream { get { return _stream; } }

        private void ActualizePosition()
        {
            if (_position != -1) {
                if (_position > _stream.Length) {
                    _stream.SetLength(_position);
                }
                _stream.Position = _position;
                _position = -1;
            }
        }

        void IStream.Clone(out IStream ppstm)
        {
            ppstm = null;
            Marshal.ThrowExceptionForHR(HResult.E_NOTIMPL);
        }

        void IStream.Commit(int grfCommitFlags)
        {
            _stream.Flush();
            ActualizePosition();
        }

        //CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);
        public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
        {
            var istream = (IStream)this;
            int bufferSize = 0x1000;
            byte[] buffer = new byte[bufferSize];

            long written = 0;
            int read;
            var ptr = new IntPtr();

            while (written < cb) {
                read = bufferSize;
                if ((written + read) > cb) {
                    read = (int)(cb - written);
                }
                // TODO: handle bug int (lo + hi)
                istream.Read(buffer, read, ptr);
                read = ptr.ToInt32();
                if (read != 0) {
                    pstm.Write(buffer, read, ptr);
                    if (ptr.ToInt32() != read) {
                        Marshal.ThrowExceptionForHR(HResult.E_FAIL);
                    }
                    written += read;
                }
            }

            if (pcbRead != IntPtr.Zero) {
                Marshal.WriteInt64(pcbRead, written);
            }
            if (pcbWritten != IntPtr.Zero) {
                Marshal.WriteInt64(pcbWritten, written);
            }
        }

        public Stream BaseStream {
            [System.Diagnostics.DebuggerStepThrough]
            get {
                return _stream;
            }
        }

        void IStream.LockRegion(long libOffset, long cb, int dwLockType)
        {
        }

        void IStream.Read(byte[] pv, int cb, IntPtr pcbRead)
        {
            int read = Read(pv, cb);
            if (pcbRead != IntPtr.Zero) {
                Marshal.WriteInt32(pcbRead, read);
            }
        }

        void IStream.Revert()
        {
            Marshal.ThrowExceptionForHR(HResult.E_NOTIMPL);
        }

        public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
        {
            long pos = _position;
            if (_position == -1) {
                pos = _stream.Position;
            }
            long length = _stream.Length;
            switch (dwOrigin) {
            case 0: {
                    if (dlibMove > length) {
                        _position = dlibMove;
                        break;
                    }
                    _stream.Position = dlibMove;
                    _position = -1;
                    break;
                }
            case 1: {
                    if ((dlibMove + pos) > length) {
                        _position = dlibMove + pos;
                        break;
                    }
                    _stream.Position = pos + dlibMove;
                    _position = -1;
                    break;
                }
            case 2: {
                    if (dlibMove > 0) {
                        _position = length + dlibMove;
                        break;
                    }
                    _stream.Position = length + dlibMove;
                    _position = -1;
                    break;
                }
            }

            if (plibNewPosition != IntPtr.Zero) {
                long position;
                position = (_position != -1)
                    ? _position
                    : _stream.Position;
                Marshal.WriteInt64(plibNewPosition, position);
            }
        }

        void IStream.SetSize(long value)
        {
            _stream.SetLength(value);
        }

        void IStream.Stat(out STATSTG pstatstg, int grfStatFlag)
        {
            pstatstg = new STATSTG() {
                type = 2,
                cbSize = _stream.Length,
                grfLocksSupported = 2
            };
        }

        void IStream.UnlockRegion(long libOffset, long cb, int dwLockType)
        {
        }

        //Write(byte[] pv, int cb, IntPtr pcbWritten)
        void IStream.Write(byte[] pv, int cb, IntPtr pcbWritten)
        {
            Write(pv, cb);
            if (pcbWritten != IntPtr.Zero) {
                Marshal.WriteInt32(pcbWritten, cb);
            }
        }

        #region Stream-like methods

        public void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public void Flush()
        {
            ((IStream)this).Commit(0);
        }

        public int Read(byte[] buffer, int length)
        {
            ActualizePosition();
            return _stream.Read(buffer, 0, length);
        }

        public void Write(byte[] buffer, int length)
        {
            ActualizePosition();
            _stream.Write(buffer, 0, length);
        }

        #endregion
    }
}
