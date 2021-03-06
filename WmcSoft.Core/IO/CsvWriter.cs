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
using System.Collections.Generic;
using System.IO;
using System.Text;

using static WmcSoft.IO.QuotedString;

namespace WmcSoft.IO
{
    /// <summary>
    /// Represents a writer that provides a fast, non-cached, forward-only way to generate streams or files that contain CSV data.
    /// </summary>
    public class CsvWriter : IDisposable
    {
        private readonly TextWriter writer;
        private readonly IReadOnlyCsvWriterSettings settings;
        private readonly char delimiter;
        private readonly char[] specialChars;

        protected CsvWriter(TextWriter writer, CsvWriterSettings settings)
        {
            this.writer = writer;
            this.settings = settings.Clone();
            delimiter = settings.Delimiter;

            var specialChars = new List<char> {
                delimiter
            };
            specialChars.AddRange(this.settings.NewLineChars.ToCharArray());
            this.specialChars = specialChars.ToArray();
        }

        #region Factory methods

        public static CsvWriter Create(string outputFileName, CsvWriterSettings settings = null)
        {
            if (outputFileName == null) throw new ArgumentNullException(nameof(outputFileName));

            settings = settings == null ? new CsvWriterSettings() : settings.Clone();
            return CreateWriter(outputFileName, settings);
        }

        private static CsvWriter CreateWriter(string outputFileName, CsvWriterSettings settings)
        {
            if (!settings.CloseOutput) {
                settings.CloseOutput = true;
            }

            FileStream fileStream = null;
            try {
                fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write, FileShare.Read, 4096);
                return CreateWriter(fileStream, settings);
            } catch {
                if (fileStream != null)
                    fileStream.Close();
                throw;
            }
        }

        public static CsvWriter Create(Stream output, CsvWriterSettings settings)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));

            settings = settings == null ? new CsvWriterSettings() : settings.Clone();
            return CreateWriter(output, settings);
        }

        private static CsvWriter CreateWriter(Stream output, CsvWriterSettings settings)
        {
            var writer = new StreamWriter(output, settings.Encoding);
            writer.NewLine = settings.NewLineChars;
            return CreateWriter(writer, settings);
        }

        public static CsvWriter Create(TextWriter output, CsvWriterSettings settings)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));

            settings = settings == null ? new CsvWriterSettings() : settings.Clone();
            settings.FormatProvider = output.FormatProvider;
            settings.NewLineChars = output.NewLine;
            return CreateWriter(output, settings);
        }

        public static CsvWriter Create(StringBuilder output, CsvWriterSettings settings)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));

            settings = settings == null ? new CsvWriterSettings() : settings.Clone();
            return CreateWriter(new StringWriter(output, settings.FormatProvider), settings);
        }

        private static CsvWriter CreateWriter(TextWriter output, CsvWriterSettings settings)
        {
            settings.Encoding = output.Encoding;
            return new CsvWriter(output, settings);
        }

        #endregion

        /// <summary>
        /// Gets the <see cref="IReadOnlyCsvWriterSettings"/> object used to create this <see cref="CsvWriter"/> instance.
        /// </summary>
        public IReadOnlyCsvWriterSettings Settings => settings;

        protected virtual void WriteUnescaped(string text)
        {
            if (string.IsNullOrEmpty(text)) {
            } else if (!UnguardedIsQuoted(text) && text.IndexOfAny(specialChars) >= 0) {
                WriteEscaped(UnguardedQuote(text));
            } else {
                WriteEscaped(text);
            }
        }

        protected virtual void WriteEscaped(string text)
        {
            writer.Write(text);
        }

        protected virtual void WriteEscaped(char[] buffer, int index, int count)
        {
            writer.Write(buffer, index, count);
        }

        /// <summary>
        /// Writes the name of the field in the header.
        /// </summary>
        /// <param name="name">The field name.</param>
        public virtual void WriteFieldName(string name)
        {
            WriteUnescaped(name);
        }

        /// <summary>
        /// Writes the header of the file.
        /// </summary>
        /// <param name="names">The field names.</param>
        public virtual void WriteHeader(params string[] names)
        {
            if (names == null || names.Length == 0)
                return;

            var length = names.Length;
            WriteFieldName(names[0]);
            for (int i = 1; i < length; i++) {
                WriteDelimiter();
                WriteFieldName(names[i]);
            }
            WriteEndOfLine();
        }

        /// <summary>
        /// Writes a record of fields.
        /// </summary>
        /// <param name="fields"></param>
        public virtual void WriteRecord(params object[] fields)
        {
            if (fields == null || fields.Length == 0)
                return;

            var length = fields.Length;
            WriteField(fields[0]);
            for (int i = 1; i < length; i++) {
                WriteDelimiter();
                WriteField(fields[i]);
            }
            WriteEndOfLine();
        }

        /// <summary>
        /// Writes a field value.
        /// </summary>
        /// <param name="value">The field value</param>
        /// <remarks>If the value is a <see cref="IQuotedString"/>, it is written as-is, otherwise it is escaped.</remarks>
        public virtual void WriteField(object value)
        {
            if (value == null)
                return;
            switch (value) {
            case IFormattable formattable:
                WriteUnescaped(formattable.ToString(null, settings.FormatProvider));
                break;
            case IQuotedString quotedString:
                WriteEscaped(quotedString.ToQuotedString());
                break;
            default:
                WriteUnescaped(value.ToString());
                break;
            }
        }

        /// <summary>
        /// Writes the field value.
        /// </summary>
        /// <param name="text"></param>
        public virtual void WriteField(string text)
        {
            WriteUnescaped(text);
        }

        /// <summary>
        /// Writes the field value.
        /// </summary>
        /// <param name="text">The already espaced value.</param>
        public virtual void WriteField(IQuotedString text)
        {
            WriteEscaped(text.ToQuotedString());
        }

        /// <summary>
        /// Writes the <see cref="IReadOnlyCsvWriterSettings.Delimiter"/>.
        /// </summary>
        public virtual void WriteDelimiter()
        {
            writer.Write(settings.Delimiter);
        }

        /// <summary>
        /// Writes the <see cref="IReadOnlyCsvWriterSettings.NewLineChars"/>.
        /// </summary>
        public virtual void WriteEndOfLine()
        {
            writer.Write(settings.NewLineChars);
        }

        /// <summary>
        /// Writes raw text from a string, i.e. without escaping it.
        /// </summary>
        /// <param name="text">String containing the text to write.</param>
        public virtual void WriteRaw(string text)
        {
            WriteEscaped(text);
        }

        /// <summary>
        /// Writes raw text from a character buffer, i.e. without escaping it.
        /// </summary>
        /// <param name="buffer">Character array containing the text to write.</param>
        /// <param name="index">The position within the buffer indicating the start of the text to write.</param>
        /// <param name="count">The number of characters to write.</param>
        public virtual void WriteRaw(char[] buffer, int index, int count)
        {
            WriteEscaped(buffer, index, count);
        }

        public void Close()
        {
            Dispose();
        }

        public void Flush()
        {
            writer.Flush();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (settings.CloseOutput)
                writer.Dispose();
        }
    }
}
