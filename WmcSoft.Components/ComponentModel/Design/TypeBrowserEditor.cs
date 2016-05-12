using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;

namespace WmcSoft.ComponentModel.Design
{
    public class TypeBrowserEditor : UITypeEditor
    {
        private class TypeDescriptorContextProxy : ITypeDescriptorContext 
{ 
    ITypeProvider typeProvider; 
    ITypeDescriptorContext realContext; 
 
    public TypeDescriptorContextProxy(ITypeDescriptorContext realContext) 
    { 
        this.realContext = realContext; 
    } 
 
    internal void SetContext(ITypeDescriptorContext realContext) 
    { 
        this.realContext = realContext; 
    } 
 
    #region ITypeDescriptorContext Members 
    #endregion 
 
    #region IServiceProvider Members 
 
    public object GetService(Type serviceType) 
    { 
        if (serviceType == typeof(ITypeProvider)) 
        { 
            if (typeProvider == null) typeProvider = new CustomTypeProvider(this); 
 
            return typeProvider; 
        } 
 
        return realContext.GetService(serviceType); 
    } 
 
    #endregion 
}
        TypeBrowserEditor editor = new TypeBrowserEditor();
        ContextProxy flyweight;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
            // Use flyweight pattern to improve performance. It's guaranteed that no more than one instance of  
            // this editor can ever be used at the same time. (it's modal) 
            if (flyweight == null) {
                flyweight = new ContextProxy(context);
            } else {
                flyweight.SetContext(context);
            }

            return editor.EditValue(flyweight, flyweight, value);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext typeDescriptorContext) {
            return editor.GetEditStyle(typeDescriptorContext);
        }
    }
}
