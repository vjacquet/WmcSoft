﻿using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class LatitudeTests
    {
        [TestMethod]
        public void CanToStringLatitude() {
            var iv = CultureInfo.InvariantCulture;
            var l = new Latitude(49, 30);

            Assert.AreEqual("49° 30' 00\"", l.ToString(iv));
            Assert.AreEqual("49° 30.00'", l.ToString("M", iv));
            Assert.AreEqual("49.5000°", l.ToString("D", iv));
        }

        [TestMethod]
        public void CanToStringLatitudeWithNegativeValues()
        {
            var iv = CultureInfo.InvariantCulture;
            var l = new Latitude(-49, 30);

            Assert.AreEqual("-49° 30' 00\"", l.ToString(iv));
            Assert.AreEqual("-49° 30.00'", l.ToString("M", iv));
            Assert.AreEqual("-49.5000°", l.ToString("D", iv));
        }

        [TestMethod]
        public void CanDeconstructLatitude()
        {
            var l = new Latitude(10, 15, 30);
            var (d, m, s) = l;

            Assert.AreEqual(10, d);
            Assert.AreEqual(15, m);
            Assert.AreEqual(30, s);
        }

        [TestMethod]
        public void CanDeconstructNegativeLatitude()
        {
            var l = new Latitude(-10, 15, 30);
            var (d, m, s) = l;

            Assert.AreEqual(-10, d);
            Assert.AreEqual(15, m);
            Assert.AreEqual(30, s);
        }
    }
}
