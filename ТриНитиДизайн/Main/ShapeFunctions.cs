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
        //public Shape SetSpline(double tension,List<Point> TPoint)
        //{
        //    Path myPath = new Path();
        //    myPath.Stroke = OptionColor.colorActive;
        //    myPath.StrokeThickness = OptionDrawLine.strokeThickness;
        //    PathGeometry myPathGeometry = new PathGeometry();
        //    CanonicalSplineHelper spline = new CanonicalSplineHelper();
        //    myPathGeometry = spline.CreateSpline(TPoint, tension, null, false, false, 0.25);
        //    myPath.Data = myPathGeometry;
        //    return myPath;
        //}

        //public void PrepareForTatami(Figure fig, bool isColorChanged)
        //{
        //    fig.preparedForTatami = true;
        //    if (isColorChanged)
        //        fig.ChangeFigureColor(OptionColor.colorActive, false);
        //    for (int i = 0; i < fig.points.Count - 1; i++)
        //    {
        //        Shape sh;
        //        fig.DictionaryPointLines.TryGetValue(fig.points[i], out sh);
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
        //                fig.points.Insert(j + i, p);
        //            }
        //        }
        //    }
        //}

        //public List<Point> PrepareForBezier(Shape clickedShape, Point clickedPoint, Point firstShapeDot, Point secondShapeDot)
        //{
        //    List<Point> bezierPoints = new List<Point>();
        //    if(clickedShape is Line)
        //    {
        //        Line ln = (Line)clickedShape;
        //        Point firstDot = new Point(ln.X1,ln.Y1);
        //        Point secondDot = new Point(ln.X2, ln.Y2);
        //        Vector vect = (secondDot - firstDot) / 3;
        //        firstDot += vect;
        //        secondDot -= vect;
        //        bezierPoints.Add(firstShapeDot);
        //        bezierPoints.Add(firstDot);
        //        bezierPoints.Add(secondDot);
        //        bezierPoints.Add(secondShapeDot);
        //    }
        //    else
        //    {
        //        Tuple<Point, Point> contPts;
        //        listFigure[indexFigure].shapeControlPoints.TryGetValue(firstShapeDot, out contPts);
        //        bezierPoints.Add(firstShapeDot);
        //        bezierPoints.Add(contPts.Item1);
        //        bezierPoints.Add(contPts.Item2);
        //        bezierPoints.Add(secondShapeDot);
        //    }
        //    t = FindT(bezierPoints[0], bezierPoints[1], bezierPoints[2], bezierPoints[3], clickedPoint);
        //    prevPoint = clickedPoint;
        //    return bezierPoints;
        //}

        //public List<Point> ChangeBezierPoints(List<Point> bezierPts, Point currentPosition)
        //{
        //    double weight;
        //    if (t == 0)
        //        t = 0.05;
        //    if (t == 1)
        //        t = 0.95;
        //    if (t <= 1.0 / 6.0) weight = 0;
        //    else if (t <= 0.5) weight = (Math.Pow((6 * t - 1) / 2.0, 3)) / 2;
        //    else if (t <= 5.0 / 6.0) weight = (1 - Math.Pow((6 * (1 - t) - 1) / 2.0, 3)) / 2 + 0.5;
        //    else weight = 1;

        //    Point delta = new Point(currentPosition.X - prevPoint.X, currentPosition.Y - prevPoint.Y);
        //    Point offset0 = new Point(((1 - weight) / (3 * t * (1 - t) * (1 - t))) * delta.X, ((1 - weight) / (3 * t * (1 - t) * (1 - t))) * delta.Y);
        //    Point offset1 = new Point((weight / (3 * t * t * (1 - t))) * delta.X, (weight / (3 * t * t * (1 - t))) * delta.Y);

        //    bezierPts[1] = new Point(bezierPts[1].X + offset0.X, bezierPts[1].Y + offset0.Y);
        //    bezierPts[2] = new Point(bezierPts[2].X + offset1.X, bezierPts[2].Y + offset1.Y);
        //    prevPoint = currentPosition;
        //    return bezierPts;
        //}

        //public void ManipulateShapes(Vector delta)
        //{
        //    for(int i = 0; i < listChangedShapes.Count; i++)
        //    {
        //        listChangedShapes[i].RemoveRectangleAndShape();
        //        listChangedShapes[i].ManipulateShape(delta);
        //    }
        //    for(int i = 0; i < listFigure[indexFigure].points.Count; i++)
        //    {
        //        if(listFigure[indexFigure].highlightedPoints.Contains(i))
        //        {
        //            listFigure[indexFigure].points[i] += delta;
        //        }
        //    }
        //}

        //public void AddManipulatedShapes(Canvas canvas)
        //{
        //    for (int i = 0; i < listChangedShapes.Count; i++)
        //    {
        //        Point contPoint1 = listChangedShapes[i].firstControlPoint;
        //        Point contPoint2 = listChangedShapes[i].secondControlPoint;
        //        Tuple<Point, Point> contPts = new Tuple<Point, Point>(contPoint1, contPoint2);
        //        Point p = listChangedShapes[i].firstPoint;
        //        listFigure[indexFigure].AddShape(listChangedShapes[i].changedShape, p, contPts);
        //    }
        //    listFigure[indexFigure].pointStart = listFigure[indexFigure].points[0];
        //    listFigure[indexFigure].pointEnd = listFigure[indexFigure].points[listFigure[indexFigure].points.Count - 1];
        //}

        //private double FindT(Point firstDot, Point controlDot1, Point controlDot2, Point lastDot, Point clickedDot)
        //{
        //    double _t = 0;
        //    List<Point> pts = new List<Point>();
        //    Point A = firstDot;
        //    Point B = controlDot1;
        //    Point C = controlDot2;
        //    Point D = lastDot;
        //    pts.Add(A);
        //    for (double i = 1; i < 500; i++)
        //    {
        //        double divide = i / 500;
        //        Point E = Lerp(A, B, divide);
        //        Point F = Lerp(B, C, divide);
        //        Point G = Lerp(C, D, divide);
        //        Point H = Lerp(E, F, divide);
        //        Point J = Lerp(F, G, divide);
        //        Point K = Lerp(H, J, divide);
        //        pts.Add(K);
        //    }
        //    pts.Add(D);
        //    double distance = Double.MaxValue;
        //    for (int i = 0; i < pts.Count; i++)
        //    {
        //        if (FindLength(pts[i], clickedDot) < distance)
        //        {
        //            distance = FindLength(pts[i], clickedDot);
        //            _t = (double)i / 500;
        //        }
        //    }
        //    return _t;
        //}

        //private List<Point> ApproximateCurveDivsionPoint(Point firstDot, Point controlDot1, Point controlDot2, Point lastDot, Point clickedDot)
        //{
        //    List<Point> curvePoints = new List<Point>();
        //    Point A = firstDot;
        //    Point B = controlDot1;
        //    Point C = controlDot2;
        //    Point D = lastDot;
        //    double distance = Double.MaxValue;
        //    for (double i = 1; i < 500; i++)
        //    {
        //        double divide = i / 500;
        //        Point E = Lerp(A, B, divide);
        //        Point F = Lerp(B, C, divide);
        //        Point G = Lerp(C, D, divide);
        //        Point H = Lerp(E, F, divide);
        //        Point J = Lerp(F, G, divide);
        //        Point K = Lerp(H, J, divide);
        //        if(FindLength(K,clickedDot) < distance)
        //        {
        //            curvePoints.Clear();
        //            distance = FindLength(K, clickedDot);
        //            curvePoints.Add(E);
        //            curvePoints.Add(H);
        //            curvePoints.Add(J);
        //            curvePoints.Add(G);
        //        }
        //    }
        //    curvePoints.Insert(0, A);
        //    curvePoints.Insert(3, clickedDot);
        //    curvePoints.Add(lastDot);
        //    return curvePoints;
        //}

        //private List<Point> ApproximateArcDivisionPoint(Shape arcShape, Point firstPoint, Point secondPoint, Point clickedPoint)
        //{
        //    Path path = (Path)arcShape;
        //    PathGeometry myPathGeometry = (PathGeometry)path.Data;
        //    List<Point> arcPoints = new List<Point>();
        //    arcPoints.Add(firstPoint);
        //    Point p;
        //    Point tg;
        //    double distance = Double.MaxValue;
        //    double fraction = 0;
        //    for (double j = 1; j <= 100; j++)
        //    {
        //        myPathGeometry.GetPointAtFractionLength(j / 100.0, out p, out tg);
        //        if(FindLength(clickedPoint,p) < distance)
        //        {
        //            distance = FindLength(clickedPoint, p);
        //            fraction = j / 100.0;
        //        }
        //    }
        //    myPathGeometry.GetPointAtFractionLength(fraction / 2, out p, out tg);
        //    arcPoints.Add(p);
        //    arcPoints.Add(clickedPoint);
        //    myPathGeometry.GetPointAtFractionLength((1 - fraction) / 2 + fraction, out p, out tg);
        //    arcPoints.Add(p);
        //    arcPoints.Add(secondPoint);
        //    return arcPoints;
        //}

        //private Point Lerp(Point A, Point B, double x)
        //{
        //    double s = 1 - x;
        //    return new Point(A.X * s + B.X * x, A.Y * s + B.Y * x);
        //}

        //public void MakeLomanaya(Figure fig, Canvas canvas)
        //{
        //    for (int i = 0; i < fig.highlightedPoints.Count - 1; i++)
        //    {
        //        if (fig.highlightedPoints[i] == (fig.highlightedPoints[i + 1] - 1))
        //        {
        //            Shape sh;
        //            fig.DictionaryPointLines.TryGetValue(fig.points[fig.highlightedPoints[i]], out sh);
        //            fig.DeleteShape(sh, fig.points[fig.highlightedPoints[i]],canvas);
        //            fig.AddLine(fig.points[fig.highlightedPoints[i]], fig.points[fig.highlightedPoints[i + 1]], OptionColor.colorActive);
        //        }
        //    }
        //}

        //public Rectangle DrawChoosingRectangle(Point p1, Point p2, Canvas canvas)
        //{
        //    Rectangle rec = new Rectangle();
        //    rec.Height = Math.Abs(p2.Y - p1.Y);
        //    rec.Width = Math.Abs(p2.X - p1.X);
        //    DoubleCollection dashes = new DoubleCollection();
        //    dashes.Add(2);
        //    dashes.Add(2);
        //    rec.StrokeDashArray = dashes;
        //    if (p2.X > p1.X)
        //    {
        //        Canvas.SetLeft(rec, p1.X);
        //    }
        //    else
        //    {
        //        Canvas.SetLeft(rec, p2.X);
        //    }
        //    if (p2.Y > p1.Y)
        //    {
        //        Canvas.SetTop(rec, p1.Y);
        //    }
        //    else
        //    {
        //        Canvas.SetTop(rec, p2.Y);
        //    }
        //    rec.Stroke = OptionColor.colorArc;
        //    rec.StrokeThickness = OptionDrawLine.strokeThickness;
        //    canvas.Children.Add(rec);
        //    return rec;
        //}

        //public void DrawAllChosenLines(Figure fig, Brush brush, Canvas canvas)
        //{
        //    for (int i = 0; i < fig.highlightedPoints.Count - 1; i++)
        //    {
        //        if (fig.highlightedPoints[i] == (fig.highlightedPoints[i + 1] - 1))
        //        {
        //            Shape sh;
        //            fig.DictionaryPointLines.TryGetValue(fig.points[fig.highlightedPoints[i]], out sh);
        //            sh.Stroke = brush;
        //        }
        //    }
        //}

        //public void ChooseNextRectangle(Figure fig, bool isNext, Canvas canvas)
        //{
        //    if (fig.highlightedPoints.Count == 1)
        //    {
        //        if (isNext)
        //        {
        //            if (fig.highlightedPoints[0] != fig.points.Count - 1)
        //                fig.highlightedPoints[0]++;
        //        }
        //        else
        //        {
        //            if (fig.highlightedPoints[0] != 0)
        //                fig.highlightedPoints[0]--;
        //        }
        //    }
        //}

        //public void SplitFigureInTwo(Figure fig, Canvas canvas)
        //{
        //    if (fig.highlightedPoints.Count == 1 && fig.highlightedPoints[0] != 0)
        //    {
        //        Figure newFig = new Figure(canvas);
        //        for (int i = 0; i <= fig.highlightedPoints[0]; i++)
        //        {
        //            Point p = fig.points[i];
        //            if (i != fig.highlightedPoints[0])
        //            {
        //                Shape sh;
        //                fig.DictionaryPointLines.TryGetValue(fig.points[i], out sh);
        //                Tuple<Point,Point> contP;
        //                fig.shapeControlPoints.TryGetValue(p, out contP);
        //                newFig.AddShape(sh, p, contP);
        //            }
        //            if (i == 0)
        //                newFig.pointStart = p;
        //            if (i == fig.highlightedPoints[0])
        //                newFig.pointEnd = p;
        //            newFig.points.Add(p);
        //        }
        //        listFigure.Insert(indexFigure, newFig);
        //        newFig = new Figure(canvas);
        //        for (int i = fig.highlightedPoints[0]; i < fig.points.Count; i++)
        //        {
        //            Point p = fig.points[i];
        //            if (i != fig.points.Count - 1)
        //            {
        //                Shape sh;
        //                fig.DictionaryPointLines.TryGetValue(fig.points[i], out sh);
        //                Tuple<Point, Point> contP;
        //                fig.shapeControlPoints.TryGetValue(p, out contP);
        //                newFig.AddShape(sh, p, contP);
        //            }
        //            if (i == fig.highlightedPoints[0])
        //                newFig.pointStart = p;
        //            if (i == fig.points.Count - 1)
        //                newFig.pointEnd = p;
        //            newFig.points.Add(p);
        //        }
        //        newFig.ChangeFigureColor(OptionColor.colorInactive, false);

        //        List<Figure> group = new List<Figure>(fig.groupFigures);
        //        foreach (Figure figGroup in group)
        //            figGroup.groupFigures.Remove(fig);

        //        listFigure.Insert(indexFigure + 1, newFig);
        //        listFigure.Remove(fig);
        //    }
        //}

        //public void MakeSpline(Figure fig,Brush brush, Canvas canvas)
        //{
        //    for (int i = 0; i < fig.highlightedPoints.Count - 1; i++)
        //    {
        //        if (fig.highlightedPoints[i] == (fig.highlightedPoints[i + 1] - 1))
        //        {
        //            List<Point> newList = new List<Point>();
        //            if(fig.points.Count == 2)
        //            {
        //                newList.Add(fig.points[fig.highlightedPoints[i]]);
        //                newList.Add(fig.points[fig.highlightedPoints[i]]);
        //                newList.Add(fig.points[fig.highlightedPoints[i] + 1]);
        //                newList.Add(fig.points[fig.highlightedPoints[i] + 1]);
        //            }
        //            else if (fig.highlightedPoints[i] == 0)
        //            {
        //                newList.Add(fig.points[fig.highlightedPoints[i]]);
        //                newList.Add(fig.points[fig.highlightedPoints[i]]);
        //                newList.Add(fig.points[fig.highlightedPoints[i] + 1]);
        //                newList.Add(fig.points[fig.highlightedPoints[i] + 2]);
        //            }
        //            else if (fig.highlightedPoints[i] == fig.points.Count - 2)
        //            {
        //                newList.Add(fig.points[fig.highlightedPoints[i] - 1]);
        //                newList.Add(fig.points[fig.highlightedPoints[i]]);
        //                newList.Add(fig.points[fig.highlightedPoints[i] + 1]);
        //                newList.Add(fig.points[fig.highlightedPoints[i] + 1]);
        //            }
        //            else
        //            {
        //                newList.Add(fig.points[fig.highlightedPoints[i] - 1]);
        //                newList.Add(fig.points[fig.highlightedPoints[i]]);
        //                newList.Add(fig.points[fig.highlightedPoints[i] + 1]);
        //                newList.Add(fig.points[fig.highlightedPoints[i] + 2]);
        //            }
        //            Shape sh;
        //            fig.DictionaryPointLines.TryGetValue(fig.points[fig.highlightedPoints[i]], out sh);
        //            fig.DeleteShape(sh, fig.points[fig.highlightedPoints[i]],mainCanvas);
        //            double SX1 = 0.7 * (newList[2].X - newList[0].X) / 3 + newList[1].X;
        //            double SY1 = 0.7 * (newList[2].Y - newList[0].Y) / 3 + newList[1].Y;
        //            double SX2 = newList[2].X - 0.7 * (newList[3].X - newList[1].X) / 3;
        //            double SY2 = newList[2].Y  - 0.7 * (newList[3].Y - newList[1].Y) / 3;
        //            Point controlPoint1 = new Point(SX1,SY1);
        //            Point controlPoint2 = new Point(SX2,SY2);
        //            sh = GeometryHelper.SetBezier(OptionColor.colorCurve, newList[1], controlPoint1, controlPoint2, newList[2], canvas);
        //            fig.AddShape(sh, fig.points[fig.highlightedPoints[i]], new Tuple<Point,Point>(controlPoint1,controlPoint2));
        //        }
        //    }
        //}

        //public void ReverseFigure(Figure fig, Canvas canvas)
        //{
        //    Figure newFig = new Figure(canvas);
        //    for (int i = fig.points.Count - 1; i >= 0; i--)
        //    {
        //        Point p = fig.points[i];
        //        if (i != 0)
        //        {
        //            Shape sh;
        //            fig.DictionaryPointLines.TryGetValue(fig.points[i - 1], out sh);
        //            Tuple<Point,Point> contP;
        //            fig.shapeControlPoints.TryGetValue(fig.points[i - 1], out contP);
        //            if(sh is Path)
        //            {
        //                if (sh.MinHeight == 5)
        //                {
        //                    contP = new Tuple<Point,Point>(contP.Item2,contP.Item1);
        //                    sh = GeometryHelper.SetBezier(OptionColor.colorActive, p, contP.Item1, contP.Item2, fig.points[i - 1], canvas);
        //                }
        //                else if(sh.MinHeight == 10)
        //                    sh = GeometryHelper.SetArc(OptionColor.colorActive, p, fig.points[i - 1], contP.Item1, canvas);
        //            }
        //            newFig.AddShape(sh, p, contP);
        //        }
        //        newFig.points.Add(p);
        //    }
        //    newFig.pointStart = fig.pointEnd;
        //    newFig.pointEnd = fig.pointStart;
        //    CopyParametersToNewFigure(newFig, fig);
        //    listFigure.Insert(indexFigure, newFig);
        //    listFigure.Remove(fig);
        //}

        //public void AddPointToFigure(Figure fig, Canvas canvas)
        //{
        //    if (canvas.Children.Contains(fig.NewPointEllipse))
        //    {
        //        fig.highlightedPoints.Clear();
        //        int index = fig.points.IndexOf(fig.pointForAddingPoints);
        //        fig.points.Insert(index + 1, fig.ellipsePoint);
        //        Shape sh;
        //        fig.DictionaryPointLines.TryGetValue(fig.points[index], out sh);
        //        if (sh is Line)
        //        {
        //            fig.DeleteShape(sh, fig.points[index], canvas);
        //            Line ln = new Line();
        //            ln = GeometryHelper.SetLine(OptionColor.colorActive, fig.points[index], fig.ellipsePoint,false, canvas);
        //            fig.AddShape(ln, fig.points[index], new Tuple<Point, Point>(new Point(), new Point()));
        //            ln = GeometryHelper.SetLine(OptionColor.colorActive, fig.ellipsePoint, fig.points[index + 2], false, canvas);
        //            fig.AddShape(ln, fig.ellipsePoint, new Tuple<Point, Point>(new Point(), new Point()));
        //        }
        //        else if(sh.Stroke == OptionColor.colorCurve)
        //        {
        //            Tuple<Point, Point> contPts;
        //            fig.shapeControlPoints.TryGetValue(fig.points[index], out contPts);
        //            List<Point> curvePts = ApproximateCurveDivsionPoint(fig.points[index], contPts.Item1, contPts.Item2, fig.points[index + 2], fig.ellipsePoint);
        //            fig.DeleteShape(sh, fig.points[index], canvas);
        //            Shape newCurve;
        //            newCurve = GeometryHelper.SetBezier(OptionColor.colorCurve, curvePts[0], curvePts[1], curvePts[2], curvePts[3], canvas);
        //            fig.AddShape(newCurve, fig.points[index], new Tuple<Point, Point>(curvePts[1], curvePts[2]));
        //            newCurve = GeometryHelper.SetBezier(OptionColor.colorCurve, curvePts[3], curvePts[4], curvePts[5], curvePts[6], canvas);
        //            fig.AddShape(newCurve, fig.ellipsePoint, new Tuple<Point, Point>(curvePts[4], curvePts[5]));
        //        }
        //        else
        //        {
        //            fig.DeleteShape(sh, fig.points[index], canvas);
        //            List<Point> arcPts = ApproximateArcDivisionPoint(sh, fig.points[index], fig.points[index + 2], fig.ellipsePoint);
        //            Shape newArc;
        //            newArc = GeometryHelper.SetArc(OptionColor.colorArc, arcPts[0], arcPts[2], arcPts[1], canvas);
        //            fig.AddShape(newArc, fig.points[index], new Tuple<Point, Point>(arcPts[1], new Point()));
        //            newArc = GeometryHelper.SetArc(OptionColor.colorArc, arcPts[2], arcPts[4], arcPts[3], canvas);
        //            fig.AddShape(newArc, arcPts[2], new Tuple<Point, Point>(arcPts[3], new Point()));
        //        }
        //    }
        //}
        
        //public void DeletePointFromFigure(Figure fig, Canvas canvas)
        //{
        //    if (fig.highlightedPoints.Count > 0)
        //    {
        //        Figure newFig = new Figure(canvas);
        //        Shape prevShape = new Path();
        //        for (int i = 0; i < fig.points.Count; i++)
        //        {
        //            bool foundDeletePoint = false;
        //            bool nextPointDeleted = false;
        //            int deletedInARow = 0;
        //            for (int j = 0; j < fig.highlightedPoints.Count; j++)
        //            {
        //                if (i == fig.highlightedPoints[j])
        //                {
        //                    i++;
        //                    foundDeletePoint = true;
        //                    deletedInARow++;
        //                }
        //                if (i + 1 == fig.highlightedPoints[j])
        //                {
        //                    nextPointDeleted = true;
        //                }
        //            }
        //            if (i != fig.points.Count)
        //            {
        //                if (foundDeletePoint)
        //                {
        //                    Shape sh;
        //                    fig.DictionaryPointLines.TryGetValue(fig.points[i - 1], out sh);
        //                    Tuple<Point, Point> contP;
        //                    fig.shapeControlPoints.TryGetValue(fig.points[i - 1], out contP);
        //                    if (sh is Path && i!= 1)
        //                    {
        //                        deletedInARow++;
        //                        if (sh.MinHeight == 5)
        //                        {
        //                            contP = new Tuple<Point, Point>(contP.Item2, contP.Item1);
        //                            sh = GeometryHelper.SetBezier(OptionColor.colorCurve, fig.points[i - deletedInARow], contP.Item1, contP.Item2, 
        //                                fig.points[i], canvas);
        //                        }
        //                        else if (sh.MinHeight == 10)
        //                            sh = GeometryHelper.SetArc(OptionColor.colorArc, fig.points[i - deletedInARow], fig.points[i], contP.Item1, canvas);
        //                        newFig.AddShape(sh, fig.points[i - deletedInARow], contP);
        //                        newFig.points.Add(fig.points[i]);
        //                    }
        //                    else
        //                        newFig.AddPoint(fig.points[i], OptionColor.colorActive, true,false, OptionDrawLine.sizeRectangle);
        //                    if (i != fig.points.Count - 1 && !nextPointDeleted)
        //                    {
        //                        fig.DictionaryPointLines.TryGetValue(fig.points[i], out prevShape);
        //                        fig.shapeControlPoints.TryGetValue(fig.points[i], out contP);
        //                        newFig.AddShape(prevShape, fig.points[i], contP);
        //                    }
        //                }
        //                else
        //                {
        //                    Point p = fig.points[i];
        //                    if (i != fig.points.Count - 1 && !nextPointDeleted)
        //                    {
        //                        fig.DictionaryPointLines.TryGetValue(fig.points[i], out prevShape);
        //                        Tuple<Point, Point> contP;
        //                        fig.shapeControlPoints.TryGetValue(fig.points[i], out contP);
        //                        newFig.AddShape(prevShape, fig.points[i], contP);
        //                    }
        //                    newFig.pointEnd = p;
        //                    newFig.points.Add(p);
        //                }
        //            }
        //        }
        //        if (newFig.points.Count > 0)
        //        {
        //            newFig.pointStart = newFig.points[0];
        //        }

        //        CopyParametersToNewFigure(newFig, fig);
        //        newFig.AddStarForSinglePoint(true, OptionColor.colorActive);
        //        listFigure.Insert(indexFigure, newFig);
        //        listFigure.Remove(fig);
        //    }
        //}

        //public void CopyParametersToNewFigure(Figure newFig, Figure oldFig)
        //{
        //    newFig.satinControlLines = oldFig.satinControlLines;
        //    newFig.oldSatinCenters = oldFig.oldSatinCenters;
        //    newFig.oldTatamiCenter = oldFig.oldTatamiCenter;
        //    newFig.modeFigure = oldFig.modeFigure;
        //    newFig.tatamiControlLine = oldFig.tatamiControlLine;

        //    newFig.groupFigures = oldFig.groupFigures;
        //    List<Figure> group = new List<Figure>(oldFig.groupFigures);
        //    foreach (Figure figGroup in group)
        //    {
        //        int index = figGroup.groupFigures.IndexOf(oldFig);
        //        figGroup.groupFigures.Insert(index, newFig);
        //        figGroup.groupFigures.Remove(oldFig);
        //    }
        //}
    }
}