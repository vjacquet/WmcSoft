﻿#region Licence

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WmcSoft;
using WmcSoft.Threading;

namespace WmcSoft.Threading
{
    public class SequenceJob : JobBase
    {
        #region Private Fields

        Queue<IJob> queue;

        #endregion

        #region Life cycle

        public SequenceJob() {
            queue = new Queue<IJob>();
        }

        public SequenceJob(params IJob[] jobs)
            : this() {
            for (int i = 0; i < jobs.Length; i++) {
                queue.Enqueue(jobs[i]);
            }
        }

        #endregion

        #region Methods

        public SequenceJob ContinueWith(IJob job) {
            queue.Enqueue(job);
            return this;
        }

        #endregion

        #region Overrides

        protected override void DoExecute(IServiceProvider serviceProvider) {
            if(queue.Count > 0) {
                IJob job = queue.Dequeue();
                job.Execute(serviceProvider);

                if (queue.Count > 0) {
                    JobDispatcher dispatcher = serviceProvider.GetService<JobDispatcher>();
                    dispatcher.Dispatch(this);
                }
            }
        }

        #endregion

    }
}