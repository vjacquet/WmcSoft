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
using System.Linq;

namespace WmcSoft.ComponentModel
{
    public class NestedContainerWithServiceContainer : NestedContainer, IServiceProvider
    {
        #region OwnerServiceProvider Class

        class OwnerServiceProvider : IServiceProvider
        {
            readonly IComponent component;

            public OwnerServiceProvider(IComponent component)
            {
                this.component = component;
            }

            public object GetService(Type serviceType)
            {
                if (serviceType.IsAssignableFrom(component.GetType()))
                    return component;
                if (component.Site != null)
                    return component.Site.GetService(serviceType);
                return null;
            }
        }

        #endregion

        #region Private Fields

        private readonly List<IServiceProvider> serviceProviders;

        #endregion

        #region Lifecycle

        public NestedContainerWithServiceContainer(IComponent owner)
            : base(owner)
        {
            ServiceContainer = new ServiceContainer(new OwnerServiceProvider(owner));
            serviceProviders = new List<IServiceProvider>();
        }

        #endregion

        #region ServiceContainer Property

        public IServiceContainer ServiceContainer { get; }

        #endregion

        #region Overridables

        public override void Add(IComponent component)
        {
            base.Add(component);
            RegisterServiceProvider(component);
        }

        public override void Add(IComponent component, string name)
        {
            base.Add(component, name);
            RegisterServiceProvider(component);
        }

        private void RegisterServiceProvider(IComponent component)
        {
            if (component is IServiceProvider serviceProvider) {
                serviceProviders.Add(serviceProvider);
            }
        }

        public override void Remove(IComponent component)
        {
            base.Remove(component);

            if (component is IServiceProvider serviceProvider) {
                serviceProviders.Remove(serviceProvider);
            }
        }

        #endregion

        #region IServiceProvider Members

        protected override object GetService(Type service)
        {
            return base.GetService(service)
                ?? serviceProviders.Select(sp => sp.GetService(service)).FirstOrDefault(s => s != null)
                ?? ServiceContainer.GetService(service);
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return GetService(serviceType);
        }

        #endregion
    }
}
