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
using WmcSoft.Collections.Generic;

namespace WmcSoft.Runtime
{
    /// <summary>
    /// This class walks through all the objects once in an object graph.
    /// </summary>
    public class ObjectWalker : IEnumerable<object>
    {
        #region Enumerator

        class ArrayFacade : ICollection<object>
        {
            readonly Array _array;

            public ArrayFacade(Array array) {
                _array = array;
            }

            public int Count
            {
                get {
                    return _array.Length;
                }
            }

            bool ICollection<object>.IsReadOnly
            {
                get { return true; }
            }

            void ICollection<object>.Add(object item) {
                throw new NotSupportedException();
            }

            void ICollection<object>.Clear() {
                throw new NotSupportedException();
            }

            public bool Contains(object item) {
                return Array.IndexOf(_array, item) >= 0;
            }

            public void CopyTo(object[] array, int arrayIndex) {
                foreach (var item in _array) {
                    array[arrayIndex++] = item;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return _array.GetEnumerator();
            }

            IEnumerator<object> IEnumerable<object>.GetEnumerator() {
                return new EnumeratorAdapter(_array.GetEnumerator());
            }

            bool ICollection<object>.Remove(object item) {
                throw new NotSupportedException();
            }
        }

        class Enumerator : IEnumerator<object>
        {
            #region State

            private readonly ObjectWalker _walker;

            private object _current;
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

            public object Current
            {
                get { return _current; }
            }

            public bool MoveNext() {
                if (_toWalk.Count == 0)
                    return false;

                // Check if the object is a terminal object (has no fields that refer to other objects).
                _current = _toWalk.Pop();
                if (!IsTerminalObject(_current)) {
                    // The object does have field, schedule the object's instance fields to be enumerated.
                    var fields = _current.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    for (int i = fields.Length - 1; i >= 0; i--) {
                        Walk(fields[i].GetValue(_current));
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

            bool IsFirstOccurrence(object data) {
                bool firstOccurrence;
                _objectIDGenerator.GetId(data, out firstOccurrence);
                return firstOccurrence;
            }

            /// <summary>
            /// Walk the reference of the passed-in object.
            /// </summary>
            /// <param name="data">the object</param>
            void Walk(object data) {
                if (data == null || !IsFirstOccurrence(data))
                    return;

                if (data.GetType().IsArray) {
                    var items = new List<object>(new ArrayFacade((Array)data));
                    for (int i = items.Count - 1; i >= 0; i--) {
                        Walk(items[i]);
                    }
                } else {
                    _toWalk.Push(data);
                }
            }

            bool IsTerminalObject(object data) {
                var t = data.GetType();
                return t.IsPrimitive || t.IsEnum || t.IsPointer || data is string;
            }

            #endregion

            #region IDisposable Members

            public void Dispose() {
            }

            #endregion
        }

        #endregion

        #region Fields

        readonly object _root;

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
