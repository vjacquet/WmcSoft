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
using System.Collections;
using System.Linq;
using System.Configuration;
using System.Xml;
using WmcSoft.Collections;
using WmcSoft.Properties;
using WmcSoft.Xml;

namespace WmcSoft.Configuration
{

    /// <summary>
    /// Provides values configuration information from a configuration 
    /// section.
    /// </summary>
    public class ListSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="ListSectionHandler"/> class.
        /// </summary>
        public ListSectionHandler() {
        }

        /// <summary>
        /// Gets the name of the value tag.
        /// </summary>
        /// <value>The value of the key.</value>
        /// <remarks>This property may be overridden by derived classes 
        /// to change the name of the value tag. 
        /// The default is "value".</remarks>
        protected virtual string ValueAttributeName {
            get { return "value"; }
        }

        #region Membres de IConfigurationSectionHandler

        /// <summary>
        /// Evaluates the given XML section and returns an <see cref="System.Collections.ArrayList"/> that 
        /// contains the results of the evaluation.
        /// </summary>
        /// <param name="parent">The configuration settings in a corresponding parent configuration section.</param>
        /// <param name="configContext">
        /// An <see cref="System.Web.Configuration.HttpConfigurationContext"/> 
        /// when Create is called from the ASP.NET configuration system. 
        /// Otherwise, this parameter is reserved and is a null reference.
        /// </param>
        /// <param name="section">The <see cref="System.Xml.XmlNode"/> that contains the configuration information to be handled. 
        /// Provides direct access to the XML contents of the configuration section.</param>
        /// <returns>A <see cref="System.Collections.ArrayList"/> that contains the section's configuration settings.</returns>
        public virtual object Create(object parent, object configContext, XmlNode section) {
            if (section.Attributes.Count > 0)
                throw new ConfigurationErrorsException(String.Format(Resources.UnrecognizedAttributeFormat, section.Attributes[0]), section);

            var array = (parent == null) ? new ArrayList() : new ArrayList((ArrayList)parent);
            foreach (var node in section.ChildNodes.OfType<XmlElement>()) {
                switch (node.Name) {
                case "add":
                    array.Ensure(node.RemoveAttributeNode(ValueAttributeName).Value);
                    break;
                case "remove":
                    array.Remove(node.RemoveAttributeNode(ValueAttributeName).Value);
                    break;
                case "clear":
                    array.Clear();
                    break;
                default:
                    throw new ConfigurationErrorsException(String.Format(Resources.UnrecognizedElementFormat, node.Name), node);
                }
            }
            return array;
        }

        #endregion
    }
}
