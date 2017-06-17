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

namespace WmcSoft.Business
{
    public class CancellableFilterPipeline<TContext>
    {
        readonly Predicate<TContext> _cancels;
        readonly IFilter<TContext>[] _filters;

        public CancellableFilterPipeline(Predicate<TContext> cancels, params IFilter<TContext>[] filters)
        {
            _cancels = cancels;
            _filters = filters;
        }

        public void Run(TContext context)
        {
            var i = 0;
            for (i = 0; i < _filters.Length; i++) {
                if (_cancels(context))
                    break;
                _filters[i].OnExecuting(context);
            }
            for (i = i - 1; i >= 0; i--) {
                _filters[i].OnExecuted(context);
            }
        }
    }
}
