using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WmcSoft.Diagnostics;

namespace ConsoleApplication
{
    class Program
    {
        #region Entry point

        static void Main(string[] args) {
            var traceSource = new TraceSource("Program");

            using (new TraceSession(typeof(Program))) {
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
