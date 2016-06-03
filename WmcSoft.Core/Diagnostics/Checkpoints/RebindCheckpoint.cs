#region Licence

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

namespace WmcSoft.Diagnostics.Checkpoints
{
    /// <summary>
    /// Decorates a checkpoint to change its level, so that the user can override the implementer settings.
    /// </summary>
    public sealed class RebindCheckpoint : CheckpointBase
    {
        private readonly ICheckpoint _base;
        private readonly Func<int, int> _binder;
        private readonly int _minimumLevel;

        static string ExtractName(ICheckpoint checkpoint) {
            if (checkpoint == null) throw new ArgumentNullException("checkpoint");
            return checkpoint.Name;
        }

        public RebindCheckpoint(ICheckpoint checkpoint, int minimumLevel, Func<int, int> binder)
            : base(ExtractName(checkpoint)) {
            if (binder == null) throw new ArgumentNullException("binder");

            _base = checkpoint;
            _minimumLevel = minimumLevel;
            _binder = binder;
        }

        public override int MinimumLevel {
            get { return Math.Min(_minimumLevel, _base.MinimumLevel); }
        }

        protected override CheckpointResult DoVerify(int level) {
            level = _binder(level);
            return _base.Verify(level);
        }
    }
}
