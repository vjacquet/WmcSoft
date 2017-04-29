#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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
    public class ActionsUnitOfWork<TEntity> : UnitOfWork<TEntity>
        where TEntity : class
    {
        private readonly Action<IList<TEntity>> _create;
        private readonly Action<IList<TEntity>> _update;
        private readonly Action<IList<TEntity>> _delete;

        public ActionsUnitOfWork(Action<IList<TEntity>> create, Action<IList<TEntity>> update, Action<IList<TEntity>> delete)
        {
            _create = create;
            _update = update;
            _delete = delete;
        }

        protected override void DeleteRemoved()
        {
            _delete(_removeInstances);
        }

        protected override void InsertNew()
        {
            _create(_newInstances);
        }

        protected override void UpdateDirty()
        {
            _update(_dirtyInstances);
        }
    }
}