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
            OptionSetka.DotSize /= mulitplier;
            OptionDrawLine.StrokeThickness /= mulitplier;
            OptionDrawLine.SizeWidthAndHeightRectangle /= mulitplier;
            OptionDrawLine.SizeWidthAndHeightInvRectangle /= mulitplier;
            OptionDrawLine.InvisibleStrokeThickness /= mulitplier;
            OptionDrawLine.SizeRectangleForScale /= mulitplier;
            OptionDrawLine.SizeRectangleForRotation /= mulitplier;
            OptionDrawLine.SizeEllipseForPoints /= mulitplier;
            OptionDrawLine.RisuiRegimDots /= mulitplier;
            OptionDrawLine.StrokeThicknessMainRec /= mulitplier;
            OptionDrawLine.OneDotCornerDistance /= mulitplier;
            OptionDrawLine.OneDotMiddleDistance /= mulitplier;
            foreach (Figure fig in ListFigure)
            {
                foreach (Shape sh in fig.Shapes)
                {
                    sh.StrokeThickness /= mulitplier;
                }
                if(fig.Points.Count == 1)
                {
                    Shape sh;
                    fig.DictionaryPointLines.TryGetValue(fig.Points[0], out sh);
                    Brush brush = sh.Stroke;
                    canvas.Children.Remove(sh);
                    fig.DeleteShape(sh, fig.Points[0], canvas);
                    sh = GeometryHelper.SetStarForSinglePoint(fig.Points[0], brush, canvas);
                    fig.AddShape(sh, fig.Points[0], null);
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
                    double xRecCenter = x + rec.Width / 2;
                    double yRecCenter = y + rec.Height / 2;

                    rec.Height /= mulitplier;
                    rec.Width /= mulitplier;
                    Canvas.SetLeft(rec, xRecCenter - rec.Height / 2);
                    Canvas.SetTop(rec, yRecCenter - rec.Width / 2);

                    rec.StrokeThickness /= mulitplier;
                }
                if (element is Ellipse)
                {
                    Ellipse ell = (Ellipse)element;
                    double x = Canvas.GetLeft(ell);
                    double y = Canvas.GetTop(ell);
                    double xEllCenter = x + ell.Width / 2;
                    double yEllCenter = y + ell.Height / 2;

                    ell.Height /= mulitplier;
                    ell.Width /= mulitplier;
                    Canvas.SetLeft(ell, xEllCenter - ell.Height / 2);
                    Canvas.SetTop(ell, yEllCenter - ell.Width / 2);

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

        public void SaveLastView()
        {
            PreviousView lastView = new PreviousView(OptionSetka.Masshtab, panTransform.X, panTransform.Y);
            PreviousViewList.Add(lastView);
        }

        public void LoadLastView()
        {
            PreviousView lastView = PreviousViewList[PreviousViewList.Count - 1];
            double lastMultiplier = lastView.ScaleValue / OptionSetka.Masshtab;
            Point center = new Point(this.ActualWidth / 2, this.ActualHeight / 2);
            ScaleCanvas(lastMultiplier, center, MainCanvas);
            panTransform.X = lastView.PanX;
            panTransform.Y = lastView.PanY;
            PreviousViewList.Remove(lastView);
        }

        public void ResetScale()
        {
            double reverseMultiplier = 1 / OptionSetka.Masshtab;
            Point center = new Point(this.ActualWidth / 2, this.ActualHeight / 2);
            ScaleCanvas(reverseMultiplier, center, MainCanvas);
            panTransform.X = 0;
            panTransform.Y = 0;
        }

        public void ScaleToFigure(Figure fig)
        {
            ResetScale();
            List<Point> outsidePts = fig.GetFourPointsOfOutSideRectangle(0);
            double width = outsidePts[2].X - outsidePts[0].X;
            double height = outsidePts[2].Y - outsidePts[0].Y;
            double scale;
            double multiplier;
            if(width > height)
            {
                multiplier = height / width;
                width += width * multiplier;
                scale = MainCanvas.ActualWidth / width;
            }
            else
            {
                multiplier = width / height;
                height += height*multiplier;
                scale = MainCanvas.ActualHeight / height;
            }
            multiplier = scale / OptionSetka.Masshtab;
            ScaleCanvas(multiplier, fig.GetCenter(), MainCanvas);
        }
    }
}
