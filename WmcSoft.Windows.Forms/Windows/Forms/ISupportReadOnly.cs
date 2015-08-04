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
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    public interface ISupportReadOnly
    {
        bool ReadOnly { get; set; }
    }

    public static class SupportReadOnlyExtensions
    {
        #region Adapters

        sealed class NotEnableAdapter : ISupportReadOnly
        {
            #region Fields

            readonly Control _control;

            #endregion

            #region Lifecycle

            public NotEnableAdapter(Control control) {
                _control = control;
            }

            #endregion

            #region ISupportReadOnly Members

            public bool ReadOnly {
                get { return !_control.Enabled; }
                set { _control.Enabled = !value; }
            }

            #endregion
        }

        sealed class TextBoxBaseAdapter : ISupportReadOnly
        {
            #region Fields

            readonly TextBoxBase _control;

            #endregion

            #region Lifecycle

            public TextBoxBaseAdapter(TextBoxBase control) {
                _control = control;
            }

            #endregion

            #region ISupportReadOnly Members

            public bool ReadOnly {
                get { return !_control.ReadOnly; }
                set { _control.ReadOnly = !value; }
            }

            #endregion
        }

        sealed class DataGridViewAdapter : ISupportReadOnly
        {
            #region Fields

            readonly DataGridView _control;

            #endregion

            #region Lifecycle

            public DataGridViewAdapter(DataGridView control) {
                _control = control;
            }

            #endregion

            #region ISupportReadOnly Members

            public bool ReadOnly {
                get { return !_control.ReadOnly; }
                set { _control.ReadOnly = !value; }
            }

            #endregion
        }

        sealed class PropertyDescriptorAdapter : ISupportReadOnly
        {
            #region Fields

            readonly object _component;
            readonly PropertyDescriptor _property;

            #endregion

            #region Lifecycle

            public PropertyDescriptorAdapter(object component, PropertyDescriptor property) {
                _component = component;
                _property = property;
            }

            #endregion

            #region ISupportReadOnly Members

            public bool ReadOnly {
                get { return Convert.ToBoolean(_property.GetValue(_component)); }
                set {
                    _property.SetValue(_component, value);
                }
            }

            #endregion
        }

        #endregion

        public static ISupportReadOnly AsReadOnly(this Control control) {
            var readOnly = control as ISupportReadOnly;
            if (readOnly != null)
                return readOnly;

            var tb = control as TextBoxBase;
            if (tb != null)
                return AsReadOnly(tb);

            var dg = control as DataGridView;
            if (dg != null)
                return AsReadOnly(tb);

            var pd = TypeDescriptor.GetProperties(control).Find("ReadOnly", true);
            if (pd != null && pd.PropertyType == typeof(bool))
                return new PropertyDescriptorAdapter(control, pd);

            return new NotEnableAdapter(control);
        }

        public static ISupportReadOnly AsReadOnly(this TextBoxBase control) {
            return new TextBoxBaseAdapter(control);
        }

        public static ISupportReadOnly AsReadOnly(this DataGridView control) {
            return new DataGridViewAdapter(control);
        }
    }
}
