using System;
using System.Text;
using Xunit;

namespace WmcSoft.Security.Cryptography
{
    public class CRC32Tests
    {
        [Theory]
        [InlineData("", 0x0)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", 0x4c2750bd)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 0xabf77822)]
        [InlineData("Hello, world", 0xe79aa9c2)]
        public void Check(string value, uint expected)
        {
            // compare with results obtain using the reference implementation found at <http://www.ietf.org/rfc/rfc1952.txt>

            var algorithm = CRC32.Create();
            var bytes = Encoding.ASCII.GetBytes(value);
            var hash = algorithm.ComputeHash(bytes);
            var actual = BitConverter.ToUInt32(hash, 0);
            Assert.Equal(expected, actual);
        }
    }
}
