﻿#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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
using System.Globalization;
using System.Text;

namespace WmcSoft.IO
{
    /// <summary>
    /// Specifies a set of features to support on the <see cref="CsvWriter"/> object created by the <see cref="CsvWriter.Create"/> method.
    /// </summary>
    public sealed class CsvWriterSettings : IReadOnlyCsvWriterSettings, ICloneable
    {
        #region Readonly

        class ReadOnlyCsvWriterSettings : IReadOnlyCsvWriterSettings
        {
            private readonly CsvWriterSettings underlying;

            public ReadOnlyCsvWriterSettings(CsvWriterSettings underlying)
            {
                this.underlying = underlying;
            }

            public bool CloseOutput => underlying.CloseOutput;
            public Encoding Encoding => underlying.Encoding;
            public char Delimiter => underlying.Delimiter;
            public string NewLineChars => underlying.NewLineChars;
            public IFormatProvider FormatProvider => underlying.FormatProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A read-only facade on these settings.</returns>
        public IReadOnlyCsvWriterSettings AsReadOnly()
        {
            return new ReadOnlyCsvWriterSettings(this);
        }

        #endregion

        public CsvWriterSettings()
        {
            Reset();
        }

        /// <summary>
        /// Resets the members of the settings class to their default values.
        /// </summary>
        public void Reset()
        {
            Encoding = Encoding.UTF8;
            NewLineChars = Environment.NewLine;
            Delimiter = ',';
            CloseOutput = false;
            FormatProvider = CultureInfo.CurrentCulture;
        }

        public bool CloseOutput { get; set; }
        public Encoding Encoding { get; set; }
        public char Delimiter { get; set; }
        public string NewLineChars { get; set; }
        public IFormatProvider FormatProvider { get; set; }

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }
    }
}
