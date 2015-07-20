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

namespace WmcSoft.Business.ProductModel
{
    /// <summary>
    /// Represents a specific feature (such a color) of a good and service 
    /// and its value.
    /// </summary>
    public class ProductFeatureInstance
    {
        #region Fields

        readonly ProductFeatureType _productFeatureType;
        readonly object _value;

        #endregion

        #region Lifecycle

        public ProductFeatureInstance(ProductFeatureType productFeatureType, object value) {
            _productFeatureType = productFeatureType;
            _value = value;
        }

        #endregion

        #region Properties

        public ProductFeatureType ProductFeatureType {
            get { return _productFeatureType; }
        }

        public string Name {
            get { return _productFeatureType.Name; }
        }

        public string Description {
            get { return _productFeatureType.Description; }
        }

        public object Value {
            get { return _value; }
        }

        #endregion
    }
}