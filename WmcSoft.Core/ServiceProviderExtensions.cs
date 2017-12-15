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
using System.ComponentModel.Design;
using WmcSoft.Properties;

namespace WmcSoft
{
    /// <summary>
    /// Defines the extension methods to the <see cref="IServiceProvider"/> interface.
    /// This is a static class.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        #region Service provider extensions

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

        #endregion

        #region Service container extensions

        /// <summary>
        /// Adds the specified service to the service container.
        /// </summary>
        /// <typeparam name="T">The type of service.</typeparam>
        /// <param name="self">The service container.</param>
        /// <param name="instance">An instance of the service type to add. </param>
        public static void AddService<T>(this IServiceContainer self, T instance) {
            self.AddService(typeof(T), instance);
        }

        /// <summary>
        /// Adds the specified service to the service container.
        /// </summary>
        /// <typeparam name="T">The type of service.</typeparam>
        /// <param name="self">The service container.</param>
        /// <param name="instance">An instance of the service type to add. </param>
        /// <param name="promote"></param>
        /// <param name="promote"><c><see langword="true"/></c> to promote this request to any parent service containers; otherwise, <c><see langword="false"/></c>.</param>
        public static void AddService<T>(this IServiceContainer self, T instance, bool promote) {
            self.AddService(typeof(T), instance, promote);
        }

        /// <summary>
        /// Adds the specified service to the service container.
        /// </summary>
        /// <typeparam name="T">The type of service.</typeparam>
        /// <param name="self">The service container.</param>
        /// <param name="factory">A callback that is used to create the service.
        /// This allows a service to be declared as available, but delays the creation of the object until the service is requested.</param>
        public static void AddService<T>(this IServiceContainer self, Func<T> factory) {
            object callback(IServiceContainer c, Type t) => factory();
            self.AddService(typeof(T), callback);
        }
        /// <summary>
        /// Adds the specified service to the service container, and optionally the service to parent service containers.
        /// </summary>
        /// <typeparam name="T">The type of service.</typeparam>
        /// <param name="self">The service container.</param>
        /// <param name="factory">A callback that is used to create the service.
        /// This allows a service to be declared as available, but delays the creation of the object until the service is requested.</param>
        /// <param name="promote"><c><see langword="true"/></c> to promote this request to any parent service containers; otherwise, <c><see langword="false"/></c>.</param>
        public static void AddService<T>(this IServiceContainer self, Func<T> factory, bool promote) {
            object callback(IServiceContainer c, Type t) => factory();
            self.AddService(typeof(T), callback, promote);
        }

        /// <summary>
        /// Adds the specified service to the service container.
        /// </summary>
        /// <typeparam name="T">The type of service.</typeparam>
        /// <param name="self">The service container.</param>
        /// <param name="factory">A callback that is used to create the service.
        /// This allows a service to be declared as available, but delays the creation of the object until the service is requested.</param>
        public static void AddService<T>(this IServiceContainer self, Func<IServiceProvider, T> factory) {
            object callback(IServiceContainer c, Type t) => factory(c);
            self.AddService(typeof(T), callback);
        }

        /// <summary>
        /// Adds the specified service to the service container, and optionally the service to parent service containers.
        /// </summary>
        /// <typeparam name="T">The type of service.</typeparam>
        /// <param name="self">The service container.</param>
        /// <param name="factory">A callback that is used to create the service.
        /// This allows a service to be declared as available, but delays the creation of the object until the service is requested.</param>
        /// <param name="promote"><c><see langword="true"/></c> to promote this request to any parent service containers; otherwise, <c><see langword="false"/></c>.</param>
        public static void AddService<T>(this IServiceContainer self, Func<IServiceProvider, T> factory, bool promote) {
            object callback(IServiceContainer c, Type t) => factory(c);
            self.AddService(typeof(T), callback, promote);
        }

        /// <summary>
        /// Removes the specified service type from the service container.
        /// </summary>
        /// <typeparam name="T">The type of service.</typeparam>
        /// <param name="self">The service container.</param>
        public static void RemoveService<T>(this IServiceContainer self) {
            self.RemoveService(typeof(T));
        }

        /// <summary>
        /// Removes the specified service type from the service container, and optionally promotes the request to parent service containers.
        /// </summary>
        /// <typeparam name="T">The type of service to remove.</typeparam>
        /// <param name="self">The service container.</param>
        /// <param name="promote"><c><see langword="true"/></c> to promote this request to any parent service containers; otherwise, <c><see langword="false"/></c>.</param>
        public static void RemoveService<T>(this IServiceContainer self, bool promote) {
            self.RemoveService(typeof(T), promote);
        }

        #endregion
    }
}
