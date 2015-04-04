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
using System.Linq;
using System.Text;

namespace WmcSoft.Threading
{
    public class JobDispatcherDecorator : JobDispatcher
    {
        #region Private fields

        JobDispatcher _jobDispatcher;
        protected JobDispatcher Inner {
            get { return _jobDispatcher; }
        }

        #endregion

        #region Lifecycle

        protected JobDispatcherDecorator(JobDispatcher jobDispatcher) {
            _jobDispatcher = jobDispatcher;
        }

        #endregion

        #region Overrides

        public override void CancelAsync() {
            _jobDispatcher.CancelAsync();
        }

        public override bool CancellationPending {
            get {
                return _jobDispatcher.CancellationPending;
            }
        }

        public override bool SupportsCancellation {
            get {
                return _jobDispatcher.SupportsCancellation;
            }
        }

        public override void Dispatch(IJob job) {
            _jobDispatcher.Dispatch(job);
        }

        protected override void Dispose(bool disposing) {
            if (_jobDispatcher != null) {
                _jobDispatcher.Dispose();
                _jobDispatcher = null;
            }
            base.Dispose(disposing);
        }

        public override object GetService(Type serviceType) {
            if (serviceType == typeof(JobDispatcher))
                return this;
            object instance = base.GetService(serviceType);
            if (instance == null)
                instance = _jobDispatcher.GetService(serviceType);
            return instance;
        }

        #endregion
    }
}
