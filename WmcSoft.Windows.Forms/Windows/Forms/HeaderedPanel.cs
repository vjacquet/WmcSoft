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

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(HeaderedPanel), "HeaderedPanel.png")]
    [Designer(typeof(Design.HeaderedPanelDesigner))]
    public partial class HeaderedPanel : UserControl
    {
        PlaceHolder placeHolder;

        public HeaderedPanel() {
            placeHolder = new PlaceHolder(this);
            InitializeComponent();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Appearance")]
        [Localizable(false)]
        public PlaceHolder PlaceHolder {
            get { return this.placeHolder; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Appearance")]
        public HeaderStrip HeaderStrip {
            get { return this.headerStrip; }
        }
    }
}

namespace WmcSoft.Windows.Forms.Design
{
    public class HeaderedPanelDesigner : ParentControlDesigner
    {
        HeaderedPanel _headerPanel;
        HeaderStrip _headerStrip;
        PlaceHolder _placeHolder;
        IDesignerHost designerHost;

        public override void Initialize(IComponent component) {
            base.Initialize(component);
            base.AutoResizeHandles = true;
            _headerPanel = component as HeaderedPanel;
            _headerStrip = _headerPanel.HeaderStrip;
            _placeHolder = _headerPanel.PlaceHolder;
            base.EnableDesignMode(_headerPanel.PlaceHolder, "PlaceHolder");
            base.EnableDesignMode(_headerPanel.HeaderStrip, "HeaderStrip");
            this.designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
        }

        public override bool CanParent(Control control) {
            return false;
        }

        //public override int NumberOfInternalControlDesigners() {
        //    return 2;// return base.NumberOfInternalControlDesigners();
        //}

        //public override ControlDesigner InternalControlDesigner(int internalControlIndex) {
        //    switch (internalControlIndex) {
        //    case 0:
        //        return (ControlDesigner)designerHost.GetDesigner(_placeHolder);
        //    case 1:
        //        return (ControlDesigner)designerHost.GetDesigner(_headerStrip);
        //    }
        //    return base.InternalControlDesigner(internalControlIndex);
        //}

        protected override void OnDragEnter(DragEventArgs de) {
            de.Effect = DragDropEffects.None;
        }

        protected override void OnPaintAdornments(PaintEventArgs pe) {
            try {
                _disableDrawGrid = true;
                base.OnPaintAdornments(pe);
            }
            finally {
                _disableDrawGrid = false;
            }
        }
        bool _disableDrawGrid;

        protected override bool DrawGrid {
            get {
                if (_disableDrawGrid) {
                    return false;
                }
                return base.DrawGrid;
            }
        }
    }
}
