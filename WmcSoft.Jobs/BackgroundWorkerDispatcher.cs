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
using System.Linq;
using System.Text;

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

        readonly Queue<IJob> _queue;
        readonly BackgroundWorker _backgroundWorker;

        #endregion

        #region Lifecycle

        public BackgroundWorkerDispatcher(BackgroundWorker backgroundWorker) {
            _queue = new Queue<IJob>();
            _backgroundWorker = backgroundWorker;
            _backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            while (_queue.Count > 0) {
                if (_backgroundWorker.CancellationPending) {
                    _queue.Clear();
                    return;
                }

                var job = _queue.Dequeue();
                try {
                    job.Execute(this);
                }
                catch (Exception exception) {
                    Trace.TraceError(exception.Message);
                }
            }
        }

        #endregion

        #region IJobDispatcher Membres

        public override void Dispatch(IJob job) {
            _queue.Enqueue(job);
        }

        #endregion
    }
}
