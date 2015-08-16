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
using System.Linq;
using System.Reflection;

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
