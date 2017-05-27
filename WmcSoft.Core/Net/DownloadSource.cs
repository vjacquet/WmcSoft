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
using System.IO;
using System.Net;
using WmcSoft.IO;

namespace WmcSoft.Net
{
    public class DownloadSource : IStreamSource
    {
        #region Fields

        private readonly WebClient _webClient;
        private readonly Uri _uri;

        #endregion

        #region Lifecycle

        public DownloadSource(WebClient webClient, Uri uri)
        {
            _webClient = webClient;
            _uri = uri;
        }

        #endregion

        #region Properties

        public WebClient WebClient { get { return _webClient; } }
        public Uri Uri { get { return _uri; } }

        #endregion

        #region IStreamSource Membres

        public Stream GetStream()
        {
            return GetStream(_uri);
        }

        protected virtual Stream GetStream(Uri uri)
        {
            return _webClient.OpenRead(uri);
        }

        #endregion
    }
}