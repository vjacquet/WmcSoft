// DataGridViewNumericUpDownXXX have been taken from
// http://msdn.microsoft.com/en-us/library/aa730881(VS.80).aspx
// and then adapted.
using System;
using System.Drawing;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics;
using WmcSoft.ComponentModel;
using WmcSoft.Collections;

namespace WmcSoft.Windows.Forms
{
    public class DataGridViewRadioButtonCell : DataGridViewComboBoxCell, IDataGridViewEditingCell
    {
        /// <summary>
        /// Convenient enumeration using privately for calculating preferred cell sizes.
        /// </summary>
        private enum DataGridViewRadioButtonFreeDimension
        {
            Both,
            Height,
            Width
        }

        private const byte IconMarginWidth = 4;
        private const byte IconMarginHeight = 4;
        private const byte IconsWidth = 12;
        private const byte IconsHeight = 11;

        internal const int DefaultMaxDisplayedItems = 3;
        private const byte Margin = 2;

        // codes used for the mouseLocationCode static variable:
        private const int MouseLocationGeneric = -3;
        private const int MouseLocationBottomScrollButton = -2; // mouse is over bottom scroll button
        private const int MouseLocationTopScrollButton = -1;    // mouse is over top scroll button

        private static int mouseLocationCode = MouseLocationGeneric;
        // -3 no particular location
        // -2 mouse over bottom scroll button
        // -1 mouse over top scroll button
        // 0-N mouse over radio button glyph

        private DataGridViewRadioButtonCellLayout _layout;   // represents the current layout information of the cell
        private PropertyDescriptor _displayMemberProperty;   // Property descriptor for the DisplayMember property
        private PropertyDescriptor _valueMemberProperty;     // Property descriptor for the ValueMember property
        private CurrencyManager _dataManager;                // Currency manager for the cell's DataSource
        private int _maxDisplayedItems;                      // Maximum number of radio buttons displayed by the cell
        private int _selectedItemIndex;                      // Index of the currently selected radio button entry
        private int _focusedItemIndex;                       // Index of the focused radio button entry
        private int _pressedItemIndex;                       // Index of the currently pressed radio button entry
        private bool _dataSourceInitializedHookedUp;         // Indicates whether the DataSource's Initialized event is listened to
        private bool _valueChanged;                          // Stores whether the cell's value was changed since it became the current cell
        private bool _handledKeyDown;                        // Indicates whether the cell handled the key down notification
        private bool _mouseUpHooked;                         // Indicates whether the cell listens to the grid's MouseUp event

        /// <summary>
        /// DataGridViewRadioButtonCell class constructor.
        /// </summary>
        public DataGridViewRadioButtonCell() {
            _maxDisplayedItems = DefaultMaxDisplayedItems;
            _layout = new DataGridViewRadioButtonCellLayout();
            _selectedItemIndex = -1;
            _focusedItemIndex = -1;
            _pressedItemIndex = -1;
        }

        #region IDataGridViewEditingCell implentation

