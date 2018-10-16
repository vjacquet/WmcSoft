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
using WmcSoft.Collections.Specialized;

namespace WmcSoft.Configuration
{
    public static class AppConfig
    {
        static Dictionary<Type, string> knownConfigurationSections;

        static AppConfig()
        {
            knownConfigurationSections = new Dictionary<Type, string>();
        }

        public static void RegisterSectionName<T>(string sectionName)
            where T : ConfigurationSection
        {
            knownConfigurationSections.Add(typeof(T), sectionName);
        }

        public static T GetSection<T>()
            where T : ConfigurationSection
        {
            var sectionName = knownConfigurationSections[typeof(T)];
            return GetSection<T>(sectionName);
        }

        public static T GetSection<T>(string sectionName)
            where T : ConfigurationSection
        {
            var section = ConfigurationManager.GetSection(sectionName);
            return (T)section;
        }

        public static T GetAppSetting<T>(string name, T defaultValue = default)
        {
            return ConfigurationManager.AppSettings.GetValue(name, defaultValue);
        }

        public static T GetAppSetting<T>(string name, TypeConverter converter, T defaultValue = default)
        {
            return ConfigurationManager.AppSettings.GetValue(name, converter, defaultValue);
        }

        public static IEnumerable<T> GetAppSettings<T>(string name)
        {
            return ConfigurationManager.AppSettings.GetValues<T>(name);
        }

        public static IEnumerable<T> GetAppSettings<T>(string name, TypeConverter converter)
        {
            return ConfigurationManager.AppSettings.GetValues<T>(name, converter);
        }
    }
}
