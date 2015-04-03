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
using System.Threading;

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
        public override object Create(object parent, object context, System.Xml.XmlNode section) {
            var array = parent as ArrayList;

            var paths = section.Attributes.RemoveNamedItem("root").GetValueOrNull()
                ?? AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;

            if (section.Attributes.Count > 0)
                throw new ConfigurationErrorsException(String.Format(Resources.UnrecognizedAttributeFormat, section.Attributes[0]), section);

            string applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var directories = new List<string>();
            directories.Add(applicationBase);

            foreach (var path in paths.Split(';')) {
                var directory = Path.Combine(applicationBase, path);
                if (Directory.Exists(directory))
                    directories.Add(directory);
            }

            var cache = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            if (array != null) {
                var adapter = new ReadOnlyCollectionOfObjectAdapter(array).AsReadOnly(x => x.ToString());
                cache.UnionWith(adapter);
            }

            string value;
            foreach (var node in section.ChildNodes.OfType<XmlElement>()) {
                switch (node.Name) {
                case "add":
                    value = node.RemoveAttributeNode(ValueAttributeName).Value;
                    foreach (string filename in directories.SelectMany(d => Directory.GetFiles(d, value))) {
                        cache.Add(filename);
                    }
                    break;
                case "remove":
                    value = node.RemoveAttributeNode(ValueAttributeName).Value;
                    foreach (string filename in directories.SelectMany(d => Directory.GetFiles(d, value))) {
                        cache.Remove(filename);
                    }
                    break;
                case "clear":
                    cache.Clear();
                    break;
                default:
                    throw new ConfigurationErrorsException(String.Format(Resources.UnrecognizedElementFormat, node.Name), node);
                }
            }

            return new ArrayList(cache.Count)
                .AddRange(cache);
        }

        #endregion
    }
}
