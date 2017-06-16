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

namespace WmcSoft.Business
{
    public class FilterPipeline<TContext>
    {
        readonly IFilter<TContext>[] _filters;

        public FilterPipeline(params IFilter<TContext>[] rules)
        {
            _filters = rules;
        }

        public void Run(TContext context)
        {
            for (int i = 0; i < _filters.Length; i++) {
                _filters[i].OnExecuting(context);
            }
            for (int i = _filters.Length - 1; i >= 0; i--) {
                _filters[i].OnExecuted(context);
            }
        }
    }
}
