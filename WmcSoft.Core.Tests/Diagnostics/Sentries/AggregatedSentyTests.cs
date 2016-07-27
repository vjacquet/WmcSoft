using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Diagnostics.Sentries
{
    /// <summary>
    /// Summary description for AggregatedSentyTests
    /// </summary>
    [TestClass]
    public class AggregatedSentyTests
    {
        class InstrumentedSentry : SentryBase
        {
            public InstrumentedSentry(string name) : base(name) {
            }

            public bool Subscribed { get; private set; }
            public bool Unsubscribed { get; private set; }

            protected override void OnObserving() {
                base.OnObserving();
                Subscribed = true;
            }
            protected override void OnObserved() {
                base.OnObserved();
                Unsubscribed = true;
            }
        }

        class BasicObserver : IObserver<SentryStatus>
        {
            public void OnCompleted() {
            }

            public void OnError(Exception error) {
            }

            public void OnNext(SentryStatus value) {
            }
        }

        [TestMethod]
        public void CheckSubscriptionCycle() {
            var sentries = new InstrumentedSentry[] { new InstrumentedSentry("a"), new InstrumentedSentry("b") };
            var aggregated = new AggregateSentry("ab", sentries);

            Assert.IsFalse(sentries[0].Subscribed);
            Assert.IsFalse(sentries[1].Subscribed);
            Assert.IsFalse(sentries[0].Unsubscribed);
            Assert.IsFalse(sentries[1].Unsubscribed);

            using (aggregated.Subscribe(new BasicObserver())) {
                Assert.IsTrue(sentries[0].Subscribed);
                Assert.IsTrue(sentries[1].Subscribed);
                Assert.IsFalse(sentries[0].Unsubscribed);
                Assert.IsFalse(sentries[1].Unsubscribed);
            }

            Assert.IsTrue(sentries[0].Subscribed);
            Assert.IsTrue(sentries[1].Subscribed);
            Assert.IsTrue(sentries[0].Unsubscribed);
            Assert.IsTrue(sentries[1].Unsubscribed);
        }
    }
}
