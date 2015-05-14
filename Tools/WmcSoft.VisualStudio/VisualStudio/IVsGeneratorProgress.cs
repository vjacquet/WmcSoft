using System;
using System.Runtime.InteropServices;

namespace WmcSoft.VisualStudio
{
    /// <summary>
    /// 
    /// </summary>
    [
        ComImport,
        Guid("BED89B98-6EC9-43CB-B0A8-41D6E2D6669D"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)
    ]
    public interface IVsGeneratorProgress
    {
        /// <summary>
        /// Communicate errors
        /// </summary>
        /// <param name="fWarning"></param>
        /// <param name="dwLevel"></param>
        /// <param name="bstrError"></param>
        /// <param name="dwLine"></param>
        /// <param name="dwColumn"></param>
        void GeneratorError(bool fWarning,
            [MarshalAs(UnmanagedType.U4)] int dwLevel,
            [MarshalAs(UnmanagedType.BStr)] string bstrError,
            [MarshalAs(UnmanagedType.U4)] int dwLine,
            [MarshalAs(UnmanagedType.U4)] int dwColumn);

        /// <summary>
        /// Report progress to the caller.
        /// </summary>
        /// <param name="nComplete"></param>
        /// <param name="nTotal"></param>
        void Progress([MarshalAs(UnmanagedType.U4)] int nComplete,
            [MarshalAs(UnmanagedType.U4)] int nTotal);
    }
}
