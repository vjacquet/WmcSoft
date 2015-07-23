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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Business.ProductModel
{
    /// <summary>
    /// Represents a specific instance of a product type.
    /// </summary>
    public class ProductInstance
    {
        #region Fields

        readonly ProductType _productType;
        readonly SerialNumber _serialNumber;
        readonly Batch _batch;
        readonly List<ProductFeatureInstance> _features;

        #endregion

        #region Lifecycle

        public ProductInstance(ProductType productType) {
            _productType = productType;
            _features = new List<ProductFeatureInstance>();
        }

        public ProductInstance(ProductType productType, SerialNumber serialNumber, Batch batch)
            : this(productType) {
            _serialNumber = serialNumber;
            _batch = batch;
        }

        #endregion

        #region Properties

        public ProductType ProductType {
            get { return _productType; }
        }

        public SerialNumber SerialNumber {
            get { return _serialNumber; }
        }

        public Batch Batch {
            get { return _batch; }
        }

        public IList<ProductFeatureInstance> Features {
            get {
                return _features;
            }
        }

        #endregion
    }
}
