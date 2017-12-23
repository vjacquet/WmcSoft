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

namespace WmcSoft.IO
{
    /// <summary>
    /// Decorates a <see cref="TextWriter"/> to write a header line when the first char is written.
    /// </summary>
    public class HeaderedTextWriter : TextWriter
    {
        private readonly TextWriter _wrapped;
        private readonly string _header;
        private bool _wroteHeader;

        public HeaderedTextWriter(TextWriter writer, string header)
        {
            _wrapped = writer;
            _header = header;
        }

        public override Encoding Encoding => _wrapped.Encoding;

        public override void Close()
        {
            _wrapped.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _wrapped.Dispose();
            }
        }

        public override void Flush()
        {
            _wrapped.Flush();
        }

        public override void Write(char value)
        {
            if (!_wroteHeader) {
                _wrapped.WriteLine(_header);
                _wroteHeader = true;
            }
            _wrapped.Write(value);
        }
    }
}
