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
using System.Diagnostics;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    ///Represents the URL for a Web page related to the Party.
    /// </summary>
    public class WebPageAddress : AddressBase
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Uri url;

        #endregion

        #region Properties

        public Uri Url {
            get => url;
            set => url = GuardWebPageAddress(value);
        }

        public override string Address => Url.ToString();

        #endregion

        #region Lifecycle

        public WebPageAddress()
        {
            url = new Uri("about:blank");
        }

        public WebPageAddress(Uri url)
        {
            Url = url;
        }

        #endregion

        #region Helpers

        static Uri GuardWebPageAddress(Uri url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (!url.IsAbsoluteUri) throw new ArgumentException(nameof(url));

            return url;
        }

        #endregion
    }
}
