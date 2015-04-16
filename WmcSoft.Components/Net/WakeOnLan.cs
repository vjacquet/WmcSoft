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
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Permissions;
using WmcSoft.ComponentModel.Design;
using WmcSoft.ComponentModel.Design.Serialization;

namespace WmcSoft.Net
{
    [ToolboxBitmap(typeof(WakeOnLan))]
    [DefaultProperty("MacAddress")]
    [Designer(typeof(WakeOnLanDesigner))]
    public partial class WakeOnLan : IComponent
    {
        #region Lifecycle

        public WakeOnLan() {
            ResetMacAddress();
            ResetAddress();
            ResetSubnetMask();
            Port = 7;
        }

        public WakeOnLan(IContainer container)
            : this() {
            container.Add(this);
        }

        #endregion

        #region Properties

        [TypeConverter(typeof(PhysicalAddressTypeConverter))]
        public PhysicalAddress MacAddress { get; set; }

        bool ShouldSerializeMacAddress() {
            return MacAddress != PhysicalAddress.None;
        }
        void ResetMacAddress() {
            MacAddress = PhysicalAddress.None;
        }

        [TypeConverter(typeof(IPAddressTypeConverter))]
        public IPAddress Address { get; set; }

        bool ShouldSerializeAddress() {
            return Address.ToString() != "0.0.0.0";
        }
        void ResetAddress() {
            Address = IPAddress.Parse("0.0.0.0");
        }

        [TypeConverter(typeof(IPAddressTypeConverter))]
        public IPAddress SubnetMask { get; set; }

        bool ShouldSerializeSubnetMask() {
            return SubnetMask.ToString() != "255.255.255.255";
        }
        void ResetSubnetMask() {
            SubnetMask = IPAddress.Parse("255.255.255.255");
        }


        /// <remarks>The "standard" default values are 7 or 9.</remarks>
        [DefaultValue(7)]
        [Localizable(false)]
        public int Port { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void WakeUp() {
            PhysicalAddress macAddress = MacAddress;
            IPAddress address = Address;
            IPAddress subnetMask = SubnetMask;

            WakeUp(macAddress, new IPEndPoint(MakeBroadcastIPAddress(address, subnetMask), Port));
        }

        #endregion

        #region Helpers

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="subnetMask"></param>
        /// <returns></returns>
        /// <remarks>The Broadcast address is address | ~subnetMask.</remarks>
        public static IPAddress MakeBroadcastIPAddress(IPAddress address, IPAddress subnetMask) {
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
        public static void WakeUp(PhysicalAddress macAddress, IPEndPoint endPoint) {
            UdpClient udpClient = new UdpClient();
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
        public virtual ISite Site {
            get { return _site; }
            set { _site = value; }
        }
        ISite _site;

        #endregion

        #region IDisposable Membres

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                lock (this) {
                    if ((_site != null) && (_site.Container != null)) {
                        _site.Container.Remove(this);
                    }

                    EventHandler handler = Disposed;
                    if (handler != null) {
                        handler(this, EventArgs.Empty);
                    }
                }
            }
        }

        #endregion
    }

    #region Design section

    public class PhysicalAddressTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string) || sourceType == typeof(byte[]))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (value == null)
                return null;

            var sourceType = value.GetType();
            if (sourceType == typeof(byte[]))
                return new PhysicalAddress((byte[])value);

            if (value is string) {
                var text = value.ToString();
                if (text == "")
                    return null;

                return PhysicalAddress.Parse(text);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(InstanceDescriptor) || destinationType == typeof(byte[]))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (value == null)
                return null;

            var address = value as PhysicalAddress;
            if (address != null) {
                if (destinationType == typeof(string)) {
                    var bytes = address.GetAddressBytes();
                    if (bytes == null || bytes.Length == 0)
                        return null;
                    return String.Join("-", bytes.ConvertAll(b => b.ToString("x2")));
                }

                if (destinationType == typeof(InstanceDescriptor))
                    //return typeof(PhysicalAddress).DescribeMethod("Parse", address.ToString());
                    return typeof(PhysicalAddress).DescribeConstructor(address.GetAddressBytes());

                if (destinationType == typeof(byte[]))
                    return address.GetAddressBytes();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class IPAddressTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string) || sourceType == typeof(byte[]))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (value == null)
                return null;

            var sourceType = value.GetType();
            if (sourceType == typeof(byte[]))
                return new IPAddress((byte[])value);

            if (value is string) {
                var text = value.ToString();
                if (text == "")
                    return null;

                IPAddress address;
                if (IPAddress.TryParse(text, out address))
                    return address;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(InstanceDescriptor) || destinationType == typeof(byte[]))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (value == null)
                return null;

            var address = value as IPAddress;
            if (address != null) {
                if (destinationType == typeof(string))
                    return address.ToString();

                if (destinationType == typeof(InstanceDescriptor))
                    //return typeof(IPAddress).DescribeMethod("Parse", address.ToString());
                    return typeof(IPAddress).DescribeConstructor(address.GetAddressBytes());

                if (destinationType == typeof(byte[]))
                    return address.GetAddressBytes();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class WakeOnLanDesigner : ComponentDesignerBase
    {
        public WakeOnLanDesigner() {
        }

        public override void Initialize(IComponent component) {
            base.Initialize(component);

            // HACK: The codedom serializer only understands TypeConverter on type and not on properties.
            DeclareConverter<IPAddress, IPAddressTypeConverter>();
            DeclareConverter<PhysicalAddress, PhysicalAddressTypeConverter>();
        }
    }

    #endregion


}
