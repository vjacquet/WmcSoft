using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Security
{
    public class Groups
    {
        public static Principal Everyone = new Everyone();
    }

    sealed class Everyone : Principal
    {
        public Everyone()
            : base("Everyone") {
        }

        public override bool Match(Principal other) {
            return true;
        }
    }
}
