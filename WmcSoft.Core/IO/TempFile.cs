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
using System.IO;

namespace WmcSoft.IO
{
    /// <summary>
    /// Creates a temporary file that will be deleted on dispose.
    /// </summary>
    public class TempFile : IDisposable
    {
        #region Fields

        private string _fullPath;

        #endregion

        #region lifecycle

        public TempFile() {
            _fullPath = Path.GetTempFileName();
        }

        #endregion

        #region Properties

        public string FullPath {
            get { return _fullPath; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            File.Delete(_fullPath);
        }

        ~TempFile() {
            try {
                Dispose(false);
            }
            catch (Exception) {
            }
        }

        #endregion
    }
}
