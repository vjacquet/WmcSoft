using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static WmcSoft.AI.NeuralNetwork.Helpers;

namespace WmcSoft.AI.NeuralNetwork
{
    [TestClass]
    public class HelpersTests
    {
        [TestMethod]
        public void CheckCount()
        {
            var values = new[] { 1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d };
            Assert.AreEqual(4, Count(values, 5.5d));
            Assert.AreEqual(3, Count(values, 7d));
        }
    }
}
