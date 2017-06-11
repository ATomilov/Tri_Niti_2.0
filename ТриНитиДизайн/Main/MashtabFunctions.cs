﻿using System;
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
        public void Plus(Canvas canvas, Point center)
        {
            OptionSetka.Masshtab *= 2;
            OptionDrawLine.StrokeThickness /= 2;
            OptionDrawLine.SizeWidthAndHeightRectangle /= 2;
            OptionDrawLine.InvisibleStrokeThickness /= 2;
            OptionDrawLine.SizeRectangleForTransform /= 2;
            foreach (Figure fig in ListFigure)
            {
                foreach(Shape sh in fig.Shapes)
                {
                    sh.StrokeThickness /= 2;
                }
            }
            foreach (Figure fig in ListPltFigure)
            {
                foreach (Shape sh in fig.Shapes)
                {
                    sh.StrokeThickness /= 2;
                }
            }
            foreach (Figure fig in LinesForGlad)
            {
                foreach (Shape sh in fig.Shapes)
                {
                    sh.StrokeThickness /= 2;
                }
            }
            foreach (Shape sh in ControlLine.Shapes)
            {
                sh.StrokeThickness /= 2;
            }
            foreach (UIElement element in canvas.Children)
            {
                if (element is Rectangle)
                {
                    Rectangle rec = (Rectangle)element;
                    double x = Canvas.GetLeft(rec);
                    double y = Canvas.GetTop(rec);
                    rec.Height /= 2;
                    rec.Width /= 2;
                    Canvas.SetLeft(rec, x + rec.Height / 2);
                    Canvas.SetTop(rec, y + rec.Height / 2);
                    rec.StrokeThickness /= 2;
                }
                if (element is Ellipse)
                {
                    Ellipse ell = (Ellipse)element;
                    double x = Canvas.GetLeft(ell);
                    double y = Canvas.GetTop(ell);
                    ell.Height /= 2;
                    ell.Width /= 2;
                    Canvas.SetLeft(ell, x + ell.Height / 2);
                    Canvas.SetTop(ell, y + ell.Height / 2);
                    ell.StrokeThickness /= 2;
                }
            }
            ScaleTransform scaleTransform = new ScaleTransform(OptionSetka.Masshtab, OptionSetka.Masshtab);
            scaleTransform.CenterX = center.X;
            scaleTransform.CenterY = center.Y;
            canvas.RenderTransform = scaleTransform;
        }

        public void Minus(Canvas canvas, Point center)
        {
            OptionSetka.Masshtab /= 2;
            OptionDrawLine.StrokeThickness *= 2;
            OptionDrawLine.SizeWidthAndHeightRectangle *= 2;
            OptionDrawLine.InvisibleStrokeThickness *= 2;
            OptionDrawLine.SizeRectangleForTransform *= 2;
            foreach (Figure fig in ListFigure)
            {
                foreach (Shape sh in fig.Shapes)
                {
                    sh.StrokeThickness *= 2;
                }
            }
            foreach (Figure fig in ListPltFigure)
            {
                foreach (Shape sh in fig.Shapes)
                {
                    sh.StrokeThickness *= 2;
                }
            }
            foreach (Figure fig in LinesForGlad)
            {
                foreach (Shape sh in fig.Shapes)
                {
                    sh.StrokeThickness *= 2;
                }
            }
            foreach (Shape sh in ControlLine.Shapes)
            {
                sh.StrokeThickness *= 2;
            }
            foreach (UIElement element in canvas.Children)
            {
                if (element is Rectangle)
                {
                    Rectangle rec = (Rectangle)element;
                    double x = Canvas.GetLeft(rec);
                    double y = Canvas.GetTop(rec);

                    Canvas.SetLeft(rec, x - rec.Height / 2);
                    Canvas.SetTop(rec, y - rec.Height / 2);
                    rec.Height *= 2;
                    rec.Width *= 2;

                    rec.StrokeThickness *= 2;
                }
                if (element is Ellipse)
                {
                    Ellipse ell = (Ellipse)element;
                    double x = Canvas.GetLeft(ell);
                    double y = Canvas.GetTop(ell);
                    ell.Height *= 2;
                    ell.Width *= 2;
                    Canvas.SetLeft(ell, x - ell.Height / 2);
                    Canvas.SetTop(ell, y - ell.Height / 2);
                    ell.StrokeThickness *= 2;
                }
            }
            ScaleTransform scaleTransform = new ScaleTransform(OptionSetka.Masshtab, OptionSetka.Masshtab);
            scaleTransform.CenterX = center.X;
            scaleTransform.CenterY = center.Y;
            canvas.RenderTransform = scaleTransform;
        }

        public void PlusWithFixedOptions(Canvas canvas, Point center, double masshtab, double stroke, double sizerectangle, double invisiblestroke, double sizerectanglefortransform)
        {
            OptionSetka.Masshtab = masshtab;
            OptionDrawLine.StrokeThickness = stroke;
            OptionDrawLine.SizeWidthAndHeightRectangle = sizerectangle;
            OptionDrawLine.InvisibleStrokeThickness = invisiblestroke;
            OptionDrawLine.SizeRectangleForTransform = sizerectanglefortransform;
            foreach (Figure fig in ListFigure)
            {
                foreach (Shape sh in fig.Shapes)
                {
                    sh.StrokeThickness = stroke;
                }
            }
            foreach (UIElement element in canvas.Children)
            {
                if (element is Rectangle)
                {
                    Rectangle rec = (Rectangle)element;
                    double x = Canvas.GetLeft(rec);
                    double y = Canvas.GetTop(rec);
                    rec.Height = sizerectangle;
                    rec.Width = sizerectangle;
                    Canvas.SetLeft(rec, x + rec.Height / 2);
                    Canvas.SetTop(rec, y + rec.Height / 2);
                    rec.StrokeThickness = stroke;
                }
            }
            ScaleTransform scaleTransform = new ScaleTransform(masshtab, masshtab);
            scaleTransform.CenterX = center.X;
            scaleTransform.CenterY = center.Y;
            canvas.RenderTransform = scaleTransform;
        }

        public void MinusWithFixedOptions(Canvas canvas, Point center, double masshtab, double stroke, double sizerectangle, double invisiblestroke, double sizerectanglefortransform)
        {
            OptionSetka.Masshtab = masshtab;
            OptionDrawLine.StrokeThickness = stroke;
            OptionDrawLine.SizeWidthAndHeightRectangle = sizerectangle;
            OptionDrawLine.InvisibleStrokeThickness = invisiblestroke;
            OptionDrawLine.SizeRectangleForTransform = sizerectanglefortransform;
            foreach (Figure fig in ListFigure)
            {
                foreach (Shape sh in fig.Shapes)
                {
                    sh.StrokeThickness = stroke;
                }
            }
            foreach (UIElement element in canvas.Children)
            {
                if (element is Rectangle)
                {
                    Rectangle rec = (Rectangle)element;
                    double x = Canvas.GetLeft(rec);
                    double y = Canvas.GetTop(rec);

                    Canvas.SetLeft(rec, x - rec.Height / 2);
                    Canvas.SetTop(rec, y - rec.Height / 2);
                    rec.Height = sizerectangle;
                    rec.Width = sizerectangle;

                    rec.StrokeThickness = stroke;
                }
            }
            ScaleTransform scaleTransform = new ScaleTransform(masshtab, masshtab);
            scaleTransform.CenterX = center.X;
            scaleTransform.CenterY = center.Y;
            canvas.RenderTransform = scaleTransform;
        }
    }
}
