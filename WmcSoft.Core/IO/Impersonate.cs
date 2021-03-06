﻿#region Licence

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

namespace WmcSoft.IO
{
    /// <summary>
    /// Impersonates a user to allow access to network resources over CIFS.
    /// </summary>
    public sealed class Impersonate : IDisposable
    {
        #region internals

        internal sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            private SafeTokenHandle()
                : base(true)
            {
            }

            protected override bool ReleaseHandle()
            {
                return NativeMethods.CloseHandle(handle);
            }
        }

        #endregion

        #region Fields

        SafeTokenHandle _safeTokenHandle;
        WindowsImpersonationContext _impersonatedUser;

        #endregion

        #region lifecycle

        /// <summary>
        /// Initializes a new instance of the <see cref="Impersonate"/> class.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public Impersonate(string domainName, string userName, string password)
        {
            const int LOGON32_PROVIDER_DEFAULT = 0;
            //const int LOGON32_LOGON_INTERACTIVE = 2;
            //const int LOGON32_LOGON_NETWORK = 3;
            //const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;
            const int LOGON32_LOGON_NEW_CREDENTIALS = 9;

            bool returnValue = NativeMethods.LogonUser(userName, domainName, password, LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_DEFAULT, out _safeTokenHandle);
            if (false == returnValue) {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
            var newId = new WindowsIdentity(_safeTokenHandle.DangerousGetHandle());
            _impersonatedUser = newId.Impersonate();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_impersonatedUser != null) {
                _impersonatedUser.Dispose();
                _impersonatedUser = null;
            }
            if (_safeTokenHandle != null) {
                _safeTokenHandle.Dispose();
                _safeTokenHandle = null;
            }
        }

        #endregion
    }
}
