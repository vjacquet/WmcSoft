using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;

using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace WmcSoft.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	public class EXCEPINFO
	{
		[MarshalAs(UnmanagedType.U2)]
		public short wCode;
		[MarshalAs(UnmanagedType.U2)]
		public short wReserved;
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrSource;
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrDescription;
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrHelpFile;
		[MarshalAs(UnmanagedType.U4)]
		public int dwHelpContext;
		public IntPtr pvReserved;
		public IntPtr pfnDeferredFillIn;
		[MarshalAs(UnmanagedType.U4)]
		public int scode;

		public EXCEPINFO()
		{
			this.pvReserved = IntPtr.Zero;
			this.pfnDeferredFillIn = IntPtr.Zero;
		}
	}

	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("3127CA40-446E-11CE-8135-00AA004BB851")]
	public interface IErrorLog
	{
		void AddError(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName, 
			[In, MarshalAs(UnmanagedType.Struct)] EXCEPINFO pExcepInfo);
	}

	[ComImport, Guid("1CF2B120-547D-101B-8E65-08002B2BD119"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IErrorInfo
	{
		[PreserveSig, SuppressUnmanagedCodeSecurity]
		int GetGUID(out Guid pguid);
		[PreserveSig, SuppressUnmanagedCodeSecurity]
		int GetSource([In, Out, MarshalAs(UnmanagedType.BStr)] ref string pBstrSource);
		[PreserveSig, SuppressUnmanagedCodeSecurity]
		int GetDescription([In, Out, MarshalAs(UnmanagedType.BStr)] ref string pBstrDescription);
		[PreserveSig, SuppressUnmanagedCodeSecurity]
		int GetHelpFile([In, Out, MarshalAs(UnmanagedType.BStr)] ref string pBstrHelpFile);
		[PreserveSig, SuppressUnmanagedCodeSecurity]
		int GetHelpContext([In, Out, MarshalAs(UnmanagedType.U4)] ref int pdwHelpContext);
	}

	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000126-0000-0000-C000-000000000046")]
	public interface IRunnableObject
	{
		void GetRunningClass(out Guid guid);
		[PreserveSig]
		int Run(IntPtr lpBindContext);
		bool IsRunning();
		void LockRunning(bool fLock, bool fLastUnlockCloses);
		void SetContainedObject(bool fContained);
	}

	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000000B-0000-0000-C000-000000000046")]
	public interface IStorage
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		IStream CreateStream([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved1, [In, MarshalAs(UnmanagedType.U4)] int reserved2);
		[return: MarshalAs(UnmanagedType.Interface)]
		IStream OpenStream([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, IntPtr reserved1, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved2);
		[return: MarshalAs(UnmanagedType.Interface)]
		IStorage CreateStorage([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved1, [In, MarshalAs(UnmanagedType.U4)] int reserved2);
		[return: MarshalAs(UnmanagedType.Interface)]
		IStorage OpenStorage([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, IntPtr pstgPriority, [In, MarshalAs(UnmanagedType.U4)] int grfMode, IntPtr snbExclude, [In, MarshalAs(UnmanagedType.U4)] int reserved);
		void CopyTo(int ciidExclude, [In, MarshalAs(UnmanagedType.LPArray)] Guid[] pIIDExclude, IntPtr snbExclude, [In, MarshalAs(UnmanagedType.Interface)] IStorage stgDest);
		void MoveElementTo([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.Interface)] IStorage stgDest, [In, MarshalAs(UnmanagedType.BStr)] string pwcsNewName, [In, MarshalAs(UnmanagedType.U4)] int grfFlags);
		void Commit(int grfCommitFlags);
		void Revert();
		void EnumElements([In, MarshalAs(UnmanagedType.U4)] int reserved1, IntPtr reserved2, [In, MarshalAs(UnmanagedType.U4)] int reserved3, [MarshalAs(UnmanagedType.Interface)] out object ppVal);
		void DestroyElement([In, MarshalAs(UnmanagedType.BStr)] string pwcsName);
		void RenameElement([In, MarshalAs(UnmanagedType.BStr)] string pwcsOldName, [In, MarshalAs(UnmanagedType.BStr)] string pwcsNewName);
		void SetElementTimes([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In] FILETIME pctime, [In] FILETIME patime, [In] FILETIME pmtime);
		void SetClass([In] ref Guid clsid);
		void SetStateBits(int grfStateBits, int grfMask);
		void Stat([Out] STATSTG pStatStg, int grfStatFlag);
	}

	[ComImport, Guid("0000010C-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersist
	{
		[SuppressUnmanagedCodeSecurity]
		void GetClassID(out Guid pClassID);
	}

	[ComImport, Guid("37D84F60-42CB-11CE-8135-00AA004BB851"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersistPropertyBag
	{
		void GetClassID(out Guid pClassID);
		void InitNew();
		void Load([In, MarshalAs(UnmanagedType.Interface)] IPropertyBag pPropBag, [In, MarshalAs(UnmanagedType.Interface)] IErrorLog pErrorLog);
		void Save([In, MarshalAs(UnmanagedType.Interface)] IPropertyBag pPropBag, [In, MarshalAs(UnmanagedType.Bool)] bool fClearDirty, [In, MarshalAs(UnmanagedType.Bool)] bool fSaveAllProperties);
	}

	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000010A-0000-0000-C000-000000000046")]
	public interface IPersistStorage
	{
		void GetClassID(out Guid pClassID);
		[PreserveSig]
		int IsDirty();
		void InitNew(IStorage pstg);
		[PreserveSig]
		int Load(IStorage pstg);
		void Save(IStorage pStgSave, bool fSameAsLoad);
		void SaveCompleted(IStorage pStgNew);
		void HandsOffStorage();
	}

	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000109-0000-0000-C000-000000000046")]
	public interface IPersistStream
	{
		void GetClassID(out Guid pClassId);
		[PreserveSig]
		int IsDirty();
		void Load([In, MarshalAs(UnmanagedType.Interface)] IStream pstm);
		void Save([In, MarshalAs(UnmanagedType.Interface)] IStream pstm, [In, MarshalAs(UnmanagedType.Bool)] bool fClearDirty);
		long GetSizeMax();
	}

	[ComImport, Guid("7FD52380-4E07-101B-AE2D-08002B2EC713"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
	public interface IPersistStreamInit
	{
		void GetClassID(out Guid pClassID);
		[PreserveSig]
		int IsDirty();
		void Load([In, MarshalAs(UnmanagedType.Interface)] IStream pstm);
		void Save([In, MarshalAs(UnmanagedType.Interface)] IStream pstm, [In, MarshalAs(UnmanagedType.Bool)] bool fClearDirty);
		void GetSizeMax([Out, MarshalAs(UnmanagedType.LPArray)] long pcbSize);
		void InitNew();
	}

	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("55272A00-42CB-11CE-8135-00AA004BB851")]
	public interface IPropertyBag
	{
		[PreserveSig]
		int Read([In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName, [In, Out] ref object pVar, [In] IErrorLog pErrorLog);
		[PreserveSig]
		int Write([In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName, [In] ref object pVar);
	}

}
