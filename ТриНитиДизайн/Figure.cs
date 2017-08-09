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
    public class Figure
    {
        public Regim regimFigure;
        public List<Shape> Shapes;
        public List<Shape> InvShapes;
        public List<Point> Points;
        public List<Shape> tempInvShapes;
        public List<Shape> tempShapes;
        public List<Point> tempPoints;
        public List<int> PointsCount;
        public List<Rectangle> RectangleOfFigures;
        public Dictionary<Point, Shape> tempDictionaryPointLines;
        public Dictionary<Shape, Shape> tempDictionaryInvLines;
        public Dictionary<Point, Shape> DictionaryPointLines;
        public Dictionary<Point, Tuple<Point,Point>> DictionaryShapeControlPoints;
        public Dictionary<Shape, Shape> DictionaryInvLines;
        public Point PointStart;
        public Point PointEnd;
        public Point EllipsePoint;
        public Point PointForAddingPoints;
        public double angle;
        public double scaleX;
        public double scaleY;
        public Canvas canvas;
        public Ellipse NewPointEllipse;
        public Rectangle SelectedRectangle;
        public bool PreparedForTatami;
        public Figure(Canvas _canvas)
        {
            regimFigure = Regim.RegimFigure;
            Shapes = new List<Shape>();
            InvShapes = new List<Shape>();
            Points = new List<Point>();
            tempShapes = new List<Shape>();
            tempInvShapes = new List<Shape>();
            tempPoints = new List<Point>();
            PointsCount = new List<int>();
            RectangleOfFigures = new List<Rectangle>();
            PreparedForTatami = false;
            angle = 0;
            scaleX = 1;
            scaleY = 1;
            canvas = _canvas;
            DictionaryShapeControlPoints = new Dictionary<Point, Tuple<Point,Point>>();
            DictionaryPointLines = new Dictionary<Point, Shape>();
            DictionaryInvLines = new Dictionary<Shape, Shape>();
            tempDictionaryPointLines = new Dictionary<Point, Shape>();
            tempDictionaryInvLines = new Dictionary<Shape, Shape>();
        }

        public void AddShape(Shape shape,Point p, Tuple<Point,Point> contPts)
        {
            Shapes.Add(shape);
            DictionaryPointLines.Add(p, shape);
            DictionaryShapeControlPoints.Add(p, contPts);
            if (shape is Path)
            {
                Path pth = (Path)shape;
                Path newPath = new Path();
                newPath.Data = pth.Data;
                newPath.Stroke = OptionColor.ColorBackground;
                newPath.StrokeThickness = OptionDrawLine.InvisibleStrokeThickness;
                newPath.Opacity = 0;
                InvShapes.Add(newPath);
                DictionaryInvLines.Add(shape, newPath);
            }
            else
            {
                Line ln = (Line)shape;
                Line newLine = new Line();
                newLine.Stroke = OptionColor.ColorBackground;
                newLine.StrokeThickness = OptionDrawLine.InvisibleStrokeThickness;
                newLine.Opacity = 0;
                newLine.X1 = ln.X1;
                newLine.Y1 = ln.Y1;
                newLine.X2 = ln.X2;
                newLine.Y2 = ln.Y2;
                InvShapes.Add(newLine);
                DictionaryInvLines.Add(shape, newLine);
            }
        }

        public void DeleteShape(Shape shape, Point p, Canvas curCanvas)
        {
            Shapes.Remove(shape);// ??? point
            DictionaryPointLines.Remove(p);
            DictionaryShapeControlPoints.Remove(p);
            Shape sh;
            DictionaryInvLines.TryGetValue(shape, out sh);
            InvShapes.Remove(sh);
            DictionaryInvLines.Remove(shape);
            curCanvas.Children.Remove(sh);
        }

        public void AddLine(Point point1, Point point2, Brush brush)
        {
            Line shape = new Line();
            shape.Stroke = brush;
            shape.StrokeThickness = OptionDrawLine.StrokeThickness;
            shape.X1 = point1.X;
            shape.Y1 = point1.Y;
            shape.X2 = point2.X;
            shape.Y2 = point2.Y;
            Shapes.Add(shape);
            DictionaryPointLines.Add(point1, shape);

            Line newLine = new Line();
            newLine.Stroke = brush;
            newLine.StrokeThickness = OptionDrawLine.InvisibleStrokeThickness;
            newLine.Opacity = 0;
            newLine.X1 = point1.X;
            newLine.Y1 = point1.Y;
            newLine.X2 = point2.X;
            newLine.Y2 = point2.Y;
            InvShapes.Add(newLine);
            DictionaryInvLines.Add(shape, newLine);
        }

        
        public void DrawAllRectangles()
        {
            RectangleOfFigures.Clear();
            double size = OptionDrawLine.SizeWidthAndHeightRectangle;
            foreach(Point p in Points)
            {
                Rectangle rec = new Rectangle();
                rec.Height = size;
                rec.Width = size;
                Canvas.SetLeft(rec, p.X - size/2);
                Canvas.SetTop(rec, p.Y - size/2);
                rec.Stroke = OptionColor.ColorSelection;
                rec.Fill = OptionColor.ColorOpacity;
                rec.StrokeThickness = OptionDrawLine.StrokeThickness;
                RectangleOfFigures.Add(rec);
                canvas.Children.Add(rec);
            }
        }

        public void ChangeRectangleColor()
        {
            for(int i = 0; i < Points.Count;i++)
            {
                if (PointsCount.Contains(i))
                    RectangleOfFigures[i].Fill = OptionColor.ColorSelection;
                else
                    RectangleOfFigures[i].Fill = OptionColor.ColorOpacity;
            }
        }

        public void DrawOutSideRectangle(Point center, double width, double height)
        {
            Rectangle rec = new Rectangle();
            rec.Height = height;
            rec.Width = width;
            Canvas.SetLeft(rec, center.X - width / 2);
            Canvas.SetTop(rec, center.Y - height / 2);
            rec.Stroke = OptionColor.ColorSelection;
            rec.StrokeThickness = OptionDrawLine.StrokeThickness;
            canvas.Children.Add(rec);
        }


        public void ClearFigure()
        {
            tempShapes = new List<Shape>();
            tempInvShapes = new List<Shape>();
            tempPoints = new List<Point>();
            Shapes = new List<Shape>();
            InvShapes = new List<Shape>();
            Points = new List<Point>();
            PointsCount = new List<int>();
            angle = 0;
            PreparedForTatami = false;
            DictionaryShapeControlPoints = new Dictionary<Point, Tuple<Point, Point>>();
            DictionaryPointLines = new Dictionary<Point, Shape>();
            DictionaryInvLines = new Dictionary<Shape, Shape>();
            tempDictionaryPointLines = new Dictionary<Point, Shape>();
            tempDictionaryInvLines = new Dictionary<Shape, Shape>();
        }

        public void SaveCurrentShapes()
        {
            tempInvShapes = new List<Shape>(InvShapes);
            tempShapes = new List<Shape>(Shapes);
            tempPoints = new List<Point>(Points);
            tempDictionaryInvLines = new Dictionary<Shape, Shape>(DictionaryInvLines);
            tempDictionaryPointLines = new Dictionary<Point, Shape>(DictionaryPointLines);
        }

        public void LoadCurrentShapes()
        {
            InvShapes = new List<Shape>(tempInvShapes);
            Shapes = new List<Shape>(tempShapes);
            Points = new List<Point>(tempPoints);
            DictionaryInvLines = new Dictionary<Shape, Shape>(tempDictionaryInvLines);
            DictionaryPointLines = new Dictionary<Point, Shape>(tempDictionaryPointLines);
            PreparedForTatami = false;
        }

        public List<Point> DrawOutSideRectanglePoints()
        {
            List<Point> PointsOutSideRectangle = new List<Point>();
            Point a, b, c, d;
            GetFourPointsOfOutSideRectangle(out a, out b, out c, out d);
            PointsOutSideRectangle.Add(a);
            PointsOutSideRectangle.Add(b);
            PointsOutSideRectangle.Add(c);
            PointsOutSideRectangle.Add(d);
            PointsOutSideRectangle.Add(new Point((a.X + b.X) / 2, (b.Y + a.Y) / 2));
            PointsOutSideRectangle.Add(new Point((b.X + c.X) / 2, (b.Y + c.Y) / 2));
            PointsOutSideRectangle.Add(new Point((c.X + d.X) / 2, (c.Y + d.Y) / 2));
            PointsOutSideRectangle.Add(new Point((d.X + a.X) / 2, (d.Y + a.Y) / 2));
            foreach(Point p in PointsOutSideRectangle)
            {
                DrawRectangle(p, canvas);
            }
            return PointsOutSideRectangle;
        }

        public List<Point> DrawOutSideRectanglePoints(double _angle)
        {
            List<Point> PointsOutSideRectangle = new List<Point>();
            Point a, b, c, d;
            GetFourPointsOfOutSideRectangle(out a, out b, out c, out d);
            PointsOutSideRectangle.Add(a);
            PointsOutSideRectangle.Add(b);
            PointsOutSideRectangle.Add(c);
            PointsOutSideRectangle.Add(d);
            PointsOutSideRectangle.Add(new Point((a.X + b.X) / 2, (b.Y + a.Y) / 2));
            PointsOutSideRectangle.Add(new Point((b.X + c.X) / 2, (b.Y + c.Y) / 2));
            PointsOutSideRectangle.Add(new Point((c.X + d.X) / 2, (c.Y + d.Y) / 2));
            PointsOutSideRectangle.Add(new Point((d.X + a.X) / 2, (d.Y + a.Y) / 2));
            foreach (Point p in PointsOutSideRectangle)
            {
                
                Rectangle rec = DrawReturnRectangle(p, canvas);
                canvas.Children.Add(rec);
                RotateTransform rotate = new RotateTransform(angle, GetCenter().X, GetCenter().Y);
                rec.RenderTransform = rotate;
                rec.MouseDown += new MouseButtonEventHandler(PointOfRectangleOutSide_MouseDown);
                rec.MouseMove += new MouseEventHandler(PointOfRectangleOutSide_MouseMove);

            }
            return PointsOutSideRectangle;
        }

        public void DrawRectangle(Point p, Canvas canvas)
        {
            Rectangle rec = new Rectangle();
            rec.Height = OptionDrawLine.SizeRectangleForTransform;
            rec.Width = rec.Height;
            Canvas.SetLeft(rec, p.X - OptionDrawLine.SizeRectangleForTransform / 2);
            Canvas.SetTop(rec, p.Y - OptionDrawLine.SizeRectangleForTransform / 2);
            rec.Stroke = OptionColor.ColorSelection;
            rec.StrokeThickness = OptionDrawLine.StrokeThickness;
            rec.Fill = Brushes.Black;
            rec.MouseDown += new MouseButtonEventHandler(PointOfRectangleOutSide_MouseDown);
            rec.MouseMove += new MouseEventHandler(PointOfRectangleOutSide_MouseMove);
            canvas.Children.Add(rec);
        }

        public Rectangle DrawReturnRectangle(Point p, Canvas canvas)
        {
            Rectangle rec = new Rectangle();
            rec.Height = OptionDrawLine.SizeRectangleForTransform;
            rec.Width = rec.Height;
            Canvas.SetLeft(rec, p.X - OptionDrawLine.SizeRectangleForTransform / 2);
            Canvas.SetTop(rec, p.Y - OptionDrawLine.SizeRectangleForTransform / 2);
            rec.Stroke = OptionColor.ColorSelection;
            rec.StrokeThickness = OptionDrawLine.StrokeThickness;
            rec.Fill = Brushes.Black;
            return rec;
        }

        private void PointOfRectangleOutSide_MouseMove(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();// делать перерисовку с погрешностью
        }

        public void PointOfRectangleOutSide_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        public void Rotate(double angle)
        {
            // отрисовка
            Point a, b, c, d;
            GetFourPointsOfOutSideRectangle(out a, out b, out c, out d);
            foreach (Shape shape in Shapes)
            {
                RotateTransform rotate = new RotateTransform(angle, GetCenter().X, GetCenter().Y);
                shape.RenderTransform = rotate;
            }
            this.angle = angle;
            //DrawOutSideRectangle(GetCenter(), FindLength(a, d), FindLength(a, b));
        }

        public void ScaleVertical(double _scaleX, double _scaleY, Point center)
        {
            scaleX += _scaleX;
            scaleY += _scaleY;
            foreach (Shape shape in Shapes)
            {
                ScaleTransform scale = new ScaleTransform(scaleX, scaleY, center.X, center.Y);
                shape.RenderTransform = scale;
            }
        }

        public Rectangle AddPoint(Point New,Brush brush, bool addRec, double recSize)
        {
            if (Points.Count == 0)
            {
                PointStart = New;
            }
            if (Points.Count != 0)
            {
                Shape notUsed;
                bool dotOverlaps = true;
                while (dotOverlaps)
                {
                    if (DictionaryPointLines.TryGetValue(PointEnd, out notUsed))
                        PointEnd.X += 0.000000001;
                    else
                        dotOverlaps = false;
                }
                Line line = GetLine(PointEnd, New);
                line.StrokeThickness = OptionDrawLine.StrokeThickness;
                line.Stroke = brush;
                canvas.Children.Add(line);
                Shapes.Add(line);
                DictionaryPointLines.Add(PointEnd, line);

                Line newLine = GetLine(PointEnd, New);
                newLine.Stroke = brush;
                newLine.StrokeThickness = OptionDrawLine.InvisibleStrokeThickness;
                newLine.Opacity = 0;
                InvShapes.Add(newLine);
                canvas.Children.Add(newLine);

                DictionaryInvLines.Add(line, newLine);
            }
            Points.Add(New);
            PointEnd = New;
            if (addRec)
            {
                Rectangle rec = new Rectangle();
                rec.Height = recSize;
                rec.Width = recSize;
                Canvas.SetLeft(rec, New.X - recSize / 2);
                Canvas.SetTop(rec, New.Y - recSize / 2);
                rec.Stroke = OptionColor.ColorSelection;
                rec.Fill = OptionColor.ColorOpacity;
                rec.StrokeThickness = OptionDrawLine.StrokeThicknessMainRec;
                canvas.Children.Add(rec);
                return rec;
            }
            return null;
        }

        public Line GetLine(Point start, Point end)
        {
            Line line = new Line();
            line.X1 = start.X;
            line.Y1 = start.Y;
            line.X2 = end.X;
            line.Y2 = end.Y;
            return line;
        }

        public void ChangeFigureColor(Brush brush,bool isModeEditFigures)
        {
            foreach (Shape shape in Shapes)
            {
                if (isModeEditFigures)
                {
                    if(shape is Path)
                    {
                        Path ph = (Path)shape;
                        if (ph.MinHeight > 0)
                            shape.Stroke = OptionColor.ColorKrivaya;
                        else
                            shape.Stroke = OptionColor.ColorChoosingRec;
                    }
                    else
                    {
                        shape.Stroke = brush;
                    }
                }
                else
                {
                    shape.Stroke = brush;
                }
            }
        }

        public void AddFigure(Canvas _canvas)
        {
            foreach (Shape shape in InvShapes)
            {
                _canvas.Children.Add(shape);
            }
            foreach (Shape shape in Shapes)
            {
                _canvas.Children.Add(shape);
            }
        }

        public void RemoveFigure(Canvas _canvas)
        {
            foreach (Shape shape in Shapes)
            {
                _canvas.Children.Remove(shape);
            }
        }
        
        public double FindLength(Point a, Point b)                  //ф-ла длины отрезка по координатам
        {
            return Math.Sqrt(Math.Pow((b.X - a.X), 2) + Math.Pow((b.Y - a.Y), 2));
        }

        public void GetFourPointsOfOutSideRectangle(out Point a, out Point b, out Point c, out Point d)
        {
            Point max = new Point(Int32.MinValue, Int32.MinValue);
            Point min = new Point(Int32.MaxValue, Int32.MaxValue);
            foreach (Point p in Points)
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
            a = new Point(min.X - 20, min.Y - 20);
            b = new Point(min.X - 20, max.Y + 20);
            c = new Point(max.X + 20, max.Y + 20);
            d = new Point(max.X + 20, min.Y - 20);
        }

        public void SetMiddleControlLine(Point a, Point b, Canvas _canvas)
        {
            double x = (a.X + b.X)/2;
            double y = (a.Y + b.Y) / 2;
            DrawEllipse(new Point(x, y),OptionColor.ColorSelection, OptionDrawLine.SizeWidthAndHeightRectangle, _canvas,true);
        }

        public void DrawDots(List<Point> pts, double size, Brush color, Canvas _canvas)
        {
            for (int i = 0; i < pts.Count; i++)
            {
                DrawEllipse(pts[i], color, OptionDrawLine.RisuiRegimDots, _canvas, false);
            }
        }

        public void DrawEllipse(Point p, Brush color, double size, Canvas _canvas,bool gladEllipse)
        {
            Ellipse ell = new Ellipse();
            ell.Height = size;
            ell.Width = size;
            ell.Stroke = color;
            ell.Fill = color;
            Canvas.SetLeft(ell, p.X - size / 2);
            Canvas.SetTop(ell, p.Y - size / 2);
            if (gladEllipse)
            {
                Shapes.Add(ell);
                _canvas.Children.Add(ell);
            }
            else
            {
                EllipsePoint = new Point(p.X, p.Y);
                NewPointEllipse = ell;
                _canvas.Children.Add(NewPointEllipse);
            }
        }

        public Point GetCenter()
        {
            Point max = new Point(Int32.MinValue, Int32.MinValue);
            Point min = new Point(Int32.MaxValue, Int32.MaxValue);
            foreach (Point p in Points)
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
            return new Point((max.X + min.X) / 2, (max.Y + min.Y) / 2);
        }

    }
}
