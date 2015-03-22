using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmcSoft.Threading
{

    public static class JobExtensions
    {
        /// <summary>
        /// Generic version of GetService for wich the type is inferred
        /// </summary>
        /// <typeparam name="T">type of the service to get.</typeparam>
        /// <param name="self">The service provider</param>
        /// <returns>The service</returns>
        public static T GetService<T>(this IServiceProvider self) where T : class {
            return (T)self.GetService(typeof(T));
        }

    }
}
