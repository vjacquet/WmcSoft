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

namespace WmcSoft.Business.ProductModel
{
    /// <summary>
    /// Represents a persistent store of product information 
    /// used in the selling process.
    /// </summary>
    public abstract class ProductCatalog
    {
        #region Fields

        readonly List<CatalogEntry> _entries;

        #endregion

        #region Lifecycle

        public ProductCatalog() {
            _entries = new List<CatalogEntry>();
        }

        #endregion

        #region Properties

        public IList<CatalogEntry> Entries {
            get { return _entries; }
        }

        #endregion

        #region Methods

        public void AddProductType(ProductType productType) {
            AddProductType(null, productType);
        }

        abstract public void AddProductType(string catalogIdentifier, ProductType productType);

        abstract public void RemoveProductType(ProductIdentifier id);

        abstract public IEnumerable<ProductType> FindProductTypeByCatalogIdentifier(string id);

        abstract public ProductType FindProductTypeByProductTypeIdentifier(ProductIdentifier id);

        abstract public IEnumerable<ProductType> FindProductTypeByName(string name);

        abstract public IEnumerable<ProductType> FindProductFeatureType(params ProductFeatureType[] features);

        #endregion
    }
}
