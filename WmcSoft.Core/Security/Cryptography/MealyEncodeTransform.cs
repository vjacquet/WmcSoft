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
using System.Security.Cryptography;

namespace WmcSoft.Security.Cryptography
{
    public class MealyEncodeTransform : ICryptoTransform, IDisposable
    {
        private readonly byte[,] _matrix;
        private byte _state;

        #region Lifecycle

        /// <summary>Initializes a new instance of the <see cref="MealyEncodeTransform"></see> class. </summary>
        public MealyEncodeTransform(byte[,] matrix)
        {
            if (matrix == null) throw new ArgumentNullException(nameof(matrix));
            var dim = matrix.GetDimensions();
            if (dim[0] != 16 || dim[1] != 16) throw new ArgumentException("", nameof(matrix));

            _matrix = matrix;
        }

        /// <summary>Releases the unmanaged resources used by the <see cref="MealyEncodeTransform"></see>.</summary>
        ~MealyEncodeTransform()
        {
            Dispose(disposing: false);
        }

        /// <summary>Releases the unmanaged resources used by the <see cref="MealyEncodeTransform"></see> and optionally releases the managed resources.</summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Behavioral Properties

        public int InputBlockSize => 1;
        public int OutputBlockSize => 1;
        public bool CanTransformMultipleBlocks => false;
        public bool CanReuseTransform => true;

        #endregion

        /// <summary>Transforms the specified region of the input byte array and copies the resulting transform to the specified region of the output byte array.</summary>
        /// <returns>The number of bytes written.</returns>
        /// <param name="outputBuffer">The output to which to write the isBusy. </param>
        /// <param name="inputBuffer">The input to uuencode. </param>
        /// <param name="inputOffset">The offset into the input byte array from which to begin using data. </param>
        /// <param name="outputOffset">The offset into the output byte array from which to begin writing data. </param>
        /// <param name="inputCount">The number of bytes in the input byte array to use as data. </param>
        /// <exception cref="CryptographicException">The data size is not valid. </exception>
        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            var written = 0;
            while(inputCount >0) {
                outputBuffer[outputOffset++] = Encode(inputBuffer[inputOffset++]);
                inputCount--;
                written++;
            }
            return written;
        }

        /// <summary>Transforms the specified region of the specified byte array.</summary>
        /// <returns>The computed transform</returns>
        /// <param name="inputBuffer">The input to uuencode. </param>
        /// <param name="inputOffset">The offset into the byte array from which to begin using data. </param>
        /// <param name="inputCount">The number of bytes in the byte array to use as data. </param>
        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            var outputBuffer = new byte[inputCount];
            TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
            _state = 0;
            return outputBuffer;
        }

        byte Encode(byte codeUnit)
        {
            var lo = _matrix[_state, Lo(codeUnit)];
            var hi = _state = _matrix[lo, Hi(codeUnit)];
            return Make(lo, hi);
        }

        static byte Lo(byte b) => unchecked((byte)(b & 0x0f));
        static byte Hi(byte b) => unchecked((byte)(b >> 4));
        static byte Make(byte lo, byte hi) => unchecked((byte)(lo | hi << 4));
    }
}
