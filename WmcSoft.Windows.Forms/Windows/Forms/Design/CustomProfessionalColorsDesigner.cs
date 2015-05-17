using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Collections;

namespace WmcSoft.Windows.Forms.Design
{
    public class CustomProfessionalColorsDesigner : ComponentDesigner
    {
        protected override void PostFilterProperties(IDictionary properties) {
            Queue candidates = new Queue();
            foreach (DictionaryEntry entry in properties) {
                if (typeof(System.Drawing.Color).IsAssignableFrom(((PropertyDescriptor)entry.Value).PropertyType))
                    candidates.Enqueue(entry.Key);
            }
            foreach (object key in candidates) {
                PropertyDescriptor pd = (PropertyDescriptor)properties[key];
                Attribute[] attributes = new Attribute[] { 
                    new BrowsableAttribute(false), 
                    new EditorBrowsableAttribute(EditorBrowsableState.Never),
                    new ProfessionalColorsTab.ToolStripCategoryAttribute(pd.Name)
                };
                properties[key] = TypeDescriptor.CreateProperty(pd.ComponentType, pd, attributes);
            }
            base.PostFilterProperties(properties);
        }
    }
}
