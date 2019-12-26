using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace WmcSoft.Security.Cryptography
{
    public class MealyTests
    {
        private static readonly byte[,] Matrix = new byte[,] {
            {  1,  8,  7, 13, 11,  9,  2, 12, 15,  5,  0, 10, 14,  4,  6,  3},
            { 13,  0,  3,  4,  8, 12,  6, 11, 15,  2,  5,  7, 14,  1,  9, 10},
            {  4,  7,  2, 13, 14,  9,  6,  0, 10, 11, 12,  5,  3, 15,  1,  8},
            { 15,  9, 10, 13,  4,  2,  6,  0,  3,  7,  1, 14, 11, 12,  8,  5},
            {  7,  1, 13, 10,  6,  3, 15, 14,  8, 11, 12,  2,  5,  9,  0,  4},
            {  6, 10,  9, 11, 12,  1,  5, 13,  3,  4,  2,  0, 14,  7,  8, 15},
            {  9,  1,  6,  7, 13, 12, 10,  0,  2,  3, 15, 11, 14,  8,  4,  5},
            {  8,  5,  0, 13, 14,  7,  1, 15,  3,  6,  9,  2, 10, 12,  4, 11},
            {  5,  4,  9, 15,  8,  7,  0, 12, 10,  2, 14,  1,  3, 11, 13,  6},
            {  5,  6, 13, 11,  7,  4,  2, 14,  9, 12, 15,  8,  0,  1, 10,  3},
            { 11,  3,  2, 10,  7,  6, 14,  9, 13,  0,  5,  1, 12, 15,  4,  8},
            {  3, 15, 11,  6,  4, 12, 5,  10, 13,  8,  0,  9,  1,  7, 14,  2},
            {  1, 12,  9,  4, 15, 11,  3,  7,  6,  0, 10,  2, 14,  5,  8, 13},
            {  8,  2,  1, 13,  6, 15, 12, 14, 11,  7,  5,  9,  0,  4,  3, 10},
            { 14,  7,  2,  9, 12,  1,  5, 11, 15,  6,  4, 10, 13,  3,  0,  8},
            {  5,  4, 10, 11,  3,  6,  8, 12,  1,  2, 15, 14,  9, 13,  0,  7},
        };

        private readonly ITestOutputHelper _output;

        public MealyTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(50)]
        [InlineData(100)]
        public void CanEncodeInt32(int value)
        {
            var ms = new MemoryStream();
            var encoder = new MealyEncodeTransform(Matrix);
            using (var stream = new CryptoStream(ms, encoder, CryptoStreamMode.Write)) {
                var buffer = BitConverter.GetBytes(value);
                stream.Write(buffer, 0, buffer.Length);
                stream.FlushFinalBlock();
            }

            var bytes = ms.ToArray();
            var encoded = BitConverter.ToInt32(bytes, 0);
            _output.WriteLine($"original {value}, encoded {encoded}");

            var decoder = new MealyDecodeTransform(Matrix);
            using (var stream = new CryptoStream(new MemoryStream(bytes), decoder, CryptoStreamMode.Read)) {
                var buffer = new byte[4];
                stream.Read(buffer, 0, buffer.Length);

                var actual = BitConverter.ToInt32(buffer, 0);
                Assert.Equal(value, actual);
            }
        }
    }
}
