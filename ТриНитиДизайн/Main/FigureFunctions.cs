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
        public void RedrawEverything(List<Figure> FigureList,int ChosenFigure, bool AllRectanglesOn, Canvas canvas)
        {
            canvas.Children.Clear();
            SetkaFigure.AddFigure(canvas);
            foreach (Line ln in centerLines)
                canvas.Children.Add(ln);
            if (OptionRegim.regim == Regim.RegimCursor)
                DrawFirstAndLastRectangle();
            for(int i = 0; i < FigureList.Count;i++)
            {
                FigureList[i].AddFigure(canvas);                        //можно не перерисовывать каждый раз
                if (AllRectanglesOn && i == ChosenFigure)
                {
                    FigureList[i].DrawAllRectangles();
                }
            }
            if (OptionRegim.regim == Regim.RegimFigure || OptionRegim.regim == Regim.RegimTatami || OptionRegim.regim == Regim.RegimCepochka
                || OptionRegim.regim == Regim.RegimGlad)
            {
                DrawInvisibleRectangles(canvas);
            }
            for (int i = 0; i < ListPltFigure.Count; i++)
            {
                ListPltFigure[i].AddFigure(canvas);
            }
        }

        public void DrawFirstAndLastRectangle()
        {
            if (ListFigure[IndexFigure].Points.Count == 0)
                return;
            MainCanvas.Children.Remove(lastRec);
            MainCanvas.Children.Remove(firstRec);
            bool isRegimCursor = false;
            double mainThickness = OptionDrawLine.StrokeThicknessMainRec;
            Point start = ListFigure[IndexFigure].PointStart;
            Point end = ListFigure[IndexFigure].PointEnd;
            if (OptionRegim.regim == Regim.RegimCursor)
            {
                isRegimCursor = true;
                mainThickness = OptionDrawLine.StrokeThickness;
                start = ListFigure[IndexFigure].groupFigures[0].PointStart;
                end = ListFigure[IndexFigure].groupFigures[ListFigure[IndexFigure].groupFigures.Count - 1].PointEnd;
            }
            firstRec = GeometryHelper.DrawRectangle(start, false, isRegimCursor, 
                OptionDrawLine.StrokeThickness, OptionColor.ColorOpacity, MainCanvas);
            lastRec = GeometryHelper.DrawRectangle(end, false, false,
                mainThickness, OptionColor.ColorOpacity, MainCanvas);
        }

        public void DrawInvisibleRectangles(Canvas canvas)
        {
            if (OptionRegim.regim == Regim.RegimGlad)
            {
                List<Point> firstPts = new List<Point>();
                List<Point> secondPts = new List<Point>();
                if (ListFigure[FirstGladFigure].tempPoints.Count > 0)
                    firstPts = ListFigure[FirstGladFigure].tempPoints;
                else
                    firstPts = ListFigure[FirstGladFigure].Points;

                if (ListFigure[SecondGladFigure].tempPoints.Count > 0)
                    secondPts = ListFigure[SecondGladFigure].tempPoints;
                else
                    secondPts = ListFigure[SecondGladFigure].Points;

                for (int i = 0; i < firstPts.Count; i++)
                    GeometryHelper.DrawRectangle(firstPts[i], true, false, OptionDrawLine.StrokeThickness, OptionColor.ColorOpacity, canvas);
                for (int i = 0; i < secondPts.Count; i++)
                    GeometryHelper.DrawRectangle(secondPts[i], true, false, OptionDrawLine.StrokeThickness, OptionColor.ColorOpacity, canvas);
            }
            else
            {
                List<Point> pts = new List<Point>();
                if (ListFigure[IndexFigure].tempPoints.Count > 0)
                    pts = ListFigure[IndexFigure].tempPoints;
                else
                    pts = ListFigure[IndexFigure].Points;

                for (int i = 0; i < pts.Count; i++)
                    GeometryHelper.DrawRectangle(pts[i], true, false, OptionDrawLine.StrokeThickness, OptionColor.ColorOpacity, canvas);
            }
        }        
        
        public void BreakFigureOrMakeJoinedFigure(List<Figure> FigureList, Object clickedShape, Canvas canvas)
        {
            if (clickedShape is Line || clickedShape is Path)
            {
                Point p = GetPointOfClickedLine((Shape)clickedShape, FigureList);
                if (ListFigure[IndexFigure].Points.Count > 0)
                {
                    for (int i = 0; i < ListFigure.Count; i++)
                    {
                        if (OptionRegim.regim == Regim.RegimFigure)
                        {
                            if (i != IndexFigure)
                            {
                                if (ListFigure[i].DictionaryPointLines.ContainsKey(p) == true)
                                {
                                    FirstGladFigure = IndexFigure;
                                    SecondGladFigure = i;
                                    ListFigure[i].ChangeFigureColor(OptionColor.ColorChoosingRec, false);
                                    ShowJoinGladMessage(LinesForGlad, ListFigure[IndexFigure], ListFigure[SecondGladFigure], MainCanvas);
                                    break;
                                }
                            }
                        }
                        else if (OptionRegim.regim == Regim.RegimCursor)
                        {
                            if (ListFigure[i].DictionaryPointLines.ContainsKey(p) == true)
                            {
                                if (ListFigure[IndexFigure].groupFigures.Count > 1)
                                {
                                    foreach (Figure fig in ListFigure[IndexFigure].groupFigures)
                                    {
                                        if (ListFigure.IndexOf(fig) == i)
                                        {
                                            ShowBreakCursorMessage(fig, MainCanvas);
                                            return;
                                        }
                                    }
                                }
                                if (i != IndexFigure)
                                    ShowJoinCursorMessage(ListFigure[IndexFigure], ListFigure[i], MainCanvas);
                                break;
                            }
                        }
                        if (OptionRegim.regim == Regim.RegimGlad)
                        {
                            if (i == FirstGladFigure || i == SecondGladFigure)
                            {
                                if (ListFigure[i].DictionaryPointLines.ContainsKey(p) == true)
                                {
                                    ListFigure[FirstGladFigure].ChangeFigureColor(OptionColor.ColorChoosingRec, false);
                                    ListFigure[SecondGladFigure].ChangeFigureColor(OptionColor.ColorChoosingRec, false);
                                    ShowBreakMessage();
                                    break;
                                }
                            }
                        }
                        if (OptionRegim.regim == Regim.RegimTatami)
                        {
                            if (i == IndexFigure)
                            {
                                if (ListFigure[i].DictionaryPointLines.ContainsKey(p) == true)
                                {
                                    ListFigure[i].ChangeFigureColor(OptionColor.ColorChoosingRec, false);
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
                        if (OptionRegim.regim == Regim.RegimGlad)
                        {
                            OptionRegim.regim = Regim.RegimFigure;
                            ListFigure[FirstGladFigure].regimFigure = Regim.RegimFigure;
                            ListFigure[SecondGladFigure].regimFigure = Regim.RegimFigure;
                            ListFigure[FirstGladFigure].LoadCurrentShapes();
                            ListFigure[SecondGladFigure].LoadCurrentShapes();
                            ChangeFiguresColor(ListFigure, MainCanvas);
                            FirstGladFigure = -1;
                            SecondGladFigure = -1;
                        }
                        else
                        {
                            OptionRegim.regim = Regim.RegimFigure;
                            ListFigure[IndexFigure].regimFigure = Regim.RegimFigure;
                            ListFigure[IndexFigure].LoadCurrentShapes();
                        }
                        ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
                        RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
                        DrawFirstAndLastRectangle();
                        ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].Points, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                        ShowPositionStatus(ListFigure[IndexFigure], false, false);
                        break;
                    }

                case MessageBoxResult.Cancel:
                    {
                        ChangeFiguresColor(ListFigure, MainCanvas);
                        break;
                    }
            }
        }

        public void ChooseFirstOrLastRectangle(Rectangle clickedRec, bool isFirstRectangle, Canvas canvas)
        {
            if(isFirstRectangle)
            {
                if (Canvas.GetLeft(clickedRec) == Canvas.GetLeft(lastRec) && Canvas.GetTop(clickedRec) == Canvas.GetTop(lastRec))
                {
                    Point pfirstRec = new Point(Canvas.GetLeft(firstRec) + firstRec.Width / 2, Canvas.GetTop(firstRec) + firstRec.Height / 2);
                    Point pLastRec = new Point(Canvas.GetLeft(lastRec) + lastRec.Width / 2, Canvas.GetTop(lastRec) + lastRec.Height / 2);
                    canvas.Children.Remove(lastRec);
                    canvas.Children.Remove(firstRec);
                    firstRec = GeometryHelper.DrawRectangle(pLastRec, false, false, OptionDrawLine.StrokeThickness, OptionColor.ColorOpacity, canvas);
                    lastRec = GeometryHelper.DrawRectangle(pfirstRec, false, false, OptionDrawLine.StrokeThicknessMainRec, OptionColor.ColorOpacity, canvas);
                }
                else
                {
                    firstRec.Opacity = 0;
                    firstRec = clickedRec;
                    firstRec.Opacity = 1;
                    firstRec.StrokeThickness = OptionDrawLine.StrokeThickness;
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
                    firstRec = GeometryHelper.DrawRectangle(pLastRec, false, false, OptionDrawLine.StrokeThickness, OptionColor.ColorOpacity, canvas);
                    lastRec = GeometryHelper.DrawRectangle(pfirstRec, false, false, OptionDrawLine.StrokeThicknessMainRec, OptionColor.ColorOpacity, canvas);
                }
                else
                {
                    lastRec.Opacity = 0;
                    lastRec = clickedRec;
                    lastRec.Opacity = 1;
                    lastRec.StrokeThickness = OptionDrawLine.StrokeThicknessMainRec;
                }
            }
        }

        public bool ChooseFigureByClicking(Point clickedP, List<Figure> FigureList, Object clickedShape, Canvas canvas)
        {
            if(clickedShape is Rectangle)
            {
                Rectangle rect = (Rectangle)clickedShape;
                double x = Canvas.GetLeft(rect) + rect.Width / 2;
                double y = Canvas.GetTop(rect) + rect.Width / 2;
                if (new Point(x, y) == ListFigure[IndexFigure].PointStart)
                {
                    if (OptionRegim.regim == Regim.RegimDraw && ListFigure[IndexFigure].Points.Count != 1)
                    {
                        ReverseFigure(ListFigure[IndexFigure], canvas);
                        RedrawEverything(FigureList, IndexFigure, false, canvas);
                        DrawFirstAndLastRectangle();
                    }
                    else if (OptionRegim.regim == Regim.RegimCursor)
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
                        if (OptionRegim.regim != Regim.RegimCursor)
                        {
                            if (IndexFigure == i)
                            {
                                if (OptionRegim.regim != Regim.RegimFigure && OptionRegim.regim != Regim.RegimEditFigures && 
                                    OptionRegim.regim != Regim.RegimTatami && OptionRegim.regim != Regim.RegimGlad && OptionRegim.regim != Regim.RegimCepochka)
                                {
                                    FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection, false);
                                    IndexFigure = FigureList.Count - 1;
                                    RedrawEverything(FigureList, IndexFigure, false, canvas);
                                }
                                else if (OptionRegim.regim == Regim.RegimEditFigures)
                                {
                                    FigureList[IndexFigure].PointForAddingPoints = p;
                                    canvas.Children.Remove(FigureList[IndexFigure].NewPointEllipse);
                                    FigureList[IndexFigure].DrawEllipse(clickedP, OptionColor.ColorSelection, OptionDrawLine.SizeEllipseForPoints, canvas, false);
                                }
                                ShowPositionStatus(FigureList[IndexFigure], false,false);
                                return false;
                            }
                            else
                            {
                                if (OptionRegim.regim != Regim.RegimEditFigures)
                                {
                                    FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection, false);
                                    IndexFigure = i;
                                    FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
                                    RedrawEverything(FigureList, IndexFigure, false, canvas);
                                    DrawFirstAndLastRectangle();
                                    if (OptionRegim.regim != Regim.RegimDraw)
                                        LoadPreviousRegim(false);
                                    if (OptionRegim.regim == Regim.RegimFigure)
                                        ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].Points, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                                }
                                else
                                {
                                    FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection, false);
                                    IndexFigure = i;
                                    FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, true);
                                    FigureList[IndexFigure].PointsCount.Clear();
                                    RedrawEverything(FigureList, IndexFigure, true, canvas);
                                }
                                ShowPositionStatus(FigureList[IndexFigure], false,false);
                                return true;
                            }
                        }
                        else
                        {
                            foreach(Figure fig in ListFigure[IndexFigure].groupFigures)
                            {
                                if (ListFigure.IndexOf(fig) == i)
                                    return false;
                            }
                            IndexFigure = i;
                            ChangeFiguresColor(ListFigure, MainCanvas);
                            RedrawEverything(FigureList, IndexFigure, false, canvas);
                            ShowPositionStatus(FigureList[IndexFigure], true,false);
                            return true;
                        }
                    }
                }
                return true;
            }
            else if (OptionRegim.regim != Regim.RegimEditFigures && OptionRegim.regim != Regim.RegimFigure && OptionRegim.regim != Regim.RegimCursor &&
                OptionRegim.regim != Regim.RegimTatami && OptionRegim.regim != Regim.RegimGlad && OptionRegim.regim != Regim.RegimCepochka)
            {
                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection,false);
                FigureList.Add(new Figure(MainCanvas));
                IndexFigure = FigureList.Count - 1;
                RedrawEverything(FigureList, IndexFigure, false, canvas);
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
                    if (clickedLine.StrokeThickness == 10)
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
                    if (path.StrokeThickness == 10)
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
            for (int i = 0; i < ListFigure.Count; i ++ )
            {
                ListFigure[i].ChangeFigureColor(OptionColor.ColorSelection, false);
            }
            FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
            if (OptionRegim.regim == Regim.RegimGlad)
            {
                FigureList[FirstGladFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
                FigureList[SecondGladFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
            }
            else if (OptionRegim.regim == Regim.RegimEditFigures)
            {
                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, true);
            }    
        }

        public Figure Cepochka(Figure figure, double step, Canvas canvas)
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
                        for (int i = 0; i < pts.Count - 1;i++)
                            distance += FindLength(pts[i], pts[i + 1]);
                    }
            int steps = Convert.ToInt32(distance / step);
            Point p;
            Point tg;
            for (double j = 0; j <= steps; j++)
            {
                myPathGeometry.GetPointAtFractionLength(j / steps, out p, out tg);
                resultFigure.AddPoint(p, OptionColor.ColorDraw, false,false, 0);
            }
            return resultFigure;
        }

        public void ExitFromRisuiRegim()
        {
            if (OptionRegim.regim == Regim.RegimRisui)
            {
                ListFigure = TempListFigure.ToList<Figure>();
                IndexFigure = TempIndexFigure;
                LinesForGlad = TempLinesForGlad.ToList<Figure>();
                RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
                if(OptionRegim.regim != Regim.RegimCursor)
                    DrawFirstAndLastRectangle();
                LoadPreviousRegim(true);
                DrawInvisibleRectangles(MainCanvas);
                if(OptionRegim.regim == Regim.RegimGlad)
                {
                    ListFigure[FirstGladFigure].DrawDots(ListFigure[FirstGladFigure].tempPoints, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                    ListFigure[SecondGladFigure].DrawDots(ListFigure[SecondGladFigure].tempPoints, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                }
                else
                {
                    ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].tempPoints, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                }
            }
            else if(OptionRegim.regim == Regim.RegimDrawStegki || OptionRegim.regim == Regim.RegimOtshit)
            {
                OptionRegim.regim = Regim.RegimCursor;
                foreach(Figure fig in ListFigure[IndexFigure].groupFigures)
                    fig.ChangeFigureColor(OptionColor.ColorDraw, false);
                DrawFirstAndLastRectangle();
                foreach(Line ln in otshitLines)
                    MainCanvas.Children.Remove(ln);
                otshitLines.Clear();
            }
            else if(OptionRegim.regim == Regim.RegimDrawInColor)
            {
                OptionRegim.regim = Regim.RegimCursor;
                ListFigure = TempListFigure.ToList<Figure>();
                MainCanvas.Background = OptionColor.ColorBackground;
                foreach (Figure fig in ListFigure[IndexFigure].groupFigures)
                    fig.ChangeFigureColor(OptionColor.ColorDraw, false);
                RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
                DrawOutsideRectangles(true, false, MainCanvas);
            }
        }

        public void LoadPreviousRegim(bool isRisui)
        {
            OptionRegim.regim = ListFigure[IndexFigure].regimFigure;
            if (OptionRegim.regim == Regim.RegimTatami)
            {
                if (!isRisui)
                {
                    ListFigure[IndexFigure].SaveCurrentShapes();
                    PrepareForTatami(ListFigure[IndexFigure], true);
                }
                InsertFirstControlLine(ListFigure[IndexFigure], ControlLine, MainCanvas, false);
            }
            if (OptionRegim.regim == Regim.RegimGlad)
            {
                FirstGladFigure = ListFigure.IndexOf(ListFigure[IndexFigure].groupFigures[0]);
                SecondGladFigure = ListFigure.IndexOf(ListFigure[IndexFigure].groupFigures[1]);
                if (!isRisui)
                {
                    ListFigure[FirstGladFigure].SaveCurrentShapes();
                    ListFigure[SecondGladFigure].SaveCurrentShapes();
                    PrepareForTatami(ListFigure[FirstGladFigure], true);
                    PrepareForTatami(ListFigure[SecondGladFigure], true);
                }
                AddFirstGladLines(LinesForGlad, ListFigure[FirstGladFigure], ListFigure[SecondGladFigure], MainCanvas);
                if (ListFigure[FirstGladFigure].tempPoints.Count > 0)
                    ListFigure[FirstGladFigure].DrawDots(ListFigure[FirstGladFigure].tempPoints, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                else
                    ListFigure[FirstGladFigure].DrawDots(ListFigure[FirstGladFigure].Points, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);

                if (ListFigure[SecondGladFigure].tempPoints.Count > 0)
                    ListFigure[SecondGladFigure].DrawDots(ListFigure[SecondGladFigure].tempPoints, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                else
                    ListFigure[SecondGladFigure].DrawDots(ListFigure[SecondGladFigure].Points, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
            }
            else
            {
                if (ListFigure[IndexFigure].tempPoints.Count > 0)
                    ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].tempPoints, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                else
                    ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].Points, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
            }
            ChangeFiguresColor(ListFigure, MainCanvas);
            ShowPositionStatus(ListFigure[IndexFigure], false, false);
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
            if (fig.Points.Count != 0 && OptionRegim.regim != Regim.RegimRisui)
            {
                if(OptionRegim.regim == Regim.RegimCursor)
                {
                    statusbar1.Content = "Группа:" + fig.groupFigures.Count;
                }
                else
                {
                    if (fig.regimFigure == Regim.RegimTatami)
                        statusbar1.Content = "Татами";
                    else if (fig.regimFigure == Regim.RegimGlad)
                        statusbar1.Content = "Гладь";
                    else if (fig.regimFigure == Regim.RegimCepochka)
                        statusbar1.Content = "Цепочка";
                    else
                        statusbar1.Content = "Стежки";
                }
                statusbar2.Content = "dX = " + Convert.ToInt32((maxX - minX) * 5) + "; dY = " + Convert.ToInt32((maxY - minY) * 5);
                statusbar3.Content = "";
            }
            else if(OptionRegim.regim != Regim.RegimRisui)
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
