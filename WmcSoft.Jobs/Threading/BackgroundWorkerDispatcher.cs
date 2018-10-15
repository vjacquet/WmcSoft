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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace WmcSoft.Threading
{
    /// <summary>
    /// Represents a dispatcher over a BackgroundWorker.
    /// </summary>
    /// <example>
    /// var backgroundWorker = new BackgroundWorker();
    /// var dispatcher = new BackgroundWorkerDispatcher(backgroundWorker);
    /// 
    /// var waitHandle = new AutoResetEvent(false);
    /// Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e) {
    ///     if (!backgroundWorker.CancellationPending) {
    ///         backgroundWorker.CancelAsync();
    ///         e.Cancel = true;
    ///     }
    /// };
    /// backgroundWorker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e) {
    ///     waitHandle.Set();
    /// };
    /// 
    /// dispatcher.Dispatch(new WhateverJob());
    /// 
    /// backgroundWorker.RunWorkerAsync();
    /// waitHandle.WaitOne();
    /// </example>
    /// <remarks>
    /// Once RunWorkerAsync()has been called on the background worker, 
    /// all jobs must be dispatcher from the background worker.
    /// </remarks>
    public class BackgroundWorkerDispatcher : JobDispatcher
    {
        #region Private Fields

        readonly Queue<IJob> queue;
        readonly BackgroundWorker backgroundWorker;

        #endregion

        #region Lifecycle

        public BackgroundWorkerDispatcher(BackgroundWorker backgroundWorker)
        {
            queue = new Queue<IJob>();
            this.backgroundWorker = backgroundWorker;
            this.backgroundWorker.DoWork += OnDoWork;
        }

        private void OnDoWork(object sender, DoWorkEventArgs e)
        {
            while (queue.Count > 0) {
                if (backgroundWorker.CancellationPending) {
                    queue.Clear();
                    return;
                }

                var job = queue.Dequeue();
                try {
                    job.Execute(this);
                } catch (Exception exception) {
                    Trace.TraceError(exception.Message);
                }
            }
        }

        #endregion

        #region IJobDispatcher Membres

        public override void Dispatch(IJob job)
        {
            queue.Enqueue(job);
        }

        public override bool SupportsCancellation => backgroundWorker.WorkerSupportsCancellation;

        public override void CancelAsync()
        {
            backgroundWorker.CancelAsync();
        }

        public override bool CancellationPending => backgroundWorker.CancellationPending;

        #endregion
    }
}
