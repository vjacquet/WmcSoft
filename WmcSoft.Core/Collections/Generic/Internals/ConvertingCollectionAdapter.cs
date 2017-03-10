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
using System.Diagnostics;
using System.Linq;

namespace WmcSoft.Collections.Generic.Internals
{
    sealed class ConvertingCollectionAdapter<TInput, TOutput> : IReadOnlyCollection<TOutput>, ICollection<TOutput>
    {
        private readonly IReadOnlyCollection<TInput> _underlying;
        private readonly Converter<TInput, TOutput> _convert;

        public ConvertingCollectionAdapter(IReadOnlyCollection<TInput> collection, Converter<TInput, TOutput> converter) {
            Debug.Assert(collection != null);
            Debug.Assert(converter != null);

            _underlying = collection;
            _convert = converter;
        }

        #region IReadOnlyCollection<TOutput> Membres

        public int Count {
            get { return _underlying.Count; }
        }

        #endregion

        #region IEnumerable<TOutput> Membres

        public IEnumerator<TOutput> GetEnumerator() {
            return _underlying.Select(i => _convert(i)).GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region ICollection<TOutput> Members

        public void Add(TOutput item) {
            throw new NotSupportedException();
        }

        public void Clear() {
            throw new NotSupportedException();
        }

        public bool Contains(TOutput item) {
            var comparer = EqualityComparer<TOutput>.Default;
            return _underlying.Any(x => comparer.Equals(_convert(x), item));
        }

        public void CopyTo(TOutput[] array, int arrayIndex) {
            foreach (var item in _underlying)
                array[arrayIndex++] = _convert(item);
        }

        public bool IsReadOnly {
            get { return true; }
        }

        public bool Remove(TOutput item) {
            throw new NotSupportedException();
        }

        #endregion
    }
}