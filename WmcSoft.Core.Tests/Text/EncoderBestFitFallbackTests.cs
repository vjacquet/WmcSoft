using System;
using System.Linq;
using System.Text;
using Xunit;

namespace WmcSoft.Text
{
    public class EncoderBestFitFallbackTests
    {
        [Fact]
        public void CanFallbackAccents()
        {
            var value = "àçéèêëïîôùû";
            var expected = "aceeeeiiouu";
            var actual = Encoding.ASCII.BestFitTranscode(value);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanFallbackUppercaseAccents()
        {
            var value = "ÀÇÉÈÊËÏÎÔÙÛ";
            var expected = "ACEEEEIIOUU";
            var actual = Encoding.ASCII.BestFitTranscode(value);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CannotFallbackOelig()
        {
            var value = "œ";
            var expected = "?";
            var actual = Encoding.ASCII.BestFitTranscode(value);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanSetUnknownChar()
        {
            var value = "œ";
            var expected = "#";
            var actual = Encoding.ASCII.BestFitTranscode(value, "#");
            Assert.Equal(expected, actual);
        }
    }
}