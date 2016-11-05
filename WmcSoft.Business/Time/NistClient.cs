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

 ****************************************************************************
 * Adapted from NISTClient.java
 * ----------------------------
 * Copyright (c) 2004 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WmcSoft.Time
{
    public sealed class NistClient : IDisposable
    {
        const string ServerDefault = "time.nist.gov";
        const int PortDefault = 13;

        private readonly IPEndPoint _endpoint;

        public NistClient() : this(ServerDefault, PortDefault) {
        }

        public NistClient(string host) : this(host, PortDefault) {
        }

        public NistClient(string host, int port) {
            if (host == null) throw new ArgumentNullException(nameof(host));

            var addresses = Dns.GetHostEntry(host).AddressList;
            _endpoint = new IPEndPoint(addresses[0], port);

            Timeout = TimeSpan.FromSeconds(30);
        }

        public TimeSpan Timeout { get; set; }

        public DateTime Query() {
            var buffer = new byte[256];
            int length;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) {
                socket.Connect(_endpoint);
                socket.ReceiveTimeout = (int)Timeout.TotalMilliseconds;

                length = socket.Receive(buffer);
                socket.Close();
            }
            var nistTime = Encoding.ASCII.GetString(buffer, 0, length);
            var time = nistTime.Substring(7, 17);
            return DateTime.SpecifyKind(DateTime.ParseExact(time, "y-M-d HH:mm:ss", CultureInfo.InvariantCulture), DateTimeKind.Utc);
        }

        public void Dispose() {
            // implement IDisposable in case the socket should be recycled.
        }
    }
}
