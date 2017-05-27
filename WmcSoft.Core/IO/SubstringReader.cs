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

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WmcSoft.IO
{
    /// <summary>
    /// Implements a <see cref="TextReader"/> that reads from a substring.
    /// </summary>
    /// <remarks>The substring is not created.</remarks>
    public class SubstringReader : TextReader
    {
        private string _s;
        private int _pos;
        private int _end;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubstringReader"/> class that reads from the specified string.
        /// </summary>
        /// <param name="s">The string to which the <see cref="SubstringReader"/> should be initialized. </param>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance. </param>
        /// <param name="length">The number of characters in the substring. </param>
        public SubstringReader(string s, int startIndex, int length)
        {
            if (s == null) throw new ArgumentNullException("s");
            if (startIndex < 0 || startIndex > s.Length) throw new ArgumentOutOfRangeException("startIndex");
            if (length < 0 || startIndex > (s.Length - length)) throw new ArgumentOutOfRangeException("length");

            _s = s;
            _pos = startIndex;
            _end = startIndex + length;
        }

        public override void Close()
        {
            Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            _s = null;
            _pos = 0;
            _end = 0;
            base.Dispose(disposing);
        }

        public override int Peek()
        {
            if (_s == null) throw new ObjectDisposedException(null);
            if (_pos == _end)
                return -1;
            return _s[_pos];
        }

        public override int Read()
        {
            if (_s == null) throw new ObjectDisposedException(null);
            if (_pos == _end)
                return -1;
            return _s[_pos++];
        }

        public override int Read(char[] buffer, int index, int count)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (index < 0) throw new ArgumentOutOfRangeException("index");
            if (count < 0) throw new ArgumentOutOfRangeException("count");
            if (buffer.Length - index < count) throw new ArgumentException();

            if (_s == null) throw new ObjectDisposedException(null);

            int n = Math.Min(_end - _pos, count);
            _s.CopyTo(_pos, buffer, index, n);
            _pos += n;
            return n;
        }

        public override string ReadToEnd()
        {
            if (_s == null) throw new ObjectDisposedException(null);

            var s = (_pos == 0 && _end == _s.Length)
                ? _s
                : _s.Substring(_pos, _end - _pos);
            _pos = _end;
            return s;
        }

        public override string ReadLine()
        {
            if (_s == null) throw new ObjectDisposedException(null);

            var i = _pos;
            while (i < _end) {
                var c = _s[i];
                if (c == '\r' || c == '\n') {
                    var result = _s.Substring(_pos, i - _pos);
                    _pos = i + 1;
                    if (c == '\r' && _pos < _end && _s[_pos] == '\n')
                        _pos++;
                    return result;
                }
                i++;
            }
            if (i > _pos) {
                var result = _s.Substring(_pos, i - _pos);
                _pos = i;
                return result;
            }
            return null;
        }

        [ComVisible(false)]
        public override Task<string> ReadLineAsync()
        {
            return Task.FromResult(ReadLine());
        }

        [ComVisible(false)]
        public override Task<string> ReadToEndAsync()
        {
            return Task.FromResult(ReadToEnd());
        }

        [ComVisible(false)]
        public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
        {
            return Task.FromResult(ReadBlock(buffer, index, count));
        }

        [ComVisible(false)]
        public override Task<int> ReadAsync(char[] buffer, int index, int count)
        {
            return Task.FromResult(Read(buffer, index, count));
        }
    }
}
