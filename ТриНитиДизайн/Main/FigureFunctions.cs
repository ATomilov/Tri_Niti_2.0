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

        void Ctezhki(List<Shape> shaps, Point pointstart, Point pointend, int indexstart, int indexend,Canvas canvas)
        {

            Line line = (Line)shaps[0];
            //if (pointstart == null && pointend == null)
            //{
            pointstart = new Point(line.X2, line.Y2);
            pointend = new Point(line.X1, line.Y1);
            indexstart = 1;
            indexend = 0;
            //}
            //else
            //{
            //    this.pointstart = pointstart;
            //    this.pointend = pointend;
            //    this.indexstart = indexstart;
            //    this.indexend = indexend;
            //}
            Line lineEnd = (Line)shaps[shaps.Count - 1];
            Line microline_before = CreateLine(lineEnd.X2, lineEnd.Y2, pointstart.X, pointstart.Y, Brushes.Black, canvas);



            for (int i = 1; indexstart < shaps.Count - 1; i++)
            {

                if (i % 2 != 0)
                {
                    //move end
                    indexend++;
                    pointend = new Point(((Line)(shaps[shaps.Count - 1 - indexend])).X2,
                        ((Line)(shaps[shaps.Count - 1 - indexend])).Y2);

                }
                else
                {
                    //move start
                    indexstart++;
                    pointstart = new Point(((Line)(shaps[indexend])).X2, ((Line)(shaps[indexend])).Y2);

                }
                Line microline = CreateLine(pointend.X, pointend.Y, pointstart.X, pointstart.Y, Brushes.Black, canvas);

            }
        }

        public Line CreateLine(double x1, double y1, double x2, double y2, SolidColorBrush black, Canvas canvas)
        {
            Line line = new Line()
            {
                Stroke = black,
                StrokeThickness = 1,
                SnapsToDevicePixels = true,
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2
            };
            line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            canvas.Children.Add(line);
            return line;
        }

        public void RedrawEverything(List<Figure> FigureList,int ChosenFigure, int ForbiddenLine, Canvas canvas)
        {
            canvas.Children.Clear();
            for(int i = 0; i < FigureList.Count;i++)
            {
                for(int j = 0; j < FigureList[i].Points.Count - 1;j++)
                {
                    if(i != ChosenFigure)
                    {
                        SetLine(FigureList[i].Points[j], FigureList[i].Points[j + 1], "normal", canvas);
                    }
                    else
                    {
                        if (j != ForbiddenLine)
                        {
                            SetLine(FigureList[i].Points[j], FigureList[i].Points[j + 1], "red", canvas);
                        }
                    }
                }
            }
        }



    }
}
