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
using System.Diagnostics;
using System.Threading;

namespace WmcSoft.Diagnostics
{
    /// <summary>
    /// Represents a Named counter.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq}")]
    public sealed class Counter : IComparable<Counter>
    {
        static readonly Comparer<int> comparer = Comparer<int>.Default;

        int _count;

        /// <summary>
        /// Construct a new counter.
        /// </summary>
        /// <param name="name">The name of the counter.</param>
        public Counter(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));

            Name = name;
        }

        /// <summary>
        /// Name of the counter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Increment the counter.
        /// </summary>
        public void Increment()
        {
            Interlocked.Increment(ref _count);
        }

        /// <summary>
        /// Number of increments since creation.
        /// </summary>
        public int Tally => _count;

        /// <summary>
        /// Resets the counter.
        /// </summary>
        /// <returns>Returns the tally before the reset.</returns>
        public int Reset()
        {
            return Interlocked.Exchange(ref _count, 0);
        }

        /// <summary>
        /// Returns a string that represents the current counter.
        /// </summary>
        /// <returns>A string that represents the current counter.</returns>
        public override string ToString()
        {
            return _count + " (" + Name + ')';
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns
        /// an integer that indicates whether the current instance precedes, follows, or
        /// occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of counters being compared, as shown in the
        /// following table.
        /// <list type="table">
        /// <listheader>
        ///   <description>Value</description>
        ///   <description>Meaning</description>
        /// </listheader>
        /// <item>
        ///   <description>Less than zero</description>
        ///   <description> This instance's value is less than <paramref name="other"/>'s value.</description>
        /// </item>
        /// <item>
        ///   <description>Zero</description>
        ///   <description>This instance's value equals <paramref name="other"/>'s value.</description>
        /// </item>
        /// <item>
        ///   <description>Greater than zero</description>
        ///   <description>This instance's value is greater than <paramref name="other"/>'s value.</description>
        /// </item>
        /// </list>
        /// </returns>
        public int CompareTo(Counter other)
        {
            if (other == null)
                return 1;

            var result = comparer.Compare(_count, other._count);
            if (result == 0)
                return StringComparer.InvariantCulture.Compare(Name, other.Name);
            return result;
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj as Counter) == 0;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(Counter left, Counter right)
        {
            if (left is null) {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Counter left, Counter right)
        {
            return !(left == right);
        }

        public static bool operator <(Counter left, Counter right)
        {
            return left is null ? !(right is null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(Counter left, Counter right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        public static bool operator >(Counter left, Counter right)
        {
            return !(left is null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(Counter left, Counter right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }
    }
}
