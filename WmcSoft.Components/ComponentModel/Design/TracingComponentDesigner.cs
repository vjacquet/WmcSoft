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

        private IDesignerHost _designerHost;
        private TracingComponent _tracingComponent;
        private IDesignerSerializationManager _manager;
        private TraceComponentSerializationProvider _provider;
        private DisposableStack _disposables;

        #endregion

        #region Lifecycle

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            _disposables = new DisposableStack();

            _tracingComponent = component as TracingComponent;

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
                _provider = new TraceComponentSerializationProvider(_tracingComponent);
                _manager.AddSerializationProvider(_provider);
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

        private void OnLaunchDebugger(object sender, EventArgs e)
        {
            if (!Debugger.IsAttached)
                Debugger.Break();
        }

        private void OnListComponents(object sender, EventArgs e)
        {
            using (var listform = new DesignerHostListForm()) {
                listform.ShowDialog(_designerHost);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_manager != null && _provider != null) {
                _manager.RemoveSerializationProvider(_provider);
                _provider = null;
                _manager = null;
            }

            if (_disposables != null) {
                _disposables.Dispose();
                _disposables = null;
            }
            base.Dispose(disposing);
        }

        void manager_SerializationComplete(object sender, EventArgs e)
        {
            var manager = sender as IDesignerSerializationManager;
            if (manager != null) {
                var codeTypeDeclaration = manager.Context[typeof(CodeTypeDeclaration)] as CodeTypeDeclaration;
                codeTypeDeclaration = null;
            }
        }

        #endregion

        #region IComponentChangeService

        private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
        {
            var component = e.Component as IComponent;
            if (component != null && component.Site != null && e.Member != null)
                Trace("IComponentChangeService", "The member `{0}` of the component `{1}` has changed.", e.Member.Name, component.Site.Name);
        }

        private void OnComponentChanging(object sender, ComponentChangingEventArgs e)
        {
            var component = e.Component as IComponent;
            if (component != null && component.Site != null && e.Member != null)
                Trace("IComponentChangeService", "The member `{0}` of the component `{1}` is changing.", e.Member.Name, component.Site.Name);
        }

        private void OnComponentAdded(object sender, ComponentEventArgs e)
        {
            Trace("IComponentChangeService", "The component of type `{0}` has been added under the name `{1}`.", e.Component.GetType().FullName, e.Component.Site.Name);
        }

        private void OnComponentAdding(object sender, ComponentEventArgs e)
        {
            Trace("IComponentChangeService", "The component of type `{0}` is being added.", e.Component.GetType().FullName);
        }

        private void OnComponentRemoved(object sender, ComponentEventArgs e)
        {
            Trace("IComponentChangeService", "The component `{0}` has been removed.", e.Component.Site.Name);
        }

        private void OnComponentRemoving(object sender, ComponentEventArgs e)
        {
            Trace("IComponentChangeService", "The component `{0}` is being removed.", e.Component.Site.Name);
        }

        private void OnComponentRename(object sender, ComponentRenameEventArgs e)
        {
            Trace("IComponentChangeService", "The component `{0}` was renamed to `{1}`.", e.OldName, e.NewName);
        }

        void Trace(string category, string format, params object[] args)
        {
            if (_tracingComponent != null) {
                _tracingComponent.TraceMessage(category, String.Format(format, args));
            }
        }

        #endregion
    }

    //[DefaultSerializationProvider(typeof(CodeDomSerializer))]
    class TraceComponentSerializationProvider : IDesignerSerializationProvider
    {
        TracingComponent _tracingComponent;

        public TraceComponentSerializationProvider(TracingComponent tracingComponent)
        {
            _tracingComponent = tracingComponent;
        }

        void Trace(string format, params object[] args)
        {
            if (_tracingComponent != null) {
                _tracingComponent.TraceMessage("DesignerSerialization", String.Format(format, args));
            }
        }

        #region IDesignerSerializationProvider Membres

        public object GetSerializer(IDesignerSerializationManager manager, object currentSerializer, Type objectType, Type serializerType)
        {
            Trace("Request serialization for `{0}`.", objectType);

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