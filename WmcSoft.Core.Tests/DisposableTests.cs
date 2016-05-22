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
        public void CanInitializeDisposableBin() {
            bool aIsDisposed = false;
            bool bIsDisposed = false;
            var a = new Disposer(() => aIsDisposed = true);
            var b = new Disposer(() => bIsDisposed = true);

            Assert.IsFalse(aIsDisposed);
            Assert.IsFalse(bIsDisposed);

            var bin = new DisposableStack {
                a, b
            };

            Assert.IsTrue(aIsDisposed);
            Assert.IsTrue(bIsDisposed);
        }
    }
}
