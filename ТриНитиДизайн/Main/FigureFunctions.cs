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
                        if(j == 0)
                        {
                            DrawRectangle(FigureList[i].Points[j], canvas);
                        }
                        if(j == FigureList[i].Points.Count - 2)
                        {
                            DrawRectangle(FigureList[i].Points[j+1], canvas);
                        }
                        if (j != ForbiddenLine)
                        {
                            
                            SetLine(FigureList[i].Points[j], FigureList[i].Points[j + 1], "red", canvas);
                        }
                    }
                }
            }
        }

        private void DrawRectangle(Point p, Canvas canvas)
        {
            Rectangle rec = new Rectangle();
            rec.Height = 8;
            rec.Width = 8;
            Canvas.SetLeft(rec, p.X - 4);
            Canvas.SetTop(rec, p.Y - 4);
            rec.Stroke = OptionColor.ColorSelection;
            rec.StrokeThickness = 1;
            canvas.Children.Add(rec);
        }

        public void DrawChoosingRectangle(Point p1, Point p2,Canvas canvas)
        {
            Rectangle rec = new Rectangle();
            rec.Height = Math.Abs(p2.Y - p1.Y);
            rec.Width = Math.Abs(p2.X - p1.X);
            DoubleCollection dashes = new DoubleCollection();
            dashes.Add(2);
            dashes.Add(2);
            rec.StrokeDashArray = dashes;
            if (p2.X > p1.X)
            {
                Canvas.SetLeft(rec, p1.X);
            }
            else
            {
                Canvas.SetLeft(rec, p2.X);
            }
            if (p2.Y > p1.Y)
            {
                Canvas.SetTop(rec, p1.Y);
            }
            else
            {
                Canvas.SetTop(rec, p2.Y);
            }
            rec.Stroke = OptionColor.ColorChoosingRec;
            rec.StrokeThickness = 1;
            canvas.Children.Add(rec);
        }
    }
}
