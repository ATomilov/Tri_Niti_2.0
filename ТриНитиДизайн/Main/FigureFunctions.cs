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
        public void RedrawScreen(List<Figure> listFig, int index, Canvas canvas)
        {
        //    canvas.Children.Clear();
        //    gridFigure.AddFigure(canvas);
        //    foreach (Line ln in centerLines)
        //        canvas.Children.Add(ln);
        //    if (OptionMode.mode == Mode.modeCursor)
        //        DrawFirstAndLastRectangle();
        //    for (int i = FigureList.Count -1 ; i >=0; i--)
        //    {
        //        FigureList[i].AddFigure(canvas);                        //можно не перерисовывать каждый раз
        //        if (AllRectanglesOn && i == ChosenFigure)
        //        {
        //            FigureList[i].DrawAllRectangles();
        //        }
        //    }
        //    if (OptionMode.mode == Mode.modeFigure || OptionMode.mode == Mode.modeTatami || OptionMode.mode == Mode.modeRunStitch
        //        || OptionMode.mode == Mode.modeSatin)
        //    {
        //        DrawInvisibleRectangles(canvas);
        //    }
        //    for (int i = 0; i < listPltFigure.Count; i++)
        //    {
        //        listPltFigure[i].AddFigure(canvas);
        //    }

            canvas.Children.Clear();
            canvas.Children.Add(gridBMP);
            WriteableBitmap bmp = BitmapFactory.New(1600, 900);
            bmp.Clear(Colors.Transparent);
            Color lineColor;
            for (int i = 0; i < listFig.Count; i++)
            {
                if (listFig[i].points.Count > 0)
                {
                    if (i == index)
                        lineColor = OptionColor.colorActive;
                    else
                        lineColor = OptionColor.colorInactive;
                    List<Point> pts = listFig[i].points;
                    for (int j = 0; j < pts.Count - 1; j++)
                    {
                        bmp.DrawLineBresenham((int)pts[j].X, (int)pts[j].Y, (int)pts[j + 1].X, (int)pts[j + 1].Y, lineColor);
                    }
                }
            }
            DrawModeRectangles(listFig, bmp);
            mainBMP = new Image
            {
                Stretch = Stretch.None,
                Source = bmp
            };
            canvas.Children.Add(mainBMP);
        }

        public void DrawLine(Point p1, Point p2, Canvas canvas)
        {
            canvas.Children.Remove(transparentBMP);
            WriteableBitmap bmp = BitmapFactory.New(1600, 900);
            bmp.Clear(Colors.Transparent);
            bmp.DrawLineBresenham((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, OptionColor.colorDrawing);
            transparentBMP = new Image
            {
                Stretch = Stretch.None,
                Source = bmp
            };
            canvas.Children.Add(transparentBMP);
        }

        public void DrawModeRectangles(List<Figure> listFig, WriteableBitmap bmp)
        {
            if(OptionMode.mode == Mode.modeDraw)
            {
                Point start = listFig[indexFigure].pointStart;
                Point end = listFig[indexFigure].pointEnd;
                DrawRectangle(bmp, start, 6, 1);
                DrawRectangle(bmp, end, 8, 2);
            }
            if(OptionMode.mode == Mode.modeEditPoints)
            {
                List<Point> pts = listFig[indexFigure].points;
                for(int i = 0; i < pts.Count; i++)
                {
                    DrawRectangle(bmp, pts[i], 6, 1);
                }
            }
        }

        public void DrawRectangle(WriteableBitmap bmp, Point p, int size, int thickness)
        {
            int half = size / 2;
            for (int i = 0; i < thickness; i++)
            {
                bmp.DrawRectangle((int)p.X - half, (int)p.Y - half, (int)p.X + half, (int)p.Y + half,
                    OptionColor.colorInactive);
                half--;
            }
        }

        //public void DrawFirstAndLastRectangle()
        //{
        //    if (listFigure[indexFigure].points.Count == 0)
        //        return;
        //    mainCanvas.Children.Remove(lastRec);
        //    mainCanvas.Children.Remove(firstRec);
        //    bool ismodeCursor = false;
        //    double mainThickness = OptionDrawLine.strokeThicknessMainRec;
        //    Point start = listFigure[indexFigure].pointStart;
        //    Point end = listFigure[indexFigure].pointEnd;
        //    if (OptionMode.mode == Mode.modeCursor)
        //    {
        //        ismodeCursor = true;
        //        mainThickness = OptionDrawLine.strokeThickness;
        //        start = listFigure[indexFigure].groupFigures[0].pointStart;
        //        end = listFigure[indexFigure].groupFigures[listFigure[indexFigure].groupFigures.Count - 1].pointEnd;
        //    }
        //    firstRec = GeometryHelper.DrawRectangle(start, false, ismodeCursor,
        //        OptionDrawLine.strokeThickness, OptionColor.colorOpacity, mainCanvas);
        //    lastRec = GeometryHelper.DrawRectangle(end, false, false,
        //        mainThickness, OptionColor.colorOpacity, mainCanvas);
        //}

        //public void DrawInvisibleRectangles(Canvas canvas)
        //{
        //    if (OptionMode.mode == Mode.modeSatin)
        //    {
        //        invisibleRectangles = new List<Rectangle>();
        //        Rectangle rec;
        //        List<Point> firstPts = new List<Point>();
        //        List<Point> secondPts = new List<Point>();
        //        if (listFigure[firstSatinFigure].tempPoints.Count > 0)
        //            firstPts = listFigure[firstSatinFigure].tempPoints;
        //        else
        //            firstPts = listFigure[firstSatinFigure].points;

        //        if (listFigure[secondSatinFigure].tempPoints.Count > 0)
        //            secondPts = listFigure[secondSatinFigure].tempPoints;
        //        else
        //            secondPts = listFigure[secondSatinFigure].points;

        //        for (int i = 0; i < firstPts.Count; i++)
        //        {
        //            rec = GeometryHelper.DrawRectangle(firstPts[i], true, false, OptionDrawLine.strokeThickness, OptionColor.colorOpacity, canvas);
        //            invisibleRectangles.Add(rec);
        //        }
        //        for (int i = 0; i < secondPts.Count; i++)
        //        {
        //            rec = GeometryHelper.DrawRectangle(secondPts[i], true, false, OptionDrawLine.strokeThickness, OptionColor.colorOpacity, canvas);
        //            invisibleRectangles.Add(rec);
        //        }
        //    }
        //    else
        //    {
        //        invisibleRectangles = new List<Rectangle>();
        //        Rectangle rec;
        //        List<Point> pts = new List<Point>();
        //        if (listFigure[indexFigure].tempPoints.Count > 0)
        //            pts = listFigure[indexFigure].tempPoints;
        //        else
        //            pts = listFigure[indexFigure].points;

        //        for (int i = 0; i < pts.Count; i++)
        //        {
        //            rec = GeometryHelper.DrawRectangle(pts[i], true, false, OptionDrawLine.strokeThickness, OptionColor.colorOpacity, canvas);
        //            invisibleRectangles.Add(rec);
        //        }
        //    }
        //}

        //public void BreakFigureOrMakeJoinedFigure(List<Figure> FigureList, Object clickedShape, Canvas canvas)
        //{
        //    if (clickedShape is Line || clickedShape is Path)
        //    {
        //        Point p = GetPointOfClickedLine((Shape)clickedShape, FigureList);
        //        if (listFigure[indexFigure].points.Count > 0)
        //        {
        //            for (int i = 0; i < listFigure.Count; i++)
        //            {
        //                if (OptionMode.mode == Mode.modeFigure)
        //                {
        //                    if (i != indexFigure)
        //                    {
        //                        if (listFigure[i].DictionaryPointLines.ContainsKey(p) == true)
        //                        {
        //                            firstSatinFigure = indexFigure;
        //                            secondSatinFigure = i;
        //                            listFigure[i].ChangeFigureColor(OptionColor.colorArc, false);
        //                            ShowJoinGladMessage(linesForSatin, listFigure[indexFigure], listFigure[secondSatinFigure], mainCanvas);
        //                            break;
        //                        }
        //                    }
        //                }
        //                else if (OptionMode.mode == Mode.modeCursor)
        //                {
        //                    if (listFigure[i].DictionaryPointLines.ContainsKey(p) == true)
        //                    {
        //                        if (listFigure[indexFigure].groupFigures.Count > 1)
        //                        {
        //                            foreach (Figure fig in listFigure[indexFigure].groupFigures)
        //                            {
        //                                if (listFigure.IndexOf(fig) == i)
        //                                {
        //                                    ShowBreakCursorMessage(fig, mainCanvas);
        //                                    return;
        //                                }
        //                            }
        //                        }
        //                        if (i != indexFigure)
        //                            ShowJoinCursorMessage(listFigure[indexFigure], listFigure[i], mainCanvas);
        //                        break;
        //                    }
        //                }
        //                if (OptionMode.mode == Mode.modeSatin)
        //                {
        //                    if (i == firstSatinFigure || i == secondSatinFigure)
        //                    {
        //                        if (listFigure[i].DictionaryPointLines.ContainsKey(p) == true)
        //                        {
        //                            listFigure[firstSatinFigure].ChangeFigureColor(OptionColor.colorArc, false);
        //                            listFigure[secondSatinFigure].ChangeFigureColor(OptionColor.colorArc, false);
        //                            ShowBreakMessage();
        //                            break;
        //                        }
        //                    }
        //                }
        //                if (OptionMode.mode == Mode.modeTatami)
        //                {
        //                    if (i == indexFigure)
        //                    {
        //                        if (listFigure[i].DictionaryPointLines.ContainsKey(p) == true)
        //                        {
        //                            listFigure[i].ChangeFigureColor(OptionColor.colorArc, false);
        //                            ShowBreakMessage();
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //public void ShowBreakMessage()
        //{
        //    string sMessageBoxText = "Разорвать?";
        //    string sCaption = "Окно";

        //    MessageBoxButton btnMessageBox = MessageBoxButton.OKCancel;

        //    MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox);

        //    switch (rsltMessageBox)
        //    {
        //        case MessageBoxResult.OK:
        //            {
        //                if (OptionMode.mode == Mode.modeSatin)
        //                {
        //                    OptionMode.mode = Mode.modeFigure;
        //                    listFigure[firstSatinFigure].modeFigure = Mode.modeFigure;
        //                    listFigure[secondSatinFigure].modeFigure = Mode.modeFigure;
        //                    listFigure[firstSatinFigure].LoadCurrentShapes();
        //                    listFigure[secondSatinFigure].LoadCurrentShapes();
        //                    listFigure[firstSatinFigure].groupFigures.Clear();
        //                    listFigure[firstSatinFigure].groupFigures.Add(listFigure[firstSatinFigure]);
        //                    listFigure[secondSatinFigure].groupFigures.Clear();
        //                    listFigure[secondSatinFigure].groupFigures.Add(listFigure[secondSatinFigure]);
        //                    ChangeFiguresColor(listFigure, mainCanvas);
        //                    firstSatinFigure = -1;
        //                    secondSatinFigure = -1;
        //                }
        //                else
        //                {
        //                    OptionMode.mode = Mode.modeFigure;
        //                    listFigure[indexFigure].modeFigure = Mode.modeFigure;
        //                    listFigure[indexFigure].LoadCurrentShapes();
        //                }
        //                listFigure[indexFigure].ChangeFigureColor(OptionColor.colorActive, false);
        //                RedrawEverything(listFigure, indexFigure, false, mainCanvas);
        //                DrawFirstAndLastRectangle();
        //                listFigure[indexFigure].DrawDots(listFigure[indexFigure].points, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, mainCanvas);
        //                ShowPositionStatus(listFigure[indexFigure], false, false);
        //                break;
        //            }

        //        case MessageBoxResult.Cancel:
        //            {
        //                ChangeFiguresColor(listFigure, mainCanvas);
        //                break;
        //            }
        //    }
        //}

        //public void ChooseFirstOrLastRectangle(Rectangle clickedRec, bool isFirstRectangle, Canvas canvas)
        //{
        //    if (isFirstRectangle)
        //    {
        //        if (Canvas.GetLeft(clickedRec) == Canvas.GetLeft(lastRec) && Canvas.GetTop(clickedRec) == Canvas.GetTop(lastRec))
        //        {
        //            Point pfirstRec = new Point(Canvas.GetLeft(firstRec) + firstRec.Width / 2, Canvas.GetTop(firstRec) + firstRec.Height / 2);
        //            Point pLastRec = new Point(Canvas.GetLeft(lastRec) + lastRec.Width / 2, Canvas.GetTop(lastRec) + lastRec.Height / 2);
        //            canvas.Children.Remove(lastRec);
        //            canvas.Children.Remove(firstRec);
        //            firstRec = GeometryHelper.DrawRectangle(pLastRec, false, false, OptionDrawLine.strokeThickness, OptionColor.colorOpacity, canvas);
        //            lastRec = GeometryHelper.DrawRectangle(pfirstRec, false, false, OptionDrawLine.strokeThicknessMainRec, OptionColor.colorOpacity, canvas);
        //        }
        //        else
        //        {
        //            int index = invisibleRectangles.IndexOf(clickedRec);
        //            if (index != -1)
        //            {
        //                firstRec.Opacity = 0;
        //                invisibleRectangles[index] = firstRec;
        //                firstRec = clickedRec;
        //                firstRec.Opacity = 1;
        //                firstRec.StrokeThickness = OptionDrawLine.strokeThickness;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (Canvas.GetLeft(clickedRec) == Canvas.GetLeft(firstRec) && Canvas.GetTop(clickedRec) == Canvas.GetTop(firstRec))
        //        {
        //            Point pfirstRec = new Point(Canvas.GetLeft(firstRec) + firstRec.Width / 2, Canvas.GetTop(firstRec) + firstRec.Height / 2);
        //            Point pLastRec = new Point(Canvas.GetLeft(lastRec) + lastRec.Width / 2, Canvas.GetTop(lastRec) + lastRec.Height / 2);
        //            canvas.Children.Remove(lastRec);
        //            canvas.Children.Remove(firstRec);
        //            firstRec = GeometryHelper.DrawRectangle(pLastRec, false, false, OptionDrawLine.strokeThickness, OptionColor.colorOpacity, canvas);
        //            lastRec = GeometryHelper.DrawRectangle(pfirstRec, false, false, OptionDrawLine.strokeThicknessMainRec, OptionColor.colorOpacity, canvas);
        //        }
        //        else
        //        {
        //            int index = invisibleRectangles.IndexOf(clickedRec);
        //            if (index != -1)
        //            {
        //                lastRec.Opacity = 0;
        //                invisibleRectangles[index] = lastRec;
        //                lastRec = clickedRec;
        //                lastRec.Opacity = 1;
        //                lastRec.StrokeThickness = OptionDrawLine.strokeThicknessMainRec;
        //            }
        //        }
        //    }
        //}

        public bool ChooseFigureByClicking(Point clickedP, List<Figure> listFig, Canvas canvas)
        {
            listFig.Add(new Figure(canvas));
            indexFigure++;
            RedrawScreen(listFig, indexFigure, canvas);
            return false;
        //    if (clickedShape is Rectangle)
        //    {
        //        Rectangle rect = (Rectangle)clickedShape;
        //        double x = Canvas.GetLeft(rect) + rect.Width / 2;
        //        double y = Canvas.GetTop(rect) + rect.Width / 2;
        //        if (new Point(x, y) == listFigure[indexFigure].pointStart)
        //        {
        //            if (OptionMode.mode == Mode.modeDraw && listFigure[indexFigure].points.Count != 1)
        //            {
        //                ReverseFigure(listFigure[indexFigure], canvas);
        //                RedrawEverything(FigureList, indexFigure, false, canvas);
        //                DrawFirstAndLastRectangle();
        //            }
        //            else if (OptionMode.mode == Mode.modeCursor)
        //            {
        //                WindowColors window = new WindowColors();
        //                window.ShowDialog();
        //            }
        //        }
        //    }
        //    else if (clickedShape is Line || clickedShape is Path)
        //    {
        //        Point p = GetPointOfClickedLine((Shape)clickedShape, FigureList);
        //        for (int i = 0; i < FigureList.Count; i++)
        //        {
        //            if (FigureList[i].DictionaryPointLines.ContainsKey(p) == true)
        //            {
        //                if (OptionMode.mode != Mode.modeCursor)
        //                {
        //                    if (indexFigure == i)
        //                    {
        //                        if (OptionMode.mode != Mode.modeFigure && OptionMode.mode != Mode.modeEditPoints &&
        //                            OptionMode.mode != Mode.modeTatami && OptionMode.mode != Mode.modeSatin && OptionMode.mode != Mode.modeRunStitch)
        //                        {
        //                            FigureList[indexFigure].ChangeFigureColor(OptionColor.colorInactive, false);
        //                            indexFigure = FigureList.Count - 1;
        //                            RedrawEverything(FigureList, indexFigure, false, canvas);
        //                            ShowPositionStatus(FigureList[indexFigure], false, false);
        //                        }
        //                        else if (OptionMode.mode == Mode.modeEditPoints)
        //                        {
        //                            FigureList[indexFigure].pointForAddingPoints = p;
        //                            canvas.Children.Remove(FigureList[indexFigure].NewPointEllipse);
        //                            FigureList[indexFigure].DrawEllipse(clickedP, OptionColor.colorInactive, OptionDrawLine.sizeEllipseForPoints, canvas, false);
        //                            ShowPositionStatus(FigureList[indexFigure], false, false);
        //                        }
        //                        return false;
        //                    }
        //                    else
        //                    {
        //                        if (OptionMode.mode != Mode.modeEditPoints)
        //                        {
        //                            FigureList[indexFigure].ChangeFigureColor(OptionColor.colorInactive, false);
        //                            indexFigure = i;
        //                            FigureList[indexFigure].ChangeFigureColor(OptionColor.colorActive, false);
        //                            RedrawEverything(FigureList, indexFigure, false, canvas);
        //                            DrawFirstAndLastRectangle();
        //                            ShowPositionStatus(listFigure[indexFigure], false, false);
        //                            if (OptionMode.mode != Mode.modeDraw)
        //                                LoadPreviousmode(false);
        //                        }
        //                        else
        //                        {
        //                            FigureList[indexFigure].ChangeFigureColor(OptionColor.colorInactive, false);
        //                            indexFigure = i;
        //                            FigureList[indexFigure].ChangeFigureColor(OptionColor.colorActive, true);
        //                            FigureList[indexFigure].highlightedPoints.Clear();
        //                            RedrawEverything(FigureList, indexFigure, true, canvas);
        //                            ShowPositionStatus(FigureList[indexFigure], false, false);
        //                        }
        //                        return true;
        //                    }
        //                }
        //                else
        //                {
        //                    foreach (Figure fig in listFigure[indexFigure].groupFigures)
        //                    {
        //                        if (listFigure.IndexOf(fig) == i)
        //                            return false;
        //                    }
        //                    indexFigure = i;
        //                    ChangeFiguresColor(listFigure, mainCanvas);
        //                    RedrawEverything(FigureList, indexFigure, false, canvas);
        //                    ShowPositionStatus(FigureList[indexFigure], true, false);
        //                    return true;
        //                }
        //            }
        //        }
        //        return true;
        //    }
        //    else if (OptionMode.mode != Mode.modeEditPoints && OptionMode.mode != Mode.modeFigure && OptionMode.mode != Mode.modeCursor &&
        //        OptionMode.mode != Mode.modeTatami && OptionMode.mode != Mode.modeSatin && OptionMode.mode != Mode.modeRunStitch)
        //    {
        //        FigureList[indexFigure].ChangeFigureColor(OptionColor.colorInactive, false);
        //        FigureList.Add(new Figure(mainCanvas));
        //        indexFigure = FigureList.Count - 1;
        //        RedrawEverything(FigureList, indexFigure, false, canvas);
        //        ClearStatusBar();
        //    }
        //    return false;
        }

        //private Point GetPointOfClickedLine(Shape clickedShape, List<Figure> FigureList)
        //{
        //    double x = 0;
        //    double y = 0;
        //    if (clickedShape is Line)
        //    {
        //        Line clickedLine = (Line)clickedShape;
        //        for (int i = 0; i < FigureList.Count; i++)
        //        {
        //            if (clickedLine.StrokeThickness == OptionDrawLine.invisibleStrokeThickness)
        //            {
        //                var invLine = FigureList[i].DictionaryInvLines.FirstOrDefault(z => z.Value == clickedLine);
        //                var point = FigureList[i].DictionaryPointLines.FirstOrDefault(z => z.Value == invLine.Key);
        //                if (point.Value != null)
        //                {
        //                    x = point.Key.X;
        //                    y = point.Key.Y;
        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                var point = FigureList[i].DictionaryPointLines.FirstOrDefault(z => z.Value == clickedLine);
        //                x = point.Key.X;
        //                y = point.Key.Y;
        //                if (point.Value != null)
        //                {
        //                    x = point.Key.X;
        //                    y = point.Key.Y;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    else if (clickedShape is Path)
        //    {
        //        Path path = (Path)clickedShape;
        //        for (int i = 0; i < FigureList.Count; i++)
        //        {
        //            if (path.StrokeThickness == OptionDrawLine.invisibleStrokeThickness)
        //            {
        //                var invLine = FigureList[i].DictionaryInvLines.FirstOrDefault(z => z.Value == path);
        //                var point = FigureList[i].DictionaryPointLines.FirstOrDefault(z => z.Value == invLine.Key);
        //                if (point.Value != null)
        //                {
        //                    x = point.Key.X;
        //                    y = point.Key.Y;
        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                var point = FigureList[i].DictionaryPointLines.FirstOrDefault(z => z.Value == path);
        //                x = point.Key.X;
        //                y = point.Key.Y;
        //                if (point.Value != null)
        //                {
        //                    x = point.Key.X;
        //                    y = point.Key.Y;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    return new Point(x, y);
        //}

        //public void ChangeFiguresColor(List<Figure> FigureList, Canvas canvas)
        //{
        //    for (int i = 0; i < listFigure.Count; i++)
        //    {
        //        listFigure[i].ChangeFigureColor(OptionColor.colorInactive, false);
        //    }
        //    FigureList[indexFigure].ChangeFigureColor(OptionColor.colorActive, false);
        //    if (OptionMode.mode == Mode.modeSatin)
        //    {
        //        FigureList[firstSatinFigure].ChangeFigureColor(OptionColor.colorActive, false);
        //        FigureList[secondSatinFigure].ChangeFigureColor(OptionColor.colorActive, false);
        //    }
        //    else if (OptionMode.mode == Mode.modeEditPoints)
        //    {
        //        FigureList[indexFigure].ChangeFigureColor(OptionColor.colorActive, true);
        //    }
        //}

        //public Figure Cepochka(Figure figure, double step, bool placeOnCanvas, Canvas canvas)
        //{
        //    Shape pathCepochka = SetSpline(0.01, figure.points);
        //    Figure resultFigure = new Figure(canvas);
        //    Path path = (Path)pathCepochka;
        //    PathGeometry myPathGeometry = (PathGeometry)path.Data;
        //    double distance = 0;
        //    foreach (var f in myPathGeometry.Figures)
        //        foreach (var s in f.Segments)
        //            if (s is PolyLineSegment)
        //            {
        //                PointCollection pts = ((PolyLineSegment)s).Points;
        //                for (int i = 0; i < pts.Count - 1; i++)
        //                    distance += FindLength(pts[i], pts[i + 1]);
        //            }
        //    int steps = Convert.ToInt32(distance / step);
        //    Point p;
        //    Point tg;
        //    for (double j = 0; j <= steps; j++)
        //    {
        //        myPathGeometry.GetPointAtFractionLength(j / steps, out p, out tg);
        //        if (placeOnCanvas)
        //            resultFigure.AddPoint(p, OptionColor.colorActive, false, false, 0);
        //        else
        //            resultFigure.points.Add(p);
        //    }
        //    return resultFigure;
        //}

        //public void ExitFromRisuimode()
        //{
        //    if (OptionMode.mode == Mode.modeRisui)
        //    {
        //        listFigure = tempListFigure.ToList<Figure>();
        //        indexFigure = tempIndexFigure;
        //        linesForSatin = tempSatinLines.ToList<Figure>();
        //        RedrawEverything(listFigure, indexFigure, false, mainCanvas);
        //        if (OptionMode.mode != Mode.modeCursor)
        //            DrawFirstAndLastRectangle();
        //        LoadPreviousmode(true);
        //        DrawInvisibleRectangles(mainCanvas);
        //    }
        //    else if (OptionMode.mode == Mode.modeDrawStitchesInColor || OptionMode.mode == Mode.modeUnembroid)
        //    {
        //        OptionMode.mode = Mode.modeCursor;
        //        foreach (Figure fig in listFigure[indexFigure].groupFigures)
        //            fig.ChangeFigureColor(OptionColor.colorActive, false);
        //        DrawFirstAndLastRectangle();
        //        foreach (Line ln in unembroidLines)
        //            mainCanvas.Children.Remove(ln);
        //        unembroidLines.Clear();
        //    }
        //    else if (OptionMode.mode == Mode.modeDrawInColor)
        //    {
        //        OptionMode.mode = Mode.modeCursor;
        //        listFigure = tempListFigure.ToList<Figure>();
        //        mainCanvas.Background = OptionColor.colorBackground;
        //        foreach (Figure fig in listFigure[indexFigure].groupFigures)
        //            fig.ChangeFigureColor(OptionColor.colorActive, false);
        //        RedrawEverything(listFigure, indexFigure, false, mainCanvas);
        //        DrawOutsideRectangles(true, false, mainCanvas);
        //    }
        //}

        //public void LoadPreviousmode(bool isRisui)
        //{
        //    OptionMode.mode = listFigure[indexFigure].modeFigure;
        //    if (OptionMode.mode == Mode.modeTatami || OptionMode.mode == Mode.modeRunStitch)
        //    {
        //        if (!isRisui)
        //        {
        //            listFigure[indexFigure].SaveCurrentShapes();
        //            PrepareForTatami(listFigure[indexFigure], true);
        //        }
        //        if(OptionMode.mode == Mode.modeTatami)
        //            InsertFirstControlLine(listFigure[indexFigure], controlLine, mainCanvas, false);
        //    }
        //    if (OptionMode.mode == Mode.modeSatin)
        //    {
        //        firstSatinFigure = listFigure.IndexOf(listFigure[indexFigure].groupFigures[0]);
        //        secondSatinFigure = listFigure.IndexOf(listFigure[indexFigure].groupFigures[1]);
        //        if (!isRisui)
        //        {
        //            listFigure[firstSatinFigure].SaveCurrentShapes();
        //            listFigure[secondSatinFigure].SaveCurrentShapes();
        //            PrepareForTatami(listFigure[firstSatinFigure], true);
        //            PrepareForTatami(listFigure[secondSatinFigure], true);
        //        }
        //        AddFirstSatinLines(linesForSatin, listFigure[firstSatinFigure], listFigure[secondSatinFigure],false, mainCanvas);
        //        RestoreControlLines(linesForSatin, listFigure[firstSatinFigure], listFigure[secondSatinFigure], mainCanvas);
        //        if (listFigure[firstSatinFigure].tempPoints.Count > 0)
        //            listFigure[firstSatinFigure].DrawDots(listFigure[firstSatinFigure].tempPoints, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, mainCanvas);
        //        else
        //            listFigure[firstSatinFigure].DrawDots(listFigure[firstSatinFigure].points, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, mainCanvas);

        //        if (listFigure[secondSatinFigure].tempPoints.Count > 0)
        //            listFigure[secondSatinFigure].DrawDots(listFigure[secondSatinFigure].tempPoints, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, mainCanvas);
        //        else
        //            listFigure[secondSatinFigure].DrawDots(listFigure[secondSatinFigure].points, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, mainCanvas);
        //        ShowPositionStatus(listFigure[indexFigure], true, false);
        //    }
        //    else
        //    {
        //        if (listFigure[indexFigure].tempPoints.Count > 0)
        //            listFigure[indexFigure].DrawDots(listFigure[indexFigure].tempPoints, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, mainCanvas);
        //        else
        //            listFigure[indexFigure].DrawDots(listFigure[indexFigure].points, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, mainCanvas);
        //        ShowPositionStatus(listFigure[indexFigure], false, false);
        //    }
        //    ChangeFiguresColor(listFigure, mainCanvas);
        //}

        //public void ShowPositionStatus(Figure fig, bool groupPos, bool cursorRecActive)
        //{
        //    if(cursorRecActive && fig.points.Count != 1)
        //    {
        //        statusbar2.Content = "dX = " + Convert.ToInt32(chRec.Width*5) + "; dY = " + Convert.ToInt32(chRec.Height*5);
        //        return;
        //    }
        //    double minX = Double.MaxValue;
        //    double minY = Double.MaxValue;
        //    double maxX = 0;
        //    double maxY = 0;
        //    if(groupPos)
        //    {
        //        foreach (Figure figGroup in fig.groupFigures)
        //        {
        //            List<Point> pts = figGroup.GetFourPointsOfOutSideRectangle(0);
        //            if (minX > pts[0].X)
        //                minX = pts[0].X;
        //            if (maxX < pts[2].X)
        //                maxX = pts[2].X;
        //            if (minY > pts[0].Y)
        //                minY = pts[0].Y;
        //            if (maxY < pts[2].Y)
        //                maxY = pts[2].Y;
        //        }
        //    }
        //    else
        //    {
        //        List<Point> pts = fig.GetFourPointsOfOutSideRectangle(0);
        //        if (minX > pts[0].X)
        //            minX = pts[0].X;
        //        if (maxX < pts[2].X)
        //            maxX = pts[2].X;
        //        if (minY > pts[0].Y)
        //            minY = pts[0].Y;
        //        if (maxY < pts[2].Y)
        //            maxY = pts[2].Y;
        //    }
        //    if (fig.points.Count != 0 && OptionMode.mode != Mode.modeRisui)
        //    {
        //        if(OptionMode.mode == Mode.modeCursor)
        //        {
        //            statusbar1.Content = "Группа:" + fig.groupFigures.Count;
        //        }
        //        else
        //        {
        //            if (fig.modeFigure == Mode.modeTatami)
        //                statusbar1.Content = "Татами";
        //            else if (fig.modeFigure == Mode.modeSatin)
        //                statusbar1.Content = "Гладь";
        //            else if (fig.modeFigure == Mode.modeRunStitch)
        //                statusbar1.Content = "Цепочка";
        //            else
        //                statusbar1.Content = "Стежки";
        //        }
        //        statusbar2.Content = "dX = " + Convert.ToInt32((maxX - minX) * 5) + "; dY = " + Convert.ToInt32((maxY - minY) * 5);
        //        statusbar3.Content = "";
        //    }
        //    else if(OptionMode.mode != Mode.modeRisui)
        //    {
        //        statusbar1.Content = "                ";
        //        statusbar2.Content = "                       ";
        //        statusbar3.Content = "";
        //    }
        //    else
        //    {
        //        statusbar1.Content = "                ";
        //        statusbar2.Content = "                       ";
        //        statusbar3.Content = "Стежков = " + fig.points.Count;
        //    }
        //}

        //public void ClearStatusBar()
        //{
        //    statusbar1.Content = "                ";
        //    statusbar2.Content = "                       ";
        //    statusbar3.Content = "";
        //}
    }
}
