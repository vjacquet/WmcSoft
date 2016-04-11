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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Services;
using System.Security.Permissions;
using System.Text;

namespace WmcSoft.Runtime.Remoting.Services
{
    public sealed class TraceTrackingHandler : ITrackingHandler
    {
        #region Private fields

        private readonly TraceSource _traceSource;

        #endregion

        #region Lifecycle

        public TraceTrackingHandler(TraceSource traceSource) {
            _traceSource = traceSource;
        }

        #endregion

        #region ITrackingHandler Members

        /// <summary>
        /// Notifies the current instance that an object has been marshaled.
        /// </summary>
        /// <param name="obj">The object that has been marshaled.</param>
        /// <param name="or">The <see cref="ObjRef"/> that results from marshaling and represents the specified object.</param>
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public void MarshaledObject(Object obj, ObjRef or) {
            var sb = new StringBuilder();

            // Notify the user of the marshal event.
            sb.AppendFormat("Tracking: An instance of {0} was marshaled.", obj.ToString())
                .AppendLine();

            // Print the channel information.
            if (or.ChannelInfo != null) {
                // Iterate over ChannelData.
                foreach (object data in or.ChannelInfo.ChannelData) {
                    ChannelDataStore channelDataStore = data as ChannelDataStore;
                    if (channelDataStore != null) {
                        // Print the URIs from the ChannelDataStore objects.
                        string[] uris = (channelDataStore).ChannelUris;
                        foreach (string uri in uris)
                            sb.Append("  ChannelUri: ").AppendLine(uri);
                    }
                }
            }

            // Print the envoy information.
            if (or.EnvoyInfo != null)
                sb.Append("  EnvoyInfo: ").AppendLine(or.EnvoyInfo.ToString());

            // Print the type information.
            if (or.TypeInfo != null) {
                sb.Append("  TypeInfo: ").AppendLine(or.TypeInfo.ToString());
                sb.Append("  TypeName: ").AppendLine(or.TypeInfo.TypeName);
            }

            // Print the URI.
            if (or.URI != null)
                sb.Append("  URI: ").AppendLine(or.URI.ToString());

            _traceSource.TraceInformation(sb.ToString());
        }

        /// <summary>
        /// Notifies the current instance that an object has been unmarshaled.
        /// </summary>
        /// <param name="obj">The unmarshalled object.</param>
        /// <param name="or">The <see cref="ObjRef"/> that represents the specified object.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public void UnmarshaledObject(Object obj, ObjRef or) {
            _traceSource.TraceInformation("Tracking: An instance of {0} was unmarshaled.", obj.ToString());
        }

        /// <summary>
        /// Notifies the current instance that an object has been disconnected from its proxy.
        /// </summary>
        /// <param name="obj">The disconnected object.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public void DisconnectedObject(Object obj) {
            _traceSource.TraceInformation("Tracking: An instance of {0} was disconnected.", obj.ToString());
        }

        #endregion
    }
}
