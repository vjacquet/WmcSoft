using System;
using System.Linq;
using Xunit;

namespace WmcSoft.Monitoring.Sentries
{
    public class AggregatedSentyTests
    {
        [Fact]
        public void AggregateSentryCanObserveItsChildren()
        {
            var a = new MockSentry("a");
            var b = new MockSentry("b");
            var aggregated = new AggregateSentry("ab", a, b);

            Assert.Equal(0, a.OnObservingCalls);
            Assert.Equal(0, b.OnObservingCalls);

            Assert.Equal(0, a.OnObservedCalls);
            Assert.Equal(0, b.OnObservedCalls);

            using (aggregated.Subscribe(new NullObserver())) {
                Assert.Equal(1, a.OnObservingCalls);
                Assert.Equal(1, b.OnObservingCalls);

                Assert.Equal(0, a.OnObservedCalls);
                Assert.Equal(0, b.OnObservedCalls);
            }

            Assert.Equal(1, a.OnObservingCalls);
            Assert.Equal(1, b.OnObservingCalls);

            Assert.Equal(1, a.OnObservedCalls);
            Assert.Equal(1, b.OnObservedCalls);
        }

        [Fact]
        public void AggregateStatusReportsErrorWhenAChildNotifiesOnError()
        {
            var a = new MockSentry("a");
            var b = new MockSentry("b");
            var aggregated = new WorstStatusAggregateSentry("ab", a, b);
            var observer = new MockObserver();

            using (aggregated.Subscribe(observer)) {
                a.SetNext(SentryStatus.Success);
                b.SetNext(SentryStatus.Success);

                Assert.Equal(SentryStatus.Success, observer.LatestValue);

                a.NotifyError(new InvalidOperationException());
                Assert.Equal(SentryStatus.Error, observer.LatestValue);

                Assert.Equal(1, a.OnObservedCalls);
                Assert.Equal(0, b.OnObservedCalls);
            }
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
            var sentries = InterpretSequence(sequence);
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
            var sentries = InterpretSequence(sequence);
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
            var sentries = InterpretSequence(sequence);
            var aggregated = new BestStatusAggregateSentry("b", sentries);

            using (aggregated.Subscribe(new NullObserver())) {
                Assert.Equal(expected, aggregated.Status);
            }
        }

        #region Helpers

        static SentryStatus Parse(string value)
        {
            return (SentryStatus)Enum.Parse(typeof(SentryStatus), value);
        }

        static StubSentry[] InterpretSequence(string sequence)
        {
            var statuses = Array.ConvertAll(sequence.Split(' '), Parse);
            var sentries = statuses.Select((s, i) => new StubSentry("#" + i, s));
            return sentries.ToArray();
        }

        #endregion
    }
}
