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
    public class SwitchOption : Option
    {
        #region Lifecycle

        [EditorBrowsable(EditorBrowsableState.Never)]
        public SwitchOption()
        {
        }

        public SwitchOption(string name)
            : base(name)
        {
        }

        public SwitchOption(string name, string description)
            : base(name, description)
        {
        }

        #endregion

        #region Overrides

        protected override bool ValidateArgument(string argument)
        {
            return string.IsNullOrEmpty(argument);
        }

        protected override void DoParseArgument(string argument)
        {
        }

        public override void WriteTemplate(TextWriter writer)
        {
            writer.WriteLine(OptionDelimiter + OptionName);
        }

        #endregion
    }
}
