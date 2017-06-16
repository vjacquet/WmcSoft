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

 ****************************************************************************
 * this code is adapted from the series
 * <https://blogs.msdn.microsoft.com/mattwar/2008/11/18/linq-building-an-iqueryable-provider-series/>
 * 
 ****************************************************************************/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Linq
{
    public class Queryable<T> : IOrderedQueryable<T>
    {
        readonly QueryProviderBase _provider;
        readonly Expression _expression;

        public Queryable(QueryProviderBase provider) {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            _provider = provider;
            _expression = Expression.Constant(this);
        }

        public Queryable(QueryProviderBase provider, Expression expression) {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type)) throw new ArgumentOutOfRangeException(nameof(expression));

            _provider = provider;
            _expression = expression;
        }

        Expression IQueryable.Expression {
            get { return _expression; }
        }

        Type IQueryable.ElementType {
            get { return typeof(T); }
        }

        IQueryProvider IQueryable.Provider {
            get { return _provider; }
        }
        public IEnumerator<T> GetEnumerator() {
            return ((IEnumerable<T>)_provider.Execute(_expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override string ToString() {
            return _provider.GetQueryText(_expression);
        }
    }

}
