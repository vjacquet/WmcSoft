using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Security
{
    [DataContract]
    public class User : Principal
    {
        public User(string name)
            : base(name) {
        }
    }
}
