using System;
using System.Text;

namespace WmcSoft.IO
{
    /// <summary>
    /// Represents the settings for the <see cref="CsvWriter"/>.
    /// </summary>
    public interface IReadOnlyCsvWriterSettings
    {
        bool CloseOutput { get; }
        char Delimiter { get; }
        Encoding Encoding { get; }
        IFormatProvider FormatProvider { get; }
        string NewLineChars { get; }
    }
}
