using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Text
{
    [TestClass]
    public class EncoderBestFitFallbackTests
    {
        [TestMethod]
        public void CanFallbackAccents() {
            var value = "àéèêëïîôùû";
            var expected = "aeeeeiiouu";
            var actual = Encoding.ASCII.BestFitTranscode(value);
            Assert.AreEqual(expected, actual);
        }
    }
}