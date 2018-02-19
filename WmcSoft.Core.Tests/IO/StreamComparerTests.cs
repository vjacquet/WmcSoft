using System;
using System.IO;
using Xunit;

namespace WmcSoft.IO
{
    public class StreamComparerTests
    {
        static byte[] Sample(int seed, int length)
        {
            var random = new Random(seed);
            var buffer = new byte[length];
            random.NextBytes(buffer);
            return buffer;
        }

        [Fact]
        public void IdenticalStreamsAreEqual()
        {
            var sample1 = Sample(2001, 512);
            var sample2 = Sample(2001, 512);
            Assert.False(ReferenceEquals(sample1, sample2));

            var comparer = new StreamComparer();
            using (var ms1 = new MemoryStream(sample1))
            using (var ms2 = new MemoryStream(sample2)) {
                Assert.True(comparer.Equals(ms1, ms2));
            }

            using (var ms1 = new MemoryStream(sample1))
            using (var ms2 = new MemoryStream(sample2)) {
                Assert.Equal(0, comparer.Compare(ms1, ms2));
            }
        }

        [Fact]
        public void LengthIsUseOnEqualForSeekableStreams()
        {
            var sample1 = Sample(2001, 256);
            var sample2 = Sample(2010, 512);

            var comparer = new StreamComparer();

            using (var ms1 = new MemoryStream(sample1))
            using (var ms2 = new MemoryStream(sample2)) {
                Assert.False(comparer.Equals(ms1, ms2));
                Assert.Equal(0, ms1.Position);
                Assert.Equal(0, ms2.Position);
            }
        }

        [Fact]
        public void DifferentStreamsOfSameLengthAreNotEqual()
        {
            var sample1 = Sample(2001, 512);
            var sample2 = Sample(2010, 512);
            Assert.False(ReferenceEquals(sample1, sample2));

            var comparer = new StreamComparer();

            using (var ms1 = new MemoryStream(sample1))
            using (var ms2 = new MemoryStream(sample2)) {
                Assert.False(comparer.Equals(ms1, ms2));
            }

            using (var ms1 = new MemoryStream(sample1))
            using (var ms2 = new MemoryStream(sample2)) {
                Assert.NotEqual(0, comparer.Compare(ms1, ms2));
            }
        }

        [Fact]
        public void StreamsOfDifferentLengthAreNotEqual()
        {
            var sample1 = Sample(2001, 256);
            var sample2 = Sample(2001, 512);
            Assert.False(ReferenceEquals(sample1, sample2));

            var comparer = new StreamComparer();
            using (var ms1 = new MemoryStream(sample1))
            using (var ms2 = new MemoryStream(sample2)) {
                Assert.False(comparer.Equals(ms1, ms2));
            }
        }

        [Fact]
        public void TruncatedStreamIsSmaller()
        {
            var sample1 = Sample(2001, 256);
            var sample2 = Sample(2001, 512);
            Assert.False(ReferenceEquals(sample1, sample2));

            var comparer = new StreamComparer();
            using (var ms1 = new MemoryStream(sample1))
            using (var ms2 = new MemoryStream(sample2)) {
                Assert.True(comparer.Compare(ms1, ms2) < 0);
            }
        }

        [Fact]
        public void CanGethashCode()
        {
            var sample = Sample(2001, 512);
            var comparer = new StreamComparer();
            using (var ms = new MemoryStream(sample)) {
                var h = comparer.GetHashCode(ms);
                Assert.Equal(ms.Length, ms.Position); // The stream should have been read completly
            }
        }
    }
}
