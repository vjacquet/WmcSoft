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

using System.Collections.Generic;

namespace WmcSoft.Business
{
    public class TaskResult
    {
        #region Fields

        static readonly TaskResult _success = new TaskResult(true);

        private readonly bool _succeeded;
        private readonly TaskError[] _errors;

        #endregion

        #region Lifecyle

        protected TaskResult(bool succeeded) {
            _succeeded = succeeded;
        }
        protected TaskResult(params TaskError[] errors) {
            _succeeded = false;
            _errors = errors;
        }

        public static TaskResult Success {
            get { return _success; }
        }

        public static TaskResult Failed(params TaskError[] errors) {
            return new TaskResult(errors);
        }

        #endregion

        #region Properties

        public bool Succeeded { get { return _succeeded; } }
        public IReadOnlyCollection<TaskError> Errors { get { return _errors; } }

        #endregion
    }

}
