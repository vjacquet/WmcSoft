using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    public static class DataGridViewExtensions
    {
        public static void ForEach<TCell>(this DataGridView dataGridView, int index, Action<TCell> action) where TCell : DataGridViewCell {
            if (dataGridView == null)
                return;
            var dataGridViewRows = dataGridView.Rows;
            int rowCount = dataGridViewRows.Count;
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++) {
                // Be careful not to unshare rows unnecessarily. 
                // This could have severe performance repercussions.
                var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                var dataGridViewCell = dataGridViewRow.Cells[index] as TCell;
                if (dataGridViewCell != null) {
                    action(dataGridViewCell);
                }
            }
        }

        public static void ForEach<TCell>(this DataGridView dataGridView, int index, Action<TCell, int> action) where TCell : DataGridViewCell {
            if (dataGridView == null)
                return;
            var dataGridViewRows = dataGridView.Rows;
            int rowCount = dataGridViewRows.Count;
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++) {
                // Be careful not to unshare rows unnecessarily. 
                // This could have severe performance repercussions.
                var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                var dataGridViewCell = dataGridViewRow.Cells[index] as TCell;
                if (dataGridViewCell != null) {
                    action(dataGridViewCell, rowIndex);
                }
            }
        }

        public static DataGridViewElementStates GetInheritedState(this DataGridView dataGridView, int columnIndex, int rowIndex) {
            return dataGridView.Rows.SharedRow(rowIndex).Cells[columnIndex].GetInheritedState(rowIndex);
        }

        public static void ForEach<TCell>(this DataGridViewColumn column, Action<TCell> action) where TCell : DataGridViewCell {
            if (column == null)
                return;
            column.DataGridView.ForEach<TCell>(column.Index, action);
        }
        public static void ForEach<TCell>(this DataGridViewColumn column, Action<TCell, int> action) where TCell : DataGridViewCell {
            if (column == null)
                return;
            column.DataGridView.ForEach<TCell>(column.Index, action);
        }

        public static void ReplaceAll(this DataGridViewComboBoxCell.ObjectCollection collection, object[] items) {
            collection.Clear();
            collection.AddRange(items);
        }
    }
}
