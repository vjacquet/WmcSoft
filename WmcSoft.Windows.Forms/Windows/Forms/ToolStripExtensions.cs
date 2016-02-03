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
        #region Except

        public static IEnumerable<ToolStripItem> Except(this ToolStripItemCollection collection, ToolStripItem item) {
            foreach (ToolStripItem i in collection) {
                if (i != item) {
                    yield return i;
                }
            }
        }

        #endregion

        #region First

        public static ToolStripItem First(this ToolStripItemCollection collection, Func<ToolStripItem, bool> predicate) {
            return collection.OfType<ToolStripItem>().First(predicate);
        }

        public static ToolStripItem First(this ToolStripItemCollection collection) {
            return collection.OfType<ToolStripItem>().First();
        }

        public static T First<T>(this ToolStripItemCollection collection, Func<T, bool> predicate)
            where T : ToolStripItem {
            return collection.OfType<T>().First(predicate);
        }

        public static T First<T>(this ToolStripItemCollection collection)
            where T : ToolStripItem {
            return collection.OfType<T>().First();
        }

        public static ToolStripItem FirstOrDefault(this ToolStripItemCollection collection, Func<ToolStripItem, bool> predicate) {
            return collection.OfType<ToolStripItem>().FirstOrDefault(predicate);
        }

        public static ToolStripItem FirstOrDefault(this ToolStripItemCollection collection) {
            return collection.OfType<ToolStripItem>().FirstOrDefault();
        }

        public static T FirstOrDefault<T>(this ToolStripItemCollection collection, Func<T, bool> predicate)
            where T : ToolStripItem {
            return collection.OfType<T>().FirstOrDefault(predicate);
        }

        public static T FirstOrDefault<T>(this ToolStripItemCollection collection)
            where T : ToolStripItem {
            return collection.OfType<T>().FirstOrDefault();
        }

        #endregion

        #region Single

        public static ToolStripItem Single(this ToolStripItemCollection collection, Func<ToolStripItem, bool> predicate) {
            return collection.OfType<ToolStripItem>().Single(predicate);
        }

        public static ToolStripItem Single(this ToolStripItemCollection collection) {
            return collection.OfType<ToolStripItem>().Single();
        }

        public static T Single<T>(this ToolStripItemCollection collection, Func<T, bool> predicate)
            where T : ToolStripItem {
            return collection.OfType<T>().Single(predicate);
        }

        public static T Single<T>(this ToolStripItemCollection collection)
            where T : ToolStripItem {
            return collection.OfType<T>().Single();
        }

        public static ToolStripItem SingleOrDefault(this ToolStripItemCollection collection, Func<ToolStripItem, bool> predicate) {
            return collection.OfType<ToolStripItem>().SingleOrDefault(predicate);
        }

        public static ToolStripItem SingleOrDefault(this ToolStripItemCollection collection) {
            return collection.OfType<ToolStripItem>().SingleOrDefault();
        }

        public static T SingleOrDefault<T>(this ToolStripItemCollection collection, Func<T, bool> predicate)
            where T : ToolStripItem {
            return collection.OfType<T>().SingleOrDefault(predicate);
        }

        public static T SingleOrDefault<T>(this ToolStripItemCollection collection)
            where T : ToolStripItem {
            return collection.OfType<T>().SingleOrDefault();
        }

        #endregion

        #region Where

        public static IEnumerable<ToolStripItem> Where(this ToolStripItemCollection collection, Func<ToolStripItem, bool> predicate) {
            return collection.OfType<ToolStripItem>().Where(predicate);
        }

        public static IEnumerable<T> Where<T>(this ToolStripItemCollection collection, Func<T, bool> predicate)
            where T : ToolStripItem {
            return collection.OfType<T>().Where(predicate);
        }

        #endregion
    }
}
