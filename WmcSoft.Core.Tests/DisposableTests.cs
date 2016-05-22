using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class DisposableTests
    {
        [TestMethod]
        public void CheckDisposableStackDisposeInReverseOrder() {
            int a = 0;
            int b = 0;
            int c = 0;
            int sequence = 0;

            var bin = new DisposableStack {
                new Disposer(() => a = ++sequence),
                new Disposer(() => b = ++sequence),
                new Disposer(() => c = ++sequence),
            };
            bin.Dispose();

            Assert.AreEqual(1, c);
            Assert.AreEqual(2, b);
            Assert.AreEqual(3, a);
        }
    }
}
