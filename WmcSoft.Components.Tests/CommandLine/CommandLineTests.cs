using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.CommandLine.Tests
{
    [TestClass]
    public class CommandLineTests
    {
        [TestMethod]
        public void CanParseArguments() {
            var bench =new BenchComponent();
            var commandLine = bench.commandLine;

            var parsed = commandLine.ParseArguments("/string:test");
            var actual = commandLine.Options["string"].Value.ToString();
            Assert.AreEqual("test", actual);
        }
    }
}
