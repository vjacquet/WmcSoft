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
