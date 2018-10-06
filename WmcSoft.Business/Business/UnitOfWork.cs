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
using System.ComponentModel;
using System.Diagnostics;

namespace WmcSoft.Business
{
    /// <summary>
    /// Maintains a list of objects affected by a business transaction and 
    /// coordinates the writing out of changes and the resolution of concurrency problems.
    /// </summary>
    /// <typeparam name="TEntity">The type of entities.</typeparam>
    /// <remarks>See http://www.martinfowler.com/eaaCatalog/unitOfWork.html </remarks>
    public abstract class UnitOfWork<TEntity> : IRevertibleChangeTracking
        where TEntity : class
    {
        #region Private fields

        protected readonly IdentityMap identityMap;
        protected readonly List<TEntity> newInstances;
        protected readonly List<TEntity> dirtyInstances;
        protected readonly List<TEntity> removeInstances;

        #endregion

        #region Lifecycle

        protected UnitOfWork()
        {
            identityMap = new IdentityMap();
            newInstances = new List<TEntity>();
            dirtyInstances = new List<TEntity>();
            removeInstances = new List<TEntity>();
        }

        #endregion

        #region Properties

        public int Count => newInstances.Count + dirtyInstances.Count + removeInstances.Count;

        #endregion

        #region Methods

        public void RegisterNew(TEntity instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            Trace.Assert(!dirtyInstances.Contains(instance), "Domain object should not be dirty");
            Trace.Assert(!removeInstances.Contains(instance), "Domain object should not be removed");
            Trace.Assert(!newInstances.Contains(instance), "Domain object should not already be registered as new");
            newInstances.Add(instance);
        }

        public void RegisterDirty(TEntity instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            Trace.Assert(!removeInstances.Contains(instance), "Domain object should not be removed");
            if (!dirtyInstances.Contains(instance) && !newInstances.Contains(instance)) {
                dirtyInstances.Add(instance);
            }
        }

        public void RegisterClean(TEntity instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            identityMap.Register(instance);
        }

        public void RegisterRemoved(TEntity instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            if (!newInstances.Remove(instance)) {
                dirtyInstances.Remove(instance);
                if (!removeInstances.Contains(instance)) {
                    removeInstances.Add(instance);
                }
            }
        }

        public void Commit()
        {
            InsertNew();
            UpdateDirty();
            DeleteRemoved();
        }

        protected abstract void InsertNew();

        protected abstract void UpdateDirty();

        protected abstract void DeleteRemoved();

        #endregion

        #region Private methods

        #endregion

        #region IChangeTracking Membres

        void IChangeTracking.AcceptChanges()
        {
            Commit();
        }

        bool IChangeTracking.IsChanged => newInstances.Count > 0 | dirtyInstances.Count > 0 | removeInstances.Count > 0;

        #endregion

        #region IRevertibleChangeTracking Membres

        void IRevertibleChangeTracking.RejectChanges()
        {
            newInstances.Clear();
            dirtyInstances.Clear();
            removeInstances.Clear();
        }

        #endregion
    }
}
