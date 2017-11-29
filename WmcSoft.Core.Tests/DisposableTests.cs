using System;
using Xunit;

namespace WmcSoft
{
    public class DisposableTests
    {
        [Fact]
        public void CheckDisposableStackDisposeInReverseOrder()
        {
            int a = 0;
            int b = 0;
            int c = 0;
            int sequence = 0;

            var bin = new DisposableStack {
                new Disposer(() => a = ++sequence),
                new Disposer(() => b = ++sequence),
                new Disposer(() => c = ++sequence),
            };
            bin.Dispose();

            Assert.Equal(1, c);
            Assert.Equal(2, b);
            Assert.Equal(3, a);
        }

        [Fact]
        public void DisposableStackDoesNotThrowOnNull()
        {
            int sequence = 0;
            var bin = new DisposableStack {
                new Disposer(() => ++sequence),
                null,
                new Disposer(() => ++sequence),
            };
            bin.Dispose();

            Assert.Equal(2, sequence);
        }

        [Fact]
        public void DisposableStackDisposeIsIdempotent()
        {
            int sequence = 0;
            var bin = new DisposableStack {
                new Disposer(() => ++sequence),
                new Disposer(() => ++sequence),
            };

            bin.Dispose();
            Assert.Equal(2, sequence);

            bin.Dispose();
            Assert.Equal(2, sequence);
        }

        [Fact]
        public void DisposableStackDoesNotAddNull()
        {
            int sequence = 0;
            var bin = new DisposableStack {
                new Disposer(() => ++sequence),
                new Disposer(() => ++sequence),
            };

            Assert.False(bin.Add(null));
        }

        [Fact]
        public void DisposableStackDoesAddNDuplicate()
        {
            int sequence = 0;
            var one = new Disposer(() => ++sequence);
            var bin = new DisposableStack {
                one,
                new Disposer(() => ++sequence),
            };

            Assert.True(bin.Add(one));
        }

        [Fact]
        public void DisposableSetDoesNotThrowOnNull()
        {
            int sequence = 0;
            var bin = new DisposableSet {
                new Disposer(() => ++sequence),
                null,
                new Disposer(() => ++sequence),
            };
            bin.Dispose();

            Assert.Equal(2, sequence);
        }

        [Fact]
        public void DisposableSetDisposeIsIdempotent()
        {
            int sequence = 0;
            var bin = new DisposableSet {
                new Disposer(() => ++sequence),
                new Disposer(() => ++sequence),
            };

            bin.Dispose();
            Assert.Equal(2, sequence);

            bin.Dispose();
            Assert.Equal(2, sequence);
        }

        [Fact]
        public void DisposableSetDoesNotAddNull()
        {
            int sequence = 0;
            var bin = new DisposableSet {
                new Disposer(() => ++sequence),
                new Disposer(() => ++sequence),
            };

            Assert.False(bin.Add(null));
        }

        [Fact]
        public void DisposableSetDoesNotAddDuplicate()
        {
            int sequence = 0;
            var one = new Disposer(() => ++sequence);
            var bin = new DisposableSet {
                one,
                new Disposer(() => ++sequence),
            };

            Assert.False(bin.Add(one));
        }

        static Disposer Create(Action action)
        {
            return new Disposer(action);
        }

        [Fact]
        public void CanThrowInBin()
        {
            int sequence = 0;
            using (var bin = new DisposableSet()) {
                var one = Create(() => ++sequence).ThrowIn(bin);
                var two = bin.Push(Create(() => ++sequence)); 
            }
            Assert.Equal(2, sequence);
        }
    }
}
