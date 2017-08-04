﻿using System;
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
        private void FigureMainButtonEvant(object sender, RoutedEventArgs e)
        {
            ExitFromRisuiRegim();
            Edit_Menu.IsEnabled = false;
            ListFigure[IndexFigure].PointsCount.Clear();
            RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
            LoadPreviousRegim(false);
            DrawFirstAndLastRectangle();
            DrawInvisibleRectangles(MainCanvas);
            if (OptionRegim.regim == Regim.RegimGlad)
            {
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
            ChosenPts = new List<Point>();
            CloseAllTabs();
            MainCanvas.Cursor = SwordCursor;   
            if (tabControl2.Visibility == Visibility.Hidden)
                tabControl2.Visibility = Visibility.Visible;
        }

        private void ChepochkaButtonEvent(object sender, RoutedEventArgs e)
        {
            ExitFromRisuiRegim();
            if (ListFigure[IndexFigure].Points.Count > 0)
            {
                bool accepted;
                accepted = ShowAcceptMessage(0);
                if (accepted)
                {
                    OptionRegim.regim = Regim.RegimCepochka;
                    ListFigure[IndexFigure].regimFigure = Regim.RegimCepochka;
                    var CepochkaSetting = new View.Cepochka();
                    CepochkaSetting.Owner = this;
                    CepochkaSetting.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                    CepochkaSetting.ShowDialog();
                    if (!ListFigure[IndexFigure].PreparedForTatami)
                    {
                        PrepareForTatami(ListFigure[IndexFigure],true);
                    }
                }
            }
        }

        private void GladButtonEvent(object sender, RoutedEventArgs e)
        {
            ExitFromRisuiRegim();
            if (LinesForGlad.Count > 0)
            {
                var GladSetting = new View.Glad();
                GladSetting.Owner = this;
                GladSetting.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                GladSetting.ShowDialog();
            }
        }

        private void TatamiButtonEvent(object sender, RoutedEventArgs e)
        {
            ExitFromRisuiRegim();
            if (ListFigure[IndexFigure].Points.Count > 0)
            {
                bool accepted;
                accepted = ShowAcceptMessage(1);
                if (accepted)
                {
                    Tatami TatamiWindow = new Tatami();
                    TatamiWindow.ShowDialog();
                    if (OptionRegim.regim == Regim.RegimGlad)
                    {
                        Figure newFig = new Figure(MainCanvas);
                        for (int i = 0; i < ListFigure[FirstGladFigure].Points.Count; i++)
                        {
                            newFig.AddPoint(ListFigure[FirstGladFigure].Points[i], OptionColor.ColorDraw, false, OptionDrawLine.SizeWidthAndHeightRectangle);
                        }
                        if(areGladPointsInversed)
                        {
                            for(int i = 0; i < ListFigure[SecondGladFigure].Points.Count;i++)
                            {
                                newFig.AddPoint(ListFigure[SecondGladFigure].Points[i], OptionColor.ColorDraw, false, OptionDrawLine.SizeWidthAndHeightRectangle);
                            }
                        }
                        else
                        {
                            for (int i = ListFigure[SecondGladFigure].Points.Count - 1; i >= 0; i--)
                            {
                                newFig.AddPoint(ListFigure[SecondGladFigure].Points[i], OptionColor.ColorDraw, false, OptionDrawLine.SizeWidthAndHeightRectangle);
                            }
                        }
                        ListFigure.Remove(ListFigure[FirstGladFigure]);
                        ListFigure.Remove(ListFigure[SecondGladFigure]);
                        FirstGladFigure = -1;
                        SecondGladFigure = -1;
                        ListFigure.Add(newFig);
                        IndexFigure = ListFigure.IndexOf(newFig);
                        ListFigure[IndexFigure].AddPoint(ListFigure[IndexFigure].Points[0], OptionColor.ColorDraw, false, OptionDrawLine.SizeWidthAndHeightRectangle);
                        RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
                        DrawFirstAndLastRectangle();
                        OptionRegim.regim = Regim.RegimTatami;
                        DrawInvisibleRectangles(MainCanvas);
                        ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].Points, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                    }
                    else
                    {
                        if (!ListFigure[IndexFigure].PreparedForTatami)
                        {
                            ListFigure[IndexFigure].AddPoint(ListFigure[IndexFigure].Points[0], OptionColor.ColorDraw, false, OptionDrawLine.SizeWidthAndHeightRectangle);
                            ListFigure[IndexFigure].SaveCurrentShapes();
                            PrepareForTatami(ListFigure[IndexFigure], true);
                        }
                    }
                    OptionRegim.regim = Regim.RegimTatami;
                    ListFigure[IndexFigure].regimFigure = Regim.RegimTatami;
                    InsertFirstControlLine(ListFigure[IndexFigure], ControlLine, MainCanvas);
                }
            }
        }

        private void StagkiButtonEvent(object sender, RoutedEventArgs e)
        {
            ExitFromRisuiRegim();
            if (OptionRegim.regim != Regim.RegimFigure)
            {
                bool accepted;
                accepted = ShowAcceptMessage(2);
                if (accepted)
                {
                    if (OptionRegim.regim == Regim.RegimTatami)
                    {
                        Line lineCon = (Line)ControlLine.Shapes[0];
                        Point p1 = new Point(lineCon.X1, lineCon.Y1);
                        Point p2 = new Point(lineCon.X2, lineCon.Y2);
                        CalculateParallelLines(p1, p2, ListFigure[IndexFigure], ControlFigures, TatamiFigures, MainCanvas);
                        ListFigure[IndexFigure].RemoveFigure(MainCanvas);
                        ControlLine.RemoveFigure(MainCanvas);
                        ListFigure.RemoveAt(IndexFigure);
                        //TODO:при добавлении динамического массива фигур переделать снизу
                        for (int i = 0; i < TatamiFigures.Count; i++)
                        {
                            if (TatamiFigures[i].Points.Count > 0)
                            {
                                ListFigure.Insert(IndexFigure, TatamiFigures[i]);
                            }
                            else
                            {
                                TatamiFigures.RemoveAt(i);
                                i--;
                            }
                        }
                        IndexFigure = ListFigure.IndexOf(TatamiFigures[0]);
                        DrawFirstAndLastRectangle();
                        ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
                        DrawInvisibleRectangles(MainCanvas);
                        ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].Points, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                        TatamiFigures.Clear();
                        OptionRegim.regim = Regim.RegimFigure;
                        ListFigure[IndexFigure].regimFigure = Regim.RegimFigure;
                    }
                    if (OptionRegim.regim == Regim.RegimGlad)
                    {
                        CalculateGladLines(ListFigure[FirstGladFigure], ListFigure[SecondGladFigure], LinesForGlad, ControlFigures, MainCanvas);
                        Figure firstFigure = ListFigure[FirstGladFigure];
                        Figure secondFigure = ListFigure[SecondGladFigure];
                        FirstGladFigure = -1;
                        SecondGladFigure = -1;
                        firstFigure.RemoveFigure(MainCanvas);
                        secondFigure.RemoveFigure(MainCanvas);
                        LinesForGlad[0].ChangeFigureColor(OptionColor.ColorDraw, false);
                        for (int i = 0; i < LinesForGlad.Count; i++)
                        {
                            ListFigure.Insert(IndexFigure, LinesForGlad[i]);
                        }
                        ListFigure.Remove(firstFigure);
                        ListFigure.Remove(secondFigure);
                        IndexFigure = ListFigure.IndexOf(LinesForGlad[0]);
                        LinesForGlad.Clear();
                        OptionRegim.regim = Regim.RegimFigure;
                        ListFigure[IndexFigure].regimFigure = Regim.RegimFigure;
                        DrawInvisibleRectangles(MainCanvas);
                        ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].Points, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                    }
                    if (OptionRegim.regim == Regim.RegimCepochka)
                    {
                        ListFigure[IndexFigure] = Cepochka(ListFigure[IndexFigure], OptionCepochka.LenthStep * 0.2, MainCanvas);
                        OptionRegim.regim = Regim.RegimFigure;
                        ListFigure[IndexFigure].regimFigure = Regim.RegimFigure;
                        DrawInvisibleRectangles(MainCanvas);
                        ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].Points, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                    }
                }
            }
        }

        private void RisuiButtonEvent(object sender, RoutedEventArgs e)
        {
            if (OptionRegim.regim == Regim.RegimTatami)
            {
                if (ControlLine != null)
                {
                    TempListFigure = ListFigure.ToList<Figure>();
                    TempIndexFigure = IndexFigure;
                    Line lineCon = (Line)ControlLine.Shapes[0];
                    Point p1 = new Point(lineCon.X1, lineCon.Y1);
                    Point p2 = new Point(lineCon.X2, lineCon.Y2);
                    CalculateParallelLines(p1, p2, ListFigure[IndexFigure], ControlFigures, TatamiFigures, MainCanvas);
                    ListFigure[IndexFigure].RemoveFigure(MainCanvas);
                    ControlLine.RemoveFigure(MainCanvas);
                    ListFigure.RemoveAt(IndexFigure);
                    //TODO:при добавлении динамического массива фигур переделать снизу
                    for (int i = 0; i < TatamiFigures.Count; i++)
                    {
                        if (TatamiFigures[i].Points.Count > 0)
                        {
                            TatamiFigures[i].ChangeFigureColor(OptionColor.ColorGlad, false);
                            ListFigure.Insert(IndexFigure, TatamiFigures[i]);
                        }
                        else
                        {
                            TatamiFigures.RemoveAt(i);
                            i--;
                        }
                    }
                    IndexFigure = ListFigure.IndexOf(TatamiFigures[0]);
                    Point pfirstRec = new Point(Canvas.GetLeft(firstRec) + firstRec.Width / 2, Canvas.GetTop(firstRec) + firstRec.Height / 2);
                    Point pLastRec = new Point(Canvas.GetLeft(lastRec) + lastRec.Width / 2, Canvas.GetTop(lastRec) + lastRec.Height / 2);
                    SetLine(pfirstRec, TatamiFigures[0].PointStart, "blue", MainCanvas);
                    for (int i = 0; i < TatamiFigures.Count; i++)                                   //два раза перебор одного и того же массива
                    {
                        TatamiFigures[i].DrawDots(TatamiFigures[i].Points, OptionDrawLine.RisuiRegimDots, OptionColor.ColorDraw, MainCanvas);
                        if (i != TatamiFigures.Count - 1)
                            SetLine(TatamiFigures[i].PointEnd, TatamiFigures[i + 1].PointStart, "blue", MainCanvas);
                    }
                    SetLine(TatamiFigures[TatamiFigures.Count-1].PointEnd, pLastRec, "blue", MainCanvas);
                    TatamiFigures.Clear();
                    OptionRegim.regim = Regim.RegimRisui;
                }
            }
            if (OptionRegim.regim == Regim.RegimGlad)
            {
                TempListFigure = ListFigure.ToList<Figure>();
                TempIndexFigure = IndexFigure;
                TempLinesForGlad = LinesForGlad.ToList<Figure>();
                CalculateGladLines(ListFigure[FirstGladFigure], ListFigure[SecondGladFigure], LinesForGlad, ControlFigures, MainCanvas);
                Figure firstFigure = ListFigure[FirstGladFigure];
                Figure secondFigure = ListFigure[SecondGladFigure];
                firstFigure.RemoveFigure(MainCanvas);
                secondFigure.RemoveFigure(MainCanvas);
                for (int i = 0; i < LinesForGlad.Count; i++)
                {
                    LinesForGlad[i].ChangeFigureColor(OptionColor.ColorGlad, false);
                    ListFigure.Insert(IndexFigure, LinesForGlad[i]);
                }
                ListFigure.Remove(firstFigure);
                ListFigure.Remove(secondFigure);
                IndexFigure = ListFigure.IndexOf(LinesForGlad[0]);
                Point pfirstRec = new Point(Canvas.GetLeft(firstRec) + firstRec.Width / 2, Canvas.GetTop(firstRec) + firstRec.Height / 2);
                Point pLastRec = new Point(Canvas.GetLeft(lastRec) + lastRec.Width / 2, Canvas.GetTop(lastRec) + lastRec.Height / 2);
                SetLine(pfirstRec, LinesForGlad[0].PointStart, "blue", MainCanvas);
                for (int i = 0; i < LinesForGlad.Count; i++)
                {
                    LinesForGlad[i].DrawDots(LinesForGlad[i].Points, OptionDrawLine.RisuiRegimDots, OptionColor.ColorDraw, MainCanvas);
                    if(i != LinesForGlad.Count - 1)
                        SetLine(LinesForGlad[i].PointEnd, LinesForGlad[i + 1].PointStart, "blue", MainCanvas);
                }
                SetLine(LinesForGlad[LinesForGlad.Count - 1].PointEnd, pLastRec, "blue", MainCanvas);
                OptionRegim.regim = Regim.RegimRisui;
            }
            if (OptionRegim.regim == Regim.RegimCepochka)
            {
                TempListFigure = ListFigure.ToList<Figure>();
                TempIndexFigure = IndexFigure;
                ListFigure[IndexFigure] = Cepochka(ListFigure[IndexFigure], OptionCepochka.LenthStep*0.2, MainCanvas);
                ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorGlad, false);
                RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
                DrawFirstAndLastRectangle();
                ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].Points, OptionDrawLine.RisuiRegimDots, OptionColor.ColorDraw, MainCanvas);
                OptionRegim.regim = Regim.RegimRisui;
            }
        }
    }
}
