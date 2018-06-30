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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using WmcSoft.Properties;

namespace WmcSoft.Diagnostics.Checkpoints
{
    /// <summary>
    /// Checks that an app settings is set in the configuration file.
    /// </summary>
    public class AppSettingsCheckpoint : CheckpointBase
    {
        private readonly string[] _keys;
        private readonly Func<string, bool> _predicate = k => !string.IsNullOrWhiteSpace(k);

        public AppSettingsCheckpoint(string key) : base(key)
        {
            _keys = new[] { key };
        }

        public AppSettingsCheckpoint(string name, params string[] keys)
            : base(name)
        {
            if (keys == null || keys.Length == 0) throw new ArgumentException(nameof(keys));
            _keys = keys;
        }

        protected override CheckpointResult DoVerify(int level)
        {
            var appSettings = ConfigurationManager.AppSettings;
            var missing = new HashSet<string>(_keys);
            missing.ExceptWith(appSettings.AllKeys.Where(k => _predicate(appSettings.Get(k))));

            var result = Success();
            foreach (var key in missing) {
                result.OverrideResultType(CheckpointResultType.Error)
                    .WriteLine(Resources.AppSettingsCheckpointKeyIsMissingError, key);
            }
            return result;
        }
    }
}