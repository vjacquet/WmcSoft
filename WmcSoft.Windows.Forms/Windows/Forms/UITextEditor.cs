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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design;
using WmcSoft.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.ComponentModel.Design;
using System.Windows.Forms.VisualStyles;
using System.Runtime.InteropServices;

namespace WmcSoft.Windows.Forms
{
    [DefaultBindingProperty("Text")]
    [ToolboxBitmap(typeof(UITextEditor), "UITextEditor.bmp")]
    [Designer(typeof(WmcSoft.Windows.Forms.Design.UITextEditorDesigner))]
    public partial class UITextEditor : ContainerControl, IWindowsFormsEditorService
    {
        #region Lifecycle

        public UITextEditor() {
            PreInitializeComponent();
            InitializeComponent();
        }

        public UITextEditor(IContainer container) {
            container.Add(this);
            PreInitializeComponent();
            InitializeComponent();
        }

        private void PreInitializeComponent() {
            SetStyle(ControlStyles.Opaque | ControlStyles.FixedHeight | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.StandardClick, false);
            SetStyle(ControlStyles.UseTextForAccessibility, false);

            DataBindings.CollectionChanged += new CollectionChangeEventHandler(DataBindings_CollectionChanged);
            BindingContextChanged += new EventHandler(UITextEditor_BindingContextChanged);
        }

        #endregion

        #region IWindowsFormsEditorService Members

        public void CloseDropDown() {
            dropDownHolder.SetControl(null);
            dropDownHolder.Visible = false;
            button.Focus();
        }

        public void DropDownControl(Control control) {
            if (dropDownHolder == null) {
                dropDownHolder = new DropDownHolder(this);
            }
            dropDownHolder.SetControl(control);
            dropDownHolder.Location = button.PointToScreen(new Point(0, button.Height));
            try {
                dropDownHolder.Visible = true;
                dropDownHolder.Lock();
                dropDownHolder.FocusControl();
                dropDownHolder.DoModalLoop();
            }
            finally {
                dropDownHolder.Unlock();
            }
        }

        public DialogResult ShowDialog(Form dialog) {
            IUIService uiService = GetService(typeof(IUIService)) as IUIService;
            if (uiService != null) {
                return uiService.ShowDialog(dialog);
            }
            return dialog.ShowDialog(this);
        }

        DropDownHolder dropDownHolder;

        #endregion

        #region Properties