        /// <summary>
        /// Represents the cell's formatted value
        /// </summary>
        public virtual object EditingCellFormattedValue {
            get {
                return GetEditingCellFormattedValue(DataGridViewDataErrorContexts.Formatting);
            }
            set {
                if (this.FormattedValueType == null) {
                    throw new ArgumentException("FormattedValueType property of a cell cannot be null.");
                }
                if (value == null || !this.FormattedValueType.IsAssignableFrom(value.GetType())) {
                    // Assigned formatted value may not be of the good type, in cases where the app
                    // is feeding wrong values to the cell in virtual / databound mode.
                    throw new ArgumentException("The value provided for the DataGridViewRadioButtonCell has the wrong type.");
                }

                // Try to locate the item that corresponds to the 'value' provided.
                for (int itemIndex = 0; itemIndex < this.Items.Count; itemIndex++) {
                    object item = this.Items[itemIndex];
                    object displayValue = GetItemDisplayValue(item);
                    if (value.Equals(displayValue)) {
                        // 'value' was found. It becomes the new selected item.
                        _selectedItemIndex = itemIndex;
                        return;
                    }
                }

                string strValue = value as string;
                if (strValue == string.Empty) {
                    // Special case the empty string situation - reset the selected item
                    _selectedItemIndex = -1;
                    return;
                }

                // 'value' could not be matched against an item in the Items collection.
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Keeps track of whether the cell's value has changed or not.
        /// </summary>
        public virtual bool EditingCellValueChanged {
            get { return _valueChanged; }
            set { _valueChanged = value; }
        }

        /// <summary>
        /// Returns the current formatted value of the cell
        /// </summary>
        public virtual object GetEditingCellFormattedValue(DataGridViewDataErrorContexts context) {
            if (FormattedValueType == null) {
                throw new InvalidOperationException("FormattedValueType property of a cell cannot be null.");
            }
            if (_selectedItemIndex == -1) {
                return null;
            }
            object item = Items[_selectedItemIndex];
            object displayValue = GetItemDisplayValue(item);
            // Making sure the returned value has an acceptable type
            if (FormattedValueType.IsAssignableFrom(displayValue.GetType())) {
                return displayValue;
            } else {
                return null;
            }
        }

        /// <summary>
        /// Called by the grid when the cell enters editing mode. 
        /// </summary>
        public virtual void PrepareEditingCellForEdit(bool selectAll) {
            // This cell type has nothing to do here.
        }

        #endregion

        /// <summary>
        /// Stores the CurrencyManager associated to the cell's DataSource
        /// </summary>
        private CurrencyManager DataManager {
            get {
                CurrencyManager cm = _dataManager;
                if (cm == null && DataSource != null && DataGridView != null && DataGridView.BindingContext != null && !(DataSource == Convert.DBNull)) {
                    var dsInit = DataSource as ISupportInitializeNotification;
                    if (dsInit != null && !dsInit.IsInitialized) {
                        // The datasource is not ready yet. Attaching to its Initialized event to be notified
                        // when it's finally ready
                        if (!_dataSourceInitializedHookedUp) {
                            dsInit.Initialized += DataSource_Initialized;
                            _dataSourceInitializedHookedUp = true;
                        }
                    } else {
                        cm = (CurrencyManager)DataGridView.BindingContext[this.DataSource];
                        DataManager = cm;
                    }
                }
                return cm;
            }
            set {
                _dataManager = value;
            }
        }

        /// <summary>
        /// Overrides the DataGridViewComboBox's implementation of the DataSource property to 
        /// initialize the displayMemberProperty and valueMemberProperty members.
        /// </summary>
        public override object DataSource {
            get {
                return base.DataSource;
            }
            set {
                if (DataSource != value) {
                    // Invalidate the currency manager
                    DataManager = null;

                    var dsInit = DataSource as ISupportInitializeNotification;
                    if (dsInit != null && _dataSourceInitializedHookedUp) {
                        // If we previously hooked the datasource's ISupportInitializeNotification
                        // Initialized event, then unhook it now (we don't always hook this event,
                        // only if we needed to because the datasource was previously uninitialized)
                        dsInit.Initialized -= DataSource_Initialized;
                        _dataSourceInitializedHookedUp = false;
                    }

                    base.DataSource = value;

                    // Update the displayMemberProperty and valueMemberProperty members.
                    try {
                        InitializeDisplayMemberPropertyDescriptor(DisplayMember);
                    }
                    catch {
                        Debug.Assert(DisplayMember != null && DisplayMember.Length > 0);
                        InitializeDisplayMemberPropertyDescriptor(null);
                    }

                    try {
                        InitializeValueMemberPropertyDescriptor(ValueMember);
                    }
                    catch {
                        Debug.Assert(ValueMember != null && ValueMember.Length > 0);
                        InitializeValueMemberPropertyDescriptor(null);
                    }

                    if (value == null) {
                        InitializeDisplayMemberPropertyDescriptor(null);
                        InitializeValueMemberPropertyDescriptor(null);
                    }
                }
            }
        }

        /// <summary>
        /// Overrides the DataGridViewComboBox's implementation of the DisplayMember property to
        /// update the displayMemberProperty member.
        /// </summary>
        public override string DisplayMember {
            get {
                return base.DisplayMember;
            }
            set {
                base.DisplayMember = value;
                InitializeDisplayMemberPropertyDescriptor(value);
            }
        }

        /// <summary>
        /// Overrides the base implementation to replace the 'complex editing experience'
        /// with a 'simple editing experience'.
        /// </summary>
        public override Type EditType {
            get {
                // Return null since no editing control is used for the editing experience.
                return null;
            }
        }

        /// <summary>
        /// Custom property that represents the maximum number of radio buttons shown by the cell.
        /// </summary>
        [DefaultValue(DefaultMaxDisplayedItems)]
        public int MaxDisplayedItems {
            get {
                return _maxDisplayedItems;
            }
            set {
                if (value < 1 || value > 100) {
                    throw new ArgumentOutOfRangeException("MaxDisplayedItems");
                }
                _maxDisplayedItems = value;

                if (DataGridView != null && !DataGridView.IsDisposed && !DataGridView.Disposing) {
                    if (RowIndex == -1) {
                        // Invalidate and autosize column
                        DataGridView.InvalidateColumn(ColumnIndex);

                        // TODO: Add code to autosize the cell's column, the rows, the column headers 
                        // and the row headers depending on their autosize settings.
                        // The DataGridView control does not expose a public method that takes care of this.
                    } else {
                        // The DataGridView control exposes a public method called UpdateCellValue
                        // that invalidates the cell so that it gets repainted and also triggers all
                        // the necessary autosizing: the cell's column and/or row, the column headers
                        // and the row headers are autosized depending on their autosize settings.
                        DataGridView.UpdateCellValue(ColumnIndex, RowIndex);
                    }
                }
            }
        }

        /// <remarks>
        /// Called internally by the DataGridViewRadioButtonColumn class to avoid the invalidation
        /// done by the MaxDisplayedItems setter above (for performance reasons).
        /// </remarks>
        internal void SetMaxDisplayedItemsInternal(int value) {
            Debug.Assert(value >= 1 && value <= 100);
            _maxDisplayedItems = value;
        }

        /// <summary>
        /// Utility function that returns the standard thickness (in pixels) of the four borders of the cell.
        /// </summary>
        private Rectangle StandardBorderWidths {
            get {
                if (DataGridView != null) {
                    var dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle();
                    var dgvabsEffective = AdjustCellBorderStyle(DataGridView.AdvancedCellBorderStyle,
                          dataGridViewAdvancedBorderStylePlaceholder,
                          singleVerticalBorderAdded: false,
                          singleHorizontalBorderAdded: false,
                          isFirstDisplayedColumn: false,
                          isFirstDisplayedRow: false);
                    return BorderWidths(dgvabsEffective);
                } else {
                    return Rectangle.Empty;
                }
            }
        }

        /// <summary>
        /// Overrides the DataGridViewComboBox's implementation of the ValueMember property to
        /// update the valueMemberProperty member.
        /// </summary>
        public override string ValueMember {
            get {
                return base.ValueMember;
            }
            set {
                base.ValueMember = value;
                InitializeValueMemberPropertyDescriptor(value);
            }
        }

        /// <summary>
        /// Utility function that returns the cell state inherited from the owning row and column.
        /// </summary>
        private DataGridViewElementStates CellStateFromColumnRowStates(DataGridViewElementStates rowState) {
            Debug.Assert(DataGridView != null);
            Debug.Assert(ColumnIndex >= 0);
            var orFlags = DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected;
            var andFlags = DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible;
            var cellState = (OwningColumn.State & orFlags)
                | (rowState & orFlags)
                | ((OwningColumn.State & andFlags) & (rowState & andFlags));
            return cellState;
        }

        /// <summary>
        /// Custom implementation of the Clone method to copy over the special properties of the cell.
        /// </summary>
        public override object Clone() {
            DataGridViewRadioButtonCell dataGridViewCell = base.Clone() as DataGridViewRadioButtonCell;
            if (dataGridViewCell != null) {
                dataGridViewCell.MaxDisplayedItems = MaxDisplayedItems;
            }
            return dataGridViewCell;
        }

        /// <summary>
        /// Computes the layout of the cell and optionally paints it.
        /// </summary>
        private void ComputeLayout(Graphics graphics,
                                   Rectangle clipBounds,
                                   Rectangle cellBounds,
                                   int rowIndex,
                                   DataGridViewElementStates cellState,
                                   object formattedValue,
                                   string errorText,
                                   DataGridViewCellStyle cellStyle,
                                   DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                   DataGridViewPaintParts paintParts,
                                   bool paint) {
            if (paint && DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.Border)) {
                // Paint the borders first
                PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
            }

            // Discard the space taken up by the borders.
            var borderWidths = BorderWidths(advancedBorderStyle);
            var valBounds = cellBounds;
            valBounds.Offset(borderWidths.X, borderWidths.Y);
            valBounds.Width -= borderWidths.Right;
            valBounds.Height -= borderWidths.Bottom;

            SolidBrush backgroundBrush = null;
            try {
                Point ptCurrentCell = this.DataGridView.CurrentCellAddress;
                bool cellCurrent = ptCurrentCell.X == this.ColumnIndex && ptCurrentCell.Y == rowIndex;
                bool cellSelected = (cellState & DataGridViewElementStates.Selected) != 0;
                bool mouseOverCell = cellBounds.Contains(this.DataGridView.PointToClient(Control.MousePosition));

                if (DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.SelectionBackground) && cellSelected) {
                    backgroundBrush = new SolidBrush(cellStyle.SelectionBackColor);
                } else {
                    backgroundBrush = new SolidBrush(cellStyle.BackColor);
                }

                if (paint && DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.Background) && backgroundBrush.Color.A == 255) {
                    Rectangle backgroundRect = valBounds;
                    backgroundRect.Intersect(clipBounds);
                    graphics.FillRectangle(backgroundBrush, backgroundRect);
                }

                // Discard the space taken up by the padding area.
                if (cellStyle.Padding != Padding.Empty) {
                    valBounds.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
                    valBounds.Width -= cellStyle.Padding.Horizontal;
                    valBounds.Height -= cellStyle.Padding.Vertical;
                }

                Rectangle errorBounds = valBounds;
                Rectangle scrollBounds = valBounds;

                _layout.ScrollingNeeded = GetScrollingNeeded(graphics, rowIndex, cellStyle, valBounds.Size);

                if (_layout.ScrollingNeeded) {
                    _layout.ScrollButtonsSize = ScrollBarRenderer.GetSizeBoxSize(graphics, ScrollBarState.Normal);
                    // Discard the space required for displaying the 2 scroll buttons
                    valBounds.Width -= _layout.ScrollButtonsSize.Width;
                }

                valBounds.Inflate(-Margin, -Margin);

                // Layout / paint the radio buttons themselves
                _layout.RadioButtonsSize = RadioButtonRenderer.GetGlyphSize(graphics, RadioButtonState.CheckedNormal);
                _layout.DisplayedItemsCount = 0;
                _layout.TotallyDisplayedItemsCount = 0;
                if (valBounds.Width > 0 && valBounds.Height > 0) {
                    _layout.FirstDisplayedItemLocation = new Point(valBounds.Left + Margin, valBounds.Top + Margin);
                    int textHeight = cellStyle.Font.Height;
                    int itemIndex = _layout.FirstDisplayedItemIndex;
                    Rectangle radiosBounds = valBounds;
                    if(Items.Count == 0) {
                        var owner = OwningColumn as DataGridViewRadioButtonColumn;
                        Items.ReplaceAll(owner.Items.ToArray());
                    }
                    while (itemIndex < Items.Count &&
                           itemIndex < _layout.FirstDisplayedItemIndex + _maxDisplayedItems &&
                           radiosBounds.Height > 0) {
                        if (paint && DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.ContentBackground)) {
                            Rectangle itemRect = radiosBounds;
                            itemRect.Intersect(clipBounds);
                            if (!itemRect.IsEmpty) {
                                bool itemReadOnly = (cellState & DataGridViewElementStates.ReadOnly) != 0;
                                bool itemSelected = false;
                                if (formattedValue != null) {
                                    object displayValue = GetItemDisplayValue(this.Items[itemIndex]);
                                    if (formattedValue.Equals(displayValue)) {
                                        itemSelected = true;
                                    }
                                }
                                PaintItem(graphics,
                                          radiosBounds,
                                          rowIndex,
                                          itemIndex,
                                          cellState,
                                          cellStyle,
                                          itemReadOnly,
                                          itemSelected,
                                          mouseOverCell,
                                          cellCurrent && _focusedItemIndex == itemIndex && DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.Focus));
                            }
                        }
                        itemIndex++;
                        radiosBounds.Y += textHeight + Margin;
                        radiosBounds.Height -= (textHeight + Margin);
                        if (radiosBounds.Height >= 0) {
                            _layout.TotallyDisplayedItemsCount++;
                        }
                        _layout.DisplayedItemsCount++;
                    }
                    _layout.ContentBounds = new Rectangle(_layout.FirstDisplayedItemLocation, new Size(_layout.RadioButtonsSize.Width, _layout.DisplayedItemsCount * (textHeight + Margin)));
                } else {
                    _layout.FirstDisplayedItemLocation = new Point(-1, -1);
                    _layout.ContentBounds = Rectangle.Empty;
                }

