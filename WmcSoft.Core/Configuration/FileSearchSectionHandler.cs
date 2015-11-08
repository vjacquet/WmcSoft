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
using System.IO;
using System.Xml;
using WmcSoft.Collections;
using WmcSoft.Collections.Generic;
using WmcSoft.Properties;
using WmcSoft.Xml;
using System.Collections.Generic;

namespace WmcSoft.Configuration
{
    /// <summary>
    /// Creates a list of file based on the inclusion (add) and exclusion filter (remove).
    /// </summary>
    public sealed class FileSearchSectionHandler : ListSectionHandler
    {
        public FileSearchSectionHandler() {
        }

        /// <summary>
        /// Gets the name of the value tag.
        /// </summary>
        protected override string ValueAttributeName {
            get { return "match"; }
        }

        #region Membres de IConfigurationSectionHandler

        /// <summary>
        /// Evaluates the given XML section and returns an 
        /// <see cref="System.Collections.ArrayList"/> that contains the results of the evaluation.
        /// </summary>
        /// <param name="parent">The configuration settings in a 
        /// corresponding parent configuration section.</param>
        /// <param name="configContext">
        /// An <see cref="System.Web.Configuration.HttpConfigurationContext"/> 
        /// when Create is called from the ASP.NET configuration system. 
        /// Otherwise, this parameter is reserved and is a null reference.
        /// </param>
        /// <param name="section">The <see cref="System.Xml.XmlNode"/> 
        /// that contains the configuration information to be handled. 
        /// Provides direct access to the XML contents of the configuration section.</param>
        /// <returns>A <see cref="System.Collections.ArrayList"/> that 
        /// contains the section's configuration settings.</returns>
        public override object Create(object parent, object context, XmlNode section) {
            var paths = section.Attributes.RemoveNamedItem("root").GetValueOrNull()
                ?? AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;

            if (section.Attributes.Count > 0)
                throw new ConfigurationErrorsException(String.Format(Resources.UnrecognizedAttributeFormat, section.Attributes[0]), section);

            string applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var directories = new List<string> { applicationBase };

            foreach (var path in paths.Split(';')) {
                var directory = Path.Combine(applicationBase, path);
                if (Directory.Exists(directory))
                    directories.Add(directory);
            }

            var cache = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            var array = parent as ArrayList;
            if (array != null) {
                // this complex expression is to ensure the HashSet knows the count 
                // of items without any array reallocation
                var adapter = new ReadOnlyCollectionOfObjectAdapter(array).AsReadOnly(x => x.ToString());
                cache.UnionWith(adapter);
            }

            string value;
            foreach (var node in section.ChildNodes.OfType<XmlElement>()) {
                switch (node.Name) {
                case "add":
                    value = node.RemoveAttributeNode(ValueAttributeName).Value;
                    cache.UnionWith(directories.SelectMany(d => Directory.GetFiles(d, value)));
                    break;
                case "remove":
                    value = node.RemoveAttributeNode(ValueAttributeName).Value;
                    cache.ExceptWith(directories.SelectMany(d => Directory.GetFiles(d, value)));
                    break;
                case "clear":
                    cache.Clear();
                    break;
                default:
                    throw new ConfigurationErrorsException(String.Format(Resources.UnrecognizedElementFormat, node.Name), node);
                }
            }

            return cache.ToArrayList();
        }

        #endregion
    }
}
