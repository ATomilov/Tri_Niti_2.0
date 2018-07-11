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
            double discrepancyX = (center.X - xCenter) / OptionGrid.scaleMultiplier;
            double discrepancyY = (center.Y - yCenter) / OptionGrid.scaleMultiplier;

            if(OptionGrid.scaleMultiplier * multiplier > 32)
                multiplier = 32 / OptionGrid.scaleMultiplier;
            else
                OptionGrid.scaleMultiplier *= multiplier;

            OptionDrawLine.strokeThickness /= multiplier;
            OptionDrawLine.sizeRectangle /= multiplier;
            OptionDrawLine.invisibleStrokeThickness /= multiplier;
            OptionDrawLine.sizeRectangleForScale /= multiplier;
            OptionDrawLine.sizeRectangleForRotation /= multiplier;
            OptionDrawLine.strokeThicknessMainRec /= multiplier;
            OptionDrawLine.oneDotCornerDistance /= multiplier;
            OptionDrawLine.oneDotMiddleDistance /= multiplier;
            OptionDrawLine.cursorModeRectangleDistance /= multiplier;

            foreach (Figure fig in listFigure)
            {
                RescaleAllShapesInFigure(fig, multiplier,canvas);
            }
            foreach (Figure fig in listPltFigure)
            {
                RescaleAllShapesInFigure(fig, multiplier, canvas);
            }
            foreach(Line ln in centerLines)
            {
                ln.StrokeThickness /= multiplier;
            }
            foreach (Line ln in unembroidLines)
            {
                ln.StrokeThickness /= multiplier;
            }
            foreach (Figure fig in linesForSatin)
            {
                RescaleAllShapesInFigure(fig, multiplier,canvas);
            }
            foreach (Figure fig in copyGroup)
            {
                RescaleAllShapesInFigure(fig, multiplier, canvas);
            }
            foreach (Figure fig in deletedGroup)
            {
                RescaleAllShapesInFigure(fig, multiplier, canvas);
            }
            foreach (Shape sh in controlLine.Shapes)
            {
                sh.StrokeThickness /= multiplier;
            }
            if (transRectangles != null)
            {
                foreach (Rectangle rec in transRectangles)
                {
                    GeometryHelper.RescaleRectangle(rec, multiplier);
                }
                MoveTransformRectangles(OptionDrawLine.cursorModeRectangleDistance,multiplier);
            }
            if (invisibleRectangles != null)
                foreach (Rectangle rec in invisibleRectangles)
                {
                    GeometryHelper.RescaleRectangle(rec, multiplier);
                }

            foreach (Shape sh in controlLine.Shapes)
            {
                if (sh is Ellipse)
                    GeometryHelper.RescaleEllipse((Ellipse)sh, multiplier);
            }

            if (firstRec!=null)
                GeometryHelper.RescaleRectangle(firstRec, multiplier);
            if(lastRec!= null)
                GeometryHelper.RescaleRectangle(lastRec, multiplier);

            panTransform.X -= discrepancyX;
            panTransform.Y -= discrepancyY;

            zoomTransform.CenterX = xCenter;
            zoomTransform.CenterY = yCenter;
            zoomTransform.ScaleX = OptionGrid.scaleMultiplier;
            zoomTransform.ScaleY = OptionGrid.scaleMultiplier;
        }

        public void MoveCanvas(Point center, Canvas canvas)
        {
            double xCenter = this.ActualWidth / 2;
            double yCenter = this.ActualHeight / 2;
            double discrepancyX = (center.X - xCenter)/OptionGrid.scaleMultiplier;
            double discrepancyY = (center.Y - yCenter) / OptionGrid.scaleMultiplier;
            panTransform.X -= discrepancyX;
            panTransform.Y -= discrepancyY;
        }

        public void SaveLastView()
        {
            PreviousView lastView = new PreviousView(OptionGrid.scaleMultiplier, panTransform.X, panTransform.Y);
            previousViewList.Add(lastView);
        }

        public void LoadLastView()
        {
            PreviousView lastView = previousViewList[previousViewList.Count - 1];
            double lastMultiplier = lastView.ScaleValue / OptionGrid.scaleMultiplier;
            Point center = new Point(this.ActualWidth / 2, this.ActualHeight / 2);
            ScaleCanvas(lastMultiplier, center, MainCanvas);
            panTransform.X = lastView.PanX;
            panTransform.Y = lastView.PanY;
            previousViewList.Remove(lastView);
        }

        public void ResetScale()
        {
            double reverseMultiplier = 1 / OptionGrid.scaleMultiplier;
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
            foreach(Rectangle rec in fig.RectangleOfFigures)
            {
                GeometryHelper.RescaleRectangle(rec, multiplier);
            }
            foreach (Ellipse ell in fig.dotsForFigure)
                GeometryHelper.RescaleEllipse(ell, multiplier);
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

        private void MoveTransformRectangles(double curDistance, double multiplier)
        {
            double prevDistance = multiplier * curDistance;
            double offset = curDistance - prevDistance;

            Canvas.SetTop(transRectangles[0], Canvas.GetTop(transRectangles[0]) - offset);

            Canvas.SetTop(transRectangles[1], Canvas.GetTop(transRectangles[1]) - offset);
            Canvas.SetLeft(transRectangles[1], Canvas.GetLeft(transRectangles[1]) + offset);

            Canvas.SetLeft(transRectangles[2], Canvas.GetLeft(transRectangles[2]) + offset);

            Canvas.SetTop(transRectangles[3], Canvas.GetTop(transRectangles[3]) + offset);
            Canvas.SetLeft(transRectangles[3], Canvas.GetLeft(transRectangles[3]) + offset);

            Canvas.SetTop(transRectangles[4], Canvas.GetTop(transRectangles[4]) + offset);

            Canvas.SetTop(transRectangles[5], Canvas.GetTop(transRectangles[5]) + offset);
            Canvas.SetLeft(transRectangles[5], Canvas.GetLeft(transRectangles[5]) - offset);

            Canvas.SetLeft(transRectangles[6], Canvas.GetLeft(transRectangles[6]) - offset);

            Canvas.SetTop(transRectangles[7], Canvas.GetTop(transRectangles[7]) - offset);
            Canvas.SetLeft(transRectangles[7], Canvas.GetLeft(transRectangles[7]) - offset);
        }
    }
}
