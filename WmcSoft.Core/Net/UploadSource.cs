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
    public class UploadSource : IStreamSource
    {
        #region Fields

        readonly WebClient _webClient;
        readonly Uri _uri;
        readonly string _method;

        #endregion

        #region Lifecycle

        public UploadSource(WebClient webClient, Uri uri, string method = null) {
            _webClient = webClient;
            _uri = uri;
            _method = method;
        }

        #endregion

        #region Properties

        public WebClient WebClient { get { return _webClient; } }
        public Uri Uri { get { return _uri; } }

        #endregion

        protected virtual string GetMethod(Uri uri) {
            return WebClientHelper.MapMethod(uri);
        }

        #region IStreamSource Membres

        public Stream GetStream() {
            var uri = _uri.IsAbsoluteUri
                ? _uri
                : new Uri(new Uri(WebClient.BaseAddress), _uri);
            return GetStream(uri, _method ?? GetMethod(uri));
        }

        protected virtual Stream GetStream(Uri uri, string method) {
            return _webClient.OpenWrite(uri, method);

        }

        #endregion
    }
}
