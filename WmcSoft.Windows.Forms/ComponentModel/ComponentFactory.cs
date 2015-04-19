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
using System.ComponentModel.Design;

namespace WmcSoft.ComponentModel
{
    [Designer(typeof(ComponentFactoryDesigner))]
    public partial class ComponentFactory : Component
    {
        // TODO: the mapping should be configurable at design time through the designer.

        #region Lifecycle

        public ComponentFactory() {
            InitializeComponent();
        }

        public ComponentFactory(IContainer container) {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region Mapping

        IDictionary<string, Type> mapping;
        protected IDictionary<string, Type> Mapping {
            get {
                if (mapping == null) {
                    mapping = new Dictionary<string, Type>();
                    mapping.Add("System.Windows.Forms.SplitContainer", typeof(WmcSoft.Windows.Forms.SplitContainer));
                    mapping.Add("System.Windows.Forms.Splitter", typeof(WmcSoft.Windows.Forms.Splitter));
                    //mapping.Add("System.Windows.Forms.ToolStrip", typeof(WmcSoft.Windows.Forms.ToolStrip));
                    //mapping.Add("System.Windows.Forms.MenuStrip", typeof(WmcSoft.Windows.Forms.MenuStrip));
                }
                return mapping;
            }
        }

        public bool TryResolve(string name, out Type type) {
            return Mapping.TryGetValue(name, out type);
        }

        public void Register(string name, Type type) {
            Mapping.Add(name, type);
        }

        public void Unregister(string name) {
            Mapping.Remove(name);
        }

        #endregion

    }

    #region Design

    public class ComponentFactoryDesigner : ComponentDesigner
    {
        class TypeResolutionService : ITypeResolutionService
        {
            ITypeResolutionService service;
            ComponentFactory factory;

            internal TypeResolutionService(ComponentFactory factory, ITypeResolutionService service) {
                this.factory = factory;
                this.service = service;
            }

            #region ITypeResolutionService Membres

            public System.Reflection.Assembly GetAssembly(System.Reflection.AssemblyName name, bool throwOnError) {
                return service.GetAssembly(name, throwOnError);
            }

            public System.Reflection.Assembly GetAssembly(System.Reflection.AssemblyName name) {
                return service.GetAssembly(name);
            }

            public string GetPathOfAssembly(System.Reflection.AssemblyName name) {
                return service.GetPathOfAssembly(name);
            }

            public Type GetType(string name, bool throwOnError, bool ignoreCase) {
                Type type;
                if (factory.TryResolve(name, out type)) {
                    return type;
                }
                return service.GetType(name, throwOnError, ignoreCase);
            }
            public Type GetType(string name, bool throwOnError) {
                return GetType(name, throwOnError, false);
            }

            public Type GetType(string name) {
                return GetType(name, true, false);
            }

            public void ReferenceAssembly(System.Reflection.AssemblyName name) {
                service.ReferenceAssembly(name);
            }

            #endregion
        }

        public ComponentFactoryDesigner() {
            typeResolutionService = null;
        }
        IDictionary<string, Type> mapping;

        public override void Initialize(IComponent component) {
            var host = component.Site.GetService<IDesignerHost>();
            var itrs = host.GetService<ITypeResolutionService>();
            if (itrs != null) {
                typeResolutionService = itrs;
                host.RemoveService(typeof(ITypeResolutionService));
                host.AddService(typeof(ITypeResolutionService), new TypeResolutionService((ComponentFactory)component, itrs));
                this.host = host;
            }

            base.Initialize(component);
        }

        protected override void Dispose(bool disposing) {
            if (host != null && typeResolutionService != null) {
                var itrs = host.GetService<ITypeResolutionService>();
                if (itrs != null) {
                    host.RemoveService(typeof(ITypeResolutionService));
                    host.AddService(typeof(ITypeResolutionService), typeResolutionService);
                }
                typeResolutionService = null;
                host = null;
            }

            base.Dispose(disposing);
        }

        ITypeResolutionService typeResolutionService;
        IDesignerHost host;
    }

    #endregion
}
