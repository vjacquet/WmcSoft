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
using System.Diagnostics;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WmcSoft.Windows.Forms.Design
{
    /// <summary>
    ///   Drop down editor for the BlendFill type, which shows the various ways to blend as well as let the user
    ///   select the colors to blend.
    /// </summary>
    [PermissionSetAttribute(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
    public class BlendFillEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            if (context != null && context.Instance != null) {
                return UITypeEditorEditStyle.DropDown;
            }
            return base.GetEditStyle(context);
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
            if (context != null && context.Instance != null && provider != null) {
                var editorService = provider.GetService<IWindowsFormsEditorService>();
                if (editorService != null) {
                    var blend = (BlendFill)value;
                    var reverse = GetReverseValue(context);
                    var editor = new BlendFillEditorUI(editorService, blend, reverse);

                    editorService.DropDownControl(editor);

                    value = editor.GetValue();
                }
            }
            return value;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context) {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e) {
            var bf = e.Value as BlendFill;
            if (bf != null) {
                using (LinearGradientBrush lgb = bf.GetLinearGradientBrush(e.Bounds)) {
                    e.Graphics.FillRectangle(lgb, e.Bounds);
                }
            } else {
                Debug.WriteLine("BlendFillEditor.Paint: the value is " + (e.Value == null ? "null" : e.Value.GetType().ToString()));
            }
        }

        private bool GetReverseValue(ITypeDescriptorContext context) {
            if (context != null && context.Instance != null) {
                var ctl = context.Instance as Control;
                if (ctl != null)
                    return ctl.GetRightToLeftValue();

                // We have to look at context.Instance, and see if it has a 
                // pd named "RightToLeft".  If so, then use that value,
                // otherwise, just assume no RTL.
                var pd = TypeDescriptor.GetProperties(context.Instance)["RightToLeft"];
                if (pd != null) {
                    var rtl = (RightToLeft)pd.GetValue(context.Instance);
                    return rtl != RightToLeft.No;
                }
            }
            return false;
        }
    }

}
