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
    /// <summary>
    /// Represents a <see cref="IStreamSource"/> to open a rstream for writing data to the specified resource with a given URI.
    /// </summary>
    public class UploadSource : IStreamSource
    {
        private readonly string _method;

        /// <summary>
        /// Constructs an instances of the <see cref="UploadSource"/>.
        /// </summary>
        /// <param name="webClient">The <see cref="WebClient"/> to use to upload the resource.</param>
        /// <param name="uri">The URI of the resource.</param>
        public UploadSource(WebClient webClient, Uri uri, string method = null)
        {
            if (webClient == null) throw new ArgumentNullException(nameof(webClient));
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            WebClient = webClient;
            Uri = uri;
            _method = method;
        }


        public WebClient WebClient { get; }
        public Uri Uri { get; }
        public string Method => _method ?? GetMethod(Uri);

        protected virtual string GetMethod(Uri uri)
        {
            return WebClientHelper.MapMethod(uri);
        }

        /// <summary>
        /// Gets astream for writing data to the specified resource.
        /// </summary>
        /// <returns>A <see cref="Stream"/> used to write data to a resource.</returns>
        /// <exception cref="WebException">The URI formed by combining <see cref="WebClient.BaseAddress"/>, address is invalid.-or-
        ///    An error occurred while downloading data.</exception>
        public Stream GetStream()
        {
            return GetStream(Uri, Method);
        }

        protected virtual Stream GetStream(Uri uri, string method)
        {
            return WebClient.OpenWrite(uri, method);
        }
    }
}
