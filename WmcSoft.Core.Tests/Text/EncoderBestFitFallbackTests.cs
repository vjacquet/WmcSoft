using System.Text;
using Xunit;

namespace WmcSoft.Text
{
    public class EncoderBestFitFallbackTests
    {
        [Theory]
        [InlineData("ÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÑÒÓÔÕÖÙÚÛÜÝŸ", "AAAAAACEEEEIIIINOOOOOUUUUYY")]
        [InlineData("àáâãäåçèéêëìíîïñòóôõöùúûüýÿ", "aaaaaaceeeeiiiinooooouuuuyy")]
        [InlineData("Šš", "Ss")]
        [InlineData("ÆæðØøþŒœÐÞßƒ", "????????????")]
        public void CanFallbackAccents(string text, string expected)
        {
            var actual = Encoding.ASCII.BestFitTranscode(text);
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
