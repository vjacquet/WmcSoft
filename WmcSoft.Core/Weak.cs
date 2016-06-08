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

namespace WmcSoft
{
    /// <summary>
    /// Represents a weak reference, which references an object while still allowing
    /// that object to be reclaimed by garbage collection.
    /// </summary>
    /// <typeparam name="T">The type of the weak reference.</typeparam>
    public class Weak<T> where T : class
    {
        private readonly WeakReference _weak;

        /// <summary>
        /// Initializes a new instance of the <see cref="Weak{T}"/> class, referencing the
        /// specified object.
        /// </summary>
        /// <param name="target">The object to track or null.</param>
        public Weak(T target = null) {
            _weak = new WeakReference(target);
        }

        /// Initializes a new instance of the System.WeakReference class, referencing the
        /// specified object and using the specified resurrection tracking.
        /// </summary>
        /// <param name="target">An object to track.</param>
        /// <param name="trackResurrection">Indicates when to stop tracking the object. If true, the object is tracked 
        /// after finalization; if false, the object is only tracked until finalization.</param>
        public Weak(T target, bool trackResurrection) {
            _weak = new WeakReference(target, trackResurrection);
        }

        /// <summary>
        /// Gets or sets the object (the target) referenced by the current <see cref="Weak{T}"/> object.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The reference to the target object is invalid. 
        /// This exception can be thrown while setting this property if the value is a null reference 
        /// or if the object has been finalized during the set operation.
        /// </exception>
        public T Target {
            get { return (T)_weak.Target; }
            set { _weak.Target = value; }
        }

        /// <summary>Gets an indication whether the object referenced by the current <see cref="Weak{T}"/>
        /// object is tracked after it is finalized.
        /// </summary>
        public bool TrackResurrection {
            get { return _weak.TrackResurrection; }
        }

        /// <summary>
        /// Gets an indication whether the object referenced by the current <see cref="Weak{T}"/>
        /// object has been garbage collected.
        /// </summary>
        public bool IsAlive {
            get { return _weak.IsAlive; }
        }
    }
}
