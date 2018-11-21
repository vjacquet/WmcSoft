using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace WmcSoft.Diagnostics
{
    public class TraceSessionTests
    {
        [Fact]
        public void CanTraceSession()
        {
            var encoding = Encoding.Unicode;
            var ms = new MemoryStream(4096);
            using (var writer = new StreamWriter(ms, encoding, 4096, true)) {
                var source = new TraceSource("Timing");
                source.Switch.Level = SourceLevels.All;
                source.Listeners.Add(new TextWriterTraceListener(writer));

                using (var session = new TraceSession(source, typeof(TraceSessionTests))) {
                    source.TraceVerbose("Hello, world!");
                    Thread.Sleep(100);
                }
            }

            ms.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(ms, encoding)) {
                Assert.Matches(@"Timing Information: 0 : Executing WmcSoft.Diagnostics.TraceSessionTests version [0-9].*", reader.ReadLine());
                Assert.Equal(@"Timing Verbose: 0 : Hello, world!", reader.ReadLine());
                Assert.Matches(@"Timing Information: 0 : Executed in [0-9]*ms\.", reader.ReadLine());
            }
        }
    }
}
