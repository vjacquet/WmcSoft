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

 ****************************************************************************
 * Based on <https://html.spec.whatwg.org/multipage/scripting.html#2dcontext>
 ****************************************************************************/

#endregion

using System.Collections;
using System.Collections.Generic;

namespace WmcSoft.Canvas
{
    public struct ByteClampedArray : IReadOnlyList<byte>
    {
        static readonly byte[] Empty = new byte[0];

        #region Adapters

        class Int16Adapter : IReadOnlyList<byte>
        {
            readonly IList<short> _buffer;

            public Int16Adapter(IList<short> buffer) {
                _buffer = buffer;
            }

            byte Clamp(short n) {
                if (n < 0) return 0;
                if (n > 255) return 255;
                return (byte)n;
            }

            public byte this[int index] {
                get { return Clamp(_buffer[index]); }
            }

            public int Count {
                get { return _buffer.Count; }
            }

            public IEnumerator<byte> GetEnumerator() {
                foreach (var n in _buffer) {
                    yield return Clamp(n);
                }
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        class UInt16Adapter : IReadOnlyList<byte>
        {
            readonly IList<ushort> _buffer;

            public UInt16Adapter(IList<ushort> buffer) {
                _buffer = buffer;
            }

            byte Clamp(ushort n) {
                if (n > 255) return 255;
                return (byte)n;
            }

            public byte this[int index] {
                get { return Clamp(_buffer[index]); }
            }

            public int Count {
                get { return _buffer.Count; }
            }

            public IEnumerator<byte> GetEnumerator() {
                foreach (var n in _buffer) {
                    yield return Clamp(n);
                }
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        class Int32Adapter : IReadOnlyList<byte>
        {
            readonly IList<int> _buffer;

            public Int32Adapter(IList<int> buffer) {
                _buffer = buffer;
            }

            byte Clamp(int n) {
                if (n < 0) return 0;
                if (n > 255) return 255;
                return (byte)n;
            }

            public byte this[int index] {
                get { return Clamp(_buffer[index]); }
            }

            public int Count {
                get { return _buffer.Count; }
            }

            public IEnumerator<byte> GetEnumerator() {
                foreach (var n in _buffer) {
                    yield return Clamp(n);
                }
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        class UInt32Adapter : IReadOnlyList<byte>
        {
            readonly IList<uint> _buffer;

            public UInt32Adapter(IList<uint> buffer) {
                _buffer = buffer;
            }

            byte Clamp(uint n) {
                if (n > 255) return 255;
                return (byte)n;
            }

            public byte this[int index] {
                get { return Clamp(_buffer[index]); }
            }

            public int Count {
                get { return _buffer.Count; }
            }

            public IEnumerator<byte> GetEnumerator() {
                foreach (var n in _buffer) {
                    yield return Clamp(n);
                }
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        class Int64Adapter : IReadOnlyList<byte>
        {
            readonly IList<long> _buffer;

            public Int64Adapter(IList<long> buffer) {
                _buffer = buffer;
            }

            byte Clamp(long n) {
                if (n < 0) return 0;
                if (n > 255) return 255;
                return (byte)n;
            }

            public byte this[int index] {
                get { return Clamp(_buffer[index]); }
            }

            public int Count {
                get { return _buffer.Count; }
            }

            public IEnumerator<byte> GetEnumerator() {
                foreach (var n in _buffer) {
                    yield return Clamp(n);
                }
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        class UInt64Adapter : IReadOnlyList<byte>
        {
            readonly IList<ulong> _buffer;

            public UInt64Adapter(IList<ulong> buffer) {
                _buffer = buffer;
            }

            byte Clamp(ulong n) {
                if (n > 255) return 255;
                return (byte)n;
            }

            public byte this[int index] {
                get { return Clamp(_buffer[index]); }
            }

            public int Count {
                get { return _buffer.Count; }
            }

            public IEnumerator<byte> GetEnumerator() {
                foreach (var n in _buffer) {
                    yield return Clamp(n);
                }
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        #endregion

        private readonly IReadOnlyList<byte> _buffer;

        public ByteClampedArray(IList<short> buffer) {
            _buffer = new Int16Adapter(buffer);
        }

        public ByteClampedArray(IList<ushort> buffer) {
            _buffer = new UInt16Adapter(buffer);
        }

        public ByteClampedArray(IList<int> buffer) {
            _buffer = new Int32Adapter(buffer);
        }

        public ByteClampedArray(IList<uint> buffer) {
            _buffer = new UInt32Adapter(buffer);
        }

        public ByteClampedArray(IList<long> buffer) {
            _buffer = new Int64Adapter(buffer);
        }

        public ByteClampedArray(IList<ulong> buffer) {
            _buffer = new UInt64Adapter(buffer);
        }

        public ByteClampedArray(params byte[] buffer) {
            _buffer = buffer;
        }

        IReadOnlyList<byte> Storage { get { return _buffer ?? Empty; } }

        public byte this[int index] {
            get {
                return Storage[index];
            }
        }

        public int Count {
            get { return Storage.Count; }
        }

        public IEnumerator<byte> GetEnumerator() {
            return Storage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
