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
    }
}
