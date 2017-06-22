using Xunit;

namespace WmcSoft.ComponentModel.Design
{
    public class DictionaryServiceTests
    {
        [Fact]
        public void CanAcceptChanges()
        {
            var svc = new DictionaryService();
            svc.SetValue("firstName", "vincent");
            svc.SetValue("lastName", "Jacquet");
            svc.SetValue("job", "developper");

            var uow = new RevertibleChangeTrackingDictionaryService(svc);
            uow.SetValue("lastName", "JACQUET");
            uow.RemoveValue("job");
            uow.SetValue("code", "c#");

            Assert.Equal("vincent", uow.GetValue("firstName"));
            Assert.Equal("JACQUET", uow.GetValue("lastName"));
            Assert.Null(uow.GetValue("job"));
            Assert.Equal("c#", uow.GetValue("code"));

            Assert.NotEqual(uow.GetValue("lastName"), svc.GetValue("lastName"));
            Assert.NotNull(svc.GetValue("job"));
            Assert.Null(svc.GetValue("code"));

            uow.AcceptChanges();

            Assert.Equal("vincent", svc.GetValue("firstName"));
            Assert.Equal("JACQUET", svc.GetValue("lastName"));
            Assert.Null(svc.GetValue("job"));
            Assert.Equal("c#", svc.GetValue("code"));
        }

        [Fact]
        public void CanRevertChanges()
        {
            var svc = new DictionaryService();
            svc.SetValue("firstName", "vincent");
            svc.SetValue("lastName", "Jacquet");
            svc.SetValue("job", "developper");

            var uow = new RevertibleChangeTrackingDictionaryService(svc);
            uow.SetValue("lastName", "JACQUET");
            uow.RemoveValue("job");
            uow.SetValue("code", "c#");

            Assert.Equal("vincent", uow.GetValue("firstName"));
            Assert.Equal("JACQUET", uow.GetValue("lastName"));
            Assert.Null(uow.GetValue("job"));
            Assert.Equal("c#", uow.GetValue("code"));

            Assert.NotEqual(uow.GetValue("lastName"), svc.GetValue("lastName"));
            Assert.NotNull(svc.GetValue("job"));
            Assert.Null(svc.GetValue("code"));

            uow.RejectChanges();

            Assert.Equal("vincent", svc.GetValue("firstName"));
            Assert.Equal("Jacquet", svc.GetValue("lastName"));
            Assert.NotNull(svc.GetValue("job"));
            Assert.Null(svc.GetValue("code"));
        }
    }
}
