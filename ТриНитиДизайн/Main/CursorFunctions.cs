using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ТриНитиДизайн
{
    public partial class MainWindow : Window
    {
        public void InitiliazeFigureRectangle()
        {
            chRec = new Rectangle();
            Point a;
            Point b;
            Point c;
            Point d;
            ListFigure[IndexFigure].GetFourPointsOfOutSideRectangle(out a, out b, out c, out d, 0);
            chRec.Height = Math.Abs(b.Y - a.Y);
            chRec.Width = Math.Abs(c.X - b.X);
            DoubleCollection dashes = new DoubleCollection();
            dashes.Add(2);
            dashes.Add(2);
            chRec.StrokeDashArray = dashes;
            Canvas.SetLeft(chRec, a.X);
            Canvas.SetTop(chRec, a.Y);
            chRec.Stroke = OptionColor.ColorChoosingRec;
            chRec.StrokeThickness = OptionDrawLine.StrokeThickness;

            firstRec = GeometryHelper.DrawRectangle(ListFigure[IndexFigure].PointStart, false, true,
                OptionDrawLine.StrokeThickness, OptionColor.ColorOpacity, MainCanvas);
            lastRec = GeometryHelper.DrawRectangle(ListFigure[IndexFigure].PointEnd, false, false,
                OptionDrawLine.StrokeThicknessMainRec, OptionColor.ColorOpacity, MainCanvas);
        }

        public void MoveFigureRectangle(Rectangle rec, Vector delta, Canvas canvas)
        {
            canvas.Children.Remove(rec);
            Point p = new Point();
            p.X = Canvas.GetLeft(rec);
            p.Y = Canvas.GetTop(rec);
            p += delta;
            Canvas.SetLeft(rec, p.X);
            Canvas.SetTop(rec, p.Y);
            canvas.Children.Add(rec);
        }

        public void MoveFigureToNewPosition()
        {
            chRec = new Rectangle();
            Point newPointStart = new Point(Canvas.GetLeft(firstRec) + firstRec.Width / 2,
                Canvas.GetTop(firstRec) + firstRec.Height / 2);
            Vector figureVect = newPointStart - ListFigure[IndexFigure].PointStart;
            for(int i = 0; i < ListFigure[IndexFigure].Points.Count; i++)
            {
                Point p = ListFigure[IndexFigure].Points[i];
                if (i != ListFigure[IndexFigure].Points.Count - 1)
                {
                    Point nextP = ListFigure[IndexFigure].Points[i + 1];
                    Shape sh;
                    Shape newSh;
                    Tuple<Point,Point> contPts = new Tuple<Point,Point>(new Point(),new Point());
                    ListFigure[IndexFigure].DictionaryPointLines.TryGetValue(p, out sh);
                    if(sh is Line)
                        newSh = GeometryHelper.SetLine(OptionColor.ColorDraw, p + figureVect, nextP + figureVect, false, MainCanvas);
                    else
                    {
                        ListFigure[IndexFigure].DictionaryShapeControlPoints.TryGetValue(p, out contPts);
                        if(sh.MinHeight == 5)
                        {
                            contPts = new Tuple<Point, Point>(contPts.Item1 + figureVect, contPts.Item2 + figureVect);
                            newSh = GeometryHelper.SetBezier(OptionColor.ColorDraw, p + figureVect, contPts.Item1,
                                contPts.Item2,nextP + figureVect, MainCanvas);
                        }
                        else
                        {
                            contPts = new Tuple<Point, Point>(contPts.Item1 + figureVect, new Point());
                            newSh = GeometryHelper.SetArc(OptionColor.ColorDraw, p + figureVect, nextP + figureVect,
                                contPts.Item1, MainCanvas);
                        }
                    }
                    ListFigure[IndexFigure].DeleteShape(sh, p, MainCanvas);
                    ListFigure[IndexFigure].AddShape(newSh, p + figureVect, contPts);
                }
                ListFigure[IndexFigure].Points[i] += figureVect;
            }
            ListFigure[IndexFigure].PointStart += figureVect;
            ListFigure[IndexFigure].PointEnd += figureVect;
        }
    }
}
