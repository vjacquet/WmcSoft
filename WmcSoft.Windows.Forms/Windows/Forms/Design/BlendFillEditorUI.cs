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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WmcSoft.Windows.Forms.Design
{
    /// <summary>
    ///   This is the design time UITypeEditor for the BlendFill class.
    /// </summary>
    [ToolboxItem(false)]
    internal partial class BlendFillEditorUI : UserControl
    {
        #region Private Fields

        private readonly bool _reverse;
        private readonly IWindowsFormsEditorService _svc;

        private BlendStyle _direction;
        private Color _startColor;
        private Color _finishColor;

        #endregion

        #region Lifecycle

        public BlendFillEditorUI(IWindowsFormsEditorService svc, BlendFill blendFill, bool reverse)
            : base() {
            InitializeComponent();

            _svc = svc;

            _direction = blendFill.Style;
            _startColor = blendFill.StartColor;
            _finishColor = blendFill.FinishColor;
            _reverse = reverse;

            var rm = new System.Resources.ResourceManager(typeof(BlendFillEditorUI));
            PopulateDirectionListBox(rm);
            PopulateAndSelectColorList(startColorList, _startColor, rm);
            PopulateAndSelectColorList(finishColorList, _finishColor, rm);
        }

        #endregion

        ColorAndNameListItem GetSelectedColorItem(ListBox list) {
            return (ColorAndNameListItem)list.Items[list.SelectedIndex];
        }

        Color GetSelectedColor(ListBox list) {
            var item = list.Items[list.SelectedIndex] as ColorAndNameListItem;
            if (item != null)
                return item.Color;
            return Color.Empty;
        }

        /// <summary>
        ///   Returns what this editor currently has represented as a value.
        /// </summary>
        /// <returns>
        ///   A BlendFill object representing the current value of the editor.
        /// </returns>
        internal BlendFill GetValue() {
            var startColor = GetSelectedColor(startColorList);
            var finishColor = GetSelectedColor(finishColorList);
            var direction = (BlendStyle)directionComboBox.SelectedIndex;

            return new BlendFill(direction, startColor, finishColor);
        }

        private void PopulateDirectionListBox(ResourceManager resources) {
            // Please note that these keys match the values/order of BlendStyle
            // exactly !!!!
            var keys = new string[] { 
                "directionHorizontal",
                "directionVertical",
                "directionForwardDiagonal",
                "directionBackwardDiagonal"
            };

            for (int x = 0; x < keys.Length; x++) {
                var s = resources.GetString(keys[x]);
                Debug.Assert(!String.IsNullOrEmpty(s));
                directionComboBox.Items.Add(s);
            }

            directionComboBox.SelectedIndex = (int)_direction;
        }

        /// <summary>
        ///   Sets up an owner draw listbox to contain most of the interesting
        ///   colors currently available on the system.  It will then select
        ///   the given color.
        /// </summary>
        /// <param name="listBox">
        ///   The owner-draw ListBox to populate.
        /// </param>
        /// <param name="selectMe">
        ///   The Color to select.
        /// </param>
        /// <param name="resources">
        ///   From where to get localized strings.
        /// </param>
        private void PopulateAndSelectColorList(ListBox listBox, Color selectMe, ResourceManager resources) {
            // 1. Add SystemColors to list box.  Please note that we'rectangle
            //    going to pass in the color to select so that we can
            //    compare against the colors in their native format, and
            //    not have to go back and  regenerate the colors from
            //    the type strings, etc ...
            AddColorsToList(listBox, typeof(SystemColors), selectMe);

            // 2. Add Regular colors to the list box.
            AddColorsToList(listBox, typeof(Color), selectMe);

            // 3. If the user gave us a color that isn't one of the predefined
            //    system-y colors, then go and add a "Custom" entry for their
            //    color.
            if (listBox.SelectedIndex == -1) {
                var s = resources.GetString("customColor");
                Debug.Assert(!string.IsNullOrEmpty(s));

                var canli = new ColorAndNameListItem();
                canli.Color = selectMe;
                canli.Name = s;

                listBox.Items.Insert(0, canli);
                listBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        ///   Given an object with a tonne of static color objects, go and
        ///   get those and add them to the list of colors.  We will also
        ///   do an optimisation, and select the appropriate color if it's
        ///   in our list.
        /// </summary>
        /// <param name="listBox">
        ///   Where to add the colors.
        /// </param>
        /// <param name="colorSource">
        ///   From which class to get the colors.
        /// </param>
        /// <param name="selectMe">
        ///   Indicates which color to select when showing the list.
        /// </param>
        private void AddColorsToList(ListBox listBox, Type colorSource, Color selectMe) {
            // Loop through all the properties looking for Color properties.
            foreach (System.Reflection.PropertyInfo pi in colorSource.GetProperties()) {
                if (pi.PropertyType == typeof(Color)) {
                    var canli = new ColorAndNameListItem();
                    canli.Color = (Color)pi.GetValue(null, null);
                    canli.Name = pi.Name;

                    var i = listBox.Items.Add(canli);
                    if (selectMe.Equals(canli.Color)) {
                        listBox.SelectedIndex = i;
                    }
                }
            }
        }

        /// <summary>
        ///   We are supposed to draw an item in the list box now.
        /// </summary>
        /// <param name="sender">
        ///   From whence cometh the event.
        /// </param>
        /// <param name="e">
        ///   Details about it, including the Graphics object.
        /// </param>
        private void startColorList_DrawItem(object sender, DrawItemEventArgs e) {
            DrawItemForListBox(((ListBox)sender), e);
        }

        /// <summary>
        ///   We are supposed to draw an item in the list box now.
        /// </summary>
        /// <param name="sender">
        ///   From whence cometh the event.
        /// </param>
        /// <param name="e">
        ///   Details about it, including the Grahpics object.
        /// </param>
        private void finishColorList_DrawItem(object sender, DrawItemEventArgs e) {
            DrawItemForListBox(((ListBox)sender), e);
        }

        /// <summary>
        ///   Draws an item for one of the two color list boxes.  We're just
        ///   going to try and look as much like the regular Color UITypeEditor
        ///   as possible.
        /// </summary>
        /// <param name="listBox">
        ///   The ListBox being drawn.
        /// </param>
        /// <param name="args">
        ///   Detailsa bout the painting event.
        /// </param>
        private void DrawItemForListBox(ListBox listBox, DrawItemEventArgs args) {
            if (listBox == null) {
                Debug.Fail("DrawItem event for something that isn't a ListBox!");
                return;
            }

            // draw the background
            args.DrawBackground();

            // Get the ColorAndNameListItem for the details of what we're doing.
            var canli = (ColorAndNameListItem)listBox.Items[args.Index];
            if (canli == null) {
                Debug.Fail("A bogus item was inserted into the " + listBox.Name + " ListBox.");
                return;
            }

            var g = args.Graphics;
            var r = args.Bounds;

            // Now, go and draw the color in a little framed box.
            using (var b = new SolidBrush(canli.Color))
            using (var p = new Pen(Color.Black)) {
                g.FillRectangle(b, r.Left + 2, r.Top + 2, 22, listBox.ItemHeight - 4);
                g.DrawRectangle(p, r.Left + 2, r.Top + 2, 22, listBox.ItemHeight - 4);
            }

            // Finally, go and draw the font next to the color !
            var color = ((args.State & DrawItemState.Selected) != 0)
                ? listBox.BackColor
                : SystemColors.ControlText;
            using (var b = new SolidBrush(color)) {
                g.DrawString(canli.Name, listBox.Font, b, r.Left + 26, args.Bounds.Top + 2);
            }
        }

        /// <summary>
        ///   Paints a little preview of the blend operation for the user.
        /// </summary>
        /// <param name="sender">
        ///   From whence cometh the event.
        /// </param>
        /// <param name="e">
        ///   Deatils about the event, including the Graphics object.
        /// </param>
        private void blendSamplePanel_Paint(object sender, PaintEventArgs e) {
            var g = e.Graphics;

            // Draw the four sample rects.
            var rects = GetPanelRects();
            for (int x = 0; x < rects.Length; x++) {
                using (var lgb = new LinearGradientBrush(rects[x], _startColor, _finishColor, GetAngle((BlendStyle)x)))
                using (var p = new Pen(Color.Black, (x == (int)_direction) ? 3 : 1)) {
                    g.FillRectangle(lgb, rects[x]);
                    g.DrawRectangle(p, rects[x]);
                }
            }
        }

        /// <summary>
        ///   Returns an angle for a LinearGradientBrush given a direction/style.
        /// </summary>
        /// <param name="direction">
        ///   The style or direction for which we wish to query the angle.
        /// </param>
        /// <returns>
        ///   The angle to draw.
        /// </returns>
        private float GetAngle(BlendStyle direction) {
            switch (direction) {
            case BlendStyle.Horizontal:
                return _reverse ? 180 : 0;
            case BlendStyle.Vertical:
                return 90;
            case BlendStyle.ForwardDiagonal:
                return _reverse ? 135 : 45;
            case BlendStyle.BackwardDiagonal:
                return _reverse ? 45 : 135;
            default:
                Debug.Fail("Bogus direction");
                return 0;
            }
        }

        /// <summary>
        ///   Returns the rectangles in which to draw the samples.
        /// </summary>
        /// <returns>
        ///   An array of rectangles in which to draw the samples.
        /// </returns>
        /// 
        private Rectangle[] GetPanelRects() {
            var w = ((int)(blendSamplePanel.Width / 2)) - 8;
            var h = ((int)(blendSamplePanel.Height / 2)) - 8;
            return new Rectangle[] {
                new Rectangle(4, 4, w, h),
                new Rectangle(w + 8, 4, w, h),
                new Rectangle(4, h + 8, w, h),
                new Rectangle(w + 8, h + 8, w, h),
            };
        }

        /// <summary>
        ///   Update the sample panel with the new selection.
        /// </summary>
        private void directionComboBox_SelectedIndexChanged(object sender, System.EventArgs e) {
            _direction = (BlendStyle)this.directionComboBox.SelectedIndex;
            blendSamplePanel.Refresh();
        }

        /// <summary>
        ///   Update the sample box.
        /// </summary>
        private void finishColorList_SelectedIndexChanged(object sender, System.EventArgs e) {
            var canli = GetSelectedColorItem(finishColorList);
            if (canli != null) {
                _finishColor = canli.Color;
            }
            blendSamplePanel.Refresh();
        }

        /// <summary>
        ///   Update the sample box.
        /// </summary>
        private void startColorList_SelectedIndexChanged(object sender, System.EventArgs e) {
            var canli = GetSelectedColorItem(startColorList);
            if (canli != null) {
                _startColor = canli.Color;
            }
            blendSamplePanel.Refresh();
        }

        /// <summary>
        ///   They've clicked on the sample panel.  Update the selection if
        ///   necessary.
        /// </summary>
        private void blendSamplePanel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
            Rectangle[] rects = GetPanelRects();

            for (int x = 0; x < rects.Length; x++) {
                if (rects[x].Contains(e.X, e.Y)) {
                    _direction = (BlendStyle)x;
                    blendSamplePanel.Refresh();
                    directionComboBox.SelectedIndex = x;
                }
            }
        }

        /// <summary>
        ///   Close down the editor.
        /// </summary>
        private void RequestCloseDropDown(object sender, System.EventArgs e) {
            if (_svc != null) {
                _svc.CloseDropDown();
            }
        }

        /// <summary>
        /// Wraps a list item in our two color list boxes.
        /// </summary>
        private class ColorAndNameListItem
        {
            public Color Color;
            public string Name;

            public override string ToString() {
                return Name;
            }
        }
    }
}


