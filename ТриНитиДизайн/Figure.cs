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
        public List<Shape> Shapes;
        public List<Point> Points;
        public List<int> PointsCount;
        public List<Rectangle> RectangleOfFigures;
        public Dictionary<Rectangle, Point> DictionaryRecPoint;
        public Dictionary<Rectangle, Tuple<Line,Line>> DictionaryPointLines;
        public Point PointStart;
        public Point PointEnd;
        public double angle;
        public Canvas canvas;
        public Rectangle SelectedRectangle;
        public Rectangle RectangleOfFigure;
        public Figure(Canvas _canvas)
        {
            Shapes = new List<Shape>();
            Points = new List<Point>();
            PointsCount = new List<int>();
            angle = 0;
            canvas = _canvas;
            DictionaryRecPoint = new Dictionary<Rectangle, Point>();
            DictionaryPointLines = new Dictionary<Rectangle, Tuple<Line, Line>>();
        }

        public void AddShape(Shape shape)
        {
            Shapes.Add(shape);
        }

        public void DeleteShape(Shape shape)
        {
            Shapes.Remove(shape);// ??? point
        }
<<<<<<< HEAD

        public void DrawAllRectangles(double size, Brush brush)
=======
        
        public void DrawAllRectangles(double size)
>>>>>>> 90023ce0a7123ced2f8766aec51496e5f27b0949
        {
            foreach(Point p in Points)
            {
                Rectangle rec = new Rectangle();
                rec.Height = size;
                rec.Width = size;
                Canvas.SetLeft(rec, p.X - size/2);
                Canvas.SetTop(rec, p.Y - size/2);
                rec.Stroke = OptionColor.ColorSelection;
                rec.Fill = brush;
                rec.StrokeThickness = 1;
                canvas.Children.Add(rec);
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
            rec.StrokeThickness = 1;
            canvas.Children.Add(rec);
        }


        public void ClearFigure()
        {
            Shapes = new List<Shape>();
            Points = new List<Point>();
            angle = 0;
            DictionaryRecPoint = new Dictionary<Rectangle, Point>();
            DictionaryPointLines = new Dictionary<Rectangle, Tuple<Line, Line>>();
        }

        public void SetDot(Point centerPoint, string type, Canvas CurCanvas)         //отрисовка точки, red - красная, blue - зеленая, grid - точка сетки
        {
            Path myPath = new Path();
            EllipseGeometry myEllipse = new EllipseGeometry();
            myEllipse.Center = centerPoint;
            myEllipse.RadiusX = 3;
            myEllipse.RadiusY = 3;
            if (type.Equals("red"))
            {
                myPath.Stroke = System.Windows.Media.Brushes.Red;
                myPath.Fill = System.Windows.Media.Brushes.Red;
            }
            if (type.Equals("blue"))
            {
                myPath.Stroke = System.Windows.Media.Brushes.Blue;
                myPath.Fill = System.Windows.Media.Brushes.Blue;
            }
            if (type.Equals("green"))
            {
                myPath.Stroke = System.Windows.Media.Brushes.Green;
                myPath.Fill = System.Windows.Media.Brushes.Green;
            }
            if (type.Equals("black"))
            {
                myPath.Stroke = System.Windows.Media.Brushes.RoyalBlue;
                myPath.Fill = System.Windows.Media.Brushes.Black;
            }
            if (type.Equals("grid"))
            {
                myPath.Stroke = System.Windows.Media.Brushes.Black;
                myEllipse.RadiusX = 1;
                myEllipse.RadiusY = 1;
            }

            myPath.Data = myEllipse;
            CurCanvas.Children.Add(myPath);
        }

        public void DrawOutSideRectanglePoints()
        {
            List<Point> PointsOutSideRectangle = new List<Point>();
            Point a, b, c, d;
            SetDot(GetCenter(out a, out b, out c, out d), "red", canvas);
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
        }

        public void DrawRectangle(Point p, Canvas canvas)
        {
            Rectangle rec = new Rectangle();
            rec.Height = 10;
            rec.Width = 10;
            Canvas.SetLeft(rec, p.X - 5);
            Canvas.SetTop(rec, p.Y - 5);
            rec.Stroke = OptionColor.ColorSelection;
            rec.StrokeThickness = 1;
            rec.Fill = Brushes.Black;
            rec.MouseDown += new MouseButtonEventHandler(PointOfRectangleOutSide_MouseDown);
            rec.MouseUp += new MouseButtonEventHandler(PointOfRectangleOutSide_MouseUp);
            canvas.Children.Add(rec);
        }

        void PointOfRectangleOutSide_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            ((Rectangle)sender).Fill = Brushes.Red;
        }
        void PointOfRectangleOutSide_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
            ((Rectangle)sender).Fill = Brushes.Yellow;
        }

        public void Rotate(int _angle)
        {
            // отрисовка
            Point a, b, c, d;
            SetDot(GetCenter(out a, out b, out c, out d), "red", canvas);
            angle += _angle;
            //foreach (Shape shape in Shapes)
            //{
                
            //    RotateTransform rotate = new RotateTransform(angle, GetCenter(out a, out b, out c, out d).X, GetCenter(out a, out b, out c, out d).Y);
            //    shape.RenderTransform = rotate;
                
            //}
            DrawOutSideRectangle(GetCenter(out a, out b, out c, out d), FindLength(a, d), FindLength(a, b));
        }
