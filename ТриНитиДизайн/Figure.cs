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
        public Dictionary<Rectangle, Point> DictionaryRecPoint;
        public Dictionary<Rectangle, Tuple<Line,Line>> DictionaryPointLines;
        public Point PointStart;
        public Point PointEnd;
        public double angle;
        public Canvas canvas;
        public Rectangle SelectedRectangle;
        public Figure(Canvas _canvas)
        {
            Shapes = new List<Shape>();
            Points = new List<Point>();
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
            rec.Stroke = OptionColor.ColorDraw;
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

        public Point GetCenter()
        {
            Point max = new Point();
            Point min = new Point();
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
