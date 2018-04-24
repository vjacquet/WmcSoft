#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Business
{
    public static class ManyToMany<TSource, TTarget>
    {
        class ManyToManyCollectionAdapter<TJoinEntity> : ICollection<TTarget>
        {
            private readonly ICollection<TJoinEntity> _collection;
            private readonly Func<TJoinEntity, TTarget> _selector;
            private readonly Func<TTarget, TJoinEntity> _creator;

            public ManyToManyCollectionAdapter(ICollection<TJoinEntity> collection, Func<TJoinEntity, TTarget> selector, Func<TTarget, TJoinEntity> creator)
            {
                _collection = collection;
                _selector = selector;
                _creator = creator;
            }

            public int Count => _collection.Count;

            public bool IsReadOnly => _collection.IsReadOnly;

            public void Add(TTarget item)
            {
                if (!Contains(item)) {
                    var join = _creator(item);
                    _collection.Add(join);
                }
            }

            public void Clear()
            {
                _collection.Clear();
            }

            public bool Contains(TTarget item)
            {
                return _collection.Any(x => Equals(_selector(x), item));
            }

            public void CopyTo(TTarget[] array, int arrayIndex)
            {
                foreach (var x in _collection) {
                    array[arrayIndex++] = _selector(x);
                }
            }

            public IEnumerator<TTarget> GetEnumerator()
            {
                return _collection.Select(_selector).GetEnumerator();
            }

            public bool Remove(TTarget item)
            {
                foreach (var x in _collection) {
                    if (Equals(_selector(x), item)) {
                        return _collection.Remove(x);
                    }
                }
                return false;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public static ICollection<TTarget> Adapt<TJoinEntity>(TSource source, ICollection<TJoinEntity> collection)
            where TJoinEntity : INavigable<TSource>, INavigable<TTarget>, new()
        {
            TTarget Select(TJoinEntity join)
            {
                return join.NavigateTo<TTarget>();
            }

            TJoinEntity Create(TTarget target)
            {
                var join = new TJoinEntity();
                ((INavigable<TSource>)join).Target = source;
                ((INavigable<TTarget>)join).Target = target;
                return join;
            }

            return new ManyToManyCollectionAdapter<TJoinEntity>(collection, Select, Create);
        }

        public static ICollection<TTarget> Adapt<TJoinEntity>(TSource source, ICollection<TJoinEntity> collection, Func<TJoinEntity, TTarget> selector, Func<TTarget, TJoinEntity> creator)
        {
            return new ManyToManyCollectionAdapter<TJoinEntity>(collection, selector, creator);
        }

        public static ICollection<TSource> Adapt<TJoinEntity>(TTarget target, ICollection<TJoinEntity> collection)
            where TJoinEntity : INavigable<TSource>, INavigable<TTarget>, new()
        {
            return ManyToMany<TTarget, TSource>.Adapt(target, collection);
        }

        public static ICollection<TSource> Adapt<TJoinEntity>(TTarget target, ICollection<TJoinEntity> collection, Func<TJoinEntity, TSource> selector, Func<TSource, TJoinEntity> creator)
        {
            return ManyToMany<TTarget, TSource>.Adapt(target, collection, selector, creator);
        }
    }
}
