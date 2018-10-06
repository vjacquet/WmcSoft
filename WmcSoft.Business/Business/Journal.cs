#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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
using System.Collections;
using System.Collections.Generic;

namespace WmcSoft.Business
{
    /// <summary>
    /// Represents a journal in which you record events.
    /// </summary>
    /// <typeparam name="TEntry">The type of journal's entry.</typeparam>
    public class Journal<TEntry> : IEnumerable<TEntry>
        where TEntry : class
    {
        private readonly List<DateTime> dates;
        private readonly List<TEntry> entries;

        public Journal()
        {
            dates = new List<DateTime>();
            entries = new List<TEntry>();
        }

        public void Record(DateTime date, TEntry entry)
        {
            var found = dates.BinarySearch(date);
            if (found >= 0)
                entries[found] = entry;
            found = ~found;
            dates.Insert(found, date);
            entries.Insert(found, entry);
        }

        public void Close(DateTime date)
        {
            var last = entries.Count - 1;
            if (last < 0 || dates[last] >= date)
                throw new ArgumentOutOfRangeException(nameof(date));
            if (entries[last] == null)
                throw new ArgumentException(nameof(date));
            dates.Add(date);
            entries.Add(default);
        }

        public TEntry this[DateTime date] {
            get {
                var found = dates.BinarySearch(date);
                if (found >= 0)
                    return entries[found];
                else if (found == -1)
                    return null;
                else
                    return entries[~found - 1];
            }
        }

        public IEnumerator<TEntry> GetEnumerator()
        {
            return ((IEnumerable<TEntry>)entries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TEntry>)entries).GetEnumerator();
        }
    }
}
