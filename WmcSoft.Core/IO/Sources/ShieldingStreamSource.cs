#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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
using System.IO;

namespace WmcSoft.IO.Sources
{
    /// <summary>
    /// Decorates a stream source that catches all exceptions.
    /// </summary>
    public class ShieldingStreamSource<TSource> : IStreamSource
        where TSource : IStreamSource
    {
        private readonly TSource underlying;

        public ShieldingStreamSource(TSource source)
        {
            underlying = source;
        }

        public Exception Error { get; private set; }

        public ShieldingStreamSource(Func<TSource> factory)
        {
            try {
                underlying = factory();
            } catch (Exception e) {
                Error = e;
                Trace.TraceError("Failed to initialized the underlying source. {0}", e);
            }
        }

        protected TResult Run<TResult>(string methodName, Func<TSource, TResult> func, TResult defaultResult = default)
        {
            Error = null; // reset the error

            if (underlying == null)
                return defaultResult;

            try {
                return func(underlying);
            } catch (Exception e) {
                Error = e;
                Trace.TraceError("Failed to run `{1}` underlying source. {0}", e, methodName);
                return defaultResult;
            }
        }

        public Stream OpenSource()
        {
            return Run(nameof(OpenSource), s => s.OpenSource());
        }

        public override string ToString()
        {
            return underlying != null ? ('[' + underlying.ToString() + ']') : "[null]";
        }
    }

    public class ShieldingStreamSource : ShieldingStreamSource<IStreamSource>
    {
        public ShieldingStreamSource(IStreamSource source) : base(source)
        {
        }

        public ShieldingStreamSource(Func<IStreamSource> factory) : base(factory)
        {
        }
    }
}
