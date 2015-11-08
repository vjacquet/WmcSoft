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

using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using WmcSoft.Reflection;

namespace WmcSoft.Configuration
{
    public abstract class ConfigurationElementCollection<T> : ConfigurationElementCollection, ICollection<T>
        where T : ConfigurationElement, new()
    {
        readonly ConfigurationElementCollectionType _collectionType;

        protected ConfigurationElementCollection() {
            var attr = GetType().GetCustomAttributes<ConfigurationCollectionAttribute>(true).FirstOrDefault();
            if (attr != null) {
                _collectionType = attr.CollectionType;
                if (!attr.AddItemName.Contains(','))
                    AddElementName = attr.AddItemName;
                RemoveElementName = attr.RemoveItemName;
                ClearElementName = attr.ClearItemsName;
            } else {
                _collectionType = base.CollectionType;
            }
        }

        public override ConfigurationElementCollectionType CollectionType {
            get { return _collectionType; }
        }

        protected override ConfigurationElement CreateNewElement() {
            return new T();
        }

        protected virtual object GetElementKey(T element) {
            return element.ElementInformation.Properties.Cast<PropertyInformation>()
                .Where(p => p.IsKey)
                .Single();
        }
        protected sealed override object GetElementKey(ConfigurationElement element) {
            return GetElementKey((T)element);
        }

        #region ICollection<T> Membres

        public void Add(T item) {
            BaseAdd(item);
        }

        public bool Contains(T item) {
            return BaseIndexOf(item) >= 0;
        }

        public bool Remove(T item) {
            var count = Count;
            BaseRemove(GetElementKey(item));
            return Count != count;
        }

        bool ICollection<T>.IsReadOnly {
            get { return base.IsReadOnly(); }
        }

        void ICollection<T>.Clear() {
            BaseClear();
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) {
            CopyTo(array, arrayIndex);
        }

        #endregion

        #region IEnumerable<T> Membres

        public new IEnumerator<T> GetEnumerator() {
            var enumerator = base.GetEnumerator();
            while (enumerator.MoveNext())
                yield return (T)enumerator.Current;
        }

        #endregion
    }
}
