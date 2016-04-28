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

namespace WmcSoft.Business
{
    public abstract class DomainObject<TKey> : IUniqueIdentifier<TKey>, IEquatable<TKey>
        where TKey : IEquatable<TKey>
    {
        #region Comparers

        public struct UniqueIdentifierEqualityComparer : IEqualityComparer<DomainObject<TKey>>
        {
            #region IEqualityComparer<DomainObject<TKey>> Membres

            bool IEqualityComparer<DomainObject<TKey>>.Equals(DomainObject<TKey> x, DomainObject<TKey> y) {
                if (x == null) return y == null;
                if (x == null) return y == null;
                return x.Id.Equals(y.Id);
            }

            int IEqualityComparer<DomainObject<TKey>>.GetHashCode(DomainObject<TKey> obj) {
                if (obj == null) return 0;
                return obj.Id.GetHashCode();
            }

            #endregion
        }
        public readonly static UniqueIdentifierEqualityComparer UniqueIdentifierComparer = new UniqueIdentifierEqualityComparer();

        #endregion

        #region Lifecycle

        protected DomainObject(TKey id) {
            Id = id;
        }

        protected DomainObject() {
        }

        #endregion

        #region Properties

        public virtual TKey Id { get; set; }

        #endregion

        #region IEquatable<TKey> Members

        public bool Equals(TKey other) {
            return ((IEquatable<TKey>)Id).Equals(other);
        }

        #endregion
    }
}
