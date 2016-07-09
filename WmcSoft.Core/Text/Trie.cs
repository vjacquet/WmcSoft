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
using System.Collections;
using System.Collections.Generic;

namespace WmcSoft.Text
{
    public class Trie<TLetter> : IList<IReadOnlyList<TLetter>>
        where TLetter : IComparable<TLetter>
    {
        class Node
        {
            readonly IReadOnlyList<TLetter> _word;
        }

        public IReadOnlyList<TLetter> this[int index] {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public int Count {
            get {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public void Add(IReadOnlyList<TLetter> item) {
            throw new NotImplementedException();
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(IReadOnlyList<TLetter> item) {
            throw new NotImplementedException();
        }

        public void CopyTo(IReadOnlyList<TLetter>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public IEnumerator<IReadOnlyList<TLetter>> GetEnumerator() {
            throw new NotImplementedException();
        }

        public int IndexOf(IReadOnlyList<TLetter> item) {
            throw new NotImplementedException();
        }

        public void Insert(int index, IReadOnlyList<TLetter> item) {
            throw new NotImplementedException();
        }

        public bool Remove(IReadOnlyList<TLetter> item) {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index) {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
