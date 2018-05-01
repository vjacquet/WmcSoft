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

using System.Collections.Generic;
using System.Threading.Tasks;

namespace WmcSoft.Business
{
    public class AsyncFilterPipeline<TContext>
    {
        readonly IAsyncFilter<TContext>[] _filters;

        public AsyncFilterPipeline(params IAsyncFilter<TContext>[] rules)
        {
            _filters = rules;
        }

        static Task RunFilter(IEnumerator<IAsyncFilter<TContext>> enumerator, TContext context)
        {
            if (enumerator.MoveNext())
                return enumerator.Current.OnExecutionAsync(context, () => RunFilter(enumerator, context));
            return Task.CompletedTask;
        }

        public Task RunAsync(TContext context)
        {
            var filters = _filters.AsEnumerable<IAsyncFilter<TContext>>().GetEnumerator();
            return RunFilter(filters, context);
        }
    }
}
