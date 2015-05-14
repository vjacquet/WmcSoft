using System;
using System.Runtime.InteropServices;

namespace WmcSoft.Interop
{
	/// <summary>
	/// The IServiceProvider interface is a generic access mechanism to locate a globally unique identifier (GUID) identified service. 
	/// </summary>
	[
		ComImport,
		Guid("6D5140C1-7436-11CE-8034-00AA006009FA"),
		InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)
	]
	public interface IServiceProvider
	{
		/// <summary>
		/// Acts as the factory method for any services exposed through an implementation of IServiceProvider.
		/// </summary>
		/// <param name="guidService">Unique identifier of the service (an SID).</param>
		/// <param name="riid">Unique identifier of the interface the caller wishes to receive for the service. </param>
		/// <param name="ppvObject">
		/// Address of the caller-allocated variable to receive the interface 
		/// pointer of the service on successful return from this function. The caller becomes 
		/// responsible for calling Release through this interface pointer when the service is no longer needed.
		/// </param>
		/// <returns>
		/// Returns one of the following values:
		/// <list type="table">
		/// <item><term>S_OK</term><description>The service was successfully created or retrieved. The caller is responsible for calling ((IUnknown *)*ppv)->Release();.</description></item>
		/// <item><term>E_OUTOFMEMORY</term><description>There is insufficient memory to create the service.</description></item>
		/// <item><term>E_UNEXPECTED</term><description>An unknown error occurred.</description></item>
		/// <item><term>E_NOINTERFACE</term><description>The service exists, but the interface requested does not exist on that service.</description></item>
		/// </list>
		/// </returns>
		[PreserveSig]
		int QueryService([In]ref Guid guidService, [In]ref Guid riid, out IntPtr ppvObject);
	}
}