        protected override Padding DefaultPadding {
            get { return new Padding(); }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AutoScroll {
            get { return false; }
            set { }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size AutoScrollMargin {
            get { return base.AutoScrollMargin; }
            set { base.AutoScrollMargin = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size AutoScrollMinSize {
            get { return base.AutoScrollMinSize; }
            set { base.AutoScrollMinSize = value; }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Always)]
        public override bool AutoSize {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Category("PropertyChanged")]
        public new event EventHandler AutoSizeChanged {
            add { base.AutoSizeChanged += value; }
            remove { base.AutoSizeChanged -= value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override Image BackgroundImage {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler BackgroundImageChanged {
            add { base.BackgroundImageChanged += value; }
            remove { base.BackgroundImageChanged -= value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override ImageLayout BackgroundImageLayout {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler BackgroundImageLayoutChanged {
            add { base.BackgroundImageLayoutChanged += value; }
            remove {
                base.BackgroundImageLayoutChanged -= value;
            }
        }

        [DefaultValue(BorderStyle.Fixed3D)]
        [DispId(-504)]
        [Category("Appearance")]
        public BorderStyle BorderStyle {
            get {
                return _borderStyle;
            }
            set {
                if (!value.IsValid(0, 2)) {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
                }
                if (_borderStyle != value) {
                    _borderStyle = value;
                    base.RecreateHandle();
                }
            }
        }
        BorderStyle _borderStyle = BorderStyle.Fixed3D;

        public override ContextMenu ContextMenu {
            get {
                return base.ContextMenu;
            }
            set {
                base.ContextMenu = value;
                textBox.ContextMenu = value;
            }
        }

        public override ContextMenuStrip ContextMenuStrip {
            get {
                return base.ContextMenuStrip;
            }
            set {
                base.ContextMenuStrip = value;
                textBox.ContextMenuStrip = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new ScrollableControl.DockPaddingEdges DockPadding {
            get { return base.DockPadding; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool Focused {
            get { return textBox.Focused; }
        }

        public override Color ForeColor {
            get {
                return textBox.ForeColor;
            }
            set {
                base.ForeColor = value;
                textBox.ForeColor = value;
            }
        }

        public override Size MaximumSize {
            get { return base.MaximumSize; }
            set { base.MaximumSize = new Size(value.Width, 0); }
        }

        public override Size MinimumSize {
            get { return base.MinimumSize; }
            set { base.MinimumSize = new Size(value.Width, 0); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler MouseEnter {
            add { base.MouseEnter += value; }
            remove { base.MouseEnter -= value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler MouseHover {
            add { base.MouseHover += value; }
            remove { base.MouseHover -= value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler MouseLeave {
            add { base.MouseLeave += value; }
            remove { base.MouseLeave -= value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event MouseEventHandler MouseMove {
            add { base.MouseMove += value; }
            remove { base.MouseMove -= value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new Padding Padding {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Category("Layout")]
        public int PreferredHeight {
            get {
                int value = base.FontHeight + 3;
                if (_borderStyle != BorderStyle.None) {
                    value += SystemInformation.BorderSize.Height * 4;
                }
                return value;
            }
        }

        [DefaultValue(false)]
        [Category("Behavior")]
        public bool ReadOnly {
            get {
                return textBox.ReadOnly;
            }
            set {
                textBox.ReadOnly = value;
                button.Enabled = !value;
            }
        }

        [Browsable(true)]
        [Localizable(true)]
        [Bindable(BindableSupport.Yes)]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public override string Text {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        private void textBox_TextChanged(object sender, EventArgs e) {
            OnTextChanged(e);
            canvas.Invalidate();
        }

        [DefaultValue(HorizontalAlignment.Left)]
        [Localizable(true)]
        [Category("Appearance")]
        public HorizontalAlignment TextAlign {
            get { return textBox.TextAlign; }
            set { textBox.TextAlign = value; }
        }

        #endregion

        #region Binding & editing

        void DataBindings_CollectionChanged(object sender, CollectionChangeEventArgs e) {
            Binding binding = e.Element as Binding;
            if (binding != null && binding.PropertyName == "Text") {
                switch (e.Action) {
                case CollectionChangeAction.Add:
                    _textBinding = binding;
                    _textBinding.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation;
                    _textBinding.Format += textBinding_Format;
                    _textBinding.Parse += textBinding_Parse;
                    break;
                case CollectionChangeAction.Refresh:
                    break;
                case CollectionChangeAction.Remove:
                    if (_bindingManagerBase != null)
                        _bindingManagerBase.CurrentChanged += BindingManagerBase_CurrentChanged;
                    _textBinding.Format -= textBinding_Format;
                    _textBinding.Parse -= textBinding_Parse;
                    _textBinding = null;
                    _editor = null;
                    canvas.Visible = false;
                    button.Visible = false;
                    _typeDescriptorContext = null;
                    break;
                default:
                    break;
                }
            }
        }

        void UITextEditor_BindingContextChanged(object sender, EventArgs e) {
            if (_textBinding != null && _textBinding.BindingManagerBase != _bindingManagerBase) {
                // Wire the current changed event on the textBinding' binding manager.
                if (_bindingManagerBase != null)
                    _bindingManagerBase.CurrentChanged -= BindingManagerBase_CurrentChanged;
                _bindingManagerBase = _textBinding.BindingManagerBase;
                if (_bindingManagerBase != null)
                    _bindingManagerBase.CurrentChanged += BindingManagerBase_CurrentChanged;
            }
        }

        void BindingManagerBase_CurrentChanged(object sender, EventArgs e) {
            BindingManagerBase bindingManagerBase = _textBinding.BindingManagerBase;
            if (bindingManagerBase != null && bindingManagerBase.Count > 0) {
                var current = bindingManagerBase.Current;
                _editor = TypeDescriptor.GetEditor(current, typeof(UITypeEditor)) as UITypeEditor;
                _propertyDescriptor = bindingManagerBase.GetItemProperty(_textBinding.BindingMemberInfo.BindingField, true);
                if (_editor == null && _propertyDescriptor != null)
                    _editor = _propertyDescriptor.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
                if (_editor != null) {
                    var parentProvider = this.GetService(typeof(IServiceProvider)) as IServiceProvider;
                    var serviceContainer = new ServiceContainer(parentProvider);
                    serviceContainer.AddService(typeof(IWindowsFormsEditorService), this);
                    _typeDescriptorContext = new TypeDescriptorContext(serviceContainer, _propertyDescriptor, current);
                    canvas.Visible = _editor.GetPaintValueSupported(_typeDescriptorContext);
                    switch (_editor.GetEditStyle(_typeDescriptorContext)) {
                    case UITypeEditorEditStyle.DropDown:
                        button.Visible = true;
                        break;
                    case UITypeEditorEditStyle.Modal:
                        button.Visible = true;
                        break;
                    case UITypeEditorEditStyle.None:
                    default:
                        button.Visible = false;
                        break;
                    }
                } else {
                    canvas.Visible = false;
                    button.Visible = false;
                    _typeDescriptorContext = null;
                }
            }
        }

        void textBinding_Parse(object sender, ConvertEventArgs e) {
            if (e.Value == null) {
                if (e.DesiredType == typeof(string))
                    e.Value = "";
            } else if (_propertyDescriptor != null) {
                e.Value = _propertyDescriptor
                    .Converter
                    .ConvertFrom(_typeDescriptorContext, CultureInfo.CurrentUICulture, e.Value);
            } else {
                TypeConverter converter = TypeDescriptor.GetConverter(e.Value);
                e.Value = converter.ConvertTo(e.Value, e.DesiredType);
            }
        }

        void textBinding_Format(object sender, ConvertEventArgs e) {
            if (e.Value == null) {
                if (e.DesiredType == typeof(string))
                    e.Value = "";
            } else if (_propertyDescriptor != null) {
                e.Value = _propertyDescriptor
                    .Converter
                    .ConvertTo(_typeDescriptorContext, CultureInfo.CurrentUICulture, e.Value, e.DesiredType);
            } else {
                var converter = TypeDescriptor.GetConverter(e.Value);
                e.Value = converter.ConvertTo(e.Value, e.DesiredType);
            }
        }

        Binding _textBinding;
        BindingManagerBase _bindingManagerBase;
        ITypeDescriptorContext _typeDescriptorContext;
        UITypeEditor _editor;
        PropertyDescriptor _propertyDescriptor;

        private void button_Click(object sender, EventArgs e) {
            if (_editor != null && _propertyDescriptor != null) {
                _textBinding.WriteValue();
                var value = _propertyDescriptor.GetValue(_bindingManagerBase.Current);
                value = _editor.EditValue(_typeDescriptorContext, value);
                _propertyDescriptor.SetValue(_bindingManagerBase.Current, value);
                _textBinding.ReadValue();
            }
        }

        #endregion

        #region Overrides

        protected override void OnFontChanged(EventArgs e) {
            base.FontHeight = -1;
            base.Height = this.PreferredHeight;
            base.OnFontChanged(e);
        }

        #endregion

        #region Layout & rendering

        protected override Size DefaultSize {
            get {
                return new Size(100, PreferredHeight);
            }
        }

        protected override CreateParams CreateParams {
            get {
                var createParams = base.CreateParams;

                createParams.Style &= unchecked((int)~NativeMethods.WS_BORDER);
                if (!Application.RenderWithVisualStyles) {
                    switch (_borderStyle) {
                    case BorderStyle.Fixed3D:
                        createParams.ExStyle |= unchecked((int)NativeMethods.WS_EX_CLIENTEDGE);
                        break;
                    case BorderStyle.FixedSingle:
                        createParams.Style |= unchecked((int)NativeMethods.WS_BORDER);
                        break;
                    }
                }
                return createParams;
            }
        }

        public override Size GetPreferredSize(Size proposedSize) {
            var preferredSize = textBox.GetPreferredSize(proposedSize);
            preferredSize.Height = Math.Max(preferredSize.Height, canvas.Height);
            switch (_borderStyle) {
            case BorderStyle.Fixed3D:
                preferredSize.Width += 2 * SystemInformation.Border3DSize.Width;
                preferredSize.Height += 2 * SystemInformation.Border3DSize.Height;
                break;
            case BorderStyle.FixedSingle:
                preferredSize.Width += 2 * SystemInformation.BorderSize.Width;
                preferredSize.Height += 2 * SystemInformation.BorderSize.Height;
                break;
            case BorderStyle.None:
            default:
                break;
            }
            return preferredSize;
        }

        protected override void OnLayout(LayoutEventArgs e) {
            if (Application.RenderWithVisualStyles) {
                int width = 0;
                int height = 0;
                switch (_borderStyle) {
                case BorderStyle.Fixed3D:
                case BorderStyle.FixedSingle:
                    width = SystemInformation.Border3DSize.Width;
                    height = SystemInformation.Border3DSize.Height;
                    break;
                //case BorderStyle.FixedSingle:
                //    width = SystemInformation.BorderSize.Width;
                //    height = SystemInformation.BorderSize.Height;
                //    break;
                case BorderStyle.None:
                default:
                    break;
                }
                tableLayoutPanel.SetBounds(width, height, this.Width - 2 * width, this.Height - 2 * height);
            }
            base.OnLayout(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
            base.OnPaintBackground(e);

            var bounds = new Rectangle(Point.Empty, this.Size);
            if (Application.RenderWithVisualStyles && TextBoxRenderer.IsSupported) {
                TextBoxRenderer.DrawTextBox(e.Graphics, bounds, TextBoxState.Normal);
            } else {
                //using (Brush brush = new SolidBrush(this.BackColor)) {
                //    e.Graphics.FillRectangle(brush, bounds);
                //}
                //switch (_borderStyle) {
                //case BorderStyle.Fixed3D:
                //    ControlPaint.DrawBorder3D(e.Graphics, bounds);
                //    break;
                //case BorderStyle.FixedSingle:
                //    ControlPaint.DrawBorder(e.Graphics, bounds, SystemColors.WindowFrame, ButtonBorderStyle.Solid);
                //    break;
                //}
            }
        }

        protected override void OnMouseHover(EventArgs e) {
            base.OnMouseHover(e);
            _mouseHover = true;
        }
        bool _mouseHover;

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            _mouseHover = false;
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            var bounds = new Rectangle(Point.Empty, this.Size);
            if (Application.RenderWithVisualStyles) {
                if (!Enabled)
                    TextBoxRenderer.DrawTextBox(e.Graphics, bounds, TextBoxState.Disabled);
                else if (_mouseHover)
                    TextBoxRenderer.DrawTextBox(e.Graphics, bounds, TextBoxState.Hot);
                else if (ReadOnly)
                    TextBoxRenderer.DrawTextBox(e.Graphics, bounds, TextBoxState.Hot);
                else
                    TextBoxRenderer.DrawTextBox(e.Graphics, bounds, TextBoxState.Normal);
            } else if (!base.Enabled && BorderStyle != BorderStyle.None) {
                bounds.Inflate(1, 1);
                ControlPaint.DrawBorder(e.Graphics, bounds, BackColor, ButtonBorderStyle.Solid);
            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e) {
            if (_editor != null && _editor.GetPaintValueSupported(_typeDescriptorContext)) {
                var value = _propertyDescriptor.GetValue(_bindingManagerBase.Current);
                _editor.PaintValue(value, e.Graphics, canvas.ClientRectangle);
            }
        }

        #endregion
    }
}
