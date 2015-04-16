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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;

namespace WmcSoft.ComponentModel
{
    [Designer(typeof(ServiceContainerComponentDesigner))]
    [ToolboxBitmap(typeof(ServiceContainerComponent))]
    [ProvideProperty("RegisterAs", typeof(IComponent))]
    public partial class ServiceContainerComponent : Component,
        IServiceContainer,
        IExtenderProvider,
        ISupportInitialize
    {
        #region Private fields

        readonly Dictionary<IComponent, Type> servicesToRegister = new Dictionary<IComponent, Type>();

        #endregion

        #region Lifecycle

        public ServiceContainerComponent(IContainer container) {
            _serviceContainer = new ServiceContainer();

            container.Add(this);
        }

        #endregion

        #region Properties & events

        public event ServiceResolveEventHandler ServiceResolve {
            add { this.Events.AddHandler(ServiceResolveEvent, value); }
            remove { this.Events.RemoveHandler(ServiceResolveEvent, value); }
        }
        private static readonly object ServiceResolveEvent = new Object();

        /// <summary>
        /// Event initializeComponent for the <see cref="ServiceResolve"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnServiceResolve(ServiceResolveEventArgs e) {
            Delegate handler = Events[ServiceResolveEvent];
            if (handler != null) {
                foreach (Delegate @delegate in handler.GetInvocationList()) {
                    var si = @delegate.Target as ISynchronizeInvoke;
                    if (si != null && si.InvokeRequired) {
                        si.Invoke(@delegate, new object[] { this, e });
                    } else {
                        ((ServiceResolveEventHandler)@delegate)(this, e);
                    }

                    if (e.Handled) {
                        if (e.Register) {
                            this.ServiceContainer.AddService(e.ServiceType, e.ServiceInstance);
                        }
                        break;
                    }
                }
            }
        }

        #endregion

        #region IServiceContainer Members

        IServiceContainer ServiceContainer {
            get {
                if (_serviceContainer == null)
                    _serviceContainer = new ServiceContainer(this);
                return _serviceContainer;
            }
        }
        IServiceContainer _serviceContainer;

