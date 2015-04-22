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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;

namespace WmcSoft.Windows.Forms
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ContextMenuStrip | ToolStripItemDesignerAvailability.MenuStrip)]
    [System.ComponentModel.DesignerCategory("Code")]
    public class ToolStripRadioButton : ToolStripMenuItem
        , IBindableComponent
    {
        #region Lifecycle

        void Initialize() {
            CheckOnClick = true;
        }

        public ToolStripRadioButton()
            : base() {
            Initialize();
        }

        public ToolStripRadioButton(string text)
            : base(text, null, (EventHandler)null) {
            Initialize();
        }

        public ToolStripRadioButton(Image image)
            : base(null, image, (EventHandler)null) {
            Initialize();
        }

        public ToolStripRadioButton(string text, Image image)
            : base(text, image, (EventHandler)null) {
            Initialize();
        }

        public ToolStripRadioButton(string text, Image image,
            EventHandler onClick)
            : base(text, image, onClick) {
            Initialize();
        }

        public ToolStripRadioButton(string text, Image image,
            EventHandler onClick, string name)
            : base(text, image, onClick, name) {
            Initialize();
        }

        public ToolStripRadioButton(string text, Image image,
            params ToolStripItem[] dropDownItems)
            : base(text, image, dropDownItems) {
            Initialize();
        }

        public ToolStripRadioButton(string text, Image image,
            EventHandler onClick, Keys shortcutKeys)
            : base(text, image, onClick) {
            Initialize();
            this.ShortcutKeys = shortcutKeys;
        }

        #endregion

        #region Overrides

        protected override void OnCheckedChanged(EventArgs e) {
            base.OnCheckedChanged(e);

            // If this item is no longer in the checked state, do nothing.
            if (!Checked)
                return;

            // Clear the checked state for all siblings. 
            foreach (var item in Parent.Items.OfType<ToolStripRadioButton>()) {
                if (item != this && item.Checked) {
                    item.Checked = false;

                    // Only one item can be selected at a time, 
                    // so there is no need to continue.
                    return;
                }
            }
        }

        protected override void OnClick(EventArgs e) {
            // If the item is already in the checked state, do not call 
            // the base method, which would toggle the value. 
            if (Checked)
                return;

            base.OnClick(e);
        }

        // Let the item paint itself, and then paint the RadioButton
        // where the check mark is displayed, covering the check mark
        // if it is present.
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            // If the client sets the Image property, the selection behavior
            // remains unchanged, but the RadioButton is not displayed and the
            // selection is indicated only by the selection rectangle. 
            if (Image != null)
                return;

            // Determine the correct state of the RadioButton.
            var buttonState = RadioButtonState.UncheckedNormal;
            if (Enabled) {
                if (mouseDownState) {
                    if (Checked)
                        buttonState = RadioButtonState.CheckedPressed;
                    else
                        buttonState = RadioButtonState.UncheckedPressed;
                } else if (mouseHoverState) {
                    if (Checked)
                        buttonState = RadioButtonState.CheckedHot;
                    else
                        buttonState = RadioButtonState.UncheckedHot;
                } else {
                    if (Checked)
                        buttonState = RadioButtonState.CheckedNormal;
                }
            } else {
                if (Checked)
                    buttonState = RadioButtonState.CheckedDisabled;
                else
                    buttonState = RadioButtonState.UncheckedDisabled;
            }

            // Calculate the position at which to display the RadioButton.
            var glyphSize = RadioButtonRenderer.GetGlyphSize(e.Graphics, buttonState);
            var offset = (ContentRectangle.Height - glyphSize.Height) / 2;
            var imageLocation = ContentRectangle.Location;
            imageLocation.Offset(4, offset);
            // If the item is selected and the RadioButton paints with partial
            // transparency, such as when theming is enabled, the check mark
            // shows through the RadioButton image. In this case, paint a 
            // non-transparent background first to cover the check mark.
            if (Checked && RadioButtonRenderer.IsBackgroundPartiallyTransparent(buttonState)) {
                glyphSize.Height--;
                glyphSize.Width--;
                Rectangle backgroundRectangle = new Rectangle(imageLocation, glyphSize);
                e.Graphics.FillEllipse(SystemBrushes.Control, backgroundRectangle);
            }

            RadioButtonRenderer.DrawRadioButton(e.Graphics, imageLocation, buttonState);
        }

        private bool mouseHoverState = false;

        protected override void OnMouseEnter(EventArgs e) {
            mouseHoverState = true;

            // Force the item to repaint with the new RadioButton state.
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            mouseHoverState = false;
            base.OnMouseLeave(e);
        }
        private bool mouseDownState = false;

        protected override void OnMouseDown(MouseEventArgs e) {
            mouseDownState = true;
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            mouseDownState = false;
            base.OnMouseUp(e);
        }

        // Enable the item only if its parent item is in the checked state 
        // and its Enabled property has not been explicitly set to false. 
        public override bool Enabled {
            get {
                // Use the base value in design mode to prevent the designer
                // from setting the base value to the calculated value.
                if (!DesignMode
                    && ownerMenuItem != null
                    && ownerMenuItem.CheckOnClick) {
                    return base.Enabled && ownerMenuItem.Checked;
                } else {
                    return base.Enabled;
                }
            }
            set {
                base.Enabled = value;
            }
        }

        // When OwnerItem becomes available, if it is a ToolStripMenuItem 
        // with a CheckOnClick property value of true, subscribe to its 
        // CheckedChanged event. 
        protected override void OnOwnerChanged(EventArgs e) {
            if (ownerMenuItem != null) {
                ownerMenuItem.CheckedChanged -= OwnerMenuItem_CheckedChanged;
            }
            ownerMenuItem = OwnerItem as ToolStripMenuItem;
            if (ownerMenuItem != null && ownerMenuItem.CheckOnClick) {
                ownerMenuItem.CheckedChanged += OwnerMenuItem_CheckedChanged;
            }
            base.OnOwnerChanged(e);
        }
        ToolStripMenuItem ownerMenuItem;

        // When the checked state of the parent item changes, 
        // repaint the item so that the new Enabled state is displayed. 
        private void OwnerMenuItem_CheckedChanged(object sender, EventArgs e) {
            Invalidate();
        }

        #endregion

        #region IBindableComponent Members

        private BindingContext bindingContext;
        private ControlBindingsCollection dataBindings;

        [Browsable(false)]
        public BindingContext BindingContext {
            get {
                if (bindingContext == null) {
                    bindingContext = new BindingContext();
                }
                return bindingContext;
            }
            set {
                bindingContext = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlBindingsCollection DataBindings {
            get {
                if (dataBindings == null) {
                    dataBindings = new ControlBindingsCollection(this);
                }
                return dataBindings;
            }
        }

        #endregion
    }
}

