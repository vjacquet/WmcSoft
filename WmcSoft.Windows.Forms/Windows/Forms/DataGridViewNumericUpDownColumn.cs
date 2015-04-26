// DataGridViewNumericUpDownXXX have been taken from
// http://msdn.microsoft.com/en-us/library/aa730881(VS.80).aspx
// and then adapted.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;
using System.Drawing;

namespace WmcSoft.Windows.Forms
{
    /// <summary>
    /// Custom column type dedicated to the DataGridViewNumericUpDownCell cell type.
    /// </summary>
    [ToolboxBitmap(typeof(DataGridViewNumericUpDownColumn), "DataGridViewNumericUpDownColumn.bmp")]
    public class DataGridViewNumericUpDownColumn : DataGridViewColumn<DataGridViewNumericUpDownCell>
    {
        /// <summary>
        /// Replicates the DecimalPlaces property of the DataGridViewNumericUpDownCell cell type.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(DataGridViewNumericUpDownCell.DefaultDecimalPlaces)]
        [Description("Indicates the number of decimal places to display.")]
        public int DecimalPlaces {
            get {
                return TypedCellTemplate.DecimalPlaces;
            }
            set {
                TypedCellTemplate.DecimalPlaces = value;
                // Call the internal SetDecimalPlaces method instead of the property to avoid invalidation 
                // of each cell. The whole column is invalidated later in a single operation for better performance.
                ForEachCell((c, i) => c.SetDecimalPlaces(i, value));
                Invalidate();
            }
        }

        /// <summary>
        /// Replicates the Increment property of the DataGridViewNumericUpDownCell cell type.
        /// </summary>
        [Category("Data")]
        [Description("Indicates the amount to increment or decrement on each button click.")]
        public decimal Increment {
            get {
                return TypedCellTemplate.Increment;
            }
            set {
                TypedCellTemplate.Increment = value;
                ForEachCell((c, i) => c.SetIncrement(i, value));
            }
        }

        /// Indicates whether the Increment property should be persisted.
        private bool ShouldSerializeIncrement() {
            return !this.Increment.Equals(DataGridViewNumericUpDownCell.DefaultIncrement);
        }
        private void ResetIncrement() {
            Increment = DataGridViewNumericUpDownCell.DefaultIncrement;
        }

        /// <summary>
        /// Replicates the Maximum property of the DataGridViewNumericUpDownCell cell type.
        /// </summary>
        [Category("Data")]
        [Description("Indicates the maximum value for the numeric up-down cells.")]
        [RefreshProperties(RefreshProperties.All)]
        public decimal Maximum {
            get {
                return TypedCellTemplate.Maximum;
            }
            set {
                TypedCellTemplate.Maximum = value;
                ForEachCell((c, i) => c.SetMaximum(i, value));
                Invalidate();
            }
        }

        /// Indicates whether the Maximum property should be persisted.
        private bool ShouldSerializeMaximum() {
            return !Maximum.Equals(DataGridViewNumericUpDownCell.DefaultMaximum);
        }
        private void ResetMaximum() {
            Maximum = DataGridViewNumericUpDownCell.DefaultMaximum;
        }

        /// <summary>
        /// Replicates the Minimum property of the DataGridViewNumericUpDownCell cell type.
        /// </summary>
        [Category("Data")]
        [Description("Indicates the minimum value for the numeric up-down cells.")]
        [RefreshProperties(RefreshProperties.All)]
        public decimal Minimum {
            get {
                return TypedCellTemplate.Minimum;
            }
            set {
                TypedCellTemplate.Minimum = value;
                ForEachCell((c, i) => c.SetMinimum(i, value));
                Invalidate();
            }
        }

        private bool ShouldSerializeMinimum() {
            return !Minimum.Equals(DataGridViewNumericUpDownCell.DefaultMinimum);
        }
        private void ResetMinimum() {
            Minimum = DataGridViewNumericUpDownCell.DefaultMinimum;
        }

        /// <summary>
        /// Replicates the ThousandsSeparator property of the DataGridViewNumericUpDownCell cell type.
        /// </summary>
        [Category("Data")]
        [DefaultValue(DataGridViewNumericUpDownCell.DefaultThousandsSeparator)]
        [Description("Indicates whether the thousands separator will be inserted between every three decimal digits.")]
        public bool ThousandsSeparator {
            get {
                return TypedCellTemplate.ThousandsSeparator;
            }
            set {
                TypedCellTemplate.ThousandsSeparator = value;
                ForEachCell((c, i) => c.SetThousandsSeparator(i, value));
                Invalidate();
            }
        }

        private void Invalidate() {
            DataGridView.InvalidateColumn(Index);
            // TODO: This column and/or grid rows may need to be autosized depending on their
            //       autosize settings. Call the autosizing methods to autosize the column, rows, 
            //       column headers / row headers as needed.
        }

        /// <summary>
        /// Returns a standard compact string representation of the column.
        /// </summary>
        public override string ToString() {
            return "DataGridViewNumericUpDownColumn { Name=" + Name + ", Index=" + Index + " }";
        }
    }
}
