// Stephen Toub
// stoub@microsoft.com
//
// XmlComments.cs
// Retrieve the xml comments stored in the assembly's comments file
// for specific types or members of types.
// ==========================================================================
// <mailto:vjacquet@club-internet.fr>
// * Moving loading function to DocumentationLoader to avoid memory
//   consumption.

namespace WmcSoft.Reflection
{
    using System.Xml;

    /// <summary>Used to retrieve the XML comments for MemberInfo objects.</summary>
    public class XmlDocumentation
    {

        #region Member Variables
        /// <summary>The entire XML comment block for this member.</summary>
        private XmlNode _comments;
        /// <summary>The summary comment for this member.</summary>
        private XmlNode _summary;
        /// <summary>The remarks comment for this member.</summary>
        private XmlNode _remarks;
        /// <summary>The returns comment for this member.</summary>
        private XmlNode _returns;
        /// <summary>The value comment for this member.</summary>
        private XmlNode _value;
        /// <summary>The example comment for this member.</summary>
        private XmlNode _example;
        /// <summary>The includes comments for this member.</summary>
        private XmlNodeList _includes;
        /// <summary>The exceptions comments for this member.</summary>
        private XmlNodeList _exceptions;
        /// <summary>The paramrefs comments for this member.</summary>
        private XmlNodeList _paramrefs;
        /// <summary>The permissions comments for this member.</summary>
        private XmlNodeList _permissions;
        /// <summary>The params comments for this member.</summary>
        private XmlNodeList _params;
        #endregion

        #region Extracting Specific Comments
        /// <summary>Gets the entire XML comment block for this member.</summary>
        public XmlNode AllComments { get { return _comments; } }
        /// <summary>Gets the summary comment for this member.</summary>
        public XmlNode Summary { get { return _summary; } }
        /// <summary>Gets the remarks comment for this member.</summary>
        public XmlNode Remarks { get { return _remarks; } }
        /// <summary>Gets the return comment for this member.</summary>
        public XmlNode Returns { get { return _returns; } }
        /// <summary>Gets the value comment for this member.</summary>
        public XmlNode Value { get { return _value; } }
        /// <summary>Gets the example comment for this member.</summary>
        public XmlNode Example { get { return _example; } }
        /// <summary>Gets the includes comments for this member.</summary>
        public XmlNodeList Includes { get { return _includes; } }
        /// <summary>Gets the exceptions comments for this member.</summary>
        public XmlNodeList Exceptions { get { return _exceptions; } }
        /// <summary>Gets the paramrefs comments for this member.</summary>
        public XmlNodeList ParamRefs { get { return _paramrefs; } }
        /// <summary>Gets the permissions comments for this member.</summary>
        public XmlNodeList Permissions { get { return _permissions; } }
        /// <summary>Gets the params comments for this member.</summary>
        public XmlNodeList Params { get { return _params; } }
        /// <summary>Renders to a string the entire XML comment block for this member.</summary>
        public override string ToString() { return _comments.OuterXml; }
        #endregion

        #region Construction

        /// <summary>Initializes the XML comments for the specified member.</summary>
        /// <param name="comments">The XML comments.</param>
        internal XmlDocumentation(XmlNode comments) {
            // Get the comments.  If we got any, parse out the important stuff.
            _comments = comments;
            if (_comments != null) {
                // Get single nodes (comments that can appear only once)
                _summary = Normalize(_comments.SelectSingleNode("summary"));
                _returns = Normalize(_comments.SelectSingleNode("returns"));
                _remarks = Normalize(_comments.SelectSingleNode("remarks"));
                _example = Normalize(_comments.SelectSingleNode("example"));
                _value = Normalize(_comments.SelectSingleNode("value"));

                // Get node lists (comments that can appear multiple times)
                _includes = Normalize(_comments.SelectNodes("include"));
                _exceptions = Normalize(_comments.SelectNodes("exception"));
                _paramrefs = Normalize(_comments.SelectNodes("paramref"));
                _permissions = Normalize(_comments.SelectNodes("permission"));
                _params = Normalize(_comments.SelectNodes("param"));
            } else {
                // Make it easier for people to use this class when no comments exist
                // by creating dummy nodes for all properties.
                _comments = new XmlDocument();
                _summary = _returns = _remarks = _example = _value = null;
                _includes = _exceptions = _paramrefs = _permissions = _params = _comments.ChildNodes;
            }
        }

        #endregion

        XmlNode Normalize(XmlNode node) {
            if (node is XmlElement) {
                var element = (XmlElement)node;
                element.InnerXml = element.InnerXml.Trim();
            }
            return node;
        }

        XmlNodeList Normalize(XmlNodeList nodeList) {
            if (nodeList == null)
                return null;

            foreach (XmlNode node in nodeList) {
                Normalize(node);
            }
            return nodeList;
        }
    }

}
