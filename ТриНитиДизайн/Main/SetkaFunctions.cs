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
        public void SetGrid()          
        {
            if(SetkaFigure != null)
            {
                SetkaFigure.RemoveFigure(MainCanvas);
            }
            SetkaFigure = new Figure(MainCanvas);
            double step = (Double)OptionSetka.MasshtabSetka;
            if (step != 0)
            {
                for (double i = -step * 2; i < MainCanvas.ActualWidth + 0; i += (step * 2))
                    for (double j = -step * 2; j < MainCanvas.ActualHeight + 0; j += (step * 2))
                    {
                        SetkaFigure.Shapes.Add(SetDot(new Point(i, j)));
                    }
            }
            SetkaFigure.AddFigure(MainCanvas);
        }

        public void SetCenter(bool isCenterSet)
        {
            if(isCenterSet)
            {
                double height = 900;
                double width = 1600;
                Line verticalLine = new Line();
                verticalLine = GeometryHelper.SetLine(OptionColor.ColorSelection, new Point(width / 2, 0),
                    new Point(width / 2, height),true, MainCanvas);
                Line horizontalLine = new Line();
                horizontalLine = GeometryHelper.SetLine(OptionColor.ColorSelection, new Point(0, height / 2), 
                    new Point(width, height / 2),true, MainCanvas);
                centerLines.Add(verticalLine);
                centerLines.Add(horizontalLine);
            }
            else
            {
                MainCanvas.Children.Remove(centerLines[0]);
                MainCanvas.Children.Remove(centerLines[1]);
                centerLines.Clear();
            }
        }

        public Path SetDot(Point centerPoint)         
        {
            System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path();
            EllipseGeometry myEllipse = new EllipseGeometry();
            myEllipse.Center = centerPoint;
            myPath.Stroke = System.Windows.Media.Brushes.Black;
            myEllipse.RadiusX = 0.5;
            myEllipse.RadiusY = 0.5;
            myPath.Data = myEllipse;
            return myPath;
        }

        public Point FindClosestDot(Point point)
        {
            double step = (Double)OptionSetka.MasshtabSetka;
            if (OptionSetka.isDotOnGrid && OptionSetka.MasshtabSetka != 0)
            {
                Point pointOnGrid = new Point();
                if (point.X % (step * 2) > step)
                {
                    pointOnGrid.X = point.X - (point.X % (step * 2)) + step * 2;
                }
                else
                {
                    pointOnGrid.X = point.X - (point.X % (step * 2));
                }
                if (point.Y % (step * 2) > step)
                {
                    pointOnGrid.Y = point.Y - (point.Y % (step * 2)) + step * 2;
                }
                else
                {
                    pointOnGrid.Y = point.Y - (point.Y % (step * 2));
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
