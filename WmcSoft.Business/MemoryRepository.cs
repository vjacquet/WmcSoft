#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Business
{
    public class MemoryRepository<T> : IRepository<T> where T : class, IUniqueIdentifier
    {
        public static readonly ConcurrentDictionary<string, T> Entities = new ConcurrentDictionary<string, T>();

        public IQueryable<T> Items {
            get { return Entities.Values.AsQueryable(); }
        }

        public T Get(string id) {
            T entity;
            var result = Entities.TryGetValue(id, out entity);
            return !result ? null : entity;
        }

        public T Add(T entity) {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }

            if (entity.Id == null) {
                // legacy code
                var id = Entities.Count > 0 ? Int32.Parse(Entities.Last().Key) : 0;
                id++;
                entity.Id = id.ToString("D");
            }

            var result = Entities.TryAdd(entity.Id, entity);
            return result == false ? null : entity;
        }

        public T Delete(string id) {
            T removed;
            var result = Entities.TryRemove(id, out removed);
            return !result ? null : removed;
        }

        public T Update(T entity) {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }

            if (!Entities.ContainsKey(entity.Id)) {
                return null;
            }

            Entities[entity.Id] = entity;
            return entity;
        }
    }
}
