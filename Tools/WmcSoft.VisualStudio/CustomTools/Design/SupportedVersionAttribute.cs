// Inspired from Mvp.Xml

using System;

namespace WmcSoft.CustomTools.Design
{
    /// <summary>
    /// Determines which versions of VS.NET are supported by the custom tool.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SupportedVersionAttribute : Attribute
    {
        Version _version;

        /// <summary>
        /// Initializes the attribute.
        /// </summary>
        /// <param name="version">Version supported by the tool.</param>
        public SupportedVersionAttribute(string version)
        {
            _version = new Version(version);
        }

        /// <summary>
        /// Version supported by the tool.
        /// </summary>
        public Version Version {
            get {
                return _version;
            }
        }
    }
}
