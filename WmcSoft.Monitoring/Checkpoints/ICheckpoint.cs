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

namespace WmcSoft.Diagnostics.Checkpoints
{
    /// <summary>
    /// Represents a diagnostics checkpoint.
    /// </summary>
    public interface ICheckpoint
    {
        /// <summary>
        /// Name of the checkpoint.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Minimum level required to run this diagnostics.
        /// </summary>
        int MinimumLevel { get; }

        /// <summary>Check</summary>
        /// <param name="level">The diagnostics level.</param>
        /// <returns>The result.</returns>
        /// <remarks>If this method throws, the result type will be based 
        /// on the exception type and the message will be the exception message.</remarks>
        CheckpointResult Verify(int level);
    }
}