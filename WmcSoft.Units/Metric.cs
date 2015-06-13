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

namespace WmcSoft.Units
{
    /// <summary>
    /// The metric represents a standard of measurement.
    /// </summary>
    /// <remarks>Based on Enterprise Patterns and MDA, isbn:9780321112309</remarks>
    [DebuggerDisplay("{Name,nq}")]
    public abstract class Metric : IEquatable<Metric>
    {
        /// <summary>
        /// Returns the localized name of the metric.
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// Returns the symbol of the metric, or null.
        /// </summary>
        public abstract string Symbol { get; }
        /// <summary>
        /// Returns the localized formal defintion of the metric.
        /// </summary>
        public abstract string Definition { get; }

        #region Equal overrides

        public override bool Equals(object obj) {
            if ((obj == null) || (GetType() != obj.GetType()))
                return false;
            Metric other = (Metric)obj;
            return (Name == other.Name) && (Symbol == other.Symbol) && (Definition == other.Definition);
        }

        public static bool operator ==(Metric left, Metric right) {
            if (((object)left) == null)
                return ((object)right) == null;
            return left.Equals(right);
        }

        public static bool operator !=(Metric left, Metric right) {
            if (((object)left) == null)
                return ((object)right) != null;
            return !left.Equals(right);
        }

        public override int GetHashCode() {
            string value;
            int hash = 0;

            value = Name;
            if (value != null)
                hash ^= value.GetHashCode();

            value = Symbol;
            if (value != null)
                hash ^= value.GetHashCode() << 4;

            return hash;
        }
        #endregion

        #region IEquatable<Metric> Members

        public bool Equals(Metric other) {
            if (other == null)
                return false;
            return (Name == other.Name) && (Symbol == other.Symbol) && (Definition == other.Definition);
        }

        #endregion
    }
}
