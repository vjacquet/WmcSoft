#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
using System.Collections.Specialized;
using System.Text;

namespace WmcSoft
{
    /// <summary>
    /// Defines the extension methods to <see cref="Uri"/> and <see cref="UriBuilder"/> classes.
    /// This is a static class.
    /// </summary>
    public static class UriExtensions
    {
        public static UriBuilder AppendToQuery(this UriBuilder builder, string name, string value) {
            return AppendToQuery(builder, Uri.EscapeUriString(name) + "=" + Uri.EscapeUriString(value));
        }

        public static UriBuilder AppendToQuery(this UriBuilder builder, NameValueCollection parameters) {
            var sb = new StringBuilder();
            foreach (string name in parameters.Keys) {
                foreach (string value in parameters.GetValues(name)) {
                    sb.Append('&')
                        .Append(Uri.EscapeUriString(name))
                        .Append('=')
                        .Append(Uri.EscapeUriString(value));
                }
            }
            if (sb.Length == 0)
                return builder;
            return AppendToQuery(builder, sb.Remove(0, 1).ToString());
        }

        public static UriBuilder AppendToQuery(this UriBuilder builder, string queryToAppend) {
            if (builder.Query != null && builder.Query.Length > 1) {
                builder.Query = builder.Query.Substring(1) + "&" + queryToAppend;
            } else {
                builder.Query = queryToAppend;
            }
            return builder;
        }
    }
}
