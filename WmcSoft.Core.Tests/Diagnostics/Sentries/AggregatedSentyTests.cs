﻿using System;
using Xunit;

namespace WmcSoft.Diagnostics.Sentries
{
    public class AggregatedSentyTests
    {
        class InstrumentedSentry : SentryBase
        {
            public InstrumentedSentry(string name) : base(name)
            {
            }

            public bool Subscribed { get; private set; }
            public bool Unsubscribed { get; private set; }

            protected override void OnObserving()
            {
                base.OnObserving();
                Subscribed = true;
            }
            protected override void OnObserved()
            {
                base.OnObserved();
                Unsubscribed = true;
            }
        }

        class BasicObserver : IObserver<SentryStatus>
        {
            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void OnNext(SentryStatus value)
            {
            }
        }

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
    }
}