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

 -----------------------------------------------------------------------------

 * This file is part of the Microsoft .NET Framework SDK Code Samples.
 
    Copyright (C) Microsoft Corporation.  All rights reserved.
 
    This source code is intended only as a supplement to Microsoft
    Development Tools and/or on-line documentation.  See these other
    materials for detailed information regarding Microsoft code samples.
 
    THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
    KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
    IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
    PARTICULAR PURPOSE.

 ****************************************************************************/

#endregion

using System;
using System.Text;

namespace WmcSoft.Text
{
    /// <summary>
    /// Best fit fallback defining custom encoding
    /// </summary>
    public class EncoderBestFitFallback : EncoderFallback
    {
        #region EncoderBestFitFallbackBuffer class

        sealed class EncoderBestFitFallbackBuffer : EncoderFallbackBuffer
        {
            private readonly Encoding _parentEncoding;
            private readonly string _unknown;

            private string _fallback = String.Empty;
            private int _charIndex = 1;

            public EncoderBestFitFallbackBuffer(EncoderBestFitFallback fallback, string unknown) {
                _parentEncoding = fallback.ParentEncoding;
                _unknown = unknown;
            }

            public override bool Fallback(char charUnknown, int index) {
                // Since both fallback methods require normalizing a string, make a string out of our char
                var s = new String(charUnknown, 1);
                return Fallback(s);
            }

            public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index) {
                // Since both fallback methods require normalizing a string, make a string out of our chars
                var s = new String(new char[] { charUnknownHigh, charUnknownLow });
                return Fallback(s);
            }

            private bool Fallback(String unknown) {
                if (_charIndex <= _fallback.Length) {
                    _charIndex = 1;
                    _fallback = String.Empty;
                    throw new ArgumentException("Unexpected recursive fallback", "chars");
                }

                var s = "";
                try {
                    s = unknown.Normalize(NormalizationForm.FormKD);
                    if (s == unknown)
                        s = "";
                }
                catch (ArgumentException) {
                    // Allow the string to become a ? fallback
                }

                _fallback = _parentEncoding.GetString(_parentEncoding.GetBytes(s));

                if ((_fallback.Length == 0) || (_fallback[0] != s[0])) {
                    _fallback = _unknown;
                }

                _charIndex = 0;
                return true;
            }

            public override char GetNextChar() {
                if (_charIndex >= _fallback.Length) {
                    if (_charIndex == _fallback.Length)
                        _charIndex++;
                    return '\0';
                }
                return _fallback[_charIndex++];
            }

            public override bool MovePrevious() {
                if (_charIndex <= _fallback.Length)
                    _charIndex--;
                return (_charIndex >= 0 || _charIndex < _fallback.Length);
            }

            public override int Remaining {
                get {
                    if (_charIndex < _fallback.Length)
                        return _fallback.Length - _charIndex;
                    return 0;
                }
            }

            public override void Reset() {
                _fallback = "";
                _charIndex = 1;
            }
        }

        #endregion

        private readonly string _unknown;
        private readonly Encoding _parentEncoding;

        public EncoderBestFitFallback(Encoding targetEncoding, string unknown = "?") {
            _parentEncoding = (Encoding)targetEncoding.Clone();
            _parentEncoding.EncoderFallback = new EncoderReplacementFallback("");
            _parentEncoding.DecoderFallback = new DecoderReplacementFallback("");
            _unknown = unknown;
        }

        public Encoding ParentEncoding {
            get { return _parentEncoding; }
        }

        public override int MaxCharCount {
            get { return 18; }
        }

        public override EncoderFallbackBuffer CreateFallbackBuffer() {
            return new EncoderBestFitFallbackBuffer(this, _unknown);
        }
    }


}
