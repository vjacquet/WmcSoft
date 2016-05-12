using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Business.PartyModel
{
    [TestClass]
    public class GeographicAddressTests
    {
        [TestMethod]
        public void CheckFrenchAddress() {
            var address = new GeographicAddress {
                AddressLines = new[] { "12 rue de la montagne" },
                ZipOrPostCode = "01234",
                City = "Exampleville",
            };
        }
    }
}
