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
        private void CanvasTest_MouseRightButtonDown(object sender, MouseButtonEventArgs e)         //при нажатии на праую кнопку мыши
        {
            Mouse.Capture(MainCanvas);
            if ((OptionMode.mode == Mode.modeFigure || OptionMode.mode == Mode.modeTatami || OptionMode.mode == Mode.modeSatin || OptionMode.mode == Mode.modeRunStitch)
                && e.OriginalSource is Rectangle)
            {
                Rectangle rect = (Rectangle)e.OriginalSource;
                ChooseFirstOrLastRectangle(rect, false, MainCanvas);
            }
            else if (OptionMode.mode == Mode.modeFigure || OptionMode.mode == Mode.modeTatami || OptionMode.mode == Mode.modeSatin ||
                OptionMode.mode == Mode.modeCursor)
                BreakFigureOrMakeJoinedFigure(listFigure, e.OriginalSource,MainCanvas);
            ExitFromRisuimode();
        }

        private void CanvasTest_MouseMove(object sender, MouseEventArgs e)
        {
            Point NewMousePosition = e.GetPosition(MainCanvas);
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (OptionMode.mode == Mode.modeDraw)
                {
                    if (listFigure[indexFigure].Points.Count > 0)
                    {
                        if (!startDrawing)
                        {
                            MainCanvas.Children.RemoveAt(MainCanvas.Children.Count - 1);
                        }
                        Point normalizedPoint = FindClosestDot(e.GetPosition(MainCanvas));
                        Line line = listFigure[indexFigure].GetLine(listFigure[indexFigure].PointEnd, normalizedPoint);
                        line.StrokeThickness = OptionDrawLine.strokeThickness;
                        line.Stroke = OptionColor.colorArc;
                        MainCanvas.Children.Add(line);
                        lastLine = line;
                        startDrawing = false;
                    }
                }
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if(OptionMode.mode == Mode.modeDrawCurve)
                {
                    MainCanvas.Children.Remove(listFigure[indexFigure].NewPointEllipse);
                    MainCanvas.Children.Remove(changedLine);
                    ChangeBezierPoints(chosenPts, e.GetPosition(MainCanvas));
                    changedLine = GeometryHelper.SetBezier(OptionColor.colorCurve, chosenPts[0], chosenPts[1], chosenPts[2], chosenPts[3], MainCanvas);
                }

                if (OptionMode.mode == Mode.modeDrawArc)
                {
                    MainCanvas.Children.Remove(listFigure[indexFigure].NewPointEllipse);
                    MainCanvas.Children.Remove(changedLine);
                    chosenPts[2] = e.GetPosition(MainCanvas);
                    changedLine = GeometryHelper.SetArc(OptionColor.colorArc, chosenPts[0], chosenPts[1], chosenPts[2], MainCanvas);
                }
                if (OptionMode.mode == Mode.modeMovePoints)
                {
                    MainCanvas.Children.Remove(listFigure[indexFigure].NewPointEllipse);
                    MainCanvas.Children.Remove(firstRec);
                    Vector delta = e.GetPosition(MainCanvas) - prevPoint;
                    ManipulateShapes(delta);
                    prevPoint = e.GetPosition(MainCanvas);
                }
                if (OptionMode.mode == Mode.modeCursorMoveRect)
                {
                    Vector delta = e.GetPosition(MainCanvas) - prevPoint;
                    MoveFigureRectangle(chRec, delta, MainCanvas);
                    foreach (Rectangle rec in movingFigurePoints)
                        MoveFigureRectangle(rec, delta, MainCanvas);
                    prevPoint = e.GetPosition(MainCanvas);
                }
                if (OptionMode.mode == Mode.modeEditFigures)
                {
                    if (choosingRectangle.Points.Count > 0)
                    {
                        if(!startDrawing)
                        {
                            MainCanvas.Children.Remove(chRec);
                        }
                        chRec = DrawChoosingRectangle(choosingRectangle.Points[0], e.GetPosition(MainCanvas), MainCanvas);
                        startDrawing = false;
                    }
                }

                if ((OptionMode.mode == Mode.modeTatami || OptionMode.mode == Mode.modeSatin) && !startDrawing)
                {
                    if (controlLine.Points.Count > 1)
                    {
                        MainCanvas.Children.RemoveAt(MainCanvas.Children.Count - 1);
                        controlLine.Points.RemoveAt(controlLine.Points.Count - 1);
                    }
                    Line line = controlLine.GetLine(controlLine.Points[0], e.GetPosition(MainCanvas));
                    Vector vect1 = new Vector(e.GetPosition(MainCanvas).X - controlLine.Points[0].X,
                        e.GetPosition(MainCanvas).Y - controlLine.Points[0].Y);
                    vect1.Normalize();
                    Vector vect2 = new Vector(1, 0);
                    double contLineAngle = Vector.AngleBetween(vect1, vect2);
                    if (contLineAngle > 0)
                        contLineAngle -= 180;
                    else
                        contLineAngle +=180;
                    statusbar3.Content = "Угол секущей = " + Math.Round(contLineAngle, 1);
                    DoubleCollection dashes = new DoubleCollection();
                    dashes.Add(2);
                    dashes.Add(2);
                    line.StrokeDashArray = dashes;
                    line.StrokeThickness = OptionDrawLine.strokeThickness;
                    line.Stroke = OptionColor.colorInactive;
                    MainCanvas.Children.Add(line);
                    MainCanvas.UpdateLayout();
                    controlLine.Points.Add(e.GetPosition(MainCanvas));
                }
                if (OptionMode.mode == Mode.modeScaleFigure)
                {
                    MainCanvas.Children.Remove(chRec);
                    if (Keyboard.IsKeyDown(Key.LeftShift))
                        MoveScalingRectangleRelativeToCenter(e.GetPosition(MainCanvas), MainCanvas);
                    else
                        MoveScalingRectangle(e.GetPosition(MainCanvas),MainCanvas);

                    ShowPositionStatus(listFigure[indexFigure], false, true);
                }
                if (OptionMode.mode == Mode.rotateFigure)
                {
                    Rectangle centerRect = transRectangles[8];
                    Point centerPoint = new Point(Canvas.GetLeft(centerRect) + centerRect.Width / 2, Canvas.GetTop(centerRect) + centerRect.Width / 2);
                    RotateRotatingRectangle(e.GetPosition(MainCanvas), centerPoint,prevPoint, MainCanvas);
                }
                if(OptionMode.mode == Mode.modeChangeRotatingCenter)
                {
                    Rectangle centerRect = transRectangles[8];
                    MainCanvas.Children.Remove(centerRect);
                    Canvas.SetTop(centerRect, e.GetPosition(MainCanvas).Y - centerRect.Height / 2);
                    Canvas.SetLeft(centerRect, e.GetPosition(MainCanvas).X - centerRect.Height / 2);
                    MainCanvas.Children.Add(centerRect);
                }
            }
        }

        private void CanvasTest_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            if (OptionMode.mode == Mode.modeTatami && !startDrawing)
            {
                statusbar3.Content = "";
                if (controlLine.Points.Count == 1)
                {
                    controlLine.Points.Add(e.GetPosition(MainCanvas));
                }
                FindControlLine(listFigure[indexFigure], controlLine, MainCanvas,false);
            }
            if (OptionMode.mode == Mode.modeSatin && !startDrawing)
            {
                if (controlLine.Points.Count == 1)
                {
                    controlLine.Points.Add(e.GetPosition(MainCanvas));
                }
                if (controlLine.Points[0] != controlLine.Points[1])
                {
                    Point a = controlLine.Points[0];
                    Point b = controlLine.Points[1];
                    FindGladControlLine(a,b, linesForSatin, listFigure[firstSatinFigure], listFigure[secondSatinFigure],false, MainCanvas);
                }
            }
            if (OptionMode.mode == Mode.modeCursorMoveRect)
            {
                OptionMode.mode = Mode.modeCursor;
                if (chRec.MaxHeight == 99999)
                {
                    MoveFigureToNewPosition(false, null,new Vector());
                    RedrawEverything(listFigure, indexFigure, false, MainCanvas);
                    DrawOutsideRectangles(true, false, MainCanvas);
                }
                else if (transRectangles[0].Fill == OptionColor.colorInactive && 
                    (listFigure[indexFigure].Points.Count != 1 || listFigure[indexFigure].groupFigures.Count != 1))
                    DrawOutsideRectangles(false, false, MainCanvas);
                else
                    DrawOutsideRectangles(true, false, MainCanvas);
            }
            if (OptionMode.mode == Mode.rotateFigure)
            {
                Rectangle centerRect = transRectangles[8];
                Point centerPoint = new Point(Canvas.GetLeft(centerRect) + centerRect.Width / 2, Canvas.GetTop(centerRect) + centerRect.Width / 2);
                RotateFigure(centerPoint);
                OptionMode.mode = Mode.modeCursor;
                RedrawEverything(listFigure, indexFigure, false, MainCanvas);
                DrawOutsideRectangles(false, true, MainCanvas);
                ShowPositionStatus(listFigure[indexFigure], true, false);
            }
            if (OptionMode.mode == Mode.modeScaleFigure)
            {
                OptionMode.mode = Mode.modeCursor;
                if (MainCanvas.Cursor != defaultCursor)
                {
                    ScaleTransformFigure();
                }
                RedrawEverything(listFigure, indexFigure, false, MainCanvas);
                DrawOutsideRectangles(true, false, MainCanvas);
                MainCanvas.Cursor = defaultCursor;
            }
            if (OptionMode.mode == Mode.modeEditFigures)
            {
                if (choosingRectangle.Points.Count > 0)
                {
                    RedrawEverything(listFigure, indexFigure, true, MainCanvas);
                    Point UpperLeftCorner = new Point();
                    Point LowerRightCorner = new Point();
                    if(e.GetPosition(MainCanvas).X < choosingRectangle.Points[0].X)
                    {
                        UpperLeftCorner.X = e.GetPosition(MainCanvas).X;
                        LowerRightCorner.X = choosingRectangle.Points[0].X;
                    }
                    else
                    {
                        UpperLeftCorner.X = choosingRectangle.Points[0].X;
                        LowerRightCorner.X = e.GetPosition(MainCanvas).X;
                    }
                    if (e.GetPosition(MainCanvas).Y < choosingRectangle.Points[0].Y)
                    {
                        UpperLeftCorner.Y = e.GetPosition(MainCanvas).Y;
                        LowerRightCorner.Y = choosingRectangle.Points[0].Y;
                    }
                    else
                    {
                        UpperLeftCorner.Y = choosingRectangle.Points[0].Y;
                        LowerRightCorner.Y = e.GetPosition(MainCanvas).Y;
                    }
                    for (int i = 0; i < listFigure[indexFigure].Points.Count;i++ )
                    {
                        Point newPoint = listFigure[indexFigure].Points[i];
                        if(newPoint.X < LowerRightCorner.X && newPoint.X > UpperLeftCorner.X &&
                            newPoint.Y < LowerRightCorner.Y && newPoint.Y > UpperLeftCorner.Y)
                        {
                            listFigure[indexFigure].highlightedPoints.Add(i);
                        }
                    }
                    listFigure[indexFigure].ChangeRectangleColor();
                }
                chRec = new Rectangle();
            }
            if (OptionMode.mode == Mode.modeMovePoints)
            {
                AddManipulatedShapes(MainCanvas);
                RedrawEverything(listFigure, indexFigure, true, MainCanvas);

                listFigure[indexFigure].ChangeRectangleColor();
                listChangedShapes.Clear();
                OptionMode.mode = Mode.modeEditFigures;
                ShowPositionStatus(listFigure[indexFigure], false, false);
            }
            if(OptionMode.mode == Mode.modeChangeRotatingCenter)
            {
                MainCanvas.Cursor = defaultCursor;
                OptionMode.mode = Mode.modeCursor;
            }
            if (OptionMode.mode == Mode.modeDrawCurve || OptionMode.mode == Mode.modeDrawArc)
            {
                if (chosenPts.Count > 1)
                {
                    if (OptionMode.mode == Mode.modeDrawCurve)
                        listFigure[indexFigure].AddShape(changedLine, chosenPts[0], new Tuple<Point,Point>(chosenPts[1],chosenPts[2]));
                    else
                        listFigure[indexFigure].AddShape(changedLine, chosenPts[0], new Tuple<Point,Point>(e.GetPosition(MainCanvas), new Point()));
                    OptionMode.mode = Mode.modeEditFigures;
                    ShowPositionStatus(listFigure[indexFigure], false, false);
                }
            }
            startDrawing = true;
        }

        private void CanvasTest_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            if (OptionMode.mode == Mode.modeDraw)
            {
                if (listFigure[indexFigure].Points.Count > 1)
                {
                    if (startDrawing)
                    {
                        MainCanvas.Children.Remove(lastRec);
                    }
                    else
                    {
                        MainCanvas.Children.Remove(lastLine);
                        MainCanvas.Children.Remove(lastRec);
                    }
                }
                if (listFigure[indexFigure].Points.Count ==1)
                {
                    lastRec.StrokeThickness = OptionDrawLine.strokeThickness;
                    firstRec = lastRec;
                    if(!startDrawing)
                        MainCanvas.Children.Remove(lastLine);
                }
                Point point = FindClosestDot(e.GetPosition(MainCanvas));
                lastRec = listFigure[indexFigure].AddPoint(point, OptionColor.colorActive, true,true, OptionDrawLine.sizeRectangle);
                ShowPositionStatus(listFigure[indexFigure],false,false);
            }
            startDrawing = true;
        }

        void CanvasTest_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)          //левая кнопка мыши
        {
            Mouse.Capture(MainCanvas);
            if ((OptionMode.mode == Mode.modeTatami || OptionMode.mode == Mode.modeSatin || OptionMode.mode == Mode.modeRunStitch)
                && e.OriginalSource is Rectangle)
            {
                Rectangle rect = (Rectangle)e.OriginalSource;
                ChooseFirstOrLastRectangle(rect, true, MainCanvas);
            }
            else if (OptionMode.mode == Mode.modeTatami)
            {
                bool isNewFigureClicked = ChooseFigureByClicking(e.GetPosition(MainCanvas), listFigure, e.OriginalSource, MainCanvas);
                if (!isNewFigureClicked)
                {
                    startDrawing = false;
                    controlLine.Points.Clear();
                    controlLine.Points.Add(e.GetPosition(MainCanvas));
                }
            }
            else if (OptionMode.mode == Mode.modeSatin)
            {
                bool isNewFigureClicked = ChooseFigureByClicking(e.GetPosition(MainCanvas), listFigure, e.OriginalSource, MainCanvas);
                if (!isNewFigureClicked)
                {
                    if (e.OriginalSource is Shape)
                    {
                        if (e.OriginalSource is Ellipse)
                        {
                            Ellipse ell = (Ellipse)e.OriginalSource;
                            Point ellPoint = new Point(Canvas.GetLeft(ell) + ell.Width/2, Canvas.GetTop(ell) + ell.Height/2);
                            int index = 0;
                            for(int i = 0; i < listFigure[firstSatinFigure].oldGladCenters.Count; i++)
                            {
                                Point oldCenter = listFigure[firstSatinFigure].oldGladCenters[i];
                                if(Math.Abs(ellPoint.X - oldCenter.X) < 0.000000001 && Math.Abs(ellPoint.Y - oldCenter.Y) < 0.000000001)
                                {
                                    index = i;
                                    break;
                                }
                            }
                            listFigure[firstSatinFigure].oldGladCenters.RemoveAt(index);
                            listFigure[firstSatinFigure].satinControlLines.RemoveAt(index);
                            for (int i = 0; i < linesForSatin.Count; i++)
                            {
                                if (linesForSatin[i].Shapes.Contains(ell))
                                {
                                    linesForSatin[i].RemoveFigure(MainCanvas);
                                    linesForSatin[i].Shapes.Clear();
                                    linesForSatin.Remove(linesForSatin[i]);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        startDrawing = false;
                        controlLine.Points.Clear();
                        controlLine.Points.Add(e.GetPosition(MainCanvas));
                    }
                }
            }
            if (OptionMode.mode == Mode.modeDraw || OptionMode.mode == Mode.modeRunStitch)
            {
                ChooseFigureByClicking(e.GetPosition(MainCanvas),listFigure, e.OriginalSource, MainCanvas);
            }
            if (OptionMode.mode == Mode.modeFigure)
            {
                bool isNewFigureClicked = ChooseFigureByClicking(e.GetPosition(MainCanvas), listFigure, e.OriginalSource, MainCanvas);
                if(!isNewFigureClicked && e.OriginalSource is Rectangle)
                {
                    Rectangle rect = (Rectangle)e.OriginalSource;
                    ChooseFirstOrLastRectangle(rect, true, MainCanvas);
                }
            }
            if (OptionMode.mode == Mode.modeEditFigures)
            {
                MainCanvas.Children.Remove(chRec);
                bool isNewFigureClicked = ChooseFigureByClicking(e.GetPosition(MainCanvas),listFigure, e.OriginalSource, MainCanvas);
                chosenPts.Clear();
                choosingRectangle.Points.Clear();
                if (e.OriginalSource is Line || e.OriginalSource is Path)
                {
                    if (!isNewFigureClicked)
                    {
                        double x2 = 0;
                        double y2 = 0;
                        Shape clickedShape = (Shape)e.OriginalSource;
                        if (clickedShape.StrokeThickness == OptionDrawLine.strokeThickness)
                        {
                            Shape sha;
                            listFigure[indexFigure].DictionaryInvLines.TryGetValue(clickedShape, out sha);
                            clickedShape = sha;
                        }
                        if (clickedShape == null)
                            return;
                        if (e.OriginalSource is Line)
                        {
                            Line clickedLine = (Line)clickedShape;
                            x2 = clickedLine.X2;
                            y2 = clickedLine.Y2;
                        }
                        else if (e.OriginalSource is Path)
                        {
                            Path path = (Path)clickedShape;
                            PathGeometry myPathGeometry = (PathGeometry)path.Data;
                            Point p;
                            Point tg;
                            myPathGeometry.GetPointAtFractionLength(1, out p, out tg);
                            x2 = p.X;
                            y2 = p.Y;
                        }
                        Shape sh;
                        var keyLine = listFigure[indexFigure].DictionaryInvLines.FirstOrDefault(x => x.Value == clickedShape);
                        if (keyLine.Key == null)
                            return;
                        if (keyLine.Key.Stroke == OptionColor.colorCurve)
                        {
                            OptionMode.mode = Mode.modeDrawCurve;
                            var point = listFigure[indexFigure].DictionaryPointLines.FirstOrDefault(x => x.Value == keyLine.Key);
                            listFigure[indexFigure].DictionaryPointLines.TryGetValue(point.Key, out sh);
                            chosenPts = PrepareForBezier((Shape)e.OriginalSource, e.GetPosition(MainCanvas), point.Key, new Point(x2,y2));

                            listFigure[indexFigure].DeleteShape(sh, point.Key, MainCanvas);
                            changedLine = sh;
                        }
                        if (keyLine.Key.Stroke == OptionColor.colorArc)
                        {
                            OptionMode.mode = Mode.modeDrawArc;
                            var point = listFigure[indexFigure].DictionaryPointLines.FirstOrDefault(x => x.Value == keyLine.Key);
                            listFigure[indexFigure].DictionaryPointLines.TryGetValue(point.Key, out sh);
                            listFigure[indexFigure].DeleteShape(sh, point.Key, MainCanvas);
                            changedLine = sh;
                            chosenPts.Add(point.Key);
                            chosenPts.Add(new Point(x2, y2));
                            chosenPts.Add(new Point());
                        }
                    }
                    else
                        listFigure[indexFigure].highlightedPoints.Clear();
                }
                else if (e.OriginalSource is Rectangle)
                {
                    if (!isNewFigureClicked)
                    {
                        firstRec = (Rectangle)e.OriginalSource;
                        double x = Canvas.GetLeft(firstRec) + firstRec.ActualHeight / 2;
                        double y = Canvas.GetTop(firstRec) + firstRec.ActualWidth / 2;
                        int index = listFigure[indexFigure].Points.IndexOf(new Point(x, y));
                        if(!listFigure[indexFigure].highlightedPoints.Contains(index))
                        {
                            listFigure[indexFigure].highlightedPoints.Clear();
                            listFigure[indexFigure].highlightedPoints.Add(index);
                            listFigure[indexFigure].ChangeRectangleColor();
                        }
                        string status;
                        Rectangle rec1 = new Rectangle();
                        Rectangle rec2 = new Rectangle();
                        Tuple<Point, Point> contPts = new Tuple<Point, Point>(new Point(), new Point());
                        for (int i = 0; i < listFigure[indexFigure].Points.Count - 1; i++)
                        {
                            Point p = listFigure[indexFigure].Points[i];
                            
                            if (!listFigure[indexFigure].highlightedPoints.Contains(i) && listFigure[indexFigure].highlightedPoints.Contains(i + 1))
                            {
                                status = "second";
                                rec2 = listFigure[indexFigure].RectangleOfFigures[i + 1];
                            }
                            else if (listFigure[indexFigure].highlightedPoints.Contains(i) && listFigure[indexFigure].highlightedPoints.Contains(i + 1))
                            {
                                status = "both";
                                rec1 = listFigure[indexFigure].RectangleOfFigures[i];
                                rec2 = listFigure[indexFigure].RectangleOfFigures[i + 1];
                            }
                            else if (listFigure[indexFigure].highlightedPoints.Contains(i) && !listFigure[indexFigure].highlightedPoints.Contains(i + 1))
                            {
                                rec1 = listFigure[indexFigure].RectangleOfFigures[i];
                                status = "first";
                            }
                            else
                                continue;
                            listFigure[indexFigure].DictionaryShapeControlPoints.TryGetValue(p, out contPts);
                            if (contPts == null)
                                contPts = new Tuple<Point, Point>(new Point(), new Point());
                            Shape sh;
                            listFigure[indexFigure].DictionaryPointLines.TryGetValue(p, out sh);
                            ChangedShape chShape = new ChangedShape(sh, status, p, contPts.Item1, contPts.Item2, listFigure[indexFigure].Points[i + 1],
                                rec1,rec2, MainCanvas);
                            listFigure[indexFigure].DeleteShape(sh, p, MainCanvas);
                            listChangedShapes.Add(chShape);
                        }
                        if (listFigure[indexFigure].Points.Count == 1 && listFigure[indexFigure].highlightedPoints.Contains(0))
                        {
                            Point p = listFigure[indexFigure].Points[0];
                            rec1 = listFigure[indexFigure].RectangleOfFigures[0];
                            status = "single";
                            Shape sh;
                            listFigure[indexFigure].DictionaryPointLines.TryGetValue(p, out sh);
                            ChangedShape chShape = new ChangedShape(sh, status, p, contPts.Item1, contPts.Item2, new Point(),
                                rec1, rec2, MainCanvas);
                            listFigure[indexFigure].DeleteShape(sh, p, MainCanvas);
                            listChangedShapes.Add(chShape);
                        }
                        prevPoint = e.GetPosition(MainCanvas);
                        OptionMode.mode = Mode.modeMovePoints;
                    }
                }
                else
                {
                    listFigure[indexFigure].highlightedPoints.Clear();
                    choosingRectangle.Points.Add(e.GetPosition(MainCanvas));
                }
            }
            if (OptionMode.mode == Mode.modeCursor)
            {
                bool isNewFigureClicked = ChooseFigureByClicking(e.GetPosition(MainCanvas), listFigure, e.OriginalSource, MainCanvas);
                if (e.OriginalSource is Line || e.OriginalSource is Path)
                {
                    if (!isNewFigureClicked)
                    {
                        foreach (Rectangle rec in transRectangles)
                            MainCanvas.Children.Remove(rec);
                        prevPoint = e.GetPosition(MainCanvas);
                        InitializeFigureRectangle(0);
                        OptionMode.mode = Mode.modeCursorMoveRect;
                    }
                    else
                        DrawOutsideRectangles(true, false, MainCanvas);
                }
                if (e.OriginalSource is Rectangle && (((Rectangle)e.OriginalSource).Width == OptionDrawLine.sizeRectangleForRotation ||
                    ((Rectangle)e.OriginalSource).Width == OptionDrawLine.sizeRectangleForScale) && 
                    (listFigure[indexFigure].Points.Count != 1 || listFigure[indexFigure].groupFigures.Count > 1))
                {
                    Rectangle rec = (Rectangle)e.OriginalSource;
                    if (rec.Fill == OptionColor.colorInactive)
                    {
                        string[] statuses = { "north", "northeast", "east", "southeast", "south", "southwest", "west", "northwest" };
                        int index = transRectangles.IndexOf(rec);
                        InitializeScaling(statuses[index]);
                        OptionMode.mode = Mode.modeScaleFigure;
                        foreach (Rectangle rect in transRectangles)
                            MainCanvas.Children.Remove(rect);
                    }
                    else
                    {
                        if (transRectangles.IndexOf(rec) == 8)
                        {
                            OptionMode.mode = Mode.modeChangeRotatingCenter;
                            MainCanvas.Cursor = Cursors.Cross;   
                        }
                        else
                        {
                            OptionMode.mode = Mode.rotateFigure;
                            foreach (Rectangle rect in transRectangles)
                                MainCanvas.Children.Remove(rect);
                            InitializeFigureRectangle(0);
                            prevPoint = e.GetPosition(MainCanvas);
                        }
                    }
                }
            }
            if(OptionMode.mode == Mode.zoomIn)
            {
                SaveLastView();
                Point currentPosition = e.GetPosition(this);
                if(OptionGrid.scaleMultiplier <= 16)
                    ScaleCanvas(2, currentPosition, MainCanvas);
                else if (OptionGrid.scaleMultiplier > 16 && OptionGrid.scaleMultiplier <32)
                {
                    double multiplier = 32 / OptionGrid.scaleMultiplier;
                    ScaleCanvas(multiplier, currentPosition, MainCanvas);
                }
                else
                    MoveCanvas(currentPosition, MainCanvas);
                SetGrid();
                OptionMode.mode = prevMode;
                MainCanvas.Cursor = prevCursor;
            }
            if (OptionMode.mode == Mode.zoomOut)
            {
                SaveLastView();
                Point currentPosition = e.GetPosition(this);
                if (OptionGrid.scaleMultiplier >= 0.5)
                    ScaleCanvas(0.5, currentPosition, MainCanvas);
                else if (OptionGrid.scaleMultiplier > 0.5 && OptionGrid.scaleMultiplier < 0.25)
                {
                    double multiplier = 0.25 / OptionGrid.scaleMultiplier;
                    ScaleCanvas(multiplier, currentPosition, MainCanvas);
                }
                else
                    MoveCanvas(currentPosition, MainCanvas);
                SetGrid();
                OptionMode.mode = prevMode;
                MainCanvas.Cursor = prevCursor;
            }
            if (OptionMode.mode == Mode.moveCanvas)
            {
                SaveLastView();
                Point currentPosition = e.GetPosition(this);
                MoveCanvas(currentPosition, MainCanvas);
                SetGrid();
                OptionMode.mode = prevMode;
                MainCanvas.Cursor = prevCursor;
            }
            if (OptionMode.mode == Mode.oneToOne)
            {
                SaveLastView();
                double multiplier = 2.1 / OptionGrid.scaleMultiplier;
                Point currentPosition = e.GetPosition(this);
                ScaleCanvas(multiplier, currentPosition, MainCanvas);
                SetGrid();
                OptionMode.mode = prevMode;
                MainCanvas.Cursor = prevCursor;
            }
            ExitFromRisuimode();
        }
    }
}
