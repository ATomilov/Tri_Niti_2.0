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
    public class Figure
    {
        public Mode modeFigure;
        //public list<shape> shapes;
        //public list<shape> invshapes;
        public List<Point> points;
        //public List<Shape> dotsForFigure;
        //public List<Shape> tempInvShapes;
        //public List<Shape> tempShapes;
        public List<Point> tempPoints;
        public List<int> highlightedPoints;
        //public List<Rectangle> RectangleOfFigures;
        public List<Figure> groupFigures;
        //public Dictionary<Point, Shape> tempDictionaryPointLines;
        //public Dictionary<Shape, Shape> tempDictionaryInvLines;
        //public Dictionary<Point, Shape> DictionaryPointLines;
        public Dictionary<Point, Tuple<Point,Point>> shapeControlPoints;
        //public Dictionary<Shape, Shape> DictionaryInvLines;
        public Point pointStart;
        public Point pointEnd;
        public Point ellipsePoint;
        public Point pointForAddingPoints;
        public Canvas canvas;
        //public Ellipse newPointEllipse;
        public Vector tatamiControlLine;
        public List<Vector> satinControlLines;
        public Point oldTatamiCenter;
        //TODO: rethink list for center points
        public List<Point> oldSatinCenters;
        public bool preparedForTatami;

        public Figure(Canvas _canvas)
        {
            modeFigure = Mode.modeFigure;
            //Shapes = new List<Shape>();
            //InvShapes = new List<Shape>();
            points = new List<Point>();
            //dotsForFigure = new List<Shape>();
            //tempShapes = new List<Shape>();
            //tempInvShapes = new List<Shape>();
            tempPoints = new List<Point>();
            highlightedPoints = new List<int>();
            //RectangleOfFigures = new List<Rectangle>();
            groupFigures = new List<Figure>();
            groupFigures.Add(this);
            tatamiControlLine = new Vector();
            satinControlLines = new List<Vector>();
            oldTatamiCenter = new Point();
            oldSatinCenters = new List<Point>();
            preparedForTatami = false;
            canvas = _canvas;
            shapeControlPoints = new Dictionary<Point, Tuple<Point,Point>>();
            //DictionaryPointLines = new Dictionary<Point, Shape>();
            //DictionaryInvLines = new Dictionary<Shape, Shape>();
            //tempDictionaryPointLines = new Dictionary<Point, Shape>();
            //tempDictionaryInvLines = new Dictionary<Shape, Shape>();
        }

        //public void AddShape(Shape shape,Point p, Tuple<Point,Point> contPts)
        //{
        //    Shapes.Add(shape);
        //    DictionaryPointLines.Add(p, shape);
        //    shapeControlPoints.Add(p, contPts);
        //    if (shape is Path)
        //    {
        //        Path pth = (Path)shape;
        //        Path newPath = new Path();
        //        newPath.Data = pth.Data;
        //        newPath.Stroke = OptionColor.colorBackground;
        //        newPath.StrokeThickness = OptionDrawLine.invisibleStrokeThickness;
        //        newPath.Opacity = 0;
        //        InvShapes.Add(newPath);
        //        DictionaryInvLines.Add(shape, newPath);
        //    }
        //    else
        //    {
        //        Line ln = (Line)shape;
        //        Line newLine = new Line();
        //        newLine.Stroke = OptionColor.colorBackground;
        //        newLine.StrokeThickness = OptionDrawLine.invisibleStrokeThickness;
        //        newLine.Opacity = 0;
        //        newLine.X1 = ln.X1;
        //        newLine.Y1 = ln.Y1;
        //        newLine.X2 = ln.X2;
        //        newLine.Y2 = ln.Y2;
        //        InvShapes.Add(newLine);
        //        DictionaryInvLines.Add(shape, newLine);
        //    }
        //}

        //public void DeleteShape(Shape shape, Point p, Canvas curCanvas)
        //{
        //    Shapes.Remove(shape);
        //    DictionaryPointLines.Remove(p);
        //    shapeControlPoints.Remove(p);
        //    Shape sh;
        //    DictionaryInvLines.TryGetValue(shape, out sh);
        //    InvShapes.Remove(sh);
        //    DictionaryInvLines.Remove(shape);
        //    curCanvas.Children.Remove(sh);
        //}

        //public void AddLine(Point point1, Point point2, Brush brush)
        //{
        //    Line shape = new Line();
        //    shape.Stroke = brush;
        //    shape.StrokeThickness = OptionDrawLine.strokeThickness;
        //    shape.X1 = point1.X;
        //    shape.Y1 = point1.Y;
        //    shape.X2 = point2.X;
        //    shape.Y2 = point2.Y;
        //    Shapes.Add(shape);
        //    DictionaryPointLines.Add(point1, shape);

        //    Line newLine = new Line();
        //    newLine.Stroke = brush;
        //    newLine.StrokeThickness = OptionDrawLine.invisibleStrokeThickness;
        //    newLine.Opacity = 0;
        //    newLine.X1 = point1.X;
        //    newLine.Y1 = point1.Y;
        //    newLine.X2 = point2.X;
        //    newLine.Y2 = point2.Y;
        //    InvShapes.Add(newLine);
        //    DictionaryInvLines.Add(shape, newLine);
        //}
        
        //public void DrawAllRectangles()
        //{
        //    RectangleOfFigures.Clear();
        //    double size = OptionDrawLine.sizeRectangle;
        //    foreach (Point p in points)
        //    {
        //        Rectangle rec = new Rectangle();
        //        rec.Height = size;
        //        rec.Width = size;
        //        Canvas.SetLeft(rec, p.X - size / 2);
        //        Canvas.SetTop(rec, p.Y - size / 2);
        //        rec.Stroke = OptionColor.colorInactive;
        //        rec.Fill = OptionColor.colorOpacity;
        //        rec.StrokeThickness = OptionDrawLine.strokeThickness;
        //        RectangleOfFigures.Add(rec);
        //        canvas.Children.Add(rec);
        //    }
        //}

        //public void ChangeRectangleColor()
        //{
        //    for (int i = 0; i < points.Count; i++)
        //    {
        //        if (highlightedPoints.Contains(i))
        //            RectangleOfFigures[i].Fill = OptionColor.colorInactive;
        //        else
        //            RectangleOfFigures[i].Fill = OptionColor.colorOpacity;
        //    }
        //}
                
        //public void ClearFigure()
        //{
        //    pointStart = new Point();
        //    pointEnd = new Point();
        //    modeFigure = Mode.modeFigure;
        //    Shapes = new List<Shape>();
        //    dotsForFigure = new List<Shape>();
        //    groupFigures = new List<Figure>();
        //    groupFigures.Add(this);
        //    tatamiControlLine = new Vector();
        //    satinControlLines = new List<Vector>();
        //    oldTatamiCenter = new Point();
        //    oldSatinCenters = new List<Point>();
        //    InvShapes = new List<Shape>();
        //    points = new List<Point>();
        //    tempShapes = new List<Shape>();
        //    tempInvShapes = new List<Shape>();
        //    tempPoints = new List<Point>();
        //    highlightedPoints = new List<int>();
        //    RectangleOfFigures = new List<Rectangle>();
        //    preparedForTatami = false;
        //    shapeControlPoints = new Dictionary<Point, Tuple<Point, Point>>();
        //    DictionaryPointLines = new Dictionary<Point, Shape>();
        //    DictionaryInvLines = new Dictionary<Shape, Shape>();
        //    tempDictionaryPointLines = new Dictionary<Point, Shape>();
        //    tempDictionaryInvLines = new Dictionary<Shape, Shape>();
        //}

        //public void SaveCurrentShapes()
        //{
        //    tempInvShapes = new List<Shape>(InvShapes);
        //    tempShapes = new List<Shape>(Shapes);
        //    tempPoints = new List<Point>(points);
        //    tempDictionaryInvLines = new Dictionary<Shape, Shape>(DictionaryInvLines);
        //    tempDictionaryPointLines = new Dictionary<Point, Shape>(DictionaryPointLines);
        //}

        //public void LoadCurrentShapes()
        //{
        //    InvShapes = new List<Shape>(tempInvShapes);
        //    Shapes = new List<Shape>(tempShapes);
        //    points = new List<Point>(tempPoints);
        //    DictionaryInvLines = new Dictionary<Shape, Shape>(tempDictionaryInvLines);
        //    DictionaryPointLines = new Dictionary<Point, Shape>(tempDictionaryPointLines);
        //    preparedForTatami = false;
        //}
        
        public void AddPoint(Point newPoint)
        {
            if (points.Count == 0)
            {
                pointStart = newPoint;
            }
            points.Add(newPoint);
        //    if(addStar)
        //        AddStarForSinglePoint(false, brush);
        //    if (points.Count > 1)
        //    {
        //        Shape notUsed;
        //        bool dotOverlaps = true;
        //        while (dotOverlaps)
        //        {
        //            if (DictionaryPointLines.TryGetValue(pointEnd, out notUsed))
        //                pointEnd.X += 0.000000001;
        //            else
        //                dotOverlaps = false;
        //        }
        //        Line line = GetLine(pointEnd, New);
        //        line.StrokeThickness = OptionDrawLine.strokeThickness;
        //        line.Stroke = brush;
        //        canvas.Children.Add(line);
        //        Shapes.Add(line);
        //        DictionaryPointLines.Add(pointEnd, line);

        //        Line newLine = GetLine(pointEnd, New);
        //        newLine.Stroke = brush;
        //        newLine.StrokeThickness = OptionDrawLine.invisibleStrokeThickness;
        //        newLine.Opacity = 0;
        //        InvShapes.Add(newLine);
        //        canvas.Children.Add(newLine);

        //        DictionaryInvLines.Add(line, newLine);
        //    }
            pointEnd = newPoint;
        //    if (addRec)
        //    {
        //        Rectangle rec = new Rectangle();
        //        rec.Height = recSize;
        //        rec.Width = recSize;
        //        Canvas.SetLeft(rec, New.X - recSize / 2);
        //        Canvas.SetTop(rec, New.Y - recSize / 2);
        //        rec.Stroke = OptionColor.colorInactive;
        //        rec.Fill = OptionColor.colorOpacity;
        //        rec.StrokeThickness = OptionDrawLine.strokeThicknessMainRec;
        //        canvas.Children.Add(rec);
        //        return rec;
        //    }
        //    return null;
        }

        //public Line GetLine(Point start, Point end)
        //{
        //    Line line = new Line();
        //    line.X1 = start.X;
        //    line.Y1 = start.Y;
        //    line.X2 = end.X;
        //    line.Y2 = end.Y;
        //    return line;
        //}

        //public void AddStarForSinglePoint(bool checkForOnePoint, Brush brush)
        //{
        //    if (points.Count == 1)
        //    {
        //        Shape sh = GeometryHelper.SetStarForSinglePoint(points[0],brush, canvas);
        //        AddShape(sh, points[0], null);
        //    }
        //    else if (points.Count == 2 && !checkForOnePoint)
        //    {
        //        Shape sh;
        //        DictionaryPointLines.TryGetValue(points[0], out sh);
        //        canvas.Children.Remove(sh);
        //        DeleteShape(sh, points[0], canvas);
        //    }
        //}

        //public void ChangeFigureColor(Brush brush,bool isModeEditFigures)
        //{
        //    foreach (Shape shape in Shapes)
        //    {
        //        if (isModeEditFigures)
        //        {
        //            if(shape is Path)
        //            {
        //                Path ph = (Path)shape;
        //                if (ph.MinHeight == 5)
        //                    shape.Stroke = OptionColor.colorCurve;
        //                else if (ph.MinHeight == 10)
        //                    shape.Stroke = OptionColor.colorArc;
        //                else
        //                    shape.Stroke = OptionColor.colorActive;
        //            }
        //            else
        //                shape.Stroke = brush;
        //        }
        //        else
        //            shape.Stroke = brush;
        //    }
        //}

        //public void AddFigure(Canvas _canvas)
        //{
        //    foreach (Shape shape in InvShapes)
        //    {
        //        _canvas.Children.Add(shape);
        //    }
        //    foreach (Shape shape in Shapes)
        //    {
        //        _canvas.Children.Add(shape);
        //    }
        //}

        //public void RemoveFigure(Canvas _canvas)
        //{
        //    foreach (Shape shape in Shapes)
        //    {
        //        _canvas.Children.Remove(shape);
        //    }
        //}
        
        //public List<Point> GetFourPointsOfOutSideRectangle(double length)
        //{
        //    List<Point> pts = new List<Point>();
        //    Point max = new Point(Double.MinValue, Double.MinValue);
        //    Point min = new Point(Double.MaxValue, Double.MaxValue);
        //    List<Point> allPoints = new List<Point>(points);
        //    if (points.Count != 1)
        //    {
        //        foreach (Shape sh in Shapes)
        //            if (sh is Path)
        //            {
        //                Path path = (Path)sh;
        //                PathGeometry myPathGeometry = (PathGeometry)path.Data;
        //                Point p;
        //                Point tg;
        //                var points = new List<Point>();
        //                double step = 50;
        //                for (var j = 1; j < step; j++)
        //                {
        //                    myPathGeometry.GetPointAtFractionLength(j / step, out p, out tg);
        //                    allPoints.Add(p);
        //                }
        //            }
        //    }
        //    foreach (Point p in allPoints)
        //    {
        //        if (p.X > max.X)
        //            max.X = p.X;
        //        if (p.Y > max.Y)
        //            max.Y = p.Y;
        //        if (p.X < min.X)
        //            min.X = p.X;
        //        if (p.Y < min.Y)
        //            min.Y = p.Y;
        //    }
        //    pts.Add(new Point(min.X - length, min.Y - length));
        //    pts.Add(new Point(min.X - length, max.Y + length));
        //    pts.Add(new Point(max.X + length, max.Y + length));
        //    pts.Add(new Point(max.X + length, min.Y - length));
        //    return pts;
        //}

        //public Point SetMiddleControlLine(Point a, Point b, Canvas _canvas)
        //{
        //    double x = (a.X + b.X)/2;
        //    double y = (a.Y + b.Y) / 2;
        //    DrawEllipse(new Point(x, y), OptionColor.colorInactive, OptionDrawLine.sizeEllipseForControlLines, _canvas, true);
        //    return new Point(x, y);
        //}

        //public void DrawDots(List<Point> pts, double size, Brush color, Canvas _canvas)
        //{
        //    dotsForFigure = new List<Shape>();
        //    for (int i = 0; i < pts.Count; i++)
        //    {
        //        Ellipse ell = DrawEllipse(pts[i], color, OptionDrawLine.risuiModeDots, _canvas, false);
        //        dotsForFigure.Add(ell);
        //    }
        //}

        //public Ellipse DrawEllipse(Point p, Brush color, double size, Canvas _canvas,bool satinEllipse)
        //{
        //    Ellipse ell = new Ellipse();
        //    ell.Height = size;
        //    ell.Width = size;
        //    ell.Stretch = Stretch.Uniform;
        //    ell.Stroke = color;
        //    ell.Fill = color;
        //    Canvas.SetLeft(ell, p.X - size / 2);
        //    Canvas.SetTop(ell, p.Y - size / 2);
        //    GeometryHelper.RescaleEllipse(ell, OptionGrid.scaleMultiplier);
        //    if (satinEllipse)
        //    {
        //        Shapes.Add(ell);
        //        _canvas.Children.Add(ell);
        //    }
        //    else
        //    {
        //        ellipsePoint = new Point(p.X, p.Y);
        //        NewPointEllipse = ell;
        //        _canvas.Children.Add(NewPointEllipse);
        //    }
        //    return ell;
        //}

        //public Point GetCenter()
        //{
        //    Point max = new Point(Int32.MinValue, Int32.MinValue);
        //    Point min = new Point(Int32.MaxValue, Int32.MaxValue);
        //    List<Point> allPoints = new List<Point>(points);
        //    foreach (Shape sh in Shapes)
        //        if (sh is Path)
        //        {
        //            Path path = (Path)sh;
        //            PathGeometry myPathGeometry = (PathGeometry)path.Data;
        //            Point p;
        //            Point tg;
        //            var points = new List<Point>();
        //            double step = 50;
        //            for (var j = 1; j < step; j++)
        //            {
        //                myPathGeometry.GetPointAtFractionLength(j / step, out p, out tg);
        //                allPoints.Add(p);
        //            }
        //        }
        //    foreach (Point p in allPoints)
        //    {
        //        if (p.X > max.X)
        //            max.X = p.X;
        //        if (p.Y > max.Y)
        //            max.Y = p.Y;
        //        if (p.X < min.X)
        //            min.X = p.X;
        //        if (p.Y < min.Y)
        //            min.Y = p.Y;
        //    }
        //    return new Point((max.X + min.X) / 2, (max.Y + min.Y) / 2);
        //}
    }
}