using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ТриНитиДизайн
{
    class PreviousView
    {
        public double ScaleValue;
        public double PanX;
        public double PanY;

        public PreviousView(double ScaleValue, double PanX, double PanY)
        {
            this.ScaleValue = ScaleValue;
            this.PanX = PanX;
            this.PanY = PanY;
        }
    }
}
