using System;
using System.Configuration;
using System.IO;
using Xunit;
using WmcSoft.Diagnostics.Checkpoints;

namespace WmcSoft.Configuration
{
    public class FactoryElementTests
    {
        static System.Configuration.Configuration OpenConfiguration(string name)
        {
            var path = Path.Combine(Environment.CurrentDirectory, @"Configuration\" + name + ".test.config");
            var fileMap = new ExeConfigurationFileMap {
                ExeConfigFilename = path
            };
            return ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }

        [Fact]
        public void CanLoadFactorySection()
        {
            var configuration = OpenConfiguration("factory-valid");
            var section = (TestSection)configuration.GetSection("wmc");
            var checkpoints = section.Checkpoints;

            var checkpoint1 = checkpoints["checkpoint1"];
            Assert.Equal(typeof(CheckpointA), checkpoint1.Type);
            Assert.Equal(5, checkpoint1.Level);
            Assert.Equal("Value 1", checkpoint1.Parameters["undefined"]);

            var checkpoint2 = checkpoints["checkpoint2"];
            Assert.Equal(typeof(CheckpointA), checkpoint2.Type);
            Assert.Equal(2, checkpoint2.Level);
            Assert.Equal("Value 2", checkpoint2.Parameters["undefined"]);
        }

        [Fact]
        public void CannotAccessInvalidProperty()
        {
            var configuration = OpenConfiguration("factory-errors");
            var section = (TestSection)configuration.GetSection("wmc");
            var checkpoints = section.Checkpoints;

            var checkpoint1 = checkpoints["checkpoint1"];
            Assert.Equal(1, checkpoint1.ElementInformation.Errors.Count);
            Assert.Equal(typeof(CheckpointA), checkpoint1.Type);
            Assert.Throws<ConfigurationErrorsException>(() => checkpoint1.Level);

            var checkpoint2 = checkpoints["checkpoint2"];
            Assert.Equal(1, checkpoint2.ElementInformation.Errors.Count);
            Assert.Equal(2, checkpoint2.Level);
            Assert.Throws<ConfigurationErrorsException>(() => checkpoint2.Type);

            var checkpoint3 = checkpoints["checkpoint3"];
            Assert.Equal(2, checkpoint3.ElementInformation.Errors.Count);
            Assert.Throws<ConfigurationErrorsException>(() => checkpoint3.Level);
            Assert.Throws<ConfigurationErrorsException>(() => checkpoint3.Type);
        }
    }

    public class CheckpointA : CheckpointBase
    {
        public CheckpointA(string name) : base(name)
        {
        }

        protected override CheckpointResult DoVerify(int level)
        {
            return Success();
        }
    }

    public class CheckpointB
    {
        public CheckpointB(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
