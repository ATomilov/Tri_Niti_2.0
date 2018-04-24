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
            if  ((OptionSetka.Masshtab < 0.5 && step == 5) ||
                (OptionSetka.Masshtab < 2 && step == 2) ||
                (OptionSetka.Masshtab < 4 && step == 1) ||
                (OptionSetka.Masshtab < 8 && step == 0.5) ||
                (OptionSetka.Masshtab < 16 && step == 0.2) ||
                (OptionSetka.Masshtab < 32 && step == 0.1))
                return;
            //warning - without those numbers setka doesn't show properly
            //no idea why this works exactly
            double incX = 8;
            double incY = 20;
            if(step != 0)
            {
                double trueScale = 1 - (1 / OptionSetka.Masshtab);
                double startX = -panTransform.X + trueScale*(MainCanvas.ActualWidth/2) + incX;
                double startY = -panTransform.Y + trueScale * (MainCanvas.ActualHeight / 2) + incY;
                startX -= (startX % (step*2));
                startY -= (startY % (step * 2));

                for (double i = startX; i < startX + MainCanvas.ActualWidth / OptionSetka.Masshtab; i += (step * 2))
                    for (double j = startY; j < startY + MainCanvas.ActualHeight / OptionSetka.Masshtab; j += (step * 2))
                    {
                        Ellipse ell = SetDot(new Point(i, j));
                        GeometryHelper.RescaleEllipse(ell, OptionSetka.Masshtab);
                        SetkaFigure.Shapes.Add(ell);
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
                verticalLine = GeometryHelper.SetLine(OptionColor.ColorSelection, new Point(width / 2, -1000),
                    new Point(width / 2, height+1000),true, MainCanvas);
                Line horizontalLine = new Line();
                horizontalLine = GeometryHelper.SetLine(OptionColor.ColorSelection, new Point(-1000, height / 2), 
                    new Point(width + 1000, height / 2),true, MainCanvas);
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
            ell.Height = OptionSetka.DotSize;
            ell.Width = OptionSetka.DotSize;
            ell.Stretch = Stretch.Uniform;
            ell.Stroke = OptionColor.ColorSelection;
            ell.Fill = OptionColor.ColorSelection;
            Canvas.SetLeft(ell, centerPoint.X - ell.Height / 2);
            Canvas.SetTop(ell, centerPoint.Y - ell.Height / 2);
            return ell;
        }

        public Point FindClosestDot(Point point)
        {
            double step;
            if (OptionSetka.isDotOnGrid)
                step = (Double)OptionSetka.MasshtabSetka;
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
