using System;
using System.Data;
using Xunit;

namespace WmcSoft.Data
{
    public class DataReaderTests
    {
        Model Make(int i, string s)
        {
            return new Model(i, s);
        }

        [Fact]
        public void CanMaterialize()
        {
            var materializer = DbCommandExtensions.MakeMaterializer((int i, string s) => new Model(i, s));
            var reader = new DataReaderAdapter(1, "two");
            var actual = materializer(reader);
            Assert.Equal(1, actual.First);
            Assert.Equal("two", actual.Second);
        }

        [Fact]
        public void CanFillTable()
        {
            var reader = new TypeDescriptorDataReader<Model>(new[] {
                new Model(1, "one"),
                new Model(2, "two"),
                new Model(3, "three"),
            });

            var dt = new DataTable();
            dt.Load(reader);

            Assert.Equal(2, dt.Columns.Count);
            Assert.Equal("First", dt.Columns[0].ColumnName);
            Assert.Equal(typeof(int), dt.Columns[0].DataType);
            Assert.Equal("Second", dt.Columns[1].ColumnName);
            Assert.Equal(typeof(string), dt.Columns[1].DataType);

            Assert.Equal(3, dt.Rows.Count);
            Assert.Equal("two", dt.Rows[1][1]);
        }

        [Fact]
        public void CheckHasRowsForAdapter()
        {
            var underlying = new TypeDescriptorDataReader<Model>(new[] {
                new Model(1, "one"),
                new Model(2, "two"),
                new Model(3, "three"),
            });
            var reader = underlying.AsDbDataReader();
            Assert.True(reader.HasRows);
            var count = 0;
            while (reader.Read())
                count++;
            Assert.Equal(3, count);
        }

        [Fact]
        public void CheckReadForAdapter()
        {
            var underlying = new TypeDescriptorDataReader<Model>(new[] {
                new Model(1, "one"),
                new Model(2, "two"),
                new Model(3, "three"),
            });
            var reader = underlying.AsDbDataReader();
            var count = 0;
            while (reader.Read())
                count++;
            Assert.Equal(3, count);
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

        public override int FieldCount {
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
            if (!_read) {
                _read = true;
                return true;
            }
            return false;
        }
    }
}
