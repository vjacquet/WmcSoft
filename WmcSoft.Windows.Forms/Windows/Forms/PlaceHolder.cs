using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WmcSoft.Windows.Forms
{
    [Designer(typeof(Design.PlaceHolderDesigner)), ToolboxItem(false), Docking(DockingBehavior.Never)]
    public sealed class PlaceHolder : Panel
    {
        private bool collapsed;
        private Control owner;

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler AutoSizeChanged {
            add { base.AutoSizeChanged += value; }
            remove { base.AutoSizeChanged -= value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new event EventHandler DockChanged {
            add { base.DockChanged += value; }
            remove { base.DockChanged -= value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public new event EventHandler LocationChanged {
            add { base.LocationChanged += value; }
            remove { base.LocationChanged -= value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new event EventHandler TabIndexChanged {
            add { base.TabIndexChanged += value; }
            remove { base.TabIndexChanged -= value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new event EventHandler TabStopChanged {
            add { base.TabStopChanged += value; }
            remove { base.TabStopChanged -= value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler VisibleChanged {
            add { base.VisibleChanged += value; }
            remove { base.VisibleChanged -= value; }
        }

        // Methods
        public PlaceHolder(Control owner) {
            this.owner = owner;
            this.Dock = DockStyle.Fill;
            owner.Controls.Add(this);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        // Properties
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override AnchorStyles Anchor {
            get { return base.Anchor; }
            set { base.Anchor = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool AutoSize {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        [Browsable(false), Localizable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override AutoSizeMode AutoSizeMode {
            get {
                return AutoSizeMode.GrowOnly;
            }
            set {
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new BorderStyle BorderStyle {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }

        internal bool Collapsed {
            get { return this.collapsed; }
            set { this.collapsed = value; }
        }

        protected override Padding DefaultMargin {
            get { return new Padding(0, 0, 0, 0); }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override DockStyle Dock {
            get { return base.Dock; }
            set { base.Dock = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ScrollableControl.DockPaddingEdges DockPadding {
            get { return base.DockPadding; }
        }

        [Category("Layout")]
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int Height {
            get {
                if (this.Collapsed) {
                    return 0;
                }
                return base.Height;
            }
            set {
                throw new NotSupportedException();//SR.GetString("SplitContainerPanelHeight"));
            }
        }

        internal int HeightInternal {
            get { return base.Height; }
            set { base.Height = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Point Location {
            get { return base.Location; }
            set { base.Location = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MaximumSize {
            get { return base.MaximumSize; }
            set { base.MaximumSize = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MinimumSize {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Name {
            get { return base.Name; }
            set { base.Name = value; }
        }

        internal Control Owner {
            get { return this.owner; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Control Parent {
            get { return base.Parent; }
            set { base.Parent = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size Size {
            get {
                if (this.Collapsed) {
                    return Size.Empty;
                }
                return base.Size;
            }
            set {
                base.Size = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int TabIndex {
            get { return base.TabIndex; }
            set { base.TabIndex = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool TabStop {
            get { return base.TabStop; }
            set { base.TabStop = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool Visible {
            get { return base.Visible; }
            set { base.Visible = value; }
        }

        [Category("Layout")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int Width {
            get {
                if (this.Collapsed) {
                    return 0;
                }
                return base.Width;
            }
            set {
                throw new NotSupportedException();//SR.GetString("SplitContainerPanelWidth"));
            }
        }

        internal int WidthInternal {
            get { return base.Width; }
            set { base.Width = value; }
        }
    }


}

namespace WmcSoft.Windows.Forms.Design
{
    public class PlaceHolderDesigner : ScrollableControlDesigner
    {
        // Fields
        private IDesignerHost designerHost;
        private bool selected;
        //private ControlDesigner splitContainerDesigner;
        private PlaceHolder placeHolder;

        protected virtual void DrawBorder(Graphics graphics) {
            Panel component = (Panel)base.Component;
            if ((component != null) && component.Visible) {
                Pen borderPen = this.BorderPen;
                Rectangle clientRectangle = this.Control.ClientRectangle;
                clientRectangle.Width--;
                clientRectangle.Height--;
                graphics.DrawRectangle(borderPen, clientRectangle);
                borderPen.Dispose();
            }
        }

        // Properties
        protected Pen BorderPen {
            get {
                Color color = (this.Control.BackColor.GetBrightness() < 0.5) ? ControlPaint.Light(this.Control.BackColor) : ControlPaint.Dark(this.Control.BackColor);
                Pen pen = new Pen(color);
                pen.DashStyle = DashStyle.Dash;
                return pen;
            }
        }

        // Methods
        public override bool CanBeParentedTo(IDesigner parentDesigner) {
            return (parentDesigner is ControlDesigner);
        }

        protected override void Dispose(bool disposing) {
            IComponentChangeService service = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
            if (service != null) {
                service.ComponentChanged -= new ComponentChangedEventHandler(this.OnComponentChanged);
            }
            base.Dispose(disposing);
        }

        internal void DrawSelectedBorder() {
            Control control = this.Control;
            Rectangle clientRectangle = control.ClientRectangle;
            using (Graphics graphics = control.CreateGraphics()) {
                Color color;
                if (control.BackColor.GetBrightness() < 0.5) {
                    color = ControlPaint.Light(control.BackColor);
                } else {
                    color = ControlPaint.Dark(control.BackColor);
                }
                using (Pen pen = new Pen(color)) {
                    pen.DashStyle = DashStyle.Dash;
                    clientRectangle.Inflate(-4, -4);
                    graphics.DrawRectangle(pen, clientRectangle);
                }
            }
        }

        internal void DrawWaterMark(Graphics g) {
            Control control = this.Control;
            Rectangle clientRectangle = control.ClientRectangle;
            string name = control.Name;
            using (Font font = new Font("Arial", 8f)) {
                int x = (clientRectangle.Width / 2) - (((int)g.MeasureString(name, font).Width) / 2);
                int y = clientRectangle.Height / 2;
                TextRenderer.DrawText(g, name, font, new Point(x, y), Color.Black, TextFormatFlags.GlyphOverhangPadding);
            }
        }

        internal void EraseBorder() {
            Control control = this.Control;
            Rectangle clientRectangle = control.ClientRectangle;
            Graphics graphics = control.CreateGraphics();
            Pen pen = new Pen(control.BackColor);
            pen.DashStyle = DashStyle.Dash;
            clientRectangle.Inflate(-4, -4);
            graphics.DrawRectangle(pen, clientRectangle);
            pen.Dispose();
            graphics.Dispose();
            control.Invalidate();
        }

        public override void Initialize(IComponent component) {
            base.Initialize(component);
            this.placeHolder = (PlaceHolder)component;
            this.designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
            //this.splitContainerDesigner = (ControlDesigner)this.designerHost.GetDesigner(this.placeHolder.Parent);
            IComponentChangeService service = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
            if (service != null) {
                service.ComponentChanged += new ComponentChangedEventHandler(this.OnComponentChanged);
            }
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(component)["Locked"];
            if ((descriptor != null) && (this.placeHolder.Parent is SplitContainer)) {
                descriptor.SetValue(component, true);
            }
        }

        private void OnComponentChanged(object sender, ComponentChangedEventArgs e) {
            if (this.placeHolder.Parent != null) {
                if (this.placeHolder.Controls.Count == 0) {
                    Graphics g = this.placeHolder.CreateGraphics();
                    this.DrawWaterMark(g);
                    g.Dispose();
                } else {
                    this.placeHolder.Invalidate();
                }
            }
        }

        protected override void OnDragDrop(DragEventArgs de) {
            if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly) {
                de.Effect = DragDropEffects.None;
            } else {
                base.OnDragDrop(de);
            }
        }

        protected override void OnDragEnter(DragEventArgs de) {
            if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly) {
                de.Effect = DragDropEffects.None;
            } else {
                base.OnDragEnter(de);
            }
        }

        protected override void OnDragLeave(EventArgs e) {
            if (this.InheritanceAttribute != InheritanceAttribute.InheritedReadOnly) {
                base.OnDragLeave(e);
            }
        }

        protected override void OnDragOver(DragEventArgs de) {
            if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly) {
                de.Effect = DragDropEffects.None;
            } else {
                base.OnDragOver(de);
            }
        }

        //protected override void OnMouseHover() {
        //    if (this.splitContainerDesigner != null) {
        //        this.splitContainerDesigner.OnMouseHover();
        //    }
        //}

        protected override void OnPaintAdornments(PaintEventArgs pe) {
            if (this.placeHolder.BorderStyle == BorderStyle.None) {
                this.DrawBorder(pe.Graphics);
            }
            if (this.Selected) {
                this.DrawSelectedBorder();
            }
            if (this.placeHolder.Controls.Count == 0) {
                this.DrawWaterMark(pe.Graphics);
            }
        }

        protected override void PreFilterProperties(IDictionary properties) {
            base.PreFilterProperties(properties);
            properties.Remove("Modifiers");
            properties.Remove("Locked");
            properties.Remove("GenerateMember");
            foreach (DictionaryEntry entry in properties) {
                PropertyDescriptor oldPropertyDescriptor = (PropertyDescriptor)entry.Value;
                if (oldPropertyDescriptor.Name.Equals("Name") && oldPropertyDescriptor.DesignTimeOnly) {
                    properties[entry.Key] = TypeDescriptor.CreateProperty(oldPropertyDescriptor.ComponentType, oldPropertyDescriptor, new Attribute[] { BrowsableAttribute.No, DesignerSerializationVisibilityAttribute.Hidden });
                    break;
                }
            }
        }

        // Properties
        protected override InheritanceAttribute InheritanceAttribute {
            get {
                if ((this.placeHolder != null) && (this.placeHolder.Parent != null)) {
                    return (InheritanceAttribute)TypeDescriptor.GetAttributes(this.placeHolder.Parent)[typeof(InheritanceAttribute)];
                }
                return base.InheritanceAttribute;
            }
        }

        internal bool Selected {
            get {
                return this.selected;
            }
            set {
                this.selected = value;
                if (this.selected) {
                    this.DrawSelectedBorder();
                } else {
                    this.EraseBorder();
                }
            }
        }

        public override SelectionRules SelectionRules {
            get {
                SelectionRules none = SelectionRules.None;
                if (this.Control.Parent is SplitContainer) {
                    none = SelectionRules.None | SelectionRules.Locked;
                }
                return none;
            }
        }

        public override IList SnapLines {
            get {
                ArrayList snapLines = null;
                base.AddPaddingSnapLines(ref snapLines);
                return snapLines;
            }
        }
    }
}