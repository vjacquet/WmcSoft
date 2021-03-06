﻿#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Monitoring.Checkpoints
{
    /// <summary>
    /// Checkpoint based on an Action. 
    /// </summary>
    /// <remarks>Throw exceptions to notify warnings or errors.</remarks>
    public sealed class ActionCheckpoint : CheckpointBase
    {
        private readonly Action _action;
        private readonly int _minimumLevel;

        public ActionCheckpoint(string name, Action action, int minimumLevel = 0)
            : base(name)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            _action = action;
            _minimumLevel = minimumLevel;
        }

        public override int MinimumLevel => _minimumLevel;

        protected override CheckpointResult DoVerify(int level)
        {
            _action();
            return Success();
        }
    }
}
