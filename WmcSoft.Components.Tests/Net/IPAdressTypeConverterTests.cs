using System;
using System.ComponentModel.Design.Serialization;
using System.Net;
using Xunit;
using WmcSoft.ComponentModel;

namespace WmcSoft.Net
{
    public class IPAdressTypeConverterTests
    {
        [Fact]
        public void CanConvertFromString()
        {
            var converter = new IPAddressTypeConverter();
            var value = "127.0.0.1";
            var expected = IPAddress.Parse(value);
            var actual = converter.ConvertFrom(value);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CannotConvertFromBadString()
        {
            var converter = new IPAddressTypeConverter();
            Assert.Throws<FormatException>(() => IPAddress.Parse("127.0.0.Z"));
        }

        [Fact]
        public void CanConvertFromBytes()
        {
            var converter = new IPAddressTypeConverter();
            var value = new byte[] { 127, 0, 0, 1 };
            var expected = new IPAddress(value);
            var actual = converter.ConvertFrom(value);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanConvertToString()
        {
            var converter = new IPAddressTypeConverter();
            var expected = "127.0.0.1";
            var address = IPAddress.Parse(expected);
            var actual = converter.ConvertTo(address, typeof(string));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanConvertToBytes()
        {
            var converter = new IPAddressTypeConverter();
            var expected = new byte[] { 127, 0, 0, 1 };
            var address = new IPAddress(expected);
            var actual = converter.ConvertTo<byte[]>(address);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanConvertToInstanceDescriptor()
        {
            var converter = new IPAddressTypeConverter();
            var expected = new byte[] { 127, 0, 0, 1 };
            var address = new IPAddress(expected);
            var descriptor = converter.ConvertTo<InstanceDescriptor>(address);
            Assert.NotNull(descriptor);
            var actual = (IPAddress)descriptor.Invoke();
            Assert.Equal(address, actual);
        }
    }
}
