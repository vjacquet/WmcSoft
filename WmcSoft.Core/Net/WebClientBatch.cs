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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.IO;

namespace WmcSoft.Net
{
    /// <summary>
    /// Implements a <see cref="Batch"/> to uploads files using <see cref="WebClient"/>.
    /// </summary>
    public class WebClientBatch : Batch
    {
        private readonly WebClient _webClient;
        private Uri _baseUri;
        private string _method;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebClientBatch"/> class.
        /// </summary>
        /// <param name="webClient">The web client.</param>
        /// <param name="method">The method to use to upload.</param>
        public WebClientBatch(WebClient webClient, string method) {
            _webClient = webClient;
            _method = method;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebClientBatch"/> class.
        /// </summary>
        /// <param name="webClient">The web client.</param>
        public WebClientBatch(WebClient webClient) {
            _webClient = webClient;
        }

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

        protected virtual string GetMethod(Uri uri) {
            if (_baseUri.Scheme == "ftp")
                return WebRequestMethods.Ftp.UploadFile;
            else if (_baseUri.Scheme == "file")
                return  WebRequestMethods.File.UploadFile;
            else
                return WebRequestMethods.Http.Put;
        }

        /// <summary>
        /// Creates the commit scope.
        /// </summary>
        /// <returns>
        /// An <see cref="IDisposable"/> instance to release resources once the commit is complete.
        /// </returns>
        protected override IDisposable CreateCommitScope() {
            _baseUri = new Uri(_webClient.BaseAddress);
            _method = _method ?? GetMethod(_baseUri);
            return base.CreateCommitScope();
        }

        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="name">Name of the entry.</param>
        /// <param name="streamSource">The data source.</param>
        protected override void Process(string name, IStreamSource streamSource) {
            try {
                using (var source = streamSource.GetStream())
                using (var target = _webClient.OpenWrite(new Uri(_baseUri, name.Replace('\\', '/')), _method)) {
                    source.CopyTo(target);
                }
            }
            catch (Exception exception) {
                if (exception.InnerException != null)
                    throw exception.InnerException;
                throw;
            }
        }

    }
}
