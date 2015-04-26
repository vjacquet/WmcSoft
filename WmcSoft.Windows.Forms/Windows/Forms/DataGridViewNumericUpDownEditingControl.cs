// DataGridViewNumericUpDownXXX have been taken from
// http://msdn.microsoft.com/en-us/library/aa730881(VS.80).aspx
// and then adapted.
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using WmcSoft.Drawing;

namespace WmcSoft.Windows.Forms
{
    class DataGridViewNumericUpDownEditingControl : NumericUpDown
        , IDataGridViewEditingControl
    {

        #region Private fields

        private DataGridView _dataGridView;
        private bool _valueChanged;
        private int _rowIndex;

        #endregion

        #region Lifecycle

        /// <summary>
        /// Constructor of the editing control class
        /// </summary>
        public DataGridViewNumericUpDownEditingControl() {
            // The editing control must not be part of the tabbing loop
            TabStop = false;
        }

        #endregion

        /// <summary>
        /// Small utility function that updates the local dirty state and 
        /// notifies the grid of the value change.
        /// </summary>
        private void NotifyDataGridViewOfValueChange() {
            if (!_valueChanged) {
                _valueChanged = true;
                _dataGridView.NotifyCurrentCellDirty(true);
            }
        }

        #region Overrides

        /// <summary>
        /// Listen to the KeyPress notification to know when the value changed, and 
        /// notify the grid of the change.
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e) {
            base.OnKeyPress(e);

            // The value changes when a digit, the decimal separator, the group separator or
            // the negative sign is pressed.
            bool notifyValueChange = false;
            if (Char.IsDigit(e.KeyChar)) {
                notifyValueChange = true;
            } else {
                var nfi = CultureInfo.CurrentCulture.NumberFormat;
                notifyValueChange = e.KeyChar.EqualsAnyOf(nfi.NumberDecimalSeparator, nfi.NumberGroupSeparator, nfi.NegativeSign);
            }

            if (notifyValueChange) {
                NotifyDataGridViewOfValueChange();
            }
        }

        /// <summary>
        /// Listen to the ValueChanged notification to forward the change to the grid.
        /// </summary>
        protected override void OnValueChanged(EventArgs e) {
            base.OnValueChanged(e);
            if (Focused) {
                NotifyDataGridViewOfValueChange();
            }
        }

        /// <summary>
        /// A few keyboard messages need to be forwarded to the inner textbox of the
        /// NumericUpDown control so that the first character pressed appears in it.
        /// </summary>
        protected override bool ProcessKeyEventArgs(ref Message m) {
            var textBox = this.GetTextBox();
            if (textBox != null) {
                textBox.SendMessage(ref m);
                return true;
            } else {
                return base.ProcessKeyEventArgs(ref m);
            }
        }

        #endregion

        #region IDataGridViewEditingControl Membres

