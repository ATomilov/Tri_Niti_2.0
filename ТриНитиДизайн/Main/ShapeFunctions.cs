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
        public Shape SetSpline(double tension,List<Point> TPoint)
        {
            Path myPath = new Path();
            myPath.Stroke = OptionColor.ColorDraw;
            myPath.StrokeThickness = OptionDrawLine.StrokeThickness;
            PathGeometry myPathGeometry = new PathGeometry();
            CanonicalSplineHelper spline = new CanonicalSplineHelper();
            myPathGeometry = spline.CreateSpline(TPoint, tension, null, false, false, 0.25);
            myPath.Data = myPathGeometry;
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

        public List<Point> PrepareForBezier(Shape clickedShape, Point clickedPoint, Point firstShapeDot, Point secondShapeDot)
        {
            List<Point> bezierPoints = new List<Point>();
            if(clickedShape is Line)
            {
                Line ln = (Line)clickedShape;
                Point firstDot = new Point(ln.X1,ln.Y1);
                Point secondDot = new Point(ln.X2, ln.Y2);
                Vector vect = (secondDot - firstDot) / 3;
                firstDot += vect;
                secondDot -= vect;
                bezierPoints.Add(firstShapeDot);
                bezierPoints.Add(firstDot);
                bezierPoints.Add(secondDot);
                bezierPoints.Add(secondShapeDot);
            }
            else
            {
                Tuple<Point, Point> contPts;
                ListFigure[IndexFigure].DictionaryShapeControlPoints.TryGetValue(firstShapeDot, out contPts);
                bezierPoints.Add(firstShapeDot);
                bezierPoints.Add(contPts.Item1);
                bezierPoints.Add(contPts.Item2);
                bezierPoints.Add(secondShapeDot);
            }
            t = FindT(bezierPoints[0], bezierPoints[1], bezierPoints[2], bezierPoints[3], clickedPoint);
            prevPoint = clickedPoint;
            return bezierPoints;
        }

        public List<Point> ChangeBezierPoints(List<Point> bezierPts, Point currentPosition)
        {
            double weight;
            if (t == 0)
                t = 0.05;
            if (t == 1)
                t = 0.95;
            if (t <= 1.0 / 6.0) weight = 0;
            else if (t <= 0.5) weight = (Math.Pow((6 * t - 1) / 2.0, 3)) / 2;
            else if (t <= 5.0 / 6.0) weight = (1 - Math.Pow((6 * (1 - t) - 1) / 2.0, 3)) / 2 + 0.5;
            else weight = 1;

            Point delta = new Point(currentPosition.X - prevPoint.X, currentPosition.Y - prevPoint.Y);
            Point offset0 = new Point(((1 - weight) / (3 * t * (1 - t) * (1 - t))) * delta.X, ((1 - weight) / (3 * t * (1 - t) * (1 - t))) * delta.Y);
            Point offset1 = new Point((weight / (3 * t * t * (1 - t))) * delta.X, (weight / (3 * t * t * (1 - t))) * delta.Y);

            bezierPts[1] = new Point(bezierPts[1].X + offset0.X, bezierPts[1].Y + offset0.Y);
            bezierPts[2] = new Point(bezierPts[2].X + offset1.X, bezierPts[2].Y + offset1.Y);
            prevPoint = currentPosition;
            return bezierPts;
        }

        public void ManipulateShapes(Vector delta)
        {
            for(int i = 0; i < listChangedShapes.Count; i++)
            {
                listChangedShapes[i].RemoveRectangleAndShape();
                listChangedShapes[i].ManipulateShape(delta);
            }
            for(int i = 0; i < ListFigure[IndexFigure].Points.Count; i++)
            {
                if(ListFigure[IndexFigure].PointsCount.Contains(i))
                {
                    ListFigure[IndexFigure].Points[i] += delta;
                }
            }
        }

        public void AddManipulatedShapes(Canvas canvas)
        {
            for (int i = 0; i < listChangedShapes.Count; i++)
            {
                Point contPoint1 = listChangedShapes[i].firstControlPoint;
                Point contPoint2 = listChangedShapes[i].secondControlPoint;
                Tuple<Point, Point> contPts = new Tuple<Point, Point>(contPoint1, contPoint2);
                Point p = listChangedShapes[i].firstPoint;
                ListFigure[IndexFigure].AddShape(listChangedShapes[i].changedShape, p, contPts);
            }
            ListFigure[IndexFigure].PointStart = ListFigure[IndexFigure].Points[0];
            ListFigure[IndexFigure].PointEnd = ListFigure[IndexFigure].Points[ListFigure[IndexFigure].Points.Count - 1];
        }

        private double FindT(Point firstDot, Point controlDot1, Point controlDot2, Point lastDot, Point clickedDot)
        {
            double _t = 0;
            List<Point> pts = new List<Point>();
            Point A = firstDot;
            Point B = controlDot1;
            Point C = controlDot2;
            Point D = lastDot;
            pts.Add(A);
            for (double i = 1; i < 500; i++)
            {
                double divide = i / 500;
                Point E = Lerp(A, B, divide);
                Point F = Lerp(B, C, divide);
                Point G = Lerp(C, D, divide);
                Point H = Lerp(E, F, divide);
                Point J = Lerp(F, G, divide);
                Point K = Lerp(H, J, divide);
                pts.Add(K);
            }
            pts.Add(D);
            double distance = Double.MaxValue;
            for (int i = 0; i < pts.Count; i++)
            {
                if (FindLength(pts[i], clickedDot) < distance)
                {
                    distance = FindLength(pts[i], clickedDot);
                    _t = (double)i / 500;
                }
            }
            return _t;
        }

        private List<Point> ApproximateCurveDivsionPoint(Point firstDot, Point controlDot1, Point controlDot2, Point lastDot, Point clickedDot)
        {
            List<Point> curvePoints = new List<Point>();
            Point A = firstDot;
            Point B = controlDot1;
            Point C = controlDot2;
            Point D = lastDot;
            double distance = Double.MaxValue;
            for (double i = 1; i < 500; i++)
            {
                double divide = i / 500;
                Point E = Lerp(A, B, divide);
                Point F = Lerp(B, C, divide);
                Point G = Lerp(C, D, divide);
                Point H = Lerp(E, F, divide);
                Point J = Lerp(F, G, divide);
                Point K = Lerp(H, J, divide);
                if(FindLength(K,clickedDot) < distance)
                {
                    curvePoints.Clear();
                    distance = FindLength(K, clickedDot);
                    curvePoints.Add(E);
                    curvePoints.Add(H);
                    curvePoints.Add(J);
                    curvePoints.Add(G);
                }
            }
            curvePoints.Insert(0, A);
            curvePoints.Insert(3, clickedDot);
            curvePoints.Add(lastDot);
            return curvePoints;
        }

        private List<Point> ApproximateArcDivisionPoint(Shape arcShape, Point firstPoint, Point secondPoint, Point clickedPoint)
        {
            Path path = (Path)arcShape;
            PathGeometry myPathGeometry = (PathGeometry)path.Data;
            List<Point> arcPoints = new List<Point>();
            arcPoints.Add(firstPoint);
            Point p;
            Point tg;
            double distance = Double.MaxValue;
            double fraction = 0;
            for (double j = 1; j <= 100; j++)
            {
                myPathGeometry.GetPointAtFractionLength(j / 100.0, out p, out tg);
                if(FindLength(clickedPoint,p) < distance)
                {
                    distance = FindLength(clickedPoint, p);
                    fraction = j / 100.0;
                }
            }
            myPathGeometry.GetPointAtFractionLength(fraction / 2, out p, out tg);
            arcPoints.Add(p);
            arcPoints.Add(clickedPoint);
            myPathGeometry.GetPointAtFractionLength((1 - fraction) / 2 + fraction, out p, out tg);
            arcPoints.Add(p);
            arcPoints.Add(secondPoint);
            return arcPoints;
        }

        private Point Lerp(Point A, Point B, double x)
        {
            double s = 1 - x;
            return new Point(A.X * s + B.X * x, A.Y * s + B.Y * x);
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
                        Tuple<Point,Point> contP;
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
                        Tuple<Point, Point> contP;
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

                List<Figure> group = new List<Figure>(fig.groupFigures);
                foreach (Figure figGroup in group)
                    figGroup.groupFigures.Remove(fig);

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
                    double SX1 = 0.7 * (newList[2].X - newList[0].X) / 3 + newList[1].X;
                    double SY1 = 0.7 * (newList[2].Y - newList[0].Y) / 3 + newList[1].Y;
                    double SX2 = newList[2].X - 0.7 * (newList[3].X - newList[1].X) / 3;
                    double SY2 = newList[2].Y  - 0.7 * (newList[3].Y - newList[1].Y) / 3;
                    Point controlPoint1 = new Point(SX1,SY1);
                    Point controlPoint2 = new Point(SX2,SY2);
                    sh = GeometryHelper.SetBezier(OptionColor.ColorKrivaya, newList[1], controlPoint1, controlPoint2, newList[2], canvas);
                    fig.AddShape(sh, fig.Points[fig.PointsCount[i]], new Tuple<Point,Point>(controlPoint1,controlPoint2));
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
                    Tuple<Point,Point> contP;
                    fig.DictionaryShapeControlPoints.TryGetValue(fig.Points[i - 1], out contP);
                    if(sh is Path)
                    {
                        if (sh.MinHeight == 5)
                        {
                            contP = new Tuple<Point,Point>(contP.Item2,contP.Item1);
                            sh = GeometryHelper.SetBezier(OptionColor.ColorDraw, p, contP.Item1, contP.Item2, fig.Points[i - 1], canvas);
                        }
                        else
                            sh = GeometryHelper.SetArc(OptionColor.ColorDraw, p, fig.Points[i - 1], contP.Item1, canvas);
                    }
                    newFig.AddShape(sh, p, contP);
                }
                newFig.Points.Add(p);
            }
            newFig.PointStart = fig.PointEnd;
            newFig.PointEnd = fig.PointStart;

            newFig.groupFigures = fig.groupFigures;
            List<Figure> group = new List<Figure>(fig.groupFigures);
            foreach (Figure figGroup in group)
            {
                int index = figGroup.groupFigures.IndexOf(fig);
                figGroup.groupFigures.Insert(index, newFig);
                figGroup.groupFigures.Remove(fig);
            }
            ListFigure.Insert(IndexFigure, newFig);
            ListFigure.Remove(fig);
        }

        public void AddPointToFigure(Figure fig, Canvas canvas)
        {
            if (canvas.Children.Contains(fig.NewPointEllipse))
            {
                fig.PointsCount.Clear();
                int index = fig.Points.IndexOf(fig.PointForAddingPoints);
                fig.Points.Insert(index + 1, fig.EllipsePoint);
                Shape sh;
                fig.DictionaryPointLines.TryGetValue(fig.Points[index], out sh);
                if (sh is Line)
                {
                    fig.DeleteShape(sh, fig.Points[index], canvas);
                    Line ln = new Line();
                    ln = GeometryHelper.SetLine(OptionColor.ColorDraw, fig.Points[index], fig.EllipsePoint,false, canvas);
                    fig.AddShape(ln, fig.Points[index], new Tuple<Point, Point>(new Point(), new Point()));
                    ln = GeometryHelper.SetLine(OptionColor.ColorDraw, fig.EllipsePoint, fig.Points[index + 2], false, canvas);
                    fig.AddShape(ln, fig.EllipsePoint, new Tuple<Point, Point>(new Point(), new Point()));
                }
                else if(sh.Stroke == OptionColor.ColorKrivaya)
                {
                    Tuple<Point, Point> contPts;
                    fig.DictionaryShapeControlPoints.TryGetValue(fig.Points[index], out contPts);
                    List<Point> curvePts = ApproximateCurveDivsionPoint(fig.Points[index], contPts.Item1, contPts.Item2, fig.Points[index + 2], fig.EllipsePoint);
                    fig.DeleteShape(sh, fig.Points[index], canvas);
                    Shape newCurve;
                    newCurve = GeometryHelper.SetBezier(OptionColor.ColorKrivaya, curvePts[0], curvePts[1], curvePts[2], curvePts[3], canvas);
                    fig.AddShape(newCurve, fig.Points[index], new Tuple<Point, Point>(curvePts[1], curvePts[2]));
                    newCurve = GeometryHelper.SetBezier(OptionColor.ColorKrivaya, curvePts[3], curvePts[4], curvePts[5], curvePts[6], canvas);
                    fig.AddShape(newCurve, fig.EllipsePoint, new Tuple<Point, Point>(curvePts[4], curvePts[5]));
                }
                else
                {
                    fig.DeleteShape(sh, fig.Points[index], canvas);
                    List<Point> arcPts = ApproximateArcDivisionPoint(sh, fig.Points[index], fig.Points[index + 2], fig.EllipsePoint);
                    Shape newArc;
                    newArc = GeometryHelper.SetArc(OptionColor.ColorChoosingRec, arcPts[0], arcPts[2], arcPts[1], canvas);
                    fig.AddShape(newArc, fig.Points[index], new Tuple<Point, Point>(arcPts[1], new Point()));
                    newArc = GeometryHelper.SetArc(OptionColor.ColorChoosingRec, arcPts[2], arcPts[4], arcPts[3], canvas);
                    fig.AddShape(newArc, arcPts[2], new Tuple<Point, Point>(arcPts[3], new Point()));
                }
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
                            newFig.AddPoint(fig.Points[i], OptionColor.ColorDraw, true,false, OptionDrawLine.SizeWidthAndHeightRectangle);
                            if (i != fig.Points.Count - 1 && !nextDotDeleted)
                            {
                                fig.DictionaryPointLines.TryGetValue(fig.Points[i], out prevShape);
                                Tuple<Point, Point> contP;
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
                                Tuple<Point, Point> contP;
                                fig.DictionaryShapeControlPoints.TryGetValue(fig.Points[i], out contP);
                                newFig.AddShape(prevShape, fig.Points[i], contP);
                            }
                            newFig.PointEnd = p;
                            newFig.Points.Add(p);
                        }
                    }
                }
                if (newFig.Points.Count > 0)
                {
                    newFig.PointStart = newFig.Points[0];
                }

                newFig.groupFigures = fig.groupFigures;
                List<Figure> group = new List<Figure>(fig.groupFigures);
                foreach (Figure figGroup in group)
                {
                    int index = figGroup.groupFigures.IndexOf(fig);
                    figGroup.groupFigures.Insert(index, newFig);
                    figGroup.groupFigures.Remove(fig);
                }
                newFig.AddStarForSinglePoint(true);
                ListFigure.Insert(IndexFigure, newFig);
                ListFigure.Remove(fig);
            }
        }
    }
}