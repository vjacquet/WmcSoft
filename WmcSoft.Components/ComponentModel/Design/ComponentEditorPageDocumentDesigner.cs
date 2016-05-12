using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Collections;

namespace WmcSoft.ComponentModel.Design
{
    public class ComponentEditorPageDocumentDesigner : DocumentDesigner
    {
        // Methods
        public ComponentEditorPageDocumentDesigner() {
            base.AutoResizeHandles = true;
        }

        //override bool CanDropComponents(DragEventArgs de) {
        //    bool isRightToLeft = base.CanDropComponents(de);
        //    if (isRightToLeft) {
        //        object[] draggingObjects = base.GetOleDragHandler().GetDraggingObjects(de);
        //        if (draggingObjects == null) {
        //            return isRightToLeft;
        //        }
        //        IDesignerHost service = (IDesignerHost)this.GetService(typeof(IDesignerHost));
        //        for (int i = 0; i < draggingObjects.Length; i++) {
        //            if (((service != null) && (draggingObjects[i] != null)) && ((draggingObjects[i] is IComponent) && (draggingObjects[i] is MainMenu))) {
        //                return false;
        //            }
        //        }
        //    }
        //    return isRightToLeft;
        //}

        protected override void PreFilterProperties(IDictionary properties) {
            base.PreFilterProperties(properties);
            string[] strArray = new string[] { "Size" };
            Attribute[] attributes = new Attribute[0];
            for (int i = 0; i < strArray.Length; i++) {
                PropertyDescriptor oldPropertyDescriptor = (PropertyDescriptor)properties[strArray[i]];
                if (oldPropertyDescriptor != null) {
                    properties[strArray[i]] = TypeDescriptor.CreateProperty(typeof(ComponentEditorPageDocumentDesigner), oldPropertyDescriptor, attributes);
                }
            }
        }

        // Properties
        private System.Drawing.Size Size {
            get {
                return this.Control.ClientSize;
            }
            set {
                this.Control.ClientSize = value;
            }
        }
    }


}
