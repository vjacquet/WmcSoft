﻿using System;
using Xunit;

namespace WmcSoft.Monitoring.Sentries
{
    public class SentryTests
    {
        [Fact]
        public void ObserveCurrentStatusWhenSubscribing()
        {
            var subject = new SubjectSentry("subject", SentryStatus.Success);
            var mock = new MockObserver();
            using (subject.Subscribe(mock)) {
                Assert.Equal(1, mock.ObservedValues);
                Assert.Equal(mock.LatestValue, subject.Status);
            }
        }

        [Fact]
        public void OnObservingIsCalledOnceOnMultipleSubscription()
        {
            var sentry = new MockSentry("mock");
            var o1 = new MockObserver();
            var o2 = new MockObserver();

            using (sentry.Subscribe(o1))
            using (sentry.Subscribe(o2)) {
                Assert.Equal(1, sentry.OnObservingCalls);
                Assert.Equal(0, sentry.OnObservedCalls);

                Assert.Equal(2, sentry.OnSubscribeCalls);
                Assert.Equal(0, sentry.OnUnsubscribeCalls);
            }

            Assert.Equal(1, sentry.OnObservingCalls);
            Assert.Equal(1, sentry.OnObservedCalls);

            Assert.Equal(2, sentry.OnSubscribeCalls);
            Assert.Equal(2, sentry.OnUnsubscribeCalls);
        }

        [Fact]
        public void OnCompletedUnsubscribesAllObservers()
        {
            var sentry = new MockSentry("mock");
            var o1 = new MockObserver();
            var o2 = new MockObserver();

            using (sentry.Subscribe(o1))
            using (sentry.Subscribe(o2)) {
                sentry.SetNext(SentryStatus.Success);
                Assert.Equal(0, sentry.OnUnsubscribeCalls);
                Assert.Equal(0, sentry.OnObservedCalls);

                sentry.NotifiyCompleted();
                Assert.Equal(2, sentry.OnUnsubscribeCalls);
                Assert.Equal(1, sentry.OnObservedCalls);
                Assert.Equal(SentryStatus.None, o1.LatestValue);
                Assert.Equal(SentryStatus.None, o2.LatestValue);

                sentry.SetNext(SentryStatus.Error);
                Assert.Equal(SentryStatus.None, o1.LatestValue);
                Assert.Equal(SentryStatus.None, o2.LatestValue);

                Assert.True(o1.IsCompleted);
                Assert.True(o2.IsCompleted);
            }
            Assert.Equal(2, sentry.OnUnsubscribeCalls);
            Assert.Equal(1, sentry.OnObservedCalls);
        }

        [Fact]
        public void OnErrorUnsubscribesAllObservers()
        {
            var sentry = new MockSentry("mock");
            var o1 = new MockObserver();
            var o2 = new MockObserver();

            var error = new InvalidOperationException();

            using (sentry.Subscribe(o1))
            using (sentry.Subscribe(o2)) {
                sentry.SetNext(SentryStatus.Success);
                Assert.Equal(0, sentry.OnUnsubscribeCalls);
                Assert.Equal(0, sentry.OnObservedCalls);

                sentry.NotifyError(error);
                Assert.Equal(2, sentry.OnUnsubscribeCalls);
                Assert.Equal(1, sentry.OnObservedCalls);
                Assert.Equal(SentryStatus.Error, o1.LatestValue);
                Assert.Equal(SentryStatus.Error, o2.LatestValue);

                sentry.SetNext(SentryStatus.Success);
                Assert.Equal(SentryStatus.Error, o1.LatestValue);
                Assert.Equal(SentryStatus.Error, o2.LatestValue);

                Assert.Equal(error, o1.Error);
                Assert.Equal(error, o2.Error);
            }
            Assert.Equal(2, sentry.OnUnsubscribeCalls);
            Assert.Equal(1, sentry.OnObservedCalls);
        }

        [Fact]
        public void CanUnsubscribeOnError()
        {
            var sentry = new MockSentry("mock");

            var subscription = sentry.Subscribe(new MockObserver());
            using (sentry.Subscribe(new GarbageCollectorObserver(subscription))) {
                sentry.NotifyError(new InvalidOperationException());
            }

            Assert.Equal(SentryStatus.Error, sentry.Status);
        }
    }
}
