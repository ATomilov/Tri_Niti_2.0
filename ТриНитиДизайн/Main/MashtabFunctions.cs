using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using Path = System.Windows.Shapes.Path;

namespace ТриНитиДизайн
{
    public partial class MainWindow : Window
    {
        public void Plus(Canvas canvas)
        {
            OptionSetka.Masshtab *= 2;
            OptionDrawLine.StrokeThickness /= 2;
            OptionDrawLine.SizeWidthAndHeightRectangle /= 2;
            OptionDrawLine.InvisibleStrokeThickness /= 2;
            foreach (Shape sh in ListFigure[IndexFigure].Shapes)
            {
                sh.StrokeThickness /= 2;
            }
            foreach (Shape sh2 in ListFigure[SecondGladFigure].Shapes)
            {
                sh2.StrokeThickness /= 2;
            }
            ScaleTransform scaleTransform = new ScaleTransform(OptionSetka.Masshtab, OptionSetka.Masshtab);
            canvas.LayoutTransform = scaleTransform;
        }

        public void Minus(Canvas canvas)
        {
            OptionSetka.Masshtab /= 2;
            OptionDrawLine.StrokeThickness *= 2;
            OptionDrawLine.SizeWidthAndHeightRectangle *= 2;
            OptionDrawLine.InvisibleStrokeThickness *= 2;
            foreach (Shape sh in ListFigure[IndexFigure].Shapes)
            {
                sh.StrokeThickness *= 2;
            }
            foreach (Shape sh2 in ListFigure[SecondGladFigure].Shapes)
            {
                sh2.StrokeThickness *= 2;
            }
            ScaleTransform scaleTransform = new ScaleTransform(OptionSetka.Masshtab, OptionSetka.Masshtab);
            canvas.LayoutTransform = scaleTransform;
        }



    }
}
