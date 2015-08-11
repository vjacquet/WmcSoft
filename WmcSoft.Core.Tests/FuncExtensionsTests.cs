using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Diagnostics;

namespace WmcSoft
{
    [TestClass]
    public class FuncExtensionsTests
    {
        static int Fault(int i) {
            if (i == 3)
                throw new ArgumentOutOfRangeException("i");
            return i * i;
        }

        [TestMethod]
        public void CanApplyWithException() {
            Func<int, int> fault = Fault;
            try {
                fault.ApplyEach(1, 2, 3, 4, 5);
            }
            catch (ArgumentOutOfRangeException e) {
                Assert.AreEqual(2, e.GetCapturedEntry("i"));
                Assert.AreEqual(3, e.GetCapturedEntry("arg"));
            }
        }
    }
}
