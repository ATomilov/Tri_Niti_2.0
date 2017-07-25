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
        public Shape SetSpline(double tension,List<Point> TPoint,bool isCurve, Canvas canvas)
        {
            Path myPath = new Path();
            myPath.MinHeight = 5;
            myPath.Stroke = OptionColor.ColorKrivaya;
            myPath.StrokeThickness = OptionDrawLine.StrokeThickness;
            PathGeometry myPathGeometry = new PathGeometry();
            CanonicalSplineHelper spline = new CanonicalSplineHelper();
            myPathGeometry = spline.CreateSpline(TPoint, tension, null,isCurve, false, false, 0.25);
            myPath.Data = myPathGeometry;
            canvas.Children.Add(myPath);
            return myPath;
        }

        public Shape SetArc(Point firstDot,Point secondDot, Point thirdDot, Shape prevSh, Canvas canvas)
        {
            Path myPath = new Path();
            myPath.Stroke = OptionColor.ColorChoosingRec;
            myPath.StrokeThickness = OptionDrawLine.StrokeThickness;
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = firstDot;
            ArcSegment arc = new ArcSegment();
            arc.Point = secondDot;
            double dist = FindLength(firstDot, secondDot);
            double x0 = thirdDot.X, y0 = thirdDot.Y;
            double x1 = firstDot.X, y1 = firstDot.Y;
            double x2 = secondDot.X, y2 = secondDot.Y;
            double height = - ((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1) / dist;
            if (height < 0)
            {
                arc.SweepDirection = SweepDirection.Clockwise;
                height = -height;
            }
            double radius = height / 2 + (dist * dist) / (8 * height);
            if (height > radius)
                arc.IsLargeArc = true;
            if (radius < 0)
                return prevSh;
            arc.Size = new Size(radius, radius);
            pathFigure.Segments.Add(arc);
            pathGeometry.Figures.Add(pathFigure);
            myPath.Data = pathGeometry;
            canvas.Children.Add(myPath);
            return myPath;
        }

        public void PrepareForTatami(Figure fig, bool isColorChanged)
        {
            if (OptionRegim.regim != Regim.RegimCepochka)
            {
                fig.PreparedForTatami = true;
            }
            if (isColorChanged)
                fig.ChangeFigureColor(OptionColor.ColorDraw, false);
            for (int i = 0; i < fig.Points.Count - 1; i++)
            {
                Shape sh;
                fig.DictionaryPointLines.TryGetValue(fig.Points[i], out sh);
                if (sh is Path)
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

        public void DrawChoosingRectangle(Point p1, Point p2, Canvas canvas)
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

        public void ChooseNextRectangle(Figure fig, bool isNext, Canvas canvas)
        {
            if (fig.PointsCount.Count == 1)
            {
                if (isNext)
                {
                    if (fig.PointsCount[0] != fig.Points.Count - 1)
                        fig.PointsCount[0]++;
                }
                else
                {
                    if (fig.PointsCount[0] != 0)
                        fig.PointsCount[0]--;
                }
            }
        }

        public void SplitFigureInTwo(Figure fig, Canvas canvas)
        {
            if (fig.PointsCount.Count == 1 && fig.PointsCount[0] != 0)
            {
                Figure newFig = new Figure(canvas);
                for (int i = 0; i <= fig.PointsCount[0]; i++)
                {
                    Point p = fig.Points[i];
                    if (i != fig.PointsCount[0])
                    {
                        Shape sh;
                        fig.DictionaryPointLines.TryGetValue(fig.Points[i], out sh);
                        newFig.AddShape(sh, p);
                    }
                    if (i == 0)
                        newFig.PointStart = p;
                    if (i == fig.PointsCount[0])
                        newFig.PointEnd = p;
                    newFig.Points.Add(p);
                }
                ListFigure.Insert(IndexFigure, newFig);
                newFig = new Figure(canvas);
                for (int i = fig.PointsCount[0]; i < fig.Points.Count; i++)
                {
                    Point p = fig.Points[i];
                    if (i != fig.Points.Count - 1)
                    {
                        Shape sh;
                        fig.DictionaryPointLines.TryGetValue(fig.Points[i], out sh);
                        newFig.AddShape(sh, p);
                    }
                    if (i == fig.PointsCount[0])
                        newFig.PointStart = p;
                    if (i == fig.Points.Count - 1)
                        newFig.PointEnd = p;
                    newFig.Points.Add(p);
                }
                newFig.ChangeFigureColor(OptionColor.ColorSelection, false);
                ListFigure.Insert(IndexFigure + 1, newFig);
                ListFigure.Remove(fig);
            }
        }

        public void MakeSpline(Figure fig, Canvas canvas)
        {
            for (int i = 0; i < fig.PointsCount.Count - 1; i++)
            {
                if (fig.PointsCount[i] == (fig.PointsCount[i + 1] - 1))
                {
                    List<Point> newList = new List<Point>();
                    if(fig.Points.Count == 2)
                    {
                        newList.Add(fig.Points[fig.PointsCount[i]]);
                        newList.Add(fig.Points[fig.PointsCount[i]]);
                        newList.Add(fig.Points[fig.PointsCount[i] + 1]);
                        newList.Add(fig.Points[fig.PointsCount[i] + 1]);
                    }
                    else if (fig.PointsCount[i] == 0)
                    {
                        newList.Add(fig.Points[fig.PointsCount[i]]);
                        newList.Add(fig.Points[fig.PointsCount[i]]);
                        newList.Add(fig.Points[fig.PointsCount[i] + 1]);
                        newList.Add(fig.Points[fig.PointsCount[i] + 2]);
                    }
                    else if (fig.PointsCount[i] == fig.Points.Count - 2)
                    {
                        newList.Add(fig.Points[fig.PointsCount[i] - 1]);
                        newList.Add(fig.Points[fig.PointsCount[i]]);
                        newList.Add(fig.Points[fig.PointsCount[i] + 1]);
                        newList.Add(fig.Points[fig.PointsCount[i] + 1]);
                    }
                    else
                    {
                        newList.Add(fig.Points[fig.PointsCount[i] - 1]);
                        newList.Add(fig.Points[fig.PointsCount[i]]);
                        newList.Add(fig.Points[fig.PointsCount[i] + 1]);
                        newList.Add(fig.Points[fig.PointsCount[i] + 2]);
                    }
                    Shape sh;
                    fig.DictionaryPointLines.TryGetValue(fig.Points[fig.PointsCount[i]], out sh);
                    fig.DeleteShape(sh, fig.Points[fig.PointsCount[i]]);
                    SetSpline(0.7, newList,false, MainCanvas);
                    fig.AddShape((Shape)MainCanvas.Children[MainCanvas.Children.Count - 1], fig.Points[fig.PointsCount[i]]);
                }
            }
        }

        public void ReverseFigure(Figure fig, Canvas canvas)
        {
            Figure newFig = new Figure(canvas);
            for (int i = fig.Points.Count - 1; i >= 0; i--)
            {
                Point p = fig.Points[i];
                if (i != 0)
                {
                    Shape sh;
                    fig.DictionaryPointLines.TryGetValue(fig.Points[i - 1], out sh);
                    newFig.AddShape(sh, p);
                }
                newFig.Points.Add(p);
            }
            newFig.PointStart = fig.PointEnd;
            newFig.PointEnd = fig.PointStart;
            ListFigure.Insert(IndexFigure, newFig);
            ListFigure.Remove(fig);
        }

        public void AddPointToFigure(Figure fig, Canvas canvas)
        {
            if (canvas.Children.Contains(fig.NewPointEllipse))
            {
                Figure newFig = new Figure(canvas);
                int index = fig.Points.IndexOf(fig.PointForAddingPoints);
                for (int i = 0; i <= index; i++)
                {
                    Point p = fig.Points[i];
                    if (i != index)
                    {
                        Shape sh;
                        fig.DictionaryPointLines.TryGetValue(fig.Points[i], out sh);
                        newFig.AddShape(sh, p);
                    }
                    if (i == 0)
                        newFig.PointStart = p;
                    if (i == index)
                        newFig.PointEnd = p;
                    newFig.Points.Add(p);
                }
                newFig.AddPoint(fig.EllipsePoint, OptionColor.ColorDraw, true, OptionDrawLine.SizeWidthAndHeightRectangle);
                newFig.AddPoint(fig.Points[index + 1], OptionColor.ColorDraw, true, OptionDrawLine.SizeWidthAndHeightRectangle);
                for (int i = index + 1; i < fig.Points.Count; i++)
                {
                    Point p = fig.Points[i];
                    if (i != fig.Points.Count - 1)
                    {
                        Shape sh;
                        fig.DictionaryPointLines.TryGetValue(fig.Points[i], out sh);
                        newFig.AddShape(sh, p);
                    }
                    if (i == fig.Points.Count - 1)
                        newFig.PointEnd = p;
                    if (i != index + 1)
                        newFig.Points.Add(p);
                }
                ListFigure.Insert(IndexFigure, newFig);
                ListFigure.Remove(fig);
            }
        }
        
        public void DeletePointFromFigure(Figure fig, Canvas canvas)
        {
            if (fig.PointsCount.Count > 0)
            {
                Figure newFig = new Figure(canvas);
                Shape prevShape = new Path();
                for (int i = 0; i < fig.Points.Count; i++)
                {
                    bool foundDeletePoint = false;
                    bool nextDotDeleted = false;
                    for (int j = 0; j < fig.PointsCount.Count; j++)
                    {
                        if (i == fig.PointsCount[j])
                        {
                            i++;
                            foundDeletePoint = true;
                        }
                        if (i + 1 == fig.PointsCount[j])
                        {
                            nextDotDeleted = true;
                        }
                    }
                    if (i != fig.Points.Count)
                    {
                        if (foundDeletePoint)
                        {
                            newFig.AddPoint(fig.Points[i], OptionColor.ColorDraw, true, OptionDrawLine.SizeWidthAndHeightRectangle);
                            if (i != fig.Points.Count - 1 && !nextDotDeleted)
                            {
                                fig.DictionaryPointLines.TryGetValue(fig.Points[i], out prevShape);
                                newFig.AddShape(prevShape, fig.Points[i]);
                            }
                        }
                        else
                        {
                            Point p = fig.Points[i];
                            if (i != fig.Points.Count - 1 && !nextDotDeleted)
                            {
                                fig.DictionaryPointLines.TryGetValue(fig.Points[i], out prevShape);
                                newFig.AddShape(prevShape, p);
                            }
                            newFig.PointEnd = p;
                            newFig.Points.Add(p);
                        }
                    }
                }
                ListFigure.Insert(IndexFigure, newFig);
                ListFigure.Remove(fig);
            }
        }

        public void LoadPreviousRegim(bool isRisui)
        {
            OptionRegim.regim = ListFigure[IndexFigure].regimFigure;
            ChangeFiguresColor(ListFigure, MainCanvas);
            if (OptionRegim.regim == Regim.RegimTatami)
            {
                if (!isRisui)
                {
                    ListFigure[IndexFigure].SaveCurrentShapes();
                    PrepareForTatami(ListFigure[IndexFigure], true);
                }
                ControlLine.AddFigure(MainCanvas);
            }
            if (OptionRegim.regim == Regim.RegimGlad)
            {
                if (!isRisui)
                {
                    ListFigure[IndexFigure].SaveCurrentShapes();
                    ListFigure[SecondGladFigure].SaveCurrentShapes();
                    PrepareForTatami(ListFigure[IndexFigure], true);
                    PrepareForTatami(ListFigure[SecondGladFigure], true);
                }
                foreach (Figure sh in LinesForGlad)
                    sh.AddFigure(MainCanvas);
            }
        }
    }
}