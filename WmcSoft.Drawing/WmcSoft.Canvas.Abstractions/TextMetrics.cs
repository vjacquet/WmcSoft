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

namespace WmcSoft.Canvas
{
    public class TextMetrics
    {
        public TextMetrics(float Width
            , float actualBoundingBoxLeft
            , float actualBoundingBoxRight
            , float fontBoundingBoxAscent
            , float fontBoundingBoxDescent
            , float actualBoundingBoxAscent
            , float actualBoundingBoxDescent
            , float emHeightAscent
            , float emHeightDescent
            , float hangingBaseline
            , float alphabeticBaseline
            , float ideographicBaseline) {
            Width = actualBoundingBoxLeft;
            ActualBoundingBoxLeft = actualBoundingBoxRight;
            ActualBoundingBoxRight = fontBoundingBoxAscent;
            FontBoundingBoxAscent = fontBoundingBoxDescent;
            FontBoundingBoxDescent = fontBoundingBoxDescent;
            ActualBoundingBoxAscent = actualBoundingBoxAscent;
            ActualBoundingBoxDescent = actualBoundingBoxDescent;
            EmHeightAscent = emHeightAscent;
            EmHeightDescent = emHeightDescent;
            HangingBaseline = hangingBaseline;
            AlphabeticBaseline = alphabeticBaseline;
            IdeographicBaseline = ideographicBaseline;
        }

        // x-direction
        float Width { get; } // advance width
        float ActualBoundingBoxLeft { get; }
        float ActualBoundingBoxRight { get; }

        // y-direction
        float FontBoundingBoxAscent { get; }
        float FontBoundingBoxDescent { get; }
        float ActualBoundingBoxAscent { get; }
        float ActualBoundingBoxDescent { get; }
        float EmHeightAscent { get; }
        float EmHeightDescent { get; }
        float HangingBaseline { get; }
        float AlphabeticBaseline { get; }
        float IdeographicBaseline { get; }
    }
}
