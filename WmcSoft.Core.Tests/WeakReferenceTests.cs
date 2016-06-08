using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class WeakReferenceTests
    {
        [TestMethod]
        public void CanCreateWeakReference() {
            var r = new WeakReference<List<int>>(new List<int>());
            r.Target.Add(5);
        }
    }
}
