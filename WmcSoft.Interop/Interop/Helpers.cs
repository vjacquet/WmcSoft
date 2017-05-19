using System;
using System.Diagnostics;

namespace WmcSoft.Interop
{
    static public class Helpers
    {
        #region HRESULT checking
        [DebuggerStepThrough]
        public static bool Succeeded(int hr)
        {
            return hr >= 0;
        }
        [DebuggerStepThrough]
        public static bool Failed(int hr)
        {
            return hr < 0;
        }
        #endregion

        #region Definition of the IID
        public static readonly Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");
        #endregion

        #region Error codes
        public const int E_FAIL = unchecked((int)0x80004005);
        public const int E_NOINTERFACE = unchecked((int)0x80004002);
        #endregion
    }
}