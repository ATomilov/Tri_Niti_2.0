using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ТриНитиДизайн
{
    public static class OptionRegim
    {
        public static Regim regim = Regim.RegimNull;
    }

    public enum Regim
    {
        RegimDraw = 0,
        RegimTatami = 1,
        RegimStegki = 2,
        RegimKrivaya = 3,
        RegimCepochka = 4,
        RegimEditFigures = 5,
        RegimCursor= 6,
        RegimDuga = 7,
        RegimLomanaya = 8,
        RegimFigure = 9,
        RegimGlad = 10,
        ZoomIn = 11,
        ZoomOut = 12,
        ResizeFigure = 13,
        RotateFigure = 14,
        RegimRisui = 15,
        RegimMoveRect = 16,
        RegimDrawInColor = 17,
        RegimDrawStegki = 18,
        RegimCursorMoveRect = 19,
        RegimScaleFigure = 20,
        RegimChangeRotatingCenter = 21,
        RegimCursorJoinChain = 22,
        RegimCursorJoinTransposition = 23,
        RegimCursorJoinShiftDots = 24,
        RegimCursorJoinShiftElements = 25,
        RegimNull = 26

    }
}
