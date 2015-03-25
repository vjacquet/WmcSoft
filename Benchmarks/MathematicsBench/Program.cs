using System.Diagnostics;
using System.Reflection;
using WmcSoft.Benchmark;
using WmcSoft.Diagnostics;

class Program
{
    public static void Main(string[] args) {
        var assembly = Assembly.GetCallingAssembly();
        using (new TraceSession(assembly)) {
            // Save all the benchmark classes from doing a nullity test
            if (args == null)
                args = new string[0];

            var traceSource = new TraceSource("Microbench");
            traceSource.TraceInformation("{1} resolution stopwatch. Frequency is {0} ticks/s", Stopwatch.Frequency, Stopwatch.IsHighResolution ? "High" : "Low");

            var engine = new MicrobenchEngine(traceSource);
            var discovery = new AssemblyBenchmarkDiscovery(assembly);
            foreach (var benchmark in discovery) {
                engine.Run(benchmark, args);
            }
        }
    }
}
