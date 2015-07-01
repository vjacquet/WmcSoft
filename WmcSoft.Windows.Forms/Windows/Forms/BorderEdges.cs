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
using System.ComponentModel;

namespace WmcSoft.Windows.Forms
{
    /// <summary>
    /// Represents the width of border edges.
    /// </summary>
    [TypeConverter(typeof(BorderEdgesConverter))]
    public class BorderEdges : ICloneable
    {
        private bool _sameOnAllSides;
        private int _top;
        private int _left;
        private int _right;
        private int _bottom;

        /// <summary>
        /// 
        /// </summary>
        public BorderEdges()
            : this(1) {
        }

        /// <summary>
        /// Creates a BorderEdges witht he same value on all sides.
        /// </summary>
        /// <param name="value">The width</param>
        public BorderEdges(int value) {
            _top = value;
            _sameOnAllSides = true;
        }

        /// <summary>
        /// Creates a BorderEdges
        /// </summary>
        /// <param name="left">The width on the left</param>
        /// <param name="top">The width on the top</param>
        /// <param name="right">The width on the right</param>
        /// <param name="bottom">The width on the bottom</param>
        public BorderEdges(int left, int top, int right, int bottom) {
            if (top == left && top == right && top == bottom) {
                _top = top;
                _sameOnAllSides = true;
            } else {
                _left = left;
                _top = top;
                _right = right;
                _bottom = bottom;
            }
        }

        #region Membres de ICloneable

        public object Clone() {
            BorderEdges edges = new BorderEdges();
            edges._sameOnAllSides = _sameOnAllSides;
            edges._top = _top;
            edges._right = _right;
            edges._bottom = _bottom;
            edges._left = _left;
            return edges;
        }

        #endregion

        #region Properties

        [RefreshProperties(RefreshProperties.All)]
        [Description("Number of pixels along all borders")]
        public int All {
            get {
                if (!_sameOnAllSides)
                    return 0;
                return _top;
            }
            set {
                if (!_sameOnAllSides || (_top != value)) {
                    _sameOnAllSides = true;
                    _top = value;
                    OnAllChanged(System.EventArgs.Empty);
                }
            }
        }

        public event EventHandler AllChanged;

        protected virtual void OnAllChanged(System.EventArgs e) {
            EventHandler handler = AllChanged;
            if (handler != null)
                handler(this, e);
        }

        private void ResetAll() {
            All = 0;
        }

        private bool SouldSerializeAll() {
            if (_sameOnAllSides)
                return (_top != 1);
            return false;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Description("Number of pixels along top border")]
        public int Top {
            get {
                return _top;
            }
            set {
                if (_sameOnAllSides || (_top != value)) {
                    _sameOnAllSides = false;
                    _top = value;
                    OnTopChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler TopChanged;

        protected virtual void OnTopChanged(System.EventArgs e) {
            EventHandler handler = TopChanged;
            if (handler != null)
                handler(this, e);
        }

        private void ResetTop() {
            Top = 1;
        }

        private bool SouldSerializeTop() {
            if (!_sameOnAllSides)
                return (_top != 1);
            return false;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Description("Number of pixels along left border")]
        public int Left {
            get {
                if (_sameOnAllSides)
                    return _top;
                return _left;
            }
            set {
                if (_sameOnAllSides || (_left != value)) {
                    _sameOnAllSides = false;
                    _left = value;
                    OnLeftChanged(System.EventArgs.Empty);
                }
            }
        }

        public event EventHandler LeftChanged;

        protected virtual void OnLeftChanged(System.EventArgs e) {
            EventHandler handler = LeftChanged;
            if (handler != null)
                handler(this, e);
        }

        private void ResetLeft() {
            Left = 1;
        }

        private bool SouldSerializeLeft() {
            if (!_sameOnAllSides)
                return (_left != 1);
            return false;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Description("Number of pixels along right border")]
        public int Right {
            get {
                if (_sameOnAllSides)
                    return _top;
                return _right;
            }
            set {
                if (_sameOnAllSides || (_right != value)) {
                    _sameOnAllSides = false;
                    _right = value;
                    OnRightChanged(System.EventArgs.Empty);
                }
            }
        }

        public event EventHandler RightChanged;

        protected virtual void OnRightChanged(System.EventArgs e) {
            EventHandler handler = RightChanged;
            if (handler != null)
                handler(this, e);
        }
        private void ResetRight() {
            Right = 1;
        }

        private bool SouldSerializeRight() {
            if (!_sameOnAllSides)
                return (_right != 1);
            return false;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Description("Number of pixels along bottom border")]
        public int Bottom {
            get {
                if (_sameOnAllSides)
                    return _top;
                return _bottom;
            }
            set {
                if (_sameOnAllSides || (_bottom != value)) {
                    _sameOnAllSides = false;
                    _bottom = value;
                    OnBottomChanged(System.EventArgs.Empty);
                }
            }
        }

        public event EventHandler BottomChanged;

        protected virtual void OnBottomChanged(System.EventArgs e) {
            EventHandler handler = BottomChanged;
            if (handler != null)
                handler(this, e);
        }

        private void ResetBottom() {
            Bottom = 1;
        }

        private bool SouldSerializeBottom() {
            if (!_sameOnAllSides)
                return (_bottom != 1);
            return false;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            BorderEdges edges = (obj as BorderEdges);
            if (edges == null)
                return false;
            if (edges._sameOnAllSides)
                return edges._sameOnAllSides && (edges._top == _top);
            return (edges._left == _left)
                && (edges._top == _top)
                && (edges._right == _right)
                && (edges._bottom == _bottom);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string ToString() {
            return String.Empty;
        }
    }
}
