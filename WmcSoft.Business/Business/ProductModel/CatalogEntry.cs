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

using System.Collections.Generic;

namespace WmcSoft.Business.ProductModel
{
    /// <summary>
    /// represents information about a specific type of product held in 
    /// a ProductCatalog.
    /// </summary>
    public class CatalogEntry
    {
        #region Fields

        readonly HashSet<ProductType> _productTypes;
        readonly string _identifier;

        #endregion

        #region Lifecycle

        public CatalogEntry(string catalogIdentifier) {
            _productTypes = new HashSet<ProductType>();
            _identifier = catalogIdentifier;
        }

        #endregion

        #region Properties

        public string CatalogIdentifier {
            get { return _identifier; }
        }

        public string Description { get; set; }

        public string[] Keywords { get; set; }

        public ICollection<ProductType> ProductTypes {
            get { return _productTypes; }
        }

        #endregion
    }
}
