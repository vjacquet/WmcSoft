#region Licence

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
    public class CsvWriter : IDisposable
    {
        private readonly TextWriter _writer;
        private readonly IReadOnlyCsvWriterSettings _settings;
        private readonly char _delimiter;
        private readonly char[] _specialChars;

        protected CsvWriter(TextWriter writer, CsvWriterSettings settings)
        {
            _writer = writer;
            _settings = settings.AsReadOnly();
            _delimiter = settings.Delimiter;

            var specialChars = new List<char> {
                _delimiter
            };
            specialChars.AddRange(_settings.NewLineChars.ToCharArray());
            _specialChars = specialChars.ToArray();
        }

        #region Factory methods

        public static CsvWriter Create(string outputFileName, CsvWriterSettings settings)
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

        public IReadOnlyCsvWriterSettings Settings => _settings;

        protected virtual void WriteUnescaped(string text)
        {
            if (string.IsNullOrEmpty(text)) {
            } else if (!UnguardedIsQuoted(text) && text.IndexOfAny(_specialChars) >= 0) {
                WriteEscaped(UnguardedQuote(text));
            } else {
                WriteEscaped(text);
            }
        }

        protected virtual void WriteEscaped(string text)
        {
            _writer.Write(text);
        }

        protected virtual void WriteEscaped(char[] buffer, int index, int count)
        {
            _writer.Write(buffer, index, count);
        }

        public virtual void WriteName(string name)
        {
            WriteUnescaped(name);
        }

        public virtual void WriteHeader(params string[] names)
        {
            if (names == null || names.Length == 0)
                return;

            var length = names.Length;
            WriteName(names[0]);
            for (int i = 1; i < length; i++) {
                WriteDelimiter();
                WriteName(names[i]);
            }
            WriteEndOfLine();
        }

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

        public virtual void WriteField(object value)
        {
            if (value == null)
                return;
            switch (value) {
            case IFormattable formattable:
                WriteUnescaped(formattable.ToString(null, _settings.FormatProvider));
                break;
            case IQuotedString quotedString:
                WriteEscaped(quotedString.ToQuotedString());
                break;
            default:
                WriteUnescaped(value.ToString());
                break;
            }
        }

        public virtual void WriteField(string text)
        {
            WriteUnescaped(text);
        }

        public virtual void WriteDelimiter()
        {
            _writer.Write(_settings.Delimiter);
        }

        public virtual void WriteEndOfLine()
        {
            _writer.Write(_settings.NewLineChars);
        }

        public virtual void WriteRaw(string text)
        {
            WriteEscaped(text);
        }

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
            _writer.Flush();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_settings.CloseOutput)
                _writer.Dispose();
        }
    }
}
