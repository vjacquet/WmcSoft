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
using System.Net;
using WmcSoft.IO;

namespace WmcSoft.Net
{
    /// <summary>
    /// Implements a <see cref="Batch"/> to download or upload files using <see cref="WebClient"/>.
    /// </summary>
    public abstract class WebClientBatch : Batch<WebClientBatch.Scope>
    {
        public class Scope : IDisposable
        {
            private readonly Uri _baseUri;
            private readonly string _method;

            public Scope(Uri baseUri, string method) {
                _baseUri = baseUri;
                _method = method;
            }

            public Uri BaseUri { get { return _baseUri; } }
            public string Method { get { return _method; } }

            #region IDisposable Membres

            public void Dispose() {
            }

            #endregion
        }
        
        private readonly WebClient _webClient;
        private readonly string _method;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebClientBatch"/> class.
        /// </summary>
        /// <param name="webClient">The web client.</param>
        /// <param name="method">The method to use to upload.</param>
        protected WebClientBatch(WebClient webClient, string method) {
            _webClient = webClient;
            _method = method;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebClientBatch"/> class.
        /// </summary>
        /// <param name="webClient">The web client.</param>
        protected WebClientBatch(WebClient webClient) {
            _webClient = webClient;
        }

        #region Properties

        protected WebClient WebClient { get { return _webClient; } }

        #endregion

        /// <summary>
        /// Impersonates the user
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public void Impersonate(string domainName, string userName, string password) {
            if (String.IsNullOrEmpty(domainName))
                _webClient.Credentials = new NetworkCredential(userName, password);
            else
                _webClient.Credentials = new NetworkCredential(userName, password, domainName);
        }

        protected abstract string GetMethod(Uri uri);

        /// <summary>
        /// Creates the commit scope.
        /// </summary>
        /// <returns>
        /// An <see cref="IDisposable"/> instance to release resources once the commit is complete.
        /// </returns>
        protected override Scope CreateCommitScope() {
            var uri = new Uri(_webClient.BaseAddress);
            return new Scope(uri, _method ?? GetMethod(uri));
        }
    }
}
