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
        public void ScaleCanvas(double multiplier, Point center, Canvas canvas)
        {
            double xCenter = this.ActualWidth / 2;
            double yCenter = this.ActualHeight / 2;
            double discrepancyX = (center.X - xCenter) / OptionSetka.Masshtab;
            double discrepancyY = (center.Y - yCenter) / OptionSetka.Masshtab;

            if(OptionSetka.Masshtab * multiplier > 32)
                multiplier = 32 / OptionSetka.Masshtab;
            else
                OptionSetka.Masshtab *= multiplier;

            OptionDrawLine.StrokeThickness /= multiplier;
            OptionDrawLine.SizeWidthAndHeightRectangle /= multiplier;
            OptionDrawLine.InvisibleStrokeThickness /= multiplier;
            OptionDrawLine.SizeRectangleForScale /= multiplier;
            OptionDrawLine.SizeRectangleForRotation /= multiplier;
            OptionDrawLine.StrokeThicknessMainRec /= multiplier;
            OptionDrawLine.OneDotCornerDistance /= multiplier;
            OptionDrawLine.OneDotMiddleDistance /= multiplier;

            foreach (Figure fig in ListFigure)
            {
                RescaleAllShapesInFigure(fig, multiplier,canvas);
            }
            foreach (Figure fig in ListPltFigure)
            {
                RescaleAllShapesInFigure(fig, multiplier, canvas);
            }
            foreach(Line ln in centerLines)
            {
                ln.StrokeThickness /= multiplier;
            }
            foreach (Line ln in otshitLines)
            {
                ln.StrokeThickness /= multiplier;
            }
            foreach (Figure fig in LinesForGlad)
            {
                RescaleAllShapesInFigure(fig, multiplier,canvas);
            }
            foreach (Figure fig in CopyGroup)
            {
                RescaleAllShapesInFigure(fig, multiplier, canvas);
            }
            foreach (Figure fig in DeletedGroup)
            {
                RescaleAllShapesInFigure(fig, multiplier, canvas);
            }
            foreach (Shape sh in ControlLine.Shapes)
            {
                sh.StrokeThickness /= multiplier;
            }
            if(transRectangles!=null)
                foreach (Rectangle rec in transRectangles)
                {
                    GeometryHelper.RescaleRectangle(rec, multiplier);
                }
            if (invisibleRectangles != null)
                foreach (Rectangle rec in invisibleRectangles)
                {
                    GeometryHelper.RescaleRectangle(rec, multiplier);
                }
            if(firstRec!=null)
                GeometryHelper.RescaleRectangle(firstRec, multiplier);
            if(lastRec!= null)
                GeometryHelper.RescaleRectangle(lastRec, multiplier);

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
            if(width > height)
            {
                width += width * 0.8;
                scale = MainCanvas.ActualWidth / width;
            }
            else
            {
                height += height*0.8;
                scale = MainCanvas.ActualHeight / height;
            }
            if (scale > 32)
                scale = 32;
            ScaleCanvas(scale, fig.GetCenter(), MainCanvas);
        }

        private void RescaleAllShapesInFigure(Figure fig, double multiplier, Canvas canvas)
        {
            foreach (Shape sh in fig.Shapes)
            {
                if (sh is Ellipse)
                    GeometryHelper.RescaleEllipse((Ellipse)sh, multiplier);
            }
            RescaleThicknessForShapes(fig.Shapes,multiplier);
            RescaleThicknessForShapes(fig.InvShapes, multiplier);
            RescaleThicknessForShapes(fig.tempInvShapes, multiplier);
            RescaleThicknessForShapes(fig.tempShapes, multiplier);
            foreach(Rectangle rec in fig.RectangleOfFigures)
            {
                GeometryHelper.RescaleRectangle(rec, multiplier);
            }
            foreach (Ellipse ell in fig.dotsForFigure)
                GeometryHelper.RescaleEllipse(ell, multiplier);
            if(fig.NewPointEllipse != null)
                GeometryHelper.RescaleEllipse(fig.NewPointEllipse, multiplier);
            if (fig.Points.Count == 1)
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

        private void RescaleThicknessForShapes(List<Shape> shapes, double multiplier)
        {
            foreach(Shape sh in shapes)
            {
                sh.StrokeThickness /= multiplier;
            }
        }

    }
}
