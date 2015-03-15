using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Benchmark
{
    public class AssemblyBenchmarkDiscovery : IEnumerable<IBenchmarkDescriptor>
    {
        private readonly Assembly _assembly;

        public AssemblyBenchmarkDiscovery(Assembly assembly) {
            _assembly = assembly;
        }

        #region IEnumerable<IBenchmarkDescriptor> Membres

        public IEnumerator<IBenchmarkDescriptor> GetEnumerator() {
            foreach (var type in _assembly.GetTypes()) {
                var benchmark = type.GetCustomAttributes(typeof(BenchmarkAttribute), false).Cast<BenchmarkAttribute>().FirstOrDefault();
                if (benchmark == null)
                    continue;

                yield return new ReflectedBenchmarkDescriptor(type);
            }
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
