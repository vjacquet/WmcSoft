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
using System.IO;
using System.Text;

namespace WmcSoft.CommandLine
{
    public class ListOption : Option
    {
        #region Lifecycle

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ListOption()
        {
        }

        public ListOption(string name)
            : base(name)
        {
        }

        public ListOption(string name, string description)
            : base(name, description)
        {
        }

        public ListOption(string name, string description, string template)
            : base(name, description)
        {
            _template = template;
        }

        #endregion

        #region Overrides

        protected override bool SupportMultipleOccurance {
            get { return true; }
        }

        protected override bool ValidateArgument(string argument)
        {
            return !String.IsNullOrEmpty(argument) && (argument[0] == ':');
        }

        protected override void DoParseArgument(string argument)
        {
            List<string> values = new List<string>(argument.Substring(1).Split(new char[] { ',' }));
            base.Value = values.ToArray();
        }

        public override void WriteTemplate(TextWriter writer)
        {
            writer.WriteLine("{0}:{1}[,{1},{1},...]", OptionDelimiter + OptionName, _template);
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
