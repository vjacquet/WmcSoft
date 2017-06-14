using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.AI.FuzzyLogic
{
    [TestClass]
    public class FuzzifierTests
    {
        [TestMethod]
        public void CheckInterpret()
        {
            var random = new DeterministicRandom(new double[] { 0.8, 0.5, 0.3, 0.3, 0.9 });
            var fuzzifier = new Fuzzifier(random);

            var vAccommodative = new FuzzyCategory("v.accommodative", 0.0, 3.0 / 14.0, 6.0 / 14.0);
            fuzzifier.Categories.Add(vAccommodative);

            var accommodative = new FuzzyCategory("accommodative", 3.0 / 14.0, 6.0 / 14.0, 9.0 / 14.0);
            fuzzifier.Categories.Add(accommodative);

            var tight = new FuzzyCategory("tight", 5.0 / 14.0, 8.5 / 14.0, 12.0 / 14.0);
            fuzzifier.Categories.Add(tight);

            var vTight = new FuzzyCategory("v.tight", 10.0 / 14.0, 12.0 / 14.0, 1.0);
            fuzzifier.Categories.Add(vTight);

            Assert.AreEqual(fuzzifier.Interpret(4.0 / 14.0), accommodative);
            Assert.AreEqual(fuzzifier.Interpret(4.0 / 14.0), vAccommodative);
            Assert.AreEqual(fuzzifier.Interpret(7.5 / 14.0), accommodative);
            Assert.AreEqual(fuzzifier.Interpret(11.0 / 14.0), tight);
            Assert.AreEqual(fuzzifier.Interpret(12.5 / 14.0), vTight);
        }
    }
}
