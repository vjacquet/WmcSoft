using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Benchmark
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BenchmarkAttribute : Attribute
    {
        public BenchmarkAttribute() {
            Iterations = 10;
        }

        public int Iterations { get; set; }
    }
}
