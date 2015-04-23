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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Microsoft.Win32;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(HeaderStrip), "HeaderStrip.png")]
    public partial class HeaderStrip : ToolStrip
    {
        #region Renderer class

        internal class LargeHeaderStripRenderer : ToolStripProfessionalRenderer
        {
            public LargeHeaderStripRenderer(ProfessionalColorTable colorTable)
                : base(colorTable) {
                this.RoundedEdges = false;
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e) {
                // Size to paint
                Rectangle bounds = new Rectangle(Point.Empty, e.ToolStrip.Size);

                // Make sure we need to do work
                if ((bounds.Width > 0) && (bounds.Height > 0)) {
                    Color start = Application.RenderWithVisualStyles ? this.ColorTable.OverflowButtonGradientMiddle : this.ColorTable.OverflowButtonGradientEnd;
                    Color end = this.ColorTable.OverflowButtonGradientEnd;
                    using (Brush b = new LinearGradientBrush(bounds, start, end, LinearGradientMode.Vertical)) {
                        e.Graphics.FillRectangle(b, bounds);
                    }
                }
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e) {
                Color color = e.TextColor;
                if (!e.Item.Selected || !e.Item.Pressed) {
                    e.TextColor = SystemColors.HighlightText;
                }
                base.OnRenderItemText(e);
                e.TextColor = color;
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
                // Size to paint
                Rectangle bounds = new Rectangle(Point.Empty, e.ToolStrip.Size);
                using (Pen pen = new Pen(this.ColorTable.OverflowButtonGradientEnd)) {
                    e.Graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
                }
            }

        }

        internal class SmallHeaderStripRenderer : ToolStripProfessionalRenderer
        {
            public SmallHeaderStripRenderer(ProfessionalColorTable colorTable)
                : base(colorTable) {
                this.RoundedEdges = false;
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e) {
                // Size to paint
                Rectangle bounds = new Rectangle(Point.Empty, e.ToolStrip.Size);

                // Make sure we need to do work
                if ((bounds.Width > 0) && (bounds.Height > 0)) {
                    Color start = this.ColorTable.MenuStripGradientEnd;
                    Color end = this.ColorTable.MenuStripGradientBegin;
                    using (Brush b = new LinearGradientBrush(bounds, start, end, LinearGradientMode.Vertical)) {
                        e.Graphics.FillRectangle(b, bounds);
                    }
                }
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
                // Size to paint
                Rectangle bounds = new Rectangle(Point.Empty, e.ToolStrip.Size);
                using (Pen pen = new Pen(this.ColorTable.MenuStripGradientBegin)) {
                    e.Graphics.DrawLine(pen, bounds.Left, bounds.Top, bounds.Right, bounds.Top);
                }
                using (Pen pen = new Pen(this.ColorTable.OverflowButtonGradientEnd)) {
                    e.Graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
                }
            }
        }

        internal class NormalHeaderStripRenderer : ToolStripProfessionalRenderer
        {
            public NormalHeaderStripRenderer(ProfessionalColorTable colorTable)
                : base(colorTable) {
                this.RoundedEdges = false;
            }
        }

        #endregion

        #region Private fields

        private HeaderAreaStyle _headerStyle = HeaderAreaStyle.Normal;

        #endregion

        #region Lifecycle

        public HeaderStrip() {
            this.SuspendLayout();

            this.AutoSize = true;
            this.Stretch = true;
            this.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.Margin = new Padding(0);

            // Set visualStyleRenderer - override background painting
            InitializeRenderer();

            // Setup Headers
            SystemEvents.UserPreferenceChanged += delegate(object sender, UserPreferenceChangedEventArgs e) {
                SetHeaderStyle();
            };
            SetHeaderStyle();

            this.ResumeLayout(false);
        }

        #endregion

        #region Properties

        [DefaultValue(HeaderAreaStyle.Normal)]
        public HeaderAreaStyle HeaderStyle {
            get { return _headerStyle; }
            set {
                // Save value
                if (_headerStyle != value) {
                    _headerStyle = value;

                    // Set Renderer & Header Style
                    InitializeRenderer();
                    SetHeaderStyle();
                }
            }
        }

        #endregion

        #region Private Implementation

        private void SetHeaderStyle() {
            ResetFont();

            // Compute the size
            this.Height = 6 + TextRenderer.MeasureText("I", this.Font).Height;
        }

        public override void ResetFont() {
            // Get system font
            Font font = SystemFonts.MenuFont;

            if (_headerStyle == HeaderAreaStyle.Large || _headerStyle == HeaderAreaStyle.Large) {
                this.Font = new Font("Arial", font.SizeInPoints + 3.75F, FontStyle.Bold);
            } else if (_headerStyle == HeaderAreaStyle.Small) {
                this.Font = font;
            } else {
                base.ResetFont();
            }
        }

        private void InitializeRenderer() {
            ToolStripManager.RendererChanged -= ToolStripManager_RendererChanged;

            ToolStripRenderer renderer = null;
            switch (renderMode) {
            case ToolStripRenderMode.ManagerRenderMode:
                renderer = ToolStripManager.Renderer;
                ToolStripManager.RendererChanged += ToolStripManager_RendererChanged;
                break;
            case ToolStripRenderMode.Professional:
                renderer = new ToolStripProfessionalRenderer();
                break;
            case ToolStripRenderMode.System:
                renderer = new ToolStripSystemRenderer();
                break;
            default:
                renderer = this.Renderer;
                break;
            }

            ToolStripProfessionalRenderer pr = renderer as ToolStripProfessionalRenderer;
            if (pr != null) {
                // Only swap out if we're setup to use a professional visualStyleRenderer
                if (_headerStyle == HeaderAreaStyle.Large)
                    renderer = new LargeHeaderStripRenderer(pr.ColorTable);
                else if (_headerStyle == HeaderAreaStyle.Small)
                    renderer = new SmallHeaderStripRenderer(pr.ColorTable);
                else
                    renderer = new NormalHeaderStripRenderer(pr.ColorTable);
            }
            this.Renderer = renderer;
        }

        void ToolStripManager_RendererChanged(object sender, EventArgs e) {
            InitializeRenderer();
        }

        [Category("Appearance")]
        [DefaultValue(ToolStripRenderMode.ManagerRenderMode)]
        public new ToolStripRenderMode RenderMode {
            get {
                return renderMode;
            }
            set {
                if (renderMode != value) {
                    renderMode = value;
                    base.RenderMode = value;
                    InitializeRenderer();
                }
            }
        }
        ToolStripRenderMode renderMode = ToolStripRenderMode.ManagerRenderMode;

        private bool ShouldSerializeRenderMode() {
            return ((this.RenderMode != ToolStripRenderMode.ManagerRenderMode)
                && (this.RenderMode != ToolStripRenderMode.Custom));
        }

        private void ResetRenderMode() {
            this.RenderMode = ToolStripRenderMode.ManagerRenderMode;
        }

        #endregion
    }

    #region HeaderAreaStyle

    public enum HeaderAreaStyle
    {
        Normal = 0,
        Small = 1,
        Large = 2,
    }

    #endregion
}
