using System.IO;
using Xunit;

namespace WmcSoft.IO
{
    public class ClosableStreamTests
    {
        [Fact]
        public void StreamIsNotClosedWhenClosingIsCancelled()
        {
            var ms = new MemoryStream();
            var closable = new ClosableStream(ms);
            var closedCount = 0;
            var closingCount = 0;

            var cancel = true;
            closable.Closing += (sender, e) => { e.Cancel = cancel; closingCount++; };
            closable.Closed += (sender, e) => { closedCount++; };

            closable.Close();
            Assert.Equal(1, closingCount);
            Assert.Equal(0, closedCount);

            cancel = false;
            closable.Close();
            Assert.Equal(2, closingCount);
            Assert.Equal(1, closedCount);
        }

        [Fact]
        public void StreamIsClosedOnlyOnce()
        {
            var ms = new MemoryStream();
            var closable = new ClosableStream(ms);
            var count = 0;

            closable.Closed += (sender, e) => { count++; };

            closable.Close();
            Assert.Equal(1, count);

            closable.Close();
            Assert.Equal(1, count);
        }

        [Fact]
        public void ClosingStreamWhenNoEventsAreRegistered()
        {
            var ms = new MemoryStream();
            var closable = new ClosableStream(ms);

            closable.Close();
            Assert.True(closable.IsClosed);
        }

        [Fact]
        public void IsClosedReflectsStreamState()
        {
            var ms = new MemoryStream();
            var closable = new ClosableStream(ms);

            Assert.False(closable.IsClosed);
            closable.Close();
            Assert.True(closable.IsClosed);
        }
    }
}
