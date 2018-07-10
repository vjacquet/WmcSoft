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
            var sentries = new [] { new MockSentry("a"), new MockSentry("b") };
            var aggregated = new AggregateSentry("ab", sentries);

            Assert.Equal(0, sentries[0].OnObservingCalls);
            Assert.Equal(0, sentries[1].OnObservingCalls);
            Assert.Equal(0, sentries[0].OnObservedCalls);
            Assert.Equal(0, sentries[1].OnObservedCalls);

            using (aggregated.Subscribe(new NullObserver())) {
                Assert.Equal(1, sentries[0].OnObservingCalls);
                Assert.Equal(1, sentries[1].OnObservingCalls);
                Assert.Equal(0,sentries[0].OnObservedCalls);
                Assert.Equal(0, sentries[1].OnObservedCalls);
            }

            Assert.Equal(1, sentries[0].OnObservingCalls);
            Assert.Equal(1, sentries[1].OnObservingCalls);
            Assert.Equal(1, sentries[0].OnObservedCalls);
            Assert.Equal(1, sentries[1].OnObservedCalls);
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

            using (aggregated.Subscribe(new NullObserver())) {
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

            using (aggregated.Subscribe(new NullObserver())) {
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

            using (aggregated.Subscribe(new NullObserver())) {
                Assert.Equal(expected, aggregated.Status);
            }
        }
    }
}
