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
using System.Collections.Generic;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    public static class DataGridViewExtensions
    {
        #region Any

        public static bool Any<T>(this DataGridViewColumnCollection columns)
            where T : DataGridViewColumn {
            foreach (DataGridViewColumn column in columns) {
                var typed = column as T;
                if (typed != null)
                    return true;
            }
            return false;
        }

        public static bool Any<T>(this DataGridViewColumnCollection columns, Func<T, bool> predicate)
            where T : DataGridViewColumn {
            foreach (DataGridViewColumn column in columns) {
                var typed = column as T;
                if (typed != null && predicate(typed))
                    return true;
            }
            return false;
        }

        #endregion

        #region ForEach

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

        #endregion

        #region GetInheritedState

        public static DataGridViewElementStates GetInheritedState(this DataGridView dataGridView, int columnIndex, int rowIndex) {
            return dataGridView.Rows.SharedRow(rowIndex).Cells[columnIndex].GetInheritedState(rowIndex);
        }

        #endregion

        #region OfType

        public static IEnumerable<T> OfType<T>(this DataGridViewColumnCollection columns)
            where T : DataGridViewColumn {
            foreach (DataGridViewColumn column in columns) {
                var typed = column as T;
                if (typed != null)
                    yield return typed;
            }
        }

        #endregion

        #region ReplaceAll

        public static void ReplaceAll(this DataGridViewComboBoxCell.ObjectCollection collection, object[] items) {
            collection.Clear();
            collection.AddRange(items);
        }

        #endregion

        #region Selection

        public static void SelectRow(this DataGridViewRow row, bool select) {
            row.Selected = select;
            //row.DataGridView.CurrentCell = row.HeaderCell;
        }

        public static bool IsFirstRowSelected(this DataGridView dataGrid) {
            if (dataGrid == null || dataGrid.SelectedRows.Count == 0)
                return false;
            return dataGrid.Rows[0].Selected;
        }

        public static bool IsLastRowSelected(this DataGridView dataGrid) {
            if (dataGrid == null || dataGrid.SelectedRows.Count == 0)
                return false;
            return dataGrid.Rows[dataGrid.Rows.Count - 1].Selected;
        }

        public static bool IsOnlyFirstRowSelected(this DataGridView dataGrid) {
            if (dataGrid == null || dataGrid.SelectedRows.Count != 1)
                return false;
            return dataGrid.Rows[0].Selected;
        }

        public static bool IsOnlyLastRowSelected(this DataGridView dataGrid) {
            if (dataGrid == null || dataGrid.SelectedRows.Count != 1)
                return false;
            return dataGrid.Rows[dataGrid.Rows.Count - 1].Selected;
        }

        #endregion
    }
}
