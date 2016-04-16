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
    public abstract class UnitOfWork<TEntity> : IRevertibleChangeTracking where TEntity : class
    {
        #region Private fields

        protected readonly IList<TEntity> _newInstances;
        protected readonly IList<TEntity> _dirtyInstances;
        protected readonly IList<TEntity> _removeInstances;
        protected readonly IdentityMap _identityMap;

        #endregion

        #region Lifecycle

        protected UnitOfWork() {
            _identityMap = new IdentityMap();
            _newInstances = new List<TEntity>();
            _dirtyInstances = new List<TEntity>();
            _removeInstances = new List<TEntity>();
        }

        #endregion

        #region Methods

        public void RegisterNew(TEntity instance) {
            if (instance == null)
                throw new ArgumentNullException("instance");
            Trace.Assert(!_dirtyInstances.Contains(instance), "Domain object should not be dirty");
            Trace.Assert(!_removeInstances.Contains(instance), "Domain object should not be removed");
            Trace.Assert(!_newInstances.Contains(instance), "Domain object should not already be registered as new");
            _newInstances.Add(instance);
        }

        public void RegisterDirty(TEntity instance) {
            if (instance == null)
                throw new ArgumentNullException("instance");
            Trace.Assert(!_removeInstances.Contains(instance), "Domain object should not be removed");
            if (!_dirtyInstances.Contains(instance) && !_newInstances.Contains(instance)) {
                _dirtyInstances.Add(instance);
            }
        }

        public void RegisterClean(TEntity instance) {
            _identityMap.Register(instance);
        }

        public void RegisterRemoved(TEntity instance) {
            if (instance == null)
                throw new ArgumentNullException("instance");
            if (!_newInstances.Remove(instance)) {
                _dirtyInstances.Remove(instance);
                if (!_removeInstances.Contains(instance)) {
                    _removeInstances.Add(instance);
                }
            }
        }

        public void Commit() {
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

        void IChangeTracking.AcceptChanges() {
            Commit();
        }

        bool IChangeTracking.IsChanged
        {
            get {
                return _newInstances.Count > 0 | _dirtyInstances.Count > 0 | _removeInstances.Count > 0;
            }
        }

        #endregion

        #region IRevertibleChangeTracking Membres

        void IRevertibleChangeTracking.RejectChanges() {
            _newInstances.Clear();
            _dirtyInstances.Clear();
            _removeInstances.Clear();
        }

        #endregion
    }
}
