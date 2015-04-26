// DataGridViewNumericUpDownXXX have been taken from
// http://msdn.microsoft.com/en-us/library/aa730881(VS.80).aspx
// and then adapted.
using System;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using WmcSoft.Collections;

namespace WmcSoft.Windows.Forms
{
    /// <summary>
    /// Custom column type dedicated to the DataGridViewRadioButtonCell cell type.
    /// </summary>
    [ToolboxBitmap(typeof(DataGridViewRadioButtonColumn), "DataGridViewRadioButtonColumn.bmp")]
    public class DataGridViewRadioButtonColumn : DataGridViewColumn<DataGridViewRadioButtonCell>
    {
        /// <summary>
        /// Replicates the DataSource property of the DataGridViewRadioButtonCell cell type.
        /// </summary>
        [AttributeProvider(typeof(IListSource))]
        [Category("Data")]
        [DefaultValue(null)]
        [Description("The data source that populates the radio buttons.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public object DataSource {
            get {
                return TypedCellTemplate.DataSource;
            }
            set {
                TypedCellTemplate.DataSource = value;
                ForEachCell(c => c.DataSource = value);
                Invalidate();
            }
        }

        /// <summary>
        /// Replicates the DisplayMember property of the DataGridViewRadioButtonCell cell type.
        /// </summary>
        [Category("Data")]
        [DefaultValue("")]
        [Description("A string that specifies the property or column from which to retrieve strings for display in the radio buttons.")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverterAttribute("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design")]
        public string DisplayMember {
            get {
                return TypedCellTemplate.DisplayMember;
            }
            set {
                TypedCellTemplate.DisplayMember = value;
                ForEachCell(c => c.DisplayMember = value);
                Invalidate();
            }
        }

        /// <summary>
        /// Replicates the Items property of the DataGridViewRadioButtonCell cell type.
        /// </summary>
        [Category("Data")]
        [Description("The collection of objects used as entries for the radio buttons.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
        public DataGridViewRadioButtonCell.ObjectCollection Items {
            get { return TypedCellTemplate.Items; }
        }

        /// <summary>
        /// Replicates the MaxDisplayedItems property of the DataGridViewRadioButtonCell cell type.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(DataGridViewRadioButtonCell.DefaultMaxDisplayedItems)]
        [Description("The maximum number of radio buttons to display in the cells of the column.")]
        public int MaxDisplayedItems {
            get {
                return TypedCellTemplate.MaxDisplayedItems;
            }
            set {
                if (MaxDisplayedItems != value) {
                    TypedCellTemplate.MaxDisplayedItems = value;
                    ForEachCell(c => c.SetMaxDisplayedItemsInternal(value));
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Replicates the ValueMember property of the DataGridViewRadioButtonCell cell type.
        /// </summary>
        [Category("Data")]
        [DefaultValue("")]
        [Description("A string that specifies the property or column from which to get values that correspond to the radio buttons.")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverterAttribute("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design")]
        public string ValueMember {
            get {
                return TypedCellTemplate.ValueMember;
            }
            set {
                TypedCellTemplate.ValueMember = value;
                ForEachCell(c => c.ValueMember = value);
                Invalidate();
            }
        }

        /// <summary>
        /// Call this public method when the Items collection of this column's CellTemplate was changed.
        /// Updates the items collection of each existing DataGridViewRadioButtonCell in the column.
        /// </summary>
        public void NotifyItemsCollectionChanged() {
            if (DataGridView != null) {
                object[] items = TypedCellTemplate.Items.ToArray();
                ForEachCell(c => c.Items.ReplaceAll(items));
                Invalidate();
            }
        }

        private void Invalidate() {
            var dataGridView = DataGridView;
            if (dataGridView != null) {
                dataGridView.InvalidateColumn(Index);
                // TODO: This column and/or grid rows may need to be autosized depending on their
                //       autosize settings. Call the autosizing methods to autosize the column, rows, 
                //       column headers / row headers as needed.
            }
        }

        /// <summary>
        /// Returns a standard compact string representation of the column.
        /// </summary>
        public override string ToString() {
            return "DataGridViewRadioButtonColumn { Name=" + Name + ", Index=" + Index + " }";
        }
    }
}