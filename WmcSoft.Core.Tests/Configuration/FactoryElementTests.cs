using System;
using System.Configuration;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Diagnostics.Checkpoints;

namespace WmcSoft.Configuration
{
    [TestClass]
    public class FactoryElementTests
    {
        static System.Configuration.Configuration OpenConfiguration(string name) {
            var path = Path.Combine(Environment.CurrentDirectory, @"Configuration\" + name + ".test.config");
            var fileMap = new ExeConfigurationFileMap {
                ExeConfigFilename = path
            };
            return ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }

        static bool Thrown<TException>(Action action) where TException : Exception {
            try {
                action();
                return false;
            }
            catch (TException) {
                return true;
            }
        }

        static bool Thrown<TException>(Func<object> func) where TException : Exception {
            try {
                var r = func();
                return false;
            }
            catch (TException) {
                return true;
            }
        }

        [TestMethod]
        public void CanLoadFactorySection() {
            var configuration = OpenConfiguration("factory-valid");
            var section = (TestSection)configuration.GetSection("wmc");
            var checkpoints = section.Checkpoints;

            var checkpoint1 = checkpoints["checkpoint1"];
            Assert.AreEqual(typeof(CheckpointA), checkpoint1.Type);
            Assert.AreEqual(5, checkpoint1.Level);
            Assert.AreEqual("Value 1", checkpoint1.Parameters["undefined"]);

            var checkpoint2 = checkpoints["checkpoint2"];
            Assert.AreEqual(typeof(CheckpointA), checkpoint2.Type);
            Assert.AreEqual(2, checkpoint2.Level);
            Assert.AreEqual("Value 2", checkpoint2.Parameters["undefined"]);
        }

        [TestMethod]
        public void CannotAccessInvalidProperty() {
            var configuration = OpenConfiguration("factory-errors");
            var section = (TestSection)configuration.GetSection("wmc");
            var checkpoints = section.Checkpoints;

            var checkpoint1 = checkpoints["checkpoint1"];
            Assert.AreEqual(1, checkpoint1.ElementInformation.Errors.Count);
            Assert.AreEqual(typeof(CheckpointA), checkpoint1.Type);
            Assert.IsTrue(Thrown<ConfigurationErrorsException>(() => checkpoint1.Level));

            var checkpoint2 = checkpoints["checkpoint2"];
            Assert.AreEqual(1, checkpoint2.ElementInformation.Errors.Count);
            Assert.AreEqual(2, checkpoint2.Level);
            Assert.IsTrue(Thrown<ConfigurationErrorsException>(() => checkpoint2.Type));

            var checkpoint3 = checkpoints["checkpoint3"];
            Assert.AreEqual(2, checkpoint3.ElementInformation.Errors.Count);
            Assert.IsTrue(Thrown<ConfigurationErrorsException>(() => checkpoint3.Level));
            Assert.IsTrue(Thrown<ConfigurationErrorsException>(() => checkpoint3.Type));
        }
    }

    public class CheckpointA : CheckpointBase
    {
        public CheckpointA(string name) : base(name) {
        }

        protected override CheckpointResult DoVerify(int level) {
            return Success();
        }
    }

    public class CheckpointB
    {
        public CheckpointB(string name) {
            Name = name;
        }

        public string Name { get; }
    }
}
