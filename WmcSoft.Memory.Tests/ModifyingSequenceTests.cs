using System;
using Xunit;

using static WmcSoft.Memory.ModifyingSequence;

namespace WmcSoft.Memory
{
    public class ModifyingSequenceTests
    {
        struct Iota : IGenerator<int>
        {
            public int Seed;

            public int Next()
            {
                return Seed++;
            }
        }

        [Fact]
        public void CanGenerate()
        {
            var storage = new int[5];
            var generator = Generate(storage.AsSpan(), new Iota());
            Assert.Equal(new[] { 0, 1, 2, 3, 4 }, storage);
            Assert.Equal(5, generator.Seed);
        }
    }
}
