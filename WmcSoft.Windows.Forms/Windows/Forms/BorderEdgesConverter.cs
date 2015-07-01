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
using System.Resources;

namespace WmcSoft.Windows.Forms
{
    /// <summary>
    /// Converter for BorderEdges.
    /// </summary>
    public class BorderEdgesConverter : TypeConverter
    {
        public BorderEdgesConverter() {
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) {
            var rm = new ResourceManager(typeof(BorderEdgesConverter));

            var collection = TypeDescriptor.GetProperties(typeof(BorderEdges), attributes);
            var array = rm.GetStrings("All", "Left", "Top", "Right", "Bottom");
            return collection.Sort(array);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context) {
            return true;
        }
    }
}
