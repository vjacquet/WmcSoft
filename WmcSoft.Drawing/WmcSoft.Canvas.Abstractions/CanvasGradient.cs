#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Canvas
{
    public abstract class CanvasGradient<TColor>
    {
        private readonly List<float> _offsets;
        private readonly List<TColor> _colors;

        protected CanvasGradient() {
            _offsets = new List<float>();
            _colors = new List<TColor>();
        }

        public virtual void AddColorStop(float offset, TColor color) {
            if (offset < 0f || offset > 1f) throw new ArgumentOutOfRangeException(nameof(offset));

            var index = _offsets.BinarySearch(offset);
            if (index < 0) {
                index = ~index;
            } else {
                index++;
            }
            _offsets.Insert(index, offset);
            _colors.Insert(index, color);
        }

        protected IReadOnlyList<float> Offsets { get { return _offsets; } }
        protected IReadOnlyList<TColor> Colors { get { return _colors; } }
    }

    public abstract class LinearGradient<T, TColor> : CanvasGradient<TColor>
    {
        private readonly T _x0;
        private readonly T _y0;
        private readonly T _x1;
        private readonly T _y1;

        protected LinearGradient(T x0, T y0, T x1, T y1) {
            _x0 = x0;
            _y0 = y0;
            _x1 = x1;
            _y1 = y1;
        }

        protected T X0 { get { return _x0; } }
        protected T Y0 { get { return _y0; } }
        protected T X1 { get { return _x1; } }
        protected T Y1 { get { return _y1; } }
    }

    public abstract class RadialGradient<T, TColor> : CanvasGradient<TColor>
    {
        private readonly T _x0;
        private readonly T _y0;
        private readonly T _r0;
        private readonly T _x1;
        private readonly T _y1;
        private readonly T _r1;

        protected RadialGradient(T x0, T y0, T r0, T x1, T y1, T r1) {
            _x0 = x0;
            _y0 = y0;
            _r0 = r0;
            _x1 = x1;
            _y1 = y1;
            _r1 = r1;
        }

        protected T X0 { get { return _x0; } }
        protected T Y0 { get { return _y0; } }
        protected T R0 { get { return _r0; } }
        protected T X1 { get { return _x1; } }
        protected T R1 { get { return _r1; } }
    }
}
