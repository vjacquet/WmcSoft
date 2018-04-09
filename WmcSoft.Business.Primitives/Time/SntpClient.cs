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
using System.Net;
using System.Net.Sockets;

namespace WmcSoft.Time
{
    /// <summary>
    /// Provides common methods for querying the current time with the SNTP protocol.
    /// </summary>
    /// <example><code lang="cs" source="..\..\WmcSoft.Business.Primitives.Tests\Time\ClockTests.cs" region="QuerySntpTime" title="Usage" /></example>
    public sealed class SntpClient : IDisposable
    {
        static readonly DateTime Epoch = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        const string ServerDefault = "time.windows.com";
        const int PortDefault = 123; //The UDP port number assigned to NTP is 123

        private readonly IPEndPoint _endpoint;

        public SntpClient() : this(ServerDefault, PortDefault)
        {
        }

        public SntpClient(string hostNameOrAddress) : this(hostNameOrAddress, PortDefault)
        {
        }

        public SntpClient(string hostNameOrAddress, int port)
            : this(new IPEndPoint(Dns.GetHostEntry(hostNameOrAddress).AddressList[0], port))
        {
        }

        internal SntpClient(IPEndPoint endpoint)
        {
            _endpoint = endpoint;
            Timeout = TimeSpan.FromSeconds(30);
        }

        public TimeSpan Timeout { get; set; }

        public DateTime Query()
        {
            // adapted from <http://stackoverflow.com/a/12150289>

            // NTP message size - 16 bytes of the digest (RFC 2030)
            var ntpData = new byte[48];

            //Setting the Leap Indicator, Version Number and Mode values
            ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

            //NTP uses UDP
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)) {
                socket.Connect(_endpoint);
                socket.ReceiveTimeout = (int)Timeout.TotalMilliseconds;//Stops code hang if NTP is blocked

                socket.Send(ntpData);
                socket.Receive(ntpData);
            }

            //Offset to get to the "Transmit Timestamp" field (time at which the reply 
            //departed the server for the client, in 64-bit timestamp format."
            const byte serverReplyTime = 40;

            //Get the seconds and fraction parts
            var seconds = ReadPart(ntpData, serverReplyTime);
            var fraction = ReadPart(ntpData, serverReplyTime + 4);

            //**UTC** time
            var milliseconds = seconds * 1000 + (fraction * 1000) / 0x100000000L;
            return Epoch.AddTicks(milliseconds * TimeSpan.TicksPerMillisecond);
        }

        static long ReadPart(byte[] buffer, int offset)
        {
            // ensures correct endianness
            return (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, offset));
        }

        public void Dispose()
        {
            // implement IDisposable in case the socket should be recycled.
        }
    }
}
