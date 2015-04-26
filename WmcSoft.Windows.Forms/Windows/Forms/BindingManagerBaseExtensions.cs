using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    public static class BindingManagerBaseExtensions
    {
        public static PropertyDescriptor GetItemProperty(this BindingManagerBase cm, string name, bool ignoreCase) {
            if (cm == null)
                return null;
            var props = cm.GetItemProperties();
            return props.Find(name, ignoreCase);
        }
    }
}
