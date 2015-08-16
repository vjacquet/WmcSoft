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
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using WmcSoft.Windows.Forms.Design;
using WmcSoft.Windows.Forms.Layout;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(DeckLayoutPanel), "DeckLayoutPanel.bmp")]
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
                _activeControl.Visible = true;
                if (inactive != null) {
                    OnControlDeactivated(new ControlEventArgs(inactive));
                    inactive.Visible = false;
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
