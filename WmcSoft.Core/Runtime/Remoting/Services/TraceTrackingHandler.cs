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
using System.Linq;
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
        public void MarshaledObject(object obj, ObjRef or) {
            var writer = new Writer("Tracking: An instance of {0} was marshaled.", obj);

            writer.Write("  ChannelUri: ", or.ChannelInfo);
            writer.Write("  EnvoyInfo: ", or.EnvoyInfo);
            writer.Write("  TypeInfo: ", or.TypeInfo);
            writer.Write("  URI: ", or.URI);

            _traceSource.TraceInformation(writer.ToString());
        }

        /// <summary>
        /// Notifies the current instance that an object has been unmarshaled.
        /// </summary>
        /// <param name="obj">The unmarshalled object.</param>
        /// <param name="or">The <see cref="ObjRef"/> that represents the specified object.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public void UnmarshaledObject(object obj, ObjRef or) {
            _traceSource.TraceInformation("Tracking: An instance of {0} was unmarshaled.", obj.ToString());
        }

        /// <summary>
        /// Notifies the current instance that an object has been disconnected from its proxy.
        /// </summary>
        /// <param name="obj">The disconnected object.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public void DisconnectedObject(object obj) {
            _traceSource.TraceInformation("Tracking: An instance of {0} was disconnected.", obj.ToString());
        }

        #endregion

        #region Helpers

        struct Writer
        {
            readonly StringBuilder sb;

            public Writer(string format, object arg0) {
                sb = new StringBuilder(string.Format(format, arg0));
            }

            void WriteLine(string prefix, string text) {
                sb.Append(prefix).AppendLine(text);
            }

            public void Write(string prefix, IChannelInfo channelInfo) {
                if (channelInfo == null)
                    return;
                var query = channelInfo.ChannelData.OfType<ChannelDataStore>().SelectMany(ds => ds.ChannelUris);
                foreach (var uri in query) {
                    WriteLine(prefix, uri);
                }
            }

            public void Write(string prefix, IRemotingTypeInfo typeInfo) {
                if (typeInfo == null)
                    return;
                WriteLine(prefix, typeInfo + " (" + typeInfo.TypeName + ")");
            }

            public void Write(string prefix, object value) {
                if (value == null)
                    return;
                WriteLine(prefix, value.ToString());
            }

            public override string ToString() {
                return sb.ToString();
            }
        }

        #endregion
    }
}
