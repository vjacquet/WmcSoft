using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;
using WmcSoft.ComponentModel.Design;
using WmcSoft.Windows.Forms.Design;
using WmcSoft.Windows.Forms.Layout;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(PlaceHolder), "DeckLayoutPanel.bmp")]
    [Designer(typeof(DeckLayoutPanelDesigner))]
    [Docking(DockingBehavior.Ask)]
    public sealed class DeckLayoutPanel : Panel, IContainerControl
    {
        #region Private fields

        #endregion

        #region Lifecycle

        public DeckLayoutPanel() {
            base.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        #endregion

        #region Properties

        public override LayoutEngine LayoutEngine {
            get { return DeckLayoutEngine.Instance; }
        }

        #endregion

        #region IContainerControl Membres

        public bool ActivateControl(Control active) {
            if (active == null)
                throw new ArgumentNullException("active");
            if (_activeControl != active) {
                if (!Controls.Contains(active))
                    return false;

                Control inactive = _activeControl;
                _activeControl = active;
                if (inactive != null) {
                    OnControlDeactivated(new ControlEventArgs(inactive));
                }
                OnControlActivated(new ControlEventArgs(active));
                LayoutEngine.Layout(this, new LayoutEventArgs(active, null));
            }
            return true;
        }

        protected override void OnControlAdded(ControlEventArgs e) {
            base.OnControlAdded(e);
            if (_activeControl == null) {
                ActivateControl(e.Control);
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e) {
            if (_activeControl == e.Control) {
                _activeControl = null;
            }
            base.OnControlRemoved(e);
            if (Controls.Count > 0) {
                _activeControl = Controls[0];
            }
        }

        [TypeConverter(typeof(ChildControlConverter))]
        public Control ActiveControl {
            get { return _activeControl; }
            set { ActivateControl(value); }
        }
        Control _activeControl;

        public event ControlEventHandler ControlActivated {
            add { Events.AddHandler(ControlActivatedEvent, value); }
            remove { Events.RemoveHandler(ControlActivatedEvent, value); }
        }
        static object ControlActivatedEvent = new Object();

        void OnControlActivated(ControlEventArgs e) {
            var handler = Events[ControlActivatedEvent] as ControlEventHandler;
            if (handler != null) {
                handler(this, e);
            }
        }

        public event ControlEventHandler ControlDeactivated {
            add { Events.AddHandler(ControlDeactivatedEvent, value); }
            remove { Events.RemoveHandler(ControlDeactivatedEvent, value); }
        }
        static object ControlDeactivatedEvent = new Object();

        void OnControlDeactivated(ControlEventArgs e) {
            var handler = Events[ControlDeactivatedEvent] as ControlEventHandler;
            if (handler != null) {
                handler(this, e);
            }
        }

        #endregion
    }
}

namespace WmcSoft.Windows.Forms.Design
{
    public class DeckLayoutPanelDesigner : ScrollableControlDesigner
    {
        private class ActiveControlActionList : DesignerActionList
        {
            private DeckLayoutPanelDesigner _owner;

            //[AttributeProvider(typeof(IListSource))]
            [TypeConverter(typeof(ChildControlConverter))]
            public Control ActiveControl {
                get {
                    return _owner.Deck.ActiveControl;
                }
                set {
                    var deck = _owner.Deck;
                    var designerHost = _owner.Component.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
                    var member = TypeDescriptor.GetProperties(deck)["ActiveControl"];
                    var componentChangeService = _owner.Component.Site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    using (var designerTransaction = designerHost.CreateTransaction("Activate control on {0}.", deck.Name)) {
                        componentChangeService.OnComponentChanging(_owner.Component, member);
                        deck.ActivateControl((Control)value);
                        componentChangeService.OnComponentChanged(_owner.Component, member, null, null);
                        designerTransaction.Commit();
                    }
                }
            }
            public ActiveControlActionList(DeckLayoutPanelDesigner owner)
                : base(owner.Component) {
                _owner = owner;
            }

            public override DesignerActionItemCollection GetSortedActionItems() {
                return new DesignerActionItemCollection {
                    new DesignerActionPropertyItem("ActiveControl", "Active control")
                    {
                        RelatedComponent = _owner.Component
                    }
                };
            }
        }

        #region Fields

        private IDesignerHost _designerHost;
        private bool _selected;
        private DeckLayoutPanel _panel;

        private DisposableStack _disposables;

        #endregion

        #region Methods

        public override DesignerActionListCollection ActionLists {
            get {
                if (_actionLists == null) {
                    _actionLists = new DesignerActionListCollection();
                    _actionLists.Add(new ActiveControlActionList(this));
                }
                return _actionLists;
            }
        }
        DesignerActionListCollection _actionLists;

        protected virtual void DrawBorder(Graphics graphics) {
            var control = Control;
            if ((control != null) && control.Visible) {
                var clientRectangle = Control.ClientRectangle;
                clientRectangle.Inflate(-1, -1);
                using (var pen = CreateBorderPen()) {
                    graphics.DrawRectangle(pen, clientRectangle);
                }
            }
        }

        public override bool CanBeParentedTo(IDesigner parentDesigner) {
            return (parentDesigner is ControlDesigner);
        }

        protected override void Dispose(bool disposing) {
            if (_disposables != null)
                _disposables.Dispose();
            base.Dispose(disposing);
        }

        protected internal void DrawSelectedBorder() {
            var control = Control;
            var clientRectangle = control.ClientRectangle;
            clientRectangle.Inflate(-4, -4);
            using (var graphics = control.CreateGraphics())
            using (var pen = CreateBorderPen()) {
                pen.DashStyle = DashStyle.Dash;
                graphics.DrawRectangle(pen, clientRectangle);
            }
        }

        protected internal void DrawWaterMark(Graphics g) {
            var control = Control;
            var clientRectangle = control.ClientRectangle;
            string name = control.Name;
            using (var font = new Font("Arial", 8f)) {
                int x = (clientRectangle.Width / 2) - (((int)g.MeasureString(name, font).Width) / 2);
                int y = clientRectangle.Height / 2;
                TextRenderer.DrawText(g, name, font, new Point(x, y), Color.Black, TextFormatFlags.GlyphOverhangPadding);
            }
        }

        protected internal void EraseBorder() {
            var control = Control;
            var clientRectangle = control.ClientRectangle;
            clientRectangle.Inflate(-4, -4);
            using (var graphics = control.CreateGraphics())
            using (var pen = new Pen(control.BackColor)) {
                graphics.DrawRectangle(pen, clientRectangle);
            }
            control.Invalidate();
        }

        public override void Initialize(IComponent component) {
            base.Initialize(component);
            _panel = (DeckLayoutPanel)component;
            _designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));

            _disposables = new DisposableStack();
            var componentChangeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            if (componentChangeService != null) {
                componentChangeService.ComponentChanged += OnComponentChanged;
                _disposables.Push(() => componentChangeService.ComponentChanged -= OnComponentChanged);
            }

            var selectionService = (ISelectionService)GetService(typeof(ISelectionService));
            if (selectionService != null) {
                selectionService.SelectionChanged += OnSelectionChanged;
                _disposables.Push(() => selectionService.SelectionChanged -= OnSelectionChanged);
            }
        }

        void OnSelectionChanged(object sender, EventArgs e) {
            var selectionService = (ISelectionService)GetService(typeof(ISelectionService));
            if (selectionService != null) {
                var selected = false;
                var active = selectionService.PrimarySelection as Control;
                if (active != null) {
                    if (active == _panel) {
                        selected = true;
                    } else if (active.Parent == _panel) {
                        _panel.ActivateControl(active);
                    }
                }
                if (selected != _selected) {
                    _selected = selected;
                    _panel.Invalidate();
                }
            }
        }

        private void OnComponentChanged(object sender, ComponentChangedEventArgs e) {
            if (_panel.Parent == null)
                return;

            if (_panel.Controls.Count == 0) {
                using (Graphics g = _panel.CreateGraphics()) {
                    DrawWaterMark(g);
                }
            } else {
                _panel.Invalidate();
            }
        }

        protected override void OnDragDrop(DragEventArgs de) {
            if (InheritanceAttribute == InheritanceAttribute.InheritedReadOnly) {
                de.Effect = DragDropEffects.None;
            } else {
                base.OnDragDrop(de);
            }
        }

        protected override void OnDragEnter(DragEventArgs de) {
            if (InheritanceAttribute == InheritanceAttribute.InheritedReadOnly) {
                de.Effect = DragDropEffects.None;
            } else {
                base.OnDragEnter(de);
            }
        }

        protected override void OnDragLeave(EventArgs e) {
            if (InheritanceAttribute != InheritanceAttribute.InheritedReadOnly) {
                base.OnDragLeave(e);
            }
        }

        protected override void OnDragOver(DragEventArgs de) {
            if (InheritanceAttribute == InheritanceAttribute.InheritedReadOnly) {
                de.Effect = DragDropEffects.None;
            } else {
                base.OnDragOver(de);
            }
        }

        protected override void OnPaintAdornments(PaintEventArgs pe) {
            if (_panel.BorderStyle == BorderStyle.None) {
                this.DrawBorder(pe.Graphics);
            }
            if (this.Selected) {
                this.DrawSelectedBorder();
            }
            if (_panel.Controls.Count == 0) {
                this.DrawWaterMark(pe.Graphics);
            }
        }

        #endregion

        #region Properties

        public DeckLayoutPanel Deck {
            get {
                return _panel;
            }
        }

        protected Pen CreateBorderPen(Color backColor) {
            var color = (backColor.GetBrightness() < 0.5) ? ControlPaint.Light(backColor) : ControlPaint.Dark(backColor);
            var pen = new Pen(color);
            pen.DashStyle = DashStyle.Dash;
            return pen;
        }
        protected Pen CreateBorderPen() {
            return CreateBorderPen(Control.BackColor);
        }

        protected internal bool Selected {
            get {
                return _selected;
            }
            set {
                _selected = value;
                if (_selected) {
                    this.DrawSelectedBorder();
                } else {
                    this.EraseBorder();
                }
            }
        }

        #endregion
    }

    public class ChildControlConverter : ReferenceConverter
    {
        public ChildControlConverter()
            : base(typeof(Control)) {

        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            var arrayList = new ArrayList();
            var control = context.Instance as Control;
            if (control == null) {
                var action = context.Instance as DesignerActionList;
                if (action != null)
                    control = action.Component as Control;
            }
            if (control != null) {
                arrayList.AddRange(control.Controls.OfType<IComponent>().ToArray());
            }
            return new TypeConverter.StandardValuesCollection(arrayList);
        }
    }
}

namespace WmcSoft.Windows.Forms.Layout
{
    internal class DeckLayoutEngine : LayoutEngine
    {
        internal static readonly DeckLayoutEngine Instance = new DeckLayoutEngine();

        public override bool Layout(object container, LayoutEventArgs layoutEventArgs) {
            var parent = container as Control;
            var containerControl = container as IContainerControl;
            if (containerControl != null) {
                Control active = containerControl.ActiveControl;

                foreach (Control control in parent.Controls) {
                    if (control == active) {
                        control.Visible = true;
                        control.Location = new Point(0, 0);
                        control.Size = parent.Size;
                    } else {
                        control.Visible = false;
                    }
                }
            }
            return false;
        }
    }
}