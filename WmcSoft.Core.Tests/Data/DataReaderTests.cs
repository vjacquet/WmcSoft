using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Data
{
    [TestClass]
    public class DataReaderTests
    {
        Model Make(int i, string s)
        {
            return new Model(i, s);
        }

        [TestMethod]
        public void Test()
        {
            var materializer = DbCommandExtensions.MakeMaterializer((int i, string s) => new Model(i, s));
            var reader = new DataReaderAdapter(1, "two");
            var actual = materializer(reader);
            Assert.AreEqual(1, actual.First);
            Assert.AreEqual("two", actual.Second);
        }
    }

    class Model
    {
        public Model(int i, string s)
        {
            First = i;
            Second = s;
        }

        public int First { get; private set; }
        public string Second { get; private set; }
    }

    class DataReaderAdapter : DataReaderBase
    {
        bool _read;
        readonly object[] _data;

        public DataReaderAdapter(params object[] data)
        {
            _read = false;
            _data = data;
        }

        public override int FieldCount
        {
            get { return _data.Length; }
        }

        public override Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public override string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public override int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(int i)
        {
            return _data[i];
        }

        public override bool Read()
        {
            if (!_read)
            {
                _read = true;
                return true;
            }
            return false;
        }
    }
}
