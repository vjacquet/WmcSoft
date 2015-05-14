// Inspired from Mvp.Xml

using System;
using System.Collections.Generic;
using System.Text;

namespace WmcSoft.CustomTools.Design
{
    /// <summary>
    /// Specifies custom tool registration information.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomToolAttribute : Attribute
    {
        string name;
        bool code;

        /// <summary>
        /// Assigns custom tool information to the class.
        /// </summary>
        /// <param name="name">Name of the custom tool.</param>
        /// <param name="generatesDesignTimeCode">
        /// If <see langword="true" />, the IDE will try to compile on the fly the 
        /// dependent the file associated with this tool, and make it available 
        /// through intellisense to the rest of the project.
        /// </param>
        public CustomToolAttribute(string name, bool generatesDesignTimeCode) {
            this.name = name;
            code = generatesDesignTimeCode;
        }

        /// <summary>
        /// Name of the custom tool.
        /// </summary>
        public string Name {
            get {
                return name;
            }
        }

        /// <summary>
        /// Specifies whether the tool generates design time code to compile on the fly.
        /// </summary>
        public bool GeneratesDesignTimeCode {
            get {
                return code;
            }
        }
    }
}
