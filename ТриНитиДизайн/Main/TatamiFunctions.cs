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
        int oldHits = 2;
        int TatamiShapesCount = 0;

        public void InsertFirstControlLine(Figure StartLines, Figure ConLine, Canvas CurCanvas)
        {
            Random rand = new Random();
            Point center = StartLines.GetCenter();
            Vector vect = new Vector();
            bool found = false;
            while (!found)
            {
                ConLine.Points.Clear();
                vect.X = rand.NextDouble() * 2 - 1;
                vect.Y = rand.NextDouble() * 2 - 1;
                Point p1 = new Point(center.X + vect.X * 500, center.Y + vect.Y * 500);
                Point p2 = new Point(center.X - vect.X * 500, center.Y - vect.Y * 500);
                ConLine.Points.Add(p1);
                ConLine.Points.Add(p2);
                found = FindControlLine(StartLines, ConLine, CurCanvas,true);
            }
        }

        public bool FindControlLine(Figure StartLines, Figure ConLine, Canvas CurCanvas,bool isFirstLine)
        {
            Point a = ConLine.Points[0];
            Point b = ConLine.Points[1];

            if (!(a.X == b.X && a.Y == b.Y) && !isFirstLine)
            {
                CurCanvas.Children.RemoveAt(MainCanvas.Children.Count - 1);
            }

            bool success;
            success = CheckForIntersection(a, b, StartLines, ConLine, null, CurCanvas, true, false);
            if (success)
            {
                if(ConLine.Shapes != null)
                {
                    ConLine.RemoveFigure(CurCanvas);
                    ConLine.Shapes.Clear();
                }
                double x = b.X - a.X;
                double y = b.Y - a.Y;
                Vector line = new Vector(x, y);
                line.Normalize();
                line *= 1000;
                double x1 = a.X + line.X;
                double y1 = a.Y + line.Y;
                double x2 = a.X - line.X;
                double y2 = a.Y - line.Y;
                CheckForIntersection(new Point(x1, y1), new Point(x2, y2), StartLines, ConLine, null, CurCanvas, false, true);
            }
            return success;
        }

        private bool CheckForIntersection(Point a, Point b, Figure StartLines, Figure ConLine, List<Figure> ListControlLines, Canvas CurCanvas, bool firstCheck, bool secondCheck)              //проверка на пересечение задающих прямых и начальных отрезков
        {
            List<Point> pts = new List<Point>();

            double A1 = a.Y - b.Y,                                                  //коэффициенты A B C задающей прямой
                    B1 = b.X - a.X,
                    C1 = -A1 * a.X - B1 * a.Y;

            int hits = 0;                                                           //сколько раз была пересечена прямая
            bool success = false;
            for (int i = 0; i < StartLines.Points.Count-1; i++)                                 //перебираем все начальные отрезки
            {
                Point c, d;
                c = StartLines.Points[i];
                d = StartLines.Points[i + 1];

                double A2 = c.Y - d.Y,                              //коэффициенты A B C начального отрезка
                    B2 = d.X - c.X,
                    C2 = -A2 * c.X - B2 * c.Y;
                double zn = FindDeterminator(A1, B1, A2, B2);
                if (zn != 0)
                {
                    double x = -FindDeterminator(C1, B1, C2, B2) / zn;          //нахождение координат пересечения
                    double y = -FindDeterminator(A1, C1, A2, C2) / zn;
                    if (IsDotOnLine(a.X, b.X, x) && IsDotOnLine(a.Y, b.Y, y)            //находятся ли координаты пересечения на отрезках задающей прямой и начального отрезка
                        && IsDotOnLine(c.X, d.X, x) && IsDotOnLine(c.Y, d.Y, y))
                    {
                        pts.Add(new Point(x, y));                                   //если да, то добавляем точки пересечения в лист
                        hits++;

                    }
                }
            }
            if (hits > 0)                                                       //если пересечения для задающей прямой произошли, то рисуем их и сортируем по порядку
            {
                if (!firstCheck && !secondCheck && hits % 2 != 1)
                {
                    OrganizeDots(pts, ConLine,ListControlLines, b, hits);
                }
                if (secondCheck && hits % 2 != 1)                                                 //отрисовка линии между первой и последней точкой пересечения, как в программе
                {
                    OrganizeDots(pts, ConLine,ListControlLines, b, hits);
                    ConLine.Shapes.Add(SetLine(ConLine.Points[2], ConLine.Points[ConLine.Points.Count - 1], "blue", CurCanvas));
                    ConLine.SetMiddleControlLine(ConLine.Points[2], ConLine.Points[ConLine.Points.Count - 1], CurCanvas);
                }
                success = true;
            }

            if (firstCheck)
            {
                if (success)
                {
                    return true;
                }
            }
            return false;
        }

        private void OrganizeDots(List<Point> pts, Figure ConLine, List<Figure> ListControlLines, Point c, int hits)        //сортировка точек пересечения в отдельные листы фигур                    
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

            if (oldHits != hits)                //если на следующей задающей прямой лежит больше точек, чем на предыдущей, то следующие точки заносим в следующие листы фигур
            {
                /*
                for(int i = figureCount; i < (figureCount + (hits / 2) + 1);i++)            //неплохо бы динамически создавать листы фигур
                {
                    controlPointsList.Add(new List<Point>());
                    tatamiPoints.Add(new List<Point>());
                }
                 * */
                TatamiShapesCount += (oldHits / 2);
                oldHits = hits;
            }
            for (int i = 0; i < hits; i += 2)                              //добалвение точек в листы фигур
            {
                if (ListControlLines != null)
                {
                    ListControlLines[TatamiShapesCount + i / 2].Points.Add(pts[numbers[i]]);
                    ListControlLines[TatamiShapesCount + i / 2].Points.Add(pts[numbers[i + 1]]);
                }
                if (ConLine != null)
                {
                    ConLine.Points.Add(pts[numbers[i]]);
                    ConLine.Points.Add(pts[numbers[i + 1]]);
                }
            }
        }

        public double FindLength(Point a, Point b)                  //ф-ла длины отрезка по координатам
        {
            return Math.Sqrt(Math.Pow((b.X - a.X), 2) + Math.Pow((b.Y - a.Y), 2));
        }

        private double FindDeterminator(double a, double b, double c, double d)          //определитель для нахождения точки пересечения
        {
            return a * d - b * c;
        }

        private bool IsDotOnLine(double a, double b, double c)
        {
            return (Math.Min(a, b) <= c) && c <= (Math.Max(a, b));
        }

        private Line SetLine(Point point1, Point point2, string type, Canvas CurCanvas)                //отрисовка линии, dash - через черту, red - красная, blue - синяя
        {
            Line shape = new Line();
            shape.Stroke = OptionColor.ColorSelection;
            shape.StrokeThickness = OptionDrawLine.StrokeThickness;
            if (type.Equals("dash"))
            {
                DoubleCollection dashes = new DoubleCollection();
                dashes.Add(2);
                dashes.Add(2);
                shape.StrokeDashArray = dashes;
            }
            if (type.Equals("red"))
            {
                shape.Stroke = OptionColor.ColorDraw;
            }
            if (type.Equals("blue"))
            {
                shape.StrokeThickness = OptionDrawLine.StrokeThickness;
                shape.Stroke = System.Windows.Media.Brushes.Blue;
            }
            shape.X1 = point1.X;
            shape.Y1 = point1.Y;
            shape.X2 = point2.X;
            shape.Y2 = point2.Y;
            CurCanvas.Children.Add(shape);
            return shape;
        }
        
        public void CalculateParallelLines(Point a, Point b, Figure StartLines, List<Figure> ListControlLines, List<Figure> ListTatamiFigures, Canvas CurCanvas)            //отрисовка параллельных задающих линий
        {
            TatamiShapesCount = 0;
            ListControlLines.Clear();
            ListTatamiFigures.Clear();
            for (int i = 0; i < 128; i++)                               
            {
                ListControlLines.Add(new Figure(CurCanvas));
                ListTatamiFigures.Add(new Figure(CurCanvas));
            }
            double x1 = a.X;
            double y1 = a.Y;
            double x2 = b.X;
            double y2 = b.Y;

            double newX = 0;
            double newY = 0;
            double x3;
            double y3;

            double step = OptionTatami.StepLine * 0.2;        //шаг задающих прямых
            double lineDistance = 2000;
            double distanceFull = 0;
            double numberOfLines = 2000/ step;                        //надо подумать над количеством линий

            Vector vectControlLine = new Vector();                  //вектор первоначальной задающей прямой
            vectControlLine.X = (x2 - x1);
            vectControlLine.Y = (y2 - y1);
            vectControlLine.Normalize();

            Vector newVect = new Vector();

            x3 = (vectControlLine.X * (lineDistance / 2)) + x1;
            y3 = (vectControlLine.Y * (lineDistance / 2)) + y1;

            newVect.X = (vectControlLine.X * step * numberOfLines) / 2;
            newVect.Y = (vectControlLine.Y * step * numberOfLines) / 2;

            x3 = x3 - newVect.Y;                            //нахождение угла прямоугольника, с которого начинают идти линии
            y3 = y3 + newVect.X;

            for (int i = 0; i < numberOfLines; i++)
            {
                newVect.X = vectControlLine.X * distanceFull;                      //вектор, перпедникулярный задающей линии
                newVect.Y = vectControlLine.Y * distanceFull;

                newX = x3 + newVect.Y;                              //координаты для создания параллельной линии
                newY = y3 - newVect.X;

                Point newLineA = new Point(newX, newY);
                Point newLineB = new Point(newX - (vectControlLine.X * lineDistance), newY - (vectControlLine.Y * lineDistance));

                CheckForIntersection(newLineA, newLineB,StartLines,null, ListControlLines, CurCanvas, false, false);            //каждую созданную линию проверяем на пересечение

                distanceFull += step;
            }
            MakeTatami(ListControlLines, ListTatamiFigures, CurCanvas);
            //vector = (x1, y1), (x1 + vectorX, y1 + vectorY)
            //vector 90 = (x1, y1), (x1 - vectorY, y + vectorX)
        }

        private void MakeTatami(List<Figure> ListControlLines, List<Figure> ListTatamiFigures, Canvas CurCanvas)                    //создание татами
        {
            double step = OptionTatami.StepStegok * 0.2;
            for (int i = 0; i < TatamiShapesCount + 1; i++)               //алгоритм создания стежков на параллельных отрезках, точки начала и конца - пересечение изначальных отрезков и задающих прямых
            {
                bool turnAround = false;
                for (int j = 0; j < ListControlLines[i].Points.Count; j += 2)
                {
                    double x;
                    double y;
                    if (!turnAround)                //нахождение начала отрезка и координат вектора, по которому идем, после каждого отрезка меняется
                    {
                        x = ListControlLines[i].Points[j].X - ListControlLines[i].Points[j + 1].X;
                        y = ListControlLines[i].Points[j].Y - ListControlLines[i].Points[j + 1].Y;
                        ListTatamiFigures[i].AddPoint(ListControlLines[i].Points[j], OptionColor.ColorSelection, false,false, 4);
                        //ListTatamiFigures[i].Points.Add(ListControlLines[i].Points[j]);
                    }
                    else
                    {
                        x = ListControlLines[i].Points[j + 1].X - ListControlLines[i].Points[j].X;
                        y = ListControlLines[i].Points[j + 1].Y - ListControlLines[i].Points[j].Y;
                        ListTatamiFigures[i].AddPoint(ListControlLines[i].Points[j + 1], OptionColor.ColorSelection, false,false, 4);
                        //ListTatamiFigures[i].Points.Add(ListControlLines[i].Points[j + 1]);
                    }     
                    Vector vect = new Vector(x, y);
                    double length = vect.Length;
                    double distance = step;
                    vect = -vect;
                    while (length > distance)           //ставим на отрезках стежки до тех пор, пока не пройдемся по всему отрезку
                    {
                        vect.Normalize();
                        vect *= distance;
                        if (!turnAround)
                        {
                            ListTatamiFigures[i].AddPoint(new Point(ListControlLines[i].Points[j].X + vect.X, ListControlLines[i].Points[j].Y + vect.Y),
                                OptionColor.ColorSelection, false,false, 4);
                            //ListTatamiFigures[i].Points.Add(new Point(ListControlLines[i].Points[j].X + vect.X, ListControlLines[i].Points[j].Y + vect.Y));
                        }
                        else
                        {
                            ListTatamiFigures[i].AddPoint(new Point(ListControlLines[i].Points[j + 1].X + vect.X, ListControlLines[i].Points[j + 1].Y + vect.Y),
                                OptionColor.ColorSelection, false,false, 4);
                            //ListTatamiFigures[i].Points.Add(new Point(ListControlLines[i].Points[j + 1].X + vect.X, ListControlLines[i].Points[j + 1].Y + vect.Y));

                        }
                        distance += step;
                    }
                    if (!turnAround)        //конец отрезка
                    {
                        ListTatamiFigures[i].AddPoint(ListControlLines[i].Points[j + 1], OptionColor.ColorSelection, false,false, 4);
                       // ListTatamiFigures[i].Points.Add(ListControlLines[i].Points[j+1]);
                    }
                    else
                    {
                        ListTatamiFigures[i].AddPoint(ListControlLines[i].Points[j], OptionColor.ColorSelection, false,false, 4);
                        //ListTatamiFigures[i].Points.Add(ListControlLines[i].Points[j]);
                    }
                    turnAround = !turnAround;               //меняем направление вектора
                }
            }
            //Test(ListTatamiFigures);
        }
        /*
        private void Test(List<Figure> ListTatamiFigures)
        {
            MainCanvas.Children.Clear();
            WriteableBitmap writeableBmp = BitmapFactory.New(1600,900);
            for (int j = 0; j < ListTatamiFigures.Count; j++)
            {
                if (ListTatamiFigures[j].Points.Count > 0)
                {
                    List<Point> pts = new List<Point>(ListTatamiFigures[j].Points);
                    for (int i = 0; i < pts.Count - 1; i++)
                    {
                        writeableBmp.DrawLineAa((int)pts[i].X, (int)pts[i].Y, (int)pts[i + 1].X, (int)pts[i + 1].Y, Colors.Red);
                    }
                }
            }

            Image waveform = new Image();
            waveform.Stretch = Stretch.None;
            waveform.Source = writeableBmp;
            MainCanvas.Children.Add(waveform);

        }
         */
    }
}
