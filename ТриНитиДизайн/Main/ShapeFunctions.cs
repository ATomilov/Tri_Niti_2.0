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
        public void SetSpline(double tension,List<Point> TPoint,Canvas canvas)
        {
            Path myPath = new Path();
            myPath.Stroke = OptionColor.ColorKrivaya;
            myPath.StrokeThickness = 1;
            PathGeometry myPathGeometry = new PathGeometry();
            CanonicalSplineHelper spline = new CanonicalSplineHelper();
            myPathGeometry = spline.CreateSpline(TPoint, tension, null, false, false, 0.25);
            myPath.Data = myPathGeometry;
            canvas.Children.Add(myPath);
        }
    }
}