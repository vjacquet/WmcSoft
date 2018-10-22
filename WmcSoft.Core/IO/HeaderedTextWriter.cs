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

using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.IO
{
    /// <summary>
    /// Decorates a <see cref="TextWriter"/> to write a header line when the first char is written.
    /// </summary>
    public class HeaderedTextWriter : TextWriter
    {
        private readonly TextWriter underlying;
        private readonly string header;
        private bool wroteHeader;

        public HeaderedTextWriter(TextWriter writer, string header)
        {
            underlying = writer;
            this.header = header;
        }

        public override Encoding Encoding => underlying.Encoding;

        public override void Close()
        {
            underlying.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                underlying.Dispose();
            }
        }

        public override void Flush()
        {
            underlying.Flush();
        }

        protected void WriteHeaderIfNecessary()
        {
            if (!wroteHeader) {
                underlying.WriteLine(header);
                wroteHeader = true;
            }
        }

        public override void Write(char value)
        {
            WriteHeaderIfNecessary();
            underlying.Write(value);
        }

        public override void Write(char[] buffer)
        {
            WriteHeaderIfNecessary();
            underlying.Write(buffer);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            WriteHeaderIfNecessary();
            underlying.Write(buffer, index, count);
        }

        public override void Write(string value)
        {
            WriteHeaderIfNecessary();
            underlying.Write(value);
        }

        public override void Write(object value)
        {
            WriteHeaderIfNecessary();
            underlying.Write(value);
        }

        public override void Write(string format, object arg0)
        {
            WriteHeaderIfNecessary();
            underlying.Write(format, arg0);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            WriteHeaderIfNecessary();
            underlying.Write(format, arg0, arg1);
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            WriteHeaderIfNecessary();
            underlying.Write(format, arg0, arg1, arg2);
        }

        public override void Write(string format, params object[] arg)
        {
            WriteHeaderIfNecessary();
            underlying.Write(format, arg);
        }

        public override Task WriteAsync(char value)
        {
            WriteHeaderIfNecessary();
            return underlying.WriteAsync(value);
        }

        public override Task WriteAsync(char[] buffer, int index, int count)
        {
            WriteHeaderIfNecessary();
            return underlying.WriteAsync(buffer, index, count);
        }

        public override Task WriteAsync(string value)
        {
            WriteHeaderIfNecessary();
            return underlying.WriteAsync(value);
        }

        public override void WriteLine()
        {
            WriteHeaderIfNecessary();
            underlying.WriteLine();
        }

        public override void WriteLine(char value)
        {
            WriteHeaderIfNecessary();
            underlying.WriteLine(value);
        }

        public override void WriteLine(char[] buffer)
        {
            WriteHeaderIfNecessary();
            underlying.WriteLine(buffer);
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            WriteHeaderIfNecessary();
            underlying.WriteLine(buffer, index, count);
        }

        public override void WriteLine(string value)
        {
            WriteHeaderIfNecessary();
            underlying.WriteLine(value);
        }

        public override void WriteLine(object value)
        {
            WriteHeaderIfNecessary();
            underlying.WriteLine(value);
        }

        public override void WriteLine(string format, object arg0)
        {
            WriteHeaderIfNecessary();
            underlying.WriteLine(format, arg0);
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            WriteHeaderIfNecessary();
            underlying.WriteLine(format, arg0, arg1);
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            WriteHeaderIfNecessary();
            underlying.WriteLine(format, arg0, arg1, arg2);
        }

        public override void WriteLine(string format, params object[] arg)
        {
            WriteHeaderIfNecessary();
            underlying.WriteLine(format, arg);
        }

        public override Task WriteLineAsync(char value)
        {
            WriteHeaderIfNecessary();
            return underlying.WriteLineAsync(value);
        }

        public override Task WriteLineAsync(char[] buffer, int index, int count)
        {
            WriteHeaderIfNecessary();
            return underlying.WriteLineAsync(buffer, index, count);
        }

        public override Task WriteLineAsync(string value)
        {
            WriteHeaderIfNecessary();
            return underlying.WriteLineAsync(value);
        }
    }
}
