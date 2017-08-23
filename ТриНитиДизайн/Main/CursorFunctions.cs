using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ТриНитиДизайн
{
    public partial class MainWindow : Window
    {
        string status;
        List<Point> ptsRec = new List<Point>();
        Vector tVect;
        Point startVector;

        public void InitializeFigureRectangle(int length)
        {
            ptsRec.Clear();
            chRec = new Rectangle();
            Point a;
            Point b;
            Point c;
            Point d;
            ListFigure[IndexFigure].GetFourPointsOfOutSideRectangle(out a, out b, out c, out d, length);
            ptsRec.Add(a);
            ptsRec.Add(b);
            ptsRec.Add(c);
            ptsRec.Add(d);
            chRec.Height = Math.Abs(b.Y - a.Y);
            chRec.Width = Math.Abs(c.X - b.X);
            DoubleCollection dashes = new DoubleCollection();
            dashes.Add(2);
            dashes.Add(2);
            chRec.StrokeDashArray = dashes;
            Canvas.SetLeft(chRec, a.X);
            Canvas.SetTop(chRec, a.Y);
            chRec.Stroke = OptionColor.ColorChoosingRec;
            chRec.StrokeThickness = OptionDrawLine.StrokeThickness;

            firstRec = GeometryHelper.DrawRectangle(ListFigure[IndexFigure].PointStart, false, true,
                OptionDrawLine.StrokeThickness, OptionColor.ColorOpacity, MainCanvas);
            lastRec = GeometryHelper.DrawRectangle(ListFigure[IndexFigure].PointEnd, false, false,
                OptionDrawLine.StrokeThicknessMainRec, OptionColor.ColorOpacity, MainCanvas);
        }

        public void InitializeScaling(string newStatus)
        {
            status = newStatus;
            InitializeFigureRectangle(0);
            tVect = new Vector();
            startVector = new Point();
        }

        public void MoveScalingRectangle(Point currentPosition, Canvas canvas)
        {
            Vector vect = new Vector();
            Vector originalVector = new Vector();
            double scale;
            tVect = new Vector();
            if (status.Equals("north"))
            {
                chRec = DrawChoosingRectangle(new Point(ptsRec[0].X, currentPosition.Y + 10), ptsRec[2], canvas);
                vect = new Point(0, ptsRec[2].Y) - new Point(0, currentPosition.Y + 10);
                originalVector = ptsRec[0] - ptsRec[1];
            }
            else if (status.Equals("northeast"))
            {
                tVect = FindFigureRectangle(new Point(ptsRec[0].X, currentPosition.Y + 10), new Point(currentPosition.X - 10, ptsRec[1].Y), canvas);
                startVector = ptsRec[1];
            }
            else if (status.Equals("east"))
            {
                chRec = DrawChoosingRectangle(ptsRec[0], new Point(currentPosition.X - 10, ptsRec[1].Y), canvas);
                vect = new Point(currentPosition.X - 10, 0) - new Point(ptsRec[0].X, 0);
                originalVector = ptsRec[2] - ptsRec[1];
            }
            else if (status.Equals("southeast"))
            {
                tVect = FindFigureRectangle(ptsRec[0], new Point(currentPosition.X - 10, currentPosition.Y - 10), canvas);
                startVector = ptsRec[0];
            }
            else if (status.Equals("south"))
            {
                chRec = DrawChoosingRectangle(ptsRec[0], new Point(ptsRec[2].X, currentPosition.Y - 10), canvas);
                vect = new Point(0, currentPosition.Y - 10) - new Point(0, ptsRec[0].Y);
                originalVector = ptsRec[1] - ptsRec[0];
            }
            else if (status.Equals("southwest"))
            {
                tVect = FindFigureRectangle(new Point(currentPosition.X + 10, ptsRec[0].Y), new Point(ptsRec[2].X, currentPosition.Y - 10), canvas);
                startVector = ptsRec[3];
            }
            else if (status.Equals("west"))
            {
                chRec = DrawChoosingRectangle(new Point(currentPosition.X + 10, ptsRec[0].Y), ptsRec[2], canvas);
                vect = new Point(ptsRec[2].X, 0) - new Point(currentPosition.X + 10, 0);
                originalVector = ptsRec[1] - ptsRec[2];
            }
            else if (status.Equals("northwest"))
            {
                tVect = FindFigureRectangle(new Point(currentPosition.X + 10, currentPosition.Y + 10), ptsRec[2], canvas);
                startVector = ptsRec[2];
            }
            if(status.Equals("north") || status.Equals("south") || status.Equals("west") || status.Equals("east"))
            {
                scale = vect.Length / originalVector.Length;
                if (vect.X < 0 || vect.Y < 0)
                    scale = -scale;

                if (vect.Y == 0)
                    tVect = new Vector(scale, 1);
                else
                    tVect = new Vector(1, scale);
            }
            ScaleRectangles(firstRec, ListFigure[IndexFigure].PointStart, startVector, tVect, canvas);
            ScaleRectangles(lastRec, ListFigure[IndexFigure].PointEnd, startVector, tVect, canvas);
        }

        private void ScaleRectangles(Rectangle rec,Point origin, Point startVectorPoint, Vector transformVect,Canvas canvas)
        {
            Point p = GetScaledPoint(origin, startVectorPoint, transformVect);
            canvas.Children.Remove(rec);
            Canvas.SetLeft(rec, p.X - rec.Width / 2);
            Canvas.SetTop(rec, p.Y - rec.Width / 2);
            canvas.Children.Add(rec);
        }

        public void ScaleTransformFigure()
        {
            chRec = new Rectangle();
            for (int i = 0; i < ListFigure[IndexFigure].Points.Count; i++)
            {
                Point p = ListFigure[IndexFigure].Points[i];
                Point scaledP = GetScaledPoint(p, startVector, tVect);
                if (i != ListFigure[IndexFigure].Points.Count - 1)
                {
                    Point nextP = ListFigure[IndexFigure].Points[i + 1];
                    Point scaledNextP = GetScaledPoint(nextP, startVector, tVect);
                    Shape sh;
                    Shape newSh;
                    Tuple<Point, Point> contPts = new Tuple<Point, Point>(new Point(), new Point());
                    ListFigure[IndexFigure].DictionaryPointLines.TryGetValue(p, out sh);
                    if (sh is Line)
                        newSh = GeometryHelper.SetLine(OptionColor.ColorDraw, scaledP, scaledNextP, false, MainCanvas);
                    else
                    {
                        ListFigure[IndexFigure].DictionaryShapeControlPoints.TryGetValue(p, out contPts);
                        if (sh.MinHeight == 5)
                        {
                            Point scaledContPoint1 = GetScaledPoint(contPts.Item1, startVector, tVect);
                            Point scaledContPoint2 = GetScaledPoint(contPts.Item2, startVector, tVect);
                            contPts = new Tuple<Point, Point>(scaledContPoint1, scaledContPoint2);
                            newSh = GeometryHelper.SetBezier(OptionColor.ColorDraw, scaledP, contPts.Item1,
                                contPts.Item2, scaledNextP, MainCanvas);
                        }
                        else
                        {
                            Point scaledContPoint = GetScaledPoint(contPts.Item1, startVector, tVect);
                            contPts = new Tuple<Point, Point>(scaledContPoint, new Point());
                            newSh = GeometryHelper.SetArc(OptionColor.ColorDraw, scaledP, scaledNextP,
                                contPts.Item1, MainCanvas);
                        }
                    }
                    ListFigure[IndexFigure].DeleteShape(sh, p, MainCanvas);
                    ListFigure[IndexFigure].AddShape(newSh, scaledP, contPts);
                }
                ListFigure[IndexFigure].Points[i] = scaledP;
            }
            ListFigure[IndexFigure].PointStart = ListFigure[IndexFigure].Points[0];
            ListFigure[IndexFigure].PointEnd = ListFigure[IndexFigure].Points[ListFigure[IndexFigure].Points.Count - 1];
        }

        private Point GetScaledPoint(Point origin, Point startVectorPoint, Vector transformVect)
        {
            Vector vect;
            Point scaledPoint = new Point();
            if (status.Equals("north"))
                startVectorPoint = new Point(origin.X, ptsRec[1].Y);
            else if (status.Equals("west"))
                startVectorPoint = new Point(ptsRec[2].X, origin.Y);
            else if (status.Equals("south"))
                startVectorPoint = new Point(origin.X, ptsRec[0].Y);
            else if (status.Equals("east"))
                startVectorPoint = new Point(ptsRec[1].X, origin.Y);
            vect = origin - startVectorPoint;
            double offsetX = vect.X * transformVect.X;
            double offsetY = vect.Y * transformVect.Y;
            scaledPoint.X = startVectorPoint.X + offsetX;
            scaledPoint.Y = startVectorPoint.Y + offsetY;
            return scaledPoint;
        }

        public void MoveFigureRectangle(Rectangle rec, Vector delta, Canvas canvas)
        {
            canvas.Children.Remove(rec);
            Point p = new Point();
            p.X = Canvas.GetLeft(rec);
            p.Y = Canvas.GetTop(rec);
            p += delta;
            Canvas.SetLeft(rec, p.X);
            Canvas.SetTop(rec, p.Y);
            canvas.Children.Add(rec);
        }

        public void MoveFigureToNewPosition()
        {
            chRec = new Rectangle();
            Point newPointStart = new Point(Canvas.GetLeft(firstRec) + firstRec.Width / 2,
                Canvas.GetTop(firstRec) + firstRec.Height / 2);
            Vector figureVect = newPointStart - ListFigure[IndexFigure].PointStart;
            for(int i = 0; i < ListFigure[IndexFigure].Points.Count; i++)
            {
                Point p = ListFigure[IndexFigure].Points[i];
                if (i != ListFigure[IndexFigure].Points.Count - 1)
                {
                    Point nextP = ListFigure[IndexFigure].Points[i + 1];
                    Shape sh;
                    Shape newSh;
                    Tuple<Point,Point> contPts = new Tuple<Point,Point>(new Point(),new Point());
                    ListFigure[IndexFigure].DictionaryPointLines.TryGetValue(p, out sh);
                    if(sh is Line)
                        newSh = GeometryHelper.SetLine(OptionColor.ColorDraw, p + figureVect, nextP + figureVect, false, MainCanvas);
                    else
                    {
                        ListFigure[IndexFigure].DictionaryShapeControlPoints.TryGetValue(p, out contPts);
                        if(sh.MinHeight == 5)
                        {
                            contPts = new Tuple<Point, Point>(contPts.Item1 + figureVect, contPts.Item2 + figureVect);
                            newSh = GeometryHelper.SetBezier(OptionColor.ColorDraw, p + figureVect, contPts.Item1,
                                contPts.Item2,nextP + figureVect, MainCanvas);
                        }
                        else
                        {
                            contPts = new Tuple<Point, Point>(contPts.Item1 + figureVect, new Point());
                            newSh = GeometryHelper.SetArc(OptionColor.ColorDraw, p + figureVect, nextP + figureVect,
                                contPts.Item1, MainCanvas);
                        }
                    }
                    ListFigure[IndexFigure].DeleteShape(sh, p, MainCanvas);
                    ListFigure[IndexFigure].AddShape(newSh, p + figureVect, contPts);
                }
                ListFigure[IndexFigure].Points[i] += figureVect;
            }
            ListFigure[IndexFigure].PointStart += figureVect;
            ListFigure[IndexFigure].PointEnd += figureVect;
        }

        public void CursorMenuDrawInColor()
        {
            TempListFigure = ListFigure.ToList<Figure>();
            MainCanvas.Children.Clear();
            ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorNewDraw, false);
            MainCanvas.Background = OptionColor.ColorNewBackground;
            ListFigure[IndexFigure].AddFigure(MainCanvas);
        }

        public void CursorMenuDrawStegki()
        {
            ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorKrivaya, false);
            MainCanvas.Children.Remove(lastRec);
            MainCanvas.Children.Remove(firstRec);
        }

        private Vector FindFigureRectangle(Point p1, Point p2, Canvas canvas)
        {
            Point b = new Point(p1.X, p2.Y);
            Point d = new Point(p2.X, p1.Y);
            double originalHeight = FindLength(ptsRec[0], ptsRec[1]);
            double originalWidth = FindLength(ptsRec[1], ptsRec[2]);
            double newHeight = FindLength(p1, b);
            double newWidth = FindLength(b, p2);
            double scale;
            if (newHeight / originalHeight > newWidth / originalWidth)
                scale = newWidth / originalWidth;
            else
                scale = newHeight / originalHeight;
            Vector vect;
            if (status.Equals("southeast"))
            {
                vect = ptsRec[2] - ptsRec[0];
                vect *= scale;
                vect = InvertVector(p1, p2, vect);
                chRec = DrawChoosingRectangle(ptsRec[0], new Point(ptsRec[0].X + vect.X, ptsRec[0].Y + vect.Y), canvas);
            }
            else if(status.Equals("southwest"))
            {
                vect = ptsRec[1] - ptsRec[3];
                vect *= scale;
                vect = InvertVector(p1, p2, vect);
                chRec = DrawChoosingRectangle(new Point(ptsRec[3].X + vect.X, ptsRec[0].Y),
                    new Point(ptsRec[2].X, ptsRec[3].Y + vect.Y), canvas);
            }
            else if (status.Equals("northwest"))
            {
                vect = ptsRec[0] - ptsRec[2];
                vect *= scale;
                vect = InvertVector(p1, p2, vect);
                chRec = DrawChoosingRectangle(new Point(ptsRec[2].X + vect.X, ptsRec[2].Y + vect.Y), ptsRec[2], canvas);
            }
            else if (status.Equals("northeast"))
            {
                vect = ptsRec[3] - ptsRec[1];
                vect *= scale;
                vect = InvertVector(p1, p2, vect);
                chRec = DrawChoosingRectangle(new Point(ptsRec[0].X, ptsRec[1].Y + vect.Y), 
                    new Point(ptsRec[1].X + vect.X, ptsRec[1].Y), canvas);
            }
            Vector scaleVector = new Vector(scale, scale);
            scaleVector = InvertVector(p1, p2, scaleVector);
            return scaleVector;
        }

        private Vector InvertVector(Point p1, Point p2, Vector vect)
        {
            if (p1.X > p2.X)
                vect.X = -vect.X;
            if (p1.Y > p2.Y)
                vect.Y = -vect.Y;
            return vect;
        }
    }
}
