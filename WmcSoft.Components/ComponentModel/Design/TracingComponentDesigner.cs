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
using System.Security;
using System.Security.Permissions;

namespace WmcSoft.ComponentModel.Design
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class TracingComponentDesigner : ComponentDesigner
    {
        #region Private Fields

        IDesignerHost _designerHost;
        IDesignerSerializationManager _manager;
        TraceComponentSerializationProvider _provider;
        private DisposableStack _disposables;

        #endregion

        #region Lifecycle

        public override void Initialize(IComponent component) {
            base.Initialize(component);

            _disposables = new DisposableStack();

            _designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
            _manager = _designerHost.GetService(typeof(IDesignerSerializationManager)) as IDesignerSerializationManager;
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
                dvc.Add(new DesignerVerb("List Components", new EventHandler(OnListComponents)));
                return dvc;
            }
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