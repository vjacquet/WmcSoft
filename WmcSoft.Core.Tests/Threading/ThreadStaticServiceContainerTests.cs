using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Threading
{
    [TestClass]
    public class ThreadStaticServiceContainerTests
    {
        class Snitch : IDisposable
        {
            ConcurrentQueue<string> _calls;

            public Snitch() {
                _calls = new ConcurrentQueue<string>();
            }

            public Snitch Call(string value) {
                _calls.Enqueue(value);
                return this;
            }
            public string[] CalledBy() {
                return _calls.ToArray();
            }

            public void Dispose() {
                Call("Disposed");
            }
        }

        [TestMethod]
        public void CheckDisposeOnServices() {
            var snitch = new Snitch();
            var manual = new ManualResetEvent(false);
            using (var serviceContainer = new ThreadStaticServiceContainer()) {
                serviceContainer.AddService(typeof(ManualResetEvent), manual);
                serviceContainer.AddService(typeof(Snitch), (container, serviceType) => snitch.Call("Created"));
                ThreadPool.QueueUserWorkItem(state => Run((IServiceProvider)state), serviceContainer);
                manual.WaitOne();
            }
            var expected = new[] { "Created", "Disposed" };
            var actual = snitch.CalledBy();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckDisposeOnServicesNotRun() {
            var snitch = new Snitch();
            using (var serviceContainer = new ThreadStaticServiceContainer()) {
                serviceContainer.AddService(typeof(Snitch), (container, serviceType) => snitch.Call("Created"));
            }
            var expected = new string[0];
            var actual = snitch.CalledBy();
            CollectionAssert.AreEqual(expected, actual);
        }

        static void Run(IServiceProvider provider) {
            var manual = provider.GetService<ManualResetEvent>();
            var snitch = provider.GetService<Snitch>();
            manual.Set();
        }
    }
}