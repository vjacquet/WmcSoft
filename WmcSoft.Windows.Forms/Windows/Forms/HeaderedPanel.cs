using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using System.ComponentModel.Design;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(HeaderedPanel), "HeaderedPanel.png")]
    [Designer(typeof(HeaderedPanelDesigner))]
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

    public class HeaderedPanelDesigner : ParentControlDesigner
    {
        HeaderedPanel _headerPanel;
        PlaceHolder _placeHolder;
        IDesignerHost designerHost;

        public override void Initialize(IComponent component) {
            base.Initialize(component);
            base.AutoResizeHandles = true;
            _headerPanel = component as HeaderedPanel;
            _placeHolder = _headerPanel.PlaceHolder;
            base.EnableDesignMode(_headerPanel.PlaceHolder, "PlaceHolder");
            this.designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
            //if (this.selectedPanel == null) {
            //    this.Selected = this.splitterPanel1;
            //}
            //this.splitContainer.MouseDown += new MouseEventHandler(this.OnSplitContainer);
            //this.splitContainer.DoubleClick += new EventHandler(this.OnSplitContainerDoubleClick);
            //ISelectionService service = (ISelectionService)this.GetService(typeof(ISelectionService));
            //if (service != null) {
            //    service.SelectionChanged += new EventHandler(this.OnSelectionChanged);
            //}
        }

        public override bool CanParent(Control control) {
            return false;
        }

        public override int NumberOfInternalControlDesigners() {
            return 1;// return base.NumberOfInternalControlDesigners();
        }

        public override ControlDesigner InternalControlDesigner(int internalControlIndex) {
            switch (internalControlIndex) {
            case 0:
                return (ControlDesigner)designerHost.GetDesigner(_placeHolder);
            }
            return base.InternalControlDesigner(internalControlIndex);
        }

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
