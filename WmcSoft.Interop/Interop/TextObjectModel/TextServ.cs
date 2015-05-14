using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

using WmcSoft.Interop;

using DWORD = System.UInt32;
using VoidPtr = System.IntPtr;
using HRESULT = System.Int32;
using INT = System.Int32;
using UINT = System.UInt32;
using LONG = System.Int32;
using WPARAM = System.UInt32;
using LPARAM = System.IntPtr;
using LRESULT = System.IntPtr;
using LONG_PTR = System.IntPtr;
using BOOL = System.Int32;
using HDC = System.IntPtr;
using HRGN = System.IntPtr;
using HBITMAP = System.IntPtr;
using HIMC = System.IntPtr;
using HCURSOR = System.IntPtr;
using RECT = WmcSoft.Interop.RECTL;
using COLORREF = System.UInt32;

namespace WmcSoft.Interop.TextObjectModel
{
	[StructLayout(LayoutKind.Sequential, Pack=4)]
	public class CHARFORMATW
	{
		public int cbSize;
		public int dwMask;
		public int dwEffects;
		public int yHeight;
		public int yOffset;
		public int crTextColor;
		public byte bCharSet;
		public byte bPitchAndFamily;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst=0x40)]
		public byte[] szFaceName;
		public CHARFORMATW()
		{
			this.cbSize = Marshal.SizeOf(typeof(CHARFORMATW));
			this.szFaceName = new byte[0x40];
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public class PARAFORMAT
	{
		public int cbSize;
		public int dwMask;
		public short wNumbering;
		public short wReserved;
		public int dxStartIndent;
		public int dxRightIndent;
		public int dxOffset;
		public short wAlignment;
		public short cTabCount;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst=0x20)]
		public int[] rgxTabs;

		public PARAFORMAT()
		{
			this.cbSize = Marshal.SizeOf(typeof(PARAFORMAT));
		}
	}

	/// <summary>
	/// Defines different background styles control.
	/// </summary>
	public enum BackStyle
	{
		/// <summary>
		/// background should show through.
		/// </summary>
		Transparent = 0,
		/// <summary>
		/// erase background
		/// </summary>
		Opaque = 1,
	}

	/// <summary>
	/// Defines different hitresults
	/// </summary>
	public enum HitResult
	{
		/// <summary>
		/// no hit
		/// </summary>
		NoHit = 0,
		/// <summary>
		/// point is within the text's rectangle, but in a 
		/// transparent region
		/// </summary>
		Transparent = 1,
		/// <summary>
		/// point is close to the text
		/// </summary>
		Close = 2,
		/// <summary>
		/// dead-on hit
		/// </summary>
		Hit = 3,
	}

	/// <summary>
	/// useful values for TxGetNaturalSize.
	/// </summary>
	public enum NaturalSize
	{
		/// <summary>
		/// Get a size that fits the content
		/// </summary>
		FitToContent = 1,
		/// <summary>
		/// Round to the nearest whole line.
		/// </summary>
		RoundToLine = 2
	}

	/// <summary>
	/// useful values for TxDraw lViewId parameter
	/// </summary>
	public enum View
	{
		Active = 0,
		Inactive = -1
	}

	/// <summary>
	/// used for CHANGENOTIFY.dwChangeType; indicates what happened 
	/// for a particular change.
	/// </summary>
	public enum ChangeType
	{
		/// <summary>
		/// Nothing special happened
		/// </summary>
		Generic = 0,
		/// <summary>
		/// the text changed
		/// </summary>
		TextChanged = 1,
		/// <summary>
		/// A new undo action was added
		/// </summary>
		NewUndo = 2,
		/// <summary>
		/// A new redo action was added
		/// </summary>
		NewRedo = 4
	}

	/// <summary>
	/// The TxGetPropertyBits and OnTxPropertyBitsChange methods can pass the following bits:
	/// </summary>
	/// <remarks>Do NOT rely on the ordering of these bits yet; the are subject to change.</remarks>
	[Flags]
	public enum PropertyBits
	{
		/// <summary>
		/// rich-text control
		/// </summary>
		RichText = 0x1,
		/// <summary>
		/// single vs multi-line control
		/// </summary>
		MultiLine = 2,
		/// <summary>
		/// read only text
		/// </summary>
		ReadOnly = 4,
		/// <summary>
		/// underline accelerator character
		/// </summary>
		ShowAccelerator = 8,
		/// <summary>
		/// use password char to display text
		/// </summary>
		UsePassword = 0x10,
		/// <summary>
		/// show selection when inactive
		/// </summary>
		HideSelection = 0x20,
		/// <summary>
		/// remember selection when inactive
		/// </summary>
		SaveSelection = 0x40,
		/// <summary>
		/// auto-word selection
		/// </summary>
		AutoWordSel = 0x80,
		/// <summary>
		/// vertical
		/// </summary>
		Vertical = 0x100,
		/// <summary>
		/// notification that the selection bar width has changed.
		/// </summary>
		/// <remarks>FUTURE: move this bit to the end to maintain 
		/// the division between properties and notifications.</remarks>
		SelBarChange = 0x200,
		/// <summary>
		/// if set, then multi-line controls should wrap words to fit the available display
		/// </summary>
		WordWrap = 0x400,
		/// <summary>
		/// enable/disable beeping
		/// </summary>
		AllowBeep = 0x800,
		/// <summary>
		/// disable/enable dragging
		/// </summary>
		DisableDrag = 0x1000,
		/// <summary>
		/// the inset changed
		/// </summary>
		ViewInsetChange = 0x2000,
		/// <summary>
		/// 
		/// </summary>
		BackStyleChange = 0x4000,
		/// <summary>
		/// 
		/// </summary>
		MaxLengthChange = 0x8000,
		/// <summary>
		/// 
		/// </summary>
		ScrollBarChange = 0x10000,
		/// <summary>
		/// 
		/// </summary>
		CharFormatChange = 0x20000,
		/// <summary>
		/// 
		/// </summary>
		ParaFormatChange = 0x40000,
		/// <summary>
		/// 
		/// </summary>
		ExtentChange = 0x80000,
		/// <summary>
		/// the client rectangle changed
		/// </summary>
		ClientRectChange = 0x100000,
		/// <summary>
		/// tells the renderer to use the current background color rather than the system default for an entire line
		/// </summary>
		UseCurrentBkg = 0x200000,
	}

	/// <summary>
	/// passed during an EN_CHANGE notification; contains information about 
	/// what actually happened for a change.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ChangeNotify
	{
		/// <summary>
		/// TEXT changed, etc
		/// </summary>
		public DWORD dwChangeType;
		/// <summary>
		/// cookie for the undo action
		/// </summary>
		public VoidPtr pvCookieData;
	}

	public delegate bool TxDrawDelegate(DWORD dw);

	/// <summary>
	/// An interface extending Microsoft's Text Object Model to provide
	/// extra functionality for windowless operation.  In conjunction
	/// with ITextHost, ITextServices provides the means by which the
	/// the RichEdit control can be used *without* creating a window.
	/// </summary>
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8d33f740-cf58-11ce-a89d-00aa006cadc5")]
	public interface ITextServices
	{
		/// <summary>
		/// Generic Send Message interface
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="wparam"></param>
		/// <param name="lparam"></param>
		/// <param name="plresult"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT TxSendMessage(UINT msg, WPARAM wparam, LPARAM lparam, IntPtr plresult) ;

		/// <summary>
		/// Rendering
		/// </summary>
		/// <param name="dwDrawAspect"></param>
		/// <param name="lindex"></param>
		/// <param name="pvAspect"></param>
		/// <param name="ptd"></param>
		/// <param name="hdcDraw"></param>
		/// <param name="hicTargetDev"></param>
		/// <param name="lprcBounds"></param>
		/// <param name="lprcWBounds"></param>
		/// <param name="lprcUpdate"></param>
		/// <param name="callback"></param>
		/// <param name="dwContinue"></param>
		/// <param name="lViewId"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT TxDraw(
			DWORD dwDrawAspect,
			LONG  lindex,
			VoidPtr pvAspect,
			[In, MarshalAs(UnmanagedType.Struct)] DVTARGETDEVICE ptd,
			HDC hdcDraw,
			HDC hicTargetDev,
			[In, MarshalAs(UnmanagedType.Struct)] RECTL lprcBounds,
			[In, MarshalAs(UnmanagedType.Struct)] RECTL lprcWBounds,
			[In, MarshalAs(UnmanagedType.Struct)] RECT lprcUpdate,
			[MarshalAs(UnmanagedType.FunctionPtr)] TxDrawDelegate callback,
			DWORD dwContinue,
			LONG lViewId);

		/// <summary>
		/// Horizontal scrollbar support
		/// </summary>
		/// <param name="plMin"></param>
		/// <param name="plMax"></param>
		/// <param name="plPos"></param>
		/// <param name="plPage"></param>
		/// <param name="pfEnabled"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT TxGetHScroll(out LONG plMin, out LONG plMax, out LONG plPos, out LONG plPage, out BOOL pfEnabled);

		/// <summary>
		/// Vertical scrollbar support
		/// </summary>
		/// <param name="plMin"></param>
		/// <param name="plMax"></param>
		/// <param name="plPos"></param>
		/// <param name="plPage"></param>
		/// <param name="pfEnabled"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT TxGetVScroll(out LONG plMin, out LONG plMax, out LONG plPos, out LONG plPage, out BOOL pfEnabled);

		/// <summary>
		/// Setcursor
		/// </summary>
		/// <param name="dwDrawAspect"></param>
		/// <param name="lindex"></param>
		/// <param name="pvAspect"></param>
		/// <param name="ptd"></param>
		/// <param name="hdcDraw"></param>
		/// <param name="hicTargetDev"></param>
		/// <param name="lprcClient"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT OnTxSetCursor(
			DWORD dwDrawAspect,
			LONG  lindex,
			VoidPtr pvAspect,
			[In, MarshalAs(UnmanagedType.Struct)] DVTARGETDEVICE ptd,
			HDC hdcDraw,
			HDC hicTargetDev,
			[In, MarshalAs(UnmanagedType.Struct)] RECT lprcClient,
			INT x,
			INT y);

		/// <summary>
		/// Hit-test
		/// </summary>
		/// <param name="dwDrawAspect"></param>
		/// <param name="lindex"></param>
		/// <param name="pvAspect"></param>
		/// <param name="ptd"></param>
		/// <param name="hdcDraw"></param>
		/// <param name="hicTargetDev"></param>
		/// <param name="lprcClient"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="pHitResult"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT TxQueryHitPoint(
			DWORD dwDrawAspect,
			LONG  lindex,
			VoidPtr pvAspect,
			[In, MarshalAs(UnmanagedType.Struct)] DVTARGETDEVICE ptd,
			HDC hdcDraw,
			HDC hicTargetDev,
			[In, MarshalAs(UnmanagedType.Struct)] RECT lprcClient,
			INT x,
			INT y,
			[Out, MarshalAs(UnmanagedType.U4)] HitResult pHitResult);

		/// <summary>
		/// Inplace activate notification
		/// </summary>
		/// <param name="lprcClient"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT OnTxInPlaceActivate([In, MarshalAs(UnmanagedType.Struct)] RECT lprcClient);

		/// <summary>
		/// Inplace deactivate notification
		/// </summary>
		void OnTxInPlaceDeactivate();

		/// <summary>
		/// UI activate notification
		/// </summary>
		/// <returns></returns>
		void OnTxUIActivate();

		/// <summary>
		/// UI deactivate notification
		/// </summary>
		void OnTxUIDeactivate();

		/// <summary>
		/// Get text in control
		/// </summary>
		/// <param name="pbstrText"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT TxGetText([MarshalAs(UnmanagedType.BStr)] out string pbstrText);

		/// <summary>
		/// Set text in control
		/// </summary>
		/// <param name="pszText"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT TxSetText(StringBuilder pszText);

		/// <summary>
		/// Get x position of
		/// </summary>
		/// <param name="retVal"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT TxGetCurTargetX(out LONG retVal);

		/// <summary>
		/// Get baseline position
		/// </summary>
		/// <param name="retVal"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT TxGetBaseLinePos(out LONG retVal);

		/// <summary>
		/// Get Size to fit / Natural size
		/// </summary>
		/// <param name="dwAspect"></param>
		/// <param name="hdcDraw"></param>
		/// <param name="hicTargetDev"></param>
		/// <param name="ptd"></param>
		/// <param name="dwMode"></param>
		/// <param name="psizelExtent"></param>
		/// <param name="pwidth"></param>
		/// <param name="pheight"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT TxGetNaturalSize(
			DWORD dwAspect,
			HDC hdcDraw,
			HDC hicTargetDev,
			[In, MarshalAs(UnmanagedType.Struct)] DVTARGETDEVICE ptd,
			DWORD dwMode, 
			[In, MarshalAs(UnmanagedType.Struct)] SIZEL psizelExtent,
			[In, Out] LONG pwidth,
			[In, Out] LONG pheight);

		/// <summary>
		/// Drag & drop
		/// </summary>
		/// <param name="ppDropTarget"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT TxGetDropTarget([Out, MarshalAs(UnmanagedType.Interface)] IDropTarget ppDropTarget);

		/// <summary>
		/// Bulk bit property change notifications
		/// </summary>
		/// <param name="dwMask"></param>
		/// <param name="dwBits"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT OnTxPropertyBitsChange(DWORD dwMask, DWORD dwBits);

		/// <summary>
		/// Fetch the cached drawing size
		/// </summary>
		/// <param name="pdwWidth"></param>
		/// <param name="pdwHeight"></param>
		/// <returns></returns>
		[PreserveSig]
		HRESULT TxGetCachedSize(out DWORD pdwWidth, out DWORD pdwHeight);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("c5bdd8d0-d26e-11ce-a89e-00aa006cadc5")]
	public interface ITextHost
	{
		/// <summary>
		/// Get the DC for the host
		/// </summary>
		/// <returns></returns>
		[PreserveSig]
		HDC TxGetDC();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hdc"></param>
		/// <returns></returns>
		[PreserveSig]
		INT TxReleaseDC(HDC hdc);

		/// <summary>
		/// Show the scroll bar
		/// </summary>
		/// <param name="fnBar"></param>
		/// <param name="fShow"></param>
		/// <returns></returns>
		[PreserveSig]
		BOOL TxShowScrollBar(INT fnBar, BOOL fShow);

		//@cmember Enable the scroll bar
		[PreserveSig]
		BOOL TxEnableScrollBar (INT fuSBFlags, INT fuArrowflags);

		//@cmember Set the scroll range
		[PreserveSig]
		BOOL TxSetScrollRange(
			INT fnBar,
			LONG nMinPos,
			INT nMaxPos,
			BOOL fRedraw);

		//@cmember Set the scroll position
		[PreserveSig]
		BOOL TxSetScrollPos (INT fnBar, INT nPos, BOOL fRedraw);

		//@cmember InvalidateRect
		[PreserveSig]
		void TxInvalidateRect([In] RECT prc, BOOL fMode);

		//@cmember Send a WM_PAINT to the window
		[PreserveSig]
		void TxViewChange(BOOL fUpdate);

		//@cmember Create the caret
		[PreserveSig]
		BOOL TxCreateCaret(HBITMAP hbmp, INT xWidth, INT yHeight);

		//@cmember Show the caret
		[PreserveSig]
		BOOL TxShowCaret(BOOL fShow);

		//@cmember Set the caret position
		[PreserveSig]
		BOOL TxSetCaretPos(INT x, INT y);

		//@cmember Create a timer with the specified timeout
		[PreserveSig]
		BOOL TxSetTimer(UINT idTimer, UINT uTimeout);

		//@cmember Destroy a timer
		[PreserveSig]
		void TxKillTimer(UINT idTimer);

		//@cmember Scroll the content of the specified window's client area
		[PreserveSig]
		void TxScrollWindowEx (
			INT dx,
			INT dy,
			[In] RECT lprcScroll,
			[In] RECT lprcClip,
			HRGN hrgnUpdate,
			[In, Out] ref RECT lprcUpdate,
			UINT fuScroll);
	
		//@cmember Get mouse capture
		[PreserveSig]
		void TxSetCapture(BOOL fCapture);

		//@cmember Set the focus to the text window
		[PreserveSig]
		void TxSetFocus();

		//@cmember Establish a new cursor shape
		[PreserveSig]
		void TxSetCursor(HCURSOR hcur, BOOL fText);

		//@cmember Converts screen coordinates of a specified point to the client coordinates
		[PreserveSig]
		BOOL TxScreenToClient([In, Out] POINTL lppt);

		//@cmember Converts the client coordinates of a specified point to screen coordinates
		[PreserveSig]
		BOOL TxClientToScreen([In, Out] POINTL lppt);

		//@cmember Request host to activate text services
		[PreserveSig]
		HRESULT TxActivate([Out] IntPtr plOldState);

		//@cmember Request host to deactivate text services
		[PreserveSig]
		HRESULT TxDeactivate(LONG lNewState);

		//@cmember Retrieves the coordinates of a window's client area
		[PreserveSig]
		HRESULT TxGetClientRect([In, Out] RECT prc);

		//@cmember Get the view rectangle relative to the inset
		[PreserveSig]
		HRESULT TxGetViewInset([In, Out] RECT prc);

		//@cmember Get the default character format for the text
		[PreserveSig]
		HRESULT TxGetCharFormat([Out] out CHARFORMATW ppCF);

		//@cmember Get the default paragraph format for the text
		[PreserveSig]
		HRESULT TxGetParaFormat([Out] out PARAFORMAT ppPF);

		//@cmember Get the background color for the window
		[PreserveSig]
		COLORREF TxGetSysColor(int nIndex);

		//@cmember Get the background (either opaque or transparent)
		[PreserveSig]
		HRESULT TxGetBackStyle(IntPtr pstyle);

		//@cmember Get the maximum length for the text
		[PreserveSig]
		HRESULT TxGetMaxLength(IntPtr plength);

		//@cmember Get the bits representing requested scroll bars for the window
		[PreserveSig]
		HRESULT TxGetScrollBars(IntPtr pdwScrollBar);

		//@cmember Get the character to display for password input
		[PreserveSig]
		HRESULT TxGetPasswordChar(IntPtr pch);

		//@cmember Get the accelerator character
		[PreserveSig]
		HRESULT TxGetAcceleratorPos(IntPtr pcp);

		//@cmember Get the native size
		[PreserveSig]
		HRESULT TxGetExtent([In] ref SIZEL lpExtent);

		//@cmember Notify host that default character format has changed
		[PreserveSig]
		HRESULT OnTxCharFormatChange([In] ref CHARFORMATW pcf);

		//@cmember Notify host that default paragraph format has changed
		[PreserveSig]
		HRESULT OnTxParaFormatChange ([In] ref PARAFORMAT ppf);

		//@cmember Bulk access to bit properties
		[PreserveSig]
		HRESULT TxGetPropertyBits(DWORD dwMask, IntPtr pdwBits);

		//@cmember Notify host of events
		[PreserveSig]
		HRESULT TxNotify(DWORD iNotify, [MarshalAs(UnmanagedType.AsAny)] object pv);

		// Far East Methods for getting the Input Context
		[PreserveSig]
		HIMC TxImmGetContext();
		[PreserveSig]
		void TxImmReleaseContext( HIMC himc );

		//@cmember Returns HIMETRIC size of the control bar.
		[PreserveSig]
		HRESULT TxGetSelectionBarWidth (IntPtr lSelBarWidth);
	}

	[ComVisible(true)]
	[ComDefaultInterface(typeof(ITextHost))]
	public class TextServiceHost : ITextHost, System.IServiceProvider
	{
		#region Interop

		[DllImport("riched20.dll", CallingConvention = CallingConvention.StdCall, PreserveSig=true)]
		[return: MarshalAs(UnmanagedType.Error)]
		static extern int CreateTextServices(
			[In, MarshalAs(UnmanagedType.IUnknown)] object punkOuter,
			[In, MarshalAs(UnmanagedType.Interface)] ITextHost pITextHost,
			[MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);

		#endregion

		#region Lifecycle

		public TextServiceHost()
		{
			dwBits = (int)(PropertyBits.RichText | PropertyBits.MultiLine | PropertyBits.WordWrap | PropertyBits.UseCurrentBkg);
		}

		#endregion

		#region IServiceProvider Members

		private object services;

		public object GetService(Type serviceType)
		{
			if (services == null)
			{
				object none = null;
				ITextHost host = (ITextHost)this;
				int errorCode = CreateTextServices(none, host, out services);
				Marshal.ThrowExceptionForHR(errorCode);
			}
			if (services != null && serviceType.IsAssignableFrom(services.GetType()))
			{
				return services;
			}
			return null;
		}

		public T GetService<T>() where T : class
		{
			return this.GetService(typeof(T)) as T;
		}

		#endregion

		#region ITextHost Members

		IntPtr ITextHost.TxGetDC()
		{
			return IntPtr.Zero;
		}

		int ITextHost.TxReleaseDC(IntPtr hdc)
		{
			return 1;
		}

		int ITextHost.TxShowScrollBar(int fnBar, int fShow)
		{
			return 0;
		}

		int ITextHost.TxEnableScrollBar(int fuSBFlags, int fuArrowflags)
		{
			return 0;
		}

		int ITextHost.TxSetScrollRange(int fnBar, int nMinPos, int nMaxPos, int fRedraw)
		{
			return 0;
		}

		int ITextHost.TxSetScrollPos(int fnBar, int nPos, int fRedraw)
		{
			return 0;
		}

		void ITextHost.TxInvalidateRect(RECTL prc, int fMode)
		{
		}

		void ITextHost.TxViewChange(int fUpdate)
		{
		}

		int ITextHost.TxCreateCaret(IntPtr hbmp, int xWidth, int yHeight)
		{
			return 0;
		}

		int ITextHost.TxShowCaret(int fShow)
		{
			return 0;
		}

		int ITextHost.TxSetCaretPos(int x, int y)
		{
			return 0;
		}

		int ITextHost.TxSetTimer(uint idTimer, uint uTimeout)
		{
			return 0;
		}

		void ITextHost.TxKillTimer(uint idTimer)
		{
		}

		void ITextHost.TxScrollWindowEx(int dx, int dy, RECTL lprcScroll, RECTL lprcClip, IntPtr hrgnUpdate, ref RECTL lprcUpdate, uint fuScroll)
		{
		}

		void ITextHost.TxSetCapture(int fCapture)
		{
		}

		void ITextHost.TxSetFocus()
		{
		}

		void ITextHost.TxSetCursor(IntPtr hcur, int fText)
		{
		}

		int ITextHost.TxScreenToClient(POINTL lppt)
		{
			return 0;
		}

		int ITextHost.TxClientToScreen(POINTL lppt)
		{
			return 0;
		}

		int ITextHost.TxActivate(IntPtr plOldState)
		{
			if (plOldState != IntPtr.Zero)
			{
				Marshal.WriteInt32(plOldState, 0);
			}
			return 0;
		}

		int ITextHost.TxDeactivate(int lNewState)
		{
			return 0;
		}

		int ITextHost.TxGetClientRect(RECTL prc)
		{
			rcClient = prc;
			return 0;
		}
		RECTL rcClient;

		int ITextHost.TxGetViewInset(RECTL prc)
		{
			rcViewInset = prc;
			return 0;
		}
		RECTL rcViewInset;

		int ITextHost.TxGetCharFormat(out CHARFORMATW ppCF)
		{
			ppCF = charFormat;
			System.Diagnostics.Debugger.Break();
			return 0;
		}
		CHARFORMATW charFormat = new CHARFORMATW();

		int ITextHost.TxGetParaFormat(out PARAFORMAT ppPF)
		{
			ppPF = paraFormat;
			return 0;
		}
		PARAFORMAT paraFormat = new PARAFORMAT();

		uint ITextHost.TxGetSysColor(int nIndex)
		{
			return (uint)System.Drawing.SystemColors.Window.ToArgb();
		}

		int ITextHost.TxGetBackStyle(IntPtr pstyle)
		{
			return HResult.E_NOTIMPL;
		}

		int ITextHost.TxGetMaxLength(IntPtr plength)
		{
			return HResult.E_NOTIMPL;
		}

		int ITextHost.TxGetScrollBars(IntPtr pdwScrollBar)
		{
			return HResult.E_NOTIMPL;
		}

		int ITextHost.TxGetPasswordChar(IntPtr pch)
		{
			Marshal.WriteInt16(pch, '*');
			return 0;
		}

		int ITextHost.TxGetAcceleratorPos(IntPtr pcp)
		{
			return HResult.E_NOTIMPL;
		}

		int ITextHost.TxGetExtent(ref SIZEL lpExtent)
		{
			return HResult.E_NOTIMPL;
		}

		int ITextHost.OnTxCharFormatChange(ref CHARFORMATW pcf)
		{
			return HResult.E_NOTIMPL;
		}

		int ITextHost.OnTxParaFormatChange(ref PARAFORMAT ppf)
		{
			return HResult.E_NOTIMPL;
		}

		int ITextHost.TxGetPropertyBits(uint dwMask, IntPtr pdwBits)
		{
			Marshal.WriteInt32(pdwBits, dwBits);
			return 0;
		}
		int dwBits;

		int ITextHost.TxNotify(uint iNotify, object pv)
		{
			return 0;
		}

		IntPtr ITextHost.TxImmGetContext()
		{
			return IntPtr.Zero;
		}

		void ITextHost.TxImmReleaseContext(IntPtr himc)
		{
		}

		int ITextHost.TxGetSelectionBarWidth(IntPtr lSelBarWidth)
		{
			Marshal.WriteInt64(lSelBarWidth, 100);
			return 0;
		}

		#endregion
	}

}


#if C

//+-----------------------------------------------------------------------
// 	Factories
//------------------------------------------------------------------------

// Text Services factory
STDAPI CreateTextServices(
	IUnknown *punkOuter,
	ITextHost *pITextHost,
	IUnknown **ppUnk);

typedef HRESULT (STDAPICALLTYPE * PCreateTextServices)(
	IUnknown *punkOuter,
	ITextHost *pITextHost,
	IUnknown **ppUnk);


#endif