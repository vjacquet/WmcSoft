// Inspired from Mvp.Xml

using System;

namespace WmcSoft.CustomTools.Design
{
    /// <summary>
    /// Specifies custom tool registration information.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomToolAttribute : Attribute
    {
        readonly string _name;
        readonly bool _code;

        /// <summary>
        /// Assigns custom tool information to the class.
        /// </summary>
        /// <param name="name">Name of the custom tool.</param>
        /// <param name="generatesDesignTimeCode">
        /// If <see langword="true" />, the IDE will try to compile on the fly the 
        /// dependent the file associated with this tool, and make it available 
        /// through intellisense to the rest of the project.
        /// </param>
        public CustomToolAttribute(string name, bool generatesDesignTimeCode)
        {
            _name = name;
            _code = generatesDesignTimeCode;
        }

        /// <summary>
        /// Name of the custom tool.
        /// </summary>
        public string Name {
            get {
                return _name;
            }
        }

        /// <summary>
        /// Specifies whether the tool generates design time code to compile on the fly.
        /// </summary>
        public bool GeneratesDesignTimeCode {
            get {
                return _code;
            }
        }
    }
}
