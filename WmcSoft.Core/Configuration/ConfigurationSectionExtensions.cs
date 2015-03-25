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
using System.Configuration;
using System.Globalization;
using WmcSoft.Collections.Specialized;

namespace WmcSoft.Configuration
{
    public static class AppConfig
    {
        static IDictionary<Type, string> _knownConfigurationSections;
        static AppConfig() {
            _knownConfigurationSections = new Dictionary<Type, string>();
        }

        public static void RegisterSectionName<T>(string sectionName) where T : ConfigurationSection {
            _knownConfigurationSections.Add(typeof(T), sectionName);
        }

        public static T GetSection<T>(string sectionName) where T : ConfigurationSection {
            var section = ConfigurationManager.GetSection(sectionName);
            return (T)section;
        }

        public static T GetAppSetting<T>(string name, T defaultValue = default(T)) {
            return ConfigurationManager.AppSettings.GetValue<T>(name);
        }
    }
}
