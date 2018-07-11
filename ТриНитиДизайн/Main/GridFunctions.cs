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
            if(gridFigure != null)
            {
                gridFigure.RemoveFigure(MainCanvas);
            }
            gridFigure = new Figure(MainCanvas);
            double step = (Double)OptionGrid.gridInterval;
            if  ((OptionGrid.scaleMultiplier < 0.5 && step == 5) ||
                (OptionGrid.scaleMultiplier < 2 && step == 2) ||
                (OptionGrid.scaleMultiplier < 4 && step == 1) ||
                (OptionGrid.scaleMultiplier < 8 && step == 0.5) ||
                (OptionGrid.scaleMultiplier < 16 && step == 0.2) ||
                (OptionGrid.scaleMultiplier < 32 && step == 0.1))
                return;
            //warning - without those numbers setka doesn't show properly
            //no idea why this works exactly
            double incX = 8;
            double incY = 20;
            if(step != 0)
            {
                double trueScale = 1 - (1 / OptionGrid.scaleMultiplier);
                double startX = -panTransform.X + trueScale*(MainCanvas.ActualWidth/2) + incX;
                double startY = -panTransform.Y + trueScale * (MainCanvas.ActualHeight / 2) + incY;
                startX -= (startX % (step*2));
                startY -= (startY % (step * 2));

                for (double i = startX; i < startX + MainCanvas.ActualWidth / OptionGrid.scaleMultiplier; i += (step * 2))
                    for (double j = startY; j < startY + MainCanvas.ActualHeight / OptionGrid.scaleMultiplier; j += (step * 2))
                    {
                        Ellipse ell = SetDot(new Point(i, j));
                        GeometryHelper.RescaleEllipse(ell, OptionGrid.scaleMultiplier);
                        gridFigure.Shapes.Add(ell);
                        MainCanvas.Children.Add(ell);
                    }
            }
        }

        public void DrawCenter(bool isCenterSet)
        {
            if(isCenterSet)
            {
                double height = MainCanvas.ActualHeight;
                double width = MainCanvas.ActualWidth;
                Line verticalLine = new Line();
                verticalLine = GeometryHelper.SetLine(OptionColor.colorInactive, new Point(width / 2, 0),
                    new Point(width / 2, height),true, MainCanvas);
                Line horizontalLine = new Line();
                horizontalLine = GeometryHelper.SetLine(OptionColor.colorInactive, new Point(0, height / 2), 
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

        public Ellipse SetDot(Point centerPoint)         
        {
            Ellipse ell = new Ellipse();
            ell.Height = OptionGrid.dotSize;
            ell.Width = OptionGrid.dotSize;
            ell.Stretch = Stretch.Uniform;
            ell.Stroke = OptionColor.colorInactive;
            ell.Fill = OptionColor.colorInactive;
            Canvas.SetLeft(ell, centerPoint.X - ell.Height / 2);
            Canvas.SetTop(ell, centerPoint.Y - ell.Height / 2);
            return ell;
        }

        public Point FindClosestDot(Point point)
        {
            double step;
            if (OptionGrid.isDotOnGrid)
                step = (Double)OptionGrid.gridInterval;
            else
                step = 0.1;
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
    }
}
