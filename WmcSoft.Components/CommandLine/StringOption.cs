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

using System.ComponentModel;
using System.IO;

namespace WmcSoft.CommandLine
{
    public class StringOption : Option
    {
        #region Lifecycle

        [EditorBrowsable(EditorBrowsableState.Never)]
        public StringOption()
        {
        }

        public StringOption(string name)
            : base(name)
        {
        }

        public StringOption(string name, string description)
            : base(name, description)
        {
        }

        public StringOption(string name, string description, string template)
            : base(name, description)
        {
            _template = template;
        }

        #endregion

        #region Overrides

        protected override bool ValidateArgument(string argument)
        {
            return (argument.Length > 0 && argument[0] == ':');
        }

        protected override void DoParseArgument(string argument)
        {
            base.Value = argument.Substring(1);
        }

        public override void WriteTemplate(TextWriter writer)
        {
            writer.WriteLine(OptionDelimiter + OptionName + ':' + _template);
        }

        #endregion

        #region Properties

        public string Template {
            get { return _template; }
            set { _template = value; }
        }
        private string _template = "xxxx";

        #endregion
    }
}
