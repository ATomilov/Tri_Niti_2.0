﻿using System;
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
        public void RedrawEverything(List<Figure> FigureList,int ChosenFigure, bool AllRectanglesOff,bool rectanglesOn, Canvas canvas)
        {
            canvas.Children.Clear();
            SetkaFigure.AddFigure(canvas);
            double size = OptionDrawLine.SizeWidthAndHeightRectangle;
            for(int i = 0; i < FigureList.Count;i++)
            {
                FigureList[i].AddFigure(canvas);                        //можно не перерисовывать каждый раз
                if (!AllRectanglesOff)
                {
                    if (i == ChosenFigure)
                    {
                        if (rectanglesOn)
                        {
                            FigureList[i].DrawAllRectangles(size, OptionColor.ColorOpacity);
                        }
                        else
                        {
                            if (FigureList[i].Points.Count > 0)
                            {
                                DrawRectangle(FigureList[i].Points[0], OptionColor.ColorOpacity, canvas);
                                DrawRectangle(FigureList[i].Points[FigureList[i].Points.Count - 1], OptionColor.ColorOpacity, canvas);
                            }
                        }
                    }
                }
                
            }
            for (int i = 0; i < ListPltFigure.Count; i++)
            {
                ListPltFigure[i].AddFigure(canvas);
            }
        }

        private void DrawRectangle(Point p, Brush brush, Canvas canvas)
        {
            Rectangle rec = new Rectangle();
            rec.Height = OptionDrawLine.SizeWidthAndHeightRectangle;
            rec.Width = OptionDrawLine.SizeWidthAndHeightRectangle;
            Canvas.SetLeft(rec, p.X - OptionDrawLine.SizeWidthAndHeightRectangle/2);
            Canvas.SetTop(rec, p.Y - OptionDrawLine.SizeWidthAndHeightRectangle/2);
            rec.Fill = brush;
            rec.Stroke = OptionColor.ColorSelection;
            rec.StrokeThickness = OptionDrawLine.StrokeThickness;
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
            rec.StrokeThickness = OptionDrawLine.StrokeThickness;
            canvas.Children.Add(rec);
        }

        public void DrawAllChosenLines(Figure fig, Brush brush, Canvas canvas)
        {
            for (int i = 0; i < fig.PointsCount.Count - 1; i++)
            {
                if (fig.PointsCount[i] == (fig.PointsCount[i + 1] - 1))
                {
                    Shape sh;
                    fig.DictionaryPointLines.TryGetValue(fig.Points[fig.PointsCount[i]], out sh);
                    sh.Stroke = brush;
                }
            }
        }

        public void MakeLomanaya(Figure fig, Canvas canvas)
        {
            for (int i = 0; i < fig.PointsCount.Count - 1; i++)
            {
                if (fig.PointsCount[i] == (fig.PointsCount[i + 1] - 1))
                {
                    Shape sh;
                    fig.DictionaryPointLines.TryGetValue(fig.Points[fig.PointsCount[i]], out sh);
                    fig.DeleteShape(sh, fig.Points[fig.PointsCount[i]]);
                    fig.AddLine(fig.Points[fig.PointsCount[i]], fig.Points[fig.PointsCount[i + 1]], OptionColor.ColorDraw);
                }
            }
        }

        public void ChooseNextRectangle(Figure fig,bool isNext, Canvas canvas)
        {
            if(fig.PointsCount.Count == 1)
            {
                if(isNext)
                {
                    if(fig.PointsCount[0] != fig.Points.Count-1)
                        fig.PointsCount[0]++;
                }
                else
                {
                    if (fig.PointsCount[0] != 0)
                        fig.PointsCount[0]--;
                }
            }
        }

        public void MakeSpline(Figure fig, Canvas canvas)
        {
            for (int i = 0; i < fig.PointsCount.Count - 1; i++)
            {
                if (fig.PointsCount[i] == (fig.PointsCount[i + 1] - 1))
                {
                    Shape sh;
                    fig.DictionaryPointLines.TryGetValue(fig.Points[fig.PointsCount[i]], out sh);
                    fig.DeleteShape(sh, fig.Points[fig.PointsCount[i]]);
                    List<Point> newList = new List<Point>();
                    newList.Add(fig.Points[fig.PointsCount[i]]);

                    double x1 = (fig.Points[fig.PointsCount[i+1]].X + fig.Points[fig.PointsCount[i]].X)/2;
                    double y1 = (fig.Points[fig.PointsCount[i+1]].Y + fig.Points[fig.PointsCount[i]].Y)/2;
                    double x2 = fig.Points[fig.PointsCount[i+1]].X - fig.Points[fig.PointsCount[i]].X;
                    double y2 = fig.Points[fig.PointsCount[i+1]].Y - fig.Points[fig.PointsCount[i]].Y;
                    double distance = FindLength(fig.Points[fig.PointsCount[i + 1]], fig.Points[fig.PointsCount[i]]);
                    Vector vect = new Vector(x2, y2);
                    vect.Normalize();

                    newList.Add(new Point(x1 + vect.Y * distance / 10, y1 - vect.X * distance / 10));
                    newList.Add(fig.Points[fig.PointsCount[i+1]]);
                    SetSpline(0.8, newList, MainCanvas);
                    fig.AddShape((Shape)MainCanvas.Children[MainCanvas.Children.Count - 1], fig.Points[fig.PointsCount[i]]);

                }
            }
        }

        public void PrepareForTatami(Figure fig, Canvas canvas)
        {
            if (OptionRegim.regim != Regim.RegimCepochka)
            {
                fig.PreparedForTatami = true;
            }
            fig.ChangeFigureColor(OptionColor.ColorDraw,false);
            for(int i = 0; i< fig.Points.Count-1;i++)
            {
                Shape sh;
                fig.DictionaryPointLines.TryGetValue(fig.Points[i],out sh);
                if(sh is Path)
                {
                    Path path = (Path)sh;
                    PathGeometry myPathGeometry = (PathGeometry)path.Data;
                    Point p;
                    Point tg;
                    var points = new List<Point>();
                    double step = 50;
                    for (var j = 1; j < step; j++)
                    {
                        myPathGeometry.GetPointAtFractionLength(j / step, out p, out tg);
                        fig.Points.Insert(j + i, p);
                    }
                }
            }
        }

        public void ChooseFigureByClicking(List<Figure> FigureList, Object clickedShape, Canvas canvas)
        {
            if (clickedShape is Line || clickedShape is Path)
            {
                double x = 0;
                double y = 0;
                if (clickedShape is Line)
                {
                    Line clickedLine = (Line)clickedShape;
                    x = clickedLine.X1;
                    y = clickedLine.Y1;
                }
                else
                {
                    Path path = (Path)clickedShape;
                    for(int i = 0; i < FigureList.Count;i++)
                    {
                       var invLine = FigureList[i].DictionaryInvLines.FirstOrDefault(z => z.Value == path);
                       var point = FigureList[i].DictionaryPointLines.FirstOrDefault(z => z.Value == invLine.Key);      //TODO: улучшить
                       if(point.Value != null)
                       {
                           x = point.Key.X;
                           y = point.Key.Y;
                           break;
                       }
                    }
                }
                for (int i = 0; i < FigureList.Count; i++)
                {// here flag-click
                    if (OptionRegim.regim != Regim.RegimEditFigures)
                    {
                        if (FigureList[i].DictionaryPointLines.ContainsKey(new Point(x, y)) == true)
                        {
                            if (IndexFigure == i && OptionRegim.regim != Regim.RegimFigure)
                            {
                                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection,false);
                                IndexFigure = FigureList.Count - 1;
                            }
                            else
                            {
                                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection,false);
                                IndexFigure = i;
                                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
                            }
                            RedrawEverything(FigureList, IndexFigure, false, false, canvas);
                            break;
                        }
                    }
                    else
                    {
                        if (FigureList[i].DictionaryPointLines.ContainsKey(new Point(x, y)) == true)
                        {
                            if (IndexFigure != i)
                            {
                                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection, false);
                                IndexFigure = i;
                                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, true);
                                RedrawEverything(FigureList, IndexFigure, false, true, canvas);
                            }
                            break;
                        }
                    }
                }
            }
            else if (OptionRegim.regim != Regim.RegimEditFigures && OptionRegim.regim != Regim.RegimFigure && OptionRegim.regim != Regim.RegimSelectFigureToEdit)
            {
                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection,false);
                FigureList.Add(new Figure(MainCanvas));
                IndexFigure = FigureList.Count - 1;
                RedrawEverything(FigureList, IndexFigure, false, false, canvas);
            }
            //else if (OptionRegim.regim == Regim.RegimSelectFigureToEdit) //переход в режим ресайз (пока работает в этом режиме поворот, так как пока не понятно как переходить в режим поворот)
            //{

            //    if (isResizeRegim)
            //    {
            //        OptionRegim.regim = Regim.ResizeFigure;
            //    }
            //    //isResizeRegim = false;
            //}
            //else if(OptionRegim.regim == Regim.ResizeFigure)
            //{
            //    if (isRotateRegim)
            //    {
            //        OptionRegim.regim = Regim.RotateFigure;
            //    }
            //}
        }

        public void ChangeFiguresColor(List<Figure> FigureList, Canvas canvas)
        {
            if(SecondGladFigure != -1)
            {
                FigureList[SecondGladFigure].ChangeFigureColor(OptionColor.ColorSelection, false);
            }
            if(OptionRegim.regim == Regim.RegimGlad)
            {
                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw,false);
                FigureList[SecondGladFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
            }
            else if (OptionRegim.regim == Regim.RegimEditFigures)
            {
                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, true);
            }
            else
            {
                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
            }
        }
    }
}
