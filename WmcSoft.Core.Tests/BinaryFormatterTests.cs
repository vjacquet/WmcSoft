﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class BinaryFormatterTests
    {
        [TestMethod]
        public void CanFormatByteWithX2() {
            var formatter = new BinaryFormatter();

            var expected = "A0";
            byte b = 0xA0;
            var actual = String.Format(formatter, "{0:X2}", b);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanFormatBytesWithX0() {
            var formatter = new BinaryFormatter();

            var expected = "0A0B0C0D";
            var b = new byte[] { 0x0A, 0x0B, 0x0C, 0x0D, };
            var actual = String.Format(formatter, "{0:X0}", b);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanFormatBytesWithX2() {
            var formatter = new BinaryFormatter();

            var expected = "0A0B 0C0D";
            var b = new byte[] { 0x0A, 0x0B, 0x0C, 0x0D, };
            var actual = String.Format(formatter, "{0:X2}", b);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanFormatBytesWithB1() {
            var formatter = new BinaryFormatter();

            var expected = "00001010 00001011 00001100 00001101";
            var b = new byte[] { 0x0A, 0x0B, 0x0C, 0x0D, };
            var actual = String.Format(formatter, "{0:B1}", b);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanFormatBytesWithB2() {
            var formatter = new BinaryFormatter();

            var expected = "0000101000001011 0000110000001101";
            var b = new byte[] { 0x0A, 0x0B, 0x0C, 0x0D, };
            var actual = String.Format(formatter, "{0:B2}", b);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanFormatBytesWithO2() {
            var formatter = new BinaryFormatter();

            var expected = "00120013 00140015";
            var b = new byte[] { 0x0A, 0x0B, 0x0C, 0x0D, };
            var actual = String.Format(formatter, "{0:O2}", b);

            Assert.AreEqual(expected, actual);
        }
    }
}
