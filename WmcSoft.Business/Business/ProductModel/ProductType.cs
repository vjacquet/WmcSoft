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

namespace WmcSoft.Business.ProductModel
{
    /// <summary>
    /// Describes the common properties of a set of goods or services.
    /// </summary>
    public abstract class ProductType
    {
        #region Fields

        readonly ProductIdentifier _identifier;
        readonly List<ProductFeatureType> _mandatory;
        readonly List<ProductFeatureType> _optional;

        #endregion

        #region Lifecycle

        public ProductType()
            : this(new ProductIdentifier()) {
        }

        public ProductType(ProductIdentifier identifier) {
            _identifier = identifier;
            _mandatory = new List<ProductFeatureType>();
            _optional = new List<ProductFeatureType>();
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        public string Description { get; set; }

        public ProductIdentifier Identifier {
            get { return _identifier; }
        }

        public IList<ProductFeatureType> MandatoryFeatures {
            get { return _mandatory; }
        }

        public IList<ProductFeatureType> OptionalFeatures {
            get { return _optional; }
        }

        #endregion
    }
}
