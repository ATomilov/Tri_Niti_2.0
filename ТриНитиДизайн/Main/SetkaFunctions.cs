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

        private void SetSetka()        
        {
            ClearScreen();                     
        }

        public void ClearScreen()                         
        {
            if (MainCanvas != null)
            {
                MainCanvas.Children.Clear();
                SetGrid();
            }
        }

        public void SetGrid()          
        {
            double step = (Double)OptionSetka.MasshtabSetka;
            if (step != 0)
            {
                for (double i = -step * 3; i < MainCanvas.ActualWidth + 0; i += (step * 3))
                    for (double j = -step * 3; j < MainCanvas.ActualHeight + 0; j += (step * 3))
                    {
                        SetDot(new Point(i, j));
                    }
            }
        }


        public void SetDot(Point centerPoint)         
        {
            System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path();
            EllipseGeometry myEllipse = new EllipseGeometry();
            myEllipse.Center = centerPoint;
            myPath.Stroke = System.Windows.Media.Brushes.Black;
            myEllipse.RadiusX = 0.5;
            myEllipse.RadiusY = 0.5;
            myPath.Data = myEllipse;
            MainCanvas.Children.Add(myPath);
        }

        public Point FindClosestDot(Point point)
        {
            double step = (Double)OptionSetka.MasshtabSetka;
            if (OptionSetka.isDotOnGrid && OptionSetka.MasshtabSetka != 0)
            {
                Point pointOnGrid = new Point();
                if (point.X % (step * 3) > (step * 3 / 2))
                {
                    pointOnGrid.X = point.X - (point.X % (step * 3)) + step * 3;
                }
                else
                {
                    pointOnGrid.X = point.X - (point.X % (step * 3));
                }
                if (point.Y % (step * 3) > (step * 3 / 2))
                {
                    pointOnGrid.Y = point.Y - (point.Y % (step * 3)) + step * 3;
                }
                else
                {
                    pointOnGrid.Y = point.Y - (point.Y % (step * 3));
                }
                return pointOnGrid;
            }
            else
            {
                return point;
            }
        }


    }
}
