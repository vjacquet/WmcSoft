using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using System.ComponentModel;
using System;

namespace WmcSoft.Windows.Forms.Design
{
    internal class UITextEditorDesigner : ControlDesigner
    {
        // Methods
        public UITextEditorDesigner() {
            //System.Diagnostics.Debugger.Break();
            base.AutoResizeHandles = true;
        }

        public override void InitializeNewComponent(IDictionary defaultValues) {
            base.InitializeNewComponent(defaultValues);
            var descriptor = TypeDescriptor.GetProperties(base.Component)["Text"];
            if (((descriptor != null) && (descriptor.PropertyType == typeof(string))) && (!descriptor.IsReadOnly && descriptor.IsBrowsable)) {
                descriptor.SetValue(base.Component, "");
            }
        }

        protected override void PreFilterProperties(IDictionary properties) {
            base.PreFilterProperties(properties);
            string[] strArray = new string[] { "Text" };
            Attribute[] attributes = new Attribute[0];
            for (int i = 0; i < strArray.Length; i++) {
                PropertyDescriptor oldPropertyDescriptor = (PropertyDescriptor)properties[strArray[i]];
                if (oldPropertyDescriptor != null) {
                    properties[strArray[i]] = TypeDescriptor.CreateProperty(typeof(UITextEditorDesigner), oldPropertyDescriptor, attributes);
                }
            }
        }

        private void ResetText() {
            this.Control.Text = "";
        }

        private bool ShouldSerializeText() {
            return TypeDescriptor.GetProperties(typeof(UITextEditor))["Text"].ShouldSerializeValue(base.Component);
        }

        // Properties
        public override SelectionRules SelectionRules {
            get {
                SelectionRules selectionRules = base.SelectionRules;
                object component = base.Component;
                selectionRules |= SelectionRules.AllSizeable;
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(component)["AutoSize"];
                if (descriptor != null) {
                    object autoSize = descriptor.GetValue(component);
                    if ((autoSize is bool) && ((bool)autoSize)) {
                        selectionRules &= ~(SelectionRules.BottomSizeable | SelectionRules.TopSizeable);
                    }
                }
                return selectionRules;
            }
        }

        public override IList SnapLines {
            get {
                ArrayList snapLines = base.SnapLines as ArrayList;
                int textBaseline = this.Control.GetTextBaseline(ContentAlignment.TopLeft);
                BorderStyle style = BorderStyle.Fixed3D;
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(base.Component)["BorderStyle"];
                if (descriptor != null) {
                    style = (BorderStyle)descriptor.GetValue(base.Component);
                }
                if (style == BorderStyle.None) {
                } else if (style == BorderStyle.FixedSingle) {
                    textBaseline += 2;
                } else if (style == BorderStyle.Fixed3D) {
                    textBaseline += 3;
                } else {
                }
                snapLines.Add(new SnapLine(SnapLineType.Baseline, textBaseline, SnapLinePriority.Medium));
                return snapLines;
            }
        }

        private string Text {
            get {
                return this.Control.Text;
            }
            set {
                this.Control.Text = value;
                //((UITextEditor)this.Control).Select(0, 0);
            }
        }


    }
}
