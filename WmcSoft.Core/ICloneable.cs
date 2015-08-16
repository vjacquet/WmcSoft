using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmcSoft
{
    public interface ICloneable<out T> : ICloneable
    {
        new T Clone();
    }
}
