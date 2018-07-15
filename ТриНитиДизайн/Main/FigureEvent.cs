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
        private void FigureMainButtonEvent(object sender, RoutedEventArgs e)
        {
            //if (OptionMode.mode == Mode.modeFigure || OptionMode.mode == Mode.modeTatami ||
            //    OptionMode.mode == Mode.modeSatin || OptionMode.mode == Mode.modeRunStitch)
            //    return;
            //if(OptionMode.mode == Mode.modeCursor)
            //{
            //    indexFigure = listFigure.IndexOf(listFigure[indexFigure].groupFigures[0]);
            //}
            //ExitFromRisuimode();
            //Edit_Menu.IsEnabled = false;
            //listFigure[indexFigure].highlightedPoints.Clear();
            //RedrawEverything(listFigure, indexFigure, false, mainCanvas);
            //LoadPreviousmode(false);
            //DrawFirstAndLastRectangle();
            //DrawInvisibleRectangles(mainCanvas);            
            //chosenPts = new List<Point>();
            //CloseAllTabs();
            //mainCanvas.Cursor = swordCursor;   
            //if (tabControl2.Visibility == Visibility.Hidden)
            //    tabControl2.Visibility = Visibility.Visible;
        }

        private void ChepochkaButtonEvent(object sender, RoutedEventArgs e)
        {
            //ExitFromRisuimode();
            //if (listFigure[indexFigure].points.Count > 1)
            //{
            //    bool accepted;
            //    accepted = ShowAcceptMessage(0);
            //    if (accepted)
            //    {
            //        List<Figure> group = new List<Figure>(listFigure[indexFigure].groupFigures);
            //        foreach (Figure fig in group)
            //        {
            //            if (listFigure.IndexOf(fig) != indexFigure)
            //                fig.groupFigures.Remove(listFigure[indexFigure]);
            //        }
            //        listFigure[indexFigure].groupFigures.Clear();
            //        listFigure[indexFigure].groupFigures.Add(listFigure[indexFigure]);

            //        OptionMode.mode = Mode.modeRunStitch;
            //        listFigure[indexFigure].modeFigure = Mode.modeRunStitch;
            //        var CepochkaSetting = new View.Cepochka();
            //        CepochkaSetting.Owner = this;
            //        CepochkaSetting.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            //        CepochkaSetting.ShowDialog();
            //        if (!listFigure[indexFigure].preparedForTatami)
            //        {
            //            listFigure[indexFigure].SaveCurrentShapes();
            //            PrepareForTatami(listFigure[indexFigure],true);
            //        }
            //        ShowPositionStatus(listFigure[indexFigure], false, false);
            //    }
            //}
        }

        private void GladButtonEvent(object sender, RoutedEventArgs e)
        {
            //ExitFromRisuimode();
            //if (linesForSatin.Count > 0)
            //{
            //    var GladSetting = new View.Satin();
            //    GladSetting.Owner = this;
            //    GladSetting.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            //    GladSetting.ShowDialog();
            //}
        }

        private void TatamiButtonEvent(object sender, RoutedEventArgs e)
        {
            //ExitFromRisuimode();
            //if (listFigure[indexFigure].points.Count > 1)
            //{
            //    bool accepted;
            //    accepted = ShowAcceptMessage(1);
            //    if (accepted)
            //    {
            //        List<Figure> group = new List<Figure>(listFigure[indexFigure].groupFigures);
            //        foreach (Figure fig in group)
            //        {
            //            if (listFigure.IndexOf(fig) != indexFigure)
            //                fig.groupFigures.Remove(listFigure[indexFigure]);
            //        }
            //        listFigure[indexFigure].groupFigures.Clear();
            //        listFigure[indexFigure].groupFigures.Add(listFigure[indexFigure]);
            //        Tatami TatamiWindow = new Tatami();
            //        TatamiWindow.ShowDialog();
            //        if (OptionMode.mode == Mode.modeSatin)
            //        {
            //            Figure newFig = new Figure(mainCanvas);
            //            for (int i = 0; i < listFigure[firstSatinFigure].points.Count; i++)
            //            {
            //                newFig.AddPoint(listFigure[firstSatinFigure].points[i], OptionColor.colorActive, false,false,
            //                    OptionDrawLine.sizeRectangle);
            //            }
            //            if(areSatinPointsInversed)
            //            {
            //                for(int i = 0; i < listFigure[secondSatinFigure].points.Count;i++)
            //                {
            //                    newFig.AddPoint(listFigure[secondSatinFigure].points[i], OptionColor.colorActive, false,false,
            //                        OptionDrawLine.sizeRectangle);
            //                }
            //            }
            //            else
            //            {
            //                for (int i = listFigure[secondSatinFigure].points.Count - 1; i >= 0; i--)
            //                {
            //                    newFig.AddPoint(listFigure[secondSatinFigure].points[i], OptionColor.colorActive, false,false,
            //                        OptionDrawLine.sizeRectangle);
            //                }
            //            }
            //            listFigure.Remove(listFigure[firstSatinFigure]);
            //            listFigure.Remove(listFigure[secondSatinFigure]);
            //            firstSatinFigure = -1;
            //            secondSatinFigure = -1;
            //            listFigure.Add(newFig);
            //            indexFigure = listFigure.IndexOf(newFig);
            //            listFigure[indexFigure].AddPoint(listFigure[indexFigure].points[0], OptionColor.colorActive, false,
            //                false, OptionDrawLine.sizeRectangle);
            //            RedrawEverything(listFigure, indexFigure, false, mainCanvas);
            //            DrawFirstAndLastRectangle();
            //            OptionMode.mode = Mode.modeTatami;
            //            DrawInvisibleRectangles(mainCanvas);
            //            listFigure[indexFigure].DrawDots(listFigure[indexFigure].points, 
            //                OptionDrawLine.risuiModeDots, OptionColor.colorInactive, mainCanvas);
            //        }
            //        else
            //        {
            //            if (!listFigure[indexFigure].preparedForTatami)
            //            {
            //                listFigure[indexFigure].AddPoint(listFigure[indexFigure].points[0], OptionColor.colorActive, false,
            //                    false, OptionDrawLine.sizeRectangle);
            //                listFigure[indexFigure].SaveCurrentShapes();
            //                PrepareForTatami(listFigure[indexFigure], true);
            //            }
            //        }
            //        OptionMode.mode = Mode.modeTatami;
            //        listFigure[indexFigure].modeFigure = Mode.modeTatami;
            //        InsertFirstControlLine(listFigure[indexFigure], controlLine, mainCanvas,true);
            //        ShowPositionStatus(listFigure[indexFigure], false, false);
            //    }
            //}
        }

        private void StagkiButtonEvent(object sender, RoutedEventArgs e)
        {
            //ExitFromRisuimode();
            //if (OptionMode.mode != Mode.modeFigure)
            //{
            //    bool accepted;
            //    accepted = ShowAcceptMessage(2);
            //    if (accepted)
            //    {
            //        if (OptionMode.mode == Mode.modeTatami && listFigure[indexFigure].points.Count > 1)
            //        {
            //            List<Figure> group = new List<Figure>(listFigure[indexFigure].groupFigures);
            //            foreach(Figure fig in group)
            //            {
            //                if (listFigure.IndexOf(fig) != indexFigure)
            //                    fig.groupFigures.Remove(listFigure[indexFigure]);
            //            }
            //            Line lineCon = (Line)controlLine.Shapes[0];
            //            Point p1 = new Point(lineCon.X1, lineCon.Y1);
            //            Point p2 = new Point(lineCon.X2, lineCon.Y2);
            //            CalculateParallelLines(p1, p2, listFigure[indexFigure], controlFigures, tatamiFigures, mainCanvas);
            //            listFigure[indexFigure].RemoveFigure(mainCanvas);
            //            controlLine.RemoveFigure(mainCanvas);
            //            listFigure.RemoveAt(indexFigure);
            //            //TODO:при добавлении динамического массива фигур переделать снизу
            //            for (int i = 0; i < tatamiFigures.Count; i++)
            //            {
            //                if (tatamiFigures[i].points.Count > 0)
            //                {
            //                    listFigure.Insert(indexFigure, tatamiFigures[i]);
            //                }
            //                else
            //                {
            //                    tatamiFigures.RemoveAt(i);
            //                    i--;
            //                }
            //            }
            //            indexFigure = listFigure.IndexOf(tatamiFigures[0]);
            //            DrawFirstAndLastRectangle();
            //            listFigure[indexFigure].ChangeFigureColor(OptionColor.colorActive, false);
            //            DrawInvisibleRectangles(mainCanvas);
            //            listFigure[indexFigure].DrawDots(listFigure[indexFigure].points, OptionDrawLine.risuiModeDots, OptionColor.colorInactive, mainCanvas);
            //            tatamiFigures.Clear();
            //            OptionMode.mode = Mode.modeFigure;
            //            listFigure[indexFigure].modeFigure = Mode.modeFigure;
            //        }
            //        if (OptionMode.mode == Mode.modeSatin)
            //        {
            //            List<Figure> group = new List<Figure>(listFigure[firstSatinFigure].groupFigures);
            //            foreach (Figure fig in group)
            //            {
            //                if (listFigure.IndexOf(fig) != firstSatinFigure && listFigure.IndexOf(fig) != secondSatinFigure)
            //                {
            //                    fig.groupFigures.Remove(listFigure[firstSatinFigure]);
            //                    fig.groupFigures.Remove(listFigure[secondSatinFigure]);
            //                }
            //            }
            //            CalculateGladLines(listFigure[firstSatinFigure], listFigure[secondSatinFigure], linesForSatin, controlFigures, mainCanvas);
            //            Figure firstFigure = listFigure[firstSatinFigure];
            //            Figure secondFigure = listFigure[secondSatinFigure];
            //            firstSatinFigure = -1;
            //            secondSatinFigure = -1;
            //            firstFigure.RemoveFigure(mainCanvas);
            //            secondFigure.RemoveFigure(mainCanvas);
            //            linesForSatin[0].ChangeFigureColor(OptionColor.colorActive, false);
            //            for (int i = 0; i < linesForSatin.Count; i++)
            //            {
            //                listFigure.Insert(indexFigure, linesForSatin[i]);
            //            }
            //            listFigure.Remove(firstFigure);
            //            listFigure.Remove(secondFigure);
            //            indexFigure = listFigure.IndexOf(linesForSatin[0]);
            //            linesForSatin.Clear();
            //            OptionMode.mode = Mode.modeFigure;
            //            listFigure[indexFigure].modeFigure = Mode.modeFigure;
            //            DrawInvisibleRectangles(mainCanvas);
            //            listFigure[indexFigure].DrawDots(listFigure[indexFigure].points, OptionDrawLine.risuiModeDots, 
            //                OptionColor.colorInactive, mainCanvas);
            //        }
            //        if (OptionMode.mode == Mode.modeRunStitch && listFigure[indexFigure].points.Count > 1)
            //        {
            //            List<Figure> group = new List<Figure>(listFigure[indexFigure].groupFigures);
            //            foreach (Figure fig in group)
            //            {
            //                if (listFigure.IndexOf(fig) != indexFigure)
            //                    fig.groupFigures.Remove(listFigure[indexFigure]);
            //            }
            //            listFigure[indexFigure].RemoveFigure(mainCanvas);
            //            listFigure[indexFigure] = Cepochka(listFigure[indexFigure], OptionRunStitch.lengthStep * 0.2,true, mainCanvas);
            //            OptionMode.mode = Mode.modeFigure;
            //            listFigure[indexFigure].modeFigure = Mode.modeFigure;
            //            DrawInvisibleRectangles(mainCanvas);
            //            listFigure[indexFigure].DrawDots(listFigure[indexFigure].points, OptionDrawLine.risuiModeDots, 
            //                OptionColor.colorInactive, mainCanvas);
            //        }
            //        ShowPositionStatus(listFigure[indexFigure], false, false);
            //    }
            //}
        }

        private void RisuiButtonEvent(object sender, RoutedEventArgs e)
        {
        //    if (OptionMode.mode == Mode.modeTatami && listFigure[indexFigure].points.Count > 1)
        //    {
        //        if (controlLine != null)
        //        {
        //            tempListFigure = listFigure.ToList<Figure>();
        //            tempIndexFigure = indexFigure;
        //            Line lineCon = (Line)controlLine.Shapes[0];
        //            Point p1 = new Point(lineCon.X1, lineCon.Y1);
        //            Point p2 = new Point(lineCon.X2, lineCon.Y2);
        //            CalculateParallelLines(p1, p2, listFigure[indexFigure], controlFigures, tatamiFigures, mainCanvas);
        //            listFigure[indexFigure].RemoveFigure(mainCanvas);
        //            controlLine.RemoveFigure(mainCanvas);
        //            listFigure.RemoveAt(indexFigure);
        //            //TODO:при добавлении динамического массива фигур переделать снизу
        //            for (int i = 0; i < tatamiFigures.Count; i++)
        //            {
        //                if (tatamiFigures[i].points.Count > 0)
        //                {
        //                    tatamiFigures[i].ChangeFigureColor(OptionColor.colorSatin, false);
        //                    listFigure.Insert(indexFigure, tatamiFigures[i]);
        //                }
        //                else
        //                {
        //                    tatamiFigures.RemoveAt(i);
        //                    i--;
        //                }
        //            }
        //            indexFigure = listFigure.IndexOf(tatamiFigures[0]);
        //            Point pfirstRec = new Point(Canvas.GetLeft(firstRec) + firstRec.Width / 2, Canvas.GetTop(firstRec) + firstRec.Height / 2);
        //            Point pLastRec = new Point(Canvas.GetLeft(lastRec) + lastRec.Width / 2, Canvas.GetTop(lastRec) + lastRec.Height / 2);
        //            SetLine(pfirstRec, tatamiFigures[0].pointStart, "blue", mainCanvas);
        //            for (int i = 0; i < tatamiFigures.Count; i++)                                   //два раза перебор одного и того же массива
        //            {
        //                tatamiFigures[i].DrawDots(tatamiFigures[i].points, OptionDrawLine.risuiModeDots, OptionColor.colorActive, mainCanvas);
        //                if (i != tatamiFigures.Count - 1)
        //                    SetLine(tatamiFigures[i].pointEnd, tatamiFigures[i + 1].pointStart, "blue", mainCanvas);
        //            }
        //            SetLine(tatamiFigures[tatamiFigures.Count - 1].pointEnd, pLastRec, "blue", mainCanvas);
        //            tatamiFigures.Clear();
        //            OptionMode.mode = Mode.modeRisui;
        //            ShowPositionStatus(listFigure[indexFigure], false, false);
        //        }
        //    }
        //    if (OptionMode.mode == Mode.modeSatin)
        //    {
        //        tempListFigure = listFigure.ToList<Figure>();
        //        tempIndexFigure = indexFigure;
        //        tempSatinLines = linesForSatin.ToList<Figure>();
        //        CalculateGladLines(listFigure[firstSatinFigure], listFigure[secondSatinFigure], linesForSatin, controlFigures, mainCanvas);
        //        Figure firstFigure = listFigure[firstSatinFigure];
        //        Figure secondFigure = listFigure[secondSatinFigure];
        //        firstFigure.RemoveFigure(mainCanvas);
        //        secondFigure.RemoveFigure(mainCanvas);
        //        for (int i = 0; i < linesForSatin.Count; i++)
        //        {
        //            linesForSatin[i].ChangeFigureColor(OptionColor.colorSatin, false);
        //            listFigure.Insert(indexFigure, linesForSatin[i]);
        //        }
        //        listFigure.Remove(firstFigure);
        //        listFigure.Remove(secondFigure);
        //        indexFigure = listFigure.IndexOf(linesForSatin[0]);
        //        Point pfirstRec = new Point(Canvas.GetLeft(firstRec) + firstRec.Width / 2, Canvas.GetTop(firstRec) + firstRec.Height / 2);
        //        Point pLastRec = new Point(Canvas.GetLeft(lastRec) + lastRec.Width / 2, Canvas.GetTop(lastRec) + lastRec.Height / 2);
        //        SetLine(pfirstRec, linesForSatin[0].pointStart, "blue", mainCanvas);
        //        for (int i = 0; i < linesForSatin.Count; i++)
        //        {
        //            linesForSatin[i].DrawDots(linesForSatin[i].points, OptionDrawLine.risuiModeDots, OptionColor.colorActive, mainCanvas);
        //            if (i != linesForSatin.Count - 1)
        //                SetLine(linesForSatin[i].pointEnd, linesForSatin[i + 1].pointStart, "blue", mainCanvas);
        //        }
        //        SetLine(linesForSatin[linesForSatin.Count - 1].pointEnd, pLastRec, "blue", mainCanvas);
        //        OptionMode.mode = Mode.modeRisui;
        //        ShowPositionStatus(listFigure[indexFigure], false, false);
        //    }
        //    if (OptionMode.mode == Mode.modeRunStitch && listFigure[indexFigure].points.Count > 1)
        //    {
        //        tempListFigure = listFigure.ToList<Figure>();
        //        tempIndexFigure = indexFigure;
        //        listFigure[indexFigure] = Cepochka(listFigure[indexFigure], OptionRunStitch.lengthStep * 0.2,true, mainCanvas);
        //        listFigure[indexFigure].ChangeFigureColor(OptionColor.colorSatin, false);
        //        RedrawEverything(listFigure, indexFigure, false, mainCanvas);
        //        DrawFirstAndLastRectangle();
        //        listFigure[indexFigure].DrawDots(listFigure[indexFigure].points, OptionDrawLine.risuiModeDots, OptionColor.colorActive, mainCanvas);
        //        OptionMode.mode = Mode.modeRisui;
        //        ShowPositionStatus(listFigure[indexFigure], false, false);
        //    }

        }
    }
}
