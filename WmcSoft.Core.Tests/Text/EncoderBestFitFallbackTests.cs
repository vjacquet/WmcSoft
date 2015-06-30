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
            var value = "àçéèêëïîôùû";
            var expected = "aceeeeiiouu";
            var actual = Encoding.ASCII.BestFitTranscode(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanFallbackUppercaseAccents() {
            var value = "ÀÇÉÈÊËÏÎÔÙÛ";
            var expected = "ACEEEEIIOUU";
            var actual = Encoding.ASCII.BestFitTranscode(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CannotFallbackOelig() {
            var value = "œ";
            var expected = "?";
            var actual = Encoding.ASCII.BestFitTranscode(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanSetUnknownChar() {
            var value = "œ";
            var expected = "#";
            var actual = Encoding.ASCII.BestFitTranscode(value, "#");
            Assert.AreEqual(expected, actual);
        }
    }
}