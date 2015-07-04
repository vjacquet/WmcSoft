using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    public interface IAuditable
    {
        string Author { get; set; }
        DateTime Created { get; set; }
        string Editor { get; set; }
        DateTime Modified { get; set; }
    }
}