        public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote) {
            _serviceContainer.AddService(serviceType, callback, promote);
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback) {
            _serviceContainer.AddService(serviceType, callback);
        }

        public void AddService(Type serviceType, object serviceInstance, bool promote) {
            _serviceContainer.AddService(serviceType, serviceInstance, promote);
        }

        public void AddService(Type serviceType, object serviceInstance) {
            _serviceContainer.AddService(serviceType, serviceInstance);
        }

        public void RemoveService(Type serviceType, bool promote) {
            _serviceContainer.RemoveService(serviceType, promote);
        }

        public void RemoveService(Type serviceType) {
            _serviceContainer.RemoveService(serviceType);
        }

        #endregion

        #region IServiceProvider Members

        protected override object GetService(Type serviceType) {
            object instance = _serviceContainer.GetService(serviceType);
            if (instance == null) {
                var e = new ServiceResolveEventArgs(serviceType);
                OnServiceResolve(e);
                if (e.Handled) {
                    return e.ServiceInstance;
                }
                //instance = base.GetService(serviceType);
            }
            return instance;
        }

        object IServiceProvider.GetService(Type service) {
            return GetService(service);
        }

        public T GetService<T>() where T : class {
            object service = GetService(typeof(T));
            return service as T;
        }

        #endregion

        #region IExtenderProvider Membres

        [DebuggerStepThrough]
        bool IExtenderProvider.CanExtend(object extendee) {
            return extendee is IComponent && extendee != this;
        }

        [TypeConverter(typeof(WmcSoft.ComponentModel.Design.ServiceableTypesConverter))]
        [Editor(typeof(WmcSoft.ComponentModel.Design.ServiceableTypesEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(null)]
        public Type GetRegisterAs(IComponent service) {
            Type serviceType;
            if (servicesToRegister.TryGetValue(service, out serviceType)) {
                return serviceType;
            }
            return null;
        }

        public void SetRegisterAs(IComponent service, Type serviceType) {
            Type previousServiceType = null;
            servicesToRegister.TryGetValue(service, out previousServiceType);
            if (serviceType != previousServiceType) {
                if (previousServiceType != null) {
                    servicesToRegister.Remove(service);
                    ServiceContainer.RemoveService(previousServiceType);
                    service.Disposed -= new EventHandler(service_Disposed);
                }
                if (serviceType != null) {
                    ServiceContainer.AddService(serviceType, service);
                    service.Disposed += new EventHandler(service_Disposed);
                    servicesToRegister[service] = serviceType;
                }
            }
        }

        void service_Disposed(object sender, EventArgs e) {
            var service = sender as IComponent;
            if (service != null) {
                ServiceContainer.RemoveService(servicesToRegister[service]);
            }
        }

        #endregion

        #region ISupportInitialize Membres

        void ISupportInitialize.BeginInit() {
        }

        void ISupportInitialize.EndInit() {
        }

        #endregion
    }

    #region Events

    public delegate void ServiceResolveEventHandler(object sender, ServiceResolveEventArgs args);

    public class ServiceResolveEventArgs : HandledEventArgs
    {
        private readonly Type _serviceType;

        public ServiceResolveEventArgs(Type serviceType) {
            _serviceType = serviceType;
            _serviceInstance = null;
        }

        public Type ServiceType {
            get { return _serviceType; }
        }

        public object ServiceInstance {
            get {
                return _serviceInstance;
            }
            set {
                if (value != null) {
                    _serviceInstance = value;
                    this.Handled = true;
                }
            }
        }
        private object _serviceInstance;

        public bool Register {
            get { return _register; }
            set { _register = value; }
        }
        bool _register;
    }

    #endregion

    #region Design section

    #region Class ServiceContainerComponentDesigner

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class ServiceContainerComponentDesigner : ComponentDesigner
    {
        #region Class ServingNestedContainerCodeDomProvider

        internal class ServingNestedContainerCodeDomProvider : CodeDomSerializer, IDesignerSerializationProvider
        {
            private static ServingNestedContainerCodeDomProvider _defaultSerializer;
            static ServingNestedContainerCodeDomProvider() {
                _defaultSerializer = new ServingNestedContainerCodeDomProvider();
            }

            internal static ServingNestedContainerCodeDomProvider Default {
                get { return _defaultSerializer; }
            }

            private const string containerName = "components";
            private readonly Type _typeToProvide;

            public ServingNestedContainerCodeDomProvider() {
                _typeToProvide = typeof(NestedContainerWithServiceContainer);
            }

            protected override object DeserializeInstance(IDesignerSerializationManager manager, Type type, object[] parameters, string name, bool addToContainer) {
                if (typeof(IContainer).IsAssignableFrom(type)) {
                    var service = manager.GetService(typeof(IContainer));
                    if (service != null) {
                        manager.SetName(service, name);
                        return service;
                    }
                }
                return base.DeserializeInstance(manager, type, parameters, name, addToContainer);
            }

            public override object Serialize(IDesignerSerializationManager manager, object value) {
                CodeExpression codeExpression;
                CodeTypeDeclaration codeTypeDeclaration = manager.Context[typeof(CodeTypeDeclaration)] as CodeTypeDeclaration;
                RootContext rootContext = manager.Context[typeof(RootContext)] as RootContext;
                CodeStatementCollection statements = new CodeStatementCollection();
                if ((codeTypeDeclaration != null) && (rootContext != null)) {
                    CodeMemberField componentsField = new CodeMemberField(typeof(IContainer), containerName);
                    componentsField.Attributes = MemberAttributes.Private;
                    codeTypeDeclaration.Members.Add(componentsField);
                    codeExpression = new CodeFieldReferenceExpression(rootContext.Expression, containerName);
                } else {
                    statements.Add(new CodeVariableDeclarationStatement(typeof(IContainer), containerName));
                    codeExpression = new CodeVariableReferenceExpression(containerName);
                }
                base.SetExpression(manager, value, codeExpression);

                // new 'typeToProvide'(this);
                CodeObjectCreateExpression containerExpression = new CodeObjectCreateExpression(_typeToProvide, new CodeExpression[] { new CodeThisReferenceExpression() });
                CodeAssignStatement statement = new CodeAssignStatement(codeExpression, containerExpression);
                statement.UserData["IContainer"] = "IContainer";
                statements.Add(statement);

                return statements;
            }

            #region IDesignerSerializationProvider Members

            public object GetSerializer(IDesignerSerializationManager manager, object currentSerializer, Type objectType, Type serializerType) {
                if (typeof(IContainer).IsAssignableFrom(objectType))
                    return ServingNestedContainerCodeDomProvider.Default;
                //if (typeof(System.Resources.ResourceManager).IsAssignableFrom(objectType)) 
                //    return null;
                return null;
            }

            #endregion

        }

        #endregion

        IDesignerSerializationManager _manager;
        IDesignerSerializationProvider _provider;

        public ServiceContainerComponentDesigner() {
        }

        public override void Initialize(IComponent component) {
            base.Initialize(component);

            // Obtain an IDesignerHost service from the design environment.
            var host = component.Site.GetService<IDesignerHost>();
            if (host == null)
                return;
            _manager = host.GetService(typeof(IDesignerSerializationManager)) as IDesignerSerializationManager;
            if (_manager == null)
                return;
            _provider = new ServingNestedContainerCodeDomProvider();
            _manager.AddSerializationProvider(_provider);
        }

        protected override void Dispose(bool disposing) {
            if (_manager != null && _provider != null) {
                _manager.RemoveSerializationProvider(_provider);
            }

            base.Dispose(disposing);
        }

    }

    #endregion

    #endregion
}
