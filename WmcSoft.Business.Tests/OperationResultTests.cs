using System;
using System.IO;
using Xunit;
using WmcSoft.IO;
using WmcSoft.Runtime.Serialization;

namespace WmcSoft.Business
{
    public class OperationResultTests
    {
        [Fact]
        public void CanConstructOperationResult()
        {
            var ok = OperationResult.Success;
            var undefined = OperationResult.Failed();
            var failed = OperationResult.Failed(new OperationError { Code = "Error" });

            Assert.True(ok.Succeeded);
            Assert.False(undefined.Succeeded);
            Assert.NotNull(undefined.Errors);
            Assert.Empty(undefined.Errors);
            Assert.False(failed.Succeeded);
            Assert.Single(failed.Errors);
        }

        [Fact]
        public void DefaultOperationResultIsUndefined()
        {
            OperationResult result = default;

            Assert.False(result.Succeeded);
            Assert.NotNull(result.Errors);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void CanRoundTripUndefinedOperationResult()
        {
            var undefined = OperationResult.Failed();
            var serializer = new BinarySerializer<OperationResult>();
            var ms = new MemoryStream();

            serializer.Serialize(ms, undefined);
            ms.Rewind();

            var result = serializer.Deserialize(ms);
            Assert.False(undefined.Succeeded);
            Assert.NotNull(undefined.Errors);
            Assert.Empty(undefined.Errors);
        }

        [Fact]
        public void CanRoundTripSuccessOperationResult()
        {
            var ok = OperationResult.Success;
            var serializer = new BinarySerializer<OperationResult>();
            var ms = new MemoryStream();

            serializer.Serialize(ms, ok);
            ms.Rewind();

            var result = serializer.Deserialize(ms);
            Assert.True(result.Succeeded);
        }

        [Fact]
        public void CanConstructOperationResultWithValue()
        {
           var ok = OperationResult.Returns(42);
           var undefined = OperationResult<int>.Failed();
           var failed = OperationResult<int>.Failed(new OperationError { Code = "Error" });

            Assert.True(ok.Succeeded);
            Assert.Equal(42, ok.Value);
            Assert.False(undefined.Succeeded);
            Assert.NotNull(undefined.Errors);
            Assert.Empty(undefined.Errors);
            Assert.False(failed.Succeeded);
            Assert.Single(failed.Errors);
        }

        [Fact]
        public void CanRoundTripSuccessOperationResultWithValue()
        {
            var ok = OperationResult.Returns(42);
            var serializer = new BinarySerializer<OperationResult<int>>();
            var ms = new MemoryStream();

            serializer.Serialize(ms, ok);
            ms.Rewind();

            var result = serializer.Deserialize(ms);
            Assert.True(result.Succeeded);
        }

        [Fact]
        public void CanRoundTripUndefinedOperationResultWithValue()
        {
            var undefined = OperationResult<int>.Failed();
            var serializer = new BinarySerializer<OperationResult<int>>();
            var ms = new MemoryStream();

            serializer.Serialize(ms, undefined);
            ms.Rewind();

            var result = serializer.Deserialize(ms);
            Assert.False(undefined.Succeeded);
            Assert.NotNull(undefined.Errors);
            Assert.Empty(undefined.Errors);
        }

        [Fact]
        public void CanRoundTripFailedOperationResultWithValue()
        {
          var failed = OperationResult<int>.Failed(new OperationError { Code = "Error" });
            var serializer = new BinarySerializer<OperationResult<int>>();
            var ms = new MemoryStream();

            serializer.Serialize(ms, failed);
            ms.Rewind();

            var result = serializer.Deserialize(ms);
            Assert.False(result.Succeeded);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Error", result.Errors[0].Code);
        }


        [Fact]
        public void DefaultOperationResultWithValueIsUndefined()
        {
            OperationResult<int> result = default;

            Assert.False(result.Succeeded);
            Assert.NotNull(result.Errors);
            Assert.Empty(result.Errors);
            Assert.False(result.HasValue);
            Assert.Throws<InvalidOperationException>(() => {
                int value = result.Value;
            });
        }

    }
}
