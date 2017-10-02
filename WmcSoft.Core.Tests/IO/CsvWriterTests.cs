using System.Globalization;
using System.Text;
using Xunit;

using static WmcSoft.IO.QuotedString;

namespace WmcSoft.IO
{
    public class CsvWriterTests
    {
        [Fact]
        public void CanCreateInMemoryWriter()
        {
            var settings = new CsvWriterSettings {
                FormatProvider = CultureInfo.InvariantCulture
            };

            var sb = new StringBuilder();
            using (var writer = CsvWriter.Create(sb, settings)) {
                writer.WriteHeader("Name", "Value");
                writer.WriteRecord("a", 1.2d);
            }

            Assert.Equal("Name,Value\r\na,1.2\r\n", sb.ToString());
        }

        [Fact]
        public void CanWriteQuotedStrings()
        {
            var settings = new CsvWriterSettings {
                FormatProvider = CultureInfo.InvariantCulture
            };

            var sb = new StringBuilder();
            using (var writer = CsvWriter.Create(sb, settings)) {
                writer.WriteRecord("start", Quote("with \"quotes\""), "end");
            }
            Assert.Equal("start,\"with \"\"quotes\"\"\",end\r\n", sb.ToString());
        }

        [Theory]
        [InlineData("this is a test", "\"this is a test\"")]
        [InlineData("\"this is a test", "\"\"\"this is a test\"")]
        [InlineData("this is a test\"", "\"this is a test\"\"\"")]
        [InlineData("this is a \"test\"", "\"this is a \"\"test\"\"\"")]
        [InlineData("\"this is a test\"", "\"this is a test\"")]
        public void CanQuoteString(string text, string quoted)
        {
            var actual = Quote(text).ToQuotedString();
            Assert.Equal(quoted, actual);
        }
    }
}
