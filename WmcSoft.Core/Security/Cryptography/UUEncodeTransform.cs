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
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace WmcSoft.Security.Cryptography
{
    /// <summary>Converts a <see cref="CryptoStream"></see> using Uuencode.</summary>
    /// <remarks>See http://en.wikipedia.org/wiki/Uuencode for more details.</remarks>
    [ComVisible(true)]
    public class UUEncodeTransform : ICryptoTransform, IDisposable
    {
        #region Lifecycle

        /// <summary>Initializes a new instance of the <see cref="ToBase64Transform"></see> class. </summary>
        public UUEncodeTransform() {
        }

        /// <summary>Releases the unmanaged resources used by the <see cref="ToBase64Transform"></see>.</summary>
        ~UUEncodeTransform() {
            Dispose(false);
        }

        /// <summary>Releases the unmanaged resources used by the <see cref="ToBase64Transform"></see> and optionally releases the managed resources.</summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing) {
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Behavioral Properties

        /// <summary>Gets a value indicating whether the current transform can be reused.</summary>
        /// <returns>Always true.</returns>
        public virtual bool CanReuseTransform {
            get { return true; }
        }

        /// <summary>Gets a value that indicates whether multiple blocks can be transformed.</summary>
        /// <returns>Always false.</returns>
        public bool CanTransformMultipleBlocks {
            get { return false; }
        }

        /// <summary>Gets the input block size.</summary>
        /// <returns>The size of the input data blocks in bytes.</returns>
        public int InputBlockSize {
            get { return 3; }
        }

        /// <summary>Gets the output block size.</summary>
        /// <returns>The size of the output data blocks in bytes.</returns>
        public int OutputBlockSize {
            get { return 4; }
        }

        #endregion

        #region Methods

        /// <summary>Encodes the specified region of the input byte array to base 64 and copies the isBusy to the specified region of the output byte array.</summary>
        /// <returns>The number of bytes written.</returns>
        /// <param name="outputBuffer">The output to which to write the isBusy. </param>
        /// <param name="inputBuffer">The input to uuencode. </param>
        /// <param name="inputOffset">The offset into the input byte array from which to begin using data. </param>
        /// <param name="outputOffset">The offset into the output byte array from which to begin writing data. </param>
        /// <param name="inputCount">The number of bytes in the input byte array to use as data. </param>
        /// <exception cref="CryptographicException">The data size is not valid. </exception>
        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {
            if (inputBuffer == null) throw new ArgumentNullException("inputBuffer");
            if (inputOffset < 0) throw new ArgumentOutOfRangeException("inputOffset");
            if (inputCount < 0 || inputCount > inputBuffer.Length) throw new ArgumentOutOfRangeException("inputCount");
            if ((inputBuffer.Length - inputCount) < inputOffset) throw new ArgumentOutOfRangeException("inputOffset");

            if (inputCount < InputBlockSize) {
                return 0;
            }

            // uuencode
            var array = new byte[] {
                (byte)(0x20 + (( inputBuffer[0] >> 2                                 ) & 0x3F)),
                (byte)(0x20 + (((inputBuffer[0] << 4) | ((inputBuffer[1] >> 4) & 0xF)) & 0x3F)),
                (byte)(0x20 + (((inputBuffer[1] << 2) | ((inputBuffer[2] >> 6) & 0x3)) & 0x3F)),
                (byte)(0x20 + (( inputBuffer[2]                                      ) & 0x3F))
            };

            Buffer.BlockCopy(array, 0, outputBuffer, outputOffset, array.Length);
            return array.Length;
        }

        /// <summary>Encodes the specified region of the specified byte array.</summary>
        /// <returns>The computed base 64 conversion.</returns>
        /// <param name="inputBuffer">The input to uuencode. </param>
        /// <param name="inputOffset">The offset into the byte array from which to begin using data. </param>
        /// <param name="inputCount">The number of bytes in the byte array to use as data. </param>
        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {
            if (inputBuffer == null) throw new ArgumentNullException("inputBuffer");
            if (inputOffset < 0) throw new ArgumentOutOfRangeException("inputOffset");
            if (inputCount < 0 || inputCount > inputBuffer.Length) throw new ArgumentOutOfRangeException("inputCount");
            if ((inputBuffer.Length - inputCount) < inputOffset) throw new ArgumentOutOfRangeException("inputOffset");
            if (inputCount > 3) throw new ArgumentOutOfRangeException("inputCount");

            // uuencode
            switch (inputCount) {
            case 1:
                return new byte[] {
                    (byte)(0x20 + (( inputBuffer[0] >> 2                                 ) & 0x3F)),
                    (byte)(0x20 + (( inputBuffer[0] << 4                                 ) & 0x3F))
                };
            case 2:
                return new byte[] {
                    (byte)(0x20 + (( inputBuffer[0] >> 2                                 ) & 0x3F)),
                    (byte)(0x20 + (((inputBuffer[0] << 4) | ((inputBuffer[1] >> 4) & 0xF)) & 0x3F)),
                    (byte)(0x20 + (( inputBuffer[1] << 2                                 ) & 0x3F))
                };
            case 3:
                return new byte[] {
                    (byte)(0x20 + (( inputBuffer[0] >> 2                                 ) & 0x3F)),
                    (byte)(0x20 + (((inputBuffer[0] << 4) | ((inputBuffer[1] >> 4) & 0xF)) & 0x3F)),
                    (byte)(0x20 + (((inputBuffer[1] << 2) | ((inputBuffer[2] >> 6) & 0x3)) & 0x3F)),
                    (byte)(0x20 + (( inputBuffer[2]                                      ) & 0x3F))
                };
            default:
                return new byte[0];
            }
        }

        /// <summary>Releases all resources used by the <see cref="ToBase64Transform"></see>.</summary>
        public void Clear() {
            Dispose();
        }

        #endregion

    }
}
