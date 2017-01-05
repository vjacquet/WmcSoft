using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmcSoft.Canvas
{
    /// <summary>
    /// Text
    /// </summary>
    public interface ICanvasTextDrawingStyles
    {
        string Font { get; set; } // (default 10px sans-serif)
        CanvasTextAlign TextAlign { get; set; } // (default: "start")
        CanvasTextBaseline TextBaseline { get; set; } // (default: "alphabetic")
        CanvasDirection Direction { get; set; } // (default: "inherit")
    }
}