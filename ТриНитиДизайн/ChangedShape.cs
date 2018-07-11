using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ТриНитиДизайн
{
    class ChangedShape
    {
        public Shape changedShape;
        private string status;
        public Point firstPoint;
        public Point firstControlPoint;
        public Point secondControlPoint;
        private Point secondPoint;
        public Rectangle rec1;
        public Rectangle rec2;
        private Canvas canvas;

        public ChangedShape(Shape _changedShape, string _status, Point _firstPoint, Point _firstControlPoint, Point _secondControlPoint, Point _secondPoint,
            Rectangle _rec1, Rectangle _rec2, Canvas _canvas)
        {
            changedShape = _changedShape;
            status = _status;
            firstPoint = _firstPoint;
            firstControlPoint = _firstControlPoint;
            secondControlPoint = _secondControlPoint;
            secondPoint = _secondPoint;
            rec1 = _rec1;
            rec2 = _rec2;
            canvas = _canvas;
        }

        public void RemoveRectangleAndShape()
        {
            if(rec1 != null)
                canvas.Children.Remove(rec1);
            if (rec2 != null)
                canvas.Children.Remove(rec2);
            canvas.Children.Remove(changedShape);
        }

        public void ManipulateShape(Vector delta)
        {
            if (status.Equals("first"))
            {
                firstPoint += delta;
                rec1 = GeometryHelper.DrawRectangle(firstPoint, 
                    false, false, OptionDrawLine.strokeThickness, OptionColor.colorInactive, canvas);
            }
            else if (status.Equals("both"))
            {
                firstPoint += delta;
                if (firstControlPoint != null)
                    firstControlPoint += delta;
                if (secondControlPoint != null)
                    secondControlPoint += delta;
                secondPoint += delta;
                rec1 = GeometryHelper.DrawRectangle(firstPoint,
                    false, false, OptionDrawLine.strokeThickness, OptionColor.colorInactive, canvas);
                rec2 = GeometryHelper.DrawRectangle(secondPoint,
                    false, false, OptionDrawLine.strokeThickness, OptionColor.colorInactive, canvas);
            }
            else if (status.Equals("second"))
            {
                secondPoint += delta;
                rec2 = GeometryHelper.DrawRectangle(secondPoint,
                    false, false, OptionDrawLine.strokeThickness, OptionColor.colorInactive, canvas);
            }
            else
            {
                firstPoint += delta;
                rec1 = GeometryHelper.DrawRectangle(firstPoint,
                    false, false, OptionDrawLine.strokeThickness, OptionColor.colorInactive, canvas);
            }
            if(changedShape is Line)
                changedShape = GeometryHelper.SetLine(OptionColor.colorActive, firstPoint, secondPoint,false, canvas);
            else
            {
                if (changedShape.MinHeight == 5)
                {
                    if (status.Equals("second"))
                        secondControlPoint += delta;
                    else if (status.Equals("first"))
                        firstControlPoint += delta;
                    changedShape = GeometryHelper.SetBezier(OptionColor.colorCurve, firstPoint, firstControlPoint, secondControlPoint, secondPoint, canvas);
                }
                else if (changedShape.MinHeight == 10)
                    changedShape = GeometryHelper.SetArc(OptionColor.colorArc, firstPoint, secondPoint, firstControlPoint, canvas);
                else
                    changedShape = GeometryHelper.SetStarForSinglePoint(firstPoint,OptionColor.colorActive, canvas);
            }
        }
    }
}
