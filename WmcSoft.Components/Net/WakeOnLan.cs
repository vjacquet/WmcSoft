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
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Permissions;
using WmcSoft.ComponentModel.Design;

namespace WmcSoft.Net
{
    [ToolboxBitmap(typeof(WakeOnLan))]
    [DefaultProperty(nameof(MacAddress))]
    [Designer(typeof(WakeOnLanDesigner))]
    public partial class WakeOnLan : IComponent
    {
        #region Lifecycle

        public WakeOnLan()
        {
        }

        public WakeOnLan(IContainer container)
            : this()
        {
            container.Add(this);
        }

        #endregion

        #region Properties

        [TypeConverter(typeof(PhysicalAddressTypeConverter))]
        public PhysicalAddress MacAddress { get; set; } = PhysicalAddress.None;

        bool ShouldSerializeMacAddress()
        {
            return MacAddress != PhysicalAddress.None;
        }
        void ResetMacAddress()
        {
            MacAddress = PhysicalAddress.None;
        }

        [TypeConverter(typeof(IPAddressTypeConverter))]
        public IPAddress Address { get; set; } = IPAddress.Any;

        bool ShouldSerializeAddress()
        {
            return Address != IPAddress.Any;
        }
        void ResetAddress()
        {
            Address = IPAddress.Any;
        }

        [TypeConverter(typeof(IPAddressTypeConverter))]
        public IPAddress SubnetMask { get; set; } = IPAddress.Broadcast;

        bool ShouldSerializeSubnetMask()
        {
            return SubnetMask != IPAddress.Broadcast;
        }
        void ResetSubnetMask()
        {
            SubnetMask = IPAddress.Broadcast;
        }

        /// <remarks>The "standard" default values are 7 or 9.</remarks>
        [DefaultValue(7)]
        [Localizable(false)]
        public int Port { get; set; } = 7;

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void WakeUp()
        {
            var ip = new IPEndPoint(MakeBroadcastIPAddress(Address, SubnetMask), Port);
            WakeUp(MacAddress, ip);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="subnetMask"></param>
        /// <returns></returns>
        /// <remarks>The Broadcast address is address | ~subnetMask.</remarks>
        public static IPAddress MakeBroadcastIPAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] broadcast = address.GetAddressBytes();
            byte[] mask = subnetMask.GetAddressBytes();

            if (broadcast.Length != mask.Length)
                throw new ArgumentException();

            for (int i = 0; i != broadcast.Length; ++i) {
                broadcast[i] = (byte)(broadcast[i] | ~mask[i]);
            }

            return new IPAddress(broadcast);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="macAddress"></param>
        /// <param name="endPoint"></param>
        public static void WakeUp(PhysicalAddress macAddress, IPEndPoint endPoint)
        {
            var udpClient = new UdpClient();
            byte[] magicPacket = new byte[102];
            int offset = 0;

            // copy the first FF:FF:FF:FF:FF:FF
            while (offset != 6) {
                magicPacket[offset++] = 0xff;
            }

            // then copy the mac address 16 times
            while (offset != 102) {
                macAddress.GetAddressBytes().CopyTo(magicPacket, offset);
                offset += 6;
            }

            udpClient.Send(magicPacket, magicPacket.Length, endPoint);
        }

        #endregion

        #region IComponent Membres

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Browsable(false)]
        public event EventHandler Disposed;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public virtual ISite Site { get; set; }

        #endregion

        #region IDisposable Membres

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) {
                lock (this) {
                    Site?.Container?.Remove(this);

                    var handler = Disposed;
                    if (handler != null) {
                        handler(this, EventArgs.Empty);
                    }
                }
            }
        }

        #endregion
    }

    #region Design section

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class WakeOnLanDesigner : ComponentDesignerBase
    {
        public WakeOnLanDesigner()
        {
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            // HACK: The codedom serializer only understands TypeConverter on type and not on properties.
            DeclareConverter<IPAddress, IPAddressTypeConverter>();
            DeclareConverter<PhysicalAddress, PhysicalAddressTypeConverter>();
        }
    }

    #endregion
}