                if (_layout.ScrollingNeeded) {
                    // Layout / paint the 2 scroll buttons
                    var rectArrow = new Rectangle(scrollBounds.Right - _layout.ScrollButtonsSize.Width,
                                                  scrollBounds.Top,
                                                  _layout.ScrollButtonsSize.Width,
                                                  _layout.ScrollButtonsSize.Height);
                    _layout.UpButtonLocation = rectArrow.Location;
                    if (paint && DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.ContentBackground)) {
                        ScrollBarRenderer.DrawArrowButton(graphics, rectArrow, GetScrollBarUpArrowButtonState(mouseOverCell ? mouseLocationCode : MouseLocationGeneric, enabled: _layout.FirstDisplayedItemIndex > 0));
                    }
                    rectArrow.Y = scrollBounds.Bottom - _layout.ScrollButtonsSize.Height;
                    _layout.DownButtonLocation = rectArrow.Location;
                    if (paint && DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.ContentBackground)) {
                        ScrollBarRenderer.DrawArrowButton(graphics, rectArrow, GetScrollBarDownArrowButtonState(mouseOverCell ? mouseLocationCode : MouseLocationGeneric, enabled: _layout.FirstDisplayedItemIndex + _layout.TotallyDisplayedItemsCount < this.Items.Count));
                    }
                }

                // Finally paint the potential error icon
                if (paint &&
                    DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.ErrorIcon) &&
                    !(cellCurrent && this.DataGridView.IsCurrentCellInEditMode) &&
                    this.DataGridView.ShowCellErrors) {
                    PaintErrorIcon(graphics, clipBounds, errorBounds, errorText);
                }
            }
            finally {
                if (backgroundBrush != null) {
                    backgroundBrush.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns whether calling the OnContentClick method would force the owning row to be unshared.
        /// </summary>
        protected override bool ContentClickUnsharesRow(DataGridViewCellEventArgs e) {
            var ptCurrentCell = DataGridView.CurrentCellAddress;
            return ptCurrentCell.X == ColumnIndex
                && ptCurrentCell.Y == e.RowIndex
                && DataGridView.IsCurrentCellInEditMode;
        }

        /// <summary>
        /// Raised when the owning grid gets a MouseUp notification
        /// </summary>
        private void DataGridView_MouseUp(object sender, MouseEventArgs e) {
            // Unhook the event handler
            DataGridView.MouseUp -= DataGridView_MouseUp;
            _mouseUpHooked = false;
            // Reset the pressed item index. Since the mouse was released, no item can be pressed anymore.
            _pressedItemIndex = -1;
        }

        /// <summary>
        /// Raised when the cell's DataSource is initialized.
        /// </summary>
        private void DataSource_Initialized(object sender, EventArgs e) {
            Debug.Assert(sender == this.DataSource);
            Debug.Assert(DataSource is ISupportInitializeNotification);
            Debug.Assert(_dataSourceInitializedHookedUp);

            var dsInit = this.DataSource as ISupportInitializeNotification;
            // Unhook the Initialized event.
            if (dsInit != null) {
                dsInit.Initialized -= DataSource_Initialized;
            }

            // The wait is over: the DataSource is initialized.
            _dataSourceInitializedHookedUp = false;

            // Check the DisplayMember and ValueMember values - will throw if values don't match existing fields.
            InitializeDisplayMemberPropertyDescriptor(DisplayMember);
            InitializeValueMemberPropertyDescriptor(ValueMember);
        }

        /// <summary>
        /// Returns whether calling the OnEnter method would force the owning row to be unshared.
        /// </summary>
        protected override bool EnterUnsharesRow(int rowIndex, bool throughMouseClick) {
            return _focusedItemIndex == -1;
        }

        /// <summary>
        /// Custom implementation of the GetContentBounds method which delegates most of the work to the ComputeLayout function.
        /// </summary>
        protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex) {
            if (DataGridView == null || rowIndex < 0 || OwningColumn == null)
                return Rectangle.Empty;

            // First determine the effective border style of this cell.
            bool singleVerticalBorderAdded = !DataGridView.RowHeadersVisible && DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
            bool singleHorizontalBorderAdded = !DataGridView.ColumnHeadersVisible && DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
            var dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle();

            Debug.Assert(rowIndex > -1 && this.OwningColumn != null);

            var dgvabsEffective = AdjustCellBorderStyle(DataGridView.AdvancedCellBorderStyle,
                                                        dataGridViewAdvancedBorderStylePlaceholder,
                                                        singleVerticalBorderAdded,
                                                        singleHorizontalBorderAdded,
                                                        isFirstDisplayedRow: rowIndex == DataGridView.Rows.GetFirstRow(DataGridViewElementStates.Displayed),
                                                        isFirstDisplayedColumn: ColumnIndex == DataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Displayed).Index);

            // Next determine the state of this cell.
            var rowState = DataGridView.Rows.GetRowState(rowIndex);
            var cellState = CellStateFromColumnRowStates(rowState);
            cellState |= State;

            // Then the bounds of this cell.
            var cellBounds = new Rectangle(new Point(0, 0), GetSize(rowIndex));

            // Finally compute the layout of the cell and return the resulting content bounds.
            ComputeLayout(graphics,
                          cellBounds,
                          cellBounds,
                          rowIndex,
                          cellState,
                          null /*formattedValue*/,            // contentBounds is independent of formattedValue
                          null /*errorText*/,                 // contentBounds is independent of errorText
                          cellStyle,
                          dgvabsEffective,
                          DataGridViewPaintParts.ContentForeground,
                          false /*paint*/);

            return _layout.ContentBounds;
        }

        /// <summary>
        /// Utility function that converts a constraintSize provided to GetPreferredSize into a 
        /// DataGridViewRadioButtonFreeDimension enum value.
        /// </summary>
        private static DataGridViewRadioButtonFreeDimension GetFreeDimensionFromConstraint(Size constraintSize) {
            if (constraintSize.Width < 0 || constraintSize.Height < 0)
                throw new ArgumentOutOfRangeException("constraintSize");

            if (constraintSize.Width == 0)
                return (constraintSize.Height == 0)
                    ? DataGridViewRadioButtonFreeDimension.Both
                    : DataGridViewRadioButtonFreeDimension.Width;
            if (constraintSize.Height == 0)
                return DataGridViewRadioButtonFreeDimension.Height;

            throw new ArgumentOutOfRangeException("constraintSize");
        }

        /// <summary>
        /// Utility function that returns the display value of an item given the 
        /// display/value property descriptors and display/value property names.
        /// </summary>
        private object GetItemDisplayValue(object item) {
            Debug.Assert(item != null);
            bool displayValueSet = false;
            object displayValue = null;
            if (_displayMemberProperty != null) {
                displayValue = _displayMemberProperty.GetValue(item);
                displayValueSet = true;
            } else if (_valueMemberProperty != null) {
                displayValue = _valueMemberProperty.GetValue(item);
                displayValueSet = true;
            } else if (!String.IsNullOrEmpty(DisplayMember)) {
                var propDesc = TypeDescriptor.GetProperties(item).Find(DisplayMember, ignoreCase: true);
                displayValueSet = propDesc.TryGetValue(item, out displayValue);
            } else if (!String.IsNullOrEmpty(ValueMember)) {
                var propDesc = TypeDescriptor.GetProperties(item).Find(ValueMember, ignoreCase: true);
                displayValueSet = propDesc.TryGetValue(item, out displayValue);
            }
            if (!displayValueSet) {
                displayValue = item;
            }
            return displayValue;
        }

        /// <summary>
        /// Utility function that returns the value of an item given the 
        /// display/value property descriptors and display/value property names.
        /// </summary>
        private object GetItemValue(object item) {
            bool valueSet = false;
            object value = null;
            if (_valueMemberProperty != null) {
                value = _valueMemberProperty.GetValue(item);
                valueSet = true;
            } else if (_displayMemberProperty != null) {
                value = _displayMemberProperty.GetValue(item);
                valueSet = true;
            } else if (!String.IsNullOrEmpty(ValueMember)) {
                var propDesc = TypeDescriptor.GetProperties(item).Find(ValueMember, ignoreCase: true);
                valueSet = propDesc.TryGetValue(item, out value);
            }
            if (!valueSet && !String.IsNullOrEmpty(DisplayMember)) {
                var propDesc = TypeDescriptor.GetProperties(item).Find(DisplayMember, ignoreCase: true);
                valueSet = propDesc.TryGetValue(item, out value);
            }
            if (!valueSet) {
                value = item;
            }
            return value;
        }

        /// <summary>
        /// Returns the code identifying the part of the cell which is underneath the mouse pointer.
        /// </summary>
        private int GetMouseLocationCode(Graphics graphics, int rowIndex, DataGridViewCellStyle cellStyle, int mouseX, int mouseY) {
            // First determine this cell's effective border style.
            bool singleVerticalBorderAdded = !DataGridView.RowHeadersVisible && DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
            bool singleHorizontalBorderAdded = !DataGridView.ColumnHeadersVisible && DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
            bool isFirstDisplayedColumn = ColumnIndex == DataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Displayed).Index;
            bool isFirstDisplayedRow = rowIndex == DataGridView.Rows.GetFirstRow(DataGridViewElementStates.Displayed);
            var dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle();
            var dataGridViewAdvancedBorderStyleEffective = AdjustCellBorderStyle(DataGridView.AdvancedCellBorderStyle,
                                                                                 dataGridViewAdvancedBorderStylePlaceholder,
                                                                                 singleVerticalBorderAdded,
                                                                                 singleHorizontalBorderAdded,
                                                                                 isFirstDisplayedColumn,
                                                                                 isFirstDisplayedRow);
            // Then its size.
            var cellBounds = this.DataGridView.GetCellDisplayRectangle(ColumnIndex, rowIndex, cutOverflow: false);
            Debug.Assert(GetSize(rowIndex) == cellBounds.Size);

            // Recompute the layout of the cell.
            ComputeLayout(graphics,
                          cellBounds,
                          cellBounds,
                          rowIndex,
                          DataGridViewElementStates.None,
                          null /*formattedValue*/,
                          null /*errorText*/,
                          cellStyle,
                          dataGridViewAdvancedBorderStyleEffective,
                          DataGridViewPaintParts.None,
                          false /*paint*/);

            // Deduce the cell part beneath the mouse pointer.
            var mousePosition = DataGridView.PointToClient(Control.MousePosition);
            Rectangle rect;
            if (_layout.ScrollingNeeded) {
                // Is the mouse over the bottom scroll button?
                rect = new Rectangle(_layout.DownButtonLocation, _layout.ScrollButtonsSize);
                if (rect.Contains(mousePosition)) {
                    return MouseLocationBottomScrollButton;
                }
                // Is the mouse over the upper scroll button?
                rect = new Rectangle(_layout.UpButtonLocation, _layout.ScrollButtonsSize);
                if (rect.Contains(mousePosition)) {
                    return MouseLocationTopScrollButton;
                }
            }
            if (_layout.DisplayedItemsCount > 0) {
                Point radioButtonLocation = _layout.FirstDisplayedItemLocation;
                int textHeight = cellStyle.Font.Height;
                int itemIndex = _layout.FirstDisplayedItemIndex;
                Rectangle radioButtonBounds = new Rectangle(radioButtonLocation, _layout.RadioButtonsSize);
                while (itemIndex < Items.Count &&
                       itemIndex < _layout.FirstDisplayedItemIndex + _maxDisplayedItems &&
                       itemIndex - _layout.FirstDisplayedItemIndex < _layout.DisplayedItemsCount) {
                    if (radioButtonBounds.Contains(mousePosition)) {
                        // The mouse is over a radio button
                        return itemIndex - _layout.FirstDisplayedItemIndex;
                    }
                    itemIndex++;
                    radioButtonBounds.Y += textHeight + Margin;
                }
            }
            return MouseLocationGeneric;
        }

        private ScrollBarArrowButtonState GetScrollBarUpArrowButtonState(int mouseLocationCode, bool enabled) {
            if (!enabled)
                return ScrollBarArrowButtonState.UpDisabled;
            if (mouseLocationCode == MouseLocationTopScrollButton)
                return (Control.MouseButtons == MouseButtons.Left)
                    ? ScrollBarArrowButtonState.UpPressed
                    : ScrollBarArrowButtonState.UpHot;
            return ScrollBarArrowButtonState.UpNormal;
        }

        private ScrollBarArrowButtonState GetScrollBarDownArrowButtonState(int mouseLocationCode, bool enabled) {
            if (!enabled)
                return ScrollBarArrowButtonState.DownDisabled;
            if (mouseLocationCode == MouseLocationBottomScrollButton)
                return (Control.MouseButtons == MouseButtons.Left)
                    ? ScrollBarArrowButtonState.DownPressed
                    : ScrollBarArrowButtonState.DownHot;
            return ScrollBarArrowButtonState.DownNormal;
        }

        /// <summary>
        /// Custom implementation of the GetPreferredSize method.
        /// </summary>
        protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize) {
            if (this.DataGridView == null) {
                return new Size(-1, -1);
            }
            DataGridViewRadioButtonFreeDimension freeDimension = DataGridViewRadioButtonCell.GetFreeDimensionFromConstraint(constraintSize);
            Rectangle borderWidthsRect = this.StandardBorderWidths;
            int borderAndPaddingWidths = borderWidthsRect.Left + borderWidthsRect.Width + cellStyle.Padding.Horizontal;
            int borderAndPaddingHeights = borderWidthsRect.Top + borderWidthsRect.Height + cellStyle.Padding.Vertical;
            int preferredHeight = 0, preferredWidth = 0;
            // Assuming here that all radio button states use the same size.
            Size radioButtonGlyphSize = RadioButtonRenderer.GetGlyphSize(graphics, RadioButtonState.CheckedNormal);

            if (freeDimension != DataGridViewRadioButtonFreeDimension.Width) {
                preferredHeight = System.Math.Min(this.Items.Count, this.MaxDisplayedItems) * (System.Math.Max(cellStyle.Font.Height, radioButtonGlyphSize.Height) + Margin) + Margin;
                preferredHeight += 2 * Margin + borderAndPaddingHeights;
            }

            if (freeDimension != DataGridViewRadioButtonFreeDimension.Height) {
                TextFormatFlags flags = TextFormatFlags.Top | TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.NoPrefix;

                if (this.Items.Count > 0) {
                    // Figure out the width of the longest entry
                    int maxPreferredItemWidth = -1, preferredItemWidth;
                    foreach (object item in this.Items) {
                        string formattedValue = GetFormattedValue(GetItemValue(item), rowIndex, ref cellStyle, null, null, DataGridViewDataErrorContexts.Formatting | DataGridViewDataErrorContexts.PreferredSize) as string;
                        if (formattedValue != null) {
                            preferredItemWidth = DataGridViewCell.MeasureTextSize(graphics, formattedValue, cellStyle.Font, flags).Width;
                        } else {
                            preferredItemWidth = DataGridViewCell.MeasureTextSize(graphics, " ", cellStyle.Font, flags).Width;
                        }
                        if (preferredItemWidth > maxPreferredItemWidth) {
                            maxPreferredItemWidth = preferredItemWidth;
                        }
                    }
                    preferredWidth = maxPreferredItemWidth;
                }

                if (freeDimension == DataGridViewRadioButtonFreeDimension.Width) {
                    Size contentSize = new Size(Int32.MaxValue, constraintSize.Height - borderAndPaddingHeights);
                    if (GetScrollingNeeded(graphics, rowIndex, cellStyle, contentSize)) {
                        // Accommodate the scrolling buttons
                        preferredWidth += ScrollBarRenderer.GetSizeBoxSize(graphics, ScrollBarState.Normal).Width;
                    }
                }

                preferredWidth += radioButtonGlyphSize.Width + 5 * Margin + borderAndPaddingWidths;
            }

            if (DataGridView.ShowCellErrors) {
                // Making sure that there is enough room for the potential error icon
                if (freeDimension != DataGridViewRadioButtonFreeDimension.Height) {
                    preferredWidth = Math.Max(preferredWidth, borderAndPaddingWidths + IconMarginWidth * 2 + IconsWidth);
                }
                if (freeDimension != DataGridViewRadioButtonFreeDimension.Width) {
                    preferredHeight = Math.Max(preferredHeight, borderAndPaddingHeights + IconMarginHeight * 2 + IconsHeight);
                }
            }

            return new Size(preferredWidth, preferredHeight);
        }

        /// <summary>
        /// Helper function that determines if scrolling buttons should be displayed
        /// </summary>
        private bool GetScrollingNeeded(Graphics graphics, int rowIndex, DataGridViewCellStyle cellStyle, Size contentSize) {
            if (Items.Count <= 1) {
                return false;
            }

            if (MaxDisplayedItems >= Items.Count &&
                Items.Count * (cellStyle.Font.Height + Margin) + Margin <= contentSize.Height /*- borderAndPaddingHeights*/) {
                // There is enough vertical room to display all the radio buttons
                return false;
            }

            // Is there enough room to display the scroll buttons?
            Size sizeBoxSize = ScrollBarRenderer.GetSizeBoxSize(graphics, ScrollBarState.Normal);
            if (2 * Margin + sizeBoxSize.Width > contentSize.Width || 2 * sizeBoxSize.Height > contentSize.Height) {
                // There isn't enough room to show the scroll buttons.
                return false;
            }

            // Scroll buttons are required and there is enough room for them.
            return true;
        }

        /// <summary>
        /// Helper function that sets the displayMemberProperty member based on the DataSource and the provided displayMember member name
        /// </summary>
        private void InitializeDisplayMemberPropertyDescriptor(string displayMember) {
            if (DataManager == null)
                return;

            if (String.IsNullOrEmpty(displayMember)) {
                _displayMemberProperty = null;
            } else {
                var displayBindingMember = new BindingMemberInfo(displayMember);
                // make the DataManager point to the sublist inside this.DataSource
                DataManager = DataGridView.BindingContext[DataSource, displayBindingMember.BindingPath] as CurrencyManager;
                var displayMemberProperty = DataManager.GetItemProperty(displayBindingMember.BindingField, ignoreCase: true);
                if (displayMemberProperty == null)
                    throw new ArgumentOutOfRangeException("displayMember");
                _displayMemberProperty = displayMemberProperty;
            }
        }

        /// <summary>
        /// Helper function that sets the valueMemberProperty member based on the DataSource and the provided valueMember member name
        /// </summary>
        private void InitializeValueMemberPropertyDescriptor(string valueMember) {
            if (DataManager == null)
                return;

            if (String.IsNullOrEmpty(valueMember)) {
                _valueMemberProperty = null;
            } else {
                BindingMemberInfo valueBindingMember = new BindingMemberInfo(valueMember);
                // make the DataManager point to the sublist inside this.DataSource
                DataManager = DataGridView.BindingContext[DataSource, valueBindingMember.BindingPath] as CurrencyManager;
                PropertyDescriptor valueMemberProperty = DataManager.GetItemProperty(valueBindingMember.BindingField, ignoreCase: true);
                if (valueMemberProperty == null)
                    throw new ArgumentOutOfRangeException("valueMember");
                _valueMemberProperty = valueMemberProperty;
            }
        }

        /// <summary>
        /// Helper function that invalidates the entire area of an entry
        /// </summary>
        private void InvalidateItem(int itemIndex, int rowIndex) {
            if (_layout.IsItemDisplayed(itemIndex)) {
                var cellStyle = GetInheritedStyle(null, rowIndex, includeColors: false);
                var radioButtonLocation = _layout.FirstDisplayedItemLocation;
                int textHeight = cellStyle.Font.Height;
                radioButtonLocation.Y += (textHeight + Margin) * (itemIndex - _layout.FirstDisplayedItemIndex);
                var cellSize = GetSize(rowIndex);
                DataGridView.Invalidate(new Rectangle(radioButtonLocation.X, radioButtonLocation.Y, cellSize.Width, Math.Max(textHeight + Margin, _layout.RadioButtonsSize.Height)));
            }
        }

        /// <summary>
        /// Helper function that invalidates the glyph section of an entry
        /// </summary>
        private void InvalidateRadioGlyph(int itemIndex, DataGridViewCellStyle cellStyle) {
            if (_layout.IsItemDisplayed(itemIndex)) {
                Point radioButtonLocation = _layout.FirstDisplayedItemLocation;
                int textHeight = cellStyle.Font.Height;
                radioButtonLocation.Y += (textHeight + Margin) * (itemIndex - _layout.FirstDisplayedItemIndex);
                DataGridView.Invalidate(new Rectangle(radioButtonLocation, _layout.RadioButtonsSize));
            }
        }

        /// <summary>
        /// Returns whether calling the OnKeyDown method would force the owning row to be unshared.
        /// </summary>
        protected override bool KeyDownUnsharesRow(KeyEventArgs e, int rowIndex) {
            if (!e.Alt && !e.Control && !e.Shift) {
                if (_handledKeyDown)
                    return true;
                if (e.KeyCode == Keys.Down && _focusedItemIndex < this.Items.Count - 1)
                    return true;
                if (e.KeyCode == Keys.Up && _focusedItemIndex > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns whether calling the OnKeyUp method would force the owning row to be unshared.
        /// </summary>
        protected override bool KeyUpUnsharesRow(KeyEventArgs e, int rowIndex) {
            if (!e.Alt && !e.Control && !e.Shift) {
                if (e.KeyCode == Keys.Down && _focusedItemIndex < this.Items.Count - 1 && _handledKeyDown)
                    return true;
                if (e.KeyCode == Keys.Up && _focusedItemIndex > 0 && _handledKeyDown)
                    return true;
            }
            if (_handledKeyDown) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns whether calling the OnMouseDown method would force the owning row to be unshared.
        /// </summary>
        protected override bool MouseDownUnsharesRow(DataGridViewCellMouseEventArgs e) {
            if (this.DataGridView == null)
                return false;

            if (e.Button == MouseButtons.Left) {
                var cellStyle = GetInheritedStyle(null, e.RowIndex, includeColors: false);
                int hit = GetMouseLocationCode(DataGridView.CreateGraphics(), e.RowIndex, cellStyle, e.X, e.Y);
                switch (hit) {
                case MouseLocationGeneric:
                    break;
                case MouseLocationBottomScrollButton:
                    if (_layout.FirstDisplayedItemIndex + _layout.DisplayedItemsCount < this.Items.Count)
                        return true;
                    break;
                case MouseLocationTopScrollButton:
                    if (_layout.FirstDisplayedItemIndex > 0)
                        return true;
                    break;
                default:
                    if (_pressedItemIndex != hit + _layout.FirstDisplayedItemIndex)
                        return true;
                    break;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns whether calling the OnMouseLeave method would force the owning row to be unshared.
        /// </summary>
        protected override bool MouseLeaveUnsharesRow(int rowIndex) {
            return _pressedItemIndex != -1 && !_mouseUpHooked;
        }

        /// <summary>
        /// Returns whether calling the OnMouseUp method would force the owning row to be unshared.
        /// </summary>
        protected override bool MouseUpUnsharesRow(DataGridViewCellMouseEventArgs e) {
            return e.Button == MouseButtons.Left && _pressedItemIndex != -1;
        }

        /// <summary>
        /// Method that declares the cell dirty and notifies the grid of the value change.
        /// </summary>
        private void NotifyDataGridViewOfValueChange() {
            _valueChanged = true;
            Debug.Assert(DataGridView != null);
            this.DataGridView.NotifyCurrentCellDirty(true);
        }

        /// <summary>
        /// Potentially updates the selected item and notifies the grid of the change.
        /// </summary>
        protected override void OnContentClick(DataGridViewCellEventArgs e) {
            if (DataGridView == null) {
                return;
            }
            Point ptCurrentCell = DataGridView.CurrentCellAddress;
            if (ptCurrentCell.X == ColumnIndex && ptCurrentCell.Y == e.RowIndex && DataGridView.IsCurrentCellInEditMode) {
                if (mouseLocationCode >= 0 && UpdateFormattedValue(_layout.FirstDisplayedItemIndex + mouseLocationCode, e.RowIndex)) {
                    NotifyDataGridViewOfValueChange();
                }
            }
        }

        /// <summary>
        /// Updates the property descriptors when the cell gets attached to the grid.
        /// </summary>
        protected override void OnDataGridViewChanged() {
            if (DataGridView != null) {
                // Will throw an error if DataGridView is set and a member is invalid
                InitializeDisplayMemberPropertyDescriptor(this.DisplayMember);
                InitializeValueMemberPropertyDescriptor(this.ValueMember);
            }
            base.OnDataGridViewChanged();
        }

        /// <summary>
        /// Makes sure that there is a focused item when the cell becomes the current one.
        /// </summary>
        protected override void OnEnter(int rowIndex, bool throughMouseClick) {
            if (_focusedItemIndex == -1) {
                _focusedItemIndex = _layout.FirstDisplayedItemIndex;
            }
            base.OnEnter(rowIndex, throughMouseClick);
        }

        /// <summary>
        /// Handles the KeyDown notification when it can result in a value change.
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e, int rowIndex) {
            if (this.DataGridView == null) {
                return;
            }
            if (!e.Alt && !e.Control && !e.Shift) {
                if (_handledKeyDown) {
                    _handledKeyDown = false;
                }
                if (e.KeyCode == Keys.Down && _focusedItemIndex < this.Items.Count - 1) {
                    _handledKeyDown = true;
                    e.Handled = true;
                } else if (e.KeyCode == Keys.Up && _focusedItemIndex > 0) {
                    _handledKeyDown = true;
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Handles the KeyUp notification to change the cell's value.
        /// </summary>
        protected override void OnKeyUp(KeyEventArgs e, int rowIndex) {
            if (this.DataGridView == null) {
                return;
            }
            if (!e.Alt && !e.Control && !e.Shift && _handledKeyDown) {
                if (e.KeyCode == Keys.Down && _focusedItemIndex < this.Items.Count - 1) {
                    // Handle the Down Arrow key
                    if (UpdateFormattedValue(_focusedItemIndex + 1, rowIndex)) {
                        NotifyDataGridViewOfValueChange();
                    } else if (_selectedItemIndex == _focusedItemIndex + 1) {
                        _focusedItemIndex++;
                    }
                    if (_focusedItemIndex >= _layout.FirstDisplayedItemIndex + _layout.TotallyDisplayedItemsCount) {
                        _layout.FirstDisplayedItemIndex++;
                    }
                    while (_focusedItemIndex < _layout.FirstDisplayedItemIndex) {
                        _layout.FirstDisplayedItemIndex--;
                    }
                    this.DataGridView.InvalidateCell(this.ColumnIndex, rowIndex);
                    e.Handled = true;
                } else if (e.KeyCode == Keys.Up && _focusedItemIndex > 0) {
                    // Handle the Up Arrow key
                    if (UpdateFormattedValue(_focusedItemIndex - 1, rowIndex)) {
                        NotifyDataGridViewOfValueChange();
                    } else if (_selectedItemIndex == _focusedItemIndex - 1) {
                        _focusedItemIndex--;
                    }
                    if (_focusedItemIndex < _layout.FirstDisplayedItemIndex) {
                        _layout.FirstDisplayedItemIndex--;
                    }
                    while (_focusedItemIndex >= _layout.FirstDisplayedItemIndex + _layout.TotallyDisplayedItemsCount) {
                        _layout.FirstDisplayedItemIndex++;
                    }
                    this.DataGridView.InvalidateCell(this.ColumnIndex, rowIndex);
                    e.Handled = true;
                }
            }
            // Always reset the flag that indicates if the KeyDown was handled.
            if (_handledKeyDown) {
                _handledKeyDown = false;
            }
        }

        /// <summary>
        /// Custom implementation of the MouseDown notification to update the cell's value or scroll the entries.
        /// </summary>
        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e) {
            if (this.DataGridView == null)
                return;

            if (e.Button == MouseButtons.Left) {
                var cellStyle = GetInheritedStyle(null, e.RowIndex, includeColors: false);
                int hit = GetMouseLocationCode(DataGridView.CreateGraphics(), e.RowIndex, cellStyle, e.X, e.Y);
                switch (hit) {
                case MouseLocationGeneric:
                    break;
                case MouseLocationBottomScrollButton:
                    if (_layout.FirstDisplayedItemIndex + _layout.TotallyDisplayedItemsCount < Items.Count) {
                        // Scroll the entries down.
                        _layout.FirstDisplayedItemIndex++;
                        DataGridView.Invalidate(new Rectangle(_layout.DownButtonLocation, _layout.ScrollButtonsSize));
                    }
                    break;
                case MouseLocationTopScrollButton:
                    if (_layout.FirstDisplayedItemIndex > 0) {
                        // Scroll the entries up.
                        _layout.FirstDisplayedItemIndex--;
                        DataGridView.Invalidate(new Rectangle(_layout.UpButtonLocation, _layout.ScrollButtonsSize));
                    }
                    break;
                default:
                    if (_pressedItemIndex != hit + _layout.FirstDisplayedItemIndex) {
                        // Update the value of the cell.
                        InvalidateItem(_pressedItemIndex, e.RowIndex);
                        _pressedItemIndex = hit + _layout.FirstDisplayedItemIndex;
                        InvalidateItem(_pressedItemIndex, e.RowIndex);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Makes sure the radio button gets hot when the mouse gets over it
        /// </summary>
        protected override void OnMouseEnter(int rowIndex) {
            if (DataGridView == null)
                return;

            if (_pressedItemIndex != -1) {
                InvalidateRadioGlyph(_pressedItemIndex, GetInheritedStyle(null, rowIndex, false /* includeColors */));
            }
        }

        /// <summary>
        /// Invalidates part of the cell as needed
        /// </summary>
        protected override void OnMouseLeave(int rowIndex) {
            if (DataGridView == null)
                return;

            int oldMouseLocationCode = mouseLocationCode;
            if (oldMouseLocationCode != MouseLocationGeneric) {
                mouseLocationCode = MouseLocationGeneric;
                if (oldMouseLocationCode == MouseLocationTopScrollButton && _layout.FirstDisplayedItemIndex > 0) {
                    DataGridView.Invalidate(new Rectangle(_layout.UpButtonLocation, _layout.ScrollButtonsSize));
                } else if (oldMouseLocationCode == MouseLocationBottomScrollButton && _layout.FirstDisplayedItemIndex + _layout.DisplayedItemsCount < this.Items.Count) {
                    DataGridView.Invalidate(new Rectangle(_layout.DownButtonLocation, _layout.ScrollButtonsSize));
                } else if (oldMouseLocationCode >= 0) {
                    InvalidateRadioGlyph(oldMouseLocationCode + _layout.FirstDisplayedItemIndex, GetInheritedStyle(null, rowIndex, false /* includeColors */));
                }
            }

            if (_pressedItemIndex != -1) {
                if (!_mouseUpHooked) {
                    // Hookup the grid's MouseUp event so that this.pressedItemIndex can be reset when the user releases the mouse button.
                    DataGridView.MouseUp += DataGridView_MouseUp;
                    _mouseUpHooked = true;
                }
                InvalidateRadioGlyph(_pressedItemIndex, GetInheritedStyle(null, rowIndex, false /* includeColors */));
            }
        }

        /// <summary>
        /// Invalidates part of the cell as needed
        /// </summary>
        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e) {
            if (DataGridView == null)
                return;

            var cellStyle = GetInheritedStyle(null, e.RowIndex, false /* includeColors */);
            int oldMouseLocationCode = mouseLocationCode;
            mouseLocationCode = GetMouseLocationCode(DataGridView.CreateGraphics(), e.RowIndex, cellStyle, e.X, e.Y);
            if (oldMouseLocationCode != mouseLocationCode) {
                if ((oldMouseLocationCode == MouseLocationTopScrollButton || mouseLocationCode == MouseLocationTopScrollButton) && _layout.FirstDisplayedItemIndex > 0) {
                    DataGridView.Invalidate(new Rectangle(_layout.UpButtonLocation, _layout.ScrollButtonsSize));
                } else if ((oldMouseLocationCode == MouseLocationBottomScrollButton || mouseLocationCode == MouseLocationBottomScrollButton) && _layout.FirstDisplayedItemIndex + _layout.DisplayedItemsCount < this.Items.Count) {
                    DataGridView.Invalidate(new Rectangle(_layout.DownButtonLocation, _layout.ScrollButtonsSize));
                } else {
                    if ((DataGridView.GetInheritedState(e.ColumnIndex, e.RowIndex) & DataGridViewElementStates.ReadOnly) != 0) {
                        return;
                    }
                    if (oldMouseLocationCode >= 0) {
                        InvalidateRadioGlyph(oldMouseLocationCode + _layout.FirstDisplayedItemIndex, cellStyle);
                    }
                    if (mouseLocationCode >= 0) {
                        InvalidateRadioGlyph(mouseLocationCode + _layout.FirstDisplayedItemIndex, cellStyle);
                    }
                }
            }
        }

        /// <summary>
        /// Invalidates the potential pressed radio button. 
        /// </summary>
        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e) {
            if (DataGridView == null)
                return;

            if (e.Button == MouseButtons.Left && _pressedItemIndex != -1) {
                InvalidateItem(_pressedItemIndex, e.RowIndex);
                _pressedItemIndex = -1;
            }
        }

        /// <summary>
        /// Paints the entire cell.
        /// </summary>
        protected override void Paint(Graphics graphics,
                                      Rectangle clipBounds,
                                      Rectangle cellBounds,
                                      int rowIndex,
                                      DataGridViewElementStates cellState,
                                      object value,
                                      object formattedValue,
                                      string errorText,
                                      DataGridViewCellStyle cellStyle,
                                      DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                      DataGridViewPaintParts paintParts) {
            ComputeLayout(graphics,
                          clipBounds,
                          cellBounds,
                          rowIndex,
                          cellState,
                          formattedValue,
                          errorText,
                          cellStyle,
                          advancedBorderStyle,
                          paintParts,
                          paint: true);
        }

        /// <summary>
        /// Paints a single item.
        /// </summary>
        private void PaintItem(Graphics graphics,
                               Rectangle radiosBounds,
                               int rowIndex,
                               int itemIndex,
                               DataGridViewElementStates cellState,
                               DataGridViewCellStyle cellStyle,
                               bool itemReadOnly,
                               bool itemSelected,
                               bool mouseOverCell,
                               bool paintFocus) {
            object itemFormattedValue = GetFormattedValue(GetItemValue(Items[itemIndex]),
                                                          rowIndex,
                                                          ref cellStyle,
                                                          null /*valueTypeConverter*/,
                                                          null /*formattedValueTypeConverter*/,
                                                          DataGridViewDataErrorContexts.Display);
            string itemFormattedText = itemFormattedValue as string;
            if (String.IsNullOrEmpty(itemFormattedText))
                return;

            //Paint the glyph & caption
            Point glyphLocation = new Point(radiosBounds.Left + Margin, radiosBounds.Top + Margin);
            var flags = TextFormatFlags.Top | TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.NoPrefix;
            Rectangle textBounds = new Rectangle(radiosBounds.Left + 2 * Margin + _layout.RadioButtonsSize.Width, radiosBounds.Top + Margin, radiosBounds.Width - (2 * Margin + _layout.RadioButtonsSize.Width), cellStyle.Font.Height + 1 /*radiosBounds.Height - 2 * DATAGRIDVIEWRADIOBUTTONCELL_margin*/);
            int hit = mouseOverCell ? mouseLocationCode : MouseLocationGeneric;
            using (var clipRegion = graphics.Clip) {
                graphics.SetClip(radiosBounds);
                RadioButtonState radioButtonState;
                if (itemReadOnly) {
                    radioButtonState = RadioButtonState.UncheckedDisabled;
                } else if (mouseOverCell && _pressedItemIndex == itemIndex) {
                    radioButtonState = (hit + _layout.FirstDisplayedItemIndex == _pressedItemIndex)
                        ? RadioButtonState.UncheckedPressed
                        : RadioButtonState.UncheckedHot;
                } else if (hit + _layout.FirstDisplayedItemIndex == itemIndex && _pressedItemIndex == -1) {
                    radioButtonState = RadioButtonState.UncheckedHot;
                } else {
                    radioButtonState = RadioButtonState.UncheckedNormal;
                }
                if (itemSelected) {
                    // HACK: the status are in the same order, but with a offset of 4.
                    radioButtonState += 4;
                }

                // Note: The cell should only show the focus rectangle when this.DataGridView.ShowFocusCues is true. However that property is
                // protected and can't be accessed directly. A custom grid derived from DataGridView could expose this notion publicly.
                RadioButtonRenderer.DrawRadioButton(graphics,
                                                    glyphLocation,
                                                    textBounds,
                                                    "",
                                                    cellStyle.Font,
                                                    flags,
                                                    paintFocus && /* DataGridView.ShowFocusCues && */ DataGridView.Focused,
                                                    radioButtonState);

                var foreColor = (cellState & DataGridViewElementStates.Selected)!=0 ? cellStyle.SelectionForeColor : cellStyle.ForeColor;
                TextRenderer.DrawText(graphics, itemFormattedText, cellStyle.Font, textBounds, foreColor, flags);

                graphics.Clip = clipRegion;
            }
        }

        /// <summary>
        /// Helper function that indicates if a paintPart needs to be painted.
        /// </summary>
        private static bool PartPainted(DataGridViewPaintParts paintParts, DataGridViewPaintParts paintPart) {
            return (paintParts & paintPart) != 0;
        }

        /// <summary>
        /// Custom implementation that follows the standard representation of cell types.
        /// </summary>
        public override string ToString() {
            return "DataGridViewRadioButtonCell { ColumnIndex=" + ColumnIndex + ", RowIndex=" + RowIndex + " }";
        }

        /// <summary>
        /// Returns true if the provided item successfully became the selected item.
        /// </summary>
        private bool UpdateFormattedValue(int newSelectedItemIndex, int rowIndex) {
            if (FormattedValueType == null || newSelectedItemIndex == _selectedItemIndex)
                return false;

            var editingCell = (IDataGridViewEditingCell)this;
            Debug.Assert(newSelectedItemIndex >= 0);
            Debug.Assert(newSelectedItemIndex < this.Items.Count);
            object item = this.Items[newSelectedItemIndex];
            object displayValue = GetItemDisplayValue(item);
            if (FormattedValueType.IsAssignableFrom(displayValue.GetType())) {
                editingCell.EditingCellFormattedValue = displayValue;
                _focusedItemIndex = _selectedItemIndex;
                DataGridView.InvalidateCell(this.ColumnIndex, rowIndex);
            }
            return true;
        }
    }
}
