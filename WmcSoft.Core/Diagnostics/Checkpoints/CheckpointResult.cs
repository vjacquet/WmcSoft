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
using System.Collections.Generic;

namespace WmcSoft.Diagnostics.Checkpoints
{
    /// <summary>
    /// Checkpoint's result builder.
    /// </summary>
    public class CheckpointResult
    {
        private readonly List<string> _lines;

        public CheckpointResult(CheckpointResultType resultType, string message) {
            ResultType = resultType;
            _lines = new List<string> { message };
            if (!string.IsNullOrEmpty(message))
                _lines.Add(message);
        }

        public CheckpointResult(string message)
            : this(CheckpointResultType.Info, message) {
        }

        public CheckpointResultType ResultType { get; private set; }
        public IReadOnlyCollection<string> Lines { get { return _lines; } }

        /// <summary>
        /// Sets the result type only if it has a stronger meaning.
        /// </summary>
        /// <param name="resultType">The new result type</param>
        /// <return>The new result type</return>
        public CheckpointResult OverrideResultType(CheckpointResultType resultType) {
            if ((int)resultType > (int)ResultType)
                ResultType = resultType;
            return this;
        }

        protected virtual void DoWriteLine(object line) {
            if (line != null)
                _lines.Add(line.ToString());
        }

        public CheckpointResult WriteLine(string line) {
            DoWriteLine(line);
            return this;
        }

        public CheckpointResult WriteLine(object line) {
            return WriteLine(line);
        }

        public CheckpointResult WriteLine(IFormatProvider provider, string format, params object[] args) {
            return WriteLine(string.Format(provider, format, args));
        }
        public CheckpointResult WriteLine(string format, object arg0) {
            return WriteLine(string.Format(format, arg0));
        }
        public CheckpointResult WriteLine(string format, object arg0, object arg1) {
            return WriteLine(string.Format(format, arg0, arg1));
        }
        public CheckpointResult WriteLine(string format, object arg0, object arg1, object arg2) {
            return WriteLine(string.Format(format, arg0, arg1, arg2));
        }
        public CheckpointResult WriteLine(string format, params object[] args) {
            return WriteLine(string.Format(format, args));
        }
    }
}