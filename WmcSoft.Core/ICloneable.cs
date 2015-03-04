using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    public interface ICloneable<out T> : ICloneable
    {
        new T Clone();
    }
}
