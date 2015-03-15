using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Collections.Generic
{
    class ConvertingListAdapter<TInput, TOutput> : IReadOnlyList<TOutput>
    {
        private readonly IReadOnlyList<TInput> _list;
        private readonly Converter<TInput, TOutput> _convert;

        public ConvertingListAdapter(IReadOnlyList<TInput> list, Converter<TInput, TOutput> convert) {
            if (list == null)
                throw new ArgumentNullException("list");
            if (convert == null)
                throw new ArgumentNullException("convert");

            _list = list;
            _convert = convert;
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
