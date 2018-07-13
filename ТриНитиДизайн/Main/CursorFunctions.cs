﻿using System;
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
    //    string status;
    //    List<Point> ptsRec = new List<Point>();
    //    List<Rectangle> movingFigurePoints = new List<Rectangle>();
    //    Vector tVect;
    //    Point startVector;
    //    double angle;

    //    public void InitializeFigureRectangle(int length)
    //    {
    //        ptsRec.Clear();
    //        chRec = new Rectangle();
    //        chRec.MaxHeight = 100000;
    //        ptsRec = GeometryHelper.GetFourOutsidePointsForGroup(listFigure[indexFigure].groupFigures, 0);
    //        chRec.Height = Math.Abs(ptsRec[1].Y - ptsRec[0].Y);
    //        chRec.Width = Math.Abs(ptsRec[2].X - ptsRec[1].X);
    //        DoubleCollection dashes = new DoubleCollection();
    //        dashes.Add(2);
    //        dashes.Add(2);
    //        chRec.StrokeDashArray = dashes;
    //        Canvas.SetLeft(chRec, ptsRec[0].X);
    //        Canvas.SetTop(chRec, ptsRec[0].Y);
    //        chRec.Stroke = OptionColor.colorArc;
    //        chRec.StrokeThickness = OptionDrawLine.strokeThickness;
    //        movingFigurePoints.Clear();
    //        foreach (Figure fig in listFigure[indexFigure].groupFigures)
    //        {
    //            movingFigurePoints.Add(GeometryHelper.DrawRectangle(fig.pointStart, false, true,
    //                OptionDrawLine.strokeThickness, OptionColor.colorOpacity, mainCanvas));
    //            movingFigurePoints.Add(GeometryHelper.DrawRectangle(fig.pointEnd, false, false,
    //                OptionDrawLine.strokeThickness, OptionColor.colorOpacity, mainCanvas));
    //        }
    //        foreach (Rectangle rec in movingFigurePoints)
    //            mainCanvas.Children.Remove(rec);
    //    }

    //    public void InitializeScaling(string newStatus)
    //    {
    //        status = newStatus;
    //        InitializeFigureRectangle(0);
    //        tVect = new Vector();
    //        startVector = new Point();
    //    }

    //    public void RotateRotatingRectangle(Point currentPos,Point centerPoint, Point firstPos, Canvas canvas)
    //    {
    //        Vector vect1 = new Vector(currentPos.X - centerPoint.X, currentPos.Y - centerPoint.Y);
    //        Vector vect2 = new Vector(firstPos.X - centerPoint.X, firstPos.Y - centerPoint.Y);
    //        angle = - Vector.AngleBetween(vect1, vect2);

    //        canvas.Children.Remove(chRec);
    //        Vector originalVect = ptsRec[2] - ptsRec[0];
    //        Vector newVect = centerPoint - ptsRec[0];
    //        double scaleX = newVect.X / originalVect.X;
    //        double scaleY = newVect.Y / originalVect.Y;
    //        chRec.RenderTransformOrigin = new Point(scaleX, scaleY);
    //        chRec.RenderTransform = new RotateTransform(angle);
    //        canvas.Children.Add(chRec);
    //        statusbar3.Content = "Угол поворота = " + Math.Round(-angle,1);
    //        angle = angle * (Math.PI / 180);
            
    //        for(int i = 0; i < movingFigurePoints.Count; i ++)
    //        {
    //            Point startPoint;

    //            if(i%2 == 0)
    //                startPoint = listFigure[indexFigure].groupFigures[i/2].pointStart;
    //            else
    //                startPoint = listFigure[indexFigure].groupFigures[i/2].pointEnd;
    //            Point recPosition = RotatePoint(angle, startPoint, centerPoint);
    //            canvas.Children.Remove(movingFigurePoints[i]);
    //            Canvas.SetLeft(movingFigurePoints[i], recPosition.X - movingFigurePoints[i].Width / 2);
    //            Canvas.SetTop(movingFigurePoints[i], recPosition.Y - movingFigurePoints[i].Width / 2);
    //            canvas.Children.Add(movingFigurePoints[i]);
    //        }
    //    }

    //    private Point RotatePoint(double newAngle, Point origin, Point centerPoint)
    //    {
    //        double s = Math.Sin(angle);
    //        double c = Math.Cos(angle);

    //        Point p = origin;
    //        p.X -= centerPoint.X;
    //        p.Y -= centerPoint.Y;

    //        double xnew = p.X * c - p.Y * s;
    //        double ynew = p.X * s + p.Y * c;

    //        p.X = xnew + centerPoint.X;
    //        p.Y = ynew + centerPoint.Y;
    //        return p;
    //    }

    //    public void RotateFigure(Point centerPoint)
    //    {
    //        statusbar3.Content = "";
    //        chRec = new Rectangle();
    //        foreach (Rectangle rec in movingFigurePoints)
    //            mainCanvas.Children.Remove(rec);
    //        movingFigurePoints.Clear();
    //        foreach (Figure fig in listFigure[indexFigure].groupFigures)
    //        {
    //            if (fig.modeFigure == Mode.modeTatami)
    //            {
    //                Point p = new Point(fig.tatamiControlLine.X, fig.tatamiControlLine.Y);
    //                p = RotatePoint(angle, p, new Point(0, 0));
    //                fig.tatamiControlLine.X = p.X;
    //                fig.tatamiControlLine.Y = p.Y;
    //                fig.tatamiControlLine.Normalize();
    //                fig.oldTatamiCenter = RotatePoint(angle, fig.oldTatamiCenter, centerPoint);
    //            }
    //            else if (fig.modeFigure == Mode.modeSatin)
    //            {
    //                for (int i = 0; i < fig.oldSatinCenters.Count; i++)
    //                {
    //                    fig.oldSatinCenters[i] = RotatePoint(angle, fig.oldSatinCenters[i], centerPoint);
    //                }
    //                for (int i = 0; i < fig.satinControlLines.Count; i++)
    //                {
    //                    Point p = new Point(fig.satinControlLines[i].X, fig.satinControlLines[i].Y);
    //                    p = RotatePoint(angle, p, new Point(0, 0));
    //                    Vector vect = new Vector(p.X, p.Y);
    //                    vect.Normalize();
    //                    fig.satinControlLines[i] = vect;
    //                }
    //            }
    //            for (int i = 0; i < fig.points.Count; i++)
    //            {
    //                Point p = fig.points[i];
    //                Point rotatedP = RotatePoint(angle, p, centerPoint);
    //                if (i != fig.points.Count - 1)
    //                {
    //                    Point nextP = fig.points[i + 1];
    //                    Point rotatedNextP = RotatePoint(angle, nextP, centerPoint);
    //                    Shape sh;
    //                    Shape newSh;
    //                    Tuple<Point, Point> contPts = new Tuple<Point, Point>(new Point(), new Point());
    //                    fig.DictionaryPointLines.TryGetValue(p, out sh);
    //                    if (sh is Line)
    //                        newSh = GeometryHelper.SetLine(OptionColor.colorActive, rotatedP, rotatedNextP, false, mainCanvas);
    //                    else
    //                    {
    //                        fig.shapeControlPoints.TryGetValue(p, out contPts);
    //                        if (sh.MinHeight == 5)
    //                        {
    //                            Point rotatedContPoint1 = RotatePoint(angle, contPts.Item1, centerPoint);
    //                            Point rotatedContPoint2 = RotatePoint(angle, contPts.Item2, centerPoint);
    //                            contPts = new Tuple<Point, Point>(rotatedContPoint1, rotatedContPoint2);
    //                            newSh = GeometryHelper.SetBezier(OptionColor.colorActive, rotatedP, contPts.Item1,
    //                                contPts.Item2, rotatedNextP, mainCanvas);
    //                        }
    //                        else
    //                        {
    //                            Point rotatedContPoint = RotatePoint(angle, contPts.Item1, centerPoint);
    //                            contPts = new Tuple<Point, Point>(rotatedContPoint, new Point());
    //                            newSh = GeometryHelper.SetArc(OptionColor.colorActive, rotatedP, rotatedNextP,
    //                                contPts.Item1, mainCanvas);
    //                        }
    //                    }
    //                    fig.DeleteShape(sh, p, mainCanvas);
    //                    fig.AddShape(newSh, rotatedP, contPts);
    //                }
    //                if (fig.points.Count == 1)
    //                {
    //                    Shape sh;
    //                    Shape newSh;
    //                    Tuple<Point, Point> contPts = new Tuple<Point, Point>(new Point(), new Point());
    //                    fig.DictionaryPointLines.TryGetValue(p, out sh);
    //                    newSh = GeometryHelper.SetStarForSinglePoint(rotatedP, OptionColor.colorActive, mainCanvas);
    //                    fig.DeleteShape(sh, p, mainCanvas);
    //                    fig.AddShape(newSh, rotatedP, contPts);
    //                }
    //                fig.points[i] = rotatedP;
    //            }
    //            fig.pointStart = fig.points[0];
    //            fig.pointEnd = fig.points[fig.points.Count - 1];
    //        }
    //    }

    //    public void MoveScalingRectangle(Point currentPosition, Canvas canvas)
    //    {
    //        SetScalingCursor();
    //        Vector vect = new Vector();
    //        Vector originalVector = new Vector();
    //        double rectangleDistance = OptionDrawLine.cursorModeRectangleDistance;
    //        tVect = new Vector();
    //        if (status.Equals("north"))
    //        {
    //            vect = new Point(0, ptsRec[2].Y) - new Point(0, currentPosition.Y + rectangleDistance);
    //            originalVector = ptsRec[0] - ptsRec[1];
    //            tVect = GetVectorForOrthogonalScaling(vect, originalVector);
    //            originalVector *= tVect.Y;
    //            chRec = DrawChoosingRectangle(new Point(ptsRec[0].X, ptsRec[2].Y + originalVector.Y), ptsRec[2], canvas);
    //        }
    //        else if (status.Equals("northeast"))
    //        {
    //            tVect = FindFigureRectangle(new Point(ptsRec[0].X, currentPosition.Y + rectangleDistance), 
    //                new Point(currentPosition.X - rectangleDistance, ptsRec[1].Y), canvas);
    //            startVector = ptsRec[1];
    //        }
    //        else if (status.Equals("east"))
    //        {
    //            vect = new Point(currentPosition.X - rectangleDistance, 0) - new Point(ptsRec[0].X, 0);
    //            originalVector = ptsRec[2] - ptsRec[1];
    //            tVect = GetVectorForOrthogonalScaling(vect, originalVector);
    //            originalVector *= tVect.X;
    //            chRec = DrawChoosingRectangle(ptsRec[0], new Point(ptsRec[1].X + originalVector.X, ptsRec[1].Y), canvas);
    //        }
    //        else if (status.Equals("southeast"))
    //        {
    //            tVect = FindFigureRectangle(ptsRec[0], 
    //                new Point(currentPosition.X - rectangleDistance, currentPosition.Y - rectangleDistance), canvas);
    //            startVector = ptsRec[0];
    //        }
    //        else if (status.Equals("south"))
    //        {
    //            vect = new Point(0, currentPosition.Y - rectangleDistance) - new Point(0, ptsRec[0].Y);
    //            originalVector = ptsRec[1] - ptsRec[0];
    //            tVect = GetVectorForOrthogonalScaling(vect, originalVector);
    //            originalVector *= tVect.Y;
    //            chRec = DrawChoosingRectangle(ptsRec[0], new Point(ptsRec[2].X, ptsRec[0].Y + originalVector.Y), canvas);
    //        }
    //        else if (status.Equals("southwest"))
    //        {
    //            tVect = FindFigureRectangle(new Point(currentPosition.X + rectangleDistance, ptsRec[0].Y), 
    //                new Point(ptsRec[2].X, currentPosition.Y - rectangleDistance), canvas);
    //            startVector = ptsRec[3];
    //        }
    //        else if (status.Equals("west"))
    //        {
    //            vect = new Point(ptsRec[2].X, 0) - new Point(currentPosition.X + rectangleDistance, 0);
    //            originalVector = ptsRec[1] - ptsRec[2];
    //            tVect = GetVectorForOrthogonalScaling(vect, originalVector);
    //            originalVector *= tVect.X;
    //            chRec = DrawChoosingRectangle(new Point(ptsRec[2].X + originalVector.X, ptsRec[0].Y), ptsRec[2], canvas);
    //        }
    //        else if (status.Equals("northwest"))
    //        {
    //            tVect = FindFigureRectangle(new Point(currentPosition.X + rectangleDistance, currentPosition.Y + rectangleDistance), 
    //                ptsRec[2], canvas);
    //            startVector = ptsRec[2];
    //        }
    //        for (int i = 0; i < movingFigurePoints.Count; i++)
    //        {
    //            Point startPoint;
    //            if (i % 2 == 0)
    //                startPoint = listFigure[indexFigure].groupFigures[i / 2].pointStart;
    //            else
    //                startPoint = listFigure[indexFigure].groupFigures[i / 2].pointEnd;
    //            ScaleRectangles(movingFigurePoints[i], startPoint, startVector, tVect, canvas);
    //        }
    //    }

    //    public void MoveScalingRectangleRelativeToCenter(Point currentPosition, Canvas canvas)
    //    {
    //        SetScalingCursor();
    //        Vector vect = new Vector();
    //        Vector originalVector = new Vector();
    //        tVect = new Vector();
    //        Point centerPoint = GetCenterForGroup(ptsRec);
    //        startVector = centerPoint;
    //        double rectangleDistance = OptionDrawLine.cursorModeRectangleDistance;
    //        if (status.Equals("north"))
    //        {
    //            vect = centerPoint - new Point(centerPoint.X, currentPosition.Y + rectangleDistance);
    //            originalVector = new Point(centerPoint.X,ptsRec[0].Y) - centerPoint;
    //            tVect = GetVectorForOrthogonalScaling(vect, originalVector);                
    //        }
    //        else if (status.Equals("northeast"))
    //        {
    //            Point p1 = new Point(centerPoint.X, currentPosition.Y + rectangleDistance);
    //            Point p2 = new Point(currentPosition.X - rectangleDistance, centerPoint.Y);
    //            tVect = GetVectorForNonOrthogonalScaling(p1, p2);
    //            tVect = InvertVector(p1, p2, tVect);
    //        }
    //        else if (status.Equals("east"))
    //        {
    //            vect =  new Point(currentPosition.X - rectangleDistance, centerPoint.Y) - centerPoint;
    //            originalVector = new Point(ptsRec[2].X, centerPoint.Y) - centerPoint;
    //            tVect = GetVectorForOrthogonalScaling(vect, originalVector);
    //        }
    //        else if (status.Equals("southeast"))
    //        {
    //            Point p1 = centerPoint;
    //            Point p2 = new Point(currentPosition.X - rectangleDistance, currentPosition.Y - rectangleDistance);
    //            tVect = GetVectorForNonOrthogonalScaling(p1, p2);
    //            tVect = InvertVector(p1, p2, tVect);
    //        }
    //        else if (status.Equals("south"))
    //        {
    //            vect =  new Point(centerPoint.X, currentPosition.Y - rectangleDistance) - centerPoint;
    //            originalVector = new Point(centerPoint.X, ptsRec[1].Y) - centerPoint;
    //            tVect = GetVectorForOrthogonalScaling(vect, originalVector);
    //        }
    //        else if (status.Equals("southwest"))
    //        {
    //            Point p1 = new Point(currentPosition.X + rectangleDistance, centerPoint.Y);
    //            Point p2 = new Point(centerPoint.X, currentPosition.Y - rectangleDistance);
    //            tVect = GetVectorForNonOrthogonalScaling(p1, p2);
    //            tVect = InvertVector(p1, p2, tVect);
    //        }
    //        else if (status.Equals("west"))
    //        {
    //            vect = centerPoint - new Point(currentPosition.X + rectangleDistance, centerPoint.Y);
    //            originalVector = new Point(ptsRec[0].X, centerPoint.Y) - centerPoint;
    //            tVect = GetVectorForOrthogonalScaling(vect, originalVector);
    //        }
    //        else if (status.Equals("northwest"))
    //        {
    //            Point p1 = new Point(currentPosition.X + rectangleDistance, currentPosition.Y + rectangleDistance);
    //            Point p2 = centerPoint;
    //            tVect = GetVectorForNonOrthogonalScaling(p1, p2);
    //            tVect = InvertVector(p1, p2, tVect);
    //        }
    //        Point firstRectanglePoint = GetScaledPoint(ptsRec[0], centerPoint, tVect, false);
    //        Point secondRectanglePoint = GetScaledPoint(ptsRec[2], centerPoint, tVect, false);
    //        chRec = DrawChoosingRectangle(firstRectanglePoint, secondRectanglePoint, canvas);
    //        for (int i = 0; i < movingFigurePoints.Count; i++)
    //        {
    //            Point startPoint;
    //            if (i % 2 == 0)
    //                startPoint = listFigure[indexFigure].groupFigures[i / 2].pointStart;
    //            else
    //                startPoint = listFigure[indexFigure].groupFigures[i / 2].pointEnd;
    //            ScaleRectangles(movingFigurePoints[i], startPoint, startVector, tVect, canvas);
    //        }
    //    }

    //    private Vector GetVectorForOrthogonalScaling(Vector vect, Vector originalVector)
    //    {
    //        double scale = vect.Length / originalVector.Length;
    //        Vector tVect;
    //        if (Keyboard.IsKeyDown(Key.LeftCtrl))
    //            scale = Math.Round(scale + 0.5);
    //        if (vect.X < 0 || vect.Y < 0)
    //            scale = -scale;
    //        if (vect.Y == 0)
    //            tVect = new Vector(scale, 1);
    //        else
    //            tVect = new Vector(1, scale);
    //        return tVect;
    //    }

    //    private Vector GetVectorForNonOrthogonalScaling(Point p1, Point p2)
    //    {
    //        Vector tVect;
    //        double scale;
    //        Point b = new Point(p1.X, p2.Y);
    //        Point d = new Point(p2.X, p1.Y);
    //        double originalHeight = FindLength(ptsRec[0], ptsRec[1]);
    //        double originalWidth = FindLength(ptsRec[1], ptsRec[2]);
    //        if (Keyboard.IsKeyDown(Key.LeftShift))
    //        {
    //            originalHeight /= 2;
    //            originalWidth /= 2;
    //        }
    //        double newHeight = FindLength(p1, b);
    //        double newWidth = FindLength(b, p2);
    //        if (newHeight / originalHeight > newWidth / originalWidth)
    //            scale = newWidth / originalWidth;
    //        else
    //            scale = newHeight / originalHeight;
    //        if (Keyboard.IsKeyDown(Key.LeftCtrl))
    //            scale = Math.Round(scale + 0.5);
    //        tVect = new Vector(scale, scale);
    //        return tVect;
    //    }

    //    private void SetScalingCursor()
    //    {
    //        if (status.Equals("north") || status.Equals("south"))
    //            mainCanvas.Cursor = Cursors.SizeNS;
    //        else if (status.Equals("east") || status.Equals("west"))
    //            mainCanvas.Cursor = Cursors.SizeWE;
    //        else
    //            mainCanvas.Cursor = Cursors.SizeAll;
    //    }

    //    private void ScaleRectangles(Rectangle rec,Point origin, Point startVectorPoint, Vector transformVect,Canvas canvas)
    //    {
    //        Point p = GetScaledPoint(origin, startVectorPoint, transformVect, false);
    //        canvas.Children.Remove(rec);
    //        Canvas.SetLeft(rec, p.X - rec.Width / 2);
    //        Canvas.SetTop(rec, p.Y - rec.Width / 2);
    //        canvas.Children.Add(rec);
    //    }

    //    public void ScaleTransformFigure()
    //    {
    //        chRec = new Rectangle();
    //        foreach (Rectangle rec in movingFigurePoints)
    //            mainCanvas.Children.Remove(rec);
    //        movingFigurePoints.Clear();
    //        foreach (Figure fig in listFigure[indexFigure].groupFigures)
    //        {
    //            if(fig.modeFigure == Mode.modeTatami)
    //            {
    //                Point p = new Point(fig.tatamiControlLine.X, fig.tatamiControlLine.Y);
    //                p = GetScaledPoint(p, new Point(0,0), tVect, true);
    //                fig.tatamiControlLine.X = p.X;
    //                fig.tatamiControlLine.Y = p.Y;
    //                fig.tatamiControlLine.Normalize();
    //                fig.oldTatamiCenter = GetScaledPoint(fig.oldTatamiCenter, startVector, tVect, false);
    //            }
    //            else if (fig.modeFigure == Mode.modeSatin)
    //            {
    //                for (int i = 0; i < fig.oldSatinCenters.Count; i++)
    //                {
    //                    fig.oldSatinCenters[i] = GetScaledPoint(fig.oldSatinCenters[i], startVector, tVect, false);
    //                }
    //                for(int i = 0; i < fig.satinControlLines.Count; i++)
    //                {
    //                    Point p = new Point(fig.satinControlLines[i].X, fig.satinControlLines[i].Y);
    //                    p = GetScaledPoint(p, new Point(0, 0), tVect, true);
    //                    Vector vect = new Vector(p.X, p.Y);
    //                    vect.Normalize();
    //                    fig.satinControlLines[i] = vect;
    //                }
    //            }
    //            for (int i = 0; i < fig.points.Count; i++)
    //            {
    //                Point p = fig.points[i];
    //                Point scaledP = GetScaledPoint(p, startVector, tVect, false);
    //                if (i != fig.points.Count - 1)
    //                {
    //                    Point nextP = fig.points[i + 1];
    //                    Point scaledNextP = GetScaledPoint(nextP, startVector, tVect, false);
    //                    Shape sh;
    //                    Shape newSh;
    //                    Tuple<Point, Point> contPts = new Tuple<Point, Point>(new Point(), new Point());
    //                    fig.DictionaryPointLines.TryGetValue(p, out sh);
    //                    if (sh is Line)
    //                        newSh = GeometryHelper.SetLine(OptionColor.colorActive, scaledP, scaledNextP, false, mainCanvas);
    //                    else
    //                    {
    //                        fig.shapeControlPoints.TryGetValue(p, out contPts);
    //                        if (sh.MinHeight == 5)
    //                        {
    //                            Point scaledContPoint1 = GetScaledPoint(contPts.Item1, startVector, tVect,false);
    //                            Point scaledContPoint2 = GetScaledPoint(contPts.Item2, startVector, tVect, false);
    //                            contPts = new Tuple<Point, Point>(scaledContPoint1, scaledContPoint2);
    //                            newSh = GeometryHelper.SetBezier(OptionColor.colorActive, scaledP, contPts.Item1,
    //                                contPts.Item2, scaledNextP, mainCanvas);
    //                        }
    //                        else
    //                        {
    //                            Point scaledContPoint = GetScaledPoint(contPts.Item1, startVector, tVect, false);
    //                            contPts = new Tuple<Point, Point>(scaledContPoint, new Point());
    //                            newSh = GeometryHelper.SetArc(OptionColor.colorActive, scaledP, scaledNextP,
    //                                contPts.Item1, mainCanvas);
    //                        }
    //                    }
    //                    fig.DeleteShape(sh, p, mainCanvas);
    //                    fig.AddShape(newSh, scaledP, contPts);
    //                }
    //                if (fig.points.Count == 1)
    //                {
    //                    Shape sh;
    //                    Shape newSh;
    //                    Tuple<Point, Point> contPts = new Tuple<Point, Point>(new Point(), new Point());
    //                    fig.DictionaryPointLines.TryGetValue(p, out sh);
    //                    newSh = GeometryHelper.SetStarForSinglePoint(scaledP, OptionColor.colorActive, mainCanvas);
    //                    fig.DeleteShape(sh, p, mainCanvas);
    //                    fig.AddShape(newSh, scaledP, contPts);
    //                }
    //                fig.points[i] = scaledP;
    //            }
    //            fig.pointStart = fig.points[0];
    //            fig.pointEnd = fig.points[fig.points.Count - 1];
    //        }
    //    }

    //    private Point GetScaledPoint(Point origin, Point startVectorPoint, Vector transformVect, bool isControlLine)
    //    {
    //        Vector vect;
    //        Point scaledPoint = new Point();
    //        if (!Keyboard.IsKeyDown(Key.LeftShift) && !isControlLine)
    //        {
    //            if (status.Equals("north"))
    //                startVectorPoint = new Point(origin.X, ptsRec[1].Y);
    //            else if (status.Equals("west"))
    //                startVectorPoint = new Point(ptsRec[2].X, origin.Y);
    //            else if (status.Equals("south"))
    //                startVectorPoint = new Point(origin.X, ptsRec[0].Y);
    //            else if (status.Equals("east"))
    //                startVectorPoint = new Point(ptsRec[1].X, origin.Y);
    //        }
    //        vect = origin - startVectorPoint;
    //        double offsetX = vect.X * transformVect.X;
    //        double offsetY = vect.Y * transformVect.Y;
    //        scaledPoint.X = startVectorPoint.X + offsetX;
    //        scaledPoint.Y = startVectorPoint.Y + offsetY;
    //        return scaledPoint;
    //    }

    //    public void MoveFigureRectangle(Rectangle rec, Vector delta, Canvas canvas)
    //    {
    //        chRec.MaxHeight = 99999;
    //        canvas.Children.Remove(rec);
    //        Point p = new Point();
    //        p.X = Canvas.GetLeft(rec);
    //        p.Y = Canvas.GetTop(rec);
    //        p += delta;
    //        Canvas.SetLeft(rec, p.X);
    //        Canvas.SetTop(rec, p.Y);
    //        canvas.Children.Add(rec);
    //    }

    //    public void MoveFigureToNewPosition(bool isShiftElements, List<Figure> group, Vector figureVect)
    //    {
    //        if (!isShiftElements)
    //        {
    //            group = listFigure[indexFigure].groupFigures;
    //            foreach (Rectangle rec in movingFigurePoints)
    //                mainCanvas.Children.Remove(rec);
    //            movingFigurePoints.Clear();
    //            Point newPointStart = new Point(Canvas.GetLeft(chRec), Canvas.GetTop(chRec));
    //            figureVect = newPointStart - ptsRec[0];
    //        }
    //        foreach (Figure fig in group)
    //        {
    //            if (fig.modeFigure == Mode.modeTatami)
    //            {
    //                fig.oldTatamiCenter += figureVect;
    //            }
    //            else if(fig.modeFigure == Mode.modeSatin)
    //            {
    //                for(int i = 0; i < fig.oldSatinCenters.Count; i++)
    //                {
    //                    fig.oldSatinCenters[i] += figureVect;
    //                }
    //            }
    //            for (int i = 0; i < fig.points.Count; i++)
    //            {
    //                Point p = fig.points[i];
    //                if (i != fig.points.Count - 1)
    //                {
    //                    Point nextP = fig.points[i + 1];
    //                    Shape sh;
    //                    Shape newSh;
    //                    Tuple<Point, Point> contPts = new Tuple<Point, Point>(new Point(), new Point());
    //                    fig.DictionaryPointLines.TryGetValue(p, out sh);
    //                    if (sh is Line)
    //                        newSh = GeometryHelper.SetLine(OptionColor.colorActive, p + figureVect, nextP + figureVect, false, mainCanvas);
    //                    else
    //                    {
    //                        fig.shapeControlPoints.TryGetValue(p, out contPts);
    //                        if (sh.MinHeight == 5)
    //                        {
    //                            contPts = new Tuple<Point, Point>(contPts.Item1 + figureVect, contPts.Item2 + figureVect);
    //                            newSh = GeometryHelper.SetBezier(OptionColor.colorActive, p + figureVect, contPts.Item1,
    //                                contPts.Item2, nextP + figureVect, mainCanvas);
    //                        }
    //                        else
    //                        {
    //                            contPts = new Tuple<Point, Point>(contPts.Item1 + figureVect, new Point());
    //                            newSh = GeometryHelper.SetArc(OptionColor.colorActive, p + figureVect, nextP + figureVect,
    //                                contPts.Item1, mainCanvas);
    //                        }
    //                    }
    //                    fig.DeleteShape(sh, p, mainCanvas);
    //                    fig.AddShape(newSh, p + figureVect, contPts);
    //                }
    //                if (fig.points.Count == 1)
    //                {
    //                    Shape sh;
    //                    Shape newSh;
    //                    Tuple<Point, Point> contPts = new Tuple<Point, Point>(new Point(), new Point());
    //                    fig.DictionaryPointLines.TryGetValue(p, out sh);
    //                    newSh = GeometryHelper.SetStarForSinglePoint(p + figureVect, OptionColor.colorActive, mainCanvas);
    //                    fig.DeleteShape(sh, p, mainCanvas);
    //                    fig.AddShape(newSh, p + figureVect, contPts);
    //                }
    //                fig.points[i] += figureVect;
    //            }
    //            fig.pointStart += figureVect;
    //            fig.pointEnd += figureVect;
    //        }
    //    }

    //    public void CursorMenuDrawInColor()
    //    {
    //        tempListFigure = listFigure.ToList<Figure>();
    //        mainCanvas.Children.Clear();
    //        foreach (Figure fig in listFigure[indexFigure].groupFigures)
    //        {
    //            fig.ChangeFigureColor(OptionColor.colorNewActive, false);
    //            fig.AddFigure(mainCanvas);
    //        }
    //        mainCanvas.Background = OptionColor.colorNewBackground;
    //    }

    //    private Vector FindFigureRectangle(Point p1, Point p2, Canvas canvas)
    //    {
    //        Vector scaleVector = GetVectorForNonOrthogonalScaling(p1, p2);
    //        Vector vect;
    //        if (status.Equals("southeast"))
    //        {
    //            vect = ptsRec[2] - ptsRec[0];
    //            vect *= scaleVector.X;
    //            vect = InvertVector(p1, p2, vect);
    //            chRec = DrawChoosingRectangle(ptsRec[0], new Point(ptsRec[0].X + vect.X, ptsRec[0].Y + vect.Y), canvas);
    //        }
    //        else if(status.Equals("southwest"))
    //        {
    //            vect = ptsRec[1] - ptsRec[3];
    //            vect *= scaleVector.X;
    //            vect = InvertVector(p1, p2, vect);
    //            chRec = DrawChoosingRectangle(new Point(ptsRec[3].X + vect.X, ptsRec[0].Y),
    //                new Point(ptsRec[2].X, ptsRec[3].Y + vect.Y), canvas);
    //        }
    //        else if (status.Equals("northwest"))
    //        {
    //            vect = ptsRec[0] - ptsRec[2];
    //            vect *= scaleVector.X;
    //            vect = InvertVector(p1, p2, vect);
    //            chRec = DrawChoosingRectangle(new Point(ptsRec[2].X + vect.X, ptsRec[2].Y + vect.Y), ptsRec[2], canvas);
    //        }
    //        else if (status.Equals("northeast"))
    //        {
    //            vect = ptsRec[3] - ptsRec[1];
    //            vect *= scaleVector.X;
    //            vect = InvertVector(p1, p2, vect);
    //            chRec = DrawChoosingRectangle(new Point(ptsRec[0].X, ptsRec[1].Y + vect.Y), 
    //                new Point(ptsRec[1].X + vect.X, ptsRec[1].Y), canvas);
    //        }
    //        scaleVector = InvertVector(p1, p2, scaleVector);
    //        return scaleVector;
    //    }

    //    private Vector InvertVector(Point p1, Point p2, Vector vect)
    //    {
    //        if (p1.X > p2.X)
    //            vect.X = -vect.X;
    //        if (p1.Y > p2.Y)
    //            vect.Y = -vect.Y;
    //        return vect;
    //    }

    //    public void DrawOutsideRectangles(bool isScale, bool rememberLastRect, Canvas canvas)
    //    {
    //        Point lastPoint = new Point();
    //        if (rememberLastRect)
    //            lastPoint = new Point(Canvas.GetLeft(transRectangles[8]) + transRectangles[8].Height / 2,
    //                Canvas.GetTop(transRectangles[8]) + transRectangles[8].Height / 2);
    //        transRectangles = new List<Rectangle>();
    //        List<Point> PointsOutSideRectangle = new List<Point>();
    //        Point a, b, c, d;
    //        List<Point> pts = GeometryHelper.GetFourOutsidePointsForGroup(listFigure[indexFigure].groupFigures,
    //            OptionDrawLine.cursorModeRectangleDistance);
    //        a = pts[0];
    //        b = pts[1];
    //        c = pts[2];
    //        d = pts[3];
    //        PointsOutSideRectangle.Add(new Point((d.X + a.X) / 2, (d.Y + a.Y) / 2));
    //        PointsOutSideRectangle.Add(d);
    //        PointsOutSideRectangle.Add(new Point((c.X + d.X) / 2, (c.Y + d.Y) / 2));
    //        PointsOutSideRectangle.Add(c);
    //        PointsOutSideRectangle.Add(new Point((b.X + c.X) / 2, (b.Y + c.Y) / 2));
    //        PointsOutSideRectangle.Add(b);
    //        PointsOutSideRectangle.Add(new Point((a.X + b.X) / 2, (b.Y + a.Y) / 2));
    //        PointsOutSideRectangle.Add(a);
    //        double size = OptionDrawLine.sizeRectangleForRotation;
    //        if (rememberLastRect)
    //            PointsOutSideRectangle.Add(lastPoint);
    //        else if (!isScale)
    //            PointsOutSideRectangle.Add(GetCenterForGroup(pts));
    //        else
    //            size = OptionDrawLine.sizeRectangleForScale;
    //        foreach (Point p in PointsOutSideRectangle)
    //        {
    //            transRectangles.Add(GeometryHelper.DrawTransformingRectangle(p, size, canvas));
    //        }
    //        if (!isScale)
    //        {
    //            for (int i = 0; i < transRectangles.Count; i++)
    //            {
    //                int index;
    //                if (i == 0 || i == 4)
    //                    index = 0;
    //                else if (i == 2 || i == 6)
    //                    index = 10;
    //                else if (i == 8)
    //                    index = 11;
    //                else
    //                    index = i * (i % 2);
    //                ImageBrush image = new ImageBrush(new BitmapImage(
    //new Uri(@"pack://application:,,,/Images/arrow" + index + ".gif", UriKind.Absolute)));
    //                transRectangles[i].Fill = image;
    //                transRectangles[i].StrokeThickness = 0;
    //            }
    //        }
    //        foreach (Figure fig in listFigure[indexFigure].groupFigures)
    //            fig.ChangeFigureColor(OptionColor.colorActive, false);
    //    }

    //    private Point GetCenterForGroup(List<Point> pts)
    //    {
    //        return new Point((pts[2].X + pts[0].X) / 2, (pts[2].Y + pts[0].Y) / 2);
    //    }

    //    public void ShowJoinCursorMessage(Figure firstFigure, Figure secondFigure,Canvas canvas)
    //    {
    //        foreach (Figure fig in secondFigure.groupFigures)
    //            fig.ChangeFigureColor(OptionColor.colorArc, false);

    //        var JoinCursorWindow = new View.JoinCursor(listFigure, firstFigure, secondFigure, canvas);
    //        JoinCursorWindow.ShowDialog();
    //        foreach(Figure fig in secondFigure.groupFigures)
    //            fig.ChangeFigureColor(OptionColor.colorInactive, false);

    //        if (OptionMode.mode == Mode.modeCursorJoinShiftElements)
    //            JoinShiftElements(firstFigure, secondFigure);
    //        OptionMode.mode = Mode.modeCursor;
    //        RedrawEverything(listFigure, indexFigure, false, mainCanvas);
    //        DrawOutsideRectangles(true, false, canvas);
    //        ShowPositionStatus(firstFigure, true,false);
    //    }

    //    public void ShowBreakCursorMessage(Figure fig, Canvas canvas)
    //    {
    //        fig.ChangeFigureColor(OptionColor.colorArc, false);
    //        int index = fig.groupFigures.IndexOf(fig);
            
    //        var RazorvatWindow = new View.Razorvat(fig.groupFigures, index, mainCanvas);
    //        RazorvatWindow.ShowDialog();
    //        foreach (Figure indFig in listFigure)
    //            if (indFig.points.Count > 0)
    //            {
    //                if (indFig.points[indFig.points.Count - 1].X == -31231 && indFig.points[indFig.points.Count - 1].Y == 312312)
    //                {
    //                    indexFigure = listFigure.IndexOf(indFig);
    //                    indFig.points.RemoveAt(indFig.points.Count - 1);
    //                    break;
    //                }
    //            }
    //        OptionMode.mode = Mode.modeCursor;
    //        ChangeFiguresColor(listFigure, canvas);
    //        RedrawEverything(listFigure, indexFigure, false, mainCanvas);
    //        DrawOutsideRectangles(true, false, canvas);
    //        ShowPositionStatus(fig, true,false);
    //    }

    //    public void JoinShiftElements(Figure firstFigure, Figure secondFigure)
    //    {
    //        Point start = firstFigure.groupFigures[firstFigure.groupFigures.Count - 1].pointEnd;
    //        Point end = secondFigure.groupFigures[0].pointStart;
    //        Vector newFigVect = start - end;
    //        MoveFigureToNewPosition(true, secondFigure.groupFigures, newFigVect);
    //        List<Figure> group1 = new List<Figure>(firstFigure.groupFigures);
    //        List<Figure> group2 = new List<Figure>(secondFigure.groupFigures);

    //        foreach (Figure fig in group1)
    //        {
    //            foreach (Figure fig2 in group2)
    //                fig.groupFigures.Add(fig2);
    //        }

    //        foreach (Figure fig in group2)
    //        {
    //            for (int i = group1.Count - 1; i > -1; i--)
    //                fig.groupFigures.Insert(0, group1[i]);
    //        }
    //    }
    }
}