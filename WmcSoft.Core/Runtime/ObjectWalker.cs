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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace WmcSoft.Runtime
{
    /// <summary>
    /// This class walks through all the objects once in an object graph.
    /// </summary>
    public class ObjectWalker : IEnumerable<object>
    {
        #region Enumerator

        class Enumerator : IEnumerator<object>
        {
            #region State

            private readonly ObjectWalker _walker;

            private Object _current;
            private Stack _toWalk;
            private ObjectIDGenerator _objectIDGenerator;

            #endregion

            #region Lifecycle

            internal Enumerator(ObjectWalker walker) {
                _walker = walker;
                Reset();
            }

            #endregion

            #region IEnumerator Membres

            public object Current {
                get { return _current; }
            }

            public bool MoveNext() {
                if (_toWalk.Count == 0)
                    return false;

                // Check if the object is a terminal object (has no fields that refer to other objects).
                _current = _toWalk.Pop();
                if (!IsTerminalObject(_current)) {
                    // The object does have field, schedule the object's instance fields to be enumerated.
                    foreach (FieldInfo fi in _current.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
                        Walk(fi.GetValue(_current));
                    }
                }
                return true;
            }

            public void Reset() {
                _toWalk = new Stack();
                _objectIDGenerator = new ObjectIDGenerator();
                _current = null;
                Walk(_walker._root);
            }

            #endregion

            #region Methods

            /// <summary>
            /// Walk the reference of the passed-in object.
            /// </summary>
            /// <param name="data"></param>
            void Walk(Object data) {
                if (data == null)
                    return;

                // Ask the ObjectIDManager if this object has been examined before.
                bool firstOccurrence;
                _objectIDGenerator.GetId(data, out firstOccurrence);

                // If this object has been examined before, do not look at it again just return.
                if (!firstOccurrence)
                    return;

                if (data.GetType().IsArray) {
                    foreach (object item in (Array)data)
                        Walk(item);
                } else {
                    _toWalk.Push(data);
                }
            }

            bool IsTerminalObject(object data) {
                Type t = data.GetType();
                return t.IsPrimitive || t.IsEnum || t.IsPointer || data is String;
            }

            #endregion

            #region IDisposable Members

            public void Dispose() {
            }

            #endregion
        }

        #endregion

        #region State

        object _root;

        #endregion

        #region Lifecycle

        /// <summary>
        /// Construct an ObjectWalker passing the root of the object graph.
        /// </summary>
        /// <param name="root">root of the object graph</param>
        public ObjectWalker(object root) {
            _root = root;
        }

        #endregion

        #region IEnumerable<object> Members

        public IEnumerator<object> GetEnumerator() {
            return new Enumerator(this);
        }

        #endregion

        #region IEnumerable Membres

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

    }
}
