using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Permissions;
using System.Drawing.Design;
using System.ComponentModel;

namespace WmcSoft.ComponentModel.Design
{
    [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
    internal class ServiceableTypesEditor : UITypeEditor
    {
        #region Lifecycle

        public ServiceableTypesEditor() {
        }

        #endregion

        public override bool IsDropDownResizable {
            get { return true; }
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context) {
            return false;
        }

        public override void PaintValue(PaintValueEventArgs e) {
            base.PaintValue(e);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.None;
        }

    }

}
