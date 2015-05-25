using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices;

using EnvDTE;
using VSLangProj;
using Microsoft.VisualStudio.Designer.Interfaces;
using WmcSoft.ComponentModel;
using WmcSoft.Interop;

using IServiceProvider = System.IServiceProvider;
using IOleServiceProvider = WmcSoft.Interop.IServiceProvider;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell.Interop;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Design;

namespace WmcSoft.VisualStudio
{
    /// <summary>
    /// This class exists to be cocreated a in a preprocessor build step.
    /// </summary>
    public abstract class BaseCodeGeneratorWithSite : BaseCodeGenerator, IObjectWithSite, IServiceProvider
    {
        private object site = null;
        private CodeDomProvider codeDomProvider = null;
        private static Guid CodeDomInterfaceGuid = new Guid("{73E59688-C7C4-4a85-AF64-A538754784C5}");
        private static Guid CodeDomServiceGuid = CodeDomInterfaceGuid;
        private ComServiceProvider serviceProvider = null;

        /// <summary>
        /// demand-creates a CodeDomProvider
        /// </summary>
        protected virtual CodeDomProvider CodeProvider {
            get {
                if (codeDomProvider == null) {
                    IVSMDCodeDomProvider vsmdCodeDomProvider = (IVSMDCodeDomProvider)GetService(CodeDomServiceGuid);
                    if (vsmdCodeDomProvider != null) {
                        codeDomProvider = (CodeDomProvider)vsmdCodeDomProvider.CodeDomProvider;
                    }
                    Debug.Assert(codeDomProvider != null, "Get CodeDomProvider Interface failed.  GetService(QueryService(CodeDomProvider) returned Null.");
                }
                return codeDomProvider;
            }
            set {
                if (value == null) {
                    throw new ArgumentNullException();
                }

                codeDomProvider = value;
            }
        }

        protected IEnumerable<Type> GetAvailableTypes(IServiceProvider provider, bool includeReferences) {
            DynamicTypeService typeService = (DynamicTypeService)provider.GetService(typeof(DynamicTypeService));
            Debug.Assert(typeService != null, "No dynamic type service registered.");

            IVsHierarchy hier = VsHelper.GetCurrentHierarchy(provider);
            Debug.Assert(hier != null, "No active hierarchy is selected.");

            ITypeDiscoveryService discovery = typeService.GetTypeDiscoveryService(hier);
            Project dteProject = VsHelper.ToDteProject(hier);

            Dictionary<string, Type> availableTypes = new Dictionary<string, Type>();
            foreach (Type type in discovery.GetTypes(typeof(object), includeReferences)) {
                // We will never allow non-public types selection, as it's terrible practice.
                if (type.IsPublic) {
                    if (!availableTypes.ContainsKey(type.FullName)) {
                        availableTypes.Add(type.FullName, type);
                    }
                }
            }

            return availableTypes.Values;
        }

        /// <summary>
        /// demand-creates a ServiceProvider given an IOleServiceProvider
        /// </summary>
        private ComServiceProvider SiteServiceProvider {
            get {
                if (serviceProvider == null) {
                    IOleServiceProvider oleServiceProvider = site as IOleServiceProvider;
                    serviceProvider = new ComServiceProvider(oleServiceProvider);
                }
                return serviceProvider;
            }
        }

        /// <summary>
        /// method to get a service by its GUID
        /// </summary>
        /// <param name="serviceGuid">GUID of service to retrieve</param>
        /// <returns>an object that implements the requested service</returns>
        protected object GetService(Guid serviceGuid) {
            return SiteServiceProvider.GetService(serviceGuid);
        }

        /// <summary>
        /// method to get a service by its Type
        /// </summary>
        /// <param name="serviceType">Type of service to retrieve</param>
        /// <returns>an object that implements the requested service</returns>
        public virtual object GetService(Type serviceType) {
            return SiteServiceProvider.GetService(serviceType);
        }

        /// <summary>
        /// gets the default extension of the output file by asking the CodeDomProvider
        /// what its default extension is.
        /// </summary>
        /// <returns></returns>
        public override string GetDefaultExtension() {
            CodeDomProvider codeDom = CodeProvider;
            Debug.Assert(codeDom != null, "CodeDomProvider is NULL.");
            string extension = codeDom.FileExtension;
            if (extension != null && extension.Length > 0) {
                if (extension[0] != '.') {
                    extension = "." + extension;
                }
            }

            return extension;
        }

        /// <summary>
        /// Method to get an ICodeGenerator with which this class can create code.
        /// </summary>
        /// <returns></returns>
        [Obsolete("Callers should not use the ICodeGenerator interface and should instead use the methods directly on the CodeDomProvider class. Those inheriting from CodeDomProvider must still implement this interface, and should exclude this warning or also obsolete this method.")]
        protected virtual ICodeGenerator GetCodeWriter() {
            CodeDomProvider codeDom = CodeProvider;
            if (codeDom != null) {
                return codeDom.CreateGenerator();
            }

            return null;
        }

