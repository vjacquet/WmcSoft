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
            Assert.Empty(undefined.Errors);
            Assert.False(failed.Succeeded);
            Assert.Single(failed.Errors);
        }

        [Fact]
        public void CanRoundTripUndefinedGatewayResult()
        {
            var undefined = GatewayResult.Failed();
            var serializer = new BinarySerializer<GatewayResult>();
            var ms = new MemoryStream();

            serializer.Serialize(ms, undefined);
            ms.Rewind();

            var result = serializer.Deserialize(ms);
            Assert.False(undefined.Succeeded);
            Assert.NotNull(undefined.Errors);
            Assert.Empty(undefined.Errors);
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

        [Fact]
        public void CanConstructGatewayResultWithValue()
        {
           var ok = GatewayResult.Returns(42);
           var undefined = GatewayResult<int>.Failed();
           var failed = GatewayResult<int>.Failed(new GatewayError { Code = "Error" });

            Assert.True(ok.Succeeded);
            Assert.Equal(42, ok.Value);
            Assert.False(undefined.Succeeded);
            Assert.NotNull(undefined.Errors);
            Assert.Empty(undefined.Errors);
            Assert.False(failed.Succeeded);
            Assert.Single(failed.Errors);
        }

        [Fact]
        public void CanRoundTripSuccessGatewayResultWithValue()
        {
            var ok = GatewayResult.Returns(42);
            var serializer = new BinarySerializer<GatewayResult<int>>();
            var ms = new MemoryStream();

            serializer.Serialize(ms, ok);
            ms.Rewind();

            var result = serializer.Deserialize(ms);
            Assert.True(result.Succeeded);
        }

        [Fact]
        public void CanRoundTripUndefinedGatewayResultWithValue()
        {
            var undefined = GatewayResult<int>.Failed();
            var serializer = new BinarySerializer<GatewayResult<int>>();
            var ms = new MemoryStream();

            serializer.Serialize(ms, undefined);
            ms.Rewind();

            var result = serializer.Deserialize(ms);
            Assert.False(undefined.Succeeded);
            Assert.NotNull(undefined.Errors);
            Assert.Empty(undefined.Errors);
        }

        [Fact]
        public void CanRoundTripFailedGatewayResultWithValue()
        {
          var failed = GatewayResult<int>.Failed(new GatewayError { Code = "Error" });
            var serializer = new BinarySerializer<GatewayResult<int>>();
            var ms = new MemoryStream();

            serializer.Serialize(ms, failed);
            ms.Rewind();

            var result = serializer.Deserialize(ms);
            Assert.False(result.Succeeded);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Error", result.Errors[0].Code);
        }
    }
}
