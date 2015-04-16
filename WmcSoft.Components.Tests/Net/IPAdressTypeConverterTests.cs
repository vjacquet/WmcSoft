using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.ComponentModel;

namespace WmcSoft.Net.Tests
{
    [TestClass]
    public class IPAdressTypeConverterTests
    {
        [TestMethod]
        public void CanConvertFromString() {
            var converter = new IPAddressTypeConverter();
            var value = "127.0.0.1";
            var expected = IPAddress.Parse(value);
            var actual = converter.ConvertFrom(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void CannotConvertFromBadString() {
            var converter = new IPAddressTypeConverter();
            var expected = "127.0.0.Z";
            var address = IPAddress.Parse(expected);
            var actual = converter.ConvertTo(address, typeof(string));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanConvertFromBytes() {
            var converter = new IPAddressTypeConverter();
            var value = new byte[] { 127, 0, 0, 1};
            var expected = new IPAddress(value);
            var actual = converter.ConvertFrom(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanConvertToString() {
            var converter = new IPAddressTypeConverter();
            var expected = "127.0.0.1";
            var address = IPAddress.Parse(expected);
            var actual = converter.ConvertTo(address, typeof(string));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanConvertToBytes() {
            var converter = new IPAddressTypeConverter();
            var expected = new byte[] { 127, 0, 0, 1 };
            var address = new IPAddress(expected);
            var actual = converter.ConvertTo<byte[]>(address);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanConvertToInstanceDescriptor() {
            var converter = new IPAddressTypeConverter();
            var expected = new byte[] { 127, 0, 0, 1 };
            var address = new IPAddress(expected);
            var descriptor = converter.ConvertTo<InstanceDescriptor>(address);
            Assert.IsNotNull(descriptor);
            var actual = (IPAddress)descriptor.Invoke();
            Assert.AreEqual(address, actual);
        }
    }
}
