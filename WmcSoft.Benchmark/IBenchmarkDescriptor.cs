using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Benchmark
{
    public interface IBenchmarkDescriptor
    {
        string Name { get; }
        void Init(string[] args);
        void Reset();
        void Check();

        int Iterations { get; }
        IEnumerable<IMeasureDescriptor> EnumerateMeasures();
    }
}
