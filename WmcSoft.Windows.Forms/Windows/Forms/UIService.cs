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
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing;
using WmcSoft.ComponentModel;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(UIService), "UIService.bmp")]
    [Designer(typeof(WmcSoft.Windows.Forms.Design.UIServiceDesigner))]
    public partial class UIService : Component, IUIService
    {
        class Initializing
        {
        }

        #region Style constants

        public const string DialogFontStyle = "DialogFont";
        public const string HighlightColorStyle = "HighlightColor";

        #endregion

        #region Fields

        public static IUIService Default {
            get {
                if (uiService == null) {
                    lock (Application.OpenForms) {
                        if (uiService == null) {
                            uiService = new UIService();
                        }
                    }
                }
                return uiService;
            }
        }
        static IUIService uiService = null;

        readonly Dictionary<string, object> _styles;
        readonly NestedContainerWithServiceContainer _container;

        #endregion

        #region Lifecycle

        private UIService(Initializing initializing) {
            _container = new NestedContainerWithServiceContainer(this);
            _container.ServiceContainer.AddService(typeof(IUIService), this);

            _styles = new Dictionary<string, object>();
            _styles[HighlightColorStyle] = SystemColors.Highlight;
            _styles[DialogFontStyle] = SystemFonts.DialogFont;
        }

        public UIService()
            : this((Initializing)null) {

            InitializeComponent();
        }

        public UIService(IContainer container)
            : this((Initializing)null) {

            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region Properties

        public ContainerControl ContainerControl {
            get { return containerControl; }
            set { containerControl = value; }
        }
        ContainerControl containerControl;

        [Bindable(BindableSupport.Yes)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [DefaultValue(false)]
        public bool IsUIDirty {
            get {
                return isUIDirty;
            }
            set {
                if (isUIDirty != value) {
                    isUIDirty = true;
                    OnIsUIDirtyChanged(EventArgs.Empty);
                }
            }
        }
        bool isUIDirty = false;

        public event EventHandler IsUIDirtyChanged {
            add { this.Events.AddHandler(IsUIDirtyChangedEvent, value); }
            remove { this.Events.RemoveHandler(IsUIDirtyChangedEvent, value); }
        }
        static object IsUIDirtyChangedEvent = new Object();

        /// <summary>
        /// Event initializeComponent for the <see cref="ServiceResolve"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnIsUIDirtyChanged(EventArgs e) {
            EventHandler handler = this.Events[IsUIDirtyChangedEvent] as EventHandler;
            if (handler != null) {
                handler(this, e);
            }
        }

        #endregion

        #region Events

        public event ShowingToolWindowEventHandler ShowingToolWindow {
            add { base.Events.AddHandler(ShowingToolWindowEvent, value); }
            remove { base.Events.RemoveHandler(ShowingToolWindowEvent, value); }
        }
        static object ShowingToolWindowEvent = new object();

        protected virtual void OnShowingToolWindow(ShowingToolWindowEventArgs e) {
            Delegate handler = base.Events[ShowingToolWindowEvent];
            if (handler != null) {
                foreach (Delegate @delegate in handler.GetInvocationList()) {
                    ISynchronizeInvoke si = @delegate.Target as ISynchronizeInvoke;
                    if (si != null && si.InvokeRequired) {
                        si.Invoke(@delegate, new object[] { this, e });
                    } else {
                        ((ShowingToolWindowEventHandler)@delegate)(this, e);
                    }
                    if (e.Handled)
                        break;
                }
            }
        }

        #endregion

        #region IUIService Membres

        public virtual bool CanShowComponentEditor(object component) {
            object editor = TypeDescriptor.GetEditor(component, typeof(ComponentEditor));
            return (null != editor);
        }

        public IWin32Window GetDialogOwnerWindow() {
            return containerControl == null ? null : containerControl.ParentForm;
        }

        public virtual void SetUIDirty() {
            IsUIDirty = true;
        }

        public virtual bool ShowComponentEditor(object component, IWin32Window parent) {
            ComponentEditor editor = TypeDescriptor.GetEditor(component, typeof(ComponentEditor)) as ComponentEditor;
            if (editor != null) {
                return editor.EditComponent(component);
            } else {
                return false;
            }
        }

        public DialogResult ShowDialog(Form form) {
            _container.Add(form);
            try {
                return form.ShowDialog();
            }
            finally {
                _container.Remove(form);
            }
        }

        public void ShowError(Exception ex, string message) {
            using (ThreadExceptionDialog form = new ThreadExceptionDialog(ex)) {
                ShowDialog(form);
            }
        }

        public void ShowError(Exception ex) {
            using (ThreadExceptionDialog form = new ThreadExceptionDialog(ex)) {
                ShowDialog(form);
            }
        }

        public void ShowError(string message) {
            MessageBox.Show(GetDialogOwnerWindow(), message, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons) {
            return MessageBox.Show(GetDialogOwnerWindow(), message, caption, buttons, MessageBoxIcon.Information);
        }

        public void ShowMessage(string message, string caption) {
            ShowMessage(message, caption, MessageBoxButtons.OK);
        }

        public void ShowMessage(string message) {
            ShowMessage(message, null, MessageBoxButtons.OK);
        }

        public bool ShowToolWindow(Guid toolWindow) {
            ShowingToolWindowEventArgs e = new ShowingToolWindowEventArgs(toolWindow);
            OnShowingToolWindow(e);
            return e.Handled;
        }

        public IDictionary Styles {
            get { return _styles; }
        }

        #endregion
    }

    public delegate void ShowingToolWindowEventHandler(object sender, ShowingToolWindowEventArgs e);

    public class ShowingToolWindowEventArgs : HandledEventArgs
    {
        public ShowingToolWindowEventArgs(Guid toolWindow) {
            _toolWindow = toolWindow;
        }

        public Guid ToolWindow {
            get { return _toolWindow; }
        }
        readonly Guid _toolWindow;

    }
}
