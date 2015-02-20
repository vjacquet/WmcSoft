using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Tests
{
    [TestClass]
    public class BinaryFormatterTests
    {
        [TestMethod]
        public void CanFormatByteWithH2() {
            var formatter = new BinaryFormatter();

            var expected = "A0";
            byte b = 0xA0;
            var actual = String.Format(formatter, "{0:X2}", b);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanFormatBytesWithH0() {
            var formatter = new BinaryFormatter();

            var expected = "0A0B0C0D";
            var b = new byte[] { 0x0A, 0x0B, 0x0C, 0x0D, };
            var actual = String.Format(formatter, "{0:X0}", b);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanFormatBytesWithH2() {
            var formatter = new BinaryFormatter();

            var expected = "0A0B 0C0D";
            var b = new byte[] { 0x0A, 0x0B, 0x0C, 0x0D, };
            var actual = String.Format(formatter, "{0:X2}", b);

            Assert.AreEqual(expected, actual);
        }
    }
}
