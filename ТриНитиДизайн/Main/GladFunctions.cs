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
        int gladShapesCount = 0;
        int oldGladHits = 0;
        public void ShowJoinMessage(List<Figure>gladLines, Figure firstFigure, Figure secondFigure, Canvas canvas)
        {
            string sMessageBoxText = "Соединить?";
            string sCaption = "Окно";

            MessageBoxButton btnMessageBox = MessageBoxButton.OKCancel;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.OK:
                    {
                        OptionRegim.regim = Regim.RegimGlad;
                        OptionRegim.oldRegim = Regim.RegimGlad;
                        ControlLine = new Figure(MainCanvas);
                        AddFirstGladLines(gladLines, firstFigure, secondFigure, canvas);
                        if (!firstFigure.PreparedForTatami)
                        {
                            PrepareForTatami(firstFigure, MainCanvas);
                        }
                        if (!secondFigure.PreparedForTatami)
                        {
                            PrepareForTatami(secondFigure, MainCanvas);
                        }
                        break;
                    }
                    
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void AddFirstGladLines(List<Figure> gladLines, Figure firstFigure, Figure secondFigure, Canvas canvas)
        {
            gladShapesCount = 0;
            gladLines.Clear();
            secondFigure.ChangeFigureColor(OptionColor.ColorDraw,false);
            gladLines.Add(new Figure(canvas));
            gladLines.Add(new Figure(canvas));
            if (CheckForGladIntersection(firstFigure.PointStart, secondFigure.PointStart, firstFigure, secondFigure, true, null))
            {
                gladLines[0].AddPoint(firstFigure.PointStart, OptionColor.ColorChoosingRec, false, 0);
                gladLines[0].AddPoint(secondFigure.PointEnd, OptionColor.ColorChoosingRec, false, 0);
                gladLines[1].AddPoint(firstFigure.PointEnd, OptionColor.ColorChoosingRec, false, 0);
                gladLines[1].AddPoint(secondFigure.PointStart, OptionColor.ColorChoosingRec, false, 0);
            }
            else
            {
                gladLines[0].AddPoint(firstFigure.PointStart, OptionColor.ColorChoosingRec, false, 0);
                gladLines[0].AddPoint(secondFigure.PointStart, OptionColor.ColorChoosingRec, false, 0);
                gladLines[1].AddPoint(firstFigure.PointEnd, OptionColor.ColorChoosingRec, false, 0);
                gladLines[1].AddPoint(secondFigure.PointEnd, OptionColor.ColorChoosingRec, false, 0);
            }

        }

        public void FindGladControlLine(Figure LineControl,List<Figure> gladLines, Figure firstFigure, Figure secondFigure, Canvas canvas)
        {
            canvas.Children.RemoveAt(canvas.Children.Count - 1);
            List<Point> pts = new List<Point>();
            int hits = 0;
            Point a = LineControl.Points[0];
            Point b = LineControl.Points[1];
            Point c = new Point();
            Point d = new Point();
            for (int i = 0; i < firstFigure.Points.Count - 1; i++)
            {
                c = firstFigure.Points[i];
                d = firstFigure.Points[i + 1];
                hits = FindIntersection(a, b, c, d, pts, hits, false,true);
                if(hits == 1)
                {
                    break;
                }
            }
            if (hits == 1)
            {
                for (int i = 0; i < secondFigure.Points.Count - 1; i++)
                {
                    c = secondFigure.Points[i];
                    d = secondFigure.Points[i + 1];
                    hits = FindIntersection(a, b, c, d, pts, hits, false, true);
                    if (hits == 2)
                    {
                        break;
                    }
                }
                if (hits == 2)
                {
                    Figure fig = new Figure(canvas);
                    fig.AddPoint(pts[0], OptionColor.ColorChoosingRec, false, 0);
                    fig.AddPoint(pts[1], OptionColor.ColorChoosingRec, false, 0);
                    gladLines.Insert(1, fig);
                }
            }
        }

        private bool CheckForGladIntersection(Point a, Point b, Figure firstFigure, Figure secondFigure, bool firstCheck, List<Figure> ListControlLines)
        {
            List<Point> pts = new List<Point>();
            Point c = new Point();
            Point d = new Point();

            int hits = 0;
            if (firstCheck)
            {
                c = firstFigure.PointEnd;
                d = secondFigure.PointEnd;
                hits = FindIntersection(a, b, c, d, pts, hits, false, false);
            }
            else
            {
                for (int i = 0; i < firstFigure.Points.Count - 1; i++)
                {
                    c = firstFigure.Points[i];
                    d = firstFigure.Points[i + 1];
                    hits = FindIntersection(a, b, c, d, pts, hits, false, false);
                }
                for (int i = 0; i < secondFigure.Points.Count - 1; i++)
                {
                    c = secondFigure.Points[i];
                    d = secondFigure.Points[i + 1];
                    hits = FindIntersection(a, b, c, d, pts, hits, false, false);
                }
                if (hits % 2 == 0)
                {
                    OrganizeGladDots(pts, ListControlLines, b, hits);
                }
            }
            if(hits > 0)
                return true;
            else
                return false;
        }

        private int FindIntersection(Point a, Point b, Point c, Point d, List<Point> pts, int hits, bool findInvisibleIntersection,bool findControlLine)
        {
            double A1 = a.Y - b.Y,
                    B1 = b.X - a.X,
                    C1 = -A1 * a.X - B1 * a.Y;
            double A2 = c.Y - d.Y,
                    B2 = d.X - c.X,
                    C2 = -A2 * c.X - B2 * c.Y;
            double zn = FindDeterminator(A1, B1, A2, B2);
            if (zn != 0)
            {
                double x = -FindDeterminator(C1, B1, C2, B2) / zn;
                double y = -FindDeterminator(A1, C1, A2, C2) / zn;
                if (findInvisibleIntersection)
                {
                    pts.Add(new Point(x, y));
                    return hits;
                }
                else
                {
                    if (IsDotOnLine(a.X, b.X, x) && IsDotOnLine(a.Y, b.Y, y)
                        && IsDotOnLine(c.X, d.X, x) && IsDotOnLine(c.Y, d.Y, y))
                    {
                        pts.Add(new Point(x, y));
                        hits++;
                        if(findControlLine)
                        {
                            return hits;
                        }
                    }
                }
            }
            return hits;
        }

        private void OrganizeGladDots(List<Point> pts, List<Figure> ListControlLines, Point c, int hits)        //сортировка точек пересечения в отдельные листы фигур                    
        {
            double[] distance = new double[pts.Count];
            int[] numbers = new int[pts.Count];
            for (int i = 0; i < pts.Count; i++)
            {
                numbers[i] = i;
            }

            for (int i = 0; i < pts.Count; i++)                             //находим расстояния от прямой, перпендикулярной задающей прямой
            {
                distance[i] = FindLength(pts[i], c);
            }

            for (int i = 0; i < pts.Count - 1; i++)             //сортировка точек в массиве по порядку расстояния
            {
                int min = i;
                for (int j = i + 1; j < pts.Count; j++)
                {
                    if (distance[j] < distance[min])
                    {
                        min = j;
                    }
                }
                double dummy = distance[i];
                distance[i] = distance[min];
                distance[min] = dummy;

                int dummy1 = numbers[i];
                numbers[i] = numbers[min];
                numbers[min] = dummy1;
            }

            if (oldGladHits != hits)                //если на следующей задающей прямой лежит больше точек, чем на предыдущей, то следующие точки заносим в следующие листы фигур
            {
                /*
                for(int i = figureCount; i < (figureCount + (hits / 2) + 1);i++)            //неплохо бы динамически создавать листы фигур
                {
                    controlPointsList.Add(new List<Point>());
                    tatamiPoints.Add(new List<Point>());
                }
                 * */
                gladShapesCount += (oldGladHits / 2);
                oldGladHits = hits;
            }
            for (int i = 0; i < hits; i += 2)                              //добалвение точек в листы фигур
            {
                ListControlLines[gladShapesCount + i / 2].Points.Add(pts[numbers[i]]);
                ListControlLines[gladShapesCount + i / 2].Points.Add(pts[numbers[i + 1]]);
            }
        }

        public void CalculateGladLines(Figure firstFigure,Figure secondFigure,List<Figure> gladLines, List<Figure> ListControlLines,Canvas canvas)
        {
            if (gladLines.Count != 2)
            {
                SortAndDeleteControlGladLines(gladLines);
            }
            oldGladHits = 0;
            ListControlLines.Clear();
            for (int i = 0; i < 128; i++)
            {
                ListControlLines.Add(new Figure(canvas));
            }
            for (int i = 0; i < gladLines.Count - 1; i++)
            {
                List<Point> IntersectionPoint = new List<Point>();
                FindIntersection(gladLines[i].PointStart, gladLines[i].PointEnd, gladLines[i + 1].PointStart, gladLines[i + 1].PointEnd, IntersectionPoint, 0, true,false);
                ///a и b - точки отрезка самых дальних точек пересечения направляющих с начальными отрезками
                Point a, b;

                double distance = FindLength(IntersectionPoint[0], gladLines[i].PointStart);
                if (distance < FindLength(IntersectionPoint[0], gladLines[i].PointEnd))
                    a = gladLines[i].PointEnd;
                else
                    a = gladLines[i].PointStart;

                distance = FindLength(IntersectionPoint[0], gladLines[i + 1].PointStart);
                if (distance < FindLength(IntersectionPoint[0], gladLines[i + 1].PointEnd))
                    b = gladLines[i + 1].PointEnd;
                else
                    b = gladLines[i + 1].PointStart;

                distance = OptionGlad.LenthStep;
                Vector vect = b - a;
                double length = vect.Length;
                Vector invisibleLine = a - IntersectionPoint[0];
                invisibleLine *= 3;
                CheckForGladIntersection(IntersectionPoint[0], new Point(IntersectionPoint[0].X + invisibleLine.X, IntersectionPoint[0].Y + invisibleLine.Y),
                    firstFigure, secondFigure, false, ListControlLines);
                while (length > distance)
                {
                    vect.Normalize();
                    vect *= distance;
                    Point pVect = new Point(a.X + vect.X, a.Y + vect.Y);
                    invisibleLine = pVect - IntersectionPoint[0];
                    invisibleLine *= 3;
                    CheckForGladIntersection(IntersectionPoint[0], new Point(IntersectionPoint[0].X + invisibleLine.X, IntersectionPoint[0].Y + invisibleLine.Y),
                    firstFigure, secondFigure, false, ListControlLines);
                    distance += OptionGlad.LenthStep;
                }
            }
            MakeGlad(gladLines, ListControlLines, canvas);
        }

        private void MakeGlad(List<Figure> gladLines, List<Figure> ListControlLines, Canvas canvas)
        {
            gladLines.Clear();
            canvas.Children.Clear();
            for (int i = 0; i < gladShapesCount + 1; i++)
            {
                bool firstPoint = true;
                if (ListControlLines[i].Points.Count > 0)
                {
                    gladLines.Add(new Figure(canvas));
                    for (int j = 0; j < ListControlLines[i].Points.Count-1 ; j++)
                    {
                        if(firstPoint)
                        {
                            gladLines[i].AddPoint(ListControlLines[i].Points[j], OptionColor.ColorSelection, true, (OptionDrawLine.SizeWidthAndHeightRectangle/2));
                            firstPoint = false;
                        }
                        double x = ListControlLines[i].Points[j + 1].X - ListControlLines[i].Points[j].X;
                        double y = ListControlLines[i].Points[j + 1].Y - ListControlLines[i].Points[j].Y;
                        double step = 25;
                        double distance = step;
                        Vector vect = new Vector(x, y);
                        double length = vect.Length;
                        while (length > distance)           //ставим на отрезках стежки до тех пор, пока не пройдемся по всему отрезку
                        {
                            vect.Normalize();
                            vect *= distance;
                            gladLines[i].AddPoint(new Point(ListControlLines[i].Points[j].X + vect.X, ListControlLines[i].Points[j].Y + vect.Y), OptionColor.ColorSelection, true, (OptionDrawLine.SizeWidthAndHeightRectangle / 2));
                            distance += step;
                        }
                        gladLines[i].AddPoint(ListControlLines[i].Points[j+1], OptionColor.ColorSelection, true, (OptionDrawLine.SizeWidthAndHeightRectangle/2));
                    }
                }
            }
        }

        private void SortAndDeleteControlGladLines(List<Figure> ControlGladLines)
        {
            int count = ControlGladLines.Count - 1;
            bool areIntersected = true; ;
            List<Point> pts = new List<Point>();
            while (areIntersected)
            {
                areIntersected = false;
                for (int i = 1; i < count; i++)
                {
                    for (int j = i + 1; j < count; j++)
                    {
                        if (i != j)
                        {
                            if (FindIntersection(ControlGladLines[i].PointStart, ControlGladLines[i].PointEnd, ControlGladLines[j].PointStart,
                        ControlGladLines[j].PointEnd, pts, 0, false, false) > 0)
                            {
                                ControlGladLines.RemoveAt(i);
                                count--;
                                areIntersected = true;
                                break;
                            }
                        }
                    }
                }
            }
            pts = new List<Point>();
            for(int i = 1; i < count;i++)
            {
                double dist = FindLength(ControlGladLines[0].PointStart, ControlGladLines[i].PointStart);
                if (dist < FindLength(ControlGladLines[0].PointStart, ControlGladLines[i].PointEnd))
                    pts.Add(ControlGladLines[i].PointEnd);
                else
                    pts.Add(ControlGladLines[i].PointStart);
            }

            double[] distance = new double[pts.Count];

            for (int i = 0; i < pts.Count; i++)
            {
                distance[i] = FindLength(pts[i], ControlGladLines[0].PointStart);
            }

            for (int i = 0; i < pts.Count - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < pts.Count; j++)
                {
                    if (distance[j] < distance[min])
                    {
                        min = j;
                    }
                }
                double dummy = distance[i];
                distance[i] = distance[min];
                distance[min] = dummy;

                Figure dummy1 = ControlGladLines[i+1];
                ControlGladLines[i + 1] = ControlGladLines[min + 1];
                ControlGladLines[min + 1] = dummy1;
            }
        }
    }
}
