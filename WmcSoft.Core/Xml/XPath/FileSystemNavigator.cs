using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace WmcSoft.Xml.XPath
{
    public class FileSystemNavigator : XPathNavigator
    {
        #region class NavigatorAdapter
        abstract class NavigatorAdapter : ICloneable
        {
            protected readonly string[] names;

            protected NavigatorAdapter(string[] names) {
                this.names = names;
            }

            public virtual string GetElementName() {
                return names[0];
            }

            public abstract int GetAttributeCount();
            public virtual string GetAttributeName(int index) {
                return names[index + 1];
            }
            public abstract string GetAttributeValue(FileSystemInfo value, int index);

            public abstract FileSystemInfo[] GetChildren(FileSystemInfo value);

            public abstract FileSystemInfo GetParent(FileSystemInfo value);

            #region ICloneable Members

            public object Clone() {
                return MemberwiseClone();
            }

            #endregion
        }

        class DirectoryInfoNavigatorAdapter : NavigatorAdapter
        {
            public DirectoryInfoNavigatorAdapter(XmlNameTable nameTable)
                : base(new string[] {
					nameTable.Add("directory"),
					nameTable.Add("name"),
					nameTable.Add("creationTime"),
					nameTable.Add("lastWriteTime"),
					nameTable.Add("lastAccessTime"),
					nameTable.Add("attributes")
				}) {
            }

            public override int GetAttributeCount() {
                return 5;
            }

            public override string GetAttributeValue(FileSystemInfo value, int index) {
                DirectoryInfo directoryInfo = (DirectoryInfo)value;
                switch (index) {
                    case 0:
                        return directoryInfo.Name;
                    case 1:
                        return directoryInfo.CreationTimeUtc.ToString("s");
                    case 2:
                        return directoryInfo.LastWriteTimeUtc.ToString("s");
                    case 3:
                        return directoryInfo.LastAccessTimeUtc.ToString("s");
                    case 4:
                        return directoryInfo.Attributes.ToString();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public override FileSystemInfo[] GetChildren(FileSystemInfo value) {
                DirectoryInfo directoryInfo = (DirectoryInfo)value;
                return directoryInfo.GetFileSystemInfos();
            }

            public override FileSystemInfo GetParent(FileSystemInfo value) {
                DirectoryInfo directoryInfo = (DirectoryInfo)value;
                return directoryInfo.Parent;
            }

        }

        class FileInfoNavigatorAdapter : NavigatorAdapter
        {

            public FileInfoNavigatorAdapter(XmlNameTable nameTable)
                : base(new string[] {
					nameTable.Add("file"),
					nameTable.Add("name"),
					nameTable.Add("creationTime"),
					nameTable.Add("lastWriteTime"),
					nameTable.Add("lastAccessTime"),
					nameTable.Add("attributes"),
					nameTable.Add("length")
				}) {
            }

            public override int GetAttributeCount() {
                return 6;
            }

            public override string GetAttributeValue(FileSystemInfo value, int index) {
                FileInfo fileInfo = (FileInfo)value;
                switch (index) {
                    case 0:
                        return fileInfo.Name;
                    case 1:
                        return fileInfo.CreationTimeUtc.ToString("s");
                    case 2:
                        return fileInfo.LastWriteTimeUtc.ToString("s");
                    case 3:
                        return fileInfo.LastAccessTimeUtc.ToString("s");
                    case 4:
                        return fileInfo.Attributes.ToString();
                    case 5:
                        return fileInfo.Length.ToString();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            static readonly FileSystemInfo[] empty = new FileSystemInfo[0];
            public override FileSystemInfo[] GetChildren(FileSystemInfo value) {
                return empty;
            }

            public override FileSystemInfo GetParent(FileSystemInfo value) {
                FileInfo fileInfo = (FileInfo)value;
                return fileInfo.Directory;
            }

        }

        #endregion

        #region class NavigatorState

        class NavigatorState : ICloneable
        {

            public XPathNodeType nodeType;
            public int attributeIndex; // -1 when type is not Attribute
            public int indexInParent;
            public FileSystemInfo parent;
            public FileSystemInfo[] sibblings;
            public FileSystemInfo[] children;

            public readonly FileSystemInfo root;
            public readonly XmlNameTable nameTable;

            public NavigatorState(FileSystemInfo root, XmlNameTable nameTable) {
                this.nameTable = nameTable;
                this.root = root;

                nodeType = XPathNodeType.Root;

                adapters = new NavigatorAdapter[] {
					new DirectoryInfoNavigatorAdapter(nameTable),
					new FileInfoNavigatorAdapter(nameTable)
				};

                parent = null;
                sibblings = null;
                children = new FileSystemInfo[1] { root };
                indexInParent = 0;
                attributeIndex = -1;
            }

            #region Adapt method

            NavigatorAdapter[] adapters;

            [DebuggerStepThrough]
            public NavigatorAdapter Adapt(FileSystemInfo fileSystemInfo) {
                if (fileSystemInfo is FileInfo)
                    return adapters[1];
                else
                    return adapters[0];
            }
            [DebuggerStepThrough]
            public NavigatorAdapter Adapt(DirectoryInfo directoryInfo) {
                return adapters[0];
            }
            [DebuggerStepThrough]
            public NavigatorAdapter Adapt(FileInfo fileInfo) {
                return adapters[1];
            }

            #endregion

            #region Delegate calls to the adapter

            public string GetElementName() {
                return Adapt(sibblings[indexInParent]).GetElementName();
            }

            public int GetAttributeCount() {
                return Adapt(sibblings[indexInParent]).GetAttributeCount();
            }

            public string GetAttributeName() {
                return Adapt(sibblings[indexInParent]).GetAttributeName(attributeIndex);
            }

            public string GetAttributeValue() {
                return Adapt(sibblings[indexInParent]).GetAttributeValue(sibblings[indexInParent], attributeIndex);
            }

            public int GetChildCount() {
                if (children == null)
                    children = Adapt(sibblings[indexInParent]).GetChildren(sibblings[indexInParent]);
                return children.Length;
            }

            #endregion

            #region Moving methods

            public bool MoveTo(int index) {
                if (index < 0 || index >= sibblings.Length)
                    return false;

                attributeIndex = -1;
                indexInParent = index;
                nodeType = XPathNodeType.Element;
                return true;
            }

            public bool MoveToChild(int index) {
                if (index < 0 || index >= GetChildCount())
                    return false;

                if (sibblings != null)
                    parent = sibblings[indexInParent];
                else
                    parent = null; // root
                sibblings = children;
                children = null;
                attributeIndex = -1;
                indexInParent = index;
                nodeType = XPathNodeType.Element;
                return true;
            }

            public bool MoveToParent() {
                if (sibblings == null) {
                    return false; // already at the root is the root
                } else if (parent == null) {
                    attributeIndex = -1;
                    indexInParent = -1;
                    parent = null;
                    children = sibblings;
                    sibblings = null;
                    nodeType = XPathNodeType.Root;
                    return true;
                } else if (parent.FullName == root.FullName) {
                    attributeIndex = -1;
                    indexInParent = 0;
                    parent = null;
                    sibblings = new FileSystemInfo[1] { root };
                    children = null;
                    nodeType = XPathNodeType.Element;
                    return true;
                } else {
                    FileSystemInfo current = sibblings[indexInParent];

                    // look for the parent in the grand-parent children
                    FileSystemInfo grandParent = Adapt(parent).GetParent(parent);
                    FileSystemInfo[] uncles = Adapt(grandParent).GetChildren(grandParent);
                    for (int i = 0; i != uncles.Length; ++i) {
                        if (uncles[i].FullName == parent.FullName) { // found the current 
                            attributeIndex = -1;
                            indexInParent = i;
                            nodeType = XPathNodeType.Element;
                            parent = grandParent;
                            sibblings = uncles;
                            children = null;
                            return true;
                        }
                    }
                }

                // did not moved
                return false;
            }

            #endregion

            #region ICloneable Members

            public object Clone() {
                NavigatorState clone = (NavigatorState)this.MemberwiseClone();
                return clone;
            }

            #endregion

        }
        #endregion

        #region Private Fields

        private NavigatorState state;
        private System.Uri baseURI;

        #endregion

        #region Lifecycle

        private FileSystemNavigator() {
        }

        public FileSystemNavigator(string path)
            : this(new Uri(path), new NameTable()) {
        }
        public FileSystemNavigator(string path, XmlNameTable nameTable)
            : this(new Uri(path), nameTable) {
        }

        public FileSystemNavigator(Uri uri)
            : this(uri, new NameTable()) {
        }

        public FileSystemNavigator(Uri uri, XmlNameTable nameTable) {
            // get the file system information
            FileSystemInfo fileSystemInfo;
            string localPath = uri.LocalPath;
            if (File.Exists(localPath)) {
                fileSystemInfo = new FileInfo(localPath);
            } else if (Directory.Exists(localPath)) {
                fileSystemInfo = new DirectoryInfo(localPath);
            } else {
                throw new NotSupportedException();
            }

            // set the base uri
            this.baseURI = uri;

            // initialize nametable
            nameTable.Add(String.Empty);
            nameTable.Add(uri.ToString());
            nameTable.Add("file");
            nameTable.Add("name");
            nameTable.Add("creationTime");
            nameTable.Add("lastWriteTime");
            nameTable.Add("lastAccessTime");
            nameTable.Add("attributes");
            nameTable.Add("length");

            // set the current state
            this.state = new NavigatorState(fileSystemInfo, nameTable);
        }

        public override XPathNavigator Clone() {
            FileSystemNavigator clone = new FileSystemNavigator();
            clone.baseURI = this.baseURI;
            clone.state = (NavigatorState)this.state.Clone();
            return clone;
        }

        #endregion

        #region Global properties

        public override XmlNameTable NameTable {
            get {
                return state.nameTable;
            }
        }

        public override string BaseURI {
            get {
                return state.nameTable.Get(baseURI.ToString());
            }
        }

        #endregion

        #region Current state properties

        public override XPathNodeType NodeType {
            [DebuggerStepThrough]
            get {
                return state.nodeType;
            }
        }

        public override string NamespaceURI {
            [DebuggerStepThrough]
            get {
                return String.Empty;
            }
        }

        public override string Prefix {
            [DebuggerStepThrough]
            get {
                return String.Empty;
            }
        }

        public override string Name {
            [DebuggerStepThrough]
            get {
                return this.LocalName;
            }
        }

        public override string LocalName {
            get {
                switch (this.NodeType) {
                    case XPathNodeType.Element:
                        return state.GetElementName();
                    case XPathNodeType.Attribute:
                        return state.GetAttributeName();
                    case XPathNodeType.Root:
                    case XPathNodeType.Namespace:
                    case XPathNodeType.ProcessingInstruction:
                    default:
                        return String.Empty;
                }
            }
        }

        public override string Value {
            get {
                switch (this.NodeType) {
                    case XPathNodeType.Attribute:
                        return state.GetAttributeValue();
                    case XPathNodeType.Root:
                    case XPathNodeType.Element:
                    default:
                        return String.Empty;
                }
            }
        }

        public override bool IsEmptyElement {
            get {
                XPathNodeType nodeType = this.NodeType;
                if ((nodeType == XPathNodeType.Element)
                    || (nodeType == XPathNodeType.Root)) {
                    return (this.state.GetChildCount() == 0);
                }
                return false;
            }
        }

        public override bool IsSamePosition(XPathNavigator other) {
            if (this.GetType() != other.GetType())
                return false;

            FileSystemNavigator navigator = (FileSystemNavigator)other;
            if (this.state.root != navigator.state.root)
                return false;

            return (this.state.nodeType == navigator.state.nodeType)
                && (this.state.attributeIndex == navigator.state.attributeIndex)
                && (this.state.parent == navigator.state.parent)
                && (this.state.indexInParent == navigator.state.indexInParent);
        }

        #endregion

        #region Move methods

        public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope) {
            return false;
        }

        public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope) {
            return false;
        }

        public override bool MoveToId(string id) {
            return false;
        }

        public override bool MoveTo(XPathNavigator other) {
            if (this.GetType() != other.GetType())
                return false;

            FileSystemNavigator navigator = (FileSystemNavigator)other;
            if (this.state.root != navigator.state.root)
                return false;

            this.state.nodeType = navigator.state.nodeType;
            this.state.attributeIndex = navigator.state.attributeIndex;
            this.state.indexInParent = navigator.state.indexInParent;
            this.state.parent = navigator.state.parent;
            this.state.sibblings = navigator.state.sibblings;
            this.state.children = navigator.state.children;

            return true;
        }

        public override bool MoveToNext() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType == XPathNodeType.Root
                || nodeType == XPathNodeType.Attribute)
                return false;

            if (nodeType == XPathNodeType.Element) {
                return state.MoveTo(state.indexInParent + 1);
            }

            return false;
        }

        public override bool MoveToPrevious() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType == XPathNodeType.Root
                || nodeType == XPathNodeType.Attribute)
                return false;

            if (nodeType == XPathNodeType.Element) {
                return state.MoveTo(state.indexInParent - 1);
            }

            return false;
        }

        public override bool MoveToFirstChild() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType != XPathNodeType.Root && nodeType != XPathNodeType.Element)
                return false;

            return state.MoveToChild(0);
        }

        public override bool MoveToFirstAttribute() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType != XPathNodeType.Element)
                return false;

            if (state.GetAttributeCount() > 0) {
                state.nodeType = XPathNodeType.Attribute;
                state.attributeIndex = 0;
                return true;
            }

            return false;
        }

        public override bool MoveToNextAttribute() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType != XPathNodeType.Attribute
                && nodeType != XPathNodeType.Element)
                return false;

            if (state.attributeIndex + 1 < state.GetAttributeCount()) {
                state.nodeType = XPathNodeType.Attribute;
                ++state.attributeIndex;
                return true;
            }

            return false;
        }

        public override bool MoveToParent() {
            XPathNodeType nodeType = this.NodeType;
            if (nodeType == XPathNodeType.Root)
                return false;
            else if (nodeType == XPathNodeType.Attribute) {
                state.attributeIndex = -1;
                state.nodeType = XPathNodeType.Element;
                return true;
            }

            return state.MoveToParent();
        }

        #endregion
    }
}
