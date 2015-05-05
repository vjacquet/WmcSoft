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
using System.CodeDom;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;

namespace WmcSoft.ComponentModel.Design
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class TracingComponentDesigner : ComponentDesigner
    {
        #region Private Fields

        IDesignerHost _designerHost;
        TracingComponent _tracingComponent;
        IDesignerSerializationManager _manager;
        TraceComponentSerializationProvider _provider;
        private DisposableStack _disposables;

        #endregion

        #region Lifecycle

        public override void Initialize(IComponent component) {
            base.Initialize(component);

            _tracingComponent = component as TracingComponent;

            _disposables = new DisposableStack();

            _designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));

            var componentChangeService = _designerHost.GetService<IComponentChangeService>();
            if (componentChangeService != null) {
                componentChangeService.ComponentChanged += OnComponentChanged;
                componentChangeService.ComponentChanging += OnComponentChanging;
                componentChangeService.ComponentAdded += OnComponentAdded;
                componentChangeService.ComponentAdding += OnComponentAdding;
                componentChangeService.ComponentRemoved += OnComponentRemoved;
                componentChangeService.ComponentRemoving += OnComponentRemoving;
                componentChangeService.ComponentRename += OnComponentRename;

                _disposables.Push(() => {
                    componentChangeService.ComponentChanged -= OnComponentChanged;
                    componentChangeService.ComponentChanging -= OnComponentChanging;
                    componentChangeService.ComponentAdded -= OnComponentAdded;
                    componentChangeService.ComponentAdding -= OnComponentAdding;
                    componentChangeService.ComponentRemoved -= OnComponentRemoved;
                    componentChangeService.ComponentRemoving -= OnComponentRemoving;
                    componentChangeService.ComponentRename -= OnComponentRename;
                });
            }

            _manager = _designerHost.GetService<IDesignerSerializationManager>();
            if (_manager != null) {
                //provider = new TraceComponentSerializationProvider();
                //manager.AddSerializationProvider(provider);
                var designerSerializationManager = _manager as DesignerSerializationManager;
                if (designerSerializationManager != null) {
                    designerSerializationManager.SessionCreated += delegate {
                        _manager.SerializationComplete += manager_SerializationComplete;
                        _disposables.Push(() => _manager.SerializationComplete -= manager_SerializationComplete);
                    };
                } else {
                    _manager.SerializationComplete += manager_SerializationComplete;
                    _disposables.Push(() => _manager.SerializationComplete -= manager_SerializationComplete);
                }
            }
        }

        public override DesignerVerbCollection Verbs {
            get {
                var dvc = new DesignerVerbCollection();
                dvc.Add(new DesignerVerb("Launch Debugger", new EventHandler(OnLaunchDebugger)));
                dvc.Add(new DesignerVerb("List Components", new EventHandler(OnListComponents)));
                return dvc;
            }
        }

        private void OnLaunchDebugger(object sender, EventArgs e) {
            if (!Debugger.IsAttached)
                Debugger.Break();
        }

        private void OnListComponents(object sender, EventArgs e) {
            using (var listform = new DesignerHostListForm()) {
                listform.ShowDialog(_designerHost);
            }
        }

        protected override void Dispose(bool disposing) {
            if (_manager != null && _provider != null)
                _manager.RemoveSerializationProvider(_provider);

            if (_disposables != null)
                _disposables.Dispose();
            base.Dispose(disposing);
        }

        void manager_SerializationComplete(object sender, EventArgs e) {
            var manager = sender as IDesignerSerializationManager;
            if (manager != null) {
                var codeTypeDeclaration = manager.Context[typeof(CodeTypeDeclaration)] as CodeTypeDeclaration;
                codeTypeDeclaration = null;
            }
        }

        #endregion

        #region IComponentChangeService

        private void OnComponentChanged(object sender, ComponentChangedEventArgs e) {
            if (e.Component != null && ((IComponent)e.Component).Site != null && e.Member != null)
                Trace("IComponentChangeService", "The " + e.Member.Name + " member of the " + ((IComponent)e.Component).Site.Name + " component has been changed.");
        }

        private void OnComponentChanging(object sender, ComponentChangingEventArgs e) {
            if (e.Component != null && ((IComponent)e.Component).Site != null && e.Member != null)
                Trace("IComponentChangeService", "The " + e.Member.Name + " member of the " + ((IComponent)e.Component).Site.Name + " component is being changed.");
        }

        private void OnComponentAdded(object sender, ComponentEventArgs e) {
            Trace("IComponentChangeService", "A component, " + e.Component.Site.Name + ", has been added.");
        }

        private void OnComponentAdding(object sender, ComponentEventArgs e) {
            Trace("IComponentChangeService", "A component of type " + e.Component.GetType().FullName + " is being added.");
        }

        private void OnComponentRemoved(object sender, ComponentEventArgs e) {
            Trace("IComponentChangeService", "A component, " + e.Component.Site.Name + ", has been removed.");
        }

        private void OnComponentRemoving(object sender, ComponentEventArgs e) {
            Trace("IComponentChangeService", "A component, " + e.Component.Site.Name + ", is being removed.");
        }

        private void OnComponentRename(object sender, ComponentRenameEventArgs e) {
            Trace("IComponentChangeService", "A component, " + e.OldName + ", was renamed to " + e.NewName + ".");
        }

        void Trace(string category, string message) {
            if (_tracingComponent != null) {
                _tracingComponent.TraceMessage(category, message);
            }
        }

        #endregion
    }

    //[DefaultSerializationProvider(typeof(CodeDomSerializer))]
    public class TraceComponentSerializationProvider : IDesignerSerializationProvider
    {
        #region IDesignerSerializationProvider Membres

        public object GetSerializer(IDesignerSerializationManager manager, object currentSerializer, Type objectType, Type serializerType) {
            if (typeof(IContainer).IsAssignableFrom(objectType)) {
                return null;
            } else if (typeof(System.ComponentModel.ComponentResourceManager).IsAssignableFrom(objectType)) {
                return null;
            }
            return null;
        }

        #endregion
    }
}