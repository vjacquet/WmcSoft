using System.Data;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Data
{
    [TestClass]
    public class DataRowCollectionExtensionsTests
    {
        [TestMethod]
        public void CanQueryDataRowCollection()
        {
            var dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Age", typeof(int));
            dt.Rows.Add("tintin", 32);
            dt.Rows.Add("milou", 5);

            var query = from r in dt
                        where "tintin" == r.Field<string>("Name")
                        select r;
            Assert.IsTrue(query.Count() == 1);
        }
    }
}
