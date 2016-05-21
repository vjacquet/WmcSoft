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
    /// Defines the extension methods to the <see cref="IServiceProvider"/> interface.
    /// This is a static class.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Generic version of GetService for wich the type is inferred
        /// </summary>
        /// <typeparam name="T">The type of the service to get.</typeparam>
        /// <param name="serviceProvider">The service provider</param>
        /// <returns>The service</returns>
        public static T GetService<T>(this IServiceProvider serviceProvider) where T : class {
            return (T)serviceProvider.GetService(typeof(T));
        }

        /// <summary>
        /// Get the service of the specified type or throws.
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <returns>The service</returns>
        /// <exception cref="InvalidOperationException">Thrown when the service cannot be provided.</exception>
        public static object RequireService(this IServiceProvider serviceProvider, Type serviceType) {
            object instance = serviceProvider.GetService(serviceType);
            if (instance == null)
                throw new InvalidOperationException(string.Format(Resources.MissingServiceMessage, serviceType.FullName));
            return instance;
        }

        /// <summary>
        /// Get the service of the specified type or throws.
        /// </summary>
        /// <typeparam name="T">The type of the service to get.</typeparam>
        /// <param name="serviceProvider">The service provider</param>
        /// <returns>The service</returns>
        /// <exception cref="InvalidOperationException">Thrown when the service cannot be provided.</exception>
        public static T RequireService<T>(this IServiceProvider self) {
            return (T)RequireService(self, typeof(T));
        }
    }
}