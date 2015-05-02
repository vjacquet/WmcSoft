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

using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms.Design
{
    public class ChildControlConverter : ReferenceConverter
    {
        public ChildControlConverter()
            : base(typeof(Control)) {

        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            var arrayList = new ArrayList();
            var control = context.Instance as Control;
            if (control == null) {
                var action = context.Instance as DesignerActionList;
                if (action != null)
                    control = action.Component as Control;
            }
            if (control != null) {
                arrayList.AddRange(control.Controls.OfType<IComponent>().ToArray());
            }
            return new TypeConverter.StandardValuesCollection(arrayList);
        }
    }
}
