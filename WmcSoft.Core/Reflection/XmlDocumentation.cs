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
using System.Xml;

namespace WmcSoft.Reflection
{
    /// <summary>Used to retrieve the XML comments for MemberInfo objects.</summary>
    [System.Diagnostics.DebuggerDisplay("{Summary == null ? string.Empty : Summary.InnerText,nq}")]
    public class XmlDocumentation
    {
        #region Extracting Specific Comments

        /// <summary>Gets the entire XML comment block for this member.</summary>
        public XmlNode AllComments { get; }
        /// <summary>Gets the summary comment for this member.</summary>
        public XmlNode Summary { get; }
        /// <summary>Gets the remarks comment for this member.</summary>
        public XmlNode Remarks { get; }
        /// <summary>Gets the return comment for this member.</summary>
        public XmlNode Returns { get; }
        /// <summary>Gets the value comment for this member.</summary>
        public XmlNode Value { get; }
        /// <summary>Gets the example comment for this member.</summary>
        public XmlNode Example { get; }
        /// <summary>Gets the includes comments for this member.</summary>
        public XmlNodeList Includes { get; }
        /// <summary>Gets the exceptions comments for this member.</summary>
        public XmlNodeList Exceptions { get; }
        /// <summary>Gets the paramrefs comments for this member.</summary>
        public XmlNodeList ParamRefs { get; }
        /// <summary>Gets the permissions comments for this member.</summary>
        public XmlNodeList Permissions { get; }
        /// <summary>Gets the params comments for this member.</summary>
        public XmlNodeList Params { get; }
        /// <summary>Renders to a string the entire XML comment block for this member.</summary>
        public override string ToString() { return AllComments.OuterXml; }

        #endregion

        #region Construction

        /// <summary>Initializes the XML comments for the specified member.</summary>
        /// <param name="comments">The XML comments.</param>
        internal XmlDocumentation(XmlNode comments) {
            // Get the comments.  If we got any, parse out the important stuff.
            AllComments = comments;
            if (AllComments != null) {
                // Get single nodes (comments that can appear only once)
                Summary = Normalize(comments.SelectSingleNode("summary"));
                Returns = Normalize(comments.SelectSingleNode("returns"));
                Remarks = Normalize(comments.SelectSingleNode("remarks"));
                Example = Normalize(comments.SelectSingleNode("example"));
                Value = Normalize(comments.SelectSingleNode("value"));

                // Get node lists (comments that can appear multiple times)
                Includes = Normalize(comments.SelectNodes("include"));
                Exceptions = Normalize(comments.SelectNodes("exception"));
                ParamRefs = Normalize(comments.SelectNodes("paramref"));
                Permissions = Normalize(comments.SelectNodes("permission"));
                Params = Normalize(comments.SelectNodes("param"));
            } else {
                // Make it easier for people to use this class when no comments exist
                // by creating dummy nodes for all properties.
                AllComments = new XmlDocument();
                Summary = Returns = Remarks = Example = Value = null;
                Includes = Exceptions = ParamRefs = Permissions = Params = AllComments.ChildNodes;
            }
        }

        #endregion

        XmlNode Normalize(XmlNode node) {
            var element = node as XmlElement;
            if (element != null) {
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
