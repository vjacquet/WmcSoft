#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

using System.Configuration;
using WmcSoft.Properties;

namespace WmcSoft.Diagnostics.Checkpoints
{
    /// <summary>
    /// Checks that a section is correctly configured in the configuration file, and can be opened.
    /// </summary>
    public class ConfigurationCheckpoint : CheckpointBase
    {
        private readonly string _sectionName;

        public ConfigurationCheckpoint(string name, string sectionName)
            : base(name) {
            _sectionName = sectionName;
        }

        public ConfigurationCheckpoint(string sectionName)
            : this(sectionName, sectionName) {
        }

        /// <summary>
        /// Minimum level required to perform the check.
        /// </summary>
        /// <returns>Returns 0 because configuration is vital to the application.</returns>
        public override int MinimumLevel {
            get { return 0; }
        }

        protected override CheckpointResult DoVerify(int level) {
            try {
                var section = ConfigurationManager.GetSection(_sectionName);
                if (section == null)
                    return Error(Resources.ConfigurationCheckpointCannotLoadSection, _sectionName);
            }
            catch (ConfigurationErrorsException exception) {
                var result = Success();
                foreach (var error in exception.Errors) {
                    result.OverrideResultType(CheckpointResultType.Error)
                        .WriteLine("{0}", error);
                }
                return result;
            }
            return Success();
        }
    }
}