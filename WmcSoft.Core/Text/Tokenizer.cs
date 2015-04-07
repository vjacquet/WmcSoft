using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Text
{
    public class Tokenizer
    {
        readonly Predicate<char> _isSeparator;

        public Tokenizer(params char[] separators) {
            if (separators == null || separators.Length == 0) {
                _isSeparator = c => c == ' ';
            } else if (separators.Length == 1) {
                var separator = separators[0];
                _isSeparator = c => c == separator;
            } else {
                Array.Sort(separators);
                _isSeparator = c => c.BinaryAnyOf(separators);
            }
        }
        public Tokenizer(Predicate<char> isSeparator) {
            _isSeparator = isSeparator;
        }

        public IEnumerable<string> Tokenize(string value) {
            var start = 0;
            var length = 0;
            foreach (var c in value) {
                if (_isSeparator(c)) {
                    if (length > 0)
                        yield return value.Substring(start, length);
                    start += length + 1;
                    length = 0;
                } else {
                    length++;
                }
            }
            if (length > 0)
                yield return value.Substring(start, length);
        }
    }
}
