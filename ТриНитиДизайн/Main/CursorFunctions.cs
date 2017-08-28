using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ТриНитиДизайн
{
    public partial class MainWindow : Window
    {
        string status;
        List<Point> ptsRec = new List<Point>();
        Vector tVect;
        Point startVector;
        double angle;

        public void InitializeFigureRectangle(int length)
        {
            ptsRec.Clear();
            chRec = new Rectangle();
            chRec.MaxHeight = 100000;
            ptsRec = GetFourOutsidePointsForGroup(0);
            chRec.Height = Math.Abs(ptsRec[1].Y - ptsRec[0].Y);
            chRec.Width = Math.Abs(ptsRec[2].X - ptsRec[1].X);
            DoubleCollection dashes = new DoubleCollection();
            dashes.Add(2);
            dashes.Add(2);
            chRec.StrokeDashArray = dashes;
            Canvas.SetLeft(chRec, ptsRec[0].X);
            Canvas.SetTop(chRec, ptsRec[0].Y);
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

        public void RotateRotatingRectangle(Point currentPos,Point centerPoint, Point firstPos, Canvas canvas)
        {
            Vector vect1 = new Vector(currentPos.X - centerPoint.X, currentPos.Y - centerPoint.Y);
            Vector vect2 = new Vector(firstPos.X - centerPoint.X, firstPos.Y - centerPoint.Y);
            angle = - Vector.AngleBetween(vect1, vect2);

            canvas.Children.Remove(chRec);
            Vector originalVect = ptsRec[2] - ptsRec[0];
            Vector newVect = centerPoint - ptsRec[0];
            double scaleX = newVect.X / originalVect.X;
            double scaleY = newVect.Y / originalVect.Y;
            chRec.RenderTransformOrigin = new Point(scaleX, scaleY);
            chRec.RenderTransform = new RotateTransform(angle);
            canvas.Children.Add(chRec);

            angle = angle * (Math.PI / 180);
            Point firstRecPos = RotatePoint(angle, ListFigure[IndexFigure].PointStart, centerPoint);
            canvas.Children.Remove(firstRec);
            Canvas.SetLeft(firstRec, firstRecPos.X - firstRec.Width/2);
            Canvas.SetTop(firstRec, firstRecPos.Y - firstRec.Width / 2);
            canvas.Children.Add(firstRec);

            Point secondRecPos = RotatePoint(angle, ListFigure[IndexFigure].PointEnd, centerPoint);
            canvas.Children.Remove(lastRec);
            Canvas.SetLeft(lastRec, secondRecPos.X - lastRec.Width / 2);
            Canvas.SetTop(lastRec, secondRecPos.Y - lastRec.Width / 2);
            canvas.Children.Add(lastRec);
        }

        private Point RotatePoint(double newAngle, Point origin, Point centerPoint)
        {
            double s = Math.Sin(angle);
            double c = Math.Cos(angle);

            Point p = origin;
            p.X -= centerPoint.X;
            p.Y -= centerPoint.Y;

            double xnew = p.X * c - p.Y * s;
            double ynew = p.X * s + p.Y * c;

            p.X = xnew + centerPoint.X;
            p.Y = ynew + centerPoint.Y;
            return p;
        }

        public void RotateFigure(Point centerPoint)
        {
            chRec = new Rectangle();
            foreach (Figure fig in ListFigure[IndexFigure].groupFigures)
            {
                for (int i = 0; i < fig.Points.Count; i++)
                {
                    Point p = fig.Points[i];
                    Point rotatedP = RotatePoint(angle, p, centerPoint);
                    if (i != fig.Points.Count - 1)
                    {
                        Point nextP = fig.Points[i + 1];
                        Point rotatedNextP = RotatePoint(angle, nextP, centerPoint);
                        Shape sh;
                        Shape newSh;
                        Tuple<Point, Point> contPts = new Tuple<Point, Point>(new Point(), new Point());
                        fig.DictionaryPointLines.TryGetValue(p, out sh);
                        if (sh is Line)
                            newSh = GeometryHelper.SetLine(OptionColor.ColorDraw, rotatedP, rotatedNextP, false, MainCanvas);
                        else
                        {
                            fig.DictionaryShapeControlPoints.TryGetValue(p, out contPts);
                            if (sh.MinHeight == 5)
                            {
                                Point rotatedContPoint1 = RotatePoint(angle, contPts.Item1, centerPoint);
                                Point rotatedContPoint2 = RotatePoint(angle, contPts.Item2, centerPoint);
                                contPts = new Tuple<Point, Point>(rotatedContPoint1, rotatedContPoint2);
                                newSh = GeometryHelper.SetBezier(OptionColor.ColorDraw, rotatedP, contPts.Item1,
                                    contPts.Item2, rotatedNextP, MainCanvas);
                            }
                            else
                            {
                                Point rotatedContPoint = RotatePoint(angle, contPts.Item1, centerPoint);
                                contPts = new Tuple<Point, Point>(rotatedContPoint, new Point());
                                newSh = GeometryHelper.SetArc(OptionColor.ColorDraw, rotatedP, rotatedNextP,
                                    contPts.Item1, MainCanvas);
                            }
                        }
                        fig.DeleteShape(sh, p, MainCanvas);
                        fig.AddShape(newSh, rotatedP, contPts);
                    }
                    fig.Points[i] = rotatedP;
                }
                fig.PointStart = fig.Points[0];
                fig.PointEnd = fig.Points[fig.Points.Count - 1];
            }
        }

        public void MoveScalingRectangle(Point currentPosition, Canvas canvas)
        {
            SetScalingCursor();
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

        private void SetScalingCursor()
        {
            if (status.Equals("north") || status.Equals("south"))
                MainCanvas.Cursor = Cursors.SizeNS;
            else if (status.Equals("east") || status.Equals("west"))
                MainCanvas.Cursor = Cursors.SizeWE;
            else
                MainCanvas.Cursor = Cursors.SizeAll;
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
            foreach (Figure fig in ListFigure[IndexFigure].groupFigures)
            {
                for (int i = 0; i < fig.Points.Count; i++)
                {
                    Point p = fig.Points[i];
                    Point scaledP = GetScaledPoint(p, startVector, tVect);
                    if (i != fig.Points.Count - 1)
                    {
                        Point nextP = fig.Points[i + 1];
                        Point scaledNextP = GetScaledPoint(nextP, startVector, tVect);
                        Shape sh;
                        Shape newSh;
                        Tuple<Point, Point> contPts = new Tuple<Point, Point>(new Point(), new Point());
                        fig.DictionaryPointLines.TryGetValue(p, out sh);
                        if (sh is Line)
                            newSh = GeometryHelper.SetLine(OptionColor.ColorDraw, scaledP, scaledNextP, false, MainCanvas);
                        else
                        {
                            fig.DictionaryShapeControlPoints.TryGetValue(p, out contPts);
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
                        fig.DeleteShape(sh, p, MainCanvas);
                        fig.AddShape(newSh, scaledP, contPts);
                    }
                    fig.Points[i] = scaledP;
                }
                fig.PointStart = fig.Points[0];
                fig.PointEnd = fig.Points[fig.Points.Count - 1];
            }
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
            chRec.MaxHeight = 99999;
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
            Point newPointStart = new Point(Canvas.GetLeft(chRec), Canvas.GetTop(chRec));
            Vector figureVect = newPointStart - ptsRec[0];
            foreach (Figure fig in ListFigure[IndexFigure].groupFigures)
            {
                for (int i = 0; i < fig.Points.Count; i++)
                {
                    Point p = fig.Points[i];
                    if (i != fig.Points.Count - 1)
                    {
                        Point nextP = fig.Points[i + 1];
                        Shape sh;
                        Shape newSh;
                        Tuple<Point, Point> contPts = new Tuple<Point, Point>(new Point(), new Point());
                        fig.DictionaryPointLines.TryGetValue(p, out sh);
                        if (sh is Line)
                            newSh = GeometryHelper.SetLine(OptionColor.ColorDraw, p + figureVect, nextP + figureVect, false, MainCanvas);
                        else
                        {
                            fig.DictionaryShapeControlPoints.TryGetValue(p, out contPts);
                            if (sh.MinHeight == 5)
                            {
                                contPts = new Tuple<Point, Point>(contPts.Item1 + figureVect, contPts.Item2 + figureVect);
                                newSh = GeometryHelper.SetBezier(OptionColor.ColorDraw, p + figureVect, contPts.Item1,
                                    contPts.Item2, nextP + figureVect, MainCanvas);
                            }
                            else
                            {
                                contPts = new Tuple<Point, Point>(contPts.Item1 + figureVect, new Point());
                                newSh = GeometryHelper.SetArc(OptionColor.ColorDraw, p + figureVect, nextP + figureVect,
                                    contPts.Item1, MainCanvas);
                            }
                        }
                        fig.DeleteShape(sh, p, MainCanvas);
                        fig.AddShape(newSh, p + figureVect, contPts);
                    }
                    fig.Points[i] += figureVect;
                }
                fig.PointStart += figureVect;
                fig.PointEnd += figureVect;
            }
        }

        public void CursorMenuDrawInColor()
        {
            TempListFigure = ListFigure.ToList<Figure>();
            MainCanvas.Children.Clear();
            foreach (Figure fig in ListFigure[IndexFigure].groupFigures)
            {
                fig.ChangeFigureColor(OptionColor.ColorNewDraw, false);
                fig.AddFigure(MainCanvas);
            }
            MainCanvas.Background = OptionColor.ColorNewBackground;
        }

        public void CursorMenuDrawStegki()
        {
            foreach (Figure fig in ListFigure[IndexFigure].groupFigures)
                fig.ChangeFigureColor(OptionColor.ColorKrivaya, false);
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

        public void DrawOutsideRectangles(bool isScale, bool rememberLastRect, Canvas canvas)
        {
            Point lastPoint = new Point();
            if (rememberLastRect)
                lastPoint = new Point(Canvas.GetLeft(transRectangles[8]) + transRectangles[8].Height / 2,
                    Canvas.GetTop(transRectangles[8]) + transRectangles[8].Height / 2);
            transRectangles = new List<Rectangle>();
            List<Point> PointsOutSideRectangle = new List<Point>();
            Point a, b, c, d;
            List<Point> pts = GetFourOutsidePointsForGroup(10);
            a = pts[0];
            b = pts[1];
            c = pts[2];
            d = pts[3];
            PointsOutSideRectangle.Add(new Point((d.X + a.X) / 2, (d.Y + a.Y) / 2));
            PointsOutSideRectangle.Add(d);
            PointsOutSideRectangle.Add(new Point((c.X + d.X) / 2, (c.Y + d.Y) / 2));
            PointsOutSideRectangle.Add(c);
            PointsOutSideRectangle.Add(new Point((b.X + c.X) / 2, (b.Y + c.Y) / 2));
            PointsOutSideRectangle.Add(b);
            PointsOutSideRectangle.Add(new Point((a.X + b.X) / 2, (b.Y + a.Y) / 2));
            PointsOutSideRectangle.Add(a);
            double size = OptionDrawLine.SizeRectangleForRotation;
            if (rememberLastRect)
                PointsOutSideRectangle.Add(lastPoint);
            else if (!isScale)
                PointsOutSideRectangle.Add(GetCenterForGroup(pts));
            else
                size = OptionDrawLine.SizeRectangleForScale;
            foreach (Point p in PointsOutSideRectangle)
            {
                transRectangles.Add(GeometryHelper.DrawTransformingRectangle(p, size, canvas));
            }
            if (!isScale)
            {
                for (int i = 0; i < transRectangles.Count; i++)
                {
                    int index;
                    if (i == 0 || i == 4)
                        index = 0;
                    else if (i == 2 || i == 6)
                        index = 10;
                    else if (i == 8)
                        index = 11;
                    else
                        index = i * (i % 2);
                    ImageBrush image = new ImageBrush(new BitmapImage(
    new Uri(@"pack://application:,,,/Images/arrow" + index + ".gif", UriKind.Absolute)));
                    transRectangles[i].Fill = image;
                    transRectangles[i].StrokeThickness = 0;
                }
            }
            foreach (Figure fig in ListFigure[IndexFigure].groupFigures)
                fig.ChangeFigureColor(OptionColor.ColorDraw, false);
        }

        private List<Point> GetFourOutsidePointsForGroup(int length)
        {
            List<Point> pts = new List<Point>();

            foreach (Figure fig in ListFigure[IndexFigure].groupFigures)
                foreach (Point p in fig.GetFourPointsOfOutSideRectangle(length))
                    pts.Add(p);

            Point max = new Point(Double.MinValue, Double.MinValue);
            Point min = new Point(Double.MaxValue, Double.MaxValue);

            foreach (Point p in pts)
            {
                if (p.X > max.X)
                    max.X = p.X;
                if (p.Y > max.Y)
                    max.Y = p.Y;
                if (p.X < min.X)
                    min.X = p.X;
                if (p.Y < min.Y)
                    min.Y = p.Y;
            }
            pts.Clear();
            pts.Add(new Point(min.X, min.Y));
            pts.Add(new Point(min.X, max.Y));
            pts.Add(new Point(max.X, max.Y));
            pts.Add(new Point(max.X, min.Y));
            return pts;
        }

        private Point GetCenterForGroup(List<Point> pts)
        {
            return new Point((pts[2].X + pts[0].X) / 2, (pts[2].Y + pts[0].Y) / 2);
        }

        public void ShowJoinCursorMessage(Figure firstFigure, Figure secondFigure,Canvas canvas)
        {
            var JoinCursorWindow = new View.JoinCursor();
            JoinCursorWindow.ShowDialog();
            secondFigure.ChangeFigureColor(OptionColor.ColorSelection, false);

            if (OptionRegim.regim == Regim.RegimCursorJoinChain)
                JoinChain(firstFigure, secondFigure);

            else if (OptionRegim.regim == Regim.RegimCursorJoinTransposition)
                JoinTransposition(firstFigure, secondFigure);

            else if (OptionRegim.regim == Regim.RegimCursorJoinShiftDots)
                JoinShiftDots(firstFigure, secondFigure);

            else if (OptionRegim.regim == Regim.RegimCursorJoinShiftElements)
                JoinShiftElements(firstFigure, secondFigure);

            OptionRegim.regim = Regim.RegimCursor;
            RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            DrawFirstAndLastRectangle();
            DrawOutsideRectangles(true, false, canvas);
        }

        public void JoinChain(Figure firstFigure, Figure secondFigure)
        {
            
        }

        public void JoinTransposition(Figure firstFigure, Figure secondFigure)
        {
            List<Figure> group = new List<Figure>(firstFigure.groupFigures);
            foreach (Figure fig in group)
                if(fig != secondFigure)
                    fig.groupFigures.Add(secondFigure);
            foreach (Figure fig in group)
                if (fig != secondFigure)
                    secondFigure.groupFigures.Add(fig);
            secondFigure.ChangeFigureColor(OptionColor.ColorDraw, false);
        }

        public void JoinShiftDots(Figure firstFigure, Figure secondFigure)
        {
            secondFigure.ChangeFigureColor(OptionColor.ColorChoosingRec, false);
        }

        public void JoinShiftElements(Figure firstFigure, Figure secondFigure)
        {
            secondFigure.ChangeFigureColor(OptionColor.ColorDraw, false);
        }
    }
}
