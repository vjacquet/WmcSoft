using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.ComponentModel
{
   static class ResourceHelpers
    {
       public static string GetString(Type resourceSource, string name) {
           var mgr = new ResourceManager(resourceSource);
           return mgr.GetString(name, CultureInfo.CurrentUICulture);
       }
    }
}
