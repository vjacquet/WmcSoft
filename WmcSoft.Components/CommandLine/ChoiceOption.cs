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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;

namespace WmcSoft.CommandLine
{
    public class ChoiceOption : Option
    {
        #region Choice class

        public class Choice
        {
            #region Lifecycle

            public Choice()
            {
                Description = string.Empty;
            }

            public Choice(string name, string description)
            {
                Name = name;
                Description = description;
            }

            #endregion

            #region Properties

            public string Name { get; set; }

            [DefaultValue("")]
            public string Description { get; set; }

            #endregion

            #region Methods

            public void WriteTemplate(TextWriter writer)
            {
                writer.Write(Name);
                writer.Write("  ");
                writer.WriteLine(Description);
            }

            #endregion
        }

        #endregion

        #region Lifecycle

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ChoiceOption()
        {
        }

        public ChoiceOption(string name)
            : base(name)
        {
        }

        public ChoiceOption(string name, string description)
            : base(name, description)
        {
        }

        public ChoiceOption(string name, string description, string template)
            : base(name, description)
        {
            Template = template;
        }

        #endregion

        #region Overrides

        protected override bool ValidateArgument(string argument)
        {
            if (argument.Length == 0 || argument[0] != ':')
                return false;

            string value = argument.Substring(1);
            foreach (Choice choice in Choices) {
                if (StringComparer.InvariantCultureIgnoreCase.Equals(value, choice.Name))
                    return true;
            }
            return false;
        }

        protected override void DoParseArgument(string argument)
        {
            base.Value = argument.Substring(1);
        }

        public override void WriteTemplate(TextWriter writer)
        {
            writer.WriteLine("{0}:{1}", OptionDelimiter + OptionName, Template);
            using (var enumerator = choices.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    writer.Write(Template);
                    writer.Write("  ");
                    enumerator.Current.WriteTemplate(writer);

                    string indent = new string(' ', Template.Length + 2);
                    while (enumerator.MoveNext()) {
                        writer.Write(indent);
                        enumerator.Current.WriteTemplate(writer);
                    }
                }
            }
        }

        #endregion

        #region Properties

        [EditorAttribute(typeof(ChoiceCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        public IList<Choice> Choices {
            get {
                return choices;
            }
        }

        private readonly IList<Choice> choices = new List<Choice>();

        public void AddChoice(string name, string description)
        {
            choices.Add(new Choice(name, description));
        }
        public void RemoveChoice(string name)
        {
            Choice toRemove = null;
            foreach (Choice choice in choices) {
                if (StringComparer.InvariantCultureIgnoreCase.Equals(name, choice.Name)) {
                    toRemove = choice;
                    break;
                }
            }
            if (toRemove != null) {
                choices.Remove(toRemove);
            }
        }

        public string Template { get; set; } = "xxxx";

        #endregion
    }

    #region Design

    internal class ChoiceCollectionEditor : CollectionEditor
    {
        public ChoiceCollectionEditor()
            : base(typeof(List<ChoiceOption.Choice>))
        {
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(ChoiceOption.Choice);
        }

        protected override string GetDisplayText(object value)
        {
            var choice = value as ChoiceOption.Choice;
            return string.IsNullOrEmpty(choice.Name)
                ? "Choice"
                : choice.Name;
        }
    }

    #endregion
}
