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
using System.ComponentModel;
using System.ComponentModel.Design;

namespace WmcSoft.ComponentModel
{
    /// <summary>
    /// Simple implementation of the <see cref="ITypeDescriptorContext"/> interface.
    /// </summary>
    public class TypeDescriptorContext : ITypeDescriptorContext
    {
        #region Fields

        private readonly IServiceProvider serviceProvider;

        #endregion

        #region Lifecycle

        public TypeDescriptorContext(IServiceProvider serviceProvider, PropertyDescriptor descriptor, object instance)
        {
            this.serviceProvider = serviceProvider;
            PropertyDescriptor = descriptor;
            Instance = instance;
        }

        #endregion

        #region ITypeDescriptorContext Members

        public IContainer Container => GetService(typeof(IContainer)) as IContainer;

        public object Instance { get; }

        void ITypeDescriptorContext.OnComponentChanged()
        {
            var service = GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (service != null) {
                service.OnComponentChanged(Instance, PropertyDescriptor, null, null);
            }
        }

        bool ITypeDescriptorContext.OnComponentChanging()
        {
            var service = GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (service != null) {
                try {
                    service.OnComponentChanging(Instance, PropertyDescriptor);
                } catch (CheckoutException exception) {
                    if (CheckoutException.Canceled != exception) {
                        throw;
                    }
                    return false;
                }
            }
            return true;
        }

        public PropertyDescriptor PropertyDescriptor { get; }

        #endregion

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            return serviceProvider?.GetService(serviceType);
        }

        #endregion
    }
}
