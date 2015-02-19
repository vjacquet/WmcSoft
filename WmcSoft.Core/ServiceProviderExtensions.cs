#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System;
using WmcSoft.Properties;

namespace WmcSoft
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceProviderExtensions
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

        public static object GetRequiredService(this IServiceProvider self, Type serviceType) {
            object instance = self.GetService(serviceType);
            if (instance == null)
                throw new InvalidOperationException(String.Format(Resources.MissingServiceMessage, serviceType.FullName));
            return instance;
        }

        public static T GetRequiredService<T>(this IServiceProvider self) where T : class {
            object instance = self.GetService(typeof(T));
            if (instance == null)
                throw new InvalidOperationException(String.Format(Resources.MissingServiceMessage, typeof(T).FullName));
            return (T)instance;
        }
    }
}
