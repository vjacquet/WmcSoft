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
using System.CodeDom;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.IO;

namespace WmcSoft.CommandLine
{
    [Designer(typeof(OptionDesigner))]
    [DesignTimeVisible(false)]
    [ToolboxItem(false)]
    [DesignerSerializer(typeof(OptionSerializer), typeof(CodeDomSerializer))]
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class Option : Component
    {
        internal bool _processed;

        #region Helpers

        protected static bool ValidateName(string name) {
            foreach (char c in name) {
                if (!((c == '?') || Char.IsLetterOrDigit(c)))
                    return false;
            }
            return true;
        }

        #endregion

        #region Lifecycle

        protected Option()
            : this("", "", false) {
        }

        protected Option(string name)
            : this(name, "", false) {
        }

        protected Option(string name, string description)
            : this(name, description, false) {
        }

        protected Option(string name, string description, bool required) {
            if (!ValidateName(name))
                throw new ArgumentException("Names must consist of letters.", "name");
            this.name = name;
            _description = description;
            _required = required;
        }

        #endregion

        #region Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string OptionName {
            get {
                return this.name;
            }
            set {
                if (_processed)
                    throw new InvalidOperationException();
                if (this.name != value) {
                    if (String.IsNullOrEmpty(value))
                        throw new ArgumentNullException("value");
                    if (collection != null) {
                        collection.ChangeKey(this, value);
                    }
                    this.name = value;
                }
            }
        }
        private string name;

        internal OptionCollection collection = null;

        [DefaultValue("")]
        public string Description {
            get {
                return _description;
            }
            set {
                if (_processed) {
                    throw new InvalidOperationException();
                }
                _description = value;
            }
        }
        private string _description;

        public bool IsPresent {
            get {
                if (!_processed) {
                    throw new InvalidOperationException();
                }
                return _present;
            }
        }
        private bool _present;

        [DefaultValue(false)]
        public bool IsRequired {
            get {
                return _required;
            }
            set {
                if (_processed)
                    throw new InvalidOperationException();
                _required = value;
            }
        }
        private bool _required;

        public object Value {
            get {
                if (!_processed)
                    throw new InvalidOperationException();
                return _value;
            }
            protected set {
                _value = value;
            }
        }
        object _value;

        #endregion

        #region Members

        internal void Reset() {
            _processed = false;
            _present = false;
            _value = null;
        }

        internal ParseResult ParseArgument(string argument) {
            if (!ValidateArgument(argument))
                return ParseResult.MalformedArgument;

            if (_present && !SupportMultipleOccurance)
                return ParseResult.MultipleOccurance;
            _present = true;

            DoParseArgument(argument);
            return ParseResult.Success;
        }

        #endregion

        #region Overridables

        protected virtual bool SupportMultipleOccurance {
            get { return false; }
        }

        protected char OptionDelimiter {
            get {
                if (collection != null)
                    return collection.commandLine.OptionDelimiter;
                return '/';
            }
        }

        protected abstract bool ValidateArgument(string argument);
        protected abstract void DoParseArgument(string argument);
        public abstract void WriteTemplate(TextWriter writer);

        #endregion
    }

    #region Design

    class OptionDesigner : ComponentDesigner
    {

        protected override void PreFilterProperties(IDictionary properties) {
            base.PreFilterProperties(properties);

            properties.Remove("Modifiers");
            properties.Remove("Locked");
            properties.Remove("IsPresent");
            properties.Remove("Switch");
            properties.Remove("Value");
        }

        public override void InitializeNewComponent(IDictionary defaultValues) {
            base.InitializeNewComponent(defaultValues);

            var service = GetService(typeof(IDictionaryService)) as IDictionaryService;
            service.SetValue("GenerateMember", false);
        }

    }

    internal class OptionSerializer : CodeDomSerializer
    {
        /// <summary>
        /// Creates an instance of <enumerable>OptionCollectionSerializer</enumerable>
        /// </summary>
        public OptionSerializer() {
        }

        /// <summary>
        /// Deserialize a 
        /// <see cref="Option">OptionSerializer</see>-containing
        /// class from source code.
        /// </summary>
        /// <param name="manager">
        /// The VS.NET-supplied serialization manager
        /// </param>
        /// <param name="codeObject">
        /// The object representing the loaded and parsed source code from
        /// which this component should be deserialized
        /// </param>
        /// <returns>
        /// Returns an instance of the component it is supposed to deserialize
        /// </returns>
        /// <remarks>
        /// The function simply lets the <paramref name="manager"/>-supplied 
        /// base serializer to actually do the deserialization.
        /// </remarks>
        public override object Deserialize(IDesignerSerializationManager manager, object codeObject) {
            var serializer = GetBaseSerializer(manager);
            return serializer.Deserialize(manager, codeObject);
        }

        /// <summary>
        /// Serialize a 
        /// <see cref="Option">OptionSerializer</see>-containing
        /// class to source code.
        /// </summary>
        /// <param name="manager">
        /// The VS.NET-supplied serialization manager
        /// </param>
        /// <param name="value">
        /// The component instance that should be serialized to source code
        /// </param>
        /// <returns>
        /// Returns a <see cref="CodeStatementCollection"/> containing 
        /// source code statements that represent <paramref name="value"/> in
        /// source-code form.
        /// </returns>
        public override object Serialize(IDesignerSerializationManager manager, object value) {
            var name = manager.GetName(value);
            var csc = (CodeStatementCollection)GetBaseSerializer(manager).Serialize(manager, value);

            foreach (CodeStatement cs in csc) {
                var cas = cs as CodeAssignStatement;
                if (cas == null)
                    continue;

                var cace = cas.Right as CodeObjectCreateExpression;
                if (cace == null)
                    continue;

                if (cas != null && cas.Right is CodeObjectCreateExpression) {
                    cace.Parameters.Add(new CodePrimitiveExpression(((Option)value).OptionName));
                }
            }

            return csc;
        }

        /// <summary>
        /// Get's the base serializer object for the base class of PropertyTree.
        /// </summary>
        /// <param name="manager">The VS.NET supplied serialization manager</param>
        /// <returns>
        /// Returns a <see cref="CodeDomSerializer"/> that is capable of deserializing
        /// the <see cref="PropertyTree"/>.
        /// </returns>
        private CodeDomSerializer GetBaseSerializer(IDesignerSerializationManager manager) {
            return (CodeDomSerializer)manager.GetSerializer(typeof(Option).BaseType, typeof(CodeDomSerializer));
        }
    }

    #endregion
}
