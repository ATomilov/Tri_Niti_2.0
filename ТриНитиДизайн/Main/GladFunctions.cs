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
                        AddFirstGladLines(gladLines, firstFigure, secondFigure, canvas);
                        break;
                    }
                    
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void AddFirstGladLines(List<Figure> gladLines, Figure firstFigure, Figure secondFigure, Canvas canvas)
        {
            gladLines.Clear();
            secondFigure.ChangeFigureColor(OptionColor.ColorDraw);
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
                hits = FindIntersection(a, b, c, d, pts, hits,false);
            }
            else
            {
                for (int i = 0; i < firstFigure.Points.Count - 1;i++)
                {
                    c = firstFigure.Points[i];
                    d = firstFigure.Points[i+1];
                    hits = FindIntersection(a, b, c, d, pts, hits,false);
                }
                for (int i = 0; i < secondFigure.Points.Count - 1; i++)
                {
                    c = secondFigure.Points[i];
                    d = secondFigure.Points[i + 1];
                    hits = FindIntersection(a, b, c, d, pts, hits,false);
                }
                if (hits > 1)
                {
                    OrganizeGladDots(pts, ListControlLines, b, hits);    
                }
            }

            if(hits > 0)
                return true;
            else
                return false;
        }

        private int FindIntersection(Point a, Point b, Point c, Point d, List<Point> pts, int hits, bool findInvisibleIntersection)
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
            oldGladHits = 0;
            gladShapesCount = 0;
            ListControlLines.Clear();
            for (int i = 0; i < 128; i++)
            {
                ListControlLines.Add(new Figure(canvas));
            }

            List<Point> IntersectionPoint = new List<Point>();
            FindIntersection(gladLines[0].PointStart, gladLines[0].PointEnd, gladLines[1].PointStart, gladLines[1].PointEnd, IntersectionPoint, 0, true);
            ///a и b - точки отрезка самых дальних точек пересечения направляющих с начальными отрезками
            Point a, b;                                                 

            double distance = FindLength(IntersectionPoint[0], gladLines[0].PointStart);
            if (distance < FindLength(IntersectionPoint[0], gladLines[0].PointEnd))
                a = gladLines[0].PointEnd;
            else
                a = gladLines[0].PointStart;

            distance = FindLength(IntersectionPoint[0], gladLines[1].PointStart);
            if (distance < FindLength(IntersectionPoint[0], gladLines[1].PointEnd))
                b = gladLines[1].PointEnd;
            else
                b = gladLines[1].PointStart;

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
            MakeGlad(gladLines, ListControlLines, canvas);
        }

        private void MakeGlad(List<Figure> gladLines, List<Figure> ListControlLines, Canvas canvas)
        {
            gladLines.Clear();
            canvas.Children.Clear();
            for (int i = 0; i < ListControlLines.Count; i++)
            {
                if (ListControlLines[i].Points.Count > 0)
                {
                    gladLines.Add(new Figure(canvas));
                    for (int j = 0; j < ListControlLines[i].Points.Count; j++)
                    {
                        gladLines[i].AddPoint(ListControlLines[i].Points[j], OptionColor.ColorGlad, true, 4);
                    }
                }
            }
        }
    }
}
