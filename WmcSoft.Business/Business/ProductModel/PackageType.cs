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
    /// Specifies a set of component ProductTypes and rules about how these 
    /// may be combined to create PackageInstances.
    /// </summary>
    public class PackageType : ProductType
    {
        #region Fields

        List<ProductType> _components;

        #endregion

        #region Lifecycle

        public PackageType()
            : this(new ProductIdentifier()) {
        }

        public PackageType(ProductIdentifier identifier)
            : base(identifier) {
            _components = new List<ProductType>();
        }

        #endregion

        #region Properties

        public IList<ProductType> Components {
            get { return _components; }
        }

        #endregion

        #region Overridables

        public bool ValidatePackage(PackageInstance package) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
