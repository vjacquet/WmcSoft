using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.ComponentModel.Design
{
    public static class DesignerHostExtensions
    {
        public static DesignerTransaction CreateTransaction(this IDesignerHost host, string format, params object[] args) {
            return host.CreateTransaction(String.Format(format, args));
        }
    }
}
