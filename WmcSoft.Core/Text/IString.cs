using System;
using System.Text;

namespace WmcSoft.Text
{
    internal interface IString
    {
        int Length { get; }
        int IndexOfAny(char[] anyOf, int startIndex, int count);
        StringBuilder AppendTo(StringBuilder sb, int startIndex, int count);
        bool StartsWith(string value, StringComparison comparison);
        bool EndsWith(string value, StringComparison comparison);
    }

    internal static class IStringExtensions
    {
        internal static string Remove<S>(this S self, params char[] args)
            where S : IString {
            var sb = new StringBuilder();
            int pos = 0;
            int count = self.Length;
            while (true) {
                var found = self.IndexOfAny(args, pos, count);
                if (found < 0) {
                    self.AppendTo(sb, pos, count);
                    break;
                } else if (found == pos) {
                    pos++;
                    count--;
                    continue;
                } else {
                    self.AppendTo(sb, pos, found - pos);
                    count -= found - pos + 1;
                    pos = found + 1;
                }
            }
            return sb.ToString();
        }
    }

    struct StringAdapter : IString
    {
        private readonly string _storage;

        public StringAdapter(string value) {
            _storage = value;
        }

        public int Length { get { return _storage.Length; } }

        public StringBuilder AppendTo(StringBuilder sb, int startIndex, int count) {
            return sb.Append(_storage, startIndex, count);
        }

        public bool EndsWith(string value, StringComparison comparison) {
            return value.EndsWith(value, comparison);
        }

        public int IndexOfAny(char[] anyOf, int startIndex, int count) {
            return _storage.IndexOfAny(anyOf, startIndex, count);
        }

        public bool StartsWith(string value, StringComparison comparison) {
            return value.StartsWith(value, comparison);
        }
    }

    struct StripAdapter : IString
    {
        private readonly Strip _storage;

        public StripAdapter(Strip value) {
            _storage = value;
        }

        public int Length { get { return _storage.Length; } }

        public StringBuilder AppendTo(StringBuilder sb, int startIndex, int count) {
            return _storage.AppendTo(sb, startIndex, count);
        }

        public bool EndsWith(string value, StringComparison comparison) {
            return value.EndsWith(value, comparison);
        }

        public int IndexOfAny(char[] anyOf, int startIndex, int count) {
            return _storage.IndexOfAny(anyOf, startIndex, count);
        }

        public bool StartsWith(string value, StringComparison comparison) {
            return value.StartsWith(value, comparison);
        }
    }
}
