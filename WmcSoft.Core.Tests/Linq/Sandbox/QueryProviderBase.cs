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
    public abstract class QueryProviderBase : IQueryProvider
    {
        protected QueryProviderBase() {
        }

        IQueryable<S> IQueryProvider.CreateQuery<S>(Expression expression) {
            return new Queryable<S>(this, expression);
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression) {
            var elementType = GetElementType(expression);
            try {
                return (IQueryable)Activator.CreateInstance(typeof(Queryable<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException tie) {
                throw tie.InnerException;
            }
        }

        S IQueryProvider.Execute<S>(Expression expression) {
            return (S)Execute(expression);
        }

        object IQueryProvider.Execute(Expression expression) {
            return Execute(expression);
        }

        protected virtual Type GetElementType(Expression expression) {
            var type = expression.Type;
            return type.GetGenericElementType() ?? type;
        }

        public abstract string GetQueryText(Expression expression);

        public abstract object Execute(Expression expression);
    }
}
