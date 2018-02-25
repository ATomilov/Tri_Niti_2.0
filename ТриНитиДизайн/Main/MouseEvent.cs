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
            if ((OptionRegim.regim == Regim.RegimFigure || OptionRegim.regim == Regim.RegimTatami || OptionRegim.regim == Regim.RegimGlad || OptionRegim.regim == Regim.RegimCepochka)
                && e.OriginalSource is Rectangle)
            {
                Rectangle rect = (Rectangle)e.OriginalSource;
                ChooseFirstOrLastRectangle(rect, false, MainCanvas);
            }
            else if (OptionRegim.regim == Regim.RegimFigure || OptionRegim.regim == Regim.RegimTatami || OptionRegim.regim == Regim.RegimGlad ||
                OptionRegim.regim == Regim.RegimCursor)
                BreakFigureOrMakeJoinedFigure(ListFigure, e.OriginalSource,MainCanvas);
            ExitFromRisuiRegim();
        }

        private void CanvasTest_MouseMove(object sender, MouseEventArgs e)
        {
            Point NewMousePosition = e.GetPosition(MainCanvas);
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (OptionRegim.regim == Regim.RegimDraw)
                {
                    if (ListFigure[IndexFigure].Points.Count > 0)
                    {
                        if (!startDrawing)
                        {
                            MainCanvas.Children.RemoveAt(MainCanvas.Children.Count - 1);
                        }
                        Line line = ListFigure[IndexFigure].GetLine(ListFigure[IndexFigure].PointEnd, e.GetPosition(MainCanvas));
                        line.StrokeThickness = OptionDrawLine.StrokeThickness;
                        line.Stroke = OptionColor.ColorChoosingRec;
                        MainCanvas.Children.Add(line);
                        lastLine = line;
                        startDrawing = false;
                    }
                }
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if(OptionRegim.regim == Regim.RegimKrivaya)
                {
                    MainCanvas.Children.Remove(ListFigure[IndexFigure].NewPointEllipse);
                    MainCanvas.Children.Remove(changedLine);
                    ChangeBezierPoints(ChosenPts, e.GetPosition(MainCanvas));
                    changedLine = GeometryHelper.SetBezier(OptionColor.ColorKrivaya, ChosenPts[0], ChosenPts[1], ChosenPts[2], ChosenPts[3], MainCanvas);
                }

                if (OptionRegim.regim == Regim.RegimDuga)
                {
                    MainCanvas.Children.Remove(ListFigure[IndexFigure].NewPointEllipse);
                    MainCanvas.Children.Remove(changedLine);
                    ChosenPts[2] = e.GetPosition(MainCanvas);
                    changedLine = GeometryHelper.SetArc(OptionColor.ColorChoosingRec, ChosenPts[0], ChosenPts[1], ChosenPts[2], MainCanvas);
                }
                if (OptionRegim.regim == Regim.RegimMovePoints)
                {
                    MainCanvas.Children.Remove(ListFigure[IndexFigure].NewPointEllipse);
                    MainCanvas.Children.Remove(firstRec);
                    Vector delta = e.GetPosition(MainCanvas) - prevPoint;
                    ManipulateShapes(delta);
                    prevPoint = e.GetPosition(MainCanvas);
                }
                if (OptionRegim.regim == Regim.RegimCursorMoveRect)
                {
                    Vector delta = e.GetPosition(MainCanvas) - prevPoint;
                    MoveFigureRectangle(chRec, delta, MainCanvas);
                    foreach (Rectangle rec in movingFigurePoints)
                        MoveFigureRectangle(rec, delta, MainCanvas);
                    prevPoint = e.GetPosition(MainCanvas);
                }
                if (OptionRegim.regim == Regim.RegimEditFigures)
                {
                    if (ChoosingRectangle.Points.Count > 0)
                    {
                        if(!startDrawing)
                        {
                            MainCanvas.Children.Remove(chRec);
                        }
                        chRec = DrawChoosingRectangle(ChoosingRectangle.Points[0], e.GetPosition(MainCanvas), MainCanvas);
                        startDrawing = false;
                    }
                }

                if ((OptionRegim.regim == Regim.RegimTatami || OptionRegim.regim == Regim.RegimGlad) && !startDrawing)
                {
                    if (ControlLine.Points.Count > 1)
                    {
                        MainCanvas.Children.RemoveAt(MainCanvas.Children.Count - 1);
                        ControlLine.Points.RemoveAt(ControlLine.Points.Count - 1);
                    }

                    Line line = ControlLine.GetLine(ControlLine.Points[0], e.GetPosition(MainCanvas));
                    Vector vect1 = new Vector(e.GetPosition(MainCanvas).X - ControlLine.Points[0].X,
                        e.GetPosition(MainCanvas).Y - ControlLine.Points[0].Y);
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
                    line.StrokeThickness = OptionDrawLine.StrokeThickness;
                    line.Stroke = OptionColor.ColorSelection;
                    MainCanvas.Children.Add(line);
                    MainCanvas.UpdateLayout();
                    ControlLine.Points.Add(e.GetPosition(MainCanvas));
                }
                if (OptionRegim.regim == Regim.RegimScaleFigure)
                {
                    MainCanvas.Children.Remove(chRec);
                    MoveScalingRectangle(e.GetPosition(MainCanvas),MainCanvas);

                    ShowPositionStatus(ListFigure[IndexFigure], false, true);
                }
                if (OptionRegim.regim == Regim.RotateFigure)
                {
                    Rectangle centerRect = transRectangles[8];
                    Point centerPoint = new Point(Canvas.GetLeft(centerRect) + centerRect.Width / 2, Canvas.GetTop(centerRect) + centerRect.Width / 2);
                    RotateRotatingRectangle(e.GetPosition(MainCanvas), centerPoint,prevPoint, MainCanvas);
                }
                if(OptionRegim.regim == Regim.RegimChangeRotatingCenter)
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
            if (OptionRegim.regim == Regim.RegimTatami && !startDrawing)
            {
                statusbar3.Content = "";
                if (ControlLine.Points.Count == 1)
                {
                    ControlLine.Points.Add(e.GetPosition(MainCanvas));
                }
                FindControlLine(ListFigure[IndexFigure], ControlLine, MainCanvas,false);
            }
            if (OptionRegim.regim == Regim.RegimGlad && !startDrawing)
            {
                if (ControlLine.Points.Count == 1)
                {
                    ControlLine.Points.Add(e.GetPosition(MainCanvas));
                }
                if (ControlLine.Points[0] != ControlLine.Points[1])
                {
                    Point a = ControlLine.Points[0];
                    Point b = ControlLine.Points[1];
                    FindGladControlLine(a,b, LinesForGlad, ListFigure[FirstGladFigure], ListFigure[SecondGladFigure],false, MainCanvas);
                }
            }
            if (OptionRegim.regim == Regim.RegimCursorMoveRect)
            {
                OptionRegim.regim = Regim.RegimCursor;
                if (chRec.MaxHeight == 99999)
                {
                    MoveFigureToNewPosition(false, null,new Vector());
                    RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
                    DrawOutsideRectangles(true, false, MainCanvas);
                }
                else if (transRectangles[0].Fill == OptionColor.ColorSelection && 
                    (ListFigure[IndexFigure].Points.Count != 1 || ListFigure[IndexFigure].groupFigures.Count != 1))
                    DrawOutsideRectangles(false, false, MainCanvas);
                else
                    DrawOutsideRectangles(true, false, MainCanvas);
            }
            if (OptionRegim.regim == Regim.RotateFigure)
            {
                Rectangle centerRect = transRectangles[8];
                Point centerPoint = new Point(Canvas.GetLeft(centerRect) + centerRect.Width / 2, Canvas.GetTop(centerRect) + centerRect.Width / 2);
                RotateFigure(centerPoint);
                OptionRegim.regim = Regim.RegimCursor;
                RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
                DrawOutsideRectangles(false, true, MainCanvas);
                ShowPositionStatus(ListFigure[IndexFigure], true, false);
            }
            if (OptionRegim.regim == Regim.RegimScaleFigure)
            {
                OptionRegim.regim = Regim.RegimCursor;
                if (MainCanvas.Cursor != NormalCursor)
                {
                    ScaleTransformFigure();
                }
                RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
                DrawOutsideRectangles(true, false, MainCanvas);
                MainCanvas.Cursor = NormalCursor;
            }
            if (OptionRegim.regim == Regim.RegimEditFigures)
            {
                if (ChoosingRectangle.Points.Count > 0)
                {
                    RedrawEverything(ListFigure, IndexFigure, true, MainCanvas);
                    Point UpperLeftCorner = new Point();
                    Point LowerRightCorner = new Point();
                    if(e.GetPosition(MainCanvas).X < ChoosingRectangle.Points[0].X)
                    {
                        UpperLeftCorner.X = e.GetPosition(MainCanvas).X;
                        LowerRightCorner.X = ChoosingRectangle.Points[0].X;
                    }
                    else
                    {
                        UpperLeftCorner.X = ChoosingRectangle.Points[0].X;
                        LowerRightCorner.X = e.GetPosition(MainCanvas).X;
                    }
                    if (e.GetPosition(MainCanvas).Y < ChoosingRectangle.Points[0].Y)
                    {
                        UpperLeftCorner.Y = e.GetPosition(MainCanvas).Y;
                        LowerRightCorner.Y = ChoosingRectangle.Points[0].Y;
                    }
                    else
                    {
                        UpperLeftCorner.Y = ChoosingRectangle.Points[0].Y;
                        LowerRightCorner.Y = e.GetPosition(MainCanvas).Y;
                    }
                    for (int i = 0; i < ListFigure[IndexFigure].Points.Count;i++ )
                    {
                        Point newPoint = ListFigure[IndexFigure].Points[i];
                        if(newPoint.X < LowerRightCorner.X && newPoint.X > UpperLeftCorner.X &&
                            newPoint.Y < LowerRightCorner.Y && newPoint.Y > UpperLeftCorner.Y)
                        {
                            ListFigure[IndexFigure].PointsCount.Add(i);
                        }
                    }
                    ListFigure[IndexFigure].ChangeRectangleColor();
                }
                chRec = new Rectangle();
            }
            if (OptionRegim.regim == Regim.RegimMovePoints)
            {
                AddManipulatedShapes(MainCanvas);
                RedrawEverything(ListFigure, IndexFigure, true, MainCanvas);

                ListFigure[IndexFigure].ChangeRectangleColor();
                listChangedShapes.Clear();
                OptionRegim.regim = Regim.RegimEditFigures;
                ShowPositionStatus(ListFigure[IndexFigure], false, false);
            }
            if(OptionRegim.regim == Regim.RegimChangeRotatingCenter)
            {
                MainCanvas.Cursor = NormalCursor;
                OptionRegim.regim = Regim.RegimCursor;
            }
            if (OptionRegim.regim == Regim.RegimKrivaya || OptionRegim.regim == Regim.RegimDuga)
            {
                if (ChosenPts.Count > 1)
                {
                    if (OptionRegim.regim == Regim.RegimKrivaya)
                        ListFigure[IndexFigure].AddShape(changedLine, ChosenPts[0], new Tuple<Point,Point>(ChosenPts[1],ChosenPts[2]));
                    else
                        ListFigure[IndexFigure].AddShape(changedLine, ChosenPts[0], new Tuple<Point,Point>(e.GetPosition(MainCanvas), new Point()));
                    OptionRegim.regim = Regim.RegimEditFigures;
                    ShowPositionStatus(ListFigure[IndexFigure], false, false);
                }
            }
            startDrawing = true;
        }

        private void CanvasTest_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            if (OptionRegim.regim == Regim.RegimDraw)
            {
                if (ListFigure[IndexFigure].Points.Count > 1)
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
                if (ListFigure[IndexFigure].Points.Count ==1)
                {
                    lastRec.StrokeThickness = OptionDrawLine.StrokeThickness;
                    firstRec = lastRec;
                    if(!startDrawing)
                        MainCanvas.Children.Remove(lastLine);
                }
                Point point = FindClosestDot(e.GetPosition(MainCanvas));
                lastRec = ListFigure[IndexFigure].AddPoint(point, OptionColor.ColorDraw, true,true, OptionDrawLine.SizeWidthAndHeightRectangle);
                ShowPositionStatus(ListFigure[IndexFigure],false,false);
            }
            startDrawing = true;
        }

        void CanvasTest_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)          //левая кнопка мыши
        {
            Mouse.Capture(MainCanvas);
            if ((OptionRegim.regim == Regim.RegimTatami || OptionRegim.regim == Regim.RegimGlad || OptionRegim.regim == Regim.RegimCepochka)
                && e.OriginalSource is Rectangle)
            {
                Rectangle rect = (Rectangle)e.OriginalSource;
                ChooseFirstOrLastRectangle(rect, true, MainCanvas);
            }
            else if (OptionRegim.regim == Regim.RegimTatami)
            {
                bool isNewFigureClicked = ChooseFigureByClicking(e.GetPosition(MainCanvas), ListFigure, e.OriginalSource, MainCanvas);
                if (!isNewFigureClicked)
                {
                    startDrawing = false;
                    ControlLine.Points.Clear();
                    ControlLine.Points.Add(e.GetPosition(MainCanvas));
                }
            }
            else if (OptionRegim.regim == Regim.RegimGlad)
            {
                bool isNewFigureClicked = ChooseFigureByClicking(e.GetPosition(MainCanvas), ListFigure, e.OriginalSource, MainCanvas);
                if (!isNewFigureClicked)
                {
                    if (e.OriginalSource is Shape)
                    {
                        if (e.OriginalSource is Ellipse)
                        {
                            Ellipse ell = (Ellipse)e.OriginalSource;
                            Point ellPoint = new Point(Canvas.GetLeft(ell) + ell.Width/2, Canvas.GetTop(ell) + ell.Height/2);
                            int index = 0;
                            for(int i = 0; i < ListFigure[FirstGladFigure].oldGladCenters.Count; i++)
                            {
                                Point oldCenter = ListFigure[FirstGladFigure].oldGladCenters[i];
                                if(Math.Abs(ellPoint.X - oldCenter.X) < 0.000000001 && Math.Abs(ellPoint.Y - oldCenter.Y) < 0.000000001)
                                {
                                    index = i;
                                    break;
                                }
                            }
                            ListFigure[FirstGladFigure].oldGladCenters.RemoveAt(index);
                            ListFigure[FirstGladFigure].gladControlLines.RemoveAt(index);
                            for (int i = 0; i < LinesForGlad.Count; i++)
                            {
                                if (LinesForGlad[i].Shapes.Contains(ell))
                                {
                                    LinesForGlad[i].RemoveFigure(MainCanvas);
                                    LinesForGlad[i].Shapes.Clear();
                                    LinesForGlad.Remove(LinesForGlad[i]);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        startDrawing = false;
                        ControlLine.Points.Clear();
                        ControlLine.Points.Add(e.GetPosition(MainCanvas));
                    }
                }
            }
            if (OptionRegim.regim == Regim.RegimDraw || OptionRegim.regim == Regim.RegimCepochka)
            {
                ChooseFigureByClicking(e.GetPosition(MainCanvas),ListFigure, e.OriginalSource, MainCanvas);
            }
            if (OptionRegim.regim == Regim.RegimFigure)
            {
                bool isNewFigureClicked = ChooseFigureByClicking(e.GetPosition(MainCanvas), ListFigure, e.OriginalSource, MainCanvas);
                if(!isNewFigureClicked && e.OriginalSource is Rectangle)
                {
                    Rectangle rect = (Rectangle)e.OriginalSource;
                    ChooseFirstOrLastRectangle(rect, true, MainCanvas);
                }
            }
            if (OptionRegim.regim == Regim.RegimEditFigures)
            {
                MainCanvas.Children.Remove(chRec);
                bool isNewFigureClicked = ChooseFigureByClicking(e.GetPosition(MainCanvas),ListFigure, e.OriginalSource, MainCanvas);
                ChosenPts.Clear();
                ChoosingRectangle.Points.Clear();
                if (e.OriginalSource is Line || e.OriginalSource is Path)
                {
                    if (!isNewFigureClicked)
                    {
                        double x2 = 0;
                        double y2 = 0;
                        Shape clickedShape = (Shape)e.OriginalSource;
                        if (clickedShape.StrokeThickness == OptionDrawLine.StrokeThickness)
                        {
                            Shape sha;
                            ListFigure[IndexFigure].DictionaryInvLines.TryGetValue(clickedShape, out sha);
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
                        var keyLine = ListFigure[IndexFigure].DictionaryInvLines.FirstOrDefault(x => x.Value == clickedShape);
                        if (keyLine.Key == null)
                            return;
                        if (keyLine.Key.Stroke == OptionColor.ColorKrivaya)
                        {
                            OptionRegim.regim = Regim.RegimKrivaya;
                            var point = ListFigure[IndexFigure].DictionaryPointLines.FirstOrDefault(x => x.Value == keyLine.Key);
                            ListFigure[IndexFigure].DictionaryPointLines.TryGetValue(point.Key, out sh);
                            ChosenPts = PrepareForBezier((Shape)e.OriginalSource, e.GetPosition(MainCanvas), point.Key, new Point(x2,y2));

                            ListFigure[IndexFigure].DeleteShape(sh, point.Key, MainCanvas);
                            changedLine = sh;
                        }
                        if (keyLine.Key.Stroke == OptionColor.ColorChoosingRec)
                        {
                            OptionRegim.regim = Regim.RegimDuga;
                            var point = ListFigure[IndexFigure].DictionaryPointLines.FirstOrDefault(x => x.Value == keyLine.Key);
                            ListFigure[IndexFigure].DictionaryPointLines.TryGetValue(point.Key, out sh);
                            ListFigure[IndexFigure].DeleteShape(sh, point.Key, MainCanvas);
                            changedLine = sh;
                            ChosenPts.Add(point.Key);
                            ChosenPts.Add(new Point(x2, y2));
                            ChosenPts.Add(new Point());
                        }
                    }
                    else
                        ListFigure[IndexFigure].PointsCount.Clear();
                }
                else if (e.OriginalSource is Rectangle)
                {
                    if (!isNewFigureClicked)
                    {
                        firstRec = (Rectangle)e.OriginalSource;
                        double x = Canvas.GetLeft(firstRec) + firstRec.ActualHeight / 2;
                        double y = Canvas.GetTop(firstRec) + firstRec.ActualWidth / 2;
                        int index = ListFigure[IndexFigure].Points.IndexOf(new Point(x, y));
                        if(!ListFigure[IndexFigure].PointsCount.Contains(index))
                        {
                            ListFigure[IndexFigure].PointsCount.Clear();
                            ListFigure[IndexFigure].PointsCount.Add(index);
                            ListFigure[IndexFigure].ChangeRectangleColor();
                        }
                        string status;
                        Rectangle rec1 = new Rectangle();
                        Rectangle rec2 = new Rectangle();
                        Tuple<Point, Point> contPts = new Tuple<Point, Point>(new Point(), new Point());
                        for (int i = 0; i < ListFigure[IndexFigure].Points.Count - 1; i++)
                        {
                            Point p = ListFigure[IndexFigure].Points[i];
                            
                            if (!ListFigure[IndexFigure].PointsCount.Contains(i) && ListFigure[IndexFigure].PointsCount.Contains(i + 1))
                            {
                                status = "second";
                                rec2 = ListFigure[IndexFigure].RectangleOfFigures[i + 1];
                            }
                            else if (ListFigure[IndexFigure].PointsCount.Contains(i) && ListFigure[IndexFigure].PointsCount.Contains(i + 1))
                            {
                                status = "both";
                                rec1 = ListFigure[IndexFigure].RectangleOfFigures[i];
                                rec2 = ListFigure[IndexFigure].RectangleOfFigures[i + 1];
                            }
                            else if (ListFigure[IndexFigure].PointsCount.Contains(i) && !ListFigure[IndexFigure].PointsCount.Contains(i + 1))
                            {
                                rec1 = ListFigure[IndexFigure].RectangleOfFigures[i];
                                status = "first";
                            }
                            else
                                continue;
                            ListFigure[IndexFigure].DictionaryShapeControlPoints.TryGetValue(p, out contPts);
                            if (contPts == null)
                                contPts = new Tuple<Point, Point>(new Point(), new Point());
                            Shape sh;
                            ListFigure[IndexFigure].DictionaryPointLines.TryGetValue(p, out sh);
                            ChangedShape chShape = new ChangedShape(sh, status, p, contPts.Item1, contPts.Item2, ListFigure[IndexFigure].Points[i + 1],
                                rec1,rec2, MainCanvas);
                            ListFigure[IndexFigure].DeleteShape(sh, p, MainCanvas);
                            listChangedShapes.Add(chShape);
                        }
                        if (ListFigure[IndexFigure].Points.Count == 1 && ListFigure[IndexFigure].PointsCount.Contains(0))
                        {
                            Point p = ListFigure[IndexFigure].Points[0];
                            rec1 = ListFigure[IndexFigure].RectangleOfFigures[0];
                            status = "single";
                            Shape sh;
                            ListFigure[IndexFigure].DictionaryPointLines.TryGetValue(p, out sh);
                            ChangedShape chShape = new ChangedShape(sh, status, p, contPts.Item1, contPts.Item2, new Point(),
                                rec1, rec2, MainCanvas);
                            ListFigure[IndexFigure].DeleteShape(sh, p, MainCanvas);
                            listChangedShapes.Add(chShape);
                        }
                        prevPoint = e.GetPosition(MainCanvas);
                        OptionRegim.regim = Regim.RegimMovePoints;
                    }
                }
                else
                {
                    ListFigure[IndexFigure].PointsCount.Clear();
                    ChoosingRectangle.Points.Add(e.GetPosition(MainCanvas));
                }
            }
            if (OptionRegim.regim == Regim.RegimCursor)
            {
                bool isNewFigureClicked = ChooseFigureByClicking(e.GetPosition(MainCanvas), ListFigure, e.OriginalSource, MainCanvas);
                if (e.OriginalSource is Line || e.OriginalSource is Path)
                {
                    if (!isNewFigureClicked)
                    {
                        foreach (Rectangle rec in transRectangles)
                            MainCanvas.Children.Remove(rec);
                        prevPoint = e.GetPosition(MainCanvas);
                        InitializeFigureRectangle(0);
                        OptionRegim.regim = Regim.RegimCursorMoveRect;
                    }
                    else
                        DrawOutsideRectangles(true, false, MainCanvas);
                }
                if (e.OriginalSource is Rectangle && (((Rectangle)e.OriginalSource).Width == OptionDrawLine.SizeRectangleForRotation ||
                    ((Rectangle)e.OriginalSource).Width == OptionDrawLine.SizeRectangleForScale) && 
                    (ListFigure[IndexFigure].Points.Count != 1 || ListFigure[IndexFigure].groupFigures.Count > 1))
                {
                    Rectangle rec = (Rectangle)e.OriginalSource;
                    if (rec.Fill == OptionColor.ColorSelection)
                    {
                        string[] statuses = { "north", "northeast", "east", "southeast", "south", "southwest", "west", "northwest" };
                        int index = transRectangles.IndexOf(rec);
                        InitializeScaling(statuses[index]);
                        OptionRegim.regim = Regim.RegimScaleFigure;
                        foreach (Rectangle rect in transRectangles)
                            MainCanvas.Children.Remove(rect);
                    }
                    else
                    {
                        if (transRectangles.IndexOf(rec) == 8)
                        {
                            OptionRegim.regim = Regim.RegimChangeRotatingCenter;
                            MainCanvas.Cursor = Cursors.Cross;   
                        }
                        else
                        {
                            OptionRegim.regim = Regim.RotateFigure;
                            foreach (Rectangle rect in transRectangles)
                                MainCanvas.Children.Remove(rect);
                            InitializeFigureRectangle(0);
                            prevPoint = e.GetPosition(MainCanvas);
                        }
                    }
                }
            }
            if(OptionRegim.regim == Regim.ZoomIn)
            {
                SaveLastView();
                Point currentPosition = e.GetPosition(this);
                if(OptionSetka.Masshtab <= 16)
                    ScaleCanvas(2, currentPosition, MainCanvas);
                else if (OptionSetka.Masshtab > 16 && OptionSetka.Masshtab <32)
                {
                    double multiplier = 32 / OptionSetka.Masshtab;
                    ScaleCanvas(multiplier, currentPosition, MainCanvas);
                }
                else
                    MoveCanvas(currentPosition, MainCanvas);
                SetGrid();
                OptionRegim.regim = prevRegim;
                MainCanvas.Cursor = prevCursor;
            }
            if (OptionRegim.regim == Regim.ZoomOut)
            {
                SaveLastView();
                Point currentPosition = e.GetPosition(this);
                if (OptionSetka.Masshtab >= 0.5)
                    ScaleCanvas(0.5, currentPosition, MainCanvas);
                else if (OptionSetka.Masshtab > 0.5 && OptionSetka.Masshtab < 0.25)
                {
                    double multiplier = 0.25 / OptionSetka.Masshtab;
                    ScaleCanvas(multiplier, currentPosition, MainCanvas);
                }
                else
                    MoveCanvas(currentPosition, MainCanvas);
                SetGrid();
                OptionRegim.regim = prevRegim;
                MainCanvas.Cursor = prevCursor;
            }
            if (OptionRegim.regim == Regim.MoveCanvas)
            {
                SaveLastView();
                Point currentPosition = e.GetPosition(this);
                MoveCanvas(currentPosition, MainCanvas);
                SetGrid();
                OptionRegim.regim = prevRegim;
                MainCanvas.Cursor = prevCursor;
            }
            if (OptionRegim.regim == Regim.OneToOne)
            {
                SaveLastView();
                double multiplier = 2.1 / OptionSetka.Masshtab;
                Point currentPosition = e.GetPosition(this);
                ScaleCanvas(multiplier, currentPosition, MainCanvas);
                SetGrid();
                OptionRegim.regim = prevRegim;
                MainCanvas.Cursor = prevCursor;
            }
            ExitFromRisuiRegim();
        }
    }
}