<<<<<<< HEAD


=======
        
>>>>>>> 90023ce0a7123ced2f8766aec51496e5f27b0949
        public void AddPoint(Point New)
        {
            if (Points.Count == 0)
            {
                PointStart = New;
            }
            if(Points.Count != 0)
            {
                Line line = GetLine(PointEnd, New);
                line.StrokeThickness = 1;
                line.Stroke = OptionColor.ColorDraw;
                canvas.Children.Add(line);
                AddShape(line);
            }
            Points.Add(New);
            PointEnd = New;
            Rectangle rec = new Rectangle();
            rec.Height = 8;
            rec.Width = 8;
            Canvas.SetLeft(rec, New.X - 4);
            Canvas.SetTop(rec, New.Y - 4);
            rec.Stroke = OptionColor.ColorSelection;
            rec.StrokeThickness = 1;
            canvas.Children.Add(rec);
            DictionaryRecPoint.Add(rec, New);
            if (Shapes.Where(p => p is Line).Count() > 1)
            {
                Tuple<Line, Line> t = new Tuple<Line, Line>((Line)Shapes[Shapes.Count - 1], (Line)Shapes[Shapes.Count - 2]);
                DictionaryPointLines.Add(rec, t);
            }
            if (Shapes.Where(p=>p is Line).Count() == 1)
                DictionaryPointLines.Add(rec, new Tuple<Line, Line>((Line)Shapes[Shapes.Count - 1], null));



            canvas.MouseMove += new MouseEventHandler(PointMouseMove);
            rec.MouseDown += new MouseButtonEventHandler (PointMouseClick);
            //Подписать поинт на изменение координат
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



        public void ClearFigure(Canvas _canvas)
        {
            
            foreach (Shape shape in Shapes)
            {
                _canvas.Children.Remove(shape);
            }
        }

        public void AddFigure(Canvas _canvas)
        {
            foreach (Shape shape in Shapes)
            {
                _canvas.Children.Add(shape);
            }
        }

        public double FindLength(Point a, Point b)                  //ф-ла длины отрезка по координатам
        {
            return Math.Sqrt(Math.Pow((b.X - a.X), 2) + Math.Pow((b.Y - a.Y), 2));
        }

        public Point GetCenter(out Point a, out Point b, out Point c, out Point d)
        {
            Point max = new Point(-100,-100);
            Point min = new Point(40000,40000);
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
            return new Point((max.X + min.X) / 2, (max.Y + min.Y) / 2);
        }

        private void PointMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed )
            {
                if (SelectedRectangle !=null)
                {
                    Point point = DictionaryRecPoint[SelectedRectangle];
                    SelectedRectangle.Stroke = OptionColor.ColorSelection;

                    Canvas.SetLeft(SelectedRectangle, e.GetPosition(canvas).X - 4);
                    Canvas.SetTop(SelectedRectangle, e.GetPosition(canvas).Y - 4);

                    Line l1 = DictionaryPointLines[SelectedRectangle].Item1;
                    Line l2 = DictionaryPointLines[SelectedRectangle].Item2;
                    if (l1 != null)
                    {
                        l1.X2 = point.X;
                        l1.Y2 = point.Y;
                    }
                    if (l2 != null)
                    {
                        l2.X1 = point.X;
                        l2.Y1 = point.Y;
                    }

                }
            }
        }

        private void PointMouseClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                SelectedRectangle = (Rectangle)sender;
            }
        }


    }
}
