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
using System.Security;
using WmcSoft.IO;
using WmcSoft.IO.Sources;

namespace WmcSoft.Net
{
    /// <summary>
    /// Implements a <see cref="Batch"/> to download or upload files using <see cref="WebClient"/>.
    /// </summary>
    public abstract class WebClientBatch : Batch<WebClientBatch.Scope>
    {
        public sealed class Scope : IDisposable
        {
            public Scope(Uri baseUri, string method)
            {
                BaseUri = baseUri;
                Method = method;
            }

            public Uri BaseUri { get; }
            public string Method { get; }

            public void Dispose()
            {
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebClientBatch"/> class.
        /// </summary>
        /// <param name="webClient">The web client.</param>
        /// <param name="method">The method to use to upload.</param>
        protected WebClientBatch(WebClient webClient, string method)
        {
            WebClient = webClient;
            Method = method;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebClientBatch"/> class.
        /// </summary>
        /// <param name="webClient">The web client.</param>
        protected WebClientBatch(WebClient webClient) : this(webClient, null)
        {
        }

        #region Properties

        protected WebClient WebClient { get; }
        protected string Method { get; }

        #endregion

        /// <summary>
        /// Impersonates the user
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public void Impersonate(string domainName, string userName, string password)
        {
            WebClient.Credentials = new NetworkCredential(userName, password, domainName ?? "");
        }

        /// <summary>
        /// Impersonates the user
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public void Impersonate(string domainName, string userName, SecureString password)
        {
            WebClient.Credentials = new NetworkCredential(userName, password, domainName ?? "");
        }

        protected abstract string GetMethod(Uri uri);

        /// <summary>
        /// Creates the commit scope.
        /// </summary>
        /// <returns>
        /// An <see cref="IDisposable"/> instance to release resources once the commit is complete.
        /// </returns>
        protected override Scope CreateCommitScope()
        {
            var uri = new Uri(WebClient.BaseAddress);
            return new Scope(uri, Method ?? GetMethod(uri));
        }
    }
}