        /// <summary>
        /// SetSite method of IOleObjectWithSite
        /// </summary>
        /// <param name="pUnkSite">site for this object to use</param>
        public virtual void SetSite(object pUnkSite) {
            site = pUnkSite;
            codeDomProvider = null;
            serviceProvider = null;
        }

        /// <summary>
        /// GetSite method of IOleObjectWithSite
        /// </summary>
        /// <param name="riid">interface to get</param>
        /// <param name="ppvSite">array in which to stuff return value</param>
        public virtual void GetSite(ref Guid riid, object[] ppvSite) {

            if (ppvSite == null) {
                throw new ArgumentNullException("ppvSite");
            }
            if (ppvSite.Length < 1) {
                throw new ArgumentException("ppvSite array must have at least 1 member", "ppvSite");
            }

            if (site == null) {
                throw new COMException("object is not sited", Helpers.E_FAIL);
            }

            IntPtr pUnknownPointer = Marshal.GetIUnknownForObject(site);
            IntPtr intPointer = IntPtr.Zero;
            Marshal.QueryInterface(pUnknownPointer, ref riid, out intPointer);

            if (intPointer == IntPtr.Zero) {
                throw new COMException("site does not support requested interface", Helpers.E_NOINTERFACE);
            }

            ppvSite[0] = Marshal.GetObjectForIUnknown(intPointer);
        }

        /// <summary>
        /// gets a string containing the DLL names to add.
        /// </summary>
        /// <param name="DLLToAdd"></param>
        /// <returns></returns>
        private string GetDLLNames(string[] DLLToAdd) {

            if (DLLToAdd == null || DLLToAdd.Length == 0) {
                return string.Empty;
            }

            string dllNames = DLLToAdd[0];
            for (int i = 1; i < DLLToAdd.Length; i++) {
                dllNames = dllNames + ", " + DLLToAdd[i];
            }
            return dllNames;
        }

        /// <summary>
        /// adds a reference to the project for each required DLL
        /// </summary>
        /// <param name="referenceDLL"></param>
        protected void AddReferenceDLLToProject(string[] referenceDLL) {

            if (referenceDLL.Length == 0) {
                return;
            }

            object serviceObject = GetService(typeof(ProjectItem));
            Debug.Assert(serviceObject != null, "Unable to get Project Item.");
            if (serviceObject == null) {
                string errorMessage = String.Format("Unable to add DLL to project references: {0}.  Please Add them manually.", GetDLLNames(referenceDLL));
                GeneratorErrorCallback(false, 1, errorMessage, 0, 0);
                return;
            }

            Project containingProject = ((ProjectItem)serviceObject).ContainingProject;
            Debug.Assert(containingProject != null, "GetService(typeof(Project)) return null.");
            if (containingProject == null) {
                string errorMessage = String.Format("Unable to add DLL to project references: {0}.  Please Add them manually.", GetDLLNames(referenceDLL));
                GeneratorErrorCallback(false, 1, errorMessage, 0, 0);
                return;
            }

            VSProject vsProj = containingProject.Object as VSProject;
            Debug.Assert(vsProj != null, "Unable to ADD DLL to current project.  Project.Object does not implement VSProject.");
            if (vsProj == null) {
                string errorMessage = String.Format("Unable to add DLL to project references: {0}.  Please Add them manually.", GetDLLNames(referenceDLL));
                GeneratorErrorCallback(false, 1, errorMessage, 0, 0);
                return;
            }

            try {
                for (int i = 0; i < referenceDLL.Length; i++) {
                    vsProj.References.Add(referenceDLL[i]);
                }
            }
            catch (Exception e) {
                Debug.Fail("**** ERROR: vsProj.References.Add() throws exception: " + e.ToString());

                string errorMessage = String.Format("Unable to add DLL to project references: {0}.  Please Add them manually.", GetDLLNames(referenceDLL));
                GeneratorErrorCallback(false, 1, errorMessage, 0, 0);
                return;
            }
        }

        /// <summary>
        /// method to create an exception message given an exception
        /// </summary>
        /// <param name="e">exception caught</param>
        /// <returns>message to display to the user</returns>
        protected virtual string CreateExceptionMessage(Exception e) {

            string message = (e.Message != null ? e.Message : string.Empty);

            Exception innerException = e.InnerException;
            while (innerException != null) {
                string innerMessage = innerException.Message;
                if (innerMessage != null && innerMessage.Length > 0) {
                    message = message + " " + innerMessage;
                }
                innerException = innerException.InnerException;
            }

            return message;
        }

        /// <summary>
        /// method to create a version comment
        /// </summary>
        /// <param name="codeNamespace"></param>
        protected virtual void GenerateVersionComment(System.CodeDom.CodeNamespace codeNamespace) {
            codeNamespace.Comments.Add(new CodeCommentStatement(string.Empty));
            codeNamespace.Comments.Add(new CodeCommentStatement(String.Format("This source code was auto-generated by {0}, Version {1}.",
                                       System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                       System.Environment.Version.ToString())));
            codeNamespace.Comments.Add(new CodeCommentStatement(string.Empty));
        }

    }
}