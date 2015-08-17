using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WmcSoft;
using WmcSoft.Diagnostics;
using WmcSoft.IO;

namespace ConsoleApplication
{
    class Program
    {
        #region Invariants

        static readonly char[] OptionMarks = new char[] { '+', '-', '/' }; // /!\ must stay sorted

        #endregion

        #region Entry point

        static void Main(string[] args) {
            var traceSource = new TraceSource("Program");

            using (new TraceSession(typeof(Program))) {
                // reading arguments & inputs
                var inputs = new List<string>();
                for (int i = 0; i < args.Length; i++) {
                    var arg = args[i];
                    if (string.IsNullOrWhiteSpace(arg))
                        continue;
                    if (arg.FirstOrDefault().BinaryAny(OptionMarks)) {
                        switch (arg[1]) {
                        case 'v':
                            //program.Verbose = true;
                            break;
                        default:
                            Console.Error.WriteLines(Properties.Resources.Usage.Split('\r', '\n'));
                            return;
                        }
                    } else {
                        // add input support here
                        inputs.Add(arg);
                    }
                }

                var waitHandle = new AutoResetEvent(false);
                var backgroundWorker = new BackgroundWorker {
                    WorkerSupportsCancellation = true,
                };
                backgroundWorker.RunWorkerCompleted += (sender, e) => {
                    if (e.Error != null) {
                        traceSource.TraceError(0, e.Error.Message);
                    }
                    waitHandle.Set();
                };

                Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e) {
                    if (!backgroundWorker.CancellationPending) {
                        backgroundWorker.CancelAsync();
                        e.Cancel = true;
                    }
                };

                backgroundWorker.DoWork += (sender, e) => {
                    throw new NotImplementedException();

                };

                backgroundWorker.RunWorkerAsync();

                waitHandle.WaitOne();
            }
        }

        #endregion

    }
}
