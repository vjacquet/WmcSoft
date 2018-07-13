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
using WmcSoft.IO.Sources;

namespace WmcSoft.Net
{
    /// <summary>
    /// Implements a <see cref="Batch"/> to uploads files using <see cref="WebClient"/>.
    /// </summary>
    public class UploadBatch : WebClientBatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebClientBatch"/> class.
        /// </summary>
        /// <param name="webClient">The web client.</param>
        /// <param name="method">The method to use to upload.</param>
        public UploadBatch(WebClient webClient, string method)
            : base(webClient, method)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebClientBatch"/> class.
        /// </summary>
        /// <param name="webClient">The web client.</param>
        public UploadBatch(WebClient webClient)
            : base(webClient)
        {
        }

        protected override string GetMethod(Uri uri)
        {
            return WebClientHelper.MapMethod(uri);
        }

        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="name">Name of the entry.</param>
        /// <param name="streamSource">The data source.</param>
        protected override void Process(Scope scope, string name, IStreamSource source)
        {
            try {
                using (var local = source.GetStream())
                using (var remote = WebClient.OpenWrite(new Uri(scope.BaseUri, name.Replace('\\', '/')), scope.Method)) {
                    local.CopyTo(remote);
                }
            } catch (Exception exception) {
                if (exception.InnerException != null)
                    throw exception.InnerException;
                throw;
            }
        }
    }
}
