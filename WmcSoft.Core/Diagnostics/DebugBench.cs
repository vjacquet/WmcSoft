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
using System.Diagnostics;

namespace WmcSoft.Diagnostics
{
    /// <summary>
    /// Utility class to bench a portion of code only in Debug mode.
    /// </summary>
    /// <remarks>The duration is printed to to the trace listeners in the <see cref="Debug.Listeners"/> collection.</remarks>
    public struct DebugBench : IDisposable
    {
        private readonly string _scopeName;
        private Stopwatch _stopwatch;

        public DebugBench(string scopeName)
        {
            _scopeName = scopeName;
            _stopwatch = null;
            Start();
        }

        [Conditional("DEBUG")]
        private void Start()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        [Conditional("DEBUG")]
        private void Stop()
        {
            _stopwatch.Stop();
            Debug.WriteLine("{0} took {1}ms", _scopeName, _stopwatch.Elapsed.TotalMilliseconds);
        }

        public void Dispose()
        {
            Stop();
        }
    }
}