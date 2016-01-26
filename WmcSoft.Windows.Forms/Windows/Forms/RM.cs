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
using System.Resources;

namespace WmcSoft.Windows.Forms
{
    internal class RM
    {
        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        internal sealed class CategoryAttribute : System.ComponentModel.CategoryAttribute
        {
            // Methods
            public CategoryAttribute(string resourceName)
                : base(resourceName + ".Category") {
            }

            protected override string GetLocalizedString(string value) {
                return RM.GetString(value);
            }
        }

        internal class DescriptionAttribute : System.ComponentModel.DescriptionAttribute
        {
            public DescriptionAttribute(string resourceName)
                : base(RM.GetString(resourceName + ".Description")) {
            }
        }

        internal class DisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
        {
            public DisplayNameAttribute(string resourceName)
                : base(RM.GetString(resourceName + ".DisplayName")) {
            }
        }

        private static readonly ResourceManager ResourceManager;
        static RM() {
            ResourceManager = new System.Resources.ResourceManager(typeof(RM));
        }

        public static string GetString(string name) {
            return ResourceManager.GetString(name);
        }
    }
}
