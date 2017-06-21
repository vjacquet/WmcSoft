using System.IO;
using Xunit;
using WmcSoft.IO;
using WmcSoft.Runtime.Serialization;

namespace WmcSoft.Business
{
    public class GatewayResultTests
    {
        [Fact]
        public void CanConstructGatewayResult()
        {
            var ok = GatewayResult.Success;
            var undefined = GatewayResult.Failed();
            var failed = GatewayResult.Failed(new GatewayError { Code = "Error" });

            Assert.True(ok.Succeeded);
            Assert.False(undefined.Succeeded);
            Assert.NotNull(undefined.Errors);
            Assert.Equal(0, undefined.Errors.Count);
            Assert.False(failed.Succeeded);
            Assert.Equal(1, failed.Errors.Count);
        }

        [Fact]
        public void CanRoundTripSuccessGatewayResult()
        {
            var ok = GatewayResult.Success;
            var serializer = new BinarySerializer<GatewayResult>();
            var ms = new MemoryStream();

            serializer.Serialize(ms, ok);
            ms.Rewind();

            var result = serializer.Deserialize(ms);
            Assert.True(result.Succeeded);
        }
    }
}