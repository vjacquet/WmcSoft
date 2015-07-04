using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    interface IUniqueIdentifier<T> where T : IEquatable<T>
    {
        T Id { get; }
    }
}
