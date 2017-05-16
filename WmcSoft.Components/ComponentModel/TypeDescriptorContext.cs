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

        readonly IServiceProvider _serviceProvider;
        readonly object _instance;
        readonly PropertyDescriptor _descriptor;

        #endregion

        #region Lifecycle

        public TypeDescriptorContext(IServiceProvider serviceProvider, PropertyDescriptor descriptor, object instance)
        {
            _serviceProvider = serviceProvider;
            _descriptor = descriptor;
            _instance = instance;
        }

        #endregion

        #region ITypeDescriptorContext Members

        public IContainer Container {
            get {
                return GetService(typeof(IContainer)) as IContainer;
            }
        }

        public object Instance {
            get { return _instance; }
        }

        void ITypeDescriptorContext.OnComponentChanged()
        {
            IComponentChangeService service = GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (service != null) {
                service.OnComponentChanged(_instance, _descriptor, null, null);
            }
        }

        bool ITypeDescriptorContext.OnComponentChanging()
        {
            IComponentChangeService service = GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (service != null) {
                try {
                    service.OnComponentChanging(_instance, _descriptor);
                } catch (CheckoutException exception) {
                    if (CheckoutException.Canceled != exception) {
                        throw;
                    }
                    return false;
                }
            }
            return true;
        }

        public PropertyDescriptor PropertyDescriptor {
            get { return _descriptor; }
        }

        #endregion

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            if (_serviceProvider == null)
                return null;
            return _serviceProvider.GetService(serviceType);
        }

        #endregion
    }

}
