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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using WmcSoft.ComponentModel.Design;

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

            _disposables = new DisposableStack();

            _panel = (DeckLayoutPanel)component;
            _designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));

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

}


