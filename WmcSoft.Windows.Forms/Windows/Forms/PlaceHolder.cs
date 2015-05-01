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
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using WmcSoft.Windows.Forms.Design;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(PlaceHolder), "PlaceHolder.bmp")]
    [Designer(typeof(Design.PlaceHolderDesigner))]
    [ToolboxItem(false)]
    [Docking(DockingBehavior.Never)]
    public sealed class PlaceHolder : Panel
    {
        #region Private fields

        private Control _owner;

        #endregion

        #region Lifecycle

        public PlaceHolder(Control owner) {
            base.Dock = DockStyle.Fill;
            _owner = owner;
            _owner.Controls.Add(this);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        #endregion

        #region Events

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

        #endregion

        #region Properties

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
            set { throw new NotSupportedException(); }
        }

        [Browsable(false), Localizable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override AutoSizeMode AutoSizeMode {
            get { return AutoSizeMode.GrowOnly; }
            set { throw new NotSupportedException(); }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new BorderStyle BorderStyle {
            get { return base.BorderStyle; }
            set { throw new NotSupportedException(); }
        }

        protected override Padding DefaultMargin {
            get { return new Padding(0, 0, 0, 0); }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override DockStyle Dock {
            get { return base.Dock; }
            set { throw new NotSupportedException(); }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ScrollableControl.DockPaddingEdges DockPadding {
            get { return base.DockPadding; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Name {
            get { return base.Name; }
            set { base.Name = value; }
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
        public new int TabIndex {
            get { return base.TabIndex; }
            set { throw new NotSupportedException(); }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool TabStop {
            get { return base.TabStop; }
            set { throw new NotSupportedException(); }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool Visible {
            get { return base.Visible; }
            set { throw new NotSupportedException(); }
        }

        internal Control Owner {
            get { return _owner; }
        }

        #endregion
    }

}

namespace WmcSoft.Windows.Forms.Design
{
    public class PlaceHolderDesigner : ScrollableControlDesigner
    {
        #region Fields

        private IDesignerHost _designerHost;
        private bool _selected;
        private PlaceHolder _placeHolder;

        #endregion

        #region Methods

        protected virtual void DrawBorder(Graphics graphics) {
            Panel component = (Panel)base.Component;
            if ((component != null) && component.Visible) {
                var clientRectangle = Control.ClientRectangle;
                clientRectangle.Inflate(-1, -1);
                using (var pen = CreateBorderPen()) {
                    graphics.DrawRectangle(pen, clientRectangle);
                }
            }
        }

        //public override bool CanBeParentedTo(IDesigner parentDesigner) {
        //    return (parentDesigner is ControlDesigner);
        //}

        protected override void Dispose(bool disposing) {
            var service = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
            if (service != null) {
                service.ComponentChanged -= OnComponentChanged;
            }
            base.Dispose(disposing);
        }

        protected internal void DrawSelectedBorder() {
            var control = this.Control;
            var clientRectangle = control.ClientRectangle;
            clientRectangle.Inflate(-4, -4);
            using (var graphics = control.CreateGraphics())
            using (var pen = CreateBorderPen()) {
                graphics.DrawRectangle(pen, clientRectangle);
            }
        }

        protected internal void DrawWaterMark(Graphics g) {
            var control = this.Control;
            var clientRectangle = control.ClientRectangle;
            var name = control.Name;
            using (var font = new Font("Arial", 8f)) {
                int x = (clientRectangle.Width / 2) - (((int)g.MeasureString(name, font).Width) / 2);
                int y = clientRectangle.Height / 2;
                TextRenderer.DrawText(g, name, font, new Point(x, y), Color.Black, TextFormatFlags.GlyphOverhangPadding);
            }
        }

        protected internal void EraseBorder() {
            var control = this.Control;
            var clientRectangle = control.ClientRectangle;
            var graphics = control.CreateGraphics();
            var pen = new Pen(control.BackColor);
            pen.DashStyle = DashStyle.Dash;
            clientRectangle.Inflate(-4, -4);
            graphics.DrawRectangle(pen, clientRectangle);
            pen.Dispose();
            graphics.Dispose();
            control.Invalidate();
        }

        protected override InheritanceAttribute InheritanceAttribute {
            get {
                return _inheritanceAttribute ?? base.InheritanceAttribute;
            }
        }
        InheritanceAttribute _inheritanceAttribute;

        internal void OverrideInheritanceAttribute(InheritanceAttribute inheritanceAttribute) {
            _inheritanceAttribute = inheritanceAttribute;
        }

        public override void Initialize(IComponent component) {
            base.Initialize(component);
            _placeHolder = (PlaceHolder)component;
            _designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
            var service = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
            if (service != null) {
                service.ComponentChanged += OnComponentChanged;
            }
            var descriptor = TypeDescriptor.GetProperties(component)["Locked"];
            if (descriptor != null) {
                descriptor.SetValue(component, true);
            }
        }

        private void OnComponentChanged(object sender, ComponentChangedEventArgs e) {
            if (_placeHolder.Parent != null) {
                if (_placeHolder.Controls.Count == 0) {
                    Graphics g = _placeHolder.CreateGraphics();
                    this.DrawWaterMark(g);
                    g.Dispose();
                } else {
                    _placeHolder.Invalidate();
                }
            }
        }

        protected override void OnPaintAdornments(PaintEventArgs pe) {
            if (_placeHolder.BorderStyle == BorderStyle.None) {
                DrawBorder(pe.Graphics);
            }
            if (Selected) {
                DrawSelectedBorder();
            }
            if (_placeHolder.Controls.Count == 0) {
                DrawWaterMark(pe.Graphics);
            }
        }

        protected override void PreFilterProperties(IDictionary properties) {
            base.PreFilterProperties(properties);
            properties.Remove("Modifiers");
            properties.Remove("Locked");
            properties.Remove("GenerateMember");
            properties.Remove("MinimumSize");
            properties.Remove("MaximumSize");
            properties.Remove("Location");
            properties.Remove("Size");
            properties.Remove("Dock");
            foreach (DictionaryEntry entry in properties) {
                PropertyDescriptor oldPropertyDescriptor = (PropertyDescriptor)entry.Value;
                if (oldPropertyDescriptor.Name.Equals("Name") && oldPropertyDescriptor.DesignTimeOnly) {
                    properties[entry.Key] = TypeDescriptor.CreateProperty(oldPropertyDescriptor.ComponentType, oldPropertyDescriptor, new Attribute[] { BrowsableAttribute.No, DesignerSerializationVisibilityAttribute.Hidden });
                    break;
                }
            }
        }

        #endregion

        #region Properties

        Pen CreateBorderPen(Color backColor) {
            var color = (backColor.GetBrightness() < 0.5) ? ControlPaint.Light(backColor) : ControlPaint.Dark(backColor);
            var pen = new Pen(color);
            pen.DashStyle = DashStyle.Dash;
            return pen;
        }
        Pen CreateBorderPen() {
            return CreateBorderPen(Control.BackColor);
        }

        protected internal bool Selected {
            get {
                return _selected;
            }
            set {
                _selected = value;
                if (_selected) {
                    DrawSelectedBorder();
                } else {
                    EraseBorder();
                }
            }
        }

        public override SelectionRules SelectionRules {
            get { return SelectionRules.None | SelectionRules.Locked; }
        }

        public override IList SnapLines {
            get {
                ArrayList snapLines = null;
                base.AddPaddingSnapLines(ref snapLines);
                return snapLines;
            }
        }

        #endregion

    }

}