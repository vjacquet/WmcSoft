using System.Diagnostics;
using System.Reflection;
using WmcSoft.Benchmark;

class Program
{
    public static void Main(string[] args) {
        // Save all the benchmark classes from doing a nullity test
        var assembly = Assembly.GetCallingAssembly();
        if (args == null)
            args = new string[0];

        var traceSource = new TraceSource("Microbench");
        traceSource.TraceInformation("{1} resolution stopwatch. Frequency is {0} ticks/s", Stopwatch.Frequency, Stopwatch.IsHighResolution ? "High" : "Low");

        var engine = new MicrobenchEngine(traceSource);
        var discovery = new AssemblyBenchmarkDiscovery(assembly);
        foreach (var benchmark in discovery) {
            engine.Run(benchmark, args);
        }

        //foreach (MethodInfo method in runs) {
        //    try {
        //    }
        //    catch (TargetInvocationException e) {
        //        Exception inner = e.InnerException;
        //        string message = (inner == null ? null : inner.Message) ?? "(No message)";
        //        Console.WriteLine("  {0}: Failed ({1})", method.Name, message);
        //    }
        //}
    }
}
