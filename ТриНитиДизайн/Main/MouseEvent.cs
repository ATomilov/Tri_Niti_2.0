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
            else if (OptionRegim.regim == Regim.RegimFigure || OptionRegim.regim == Regim.RegimTatami || OptionRegim.regim == Regim.RegimGlad)
                BreakFigureOrMakeGladFigure(ListFigure, e.OriginalSource,MainCanvas);
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
                if (OptionRegim.regim == Regim.RegimMoveRect)
                {
                    MainCanvas.Children.Remove(ListFigure[IndexFigure].NewPointEllipse);
                    MainCanvas.Children.Remove(firstRec);
                    Vector delta = e.GetPosition(MainCanvas) - prevPoint;
                    ManipulateShapes(delta);
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
                if (OptionRegim.regim == Regim.RegimCursor)//поворот при режиме ресайз для проверки корректности перехода в режим ресайз при моей логике (upd: поворот теперь не работает, ибо не переходит в нужный режим)
                {
                    //double dx = NewMousePosition.X - ListFigure[IndexFigure].GetCenter().X;
                    //double dy = NewMousePosition.Y - ListFigure[IndexFigure].GetCenter().Y;
                    //double new_angle = Math.Atan2(dy, dx);
                    //CurrentAngle = new_angle;
                    //CurrentAngle *= 180 / Math.PI;
                    //ListFigure[IndexFigure].Rotate(CurrentAngle);
                    //ListFigure[IndexFigure].ScaleVertical(2, CoordinatesOfTransformRectangles[4]);
                }
                if (OptionRegim.regim == Regim.RotateFigure)//работа с изменением размера
                {
                    //for (int i = 0; i < ListFigure[IndexFigure].Points.Count; i++)
                    //{
                    //    Point CurrentPoint = ListFigure[IndexFigure].Points[i];
                    //    CurrentPoint.Y += NewMousePosition.Y;
                    //    ListFigure[IndexFigure].Points[i] = CurrentPoint;
                    //}
                    //RedrawEverything(ListFigure, IndexFigure, false, false, MainCanvas);
                    //ListFigure[IndexFigure].ScaleVertical(2);
                }
            }
        }

        private void CanvasTest_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            //TotalAngle = CurrentAngle;
            if (OptionRegim.regim == Regim.RegimTatami && !startDrawing)
            {
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
                    FindGladControlLine(ControlLine, LinesForGlad, ListFigure[FirstGladFigure], ListFigure[SecondGladFigure], MainCanvas);
                }
            }
            if (OptionRegim.regim == Regim.RegimCursor)
            {
                CoordinatesOfTransformRectangles = ListFigure[IndexFigure].DrawOutSideRectanglePoints(ListFigure[IndexFigure].angle);
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
            }
            if(OptionRegim.regim == Regim.RegimMoveRect)
            {
                AddManipulatedShapes(MainCanvas);
                RedrawEverything(ListFigure, IndexFigure, true, MainCanvas);
                ListFigure[IndexFigure].ChangeRectangleColor();
                listChangedShapes.Clear();
                OptionRegim.regim = Regim.RegimEditFigures;
            }
            if (OptionRegim.regim == Regim.RegimKrivaya || OptionRegim.regim == Regim.RegimDuga)
            {
                if (ChosenPts.Count > 1)
                {
                    if (OptionRegim.regim == Regim.RegimKrivaya)
                        ListFigure[IndexFigure].AddShape(changedLine, ChosenPts[0], new Tuple<Point,Point>(ChosenPts[1],ChosenPts[2]));
                    else
                        ListFigure[IndexFigure].AddShape(changedLine, ChosenPts[0], new Tuple<Point,Point>(e.GetPosition(MainCanvas), new Point()));
                    ChosenPts.Clear();
                    OptionRegim.regim = Regim.RegimEditFigures;
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
                    if(!startDrawing)
                        MainCanvas.Children.Remove(lastLine);
                }
                Point point = FindClosestDot(e.GetPosition(MainCanvas));
                lastRec = ListFigure[IndexFigure].AddPoint(point, OptionColor.ColorDraw, true, OptionDrawLine.SizeWidthAndHeightRectangle);
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
                startDrawing = false;
                ControlLine.Points.Clear();
                ControlLine.Points.Add(e.GetPosition(MainCanvas));
            }
            else if (OptionRegim.regim == Regim.RegimGlad)
            {
                if (e.OriginalSource is Shape)
                {
                    if(e.OriginalSource is Ellipse)
                    {
                        Ellipse ell = (Ellipse)e.OriginalSource;
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
            if (OptionRegim.regim == Regim.RegimDraw)
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
                        for (int i = 0; i < ListFigure[IndexFigure].Points.Count - 1; i++)
                        {
                            Tuple<Point,Point> contPts = new Tuple<Point,Point>(new Point(), new Point());
                            Point p = ListFigure[IndexFigure].Points[i];
                            Rectangle rec1 = new Rectangle();
                            Rectangle rec2 = new Rectangle();
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
                        
                        prevPoint = e.GetPosition(MainCanvas);
                        OptionRegim.regim = Regim.RegimMoveRect;
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
                CoordinatesOfTransformRectangles = ListFigure[IndexFigure].DrawOutSideRectanglePoints();
                if (e.OriginalSource is Line || e.OriginalSource is Path)
                {
                    //по идее в этом моменте надо менять флаг режима поворота, не снимая выделения с фигуры
                    //isRotateRegim = true;
                    //ChooseFigureByClicking(ListFigure, e.OriginalSource, MainCanvas);

                }
                if (e.OriginalSource is Rectangle && ((Rectangle)e.OriginalSource).Width == OptionDrawLine.SizeRectangleForTransform)
                {
                    Point OldCursorPosition = e.GetPosition(MainCanvas);
                    double dx = OldCursorPosition.X - ListFigure[IndexFigure].GetCenter().X;
                    double dy = OldCursorPosition.Y - ListFigure[IndexFigure].GetCenter().Y;
                    StartAngle = Math.Atan2(dy, dx);
                    Rectangle CurrentRec = (Rectangle)e.OriginalSource;
                    int CurrentIndex = 0;
                    double CurrentRecX, CurrentRecY;
                    CurrentRecX = Canvas.GetLeft(CurrentRec) + ((OptionDrawLine.SizeRectangleForTransform) / 2);
                    CurrentRecY = Canvas.GetTop(CurrentRec) + ((OptionDrawLine.SizeRectangleForTransform) / 2);
                    for (int i = 0; i < CoordinatesOfTransformRectangles.Count; i++)
                    {
                        if (CurrentRecX == CoordinatesOfTransformRectangles[i].X && CurrentRecY == CoordinatesOfTransformRectangles[i].Y)
                        {
                            CurrentIndex = i;
                        }
                    }
                    if (CurrentIndex < 2)
                    {
                        ListFigure[IndexFigure].ScaleVertical(OptionScale.scaleX, OptionScale.scaleY, CoordinatesOfTransformRectangles[CurrentIndex + 2]);
                        //foreach (Figure fig in ListFigure)
                        //{
                        //    foreach (Shape sh in fig.Shapes)
                        //    {
                        //        sh.StrokeThickness /= 0.1;
                        //    }
                        //}
                        //foreach (Figure fig in LinesForGlad)
                        //{
                        //    foreach (Shape sh in fig.Shapes)
                        //    {
                        //        sh.StrokeThickness /= 2;
                        //    }
                        //}
                        //foreach (Shape sh in ControlLine.Shapes)
                        //{
                        //    sh.StrokeThickness /= 2;
                        //}
                    }
                    if (CurrentIndex >= 2 && CurrentIndex <= 3)
                    {
                        ListFigure[IndexFigure].ScaleVertical(OptionScale.scaleX, OptionScale.scaleY, CoordinatesOfTransformRectangles[CurrentIndex - 2]);
                    }
                    if (CurrentIndex == 4)
                    {
                        ListFigure[IndexFigure].ScaleVertical(OptionScale.scaleX, 0, CoordinatesOfTransformRectangles[CurrentIndex + 2]);
                    }
                    if (CurrentIndex == 6)
                    {
                        ListFigure[IndexFigure].ScaleVertical(OptionScale.scaleX, 0, CoordinatesOfTransformRectangles[CurrentIndex - 2]);
                    }
                    if (CurrentIndex == 7)
                    {
                        ListFigure[IndexFigure].ScaleVertical(0, OptionScale.scaleY, CoordinatesOfTransformRectangles[CurrentIndex - 2]);
                    }
                    if (CurrentIndex == 5)
                    {
                        ListFigure[IndexFigure].ScaleVertical(0, OptionScale.scaleY, CoordinatesOfTransformRectangles[CurrentIndex + 2]);
                    }
                    //ListFigure[IndexFigure].ScaleVertical(2, 1, CoordinatesOfTransformRectangles[4]);
                }
                else
                {
                    //isResizeRegim = true;
                    ChooseFigureByClicking(e.GetPosition(MainCanvas),ListFigure, e.OriginalSource, MainCanvas);
                    //CoordinatesOfTransformRectangles = ListFigure[IndexFigure].DrawOutSideRectanglePoints();
                }
            }
            if(OptionRegim.regim == Regim.ZoomIn)
            {
                Point currentPosition = e.GetPosition(MainCanvas);
                if (OptionSetka.Masshtab == 64)
                {
                    PlusWithFixedOptions(MainCanvas, currentPosition, 64, 0.015625, 0.125, 0.15625, 0.15625);
                }
                else
                {
                    Plus(MainCanvas, currentPosition);
                }
            }
            if (OptionRegim.regim == Regim.ZoomOut)
            {
                Point currentPosition = e.GetPosition(MainCanvas);
                if (OptionSetka.Masshtab == 0.25)
                {
                    MinusWithFixedOptions(MainCanvas, currentPosition, 0.25, 4, 32, 40, 40);
                }
                else
                {
                    Minus(MainCanvas, currentPosition);
                }
            }
            ExitFromRisuiRegim();
        }
    }
}
