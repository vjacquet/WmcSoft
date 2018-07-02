using System;
using System.Linq;
using Xunit;

namespace WmcSoft.Monitoring.Sentries
{
    public class AggregatedSentyTests
    {
        [Fact]
        public void CheckSubscriptionCycle()
        {
            var sentries = new InstrumentedSentry[] { new InstrumentedSentry("a"), new InstrumentedSentry("b") };
            var aggregated = new AggregateSentry("ab", sentries);

            Assert.False(sentries[0].Subscribed);
            Assert.False(sentries[1].Subscribed);
            Assert.False(sentries[0].Unsubscribed);
            Assert.False(sentries[1].Unsubscribed);

            using (aggregated.Subscribe(new BasicObserver())) {
                Assert.True(sentries[0].Subscribed);
                Assert.True(sentries[1].Subscribed);
                Assert.False(sentries[0].Unsubscribed);
                Assert.False(sentries[1].Unsubscribed);
            }

            Assert.True(sentries[0].Subscribed);
            Assert.True(sentries[1].Subscribed);
            Assert.True(sentries[0].Unsubscribed);
            Assert.True(sentries[1].Unsubscribed);
        }

        [Theory]
        [InlineData("Success Warning Warning None", SentryStatus.Warning)]
        [InlineData("Success Warning Warning Error", SentryStatus.Warning)]
        [InlineData("Success Success Success Error", SentryStatus.Warning)]
        [InlineData("Error Error Error Error", SentryStatus.Error)]
        [InlineData("Success Success Success Success", SentryStatus.Success)]
        [InlineData("Success None None None", SentryStatus.Success)]
        [InlineData("None None None None", SentryStatus.None)]
        [InlineData("Warning Error", SentryStatus.Warning)]
        public void CanAggregate(string sequence, SentryStatus expected)
        {
            var sentries = sequence.Split(' ').Select((s, i) => new StubSentry("#" + i, (SentryStatus)Enum.Parse(typeof(SentryStatus), s))).ToArray();
            var aggregated = new AggregateSentry("a", sentries);

            using (aggregated.Subscribe(new BasicObserver())) {
                Assert.Equal(expected, aggregated.Status);
            }
        }

        [Theory]
        [InlineData("Success Warning Warning None", SentryStatus.Warning)]
        [InlineData("Success Warning Warning Error", SentryStatus.Error)]
        [InlineData("Success Success Success Error", SentryStatus.Error)]
        [InlineData("Error Error Error Error", SentryStatus.Error)]
        [InlineData("Success Success Success Success", SentryStatus.Success)]
        [InlineData("Success None None None", SentryStatus.Success)]
        [InlineData("None None None None", SentryStatus.None)]
        [InlineData("Warning Error", SentryStatus.Error)]
        public void CanWorstStatusAggregate(string sequence, SentryStatus expected)
        {
            var sentries = sequence.Split(' ').Select((s, i) => new StubSentry("#" + i, (SentryStatus)Enum.Parse(typeof(SentryStatus), s))).ToArray();
            var aggregated = new WorstStatusAggregateSentry("w", sentries);

            using (aggregated.Subscribe(new BasicObserver())) {
                Assert.Equal(expected, aggregated.Status);
            }
        }

        [Theory]
        [InlineData("Success Warning Warning None", SentryStatus.Success)]
        [InlineData("Success Warning Warning Error", SentryStatus.Success)]
        [InlineData("Success Success Success Error", SentryStatus.Success)]
        [InlineData("Error Error Error Error", SentryStatus.Error)]
        [InlineData("Success Success Success Success", SentryStatus.Success)]
        [InlineData("Success None None None", SentryStatus.Success)]
        [InlineData("None None None None", SentryStatus.None)]
        [InlineData("Warning Error", SentryStatus.Warning)]
        public void CanBestStatusAggregate(string sequence, SentryStatus expected)
        {
            var sentries = sequence.Split(' ').Select((s, i) => new StubSentry("#" + i, (SentryStatus)Enum.Parse(typeof(SentryStatus), s))).ToArray();
            var aggregated = new BestStatusAggregateSentry("b", sentries);

            using (aggregated.Subscribe(new BasicObserver())) {
                Assert.Equal(expected, aggregated.Status);
            }
        }
    }
}
