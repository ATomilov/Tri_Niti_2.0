using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ТриНитиДизайн
{
    public static class OptionMode
    {
        public static Mode mode = Mode.modeNull;
    }

    public enum Mode
    {
        modeDraw = 0,
        modeTatami = 1,
        modeStitches = 2,
        modeDrawCurve = 3,
        modeRunStitch = 4,
        modeEditFigures = 5,
        modeCursor= 6,
        modeDrawArc = 7,
        modeFigure = 8,
        modeSatin = 9,
        zoomIn = 10,
        zoomOut = 11,
        resizeFigure = 12,
        rotateFigure = 13,
        modeRisui = 14,
        modeMovePoints = 15,
        modeDrawInColor = 16,
        modeDrawStitchesInColor = 17,
        modeUnembroid = 18,
        modeCursorMoveRect = 19,
        modeScaleFigure = 20,
        modeChangeRotatingCenter = 21,
        modeCursorJoinShiftElements = 22,
        moveCanvas = 23,
        oneToOne = 24,
        modeNull = 25
    }
}
