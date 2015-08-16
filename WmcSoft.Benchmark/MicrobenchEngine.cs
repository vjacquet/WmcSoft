#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using WmcSoft.Collections.Generic;
using WmcSoft.Diagnostics;

namespace WmcSoft.Benchmark
{
    public class MicrobenchEngine
    {
        readonly TraceSource _trace;

        public MicrobenchEngine(TraceSource traceSource) {
            _trace = traceSource;
        }
        public MicrobenchEngine()
            : this(new TraceSource("Microbench")) {
        }

        public void Run(IBenchmarkDescriptor benchmark, params string[] args) {
            _trace.TraceInformation("Benchmarking {0}", benchmark.Name);

            var measures = benchmark.EnumerateMeasures()
                .Repeat(benchmark.Iterations).ToList();
            measures.Suffle(new Random(1789));

            try {
                benchmark.Init(args);
            }
            catch (TargetInvocationException e) {
                Exception inner = e.InnerException;
                string message = (inner == null ? null : inner.Message) ?? "No message";
                _trace.TraceError(1000, "  {0}: Failed ({1})", benchmark.Name, message);
                throw;
            }
            catch (Exception e) {
                _trace.TraceError(1000, "  {0}: Failed ({1})", benchmark.Name, e.Message);
                throw;
            }

            var results = new Dictionary<IMeasureDescriptor, double>();

            foreach (var measure in measures) {
                try {
                    benchmark.Reset();

                    // Give the test as good a chance as possible
                    // of avoiding garbage collection
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    // Now run the test itself
                    var stopwatch = Stopwatch.StartNew();
                    measure.Invoke();
                    stopwatch.Stop();

                    // Check the results (if appropriate)
                    // Note that this doesn't affect the timing
                    benchmark.Check();

                    // If everything's worked, report the time taken, 
                    // nicely lined up (assuming no very long method names!)
                    double duration;
                    if (results.TryGetValue(measure, out duration)) {
                        results[measure] = duration + stopwatch.Elapsed.TotalMilliseconds;
                    } else {
                        // discard first run
                        results.Add(measure, 0d);
                    }
                }
                catch (TargetInvocationException e) {
                    Exception inner = e.InnerException;
                    string message = (inner == null ? null : inner.Message) ?? "No message";
                    _trace.TraceError(1001, "  {0}: Failed ({1})", measure.Name, message);
                    throw;
                }
                catch (Exception e) {
                    _trace.TraceError(1001, "  {0}: Failed ({1})", measure.Name, e.Message);
                    throw;
                }
            }

            foreach (var measure in results.OrderBy(r => r.Key.Name)) {
                var duration = measure.Value / benchmark.Iterations;
                if (duration < 1d)
                    _trace.TraceInformation("  {0,-32} {1,10:N3}µs", measure.Key.Name, 1000d * duration);
                else
                    _trace.TraceInformation("  {0,-32} {1,10:N3}ms", measure.Key.Name, duration);
            }
        }
    }


}
