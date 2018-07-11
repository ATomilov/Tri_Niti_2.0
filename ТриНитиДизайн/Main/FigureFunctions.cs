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
        public void RedrawEverything(List<Figure> FigureList, int ChosenFigure, bool AllRectanglesOn, Canvas canvas)
        {
            canvas.Children.Clear();
            gridFigure.AddFigure(canvas);
            foreach (Line ln in centerLines)
                canvas.Children.Add(ln);
            if (OptionMode.mode == Mode.modeCursor)
                DrawFirstAndLastRectangle();
            for (int i = FigureList.Count -1 ; i >=0; i--)
            {
                FigureList[i].AddFigure(canvas);                        //можно не перерисовывать каждый раз
                if (AllRectanglesOn && i == ChosenFigure)
                {
                    FigureList[i].DrawAllRectangles();
                }
            }
            if (OptionMode.mode == Mode.modeFigure || OptionMode.mode == Mode.modeTatami || OptionMode.mode == Mode.modeRunStitch
                || OptionMode.mode == Mode.modeSatin)
            {
                DrawInvisibleRectangles(canvas);
            }
            for (int i = 0; i < listPltFigure.Count; i++)
            {
                listPltFigure[i].AddFigure(canvas);
            }
        }

        public void DrawFirstAndLastRectangle()
        {
            if (listFigure[indexFigure].Points.Count == 0)
                return;
            MainCanvas.Children.Remove(lastRec);
            MainCanvas.Children.Remove(firstRec);
            bool ismodeCursor = false;
            double mainThickness = OptionDrawLine.strokeThicknessMainRec;
            Point start = listFigure[indexFigure].PointStart;
            Point end = listFigure[indexFigure].PointEnd;
            if (OptionMode.mode == Mode.modeCursor)
            {
                ismodeCursor = true;
                mainThickness = OptionDrawLine.strokeThickness;
                start = listFigure[indexFigure].groupFigures[0].PointStart;
                end = listFigure[indexFigure].groupFigures[listFigure[indexFigure].groupFigures.Count - 1].PointEnd;
            }
            firstRec = GeometryHelper.DrawRectangle(start, false, ismodeCursor,
                OptionDrawLine.strokeThickness, OptionColor.colorOpacity, MainCanvas);
            lastRec = GeometryHelper.DrawRectangle(end, false, false,
                mainThickness, OptionColor.colorOpacity, MainCanvas);
        }

        public void DrawInvisibleRectangles(Canvas canvas)
        {
            if (OptionMode.mode == Mode.modeSatin)
            {
                invisibleRectangles = new List<Rectangle>();
                Rectangle rec;
                List<Point> firstPts = new List<Point>();
                List<Point> secondPts = new List<Point>();
                if (listFigure[firstSatinFigure].tempPoints.Count > 0)
                    firstPts = listFigure[firstSatinFigure].tempPoints;
                else
                    firstPts = listFigure[firstSatinFigure].Points;

                if (listFigure[secondSatinFigure].tempPoints.Count > 0)
                    secondPts = listFigure[secondSatinFigure].tempPoints;
                else
                    secondPts = listFigure[secondSatinFigure].Points;

                for (int i = 0; i < firstPts.Count; i++)
                {
                    rec = GeometryHelper.DrawRectangle(firstPts[i], true, false, OptionDrawLine.strokeThickness, OptionColor.colorOpacity, canvas);
                    invisibleRectangles.Add(rec);
                }
                for (int i = 0; i < secondPts.Count; i++)
                {
                    rec = GeometryHelper.DrawRectangle(secondPts[i], true, false, OptionDrawLine.strokeThickness, OptionColor.colorOpacity, canvas);
                    invisibleRectangles.Add(rec);
                }
            }
            else
            {
                invisibleRectangles = new List<Rectangle>();
                Rectangle rec;
                List<Point> pts = new List<Point>();
                if (listFigure[indexFigure].tempPoints.Count > 0)
                    pts = listFigure[indexFigure].tempPoints;
                else
                    pts = listFigure[indexFigure].Points;

                for (int i = 0; i < pts.Count; i++)
                {
                    rec = GeometryHelper.DrawRectangle(pts[i], true, false, OptionDrawLine.strokeThickness, OptionColor.colorOpacity, canvas);
                    invisibleRectangles.Add(rec);
                }
            }
        }

        public void BreakFigureOrMakeJoinedFigure(List<Figure> FigureList, Object clickedShape, Canvas canvas)
        {
            if (clickedShape is Line || clickedShape is Path)
            {
                Point p = GetPointOfClickedLine((Shape)clickedShape, FigureList);
                if (listFigure[indexFigure].Points.Count > 0)
                {
                    for (int i = 0; i < listFigure.Count; i++)
                    {
                        if (OptionMode.mode == Mode.modeFigure)
                        {
                            if (i != indexFigure)
                            {
                                if (listFigure[i].DictionaryPointLines.ContainsKey(p) == true)
                                {
                                    firstSatinFigure = indexFigure;
                                    secondSatinFigure = i;
                                    listFigure[i].ChangeFigureColor(OptionColor.colorArc, false);
                                    ShowJoinGladMessage(linesForSatin, listFigure[indexFigure], listFigure[secondSatinFigure], MainCanvas);
                                    break;
                                }
                            }
                        }
                        else if (OptionMode.mode == Mode.modeCursor)
                        {
                            if (listFigure[i].DictionaryPointLines.ContainsKey(p) == true)
                            {
                                if (listFigure[indexFigure].groupFigures.Count > 1)
                                {
                                    foreach (Figure fig in listFigure[indexFigure].groupFigures)
                                    {
                                        if (listFigure.IndexOf(fig) == i)
                                        {
                                            ShowBreakCursorMessage(fig, MainCanvas);
                                            return;
                                        }
                                    }
                                }
                                if (i != indexFigure)
                                    ShowJoinCursorMessage(listFigure[indexFigure], listFigure[i], MainCanvas);
                                break;
                            }
                        }
                        if (OptionMode.mode == Mode.modeSatin)
                        {
                            if (i == firstSatinFigure || i == secondSatinFigure)
                            {
                                if (listFigure[i].DictionaryPointLines.ContainsKey(p) == true)
                                {
                                    listFigure[firstSatinFigure].ChangeFigureColor(OptionColor.colorArc, false);
                                    listFigure[secondSatinFigure].ChangeFigureColor(OptionColor.colorArc, false);
                                    ShowBreakMessage();
                                    break;
                                }
                            }
                        }
                        if (OptionMode.mode == Mode.modeTatami)
                        {
                            if (i == indexFigure)
                            {
                                if (listFigure[i].DictionaryPointLines.ContainsKey(p) == true)
                                {
                                    listFigure[i].ChangeFigureColor(OptionColor.colorArc, false);
                                    ShowBreakMessage();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ShowBreakMessage()
        {
            string sMessageBoxText = "Разорвать?";
            string sCaption = "Окно";

            MessageBoxButton btnMessageBox = MessageBoxButton.OKCancel;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.OK:
                    {
                        if (OptionMode.mode == Mode.modeSatin)
                        {
                            OptionMode.mode = Mode.modeFigure;
                            listFigure[firstSatinFigure].modeFigure = Mode.modeFigure;
                            listFigure[secondSatinFigure].modeFigure = Mode.modeFigure;
                            listFigure[firstSatinFigure].LoadCurrentShapes();
                            listFigure[secondSatinFigure].LoadCurrentShapes();
                            listFigure[firstSatinFigure].groupFigures.Clear();
                            listFigure[firstSatinFigure].groupFigures.Add(listFigure[firstSatinFigure]);
                            listFigure[secondSatinFigure].groupFigures.Clear();
                            listFigure[secondSatinFigure].groupFigures.Add(listFigure[secondSatinFigure]);
                            ChangeFiguresColor(listFigure, MainCanvas);
                            firstSatinFigure = -1;
                            secondSatinFigure = -1;
                        }
                        else
                        {
                            OptionMode.mode = Mode.modeFigure;
                            listFigure[indexFigure].modeFigure = Mode.modeFigure;
                            listFigure[indexFigure].LoadCurrentShapes();
                        }
                        listFigure[indexFigure].ChangeFigureColor(OptionColor.colorActive, false);
                        RedrawEverything(listFigure, indexFigure, false, MainCanvas);
                        DrawFirstAndLastRectangle();
                        listFigure[indexFigure].DrawDots(listFigure[indexFigure].Points, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, MainCanvas);
                        ShowPositionStatus(listFigure[indexFigure], false, false);
                        break;
                    }

                case MessageBoxResult.Cancel:
                    {
                        ChangeFiguresColor(listFigure, MainCanvas);
                        break;
                    }
            }
        }

        public void ChooseFirstOrLastRectangle(Rectangle clickedRec, bool isFirstRectangle, Canvas canvas)
        {
            if (isFirstRectangle)
            {
                if (Canvas.GetLeft(clickedRec) == Canvas.GetLeft(lastRec) && Canvas.GetTop(clickedRec) == Canvas.GetTop(lastRec))
                {
                    Point pfirstRec = new Point(Canvas.GetLeft(firstRec) + firstRec.Width / 2, Canvas.GetTop(firstRec) + firstRec.Height / 2);
                    Point pLastRec = new Point(Canvas.GetLeft(lastRec) + lastRec.Width / 2, Canvas.GetTop(lastRec) + lastRec.Height / 2);
                    canvas.Children.Remove(lastRec);
                    canvas.Children.Remove(firstRec);
                    firstRec = GeometryHelper.DrawRectangle(pLastRec, false, false, OptionDrawLine.strokeThickness, OptionColor.colorOpacity, canvas);
                    lastRec = GeometryHelper.DrawRectangle(pfirstRec, false, false, OptionDrawLine.strokeThicknessMainRec, OptionColor.colorOpacity, canvas);
                }
                else
                {
                    int index = invisibleRectangles.IndexOf(clickedRec);
                    if (index != -1)
                    {
                        firstRec.Opacity = 0;
                        invisibleRectangles[index] = firstRec;
                        firstRec = clickedRec;
                        firstRec.Opacity = 1;
                        firstRec.StrokeThickness = OptionDrawLine.strokeThickness;
                    }
                }
            }
            else
            {
                if (Canvas.GetLeft(clickedRec) == Canvas.GetLeft(firstRec) && Canvas.GetTop(clickedRec) == Canvas.GetTop(firstRec))
                {
                    Point pfirstRec = new Point(Canvas.GetLeft(firstRec) + firstRec.Width / 2, Canvas.GetTop(firstRec) + firstRec.Height / 2);
                    Point pLastRec = new Point(Canvas.GetLeft(lastRec) + lastRec.Width / 2, Canvas.GetTop(lastRec) + lastRec.Height / 2);
                    canvas.Children.Remove(lastRec);
                    canvas.Children.Remove(firstRec);
                    firstRec = GeometryHelper.DrawRectangle(pLastRec, false, false, OptionDrawLine.strokeThickness, OptionColor.colorOpacity, canvas);
                    lastRec = GeometryHelper.DrawRectangle(pfirstRec, false, false, OptionDrawLine.strokeThicknessMainRec, OptionColor.colorOpacity, canvas);
                }
                else
                {
                    int index = invisibleRectangles.IndexOf(clickedRec);
                    if (index != -1)
                    {
                        lastRec.Opacity = 0;
                        invisibleRectangles[index] = lastRec;
                        lastRec = clickedRec;
                        lastRec.Opacity = 1;
                        lastRec.StrokeThickness = OptionDrawLine.strokeThicknessMainRec;
                    }
                }
            }
        }

        public bool ChooseFigureByClicking(Point clickedP, List<Figure> FigureList, Object clickedShape, Canvas canvas)
        {
            if (clickedShape is Rectangle)
            {
                Rectangle rect = (Rectangle)clickedShape;
                double x = Canvas.GetLeft(rect) + rect.Width / 2;
                double y = Canvas.GetTop(rect) + rect.Width / 2;
                if (new Point(x, y) == listFigure[indexFigure].PointStart)
                {
                    if (OptionMode.mode == Mode.modeDraw && listFigure[indexFigure].Points.Count != 1)
                    {
                        ReverseFigure(listFigure[indexFigure], canvas);
                        RedrawEverything(FigureList, indexFigure, false, canvas);
                        DrawFirstAndLastRectangle();
                    }
                    else if (OptionMode.mode == Mode.modeCursor)
                    {
                        WindowColors window = new WindowColors();
                        window.ShowDialog();
                    }
                }
            }
            else if (clickedShape is Line || clickedShape is Path)
            {
                Point p = GetPointOfClickedLine((Shape)clickedShape, FigureList);
                for (int i = 0; i < FigureList.Count; i++)
                {
                    if (FigureList[i].DictionaryPointLines.ContainsKey(p) == true)
                    {
                        if (OptionMode.mode != Mode.modeCursor)
                        {
                            if (indexFigure == i)
                            {
                                if (OptionMode.mode != Mode.modeFigure && OptionMode.mode != Mode.modeEditFigures &&
                                    OptionMode.mode != Mode.modeTatami && OptionMode.mode != Mode.modeSatin && OptionMode.mode != Mode.modeRunStitch)
                                {
                                    FigureList[indexFigure].ChangeFigureColor(OptionColor.colorInactive, false);
                                    indexFigure = FigureList.Count - 1;
                                    RedrawEverything(FigureList, indexFigure, false, canvas);
                                    ShowPositionStatus(FigureList[indexFigure], false, false);
                                }
                                else if (OptionMode.mode == Mode.modeEditFigures)
                                {
                                    FigureList[indexFigure].PointForAddingPoints = p;
                                    canvas.Children.Remove(FigureList[indexFigure].NewPointEllipse);
                                    FigureList[indexFigure].DrawEllipse(clickedP, OptionColor.colorInactive, OptionDrawLine.sizeEllipseForPoints, canvas, false);
                                    ShowPositionStatus(FigureList[indexFigure], false, false);
                                }
                                return false;
                            }
                            else
                            {
                                if (OptionMode.mode != Mode.modeEditFigures)
                                {
                                    FigureList[indexFigure].ChangeFigureColor(OptionColor.colorInactive, false);
                                    indexFigure = i;
                                    FigureList[indexFigure].ChangeFigureColor(OptionColor.colorActive, false);
                                    RedrawEverything(FigureList, indexFigure, false, canvas);
                                    DrawFirstAndLastRectangle();
                                    ShowPositionStatus(listFigure[indexFigure], false, false);
                                    if (OptionMode.mode != Mode.modeDraw)
                                        LoadPreviousmode(false);
                                }
                                else
                                {
                                    FigureList[indexFigure].ChangeFigureColor(OptionColor.colorInactive, false);
                                    indexFigure = i;
                                    FigureList[indexFigure].ChangeFigureColor(OptionColor.colorActive, true);
                                    FigureList[indexFigure].highlightedPoints.Clear();
                                    RedrawEverything(FigureList, indexFigure, true, canvas);
                                    ShowPositionStatus(FigureList[indexFigure], false, false);
                                }
                                return true;
                            }
                        }
                        else
                        {
                            foreach (Figure fig in listFigure[indexFigure].groupFigures)
                            {
                                if (listFigure.IndexOf(fig) == i)
                                    return false;
                            }
                            indexFigure = i;
                            ChangeFiguresColor(listFigure, MainCanvas);
                            RedrawEverything(FigureList, indexFigure, false, canvas);
                            ShowPositionStatus(FigureList[indexFigure], true, false);
                            return true;
                        }
                    }
                }
                return true;
            }
            else if (OptionMode.mode != Mode.modeEditFigures && OptionMode.mode != Mode.modeFigure && OptionMode.mode != Mode.modeCursor &&
                OptionMode.mode != Mode.modeTatami && OptionMode.mode != Mode.modeSatin && OptionMode.mode != Mode.modeRunStitch)
            {
                FigureList[indexFigure].ChangeFigureColor(OptionColor.colorInactive, false);
                FigureList.Add(new Figure(MainCanvas));
                indexFigure = FigureList.Count - 1;
                RedrawEverything(FigureList, indexFigure, false, canvas);
                ClearStatusBar();
            }
            return false;
        }

        private Point GetPointOfClickedLine(Shape clickedShape, List<Figure> FigureList)
        {
            double x = 0;
            double y = 0;
            if (clickedShape is Line)
            {
                Line clickedLine = (Line)clickedShape;
                for (int i = 0; i < FigureList.Count; i++)
                {
                    if (clickedLine.StrokeThickness == OptionDrawLine.invisibleStrokeThickness)
                    {
                        var invLine = FigureList[i].DictionaryInvLines.FirstOrDefault(z => z.Value == clickedLine);
                        var point = FigureList[i].DictionaryPointLines.FirstOrDefault(z => z.Value == invLine.Key);
                        if (point.Value != null)
                        {
                            x = point.Key.X;
                            y = point.Key.Y;
                            break;
                        }
                    }
                    else
                    {
                        var point = FigureList[i].DictionaryPointLines.FirstOrDefault(z => z.Value == clickedLine);
                        x = point.Key.X;
                        y = point.Key.Y;
                        if (point.Value != null)
                        {
                            x = point.Key.X;
                            y = point.Key.Y;
                            break;
                        }
                    }
                }
            }
            else if (clickedShape is Path)
            {
                Path path = (Path)clickedShape;
                for (int i = 0; i < FigureList.Count; i++)
                {
                    if (path.StrokeThickness == OptionDrawLine.invisibleStrokeThickness)
                    {
                        var invLine = FigureList[i].DictionaryInvLines.FirstOrDefault(z => z.Value == path);
                        var point = FigureList[i].DictionaryPointLines.FirstOrDefault(z => z.Value == invLine.Key);
                        if (point.Value != null)
                        {
                            x = point.Key.X;
                            y = point.Key.Y;
                            break;
                        }
                    }
                    else
                    {
                        var point = FigureList[i].DictionaryPointLines.FirstOrDefault(z => z.Value == path);
                        x = point.Key.X;
                        y = point.Key.Y;
                        if (point.Value != null)
                        {
                            x = point.Key.X;
                            y = point.Key.Y;
                            break;
                        }
                    }
                }
            }
            return new Point(x, y);
        }

        public void ChangeFiguresColor(List<Figure> FigureList, Canvas canvas)
        {
            for (int i = 0; i < listFigure.Count; i++)
            {
                listFigure[i].ChangeFigureColor(OptionColor.colorInactive, false);
            }
            FigureList[indexFigure].ChangeFigureColor(OptionColor.colorActive, false);
            if (OptionMode.mode == Mode.modeSatin)
            {
                FigureList[firstSatinFigure].ChangeFigureColor(OptionColor.colorActive, false);
                FigureList[secondSatinFigure].ChangeFigureColor(OptionColor.colorActive, false);
            }
            else if (OptionMode.mode == Mode.modeEditFigures)
            {
                FigureList[indexFigure].ChangeFigureColor(OptionColor.colorActive, true);
            }
        }

        public Figure Cepochka(Figure figure, double step, bool placeOnCanvas, Canvas canvas)
        {
            Shape pathCepochka = SetSpline(0.01, figure.Points);
            Figure resultFigure = new Figure(canvas);
            Path path = (Path)pathCepochka;
            PathGeometry myPathGeometry = (PathGeometry)path.Data;
            double distance = 0;
            foreach (var f in myPathGeometry.Figures)
                foreach (var s in f.Segments)
                    if (s is PolyLineSegment)
                    {
                        PointCollection pts = ((PolyLineSegment)s).Points;
                        for (int i = 0; i < pts.Count - 1; i++)
                            distance += FindLength(pts[i], pts[i + 1]);
                    }
            int steps = Convert.ToInt32(distance / step);
            Point p;
            Point tg;
            for (double j = 0; j <= steps; j++)
            {
                myPathGeometry.GetPointAtFractionLength(j / steps, out p, out tg);
                if (placeOnCanvas)
                    resultFigure.AddPoint(p, OptionColor.colorActive, false, false, 0);
                else
                    resultFigure.Points.Add(p);
            }
            return resultFigure;
        }

        public void ExitFromRisuimode()
        {
            if (OptionMode.mode == Mode.modeRisui)
            {
                listFigure = tempListFigure.ToList<Figure>();
                indexFigure = tempIndexFigure;
                linesForSatin = tempSatinLines.ToList<Figure>();
                RedrawEverything(listFigure, indexFigure, false, MainCanvas);
                if (OptionMode.mode != Mode.modeCursor)
                    DrawFirstAndLastRectangle();
                LoadPreviousmode(true);
                DrawInvisibleRectangles(MainCanvas);
            }
            else if (OptionMode.mode == Mode.modeDrawStitchesInColor || OptionMode.mode == Mode.modeUnembroid)
            {
                OptionMode.mode = Mode.modeCursor;
                foreach (Figure fig in listFigure[indexFigure].groupFigures)
                    fig.ChangeFigureColor(OptionColor.colorActive, false);
                DrawFirstAndLastRectangle();
                foreach (Line ln in unembroidLines)
                    MainCanvas.Children.Remove(ln);
                unembroidLines.Clear();
            }
            else if (OptionMode.mode == Mode.modeDrawInColor)
            {
                OptionMode.mode = Mode.modeCursor;
                listFigure = tempListFigure.ToList<Figure>();
                MainCanvas.Background = OptionColor.colorBackground;
                foreach (Figure fig in listFigure[indexFigure].groupFigures)
                    fig.ChangeFigureColor(OptionColor.colorActive, false);
                RedrawEverything(listFigure, indexFigure, false, MainCanvas);
                DrawOutsideRectangles(true, false, MainCanvas);
            }
        }

        public void LoadPreviousmode(bool isRisui)
        {
            OptionMode.mode = listFigure[indexFigure].modeFigure;
            if (OptionMode.mode == Mode.modeTatami || OptionMode.mode == Mode.modeRunStitch)
            {
                if (!isRisui)
                {
                    listFigure[indexFigure].SaveCurrentShapes();
                    PrepareForTatami(listFigure[indexFigure], true);
                }
                if(OptionMode.mode == Mode.modeTatami)
                    InsertFirstControlLine(listFigure[indexFigure], controlLine, MainCanvas, false);
            }
            if (OptionMode.mode == Mode.modeSatin)
            {
                firstSatinFigure = listFigure.IndexOf(listFigure[indexFigure].groupFigures[0]);
                secondSatinFigure = listFigure.IndexOf(listFigure[indexFigure].groupFigures[1]);
                if (!isRisui)
                {
                    listFigure[firstSatinFigure].SaveCurrentShapes();
                    listFigure[secondSatinFigure].SaveCurrentShapes();
                    PrepareForTatami(listFigure[firstSatinFigure], true);
                    PrepareForTatami(listFigure[secondSatinFigure], true);
                }
                AddFirstSatinLines(linesForSatin, listFigure[firstSatinFigure], listFigure[secondSatinFigure],false, MainCanvas);
                RestoreControlLines(linesForSatin, listFigure[firstSatinFigure], listFigure[secondSatinFigure], MainCanvas);
                if (listFigure[firstSatinFigure].tempPoints.Count > 0)
                    listFigure[firstSatinFigure].DrawDots(listFigure[firstSatinFigure].tempPoints, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, MainCanvas);
                else
                    listFigure[firstSatinFigure].DrawDots(listFigure[firstSatinFigure].Points, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, MainCanvas);

                if (listFigure[secondSatinFigure].tempPoints.Count > 0)
                    listFigure[secondSatinFigure].DrawDots(listFigure[secondSatinFigure].tempPoints, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, MainCanvas);
                else
                    listFigure[secondSatinFigure].DrawDots(listFigure[secondSatinFigure].Points, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, MainCanvas);
                ShowPositionStatus(listFigure[indexFigure], true, false);
            }
            else
            {
                if (listFigure[indexFigure].tempPoints.Count > 0)
                    listFigure[indexFigure].DrawDots(listFigure[indexFigure].tempPoints, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, MainCanvas);
                else
                    listFigure[indexFigure].DrawDots(listFigure[indexFigure].Points, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, MainCanvas);
                ShowPositionStatus(listFigure[indexFigure], false, false);
            }
            ChangeFiguresColor(listFigure, MainCanvas);
        }

        public void ShowPositionStatus(Figure fig, bool groupPos, bool cursorRecActive)
        {
            if(cursorRecActive && fig.Points.Count != 1)
            {
                statusbar2.Content = "dX = " + Convert.ToInt32(chRec.Width*5) + "; dY = " + Convert.ToInt32(chRec.Height*5);
                return;
            }
            double minX = Double.MaxValue;
            double minY = Double.MaxValue;
            double maxX = 0;
            double maxY = 0;
            if(groupPos)
            {
                foreach (Figure figGroup in fig.groupFigures)
                {
                    List<Point> pts = figGroup.GetFourPointsOfOutSideRectangle(0);
                    if (minX > pts[0].X)
                        minX = pts[0].X;
                    if (maxX < pts[2].X)
                        maxX = pts[2].X;
                    if (minY > pts[0].Y)
                        minY = pts[0].Y;
                    if (maxY < pts[2].Y)
                        maxY = pts[2].Y;
                }
            }
            else
            {
                List<Point> pts = fig.GetFourPointsOfOutSideRectangle(0);
                if (minX > pts[0].X)
                    minX = pts[0].X;
                if (maxX < pts[2].X)
                    maxX = pts[2].X;
                if (minY > pts[0].Y)
                    minY = pts[0].Y;
                if (maxY < pts[2].Y)
                    maxY = pts[2].Y;
            }
            if (fig.Points.Count != 0 && OptionMode.mode != Mode.modeRisui)
            {
                if(OptionMode.mode == Mode.modeCursor)
                {
                    statusbar1.Content = "Группа:" + fig.groupFigures.Count;
                }
                else
                {
                    if (fig.modeFigure == Mode.modeTatami)
                        statusbar1.Content = "Татами";
                    else if (fig.modeFigure == Mode.modeSatin)
                        statusbar1.Content = "Гладь";
                    else if (fig.modeFigure == Mode.modeRunStitch)
                        statusbar1.Content = "Цепочка";
                    else
                        statusbar1.Content = "Стежки";
                }
                statusbar2.Content = "dX = " + Convert.ToInt32((maxX - minX) * 5) + "; dY = " + Convert.ToInt32((maxY - minY) * 5);
                statusbar3.Content = "";
            }
            else if(OptionMode.mode != Mode.modeRisui)
            {
                statusbar1.Content = "                ";
                statusbar2.Content = "                       ";
                statusbar3.Content = "";
            }
            else
            {
                statusbar1.Content = "                ";
                statusbar2.Content = "                       ";
                statusbar3.Content = "Стежков = " + fig.Points.Count;
            }
        }

        public void ClearStatusBar()
        {
            statusbar1.Content = "                ";
            statusbar2.Content = "                       ";
            statusbar3.Content = "";
        }
    }
}
