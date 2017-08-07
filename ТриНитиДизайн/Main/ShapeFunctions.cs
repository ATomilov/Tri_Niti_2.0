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
        public Shape SetSpline(double minHeight, double tension,List<Point> TPoint,bool isCurve, Brush brush, Canvas canvas)
        {
            Path myPath = new Path();
            myPath.MinHeight = minHeight;
            myPath.Stroke = brush;
            myPath.StrokeThickness = OptionDrawLine.StrokeThickness;
            PathGeometry myPathGeometry = new PathGeometry();
            CanonicalSplineHelper spline = new CanonicalSplineHelper();
            myPathGeometry = spline.CreateSpline(TPoint, tension, null,isCurve, false, false, 0.25);
            myPath.Data = myPathGeometry;
            canvas.Children.Add(myPath);
            return myPath;
        }

        public Shape SetArc(Brush brush, Point firstDot,Point secondDot, Point thirdDot, Canvas canvas)
        {
            Path myPath = new Path();
            myPath.Stroke = brush;
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
            double xc = -0.5 * (y0 * (x1 * x1 + y1 * y1 - x2 * x2 - y2 * y2) + y1 * (x2 * x2 + y2 * y2 - x0 * x0 - y0 * y0) +
                y2 * (x0 * x0 + y0 * y0 - x1 * x1 - y1 * y1)) / (x0 * (y1 - y2) + x1 * (y2 - y0) + x2 * (y0 - y1));
            double yc = 0.5 * (x0 * (x1 * x1 + y1 * y1 - x2 * x2 - y2 * y2) + x1 * (x2 * x2 + y2 * y2 - x0 * x0 - y0 * y0) +
                x2 * (x0 * x0 + y0 * y0 - x1 * x1 - y1 * y1)) / (x0 * (y1 - y2) + x1 * (y2 - y0) + x2 * (y0 - y1));
            Vector vect1 = new Vector(x1 - xc, y1 - yc);
            Vector vect2 = new Vector(x2 - xc, y2 - yc);
            double atan = Math.Atan2(vect1.X * vect2.Y - vect1.Y * vect2.X, vect1.X * vect2.X + vect1.Y * vect2.Y);
            double radius = FindLength(firstDot, new Point(xc, yc));
            double height = - ((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1) / dist;
            if (height < 0)
            {
                arc.SweepDirection = SweepDirection.Clockwise;
                atan = -atan;
            }
            if (atan > 0)
                arc.IsLargeArc = true;
            arc.Size = new Size(radius, radius);
            pathFigure.Segments.Add(arc);
            pathGeometry.Figures.Add(pathFigure);
            myPath.Data = pathGeometry;
            canvas.Children.Add(myPath);
            return myPath;
        }

        public Line SetLine(Point p1, Point p2, Canvas canvas)
        {
            Line line = new Line();
            line.X1 = p1.X;
            line.Y1 = p1.Y;
            line.X2 = p2.X;
            line.Y2 = p2.Y;
            line.StrokeThickness = OptionDrawLine.StrokeThickness;
            line.Stroke = OptionColor.ColorDraw;
            canvas.Children.Add(line);
            return line;
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
                    fig.DeleteShape(sh, fig.Points[fig.PointsCount[i]],canvas);
                    fig.AddLine(fig.Points[fig.PointsCount[i]], fig.Points[fig.PointsCount[i + 1]], OptionColor.ColorDraw);
                }
            }
        }

        public Rectangle DrawChoosingRectangle(Point p1, Point p2, Canvas canvas)
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
            return rec;
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
                        Point contP;
                        fig.DictionaryShapeControlPoints.TryGetValue(p, out contP);
                        newFig.AddShape(sh, p, contP);
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
                        Point contP;
                        fig.DictionaryShapeControlPoints.TryGetValue(p, out contP);
                        newFig.AddShape(sh, p, contP);
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

        public void MakeSpline(Figure fig,Brush brush, Canvas canvas)
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
                    fig.DeleteShape(sh, fig.Points[fig.PointsCount[i]],MainCanvas);
                    sh = SetSpline(10, 0.7, newList, false, brush, MainCanvas);
                    fig.AddShape(sh, fig.Points[fig.PointsCount[i]],new Point(-500,-500));
                }
            }
        }

        public void ReverseFigure(Figure fig, Canvas canvas)
        {
            Figure newFig = new Figure(canvas);
            List<Point> ptsForCurves = new List<Point>();
            Dictionary<Point, Shape> tempDicShape = new Dictionary<Point, Shape>();
            Dictionary<Point, Point> tempDicContPoints = new Dictionary<Point, Point>();
            for (int i = fig.Points.Count - 1; i >= 0; i--)
            {
                Point p = fig.Points[i];
                if (i != 0)
                {
                    Shape sh;
                    fig.DictionaryPointLines.TryGetValue(fig.Points[i - 1], out sh);
                    Point contP;
                    fig.DictionaryShapeControlPoints.TryGetValue(fig.Points[i - 1], out contP);
                    if(sh is Path)
                    {
                        if (sh.MinHeight == 10)
                        {
                            if (!newFig.PointsCount.Contains(fig.Points.Count - i))
                                newFig.PointsCount.Add(fig.Points.Count - i);
                            if (!newFig.PointsCount.Contains(fig.Points.Count - i - 1))
                                newFig.PointsCount.Add(fig.Points.Count - i - 1);
                        }
                        else
                        {
                            ptsForCurves.Add(p);
                            ptsForCurves.Add(fig.Points[i - 1]);
                            tempDicShape.Add(p, sh);
                            tempDicContPoints.Add(p, contP);
                        }
                    }
                    newFig.AddShape(sh, p, contP);
                }
                newFig.Points.Add(p);
            }
            newFig.PointStart = fig.PointEnd;
            newFig.PointEnd = fig.PointStart;
            newFig.PointsCount.Sort();
            if (newFig.PointsCount.Count > 0)
                MakeSpline(newFig, OptionColor.ColorDraw, canvas);

            for (int i = 0; i < ptsForCurves.Count; i+=2 )
            {
                Shape sh;
                newFig.DictionaryPointLines.TryGetValue(ptsForCurves[i], out sh);
                newFig.DeleteShape(sh, ptsForCurves[i], canvas);
                tempDicShape.TryGetValue(ptsForCurves[i], out sh);
                Point contP;
                tempDicContPoints.TryGetValue(ptsForCurves[i], out contP);
                if (sh.MinHeight == 5)
                {
                    List<Point> pts = new List<Point>();
                    pts.Add(ptsForCurves[i]);
                    pts.Add(contP);
                    pts.Add(ptsForCurves[i + 1]);
                    sh = SetSpline(5, 0.75, pts, true, OptionColor.ColorDraw, canvas);
                }
                else
                    sh = SetArc(OptionColor.ColorDraw, ptsForCurves[i], ptsForCurves[i + 1], contP, canvas);
                newFig.AddShape(sh, ptsForCurves[i], contP);
            }
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
                        Point contP;
                        fig.DictionaryShapeControlPoints.TryGetValue(p, out contP);
                        newFig.AddShape(sh, p, contP);
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
                        Point contP;
                        fig.DictionaryShapeControlPoints.TryGetValue(p, out contP);
                        newFig.AddShape(sh, p, contP);
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
                                Point contP;
                                fig.DictionaryShapeControlPoints.TryGetValue(fig.Points[i], out contP);
                                newFig.AddShape(prevShape, fig.Points[i], contP);
                            }
                        }
                        else
                        {
                            Point p = fig.Points[i];
                            if (i != fig.Points.Count - 1 && !nextDotDeleted)
                            {
                                fig.DictionaryPointLines.TryGetValue(fig.Points[i], out prevShape);
                                Point contP;
                                fig.DictionaryShapeControlPoints.TryGetValue(fig.Points[i], out contP);
                                newFig.AddShape(prevShape, fig.Points[i], contP);
                            }
                            newFig.PointEnd = p;
                            newFig.Points.Add(p);
                        }
                    }
                }
                if(newFig.Points.Count > 0)
                    newFig.PointStart = newFig.Points[0];
                ListFigure.Insert(IndexFigure, newFig);
                ListFigure.Remove(fig);
            }
        }

    }
}