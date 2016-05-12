using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace WmcSoft.ComponentModel.Design
{
    [Designer("WmcSoft.ComponentModel.Design.ComponentEditorPageDocumentDesigner, WmcSoft.ControlLibrary", typeof(IRootDesigner))]
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [DesignerCategory("UserControl")]
    public class ComponentEditorPage : System.Windows.Forms.Design.ComponentEditorPage
    {
        protected override void LoadComponent() {
        }

        protected override void SaveComponent() {
        }

    }
}
