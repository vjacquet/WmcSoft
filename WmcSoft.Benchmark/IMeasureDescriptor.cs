using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Benchmark
{
    public interface IMeasureDescriptor
    {
        string Name { get; }
        void Invoke();
    }
}
