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
using System.Security.Cryptography;

namespace WmcSoft
{
    /// <summary>
    /// Represents a pseudo-random number generator, a device that produces a sequence of numbers 
    /// that meet certain statistical requirements for randomness.
    /// </summary>
    public class RNGRandom : Random
    {
        readonly RNGCryptoServiceProvider _rng;
        byte[] _buffer;
        int _startIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="RNGRandom"/> class.
        /// </summary>
        public RNGRandom()
        {
            _rng = new RNGCryptoServiceProvider();
            _buffer = new byte[1024];
            NextBytes(_buffer);
            _startIndex = 0;
        }

        private void EnsureCapacity(int size)
        {
            Debug.Assert(size < _buffer.Length);
            if ((_buffer.Length - _startIndex) < size) {
                NextBytes(_buffer);
                _startIndex = 0;
            }
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. 
        /// maxValue must be greater than or equal to zero. </param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to zero, and less than <paramref name="maxValue"/>; that is, 
        /// the range of return values ordinarily includes zero but not <paramref name="maxValue"/>. 
        /// However, if <paramref name="maxValue"/> equals zero, <paramref name="maxValue"/> is returned. 
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than zero. </exception>
        public override int Next(int maxValue)
        {
            if (maxValue < Byte.MaxValue) {
                EnsureCapacity(1);
                return (int)_buffer[_startIndex++] % maxValue;
            } else if (maxValue < UInt16.MaxValue) {
                EnsureCapacity(sizeof(UInt16));
                int value = BitConverter.ToUInt16(_buffer, _startIndex);
                _startIndex += sizeof(UInt16);
                return value % maxValue;
            }
            EnsureCapacity(sizeof(int));
            return Next() % maxValue;
        }

        /// <summary>
        /// Returns a nonnegative random number.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to zero and less than <see cref="F:System.Int32.MaxValue"/>.
        /// </returns>
        public override int Next()
        {
            EnsureCapacity(sizeof(int));
            int value = BitConverter.ToInt32(_buffer, _startIndex);
            _startIndex += sizeof(int);
            return value > 0 ? value : -value;
        }

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <param name="buffer">An array of bytes to contain random numbers. </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null. </exception>
        public override void NextBytes(byte[] buffer)
        {
            _rng.GetBytes(buffer);
        }

        protected override double Sample()
        {
            return ((double)Next()) / Int32.MaxValue;
        }
    }
}
