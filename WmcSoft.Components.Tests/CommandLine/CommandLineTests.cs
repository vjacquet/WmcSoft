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

            var parsed = commandLine.ParseArguments("/string:test", "/choice:A", "/switch", "/boolean-");
            Assert.AreEqual("test", commandLine.Options["string"].Value.ToString());
            Assert.AreEqual("A", commandLine.Options["choice"].Value.ToString());
            Assert.AreEqual(true, commandLine.Options["switch"].IsPresent);
            Assert.AreEqual(true, commandLine.Options["boolean"].IsPresent);
            Assert.AreEqual(true, commandLine.Options["boolean"].Value.Equals(false));
        }
    }
}
