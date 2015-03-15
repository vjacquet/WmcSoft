using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Benchmark
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MeasureAttribute : Attribute
    {
    }
}
