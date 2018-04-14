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

namespace WmcSoft
{
    static class GeoCoordinate
    {
        public static int Encode(int degrees)
        {
            return degrees * 3600_000;
        }

        public static int Encode(int degrees, int minutes, int seconds, int milliseconds)
        {
            return degrees >= 0
                ? degrees * 3600_000 + minutes * 60_000 + seconds * 1000 + milliseconds
                : degrees * 3600_000 - minutes * 60_000 - seconds * 1000 - milliseconds;
        }

        public static int Decode(int x, out int degrees, out int minutes, out int seconds)
        {
            degrees = x / 3600_000;
            if (x < 0) {
                x = -x;
            }
            minutes = (x / 60_000) % 60;
            seconds = (x / 1000) % 60;
            return x;
        }

        public static int Decode(int x, out int degrees, out int minutes, out int seconds, out int milliseconds)
        {
            x = Decode(x, out degrees, out minutes, out seconds);
            milliseconds = x / 1000;
            return x;
        }

        public static int DecodeDegrees(int x)
        {
            return x / 3600_000;
        }

        public static int DecodeMinutes(int x)
        {
            return (Abs(x) / 60_000) % 60;
        }

        public static int DecodeSeconds(int x)
        {
            return Abs(x / 1000) % 60;
        }

        public static int DecodeMilliseconds(int x)
        {
            return Abs(x) % 1000;
        }

        static int Abs(int x)
        {
            return (x >= 0) ? x : -x;
        }
    }
}
