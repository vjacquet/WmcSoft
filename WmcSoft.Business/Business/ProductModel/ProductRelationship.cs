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

namespace WmcSoft.Business.ProductModel
{
    /// <summary>
    /// Represents a fixed relationship between product types that is not packaging
    /// or containment relationship.
    /// </summary>
    public abstract class ProductRelationship
    {
        #region Fields

        private readonly ProductType _client;
        private readonly ProductType _supplier;

        #endregion

        #region Lifecycle

        protected ProductRelationship(ProductType client, ProductType supplier) {
            _client = client;
            _supplier = supplier;
        }

        #endregion

        #region Properties

        public virtual string Name {
            get {
                // crawl the hierarchy of types to find a name
                var type = GetType();
                while (type != typeof(ProductRelationship)) {
                    var name = RM.GetName(type);
                    if (!String.IsNullOrEmpty(name))
                        return name;
                    type = type.BaseType;
                }
                return GetType().Name;
            }
        }

        public virtual string Description {
            get {
                var type = GetType();
                while (type != typeof(ProductRelationship)) {
                    // crawl the hierarchy of types to find a name
                    // then get the associated description
                    var name = RM.GetName(type);
                    if (!String.IsNullOrEmpty(name))
                        return RM.GetDescription(type);
                    type = type.BaseType;
                }
                return null;
            }
        }

        public ProductType Client {
            get { return _client; }
        }

        public ProductType Supplier {
            get { return _supplier; }
        }

        #endregion

        #region Management of contraints

        //protected static IEnumerable<PartyRelationshipConstraintAttribute> GetConstraints(Type type) {
        //    return TypeDescriptor.GetAttributes(type).OfType<PartyRelationshipConstraintAttribute>();
        //}

        //public static IEnumerable<PartyRelationshipConstraintAttribute> GetConstraintsOf<R>() where R : PartyRelationship {
        //    return GetConstraints(typeof(R));
        //}

        #endregion
    }

    /// <summary>
    /// Provides a way to show that the suppliers in a ProductRelationship
    /// represent upgrades to the client.
    /// </summary>
    public class UpgradableTo : ProductRelationship
    {
        public UpgradableTo(ProductType client, ProductType supplier)
            : base(client, supplier) {
        }
    }

    /// <summary>
    /// Provides a way to show that an instance of the client in a ProductRelationship
    /// may be substituted by an instance of one othe the suppliers.
    /// </summary>
    public class SubstitutedBy : ProductRelationship
    {
        public SubstitutedBy(ProductType client, ProductType supplier)
            : base(client, supplier) {
        }
    }

    /// <summary>
    /// Provides a way to show that the client in a ProductRelationship has been 
    /// superseded by the suppliers - this means that the client is now
    /// obsolete and must be replaced by one of the suppliers.
    /// </summary>
    public class ReplacedBy : ProductRelationship
    {
        public ReplacedBy(ProductType client, ProductType supplier)
            : base(client, supplier) {
        }
    }

    /// <summary>
    /// Provides a way to show that the client in a ProductRelationship may be 
    /// complemented in some way by one of the suppliers.
    /// </summary>
    public class ComplementedBy : ProductRelationship
    {
        public ComplementedBy(ProductType client, ProductType supplier)
            : base(client, supplier) {
        }
    }

    /// <summary>
    /// Provides a way to show that the client in a ProductRelationship is
    /// compatible with all of the suppliers.
    /// </summary>
    public class CompatibleWith : ProductRelationship
    {
        public CompatibleWith(ProductType client, ProductType supplier)
            : base(client, supplier) {
        }
    }

    /// <summary>
    /// Provides a way to show that the client in a ProductRelationship is
    /// not compatible with any of the suppliers.
    /// </summary>
    public class IncompatibleWith : ProductRelationship
    {
        public IncompatibleWith(ProductType client, ProductType supplier)
            : base(client, supplier) {
        }
    }

}
