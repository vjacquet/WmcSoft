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
using WmcSoft.IO.Sources;

namespace WmcSoft.Net
{
    /// <summary>
    /// Represents a <see cref="IStreamSource"/> to open a readable stream for the data downloaded from a resource with a given URI.
    /// </summary>
    public class DownloadSource : IStreamSource
    {
        /// <summary>
        /// Constructs an instances of the <see cref="DownloadSource"/>.
        /// </summary>
        /// <param name="webClient">The <see cref="WebClient"/> to use to download the resource.</param>
        /// <param name="uri">The URI of the resource.</param>
        public DownloadSource(WebClient webClient, Uri uri)
        {
            if (webClient == null) throw new ArgumentNullException(nameof(webClient));
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            WebClient = webClient;
            Uri = uri;
        }

        public WebClient WebClient { get; }
        public Uri Uri { get; }

        /// <summary>
        /// Gets a readable stream for the data downloaded from the given resource.
        /// </summary>
        /// <returns>A <see cref="Stream"/> used to read data from a resource.</returns>
        /// <exception cref="WebException">The URI formed by combining <see cref="WebClient.BaseAddress"/>, address is invalid.-or-
        ///    An error occurred while downloading data.</exception>
        public Stream OpenSource()
        {
            return GetStream(Uri);
        }

        protected virtual Stream GetStream(Uri uri)
        {
            return WebClient.OpenRead(uri);
        }
    }
}
