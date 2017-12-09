using Xunit;

namespace WmcSoft
{
    public class SubscriptionTests
    {
        [Fact]
        public void CanUnsubscribe()
        {
            int count = 0;

            var subscripton = new Subscription(() => count++);
            Assert.Equal(0, count);

            subscripton.Unsubscribe();
            Assert.Equal(1, count);

            subscripton.Unsubscribe();
            Assert.Equal(1, count);
        }
    }
}