        /// <summary>
        /// Changes the control's user interface (UI) to be consistent with the specified cell style.
        /// </summary>
        public virtual void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle) {
            Font = dataGridViewCellStyle.Font;
            if (dataGridViewCellStyle.BackColor.A < 255) {
                // The NumericUpDown control does not support transparent back colors
                var opaqueBackColor = dataGridViewCellStyle.BackColor.MakeOpaque();
                BackColor = opaqueBackColor;
                //_dataGridView.EditingPanel.BackColor = opaqueBackColor;
            } else {
                BackColor = dataGridViewCellStyle.BackColor;
            }
            ForeColor = dataGridViewCellStyle.ForeColor;
            TextAlign = DataGridViewNumericUpDownCell.TranslateAlignment(dataGridViewCellStyle.Alignment);
        }

        /// <summary>
        /// Gets or sets the DataGridView that contains the cell.
        /// </summary>
        public virtual DataGridView EditingControlDataGridView {
            get { return _dataGridView; }
            set { _dataGridView = value; }
        }

        /// <summary>
        /// Gets or sets the formatted value of the cell being modified by the editor.
        /// </summary>
        public virtual object EditingControlFormattedValue {
            get { return GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting); }
            set { Text = (string)value; }
        }

        /// <summary>
        /// Gets or sets the index of the hosting cell's parent row.
        /// </summary>
        public virtual int EditingControlRowIndex {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the value of the editing control 
        /// differs from the value of the hosting cell.
        /// </summary>
        public virtual bool EditingControlValueChanged {
            get { return _valueChanged; }
            set { _valueChanged = value; }
        }

        /// <summary>
        /// Determines whether the specified key is a regular input key that the editing control should process
        /// or a special key that the DataGridView should process.
        /// </summary>
        /// <param name="keyData">A Keys that represents the key that was pressed.</param>
        /// <param name="dataGridViewWantsInputKey">true when the DataGridView wants to process the Keys in keyData; otherwise, false.</param>
        /// <returns>true if the specified key is a regular input key that should be handled by the editing control;
        /// otherwise, false.</returns>
        public virtual bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey) {
            switch (keyData & Keys.KeyCode) {
            case Keys.Down:
                // If the current value hasn't reached its minimum yet, handle the key. Otherwise let
                // the grid handle it.
                if (Value > Minimum)
                    return true;
                break;
            case Keys.Up:
                // If the current value hasn't reached its maximum yet, handle the key. Otherwise let
                // the grid handle it.
                if (Value < Maximum)
                    return true;
                break;
            case Keys.Right: {
                    var textBox = Controls[1] as TextBox;
                    if (textBox != null) {
                        // If the end of the selection is at the end of the string,
                        // let the DataGridView treat the key message
                        if ((RightToLeft == RightToLeft.No && !(textBox.SelectionLength == 0 && textBox.SelectionStart == textBox.TextLength))
                            || (RightToLeft == RightToLeft.Yes && !(textBox.SelectionLength == 0 && textBox.SelectionStart == 0))) {
                            return true;
                        }
                    }
                    break;
                }
            case Keys.Left: {
                    var textBox = Controls[1] as TextBox;
                    if (textBox != null) {
                        // If the end of the selection is at the begining of the string
                        // or if the entire text is selected and we did not start editing,
                        // send this character to the dataGridView, else process the key message
                        if ((RightToLeft == RightToLeft.No && !(textBox.SelectionLength == 0 && textBox.SelectionStart == 0))
                            || (RightToLeft == RightToLeft.Yes && !(textBox.SelectionLength == 0 && textBox.SelectionStart == textBox.TextLength))) {
                            return true;
                        }
                    }
                    break;
                }
            case Keys.Home:
            case Keys.End: {
                    // Let the grid handle the key if the entire text is selected.
                    var textBox = Controls[1] as TextBox;
                    if (textBox != null) {
                        if (textBox.SelectionLength != textBox.TextLength)
                            return true;
                    }
                    break;
                }
            case Keys.Delete: {
                    // Let the grid handle the key if the carret is at the end of the text.
                    var textBox = Controls[1] as TextBox;
                    if (textBox != null) {
                        if (textBox.SelectionLength > 0 || textBox.SelectionStart < textBox.TextLength)
                            return true;
                    }
                    break;
                }
            }
            return !dataGridViewWantsInputKey;
        }

        /// <summary>
        /// Gets the cursor used when the mouse pointer is over the DataGridView.EditingPanel but not over the editing control.
        /// </summary>
        public virtual Cursor EditingPanelCursor {
            get { return Cursors.Default; }
        }

        /// <summary>
        /// Retrieves the formatted value of the cell.
        /// </summary>
        public virtual object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context) {
            bool userEdit = UserEdit;
            try {
                // Prevent the Value from being set to Maximum or Minimum when the cell is being painted.
                UserEdit = (context & DataGridViewDataErrorContexts.Display) == 0;
                return Value.ToString((ThousandsSeparator ? "N" : "F") + DecimalPlaces.ToString());
            }
            finally {
                UserEdit = userEdit;
            }
        }

        /// <summary>
        /// Prepares the currently selected cell for editing.
        /// </summary>
        public virtual void PrepareEditingControlForEdit(bool selectAll) {
            var textBox = Controls[1] as TextBox;
            if (textBox == null)
                return;

            if (selectAll) {
                textBox.SelectAll();
            } else {
                // Do not select all the text, but
                // position the caret at the end of the text
                textBox.SelectionStart = textBox.TextLength;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cell contents need to be repositioned whenever the value changes.
        /// </summary>
        public virtual bool RepositionEditingControlOnValueChange {
            get { return false; }
        }

        #endregion
    }

}
