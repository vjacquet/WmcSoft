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

 ****************************************************************************
 * adapted from <https://raw.githubusercontent.com/component/rope/master/index.js>
 ****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Text
{
    /// <summary>
    /// Represents a data structure used to efficiently store and manipulate a very long string.
    /// </summary>
    public class Rope
    {
        const int SPLIT_LENGTH = 1000; // The threshold used to split a leaf node into two child nodes.
        const int JOIN_LENGTH = 500; // The threshold used to join two child nodes into one leaf node.
        const double REBALANCE_RATIO = 1.2d; // The threshold used to trigger a tree node rebuild when rebalancing the rope.

        private string _value;
        private int _length;
        private Rope _left;
        private Rope _right;

        public Rope(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            _value = value;
            _length = value.Length;
            Adjust();
        }

        public int Length { get { return _length; } }

        /// <summary>
        /// Adjusts the tree structure, so that very long nodes are split
        ///  and short ones are joined
        /// </summary>
        private void Adjust()
        {
            if (_value != null) {
                if (_length > Rope.SPLIT_LENGTH) {
                    int divide = _length / 2;
                    _left = new Rope(_value.Substring(0, divide));
                    _right = new Rope(_value.Substring(divide));
                    _value = null;
                }
            } else {
                if (_length < Rope.JOIN_LENGTH) {
                    UnguardedRebuild();
                }
            }
        }

        public override string ToString()
        {
            if (_value != null)
                return _value;
            return _left.ToString() + _right.ToString();
        }

        /// <summary>
        /// Removes text from the rope between the <paramref name="startIndex"/> and `end` positions.
        /// The character at `start` gets removed, but the character at `end` is 
        /// not removed. 
        /// /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public void Remove(int startIndex, int length)
        {
            int endIndex = startIndex + length;
            if (startIndex < 0 || startIndex > _length) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (endIndex < 0 || endIndex > _length) throw new ArgumentException(nameof(length));
            if (startIndex > endIndex) throw new ArgumentException(nameof(startIndex));

            if (_value != null) {
                _value = _value.Substring(0, startIndex) + _value.Substring(endIndex);
                _length = _value.Length;
            } else {
                var leftLength = _left._length;
                var leftStart = Math.Min(startIndex, leftLength);
                var leftEnd = Math.Min(endIndex, leftLength);
                var rightLength = _right.Length;
                var rightStart = Math.Max(0, Math.Min(startIndex - leftLength, rightLength));
                var rightEnd = Math.Max(0, Math.Min(endIndex - leftLength, rightLength));
                if (leftStart < leftLength) {
                    _left.Remove(leftStart, leftEnd - leftStart);
                }
                if (rightEnd > 0) {
                    _right.Remove(rightStart, rightEnd - rightStart);
                }
                _length = _left.Length + _right.Length;
            }

            Adjust();
        }

        /// <summary>
        /// Inserts text into the rope on the specified position.
        /// </summary>
        /// <param name="index">Where to insert the text.</param>
        /// <param name="value">Text to be inserted.</param>
        public void Insert(int index, string value)
        {
            if (index < 0 || index > _length) throw new ArgumentOutOfRangeException(nameof(index));

            if (_value != null) {
                _value = _value.Substring(0, index) + value.ToString() + _value.Substring(index);
                _length = _value.Length;
            } else {
                var leftLength = _left.Length;
                if (index < _left.Length) {
                    _left.Insert(index, value);
                    _length = _left.Length + _right.Length;
                } else {
                    _right.Insert(index - leftLength, value);
                }
            }
            Adjust();
        }

        public void Append(string value)
        {
            Insert(_length, value);
        }

        /// <summary>
        /// Rebuilds the entire rope structure, producing a balanced tree.
        /// </summary>
        public void Rebuild()
        {
            if (_value == null) {
                UnguardedRebuild();
            }
        }

        void UnguardedRebuild()
        {
            _value = _left.ToString() + _right.ToString();
            _left = null;
            _right = null;
        }

        /// <summary>
        /// Finds unbalanced nodes in the tree and rebuilds them.
        /// </summary>
        public void Rebalance()
        {
            if (_value == null) {
                if (_left.Length / _right.Length > Rope.REBALANCE_RATIO || _right.Length / _left.Length > Rope.REBALANCE_RATIO) {
                    UnguardedRebuild();
                } else {
                    _left.Rebalance();
                    _right.Rebalance();
                }
            }
        }

        /// <summary>
        /// Returns a string of <paramref name="length"/> characters from the rope, starting at the <paramref name="startIndex"/> position.
        /// </summary>
        /// <param name="startIndex">Initial position</param>
        /// <param name="length">Size of the string to return</param>
        /// <returns></returns>
        public string Substring(int startIndex, int length)
        {
            if (startIndex < 0) {
                startIndex = _length + startIndex;
                if (startIndex < 0) {
                    startIndex = 0;
                }
            }
            if (length < 0) {
                length = 0;
            }

            if (_value != null) {
                return _value.Substring(startIndex, length);
            }

            var endIndex = startIndex + length;
            var leftLength = _left.Length;
            var leftStart = Math.Min(startIndex, leftLength);
            var leftEnd = Math.Min(endIndex, leftLength);
            var rightLength = _right.Length;
            var rightStart = Math.Max(0, Math.Min(startIndex - leftLength, rightLength));
            var rightEnd = Math.Max(0, Math.Min(endIndex - leftLength, rightLength));

            if (leftStart != leftEnd) {
                if (rightStart != rightEnd) {
                    return _left.Substring(leftStart, leftEnd) + _right.Substring(rightStart, rightEnd);
                } else {
                    return _left.Substring(leftStart, leftEnd);
                }
            } else {
                if (rightStart != rightEnd) {
                    return _right.Substring(rightStart, rightEnd);
                } else {
                    return "";
                }
            }
        }

        public char this[int index] {
            get { return Substring(index, 1)[0]; }
        }
    }
}