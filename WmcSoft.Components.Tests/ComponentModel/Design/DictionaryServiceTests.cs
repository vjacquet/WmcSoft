using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.ComponentModel.Design
{
    [TestClass]
    public class DictionaryServiceTests
    {
        [TestMethod]
        public void CanAcceptChanges() {
            var svc = new DictionaryService();
            svc.SetValue("firstName", "vincent");
            svc.SetValue("lastName", "Jacquet");
            svc.SetValue("job", "developper");

            var uow = new RevertibleChangeTrackingDictionaryService(svc);
            uow.SetValue("lastName", "JACQUET");
            uow.RemoveValue("job");
            uow.SetValue("code", "c#");

            Assert.AreEqual("vincent", uow.GetValue("firstName"));
            Assert.AreEqual("JACQUET", uow.GetValue("lastName"));
            Assert.IsNull(uow.GetValue("job"));
            Assert.AreEqual("c#", uow.GetValue("code"));

            Assert.AreNotEqual(uow.GetValue("lastName"), svc.GetValue("lastName"));
            Assert.IsNotNull(svc.GetValue("job"));
            Assert.IsNull(svc.GetValue("code"));

            uow.AcceptChanges();

            Assert.AreEqual("vincent", svc.GetValue("firstName"));
            Assert.AreEqual("JACQUET", svc.GetValue("lastName"));
            Assert.IsNull(svc.GetValue("job"));
            Assert.AreEqual("c#", svc.GetValue("code"));
        }

        [TestMethod]
        public void CanRevertChanges() {
            var svc = new DictionaryService();
            svc.SetValue("firstName", "vincent");
            svc.SetValue("lastName", "Jacquet");
            svc.SetValue("job", "developper");

            var uow = new RevertibleChangeTrackingDictionaryService(svc);
            uow.SetValue("lastName", "JACQUET");
            uow.RemoveValue("job");
            uow.SetValue("code", "c#");

            Assert.AreEqual("vincent", uow.GetValue("firstName"));
            Assert.AreEqual("JACQUET", uow.GetValue("lastName"));
            Assert.IsNull(uow.GetValue("job"));
            Assert.AreEqual("c#", uow.GetValue("code"));

            Assert.AreNotEqual(uow.GetValue("lastName"), svc.GetValue("lastName"));
            Assert.IsNotNull(svc.GetValue("job"));
            Assert.IsNull(svc.GetValue("code"));

            uow.RejectChanges();

            Assert.AreEqual("vincent", svc.GetValue("firstName"));
            Assert.AreEqual("Jacquet", svc.GetValue("lastName"));
            Assert.IsNotNull(svc.GetValue("job"));
            Assert.IsNull(svc.GetValue("code"));
        }
    }
}
