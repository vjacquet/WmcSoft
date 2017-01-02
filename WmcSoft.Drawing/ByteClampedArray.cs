using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Drawing
{
    public struct ByteClampedArray : IReadOnlyList<byte>
    {
        public byte this[int index] {
            get {
                throw new NotImplementedException();
            }
        }

        public int Count {
            get {
                throw new NotImplementedException();
            }
        }

        public IEnumerator<byte> GetEnumerator() {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
