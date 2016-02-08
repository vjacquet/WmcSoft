#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(HorizontalFormLayoutPanel), "HorizontalFormLayoutPanel.bmp")]
    [ProvideProperty("Caption", typeof(Control))]
    [Docking(DockingBehavior.Ask)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    public partial class HorizontalFormLayoutPanel : Panel, IExtenderProvider
    {
        internal readonly HorizontalFormLayoutSettings LayoutSettings;

        public HorizontalFormLayoutPanel() {
            MinLabelWidth = 50;

            LayoutSettings = new HorizontalFormLayoutSettings();
            //SetStyle(ControlStyles.ResizeRedraw, true);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the container enables the user to scroll to any controls placed outside of its visible boundaries.
        /// </summary>
        /// <returns>
        /// true if the container enables auto-scrolling; otherwise, false. The default value is false. 
        /// </returns>
        public override bool AutoScroll
        {
            get { return true; }
            set { }
        }

        bool IExtenderProvider.CanExtend(object extendee) {
            var control = extendee as Control;
            return control != null && control.Parent == this;
        }

        [DisplayName("Caption")]
        [Category("Appearance")]
        public string GetCaption(Control control) {
            return LayoutSettings.GetCaption(control);
        }

        [DisplayName("Caption")]
        [Category("Appearance")]
        public void SetCaption(Control control, string value) {
            LayoutSettings.SetCaption(control, value);
        }

        [DefaultValue(50)]
        public int MinLabelWidth { get; set; }

        /// <summary>
        /// Gets a cached instance of the control's layout engine.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Windows.Forms.Layout.LayoutEngine"/> for the control's contents.
        /// </returns>
        public override LayoutEngine LayoutEngine
        {
            get {
                Debug.Assert(LayoutSettings.LayoutEngine != null, "LayoutSettings.LayoutEngine != null");
                return LayoutSettings.LayoutEngine;
            }
        }

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaintBackground(PaintEventArgs e) {
            base.OnPaintBackground(e);

            var displayRectangle = DisplayRectangle;
            var left = displayRectangle.X;
            foreach (var control in Controls.Where(c => c.Visible)) {
                var location = PointToClient(control.PointToScreen(control.ClientRectangle.Location));
                var caption = GetCaption(control);
                TextRenderer.DrawText(e.Graphics, caption, Font, new Point(left, location.Y + 1), ForeColor);
            }
        }
    }
}
