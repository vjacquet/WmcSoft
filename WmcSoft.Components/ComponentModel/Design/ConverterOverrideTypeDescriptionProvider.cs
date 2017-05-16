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

namespace WmcSoft.ComponentModel.Design
{
    /// <summary>
    /// Overrides the TypeConverter of a specified type descriptor.
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    /// <typeparam name="C">The TypeConverter for the type</typeparam>
    public class ConverterOverrideTypeDescriptionProvider<T, C> : TypeDescriptionProvider where C : TypeConverter, new()
    {
        class ConverterOverrideTypeDescriptor : CustomTypeDescriptorDecorator
        {
            private readonly TypeConverter _converter;

            public ConverterOverrideTypeDescriptor()
                : base(TypeDescriptor.GetProvider(typeof(T)).GetTypeDescriptor(typeof(T)))
            {
                _converter = new C();
            }

            public override TypeConverter GetConverter()
            {
                return _converter;
            }
        }

        ICustomTypeDescriptor _descriptor;

        public ConverterOverrideTypeDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof(T)))
        {
            _descriptor = new ConverterOverrideTypeDescriptor();
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return _descriptor;
        }
    }
}
