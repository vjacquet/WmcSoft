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

using System.Collections.Generic;
using System.Linq;

namespace WmcSoft.Business.ProductModel
{
    /// <summary>
    /// Represents a set of ProductIdentifiers that refers to ProductTypes.
    /// </summary>
    public class ProductSet : ICollection<ProductIdentifier>
    {
        #region Fields

        readonly List<ProductIdentifier> _items;

        #endregion

        #region Lifecyle

        public ProductSet() {
            _items = new List<ProductIdentifier>();
        }

        #endregion

        #region Methods

        public int GetCountOf(ProductIdentifier reference) {
            return _items.Count(i => i == reference);
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        public ICollection<ProductIdentifier> ProductReferences {
            get { return _items; }
        }

        #endregion

        #region ICollection<ProductIdentifier> Members

        public void Add(ProductIdentifier item) {
            _items.Add(item);
        }

        public void Clear() {
            _items.Clear();
        }

        public bool Contains(ProductIdentifier item) {
            return _items.Contains(item);
        }

        public void CopyTo(ProductIdentifier[] array, int arrayIndex) {
            _items.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return _items.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(ProductIdentifier item) {
            return _items.Remove(item);
        }

        #endregion

        #region IEnumerable<ProductIdentifier> Members

        public IEnumerator<ProductIdentifier> GetEnumerator() {
            return _items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _items.GetEnumerator();
        }

        #endregion
    }
}
