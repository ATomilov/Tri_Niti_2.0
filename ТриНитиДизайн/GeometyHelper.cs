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
    public static class GeometryHelper
    {
        public static Shape SetArc(Brush brush, Point firstDot, Point secondDot, Point thirdDot, Canvas canvas)
        {
            Path myPath = new Path();
            myPath.Stroke = brush;
            myPath.StrokeThickness = OptionDrawLine.StrokeThickness;
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = firstDot;
            ArcSegment arc = new ArcSegment();
            arc.Point = secondDot;
            double dist = FindLength(firstDot, secondDot);
            double x0 = thirdDot.X, y0 = thirdDot.Y;
            double x1 = firstDot.X, y1 = firstDot.Y;
            double x2 = secondDot.X, y2 = secondDot.Y;
            double xc = -0.5 * (y0 * (x1 * x1 + y1 * y1 - x2 * x2 - y2 * y2) + y1 * (x2 * x2 + y2 * y2 - x0 * x0 - y0 * y0) +
                y2 * (x0 * x0 + y0 * y0 - x1 * x1 - y1 * y1)) / (x0 * (y1 - y2) + x1 * (y2 - y0) + x2 * (y0 - y1));
            double yc = 0.5 * (x0 * (x1 * x1 + y1 * y1 - x2 * x2 - y2 * y2) + x1 * (x2 * x2 + y2 * y2 - x0 * x0 - y0 * y0) +
                x2 * (x0 * x0 + y0 * y0 - x1 * x1 - y1 * y1)) / (x0 * (y1 - y2) + x1 * (y2 - y0) + x2 * (y0 - y1));
            Vector vect1 = new Vector(x1 - xc, y1 - yc);
            Vector vect2 = new Vector(x2 - xc, y2 - yc);
            double atan = Math.Atan2(vect1.X * vect2.Y - vect1.Y * vect2.X, vect1.X * vect2.X + vect1.Y * vect2.Y);
            double radius = FindLength(firstDot, new Point(xc, yc));
            double height = -((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1) / dist;
            if (height < 0)
            {
                arc.SweepDirection = SweepDirection.Clockwise;
                atan = -atan;
            }
            if (atan > 0)
                arc.IsLargeArc = true;
            arc.Size = new Size(radius, radius);
            pathFigure.Segments.Add(arc);
            pathGeometry.Figures.Add(pathFigure);
            myPath.Data = pathGeometry;
            canvas.Children.Add(myPath);
            return myPath;
        }

        public static Shape SetBezier(Brush brush, Point firstDot, Point controlDot1, Point controlDot2, Point lastDot, Canvas canvas)
        {
            Path myPath = new Path();
            myPath.MinHeight = 5;
            myPath.Stroke = brush;
            myPath.StrokeThickness = OptionDrawLine.StrokeThickness;
            PathGeometry myPathGeometry = new PathGeometry();
            PathFigure myPathFigure = new PathFigure();
            BezierSegment myArcSegment = new BezierSegment();
            myPathFigure.StartPoint = firstDot;
            myArcSegment.Point1 = controlDot1;
            myArcSegment.Point2 = controlDot2;
            myArcSegment.Point3 = lastDot;
            myPathFigure.Segments.Add(myArcSegment);
            myPathGeometry.Figures.Add(myPathFigure);
            myPath.Data = myPathGeometry;
            canvas.Children.Add(myPath);
            return myPath;
        }

        public static Line SetLine(Brush brush, Point p1, Point p2, bool isDashed, Canvas canvas)
        {
            Line line = new Line();
            line.X1 = p1.X;
            line.Y1 = p1.Y;
            line.X2 = p2.X;
            line.Y2 = p2.Y;
            line.StrokeThickness = OptionDrawLine.StrokeThickness;
            if(isDashed)
            {
                DoubleCollection dashes = new DoubleCollection();
                dashes.Add(3);
                dashes.Add(3);
                line.StrokeDashArray = dashes;
            }
            line.Stroke = brush;
            canvas.Children.Add(line);
            return line;
        }

        public static Rectangle DrawRectangle(Point p, bool invRectangles, bool smallRec, double thickness, Brush brush, Canvas canvas)
        {
            Rectangle rec = new Rectangle();
            if (!invRectangles)
            {
                rec.Height = OptionDrawLine.SizeWidthAndHeightRectangle;
                rec.Width = OptionDrawLine.SizeWidthAndHeightRectangle;
            }
            else
            {
                rec.Height = OptionDrawLine.SizeWidthAndHeightInvRectangle;
                rec.Width = OptionDrawLine.SizeWidthAndHeightInvRectangle;
            }
            if (smallRec)
            {
                rec.Height = OptionDrawLine.SizeWidthAndHeightRectangle / 1.5;
                rec.Width = OptionDrawLine.SizeWidthAndHeightRectangle / 1.5;
            }
            Canvas.SetLeft(rec, p.X - rec.Height / 2);
            Canvas.SetTop(rec, p.Y - rec.Width / 2);

            rec.Fill = brush;
            rec.Stroke = OptionColor.ColorSelection;
            rec.StrokeThickness = thickness;
            if (invRectangles)
                rec.Opacity = 0;
            canvas.Children.Add(rec);
            return rec;
        }

        private static double FindLength(Point a, Point b)                  //ф-ла длины отрезка по координатам
        {
            return Math.Sqrt(Math.Pow((b.X - a.X), 2) + Math.Pow((b.Y - a.Y), 2));
        }
    }
}
