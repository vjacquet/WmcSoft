using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Benchmark
{
    public class ReflectedMeasureDescriptor : IMeasureDescriptor
    {
        readonly MethodInfo _method;

        public ReflectedMeasureDescriptor(MethodInfo method) {
            _method = method;
        }

        #region IMeasureDescriptor Membres

        public string Name {
            get { return _method.Name; }
        }

        public void Invoke() {
            _method.Invoke(null, null);
        }

        #endregion
    }
}
