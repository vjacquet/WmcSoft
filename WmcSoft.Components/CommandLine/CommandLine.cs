using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.ComponentModel.Design.Serialization;
using System.CodeDom;
using System.Reflection;
using System.Drawing;

namespace WmcSoft.CommandLine
{
    [ToolboxBitmap(typeof(CommandLine))]
    [DefaultProperty("Options")]
    public class CommandLine : Component
    {
        bool processed;
        Dictionary<string, ParseResult> errors = new Dictionary<string, ParseResult>();
        List<string> nonoptions = new List<string>();

        OptionCollection options;

        public CommandLine() {
            processed = false;
            options = new OptionCollection(this);
        }

        public bool ParseArguments() {
            return ParseArguments(Environment.GetCommandLineArgs());
        }

        public bool ParseArguments(params string[] args) {
            return ParseArguments((IEnumerable<string>)args);
        }
        public bool ParseArguments(IEnumerable<string> args) {
            if (processed) {
                foreach (Option option in options) {
                    option.Reset();
                }
            }
            this.DoParseArguments(args);
            processed = true;
            return (errors.Count == 0);
        }

        public IComponent Owner { get; set; }

        [DefaultValue('/')]
        public char OptionDelimiter {
            get { return optionDelimiter; }
            set { optionDelimiter = value; }
        }
        char optionDelimiter = '/';

        private void DoParseArguments(IEnumerable<string> args) {
            foreach (string arg in args) {
                if (arg.Length == 0)
                    continue;
                if (arg[0] == '/') {
                    // option processing
                    // find the named option
                    int index = 1;
                    while (index < arg.Length) {
                        char c = arg[index];
                        if (!Char.IsLetter(c) && (c != '?'))
                            break;
                        index++;
                    }
                    string key = arg.Substring(1, index - 1);
                    string value = arg.Substring(index);
                    Option option = null;

                    // invoke the appropriate logic
                    if (this.options.TryGetValue(key, out option)) {
                        ParseResult result = option.ParseArgument(value);
                        if (result != ParseResult.Success) {
                            this.errors.Add(value, result);
                        }
                    } else {
                        this.errors.Add(value, ParseResult.UnrecognizedOption);
                    }
                } else if (arg[0] == '@') {
                    string path = arg.Substring(1);
                    List<string> responses = new List<string>();
                    using (TextReader reader = File.OpenText(path)) {
                        while (true) {
                            string response = reader.ReadLine();
                            if (response == null)
                                break;
                            responses.Add(response);
                        }
                    }
                    DoParseArguments(responses);
                } else {
                    // non-option processing
                    this.nonoptions.Add(arg);
                }
            }

            // make sure the required arguments were present
            foreach (Option option in this.options) {
                option._processed = true;
                if (!(!option.IsRequired || option.IsPresent)) {
                    this.errors.Add(option.SwitchName, ParseResult.MissingOption);
                }
            }
        }

        public void WriteBanner(TextWriter writer) {
            if (writer == null) {
                throw new ArgumentNullException("writer");
            }
            Assembly application = Assembly.GetCallingAssembly();
            AssemblyName applicationData = application.GetName();
            Console.WriteLine("{0} (v{1})", applicationData.Name, applicationData.Version);
            Object[] copyrightAttributes = application.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);
            foreach (AssemblyCopyrightAttribute copyrightAttribute in copyrightAttributes) {
                writer.WriteLine(copyrightAttribute.Copyright);
            }
        }

        public void WriteUsage(TextWriter writer, bool showOptions) {
            if (writer == null) {
                throw new ArgumentNullException("writer");
            }
            if (!String.IsNullOrEmpty(usage))
                writer.WriteLine(usage);
            if (showOptions)
                WriteOptionSummary(writer);
        }

        public void WriteOptionSummary(TextWriter writer) {
            if (writer == null) {
                throw new ArgumentNullException("writer");
            }
            foreach (Option option in this.options) {
                writer.WriteLine();
                option.WriteTemplate(writer);
                writer.WriteLine(option.Description);
            }
        }

        public void WriteParseErrors(TextWriter writer) {
            if (writer == null) {
                throw new ArgumentNullException("writer");
            }
            foreach (KeyValuePair<string, ParseResult> pair in this.errors) {
                writer.WriteLine("{0}: {1}", pair.Value, pair.Key);
            }
        }

        [EditorAttribute(typeof(OptionCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        public OptionCollection Options {
            get {
                return this.options;
            }
        }

        [DefaultValue("")]
        public string Usage {
            get {
                return usage;
            }
            set {
                usage = value;
            }
        }
        string usage;

        public ParseStatus ParseStatus {
            get {
                if (!processed)
                    return ParseStatus.NotParsed;
                else if (this.errors.Count == 0)
                    return ParseStatus.Success;
                else
                    return ParseStatus.ParseFailed;
            }
        }

        [ReadOnly(true)]
        public ReadOnlyCollection<string> UnusedArguments {
            get {
                return this.nonoptions.AsReadOnly();
            }
        }
    }

    internal enum ParseResult
    {
        Success,
        ArgumentNotAllowed,
        MalformedArgument,
        MissingOption,
        UnrecognizedOption,
        MultipleOccurance
    }

    public enum ParseStatus
    {
        NotParsed,
        ParseFailed,
        Success
    }

}
