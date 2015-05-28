using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Reflection.Tests
{
    [TestClass]
    public class ReflectionExtensionsTests
    {
        public class Bench
        {
            public string AString { get; set; }
            public int AInt32 { get; set; }
        }

        [TestMethod]
        public void CanGetNullValue() {
            var bench = new Bench { AString = null };
            var actual = typeof(Bench).GetProperty("AString").GetValue<string>(bench);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void CanGetValueConvertedToString() {
            var bench = new Bench { AInt32 = 51 };
            var actual = typeof(Bench).GetProperty("AInt32").GetValue<string>(bench);
            Assert.AreEqual("51", actual);
        }

        [TestMethod]
        public void CanGetValueConvertedToInt32() {
            var bench = new Bench { AString = "51" };
            var actual = typeof(Bench).GetProperty("AString").GetValue<int>(bench);
            Assert.AreEqual(51, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void CannotGetValueThatCannotGetConvertedToInt32() {
            var bench = new Bench { AString = "abc" };
            var actual = typeof(Bench).GetProperty("AString").GetValue<int>(bench);
            Assert.AreEqual(51, actual);
        }
    }
}
