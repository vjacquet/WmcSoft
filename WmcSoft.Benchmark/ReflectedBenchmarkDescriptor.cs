using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Benchmark
{
    public class ReflectedBenchmarkDescriptor : IBenchmarkDescriptor
    {
        Type _type;
        MethodInfo _initMethod;
        MethodInfo _resetMethod;
        MethodInfo _checkMethod;
        int _iterations;

        public ReflectedBenchmarkDescriptor(Type type) {
            var benchmark = type.GetCustomAttributes(typeof(BenchmarkAttribute), false).Cast<BenchmarkAttribute>().FirstOrDefault();
            _iterations = (benchmark != null) ? benchmark.Iterations : 10;
            _type = type;
            _initMethod = type.GetMethod("Init", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string[]) }, null);
            _resetMethod = type.GetMethod("Reset", BindingFlags.Public | BindingFlags.Static, null, Type.EmptyTypes, null);
            _checkMethod = type.GetMethod("Check", BindingFlags.Public | BindingFlags.Static, null, Type.EmptyTypes, null);
        }

        #region IBenchmarkDescriptor Membres

        public string Name {
            get { return _type.Name; }
        }

        public void Init(string[] args) {
            if (_initMethod != null)
                _initMethod.Invoke(null, new object[] { args });
        }

        public void Reset() {
            if (_resetMethod != null)
                _resetMethod.Invoke(null, null);
        }

        public void Check() {
            if (_checkMethod != null)
                _checkMethod.Invoke(null, null);
        }

        public int Iterations {
            get { return _iterations; }
        }

        public IEnumerable<IMeasureDescriptor> EnumerateMeasures() {
            foreach (var method in _type.GetMethods(BindingFlags.Public | BindingFlags.Static)) {
                var parameters = method.GetParameters();
                if (parameters.Length != 0)
                    continue;

                if (method.GetCustomAttributes(typeof(MeasureAttribute), false).Length != 0) {
                    yield return new ReflectedMeasureDescriptor(method);
                }
            }
        }

        #endregion
    }
}
