using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            myPath.StrokeThickness = OptionDrawLine.strokeThickness;
            myPath.MinHeight = 10;
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
            myPath.StrokeThickness = OptionDrawLine.strokeThickness;
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
            line.StrokeThickness = OptionDrawLine.strokeThickness;
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

        public static Path SetStarForSinglePoint(Point p, Brush brush, Canvas canvas)
        {
            PathFigureCollection myPathFigureCollection = new PathFigureCollection();
            double cornerDist = OptionDrawLine.oneDotCornerDistance;
            double middleDist = OptionDrawLine.oneDotMiddleDistance;
            Vector[] vect = new Vector[6];
            vect[0] = new Vector(cornerDist, cornerDist);
            vect[1] = new Vector(-cornerDist, cornerDist);
            vect[2] = new Vector(cornerDist, -cornerDist);
            vect[3] = new Vector(-cornerDist, -cornerDist);
            vect[4] = new Vector(-middleDist, 0);
            vect[5] = new Vector(middleDist, 0);
            for (int i = 0; i < 6; i++)
            {
                PathFigure myPathFigure = new PathFigure();
                myPathFigure.StartPoint = p;
                PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
                LineSegment myLineSegment = new LineSegment();
                myLineSegment.Point = new Point(p.X + vect[i].X, p.Y + vect[i].Y);
                myPathSegmentCollection.Add(myLineSegment);
                myPathFigure.Segments = myPathSegmentCollection;
                myPathFigureCollection.Add(myPathFigure);
            }

            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures = myPathFigureCollection;
            Path myPath = new Path();
            myPath.Stroke = brush;
            myPath.StrokeThickness = OptionDrawLine.strokeThickness;
            myPath.Data = myPathGeometry;
            canvas.Children.Add(myPath);
            return myPath;
        }

        public static Rectangle DrawRectangle(Point p, bool invRectangles, bool smallRec, double thickness, Brush brush, Canvas canvas)
        {
            Rectangle rec = new Rectangle();
                rec.Height = OptionDrawLine.sizeRectangle;
                rec.Width = OptionDrawLine.sizeRectangle;
            if (smallRec)
            {
                rec.Height = OptionDrawLine.sizeRectangle / 1.5;
                rec.Width = OptionDrawLine.sizeRectangle / 1.5;
            }
            Canvas.SetLeft(rec, p.X - rec.Height / 2);
            Canvas.SetTop(rec, p.Y - rec.Width / 2);

            rec.Fill = brush;
            rec.Stroke = OptionColor.colorInactive;
            rec.StrokeThickness = thickness;
            if (invRectangles)
                rec.Opacity = 0;
            canvas.Children.Add(rec);
            return rec;
        }
        
        public static Rectangle DrawTransformingRectangle(Point p, double size, Canvas canvas)
        {
            Rectangle rec = new Rectangle();
            rec.Cursor = Cursors.Cross;
            rec.Height = size;
            rec.Width = rec.Height;
            Canvas.SetLeft(rec, p.X - size / 2);
            Canvas.SetTop(rec, p.Y - size / 2);
            rec.Stroke = OptionColor.colorInactive;
            rec.StrokeThickness = OptionDrawLine.strokeThickness;
            rec.Fill = OptionColor.colorInactive;
            canvas.Children.Add(rec);
            return rec;
        }

        public static void RescaleEllipse(Ellipse ell, double scale)
        {
            double x = Canvas.GetLeft(ell);
            double y = Canvas.GetTop(ell);
            double xEllCenter = x + ell.Width / 2;
            double yEllCenter = y + ell.Height / 2;

            ell.Height /= scale;
            ell.Width /= scale;
            Canvas.SetLeft(ell, xEllCenter - ell.Height / 2);
            Canvas.SetTop(ell, yEllCenter - ell.Width / 2);

            ell.StrokeThickness /= scale;
        }

        public static void RescaleRectangle(Rectangle rec, double scale)
        {
            double x = Canvas.GetLeft(rec);
            double y = Canvas.GetTop(rec);
            double xRecCenter = x + rec.Width / 2;
            double yRecCenter = y + rec.Height / 2;

            rec.Height /= scale;
            rec.Width /= scale;
            Canvas.SetLeft(rec, xRecCenter - rec.Height / 2);
            Canvas.SetTop(rec, yRecCenter - rec.Width / 2);

            rec.StrokeThickness /= scale;
        }

        private static double FindLength(Point a, Point b)                  //ф-ла длины отрезка по координатам
        {
            return Math.Sqrt(Math.Pow((b.X - a.X), 2) + Math.Pow((b.Y - a.Y), 2));
        }

        public static List<Point> GetFourOutsidePointsForGroup(List<Figure> group, double length)
        {
            List<Point> pts = new List<Point>();

            foreach (Figure fig in group)
                foreach (Point p in fig.GetFourPointsOfOutSideRectangle(length))
                    pts.Add(p);

            Point max = new Point(Double.MinValue, Double.MinValue);
            Point min = new Point(Double.MaxValue, Double.MaxValue);

            foreach (Point p in pts)
            {
                if (p.X > max.X)
                    max.X = p.X;
                if (p.Y > max.Y)
                    max.Y = p.Y;
                if (p.X < min.X)
                    min.X = p.X;
                if (p.Y < min.Y)
                    min.Y = p.Y;
            }
            pts.Clear();
            pts.Add(new Point(min.X, min.Y));
            pts.Add(new Point(min.X, max.Y));
            pts.Add(new Point(max.X, max.Y));
            pts.Add(new Point(max.X, min.Y));
            return pts;
        }
    }
}
