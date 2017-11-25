using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Design.Serialization;

namespace WmcSoft
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideObject(typeof(CustomTools.CodeGenerator))]
    [ProvideGenerator(typeof(CustomTools.CodeGenerator), "CodeGenerator", "Declarative code generator", "{FAE04EC1-301F-11d3-BF4B-00C04F79EFBC}", true)]
    [DefaultRegistryRoot("SOFTWARE\\Microsoft\\VisualStudio\\12.0")]
    [Guid(GuidList.guidVSPackagePkgString)]
    public sealed class VisualStudio2013Package : Package
    {
    }
}
