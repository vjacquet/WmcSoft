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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    public static class ToolStripExtensions
    {
        public static IEnumerable<T> Where<T>(this ToolStripItemCollection collection, Func<T, bool> predicate)
            where T : ToolStripItem {
            return collection.OfType<T>().Where(predicate);
        }

        public static T First<T>(this ToolStripItemCollection collection)
            where T : ToolStripItem {
            return collection.OfType<T>().First();
        }

        public static T First<T>(this ToolStripItemCollection collection, Func<T, bool> predicate)
            where T : ToolStripItem {
            return collection.OfType<T>().First(predicate);
        }

        public static IEnumerable<ToolStripItem> Except(this ToolStripItemCollection collection, ToolStripItem item) {
            foreach (ToolStripItem i in collection) {
                if (i != item) {
                    yield return i;
                }
            }
        }

        public static ToolStripItem First(this ToolStripItemCollection collection) {
            return collection.OfType<ToolStripItem>().First();
        }
    }
}
