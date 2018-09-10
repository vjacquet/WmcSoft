using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Xunit;

namespace WmcSoft.Business
{
    public class ResourcesTests
    {
        [Fact]
        public void CanGetRegistrationPolicyLocalizedDisplayName()
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            var uiCulture = Thread.CurrentThread.CurrentUICulture;
            var french = CultureInfo.GetCultureInfo("fr-fr");
            try {
                Thread.CurrentThread.CurrentCulture = french;
                Thread.CurrentThread.CurrentUICulture = french;

                var mandatory = typeof(RegistrationPolicy).GetField("Mandatory");
                var displayName = mandatory.GetCustomAttribute<DisplayAttribute>();
                var actual = displayName.GetName();
                Assert.Equal("Obligatoire", actual);

                //var properties = TypeDescriptor.GetProperties(typeof(RegistrationPolicy));
                //var mandatory = properties["Mandatory"];
                //var displayName = mandatory.DisplayName;

            } finally {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = uiCulture;
            }
        }
    }
}
