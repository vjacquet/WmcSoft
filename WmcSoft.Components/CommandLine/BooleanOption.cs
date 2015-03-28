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
    public class BooleanOption : Option
    {
        #region Lifecycle

        [EditorBrowsable(EditorBrowsableState.Never)]
        public BooleanOption() {
        }

        public BooleanOption(string name)
            : base(name) {
        }

        public BooleanOption(string name, string description)
            : base(name, description) {
        }

        #endregion

        #region Overrides

        protected override bool ValidateArgument(string argument) {
            return !((argument != "+") && (argument != "-"));
        }

        protected override void DoParseArgument(string argument) {
            base.Value = (argument == "+");
        }

        public override void WriteTemplate(TextWriter writer) {
            writer.WriteLine("/{0}+|-", base.SwitchName);
        }

        #endregion
    }
}
