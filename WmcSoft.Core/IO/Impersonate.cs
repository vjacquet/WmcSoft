#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace WmcSoft.IO
{
    /// <summary>
    /// Impersonates a user to allow access to network resources over CIFS.
    /// </summary>
    public class Impersonate : IDisposable
    {
        sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            private SafeTokenHandle()
                : base(true) {
            }

            [DllImport("kernel32.dll")]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            [SuppressUnmanagedCodeSecurity]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool CloseHandle(IntPtr handle);

            protected override bool ReleaseHandle() {
                return CloseHandle(handle);
            }
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);
        SafeTokenHandle safeTokenHandle;
        WindowsImpersonationContext impersonatedUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="Impersonate"/> class.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public Impersonate(string domainName, string userName, string password) {
            const int LOGON32_PROVIDER_DEFAULT = 0;
            //const int LOGON32_LOGON_INTERACTIVE = 2;
            //const int LOGON32_LOGON_NETWORK = 3;
            //const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;
            const int LOGON32_LOGON_NEW_CREDENTIALS = 9;

            bool returnValue = LogonUser(userName, domainName, password, LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_DEFAULT, out safeTokenHandle);
            if (false == returnValue) {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
            WindowsIdentity newId = new WindowsIdentity(safeTokenHandle.DangerousGetHandle());
            impersonatedUser = newId.Impersonate();
        }

        void IDisposable.Dispose() {
            if (impersonatedUser != null) {
                impersonatedUser.Dispose();
                impersonatedUser = null;
            }
            if (safeTokenHandle != null) {
                safeTokenHandle.Dispose();
                safeTokenHandle = null;
            }
        }
    }

}
