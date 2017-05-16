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

namespace WmcSoft.ComponentModel
{
    /// <summary>
    /// Base class for implementing ICustomTypeDescriptor decorators.
    /// </summary>
    /// <remarks>Unlike CustomTypeDescriptor, the parent descriptor is required 
    /// and the parameterless GetProperties and GetEvents are sealed and call the version 
    /// with the attributes parameter.</remarks>
    public class CustomTypeDescriptorDecorator : ICustomTypeDescriptor
    {
        private readonly ICustomTypeDescriptor _descriptor;

        protected CustomTypeDescriptorDecorator(ICustomTypeDescriptor descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));
            _descriptor = descriptor;
        }

        #region ICustomTypeDescriptor Membres

        public virtual AttributeCollection GetAttributes()
        {
            return _descriptor.GetAttributes();
        }

        public virtual string GetClassName()
        {
            return _descriptor.GetClassName();
        }

        public virtual string GetComponentName()
        {
            return _descriptor.GetComponentName();
        }

        public virtual TypeConverter GetConverter()
        {
            return _descriptor.GetConverter();
        }

        public virtual EventDescriptor GetDefaultEvent()
        {
            return _descriptor.GetDefaultEvent();
        }

        public virtual PropertyDescriptor GetDefaultProperty()
        {
            return _descriptor.GetDefaultProperty();
        }

        public virtual object GetEditor(Type editorBaseType)
        {
            return _descriptor.GetEditor(editorBaseType);
        }

        public virtual EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return _descriptor.GetEvents(attributes);
        }

        public EventDescriptorCollection GetEvents()
        {
            return GetEvents(null);
        }

        public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return _descriptor.GetProperties(attributes);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }

        public virtual object GetPropertyOwner(PropertyDescriptor pd)
        {
            return _descriptor.GetPropertyOwner(pd);
        }

        #endregion
    }
}
