using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class TreeTests
    {
        [TestMethod]
        public void CanCreateTree() {
            var tree = new Tree<int>(5);
        }
    }
}
