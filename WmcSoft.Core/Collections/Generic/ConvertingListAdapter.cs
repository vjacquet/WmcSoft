﻿#region Licence

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

namespace WmcSoft.Collections.Generic
{
    sealed class ConvertingListAdapter<TInput, TOutput> : IReadOnlyList<TOutput>
    {
        private readonly IReadOnlyList<TInput> _list;
        private readonly Converter<TInput, TOutput> _convert;

        public ConvertingListAdapter(IReadOnlyList<TInput> list, Converter<TInput, TOutput> converter) {
            if (list == null)
                throw new ArgumentNullException("list");
            if (converter == null)
                throw new ArgumentNullException("convert");

            _list = list;
            _convert = converter;
        }

        #region IReadOnlyList<TOutput> Membres

        public TOutput this[int index] {
            get { return _convert(_list[index]); }
        }

        #endregion

        #region IReadOnlyCollection<TOutput> Membres

        public int Count {
            get { return _list.Count; }
        }

        #endregion

        #region IEnumerable<TOutput> Membres

        public IEnumerator<TOutput> GetEnumerator() {
            return _list.Select(i => _convert(i)).GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
