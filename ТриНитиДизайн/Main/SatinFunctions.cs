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
        //int satinShapesCount = 0;
        //int oldGladHits = 0;

        //public void ShowJoinGladMessage(List<Figure>satinLines, Figure firstFigure, Figure secondFigure, Canvas canvas)
        //{
        //    string sMessageBoxText = "Соединить?";
        //    string sCaption = "Окно";

        //    MessageBoxButton btnMessageBox = MessageBoxButton.OKCancel;

        //    MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox);

        //    switch (rsltMessageBox)
        //    {
        //        case MessageBoxResult.OK:
        //            {
        //                firstFigure.satinControlLines.Clear();
        //                secondFigure.satinControlLines.Clear();
        //                firstFigure.oldSatinCenters.Clear();
        //                secondFigure.oldSatinCenters.Clear();
        //                List<Figure> group = new List<Figure>(firstFigure.groupFigures);
        //                foreach (Figure fig in group)
        //                {
        //                    if (listFigure.IndexOf(fig) != firstSatinFigure && listFigure.IndexOf(fig) != secondSatinFigure)
        //                    {
        //                        fig.groupFigures.Remove(firstFigure);
        //                        fig.groupFigures.Remove(secondFigure);
        //                    }
        //                }
        //                firstFigure.groupFigures.Clear();
        //                firstFigure.groupFigures.Add(firstFigure);
        //                firstFigure.groupFigures.Add(secondFigure);
        //                secondFigure.groupFigures.Clear();
        //                secondFigure.groupFigures.Add(firstFigure);
        //                secondFigure.groupFigures.Add(secondFigure);

        //                OptionMode.mode = Mode.modeSatin;                        
        //                listFigure[indexFigure].modeFigure = Mode.modeSatin;
        //                listFigure[secondSatinFigure].modeFigure = Mode.modeSatin;
        //                controlLine = new Figure(mainCanvas);
        //                AddFirstSatinLines(satinLines, firstFigure, secondFigure,true, canvas);
        //                if (!firstFigure.preparedForTatami)
        //                {
        //                    firstFigure.SaveCurrentShapes();
        //                    PrepareForTatami(firstFigure,true);
        //                }
        //                if (!secondFigure.preparedForTatami)
        //                {
        //                    secondFigure.SaveCurrentShapes();
        //                    PrepareForTatami(secondFigure,true);
        //                }
        //                ShowPositionStatus(listFigure[indexFigure], true, false);
        //                break;
        //            }

        //        case MessageBoxResult.Cancel:
        //            {
        //                listFigure[secondSatinFigure].ChangeFigureColor(OptionColor.colorInactive, false);
        //                break;
        //            }
        //    }
        //}
        
        //private void AddFirstSatinLines(List<Figure> satinLines, Figure firstFigure, Figure secondFigure, bool satinCreated, Canvas canvas)
        //{
        //    satinShapesCount = 0;
        //    satinLines.Clear();
        //    secondFigure.ChangeFigureColor(OptionColor.colorActive,false);
        //    satinLines.Add(new Figure(canvas));
        //    satinLines.Add(new Figure(canvas));
        //    if (CheckForGladIntersection(firstFigure.pointStart, secondFigure.pointStart, firstFigure, secondFigure, true, null))
        //    {
        //        areSatinPointsInversed = true;
        //        satinLines[0].AddPoint(firstFigure.pointStart, OptionColor.colorArc, false,false, 0);
        //        satinLines[0].AddPoint(secondFigure.pointEnd, OptionColor.colorArc, false,false, 0);
        //        satinLines[1].AddPoint(firstFigure.pointEnd, OptionColor.colorArc, false,false, 0);
        //        satinLines[1].AddPoint(secondFigure.pointStart, OptionColor.colorArc, false,false, 0);
        //    }
        //    else
        //    {
        //        areSatinPointsInversed = false;
        //        satinLines[0].AddPoint(firstFigure.pointStart, OptionColor.colorArc, false,false, 0);
        //        satinLines[0].AddPoint(secondFigure.pointStart, OptionColor.colorArc, false,false, 0);
        //        satinLines[1].AddPoint(firstFigure.pointEnd, OptionColor.colorArc, false,false, 0);
        //        satinLines[1].AddPoint(secondFigure.pointEnd, OptionColor.colorArc, false,false, 0);
        //    }
        //    if(satinCreated)
        //        listFigure[secondSatinFigure].DrawDots(listFigure[secondSatinFigure].points, 
        //            OptionDrawLine.risuiModeDots, OptionColor.colorInactive, mainCanvas);
        //    DrawInvisibleRectangles(canvas);
        //}

        //public void RestoreControlLines(List<Figure> satinLines, Figure firstFigure, Figure secondFigure, Canvas canvas)
        //{
        //    for(int i = 0; i < firstFigure.satinControlLines.Count; i++)
        //    {
        //        Vector vect = firstFigure.satinControlLines[i];
        //        Point center = firstFigure.oldSatinCenters[i];
        //        Point p1 = new Point(center.X + vect.X * 500, center.Y + vect.Y * 500);
        //        Point p2 = new Point(center.X - vect.X * 500, center.Y - vect.Y * 500);
        //        FindGladControlLine(p1, p2, satinLines, firstFigure, secondFigure, true, canvas);
        //    }
        //}

        //public void FindGladControlLine(Point a, Point b,List<Figure> satinLines, Figure firstFigure, Figure secondFigure, bool restoreLines, Canvas canvas)
        //{
        //    if(!restoreLines)
        //        canvas.Children.RemoveAt(canvas.Children.Count - 1);
        //    List<Point> pts = new List<Point>();
        //    int hits = 0;
        //    Point c = new Point();
        //    Point d = new Point();
        //    for (int i = 0; i < firstFigure.points.Count - 1; i++)
        //    {
        //        c = firstFigure.points[i];
        //        d = firstFigure.points[i + 1];
        //        hits = FindIntersection(a, b, c, d, pts, hits, false,true);
        //        if(hits == 1)
        //        {
        //            break;
        //        }
        //    }
        //    if (hits == 1)
        //    {
        //        for (int i = 0; i < secondFigure.points.Count - 1; i++)
        //        {
        //            c = secondFigure.points[i];
        //            d = secondFigure.points[i + 1];
        //            hits = FindIntersection(a, b, c, d, pts, hits, false, true);
        //            if (hits == 2)
        //            {
        //                break;
        //            }
        //        }
        //        if (hits == 2)
        //        {
        //            if(!restoreLines)
        //            {
        //                Vector vect = pts[1] - pts[0];
        //                vect.Normalize();
        //                firstFigure.satinControlLines.Add(vect);
        //                secondFigure.satinControlLines.Add(vect);
        //            }
        //            Figure fig = new Figure(canvas);
        //            fig.AddPoint(pts[0], OptionColor.colorArc, false,false, 0);
        //            fig.AddPoint(pts[1], OptionColor.colorArc, false,false, 0);
        //            if (restoreLines)
        //                fig.SetMiddleControlLine(pts[0], pts[1], canvas);
        //            else
        //                firstFigure.oldSatinCenters.Add(fig.SetMiddleControlLine(pts[0], pts[1], canvas));
        //            satinLines.Insert(1, fig);
        //        }
        //    }
        //}

        //private bool CheckForGladIntersection(Point a, Point b, Figure firstFigure, Figure secondFigure, bool firstCheck, List<Figure> ListControlLines)
        //{
        //    List<Point> pts = new List<Point>();
        //    Point c = new Point();
        //    Point d = new Point();

        //    int hits = 0;
        //    if (firstCheck)
        //    {
        //        c = firstFigure.pointEnd;
        //        d = secondFigure.pointEnd;
        //        hits = FindIntersection(a, b, c, d, pts, hits, false, false);
        //    }
        //    else
        //    {
        //        for (int i = 0; i < firstFigure.points.Count - 1; i++)
        //        {
        //            c = firstFigure.points[i];
        //            d = firstFigure.points[i + 1];
        //            hits = FindIntersection(a, b, c, d, pts, hits, false, false);
        //        }
        //        for (int i = 0; i < secondFigure.points.Count - 1; i++)
        //        {
        //            c = secondFigure.points[i];
        //            d = secondFigure.points[i + 1];
        //            hits = FindIntersection(a, b, c, d, pts, hits, false, false);
        //        }
        //        if (hits % 2 == 0)
        //        {
        //            OrganizeGladDots(pts, ListControlLines, b, hits);
        //        }
        //    }
        //    if(hits > 0)
        //        return true;
        //    else
        //        return false;
        //}

        //private int FindIntersection(Point a, Point b, Point c, Point d, List<Point> pts, int hits, bool findInvisibleIntersection,bool findControlLine)
        //{
        //    double A1 = a.Y - b.Y,
        //            B1 = b.X - a.X,
        //            C1 = -A1 * a.X - B1 * a.Y;
        //    double A2 = c.Y - d.Y,
        //            B2 = d.X - c.X,
        //            C2 = -A2 * c.X - B2 * c.Y;
        //    double zn = FindDeterminator(A1, B1, A2, B2);
        //    if (zn != 0)
        //    {
        //        double x = -FindDeterminator(C1, B1, C2, B2) / zn;
        //        double y = -FindDeterminator(A1, C1, A2, C2) / zn;
        //        if (findInvisibleIntersection)
        //        {
        //            pts.Add(new Point(x, y));
        //            return hits;
        //        }
        //        else
        //        {
        //            if (IsDotOnLine(a.X, b.X, x) && IsDotOnLine(a.Y, b.Y, y)
        //                && IsDotOnLine(c.X, d.X, x) && IsDotOnLine(c.Y, d.Y, y))
        //            {
        //                pts.Add(new Point(x, y));
        //                hits++;
        //                if(findControlLine)
        //                {
        //                    return hits;
        //                }
        //            }
        //        }
        //    }
        //    return hits;
        //}

        //private void OrganizeGladDots(List<Point> pts, List<Figure> ListControlLines, Point c, int hits)        //сортировка точек пересечения в отдельные листы фигур                    
        //{
        //    double[] distance = new double[pts.Count];
        //    int[] numbers = new int[pts.Count];
        //    for (int i = 0; i < pts.Count; i++)
        //    {
        //        numbers[i] = i;
        //    }

        //    for (int i = 0; i < pts.Count; i++)                             //находим расстояния от прямой, перпендикулярной задающей прямой
        //    {
        //        distance[i] = FindLength(pts[i], c);
        //    }

        //    for (int i = 0; i < pts.Count - 1; i++)             //сортировка точек в массиве по порядку расстояния
        //    {
        //        int min = i;
        //        for (int j = i + 1; j < pts.Count; j++)
        //        {
        //            if (distance[j] < distance[min])
        //            {
        //                min = j;
        //            }
        //        }
        //        double dummy = distance[i];
        //        distance[i] = distance[min];
        //        distance[min] = dummy;

        //        int dummy1 = numbers[i];
        //        numbers[i] = numbers[min];
        //        numbers[min] = dummy1;
        //    }

        //    if (oldGladHits != hits)                //если на следующей задающей прямой лежит больше точек, чем на предыдущей, то следующие точки заносим в следующие листы фигур
        //    {
        //        /*
        //        for(int i = figureCount; i < (figureCount + (hits / 2) + 1);i++)            //неплохо бы динамически создавать листы фигур
        //        {
        //            controlPointsList.Add(new List<Point>());
        //            tatamiPoints.Add(new List<Point>());
        //        }
        //         * */
        //        satinShapesCount += (oldGladHits / 2);
        //        oldGladHits = hits;
        //    }
        //    for (int i = 0; i < hits; i += 2)                              //добалвение точек в листы фигур
        //    {
        //        ListControlLines[satinShapesCount + i / 2].points.Add(pts[numbers[i]]);
        //        ListControlLines[satinShapesCount + i / 2].points.Add(pts[numbers[i + 1]]);
        //    }
        //}

        //public void CalculateGladLines(Figure firstFigure,Figure secondFigure,List<Figure> satinLines, List<Figure> ListControlLines,Canvas canvas)
        //{
        //    foreach (Figure fig in satinLines)
        //        fig.RemoveFigure(canvas);
        //    if (satinLines.Count != 2)
        //    {
        //        SortAndDeleteControlGladLines(satinLines);
        //    }
        //    oldGladHits = 0;
        //    ListControlLines.Clear();
        //    for (int i = 0; i < 128; i++)
        //    {
        //        ListControlLines.Add(new Figure(canvas));
        //    }
        //    for (int i = 0; i < satinLines.Count - 1; i++)
        //    {
        //        List<Point> IntersectionPoint = new List<Point>();
        //        FindIntersection(satinLines[i].pointStart, satinLines[i].pointEnd, satinLines[i + 1].pointStart, satinLines[i + 1].pointEnd, IntersectionPoint, 0, true,false);
        //        ///a и b - точки отрезка самых дальних точек пересечения направляющих с начальными отрезками
        //        Point a, b;

        //        double distance = FindLength(IntersectionPoint[0], satinLines[i].pointStart);
        //        if (distance < FindLength(IntersectionPoint[0], satinLines[i].pointEnd))
        //            a = satinLines[i].pointEnd;
        //        else
        //            a = satinLines[i].pointStart;

        //        distance = FindLength(IntersectionPoint[0], satinLines[i + 1].pointStart);
        //        if (distance < FindLength(IntersectionPoint[0], satinLines[i + 1].pointEnd))
        //            b = satinLines[i + 1].pointEnd;
        //        else
        //            b = satinLines[i + 1].pointStart;

        //        distance = OptionSatin.lengthStep *0.2;
        //        Vector vect = b - a;
        //        double length = vect.Length;
        //        Vector invisibleLine = a - IntersectionPoint[0];
        //        invisibleLine *= 3;
        //        CheckForGladIntersection(IntersectionPoint[0], new Point(IntersectionPoint[0].X + invisibleLine.X, IntersectionPoint[0].Y + invisibleLine.Y),
        //            firstFigure, secondFigure, false, ListControlLines);
        //        while (length > distance)
        //        {
        //            vect.Normalize();
        //            vect *= distance;
        //            Point pVect = new Point(a.X + vect.X, a.Y + vect.Y);
        //            invisibleLine = pVect - IntersectionPoint[0];
        //            invisibleLine *= 3;
        //            CheckForGladIntersection(IntersectionPoint[0], new Point(IntersectionPoint[0].X + invisibleLine.X, IntersectionPoint[0].Y + invisibleLine.Y),
        //            firstFigure, secondFigure, false, ListControlLines);
        //            distance += OptionSatin.lengthStep;
        //        }
        //    }
        //    MakeGlad(satinLines, ListControlLines, canvas);
        //}

        //private void MakeGlad(List<Figure> satinLines, List<Figure> ListControlLines, Canvas canvas)
        //{
        //    double step = 25;
        //    satinLines.Clear();
        //    for (int i = 0; i < satinShapesCount + 1; i++)
        //    {
        //        bool firstPoint = true;
        //        if (ListControlLines[i].points.Count > 0)
        //        {
        //            satinLines.Add(new Figure(canvas));
        //            for (int j = 0; j < ListControlLines[i].points.Count-1 ; j++)
        //            {
        //                if(firstPoint)
        //                {
        //                    satinLines[i].AddPoint(ListControlLines[i].points[j], OptionColor.colorInactive, false,false, 0);
        //                    firstPoint = false;
        //                }
        //                double x = ListControlLines[i].points[j + 1].X - ListControlLines[i].points[j].X;
        //                double y = ListControlLines[i].points[j + 1].Y - ListControlLines[i].points[j].Y;
        //                double distance = step;
        //                Vector vect = new Vector(x, y);
        //                double length = vect.Length;
        //                while (length > distance)           //ставим на отрезках стежки до тех пор, пока не пройдемся по всему отрезку
        //                {
        //                    vect.Normalize();
        //                    vect *= distance;
        //                    satinLines[i].AddPoint(new Point(ListControlLines[i].points[j].X + vect.X, ListControlLines[i].points[j].Y + vect.Y),
        //                        OptionColor.colorInactive, false,false, 0);
        //                    distance += step;
        //                }
        //                satinLines[i].AddPoint(ListControlLines[i].points[j+1], OptionColor.colorInactive, false,false, 0);
        //            }
        //        }
        //    }
        //}

        //private void SortAndDeleteControlGladLines(List<Figure> ControlGladLines)
        //{
        //    int count = ControlGladLines.Count - 1;
        //    bool areIntersected = true; ;
        //    List<Point> pts = new List<Point>();
        //    while (areIntersected)
        //    {
        //        areIntersected = false;
        //        for (int i = 1; i < count; i++)
        //        {
        //            for (int j = i + 1; j < count; j++)
        //            {
        //                if (i != j)
        //                {
        //                    if (FindIntersection(ControlGladLines[i].pointStart, ControlGladLines[i].pointEnd, ControlGladLines[j].pointStart,
        //                ControlGladLines[j].pointEnd, pts, 0, false, false) > 0)
        //                    {
        //                        ControlGladLines.RemoveAt(i);
        //                        count--;
        //                        areIntersected = true;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    Figure smoothedGladFigure = Cepochka(listFigure[firstSatinFigure], 5, false, mainCanvas);
        //    int index = 0;
        //    for (int i = 0; i < smoothedGladFigure.points.Count; i++)
        //    {
        //        for (int j = index; j < ControlGladLines.Count; j++)
        //        {
        //            if (FindLength(smoothedGladFigure.points[i], ControlGladLines[j].pointStart) < 5 ||
        //                FindLength(smoothedGladFigure.points[i], ControlGladLines[j].pointEnd) < 5)
        //            {
        //                Figure temp = ControlGladLines[j];
        //                ControlGladLines[j] = ControlGladLines[index];
        //                ControlGladLines[index] = temp;
        //                index++;
        //                break;
        //            }
        //        }
        //    }
        //}
    }
}
