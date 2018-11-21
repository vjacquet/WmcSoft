using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace WmcSoft.Diagnostics
{
    public class TimingTraceTests
    {
        [Fact]
        public void CanTimeTrace()
        {
            var encoding = Encoding.Unicode;
            var ms = new MemoryStream(4096);
            using (var writer = new StreamWriter(ms, encoding, 4096, true)) {
                var source = new TraceSource("Timing");
                source.Switch.Level = SourceLevels.All;
                source.Listeners.Add(new TextWriterTraceListener(writer));

                using (var tracer = new TimingTrace(source, "Trace")) {
                    Thread.Sleep(100);
                }
            }

            ms.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(ms, encoding)) {
                var line = reader.ReadLine();
                Assert.Matches("Timing Information: 0 : [0-9]*ms\tTrace", line);
            }
        }
    }
}
