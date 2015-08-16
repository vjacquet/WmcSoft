using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Data
{
    [TestClass]
    public class DataReaderTests
    {
        Model Make(int i, string s) {
            return null;
        }

        [TestMethod]
        public void Test() {
            var materializer = DbCommandExtensions.MakeMaterializer((int i, string s) => new Model(i,s));
        }
    }

    class Model
    {
        public Model(int i, string s) {

        }
    }
}
