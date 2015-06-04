using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Diagnostics;

namespace WmcSoft
{
    [TestClass]
    public class ExpectedTests
    {
        static Expected<T> Success<T>(T value) {
            return value;
        }
        static Expected<T> Failed<T>(Exception exception) {
            return exception;
        }

        [TestMethod]
        public void CheckSuccessOperation() {
            var result = Success(5);
            Assert.AreEqual(6, (int)result + 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CheckFailedOperation() {
            var result = Failed<int>(new ArgumentOutOfRangeException());
            Assert.AreEqual(6, (int)result + 1);
        }

        Expected<int> MethodWithParameters(string s, int n) {
            var exception = new ArgumentNullException();
            exception.CaptureContext(new { s, n })
                .CaptureCaller();
            return exception;
        }

        [TestMethod]
        public void CheckCapture() {
            var r = MethodWithParameters("beer", 1664);
            Assert.IsTrue(r.IsFaulted);
            Assert.AreEqual("beer", r.Exception.GetCapturedEntry("s"));
            Assert.AreEqual(1664, r.Exception.GetCapturedEntry("n"));
            Assert.IsNotNull(r.Exception.GetCapturedEntry("caller"));
        }

        [TestMethod]
        public void CheckDataKeyConverterWithPrefixes() {
            var converter = DataKeyConverter.Basic.WithPrefix("own.").WithPrefix("my.");
            var actual = converter.ToKey("key");
            Assert.AreEqual("my.own.key", actual);
        }
    }
}
