﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Time
{
    [TestClass]
    public class TimeOfDayTests
    {
        [TestMethod]
        public void CanCreateTimeOfDay() {
            var time = new TimeOfDay(8, 30);

            Assert.AreEqual(8, time.Hour);
            Assert.AreEqual(30, time.Minutes);
        }

        [TestMethod]
        public void CanCompareTimes() {
            var h0830 = new TimeOfDay(8, 30);
            var h1120 = new TimeOfDay(11, 20);

            Assert.IsFalse(h0830.IsAfter(h1120));
            Assert.IsTrue(h0830.IsBefore(h1120));
        }
    }
}