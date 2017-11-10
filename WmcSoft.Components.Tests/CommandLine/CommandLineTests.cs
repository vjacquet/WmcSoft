using Xunit;

namespace WmcSoft.CommandLine
{
    public class CommandLineTests
    {
        [Fact]
        public void CanParseArguments()
        {
            var bench = new BenchComponent();
            var commandLine = bench.commandLine;

            var parsed = commandLine.ParseArguments("/string:test", "/choice:A", "/switch", "/boolean-");
            Assert.Equal("test", commandLine.Options["string"].Value.ToString());
            Assert.Equal("A", commandLine.Options["choice"].Value.ToString());
            Assert.True(commandLine.Options["switch"].IsPresent);
            Assert.True(commandLine.Options["boolean"].IsPresent);
            Assert.True(commandLine.Options["boolean"].Value.Equals(false));
        }
    }
}
