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
using System.ComponentModel;

namespace WmcSoft.Diagnostics.Checkpoints
{
    /// <summary>
    /// Base implementation of a <see cref="Checkpoint"/>.
    /// </summary>
    public abstract class CheckpointBase : ICheckpoint
    {
        protected CheckpointBase(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public string Name { get; }
        public virtual int MinimumLevel => 0;

        public CheckpointResult Verify(int level)
        {
            if (level < MinimumLevel)
                return Skipped();

            try {
                return DoVerify(level);
            } catch (WarningException exception) {
                return Warn(exception.Message);
            } catch (Exception exception) {
                return Error(exception.Message);
            }
        }

        protected abstract CheckpointResult DoVerify(int level);

        #region Helpers

        protected CheckpointResult Skipped()
        {
            return new CheckpointResult(CheckpointResultType.Info, "Skipped");
        }

        protected CheckpointResult Success()
        {
            return new CheckpointResult(CheckpointResultType.Success, null);
        }

        protected CheckpointResult Failed()
        {
            return new CheckpointResult(CheckpointResultType.Failed, null);
        }

        protected CheckpointResult Info(string message)
        {
            return new CheckpointResult(CheckpointResultType.Info, message);
        }

        protected CheckpointResult Info(string format, object arg0)
        {
            return new CheckpointResult(CheckpointResultType.Info, String.Format(format, arg0));
        }

        protected CheckpointResult Info(string format, object arg0, object arg1)
        {
            return new CheckpointResult(CheckpointResultType.Info, String.Format(format, arg0, arg1));
        }

        protected CheckpointResult Info(string format, object arg0, object arg1, object arg2)
        {
            return new CheckpointResult(CheckpointResultType.Info, String.Format(format, arg0, arg1, arg2));
        }

        protected CheckpointResult Info(string format, params object[] args)
        {
            return new CheckpointResult(CheckpointResultType.Info, String.Format(format, args));
        }

        protected CheckpointResult Warn(string message)
        {
            return new CheckpointResult(CheckpointResultType.Warn, message);
        }

        protected CheckpointResult Warn(string format, object arg0)
        {
            return new CheckpointResult(CheckpointResultType.Warn, String.Format(format, arg0));
        }

        protected CheckpointResult Warn(string format, object arg0, object arg1)
        {
            return new CheckpointResult(CheckpointResultType.Warn, String.Format(format, arg0, arg1));
        }

        protected CheckpointResult Warn(string format, object arg0, object arg1, object arg2)
        {
            return new CheckpointResult(CheckpointResultType.Warn, String.Format(format, arg0, arg1, arg2));
        }

        protected CheckpointResult Warn(string format, params object[] args)
        {
            return new CheckpointResult(CheckpointResultType.Info, String.Format(format, args));
        }

        protected CheckpointResult Error(string message)
        {
            return new CheckpointResult(CheckpointResultType.Error, message);
        }

        protected CheckpointResult Error(string format, object arg0)
        {
            return new CheckpointResult(CheckpointResultType.Error, String.Format(format, arg0));
        }

        protected CheckpointResult Error(string format, object arg0, object arg1)
        {
            return new CheckpointResult(CheckpointResultType.Error, String.Format(format, arg0, arg1));
        }

        protected CheckpointResult Error(string format, object arg0, object arg1, object arg2)
        {
            return new CheckpointResult(CheckpointResultType.Info, String.Format(format, arg0, arg1, arg2));
        }

        protected CheckpointResult Error(string format, params object[] args)
        {
            return new CheckpointResult(CheckpointResultType.Error, String.Format(format, args));
        }

        #endregion
    }
}