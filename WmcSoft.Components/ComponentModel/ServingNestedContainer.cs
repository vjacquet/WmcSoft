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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace WmcSoft.ComponentModel
{
    public class NestedContainerWithServiceContainer : NestedContainer, IServiceProvider
    {
        #region OwnerServiceProvider Class

        class OwnerServiceProvider : IServiceProvider
        {
            #region Private fields

            readonly IComponent _component;

            #endregion

            #region Lifecycle

            public OwnerServiceProvider(IComponent component) {
                _component = component;
            }

            #endregion

            #region IServiceProvider Membres

            public object GetService(Type serviceType) {
                if (serviceType.IsAssignableFrom(_component.GetType())) {
                    return _component;
                }
                if (_component.Site != null) {
                    return _component.Site.GetService(serviceType);
                }
                return null;
            }

            #endregion
        }

        #endregion

        #region Private Fields

        readonly IServiceContainer _serviceContainer;
        readonly List<IServiceProvider> _serviceProviders;

        #endregion

        #region Lifecycle

        public NestedContainerWithServiceContainer(IComponent owner)
            : base(owner) {
            _serviceContainer = new ServiceContainer(new OwnerServiceProvider(owner));
            _serviceProviders = new List<IServiceProvider>();
        }

        #endregion

        #region ServiceContainer Property

        public IServiceContainer ServiceContainer {
            get { return _serviceContainer; }
        }

        #endregion

        #region Overridables

        public override void Add(IComponent component) {
            base.Add(component);
            RegisterServiceProvider(component);
        }

        public override void Add(IComponent component, string name) {
            base.Add(component, name);
            RegisterServiceProvider(component);
        }

        private void RegisterServiceProvider(IComponent component) {
            var serviceProvider = component as IServiceProvider;
            if (serviceProvider != null) {
                _serviceProviders.Add(serviceProvider);
            }
        }

        public override void Remove(IComponent component) {
            base.Remove(component);

            var serviceProvider = component as IServiceProvider;
            if (serviceProvider != null) {
                _serviceProviders.Remove(serviceProvider);
            }
        }

        #endregion

        #region IServiceProvider Members

        protected override object GetService(Type service) {
            var serviceInstance = base.GetService(service);
            if (serviceInstance != null)
                return serviceInstance;

            foreach (var serviceProvider in _serviceProviders) {
                serviceInstance = serviceProvider.GetService(service);
                if (serviceInstance != null)
                    return serviceInstance;
            }

            serviceInstance = _serviceContainer.GetService(service);
            return serviceInstance;
        }

        object IServiceProvider.GetService(Type serviceType) {
            return GetService(serviceType);
        }

        #endregion
    }
}
