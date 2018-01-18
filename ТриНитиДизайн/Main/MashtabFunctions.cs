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
        public void ScaleCanvas(double mulitplier, Point center, Canvas canvas)
        {
            double xCenter = this.ActualWidth / 2;
            double yCenter = this.ActualHeight / 2;
            double discrepancyX = (center.X - xCenter) / OptionSetka.Masshtab;
            double discrepancyY = (center.Y - yCenter) / OptionSetka.Masshtab;
            OptionSetka.Masshtab *= mulitplier;
            OptionDrawLine.StrokeThickness /= mulitplier;
            OptionDrawLine.SizeWidthAndHeightRectangle /= mulitplier;
            OptionDrawLine.SizeWidthAndHeightInvRectangle /= mulitplier;
            OptionDrawLine.InvisibleStrokeThickness /= mulitplier;
            OptionDrawLine.SizeRectangleForScale /= mulitplier;
            OptionDrawLine.SizeRectangleForRotation /= mulitplier;
            OptionDrawLine.SizeEllipseForPoints /= mulitplier;
            OptionDrawLine.RisuiRegimDots /= mulitplier;
            OptionDrawLine.StrokeThicknessMainRec /= mulitplier;
            foreach (Figure fig in ListFigure)
            {
                foreach (Shape sh in fig.Shapes)
                {
                    sh.StrokeThickness /= mulitplier;
                }
            }
            foreach (Figure fig in ListPltFigure)
            {
                foreach (Shape sh in fig.Shapes)
                {
                    sh.StrokeThickness /= mulitplier;
                }
            }
            foreach (Figure fig in LinesForGlad)
            {
                foreach (Shape sh in fig.Shapes)
                {
                    sh.StrokeThickness /= mulitplier;
                }
            }
            foreach (Shape sh in ControlLine.Shapes)
            {
                sh.StrokeThickness /= mulitplier;
            }
            foreach (UIElement element in canvas.Children)
            {
                if (element is Rectangle)
                {
                    Rectangle rec = (Rectangle)element;
                    double x = Canvas.GetLeft(rec);
                    double y = Canvas.GetTop(rec);
                    if (mulitplier > 1)
                    {
                        rec.Height /= mulitplier;
                        rec.Width /= mulitplier;
                        Canvas.SetLeft(rec, x + rec.Height / 2);
                        Canvas.SetTop(rec, y + rec.Height / 2);
                    }
                    else
                    {
                        Canvas.SetLeft(rec, x - rec.Height / 2);
                        Canvas.SetTop(rec, y - rec.Height / 2);
                        rec.Height /= mulitplier;
                        rec.Width /= mulitplier;
                    }
                    rec.StrokeThickness /= mulitplier;
                }
                if (element is Ellipse)
                {
                    Ellipse ell = (Ellipse)element;
                    double x = Canvas.GetLeft(ell);
                    double y = Canvas.GetTop(ell);
                    if (mulitplier > 1)
                    {
                        ell.Height /= mulitplier;
                        ell.Width /= mulitplier;
                        Canvas.SetLeft(ell, x + ell.Height / 2);
                        Canvas.SetTop(ell, y + ell.Height / 2);
                    }
                    else
                    {
                        Canvas.SetLeft(ell, x - ell.Height / 2);
                        Canvas.SetTop(ell, y - ell.Height / 2);
                        ell.Height /= mulitplier;
                        ell.Width /= mulitplier;
                    }
                    ell.StrokeThickness /= mulitplier;
                }
            }
            panTransform.X -= discrepancyX;
            panTransform.Y -= discrepancyY;

            zoomTransform.CenterX = xCenter;
            zoomTransform.CenterY = yCenter;
            zoomTransform.ScaleX = OptionSetka.Masshtab;
            zoomTransform.ScaleY = OptionSetka.Masshtab;
            
        }

        public void MoveCanvas(Point center, Canvas canvas)
        {
            double xCenter = this.ActualWidth / 2;
            double yCenter = this.ActualHeight / 2;
            double discrepancyX = (center.X - xCenter)/OptionSetka.Masshtab;
            double discrepancyY = (center.Y - yCenter) / OptionSetka.Masshtab;
            panTransform.X -= discrepancyX;
            panTransform.Y -= discrepancyY;
        }

    }
}
