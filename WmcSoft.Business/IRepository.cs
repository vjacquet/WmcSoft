using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Business
{
    public interface IRepository<TKey, TValue> where TValue : IUniqueIdentifier<TKey>
    {
        TValue Add(TValue entity);
        TValue Delete(TKey key);
        TValue Get(TKey key);
        TValue Update(TValue entity);
        IQueryable<TValue> Items { get; }
    }
}


public class MemoryRepository<T> : IRepository<T> where T : class, IUniqueIdentifier
{
    public static readonly ConcurrentDictionary<string, T> Entities = new ConcurrentDictionary<string, T>();

    public IQueryable<T> Items
    {
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




    /// <summary>
    /// Utility class to store values and and keep track of wether it changed or not.
    /// </summary>
    /// <typeparam name="T">The type of the value to store.</typeparam>
    /// <remarks>Think of a better name</remarks>
    [DebuggerDisplay("{_value,nq}{_changed==1 ? \"*\" : \"\",nq}")]
    public class Publishable<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T _value;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _changed;
        private readonly IEqualityComparer<T> _comparer;

        public Publishable() {
            _comparer = EqualityComparer<T>.Default;
        }

        public Publishable(T value, IEqualityComparer<T> comparer) {
            _comparer = comparer;
            _value = value;
        }

        public Publishable(IEqualityComparer<T> comparer) {
            _comparer = comparer;
        }

        public Publishable(T value)
            : this() {
            _value = value;
        }

        /// <summary>
        /// Updates the stored value
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>Returns true if the value has changed.</returns>
        /// <remarks>We always keep the most recent value, wether it changed or not.
        /// It maintenains the lifetime of references short.</remarks>
        public bool Update(T value) {
            T current = _value;
            _value = value;
            if (_changed == 0 && !_comparer.Equals(current, value))
                _changed = 1;
            return _changed != 0;
        }

        /// <summary>
        /// Returns the stored value and a boolean value indicated if it changed or not since last time.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The dirty flag</returns>
        public bool TryGetChanges(out T value) {
            value = _value;
            var changed = Interlocked.CompareExchange(ref _changed, 0, 1);
            return changed == 1;
        }

        /// <summary>
        /// Forces the dirty flag to true.
        /// </summary>
        public void MarkAsDirty() {
            _changed = 1;
        }

        /// <summary>
        /// The value
        /// </summary>
        public T Value
        {
            get {
                return _value;
            }
        }

        /// <summary>
        /// Implicit cast to get the stored value.
        /// </summary>
        /// <param name="self">The instance</param>
        /// <returns>Its stored value.</returns>
        public static implicit operator T(Publishable<T> self) {
            return self.Value;
        }
    }

    public static class ChangeWatcherExtensions
    {
        /// <summary>
        /// Gets the spot value of a publishable over an array.
        /// </summary>
        /// <typeparam name="T">The type of the items in the array</typeparam>
        /// <param name="self">The publishable</param>
        /// <param name="defaultValue">The default value to return when the array is empty</param>
        /// <returns>The spot value or default(T) when the array is empty.</returns>
        /// <remarks>Gets the value once</remarks>
        public static T Spot<T>(this Publishable<T[]> self, T defaultValue = default(T)) {
            var value = self.Value;
            if (value.Length > 0)
                return value[0];
            return defaultValue;
        }

        /// <summary>
        /// Applies a function on the spot value of a publishable over an array.
        /// </summary>
        /// <typeparam name="T">The type of the items in the array</typeparam>
        /// <typeparam name="TResult">The return type of the function to apply</typeparam>
        /// <param name="self">The publishable</param>
        /// <param name="selector">The function to apply</param>
        /// <param name="defaultValue">The default value to return when the array is empty</param>
        /// <returns></returns>
        /// <remarks>Gets the value once</remarks>
        public static TResult Spot<T, TResult>(this Publishable<T[]> self, Func<T, TResult> selector, TResult defaultValue = default(TResult)) {
            var value = self.Value;
            if (value.Length > 0)
                return selector(value[0]);
            return defaultValue;
        }

        /// <summary>
        /// Gets the spot value of an array.
        /// </summary>
        /// <typeparam name="T">The type of the items in the array</typeparam>
        /// <param name="value">The array</param>
        /// <param name="defaultValue">The default value to return when the array is empty</param>
        /// <returns>The spot value or default(T) when the array is empty.</returns>
        /// <remarks>Gets the value once</remarks>
        public static T Spot<T>(this T[] value, T defaultValue = default(T)) {
            if (value.Length > 0)
                return value[0];
            return defaultValue;
        }

        /// <summary>
        /// Applies a function on the spot value of an array.
        /// </summary>
        /// <typeparam name="T">The type of the items in the array</typeparam>
        /// <typeparam name="TResult">The return type of the function to apply</typeparam>
        /// <param name="value">The array</param>
        /// <param name="selector">The function to apply</param>
        /// <param name="defaultValue">The default value to return when the array is empty</param>
        /// <returns></returns>
        /// <remarks>Gets the value once</remarks>
        public static TResult Spot<T, TResult>(this T[] value, Func<T, TResult> selector, TResult defaultValue = default(TResult)) {
            if (value.Length > 0)
                return selector(value[0]);
            return defaultValue;
        }
    }