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
using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace WmcSoft.Business.Accounting
{
    /// <summary>
    /// Description r�sum�e de RM.
    /// </summary>
    [DebuggerStepThrough]
    internal sealed class RM
    {
        internal const string IncompatibleCurrencyException = "IncompatibleCurrencyException";

        static ResourceManager rm;

        static ResourceManager GetManager() {
            if (rm == null) {
                lock (typeof(RM)) {
                    if (rm == null) {
                        rm = new ResourceManager(typeof(RM));
                    }
                }
            }
            return rm;
        }

        public static string GetString(string name) {
            return GetString(name, (CultureInfo)null);
        }

        public static string GetString(string name, CultureInfo culture) {
            return GetManager().GetString(name, culture);
        }

        public static string Format(string name, params object[] args) {
            return Format(name, (CultureInfo)null, args);
        }

        public static string Format(string name, CultureInfo culture, params object[] args) {
            return String.Format(GetManager().GetString(name, culture), args);
        }

        public static string GetName(string name) {
            return GetName(name, (CultureInfo)null);
        }

        public static string GetName(string name, CultureInfo culture) {
            return GetManager().GetString(String.Format("{0}.Name", name), culture);
        }

        public static string GetDefinition(string name) {
            return GetDefinition(name, (CultureInfo)null);
        }

        public static string GetDefinition(string name, CultureInfo culture) {
            return GetManager().GetString(String.Format("{0}.Definition", name), culture);
        }

        public static string GetSymbol(string name) {
            return GetSymbol(name, (CultureInfo)null);
        }

        public static string GetSymbol(string name, CultureInfo culture) {
            return GetManager().GetString(String.Format("{0}.Symbol", name), culture);
        }

    }
}
