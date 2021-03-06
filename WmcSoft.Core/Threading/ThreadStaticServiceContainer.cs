﻿#region Licence

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
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;

namespace WmcSoft.Threading
{
    /// <summary>
    /// Provides a container for services stored in a thread static dictionary, so each calling thread access only its own version.
    /// </summary>
    public sealed class ThreadStaticServiceContainer : IServiceContainer, IDisposable
    {
        #region Private fields

        private readonly ServiceContainer _serviceContainer;
        private readonly ThreadLocal<ServiceContainer> _local;
        private Dictionary<Type, ServiceCreatorCallback> _callbacks;

        #endregion

        #region Lifecycle

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadStaticServiceContainer"/> class.
        /// </summary>
        public ThreadStaticServiceContainer()
        {
            _serviceContainer = new ServiceContainer();
            _local = new ThreadLocal<ServiceContainer>(CreateLocalServiceContainer, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadStaticServiceContainer"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public ThreadStaticServiceContainer(IServiceProvider serviceProvider)
        {
            _serviceContainer = new ServiceContainer(serviceProvider);
            _local = new ThreadLocal<ServiceContainer>(CreateLocalServiceContainer, true);
        }

        #endregion

        #region Properties

        ServiceContainer CreateLocalServiceContainer()
        {
            var serviceContainer = new ServiceContainer(_serviceContainer);
            if (_callbacks != null) {
                foreach (var callback in _callbacks) {
                    serviceContainer.AddService(callback.Key, callback.Value, false);
                }
            }
            return serviceContainer;
        }

        /// <summary>
        /// Gets the thread local static container.
        /// </summary>
        private IServiceContainer ThreadLocalStaticContainer {
            get { return _local.Value; }
        }

        #endregion

        #region IServiceContainer Membres

        /// <summary>
        /// Adds the specified service to the service container, and optionally promotes the service to parent service containers.
        /// </summary>
        /// <param name="serviceType">The type of service to add.</param>
        /// <param name="callback">A callback object that is used to create the service. This allows a service to be declared as available, but delays the creation of the object until the service is requested.</param>
        /// <param name="promote"><c>true</c> to promote this request to any parent service containers; otherwise, <c>false</c>.</param>
        public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            if (!promote) {
                lock (_serviceContainer) {
                    if (_callbacks == null) {
                        _callbacks = new Dictionary<Type, ServiceCreatorCallback>();
                    }
                    _callbacks.Add(serviceType, callback);
                }
            } else {
                _serviceContainer.AddService(serviceType, callback, true);
            }
        }
        /// <summary>
        /// Adds the specified service to the service container.
        /// </summary>
        /// <param name="serviceType">The type of service to add.</param>
        /// <param name="callback">A callback object that is used to create the service. This allows a service to be declared as available, but delays the creation of the object until the service is requested.</param>
        public void AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            AddService(serviceType, callback, false);
        }
        /// <summary>
        /// Adds the specified service to the service container, and optionally promotes the service to any parent service containers.
        /// </summary>
        /// <param name="serviceType">The type of service to add.</param>
        /// <param name="serviceInstance">An instance of the service type to add. This object must implement or inherit from the type indicated by the <paramref name="serviceType"/> parameter.</param>
        /// <param name="promote"><c>true</c> to promote this request to any parent service containers; otherwise, <c>false</c>.</param>
        public void AddService(Type serviceType, object serviceInstance, bool promote)
        {
            _serviceContainer.AddService(serviceType, serviceInstance, promote);
        }
        /// <summary>
        /// Adds the specified service to the service container.
        /// </summary>
        /// <param name="serviceType">The type of service to add.</param>
        /// <param name="serviceInstance">An instance of the service type to add. This object must implement or inherit from the type indicated by the <paramref name="serviceType"/> parameter.</param>
        public void AddService(Type serviceType, object serviceInstance)
        {
            AddService(serviceType, serviceInstance, false);
        }

        /// <summary>
        /// Removes the specified service type from the service container, and optionally promotes the service to parent service containers.
        /// </summary>
        /// <param name="serviceType">The type of service to remove.</param>
        /// <param name="promote"><c>true</c> to promote this request to any parent service containers; otherwise, <c>false</c>.</param>
        public void RemoveService(Type serviceType, bool promote)
        {
            if (!promote && _callbacks == null) {
                lock (_serviceContainer) {
                    _callbacks.Remove(serviceType);
                }
            }
            _serviceContainer.RemoveService(serviceType, promote);
        }
        /// <summary>
        /// Removes the specified service type from the service container.
        /// </summary>
        /// <param name="serviceType">The type of service to remove.</param>
        public void RemoveService(Type serviceType)
        {
            RemoveService(serviceType, false);
        }

        #endregion

        #region IServiceProvider Membres

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type <paramref name="serviceType"/>.
        /// -or- 
        /// <c>null</c> if there is no service object of type <paramref name="serviceType"/>.
        /// </returns>
        public object GetService(Type serviceType)
        {
            return ThreadLocalStaticContainer.GetService(serviceType);
        }

        #endregion

        public void Dispose()
        {
            foreach (var container in _local.Values)
                container.Dispose();
            _local.Dispose();
            _serviceContainer.Dispose();
        }
    }
}