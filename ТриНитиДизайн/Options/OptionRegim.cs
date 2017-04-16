﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ТриНитиДизайн
{
    public static class OptionRegim
    {
        public static Regim regim = Regim.RegimDraw;
    }

    public enum Regim
    {
        RegimDraw = 0,
        RegimTatami = 1,
        RegimStegki = 2,
        RegimKrivaya = 3,
        RegimCepochka = 4,
        RegimEditFigures = 5,
        RegimSelectFigureToEdit = 6,
        RegimDuga = 7,
        RegimLomanaya = 8,
        RegimFigure = 9,
        RegimGlad = 10
    }
}
