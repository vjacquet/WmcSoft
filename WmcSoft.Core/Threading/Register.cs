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

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace WmcSoft.Threading
{
    /// <summary>
    /// Utility class to store values and and keep track of wether it changed or not.
    /// </summary>
    /// <typeparam name="T">The type of the value to store.</typeparam>
    [DebuggerDisplay("{_value,nq}{_changed==1 ? \"*\" : \"\",nq}")]
    public class Register<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T _value;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _changed;
        private readonly IEqualityComparer<T> _comparer;

        public Register(T value, IEqualityComparer<T> comparer) {
            _value = value;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public Register(IEqualityComparer<T> comparer) : this(default(T), comparer) {
        }

        public Register(T value = default(T)) : this(value, null) {
        }

        /// <summary>
        /// Updates the stored value
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>Returns true if the value has changed.</returns>
        /// <remarks>We always keep the most recent value, wether it changed or not.
        /// It maintenains the lifetime of references short.</remarks>
        public bool Update(T value) {
            T current = _value;
            _value = value;
            if (_changed == 0 && !_comparer.Equals(current, value))
                _changed = 1;
            return _changed != 0;
        }

        /// <summary>
        /// Returns the stored value and a boolean value indicated if it changed or not since last time.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The dirty flag</returns>
        public bool TryGetChanges(out T value) {
            value = _value;
            var changed = Interlocked.CompareExchange(ref _changed, 0, 1);
            return changed == 1;
        }

        /// <summary>
        /// Forces the dirty flag to true.
        /// </summary>
        public void MarkAsDirty() {
            _changed = 1;
        }

        /// <summary>
        /// The value.
        /// </summary>
        public T Value {
            get { return _value; }
        }

        /// <summary>
        /// Implicit cast to get the stored value.
        /// </summary>
        /// <param name="self">The instance</param>
        /// <returns>Its stored value.</returns>
        public static implicit operator T(Register<T> self) {
            return self.Value;
        }
    }
}