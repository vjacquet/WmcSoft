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
		protected Stream stream;
		private long position;

		// Methods
		protected Com2ManagedStreamAdapater()
		{
			position = -1;
		}

		public Com2ManagedStreamAdapater(Stream stream)
			: this()
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.stream = stream;
		}

		private void ActualizePosition()
		{
			if (this.position != -1)
			{
				if (this.position > this.stream.Length)
				{
					this.stream.SetLength(this.position);
				}
				this.stream.Position = this.position;
				this.position = -1;
			}
		}

		void IStream.Clone(out IStream ppstm)
		{
			ppstm = null;
			Marshal.ThrowExceptionForHR(HResult.E_NOTIMPL);
		}

		void IStream.Commit(int grfCommitFlags)
		{
			this.stream.Flush();
			this.ActualizePosition();
		}

		//CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);
		public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
		{
			IStream istream = (IStream)this;
			int bufferSize = 0x1000;
			byte[] buffer = new byte[bufferSize];

			long written = 0;
			int read;
			IntPtr ptr = new IntPtr();

			while (written < cb)
			{
				read = bufferSize;
				if ((written + read) > cb)
				{
					read = (int)(cb - written);
				}
				// TODO: handle bug int (lo + hi)
				istream.Read(buffer, read, ptr);
				read = ptr.ToInt32();
				if (read != 0)
				{
					pstm.Write(buffer, read, ptr);
					if (ptr.ToInt32() != read)
					{
						Marshal.ThrowExceptionForHR(HResult.E_FAIL);
					}
					written += read;
				}
			}

			if (pcbRead != IntPtr.Zero)
			{
				Marshal.WriteInt64(pcbRead, written);
			}
			if (pcbWritten != IntPtr.Zero)
			{
				Marshal.WriteInt64(pcbWritten, written);
			}
		}

		public Stream BaseStream
		{
		[System.Diagnostics.DebuggerStepThrough]
			get
			{
				return this.stream;
			}
		}

		void IStream.LockRegion(long libOffset, long cb, int dwLockType)
		{
		}

		void IStream.Read(byte[] pv, int cb, IntPtr pcbRead)
		{
			int read = this.Read(pv, cb);
			if (pcbRead != IntPtr.Zero)
			{
				Marshal.WriteInt32(pcbRead, read);
			}
		}

		void IStream.Revert()
		{
			Marshal.ThrowExceptionForHR(HResult.E_NOTIMPL);
		}

		public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
		{
			long pos = this.position;
			if (this.position == -1)
			{
				pos = this.stream.Position;
			}
			long length = this.stream.Length;
			switch (dwOrigin)
			{
				case 0:
					{
						if (dlibMove > length)
						{
							this.position = dlibMove;
							break;
						}
						this.stream.Position = dlibMove;
						this.position = -1;
						break;
					}
				case 1:
					{
						if ((dlibMove + pos) > length)
						{
							this.position = dlibMove + pos;
							break;
						}
						this.stream.Position = pos + dlibMove;
						this.position = -1;
						break;
					}
				case 2:
					{
						if (dlibMove > 0)
						{
							this.position = length + dlibMove;
							break;
						}
						this.stream.Position = length + dlibMove;
						this.position = -1;
						break;
					}
			}

			if (plibNewPosition != IntPtr.Zero)
			{
				long position;
				position =  (this.position != -1) 
					? this.position
					: this.stream.Position;
				Marshal.WriteInt64(plibNewPosition, position);
			}
		}

		void IStream.SetSize(long value)
		{
			this.stream.SetLength(value);
		}

		void IStream.Stat(out STATSTG pstatstg, int grfStatFlag)
		{
			pstatstg = new STATSTG();
			pstatstg.type = 2;
			pstatstg.cbSize = this.stream.Length;
			pstatstg.grfLocksSupported = 2;
		}

		void IStream.UnlockRegion(long libOffset, long cb, int dwLockType)
		{
		}

		//Write(byte[] pv, int cb, IntPtr pcbWritten)
		void IStream.Write(byte[] pv, int cb, IntPtr pcbWritten)
		{
			this.Write(pv, cb);
			if (pcbWritten != IntPtr.Zero)
			{
				Marshal.WriteInt32(pcbWritten, cb);
			}
		}

		#region Stream-like methods

		public void SetLength(long value)
		{
			this.stream.SetLength(value);
		}

		public void Flush()
		{
			((IStream)this).Commit(0);
		}

		public int Read(byte[] buffer, int length)
		{
			this.ActualizePosition();
			return this.stream.Read(buffer, 0, length);
		}

		public void Write(byte[] buffer, int length)
		{
			this.ActualizePosition();
			this.stream.Write(buffer, 0, length);
		}
		#endregion

	}

}
