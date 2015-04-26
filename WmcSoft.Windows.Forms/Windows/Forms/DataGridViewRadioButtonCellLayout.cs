// DataGridViewNumericUpDownXXX have been taken from
// http://msdn.microsoft.com/en-us/library/aa730881(VS.80).aspx
// and then adapted.
using System;
using System.Drawing;

namespace WmcSoft.Windows.Forms
{
    /// <summary>
    /// Internal class that represents the layout of a DataGridViewRadioButtonCell cell.
    /// It tracks the first displayed item index, the number of displayed items, 
    /// scrolling information and various location/size information.
    /// </summary>
    internal class DataGridViewRadioButtonCellLayout
    {
        public DataGridViewRadioButtonCellLayout() {
        }

        /// <summary>
        /// Boundaries of the cell content defined as the radio buttons of the displayed items.
        /// </summary>
        public Rectangle ContentBounds { get; set; }

        /// <summary>
        /// Index of the first displayed item.
        /// </summary>
        public int FirstDisplayedItemIndex { get; set; }

        /// <summary>
        /// Number of displayed items (includes potential partially displayed one).
        /// </summary>
        public int DisplayedItemsCount { get; set; }

        /// <summary>
        /// Number of totally displayed items.
        /// </summary>
        public int TotallyDisplayedItemsCount { get; set; }

        /// <summary>
        /// Indicates whether the scroll buttons need to be shown or not.
        /// </summary>
        public bool ScrollingNeeded { get; set; }

        /// <summary>
        /// Location of the Down scroll button.
        /// </summary>
        public Point DownButtonLocation { get; set; }

        /// <summary>
        /// Location of the Up scroll button.
        /// </summary>
        public Point UpButtonLocation { get; set; }

        /// <summary>
        /// Location of the top most displayed item.
        /// </summary>
        public Point FirstDisplayedItemLocation { get; set; }

        /// <summary>
        /// Size of the scroll buttons.
        /// </summary>
        public Size ScrollButtonsSize { get; set; }

        /// <summary>
        /// Size of the radio button glyphs.
        /// </summary>
        public Size RadioButtonsSize { get; set; }

        /// <summary>
        /// Checks if an item is displayed.
        /// </summary>
        /// <param name="itemIndex">The index of the item.</param>
        /// <returns>true if the item is displayed; otherwise false.</returns>
        public bool IsItemDisplayed(int itemIndex) {
            return itemIndex >= FirstDisplayedItemIndex
                && itemIndex < FirstDisplayedItemIndex + DisplayedItemsCount;
        }
    }
}
