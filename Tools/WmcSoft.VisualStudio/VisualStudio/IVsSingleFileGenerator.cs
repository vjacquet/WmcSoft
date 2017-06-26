using System;
using System.Runtime.InteropServices;

namespace WmcSoft.VisualStudio
{
    [
        ComImport,
        Guid("3634494C-492F-4F91-8009-4541234E4E99"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)
    ]
    public interface IVsSingleFileGenerator
    {
        /// <summary>
        /// Retrieve default properties for the generator.
        /// </summary>
        /// <returns></returns>
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetDefaultExtension();

        /// <summary>
        /// Generate the file.
        /// </summary>
        /// <param name="wszInputFilePath"></param>
        /// <param name="bstrInputFileContents"></param>
        /// <param name="wszDefaultNamespace"></param>
        /// <param name="rgbOutputFileContents"></param>
        /// <param name="pcbOutput"></param>
        /// <param name="pGenerateProgress"></param>
        void Generate([MarshalAs(UnmanagedType.LPWStr)] string wszInputFilePath,
            [MarshalAs(UnmanagedType.BStr)] string bstrInputFileContents,
            [MarshalAs(UnmanagedType.LPWStr)] string wszDefaultNamespace,
            out IntPtr rgbOutputFileContents,
            [MarshalAs(UnmanagedType.U4)] out int pcbOutput,
            IVsGeneratorProgress pGenerateProgress);
    }
}
