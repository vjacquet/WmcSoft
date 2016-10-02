using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.IO;
using WmcSoft.Runtime.Serialization;

namespace WmcSoft.Business
{
    [TestClass]
    public class GatewayResultTests
    {
        [TestMethod]
        public void CanConstructGatewayResult() {
            var ok = GatewayResult.Success;
            var undefined = GatewayResult.Failed();
            var failed = GatewayResult.Failed(new GatewayError { Code = "Error" });

            Assert.IsTrue(ok.Succeeded);
            Assert.IsFalse(undefined.Succeeded);
            Assert.IsNotNull(undefined.Errors);
            Assert.AreEqual(0, undefined.Errors.Count);
            Assert.IsFalse(failed.Succeeded);
            Assert.AreEqual(1, failed.Errors.Count);
        }

        [TestMethod]
        public void CanRoundTripSuccessGatewayResult() {
            var ok = GatewayResult.Success;
            var serializer = new BinarySerializer<GatewayResult>();
            var ms = new MemoryStream();

            serializer.Serialize(ms, ok);
            ms.Rewind();

            var result = serializer.Deserialize(ms);
            Assert.IsTrue(result.Succeeded);
        }
    }
}
