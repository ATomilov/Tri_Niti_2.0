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
        public void RedrawEverything(List<Figure> FigureList,int ChosenFigure, bool AllRectanglesOn, Canvas canvas)
        {
            canvas.Children.Clear();
            SetkaFigure.AddFigure(canvas);
            for(int i = 0; i < FigureList.Count;i++)
            {
                FigureList[i].AddFigure(canvas);                        //можно не перерисовывать каждый раз
                if (AllRectanglesOn && i == ChosenFigure)
                {
                    FigureList[i].DrawAllRectangles();
                }
            }
            if (OptionRegim.regim == Regim.RegimFigure || OptionRegim.regim == Regim.RegimTatami)
            {
                DrawInvisibleRectangles(canvas);
            }
            for (int i = 0; i < ListPltFigure.Count; i++)
            {
                ListPltFigure[i].AddFigure(canvas);
            }
        }

        public void DrawFirstAndLastRectangle()
        {
            MainCanvas.Children.Remove(lastRec);
            MainCanvas.Children.Remove(firstRec);
            bool isRegimCursor = false;
            if (OptionRegim.regim == Regim.RegimCursor)
                isRegimCursor = true;
            firstRec = DrawRectangle(ListFigure[IndexFigure].PointStart, false, isRegimCursor, OptionDrawLine.StrokeThickness, OptionColor.ColorOpacity, MainCanvas);
            lastRec = DrawRectangle(ListFigure[IndexFigure].PointEnd, false, false, OptionDrawLine.StrokeThicknessMainRec, OptionColor.ColorOpacity, MainCanvas);
        }

        public void DrawInvisibleRectangles(Canvas canvas)
        {
            if (OptionRegim.regim == Regim.RegimGlad)
            {
                List<Point> firstPts = new List<Point>();
                List<Point> secondPts = new List<Point>();
                if (ListFigure[FirstGladFigure].tempPoints.Count > 0)
                    firstPts = ListFigure[FirstGladFigure].tempPoints;
                else
                    firstPts = ListFigure[FirstGladFigure].Points;

                if (ListFigure[SecondGladFigure].tempPoints.Count > 0)
                    secondPts = ListFigure[SecondGladFigure].tempPoints;
                else
                    secondPts = ListFigure[SecondGladFigure].Points;

                for (int i = 0; i < firstPts.Count; i++)
                    DrawRectangle(firstPts[i], true,false, OptionDrawLine.StrokeThickness, OptionColor.ColorOpacity, canvas);
                for (int i = 0; i < secondPts.Count; i++)
                    DrawRectangle(secondPts[i], true,false, OptionDrawLine.StrokeThickness, OptionColor.ColorOpacity, canvas);
            }
            else
            {
                List<Point> pts = new List<Point>();
                if (ListFigure[IndexFigure].tempPoints.Count > 0)
                    pts = ListFigure[IndexFigure].tempPoints;
                else
                    pts = ListFigure[IndexFigure].Points;

                for (int i = 0; i < pts.Count; i++)
                    DrawRectangle(pts[i], true,false, OptionDrawLine.StrokeThickness, OptionColor.ColorOpacity, canvas);
            }
        }
        
        private Rectangle DrawRectangle(Point p, bool invRectangles,bool smallRec, double thickness , Brush brush, Canvas canvas)
        {
            Rectangle rec = new Rectangle();
            if (!invRectangles)
            {
                rec.Height = OptionDrawLine.SizeWidthAndHeightRectangle;
                rec.Width = OptionDrawLine.SizeWidthAndHeightRectangle;
            }
            else
            {
                rec.Height = OptionDrawLine.SizeWidthAndHeightInvRectangle;
                rec.Width = OptionDrawLine.SizeWidthAndHeightInvRectangle;
            }
            if (smallRec)
            {
                rec.Height = OptionDrawLine.SizeWidthAndHeightRectangle/1.5;
                rec.Width = OptionDrawLine.SizeWidthAndHeightRectangle/1.5;
            }
            Canvas.SetLeft(rec, p.X - rec.Height / 2);
            Canvas.SetTop(rec, p.Y - rec.Width / 2);

            rec.Fill = brush;
            rec.Stroke = OptionColor.ColorSelection;
            rec.StrokeThickness = thickness;
            if (invRectangles)
                rec.Opacity = 0;
            canvas.Children.Add(rec);
            return rec;
        }
        
        public void BreakFigureOrMakeGladFigure(List<Figure> FigureList, Object clickedShape, Canvas canvas)
        {
            if (clickedShape is Line || clickedShape is Path)
            {
                double x = 0;
                double y = 0;
                if (clickedShape is Line)
                {
                    Line clickedLine = (Line)clickedShape;
                    x = clickedLine.X1;
                    y = clickedLine.Y1;
                }
                else
                {
                    Path path = (Path)clickedShape;
                    for (int i = 0; i < FigureList.Count; i++)
                    {
                        var invLine = FigureList[i].DictionaryInvLines.FirstOrDefault(z => z.Value == path);
                        var point = FigureList[i].DictionaryPointLines.FirstOrDefault(z => z.Value == invLine.Key);      //TODO: улучшить
                        if (point.Value != null)
                        {
                            x = point.Key.X;
                            y = point.Key.Y;
                            break;
                        }
                    }
                }
                if (ListFigure[IndexFigure].Points.Count > 1)
                {
                    for (int i = 0; i < ListFigure.Count; i++)
                    {
                        if (OptionRegim.regim == Regim.RegimFigure)
                        {
                            if (i != IndexFigure)
                            {
                                if (ListFigure[i].DictionaryPointLines.ContainsKey(new Point(x, y)) == true)
                                {
                                    FirstGladFigure = IndexFigure;
                                    SecondGladFigure = i;
                                    ListFigure[i].ChangeFigureColor(OptionColor.ColorChoosingRec, false);
                                    ShowJoinMessage(LinesForGlad, ListFigure[IndexFigure], ListFigure[SecondGladFigure], MainCanvas);
                                    break;
                                }
                            }
                        }
                        if (OptionRegim.regim == Regim.RegimGlad)
                        {
                            if (i == FirstGladFigure || i == SecondGladFigure)
                            {
                                if (ListFigure[i].DictionaryPointLines.ContainsKey(new Point(x, y)) == true)
                                {
                                    ListFigure[FirstGladFigure].ChangeFigureColor(OptionColor.ColorChoosingRec, false);
                                    ListFigure[SecondGladFigure].ChangeFigureColor(OptionColor.ColorChoosingRec, false);
                                    ShowBreakMessage();
                                    break;
                                }
                            }
                        }
                        if (OptionRegim.regim == Regim.RegimTatami)
                        {
                            if (i == IndexFigure)
                            {
                                if (ListFigure[i].DictionaryPointLines.ContainsKey(new Point(x, y)) == true)
                                {
                                    ListFigure[i].ChangeFigureColor(OptionColor.ColorChoosingRec, false);
                                    ShowBreakMessage();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ShowBreakMessage()
        {
            string sMessageBoxText = "Разорвать?";
            string sCaption = "Окно";

            MessageBoxButton btnMessageBox = MessageBoxButton.OKCancel;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.OK:
                    {
                        if (OptionRegim.regim == Regim.RegimGlad)
                        {
                            OptionRegim.regim = Regim.RegimFigure;
                            ListFigure[FirstGladFigure].regimFigure = Regim.RegimFigure;
                            ListFigure[SecondGladFigure].regimFigure = Regim.RegimFigure;
                            ListFigure[FirstGladFigure].LoadCurrentShapes();
                            ListFigure[SecondGladFigure].LoadCurrentShapes();
                            ChangeFiguresColor(ListFigure, MainCanvas);
                            FirstGladFigure = -1;
                            SecondGladFigure = -1;
                        }
                        else
                        {
                            OptionRegim.regim = Regim.RegimFigure;
                            ListFigure[IndexFigure].regimFigure = Regim.RegimFigure;
                            ListFigure[IndexFigure].LoadCurrentShapes();
                        }
                        ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
                        RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
                        DrawFirstAndLastRectangle();
                        ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].Points, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                        break;
                    }

                case MessageBoxResult.Cancel:
                    {
                        ChangeFiguresColor(ListFigure, MainCanvas);
                        break;
                    }
            }
        }

        public void ChooseFirstOrLastRectangle(Rectangle clickedRec, bool isFirstRectangle, Canvas canvas)
        {
            if(isFirstRectangle)
            {
                if (Canvas.GetLeft(clickedRec) == Canvas.GetLeft(lastRec) && Canvas.GetTop(clickedRec) == Canvas.GetTop(lastRec))
                {
                    Point pfirstRec = new Point(Canvas.GetLeft(firstRec) + firstRec.Width / 2, Canvas.GetTop(firstRec) + firstRec.Height / 2);
                    Point pLastRec = new Point(Canvas.GetLeft(lastRec) + lastRec.Width / 2, Canvas.GetTop(lastRec) + lastRec.Height / 2);
                    canvas.Children.Remove(lastRec);
                    canvas.Children.Remove(firstRec);
                    firstRec = DrawRectangle(pLastRec, false,false, OptionDrawLine.StrokeThickness, OptionColor.ColorOpacity, canvas);
                    lastRec = DrawRectangle(pfirstRec, false,false, OptionDrawLine.StrokeThicknessMainRec, OptionColor.ColorOpacity, canvas);
                }
                else
                {
                    firstRec.Opacity = 0;
                    firstRec = clickedRec;
                    firstRec.Opacity = 1;
                    firstRec.StrokeThickness = OptionDrawLine.StrokeThickness;
                }
            }
            else
            {
                if (Canvas.GetLeft(clickedRec) == Canvas.GetLeft(firstRec) && Canvas.GetTop(clickedRec) == Canvas.GetTop(firstRec))
                {
                    Point pfirstRec = new Point(Canvas.GetLeft(firstRec) + firstRec.Width / 2, Canvas.GetTop(firstRec) + firstRec.Height / 2);
                    Point pLastRec = new Point(Canvas.GetLeft(lastRec) + lastRec.Width / 2, Canvas.GetTop(lastRec) + lastRec.Height / 2);
                    canvas.Children.Remove(lastRec);
                    canvas.Children.Remove(firstRec);
                    firstRec = DrawRectangle(pLastRec, false,false, OptionDrawLine.StrokeThickness, OptionColor.ColorOpacity, canvas);
                    lastRec = DrawRectangle(pfirstRec, false,false, OptionDrawLine.StrokeThicknessMainRec, OptionColor.ColorOpacity, canvas);
                }
                else
                {
                    lastRec.Opacity = 0;
                    lastRec = clickedRec;
                    lastRec.Opacity = 1;
                    lastRec.StrokeThickness = OptionDrawLine.StrokeThicknessMainRec;
                }
            }
        }

        public bool ChooseFigureByClicking(Point clickedP, List<Figure> FigureList, Object clickedShape, Canvas canvas)
        {
            if(clickedShape is Rectangle)
            {
                Rectangle rect = (Rectangle)clickedShape;
                double x = Canvas.GetLeft(rect) + rect.ActualHeight / 2;
                double y = Canvas.GetTop(rect) + rect.ActualWidth / 2;
                if (new Point(x, y) == ListFigure[IndexFigure].PointStart)
                {
                    if (OptionRegim.regim == Regim.RegimDraw)
                    {
                        ReverseFigure(ListFigure[IndexFigure], canvas);
                        RedrawEverything(FigureList, IndexFigure, false, canvas);
                        DrawFirstAndLastRectangle();
                    }
                    else if (OptionRegim.regim == Regim.RegimCursor)
                    {
                        WindowColors window = new WindowColors();
                        window.ShowDialog();
                    }
                }
            }
            else if (clickedShape is Line || clickedShape is Path)
            {
                double x = 0;
                double y = 0;
                if (clickedShape is Line)
                {
                    Line clickedLine = (Line)clickedShape;
                    x = clickedLine.X1;
                    y = clickedLine.Y1;
                }
                else if (clickedShape is Path)
                {
                    Path path = (Path)clickedShape;
                    for(int i = 0; i < FigureList.Count;i++)
                    {
                        if (path.StrokeThickness == 10)
                        {
                            var invLine = FigureList[i].DictionaryInvLines.FirstOrDefault(z => z.Value == path);
                            var point = FigureList[i].DictionaryPointLines.FirstOrDefault(z => z.Value == invLine.Key);      //TODO: улучшить
                            if (point.Value != null)
                            {
                                x = point.Key.X;
                                y = point.Key.Y;
                                break;
                            }
                        }
                        else
                        {
                            var point = FigureList[i].DictionaryPointLines.FirstOrDefault(z => z.Value == path);      //TODO: улучшить
                            x = point.Key.X;
                            y = point.Key.Y;
                            if (point.Value != null)
                            {
                                x = point.Key.X;
                                y = point.Key.Y;
                                break;
                            }
                        }
                    }
                }
                for (int i = 0; i < FigureList.Count; i++)
                {
                    if (OptionRegim.regim != Regim.RegimEditFigures)
                    {
                        if (FigureList[i].DictionaryPointLines.ContainsKey(new Point(x, y)) == true)
                        {
                            if (IndexFigure == i)
                            {
                                if (OptionRegim.regim != Regim.RegimFigure && OptionRegim.regim != Regim.RegimCursor)
                                {
                                    FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection, false);
                                    IndexFigure = FigureList.Count - 1;
                                    RedrawEverything(FigureList, IndexFigure, false, canvas);
                                }
                                return true;
                            }
                            else
                            {                                
                                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection,false);
                                IndexFigure = i;                                
                                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
                                RedrawEverything(FigureList, IndexFigure, false, canvas);
                                DrawFirstAndLastRectangle();
                                if(OptionRegim.regim == Regim.RegimFigure)
                                    ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].Points,OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                                return false;
                            }       
                        }
                    }
                    else
                    {
                        if (FigureList[i].DictionaryPointLines.ContainsKey(new Point(x, y)) == true)
                        {
                            if (IndexFigure != i)
                            {
                                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection, false);
                                IndexFigure = i;
                                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, true);
                                FigureList[IndexFigure].PointsCount.Clear();
                                RedrawEverything(FigureList, IndexFigure, true, canvas);
                                return true;
                            }
                            else
                            {
                                FigureList[IndexFigure].PointForAddingPoints = new Point(x, y);
                                canvas.Children.Remove(FigureList[IndexFigure].NewPointEllipse);
                                FigureList[IndexFigure].DrawEllipse(clickedP,OptionColor.ColorSelection, OptionDrawLine.SizeEllipseForPoints, canvas,false);
                                return false;
                            }
                        }
                    }
                }
                return false;
            }
            else if (OptionRegim.regim != Regim.RegimEditFigures && OptionRegim.regim != Regim.RegimFigure && OptionRegim.regim != Regim.RegimCursor)
            {
                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection,false);
                FigureList.Add(new Figure(MainCanvas));
                IndexFigure = FigureList.Count - 1;
                RedrawEverything(FigureList, IndexFigure, false, canvas);
            }
            return false;
            //else if (OptionRegim.regim == Regim.RegimSelectFigureToEdit) //переход в режим ресайз (пока работает в этом режиме поворот, так как пока не понятно как переходить в режим поворот)
            //{

            //    if (isResizeRegim)
            //    {
            //        OptionRegim.regim = Regim.ResizeFigure;
            //    }
            //    //isResizeRegim = false;
            //}
            //else if(OptionRegim.regim == Regim.ResizeFigure)
            //{
            //    if (isRotateRegim)
            //    {
            //        OptionRegim.regim = Regim.RotateFigure;
            //    }
            //}
        }

        public void ChangeFiguresColor(List<Figure> FigureList, Canvas canvas)
        {
            if (FirstGladFigure != -1)
            {
                FigureList[FirstGladFigure].ChangeFigureColor(OptionColor.ColorSelection, false);
            }
            if(SecondGladFigure != -1)
            {
                FigureList[SecondGladFigure].ChangeFigureColor(OptionColor.ColorSelection, false);
            }
            if(OptionRegim.regim == Regim.RegimGlad)
            {
                FigureList[FirstGladFigure].ChangeFigureColor(OptionColor.ColorDraw,false);
                FigureList[SecondGladFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
            }
            else if (OptionRegim.regim == Regim.RegimEditFigures)
            {
                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, true);
            }
            else
            {
                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
            }
        }

        public Figure Cepochka(Figure figure, double step, Canvas canvas)
        {
            Figure resultFigure = new Figure(canvas);
            for (int i = 0; i < figure.Points.Count - 1; i++)
            {
                resultFigure.AddPoint(figure.Points[i], OptionColor.ColorDraw, false, OptionDrawLine.SizeWidthAndHeightRectangle);
                double x;
                double y;
                x = figure.Points[i + 1].X - figure.Points[i].X;
                y = figure.Points[i + 1].Y - figure.Points[i].Y;
                double distance = step;
                Vector vect = new Vector(x, y);
                double length = vect.Length;
                while (length > distance)           //ставим на отрезках стежки до тех пор, пока не пройдемся по всему отрезку
                {
                    vect.Normalize();
                    vect *= distance;
                    resultFigure.AddPoint(new Point(figure.Points[i].X + vect.X, figure.Points[i].Y + vect.Y), OptionColor.ColorDraw, false, OptionDrawLine.SizeWidthAndHeightRectangle);
                    distance += step;
                }
            }
            return resultFigure;
        }

        public void ExitFromRisuiRegim()
        {
            if (OptionRegim.regim == Regim.RegimRisui)
            {
                ListFigure = TempListFigure.ToList<Figure>();
                IndexFigure = TempIndexFigure;
                LinesForGlad = TempLinesForGlad.ToList<Figure>();
                RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
                DrawFirstAndLastRectangle();
                LoadPreviousRegim(true);
                DrawInvisibleRectangles(MainCanvas);
                if(OptionRegim.regim == Regim.RegimGlad)
                {
                    ListFigure[FirstGladFigure].DrawDots(ListFigure[FirstGladFigure].tempPoints, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                    ListFigure[SecondGladFigure].DrawDots(ListFigure[SecondGladFigure].tempPoints, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                }
                else
                {
                    ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].tempPoints, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                }
                
            }
            else if(OptionRegim.regim == Regim.RegimDrawStegki)
            {
                OptionRegim.regim = Regim.RegimCursor;
                ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw,false);
                DrawFirstAndLastRectangle();
            }
            else if(OptionRegim.regim == Regim.RegimDrawInColor)
            {
                OptionRegim.regim = Regim.RegimCursor;
                ListFigure = TempListFigure.ToList<Figure>();
                MainCanvas.Background = OptionColor.ColorBackground;
                ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
                RedrawEverything(ListFigure, IndexFigure, false, MainCanvas);
                ListFigure[IndexFigure].DrawOutSideRectanglePoints();
                DrawFirstAndLastRectangle();
            }
        }

        public void LoadPreviousRegim(bool isRisui)
        {
            OptionRegim.regim = ListFigure[IndexFigure].regimFigure;
            ChangeFiguresColor(ListFigure, MainCanvas);
            if (OptionRegim.regim == Regim.RegimTatami)
            {
                if (!isRisui)
                {
                    ListFigure[IndexFigure].SaveCurrentShapes();
                    PrepareForTatami(ListFigure[IndexFigure], true);
                }
                ControlLine.AddFigure(MainCanvas);
            }
            if (OptionRegim.regim == Regim.RegimGlad)
            {
                if (!isRisui)
                {
                    ListFigure[IndexFigure].SaveCurrentShapes();
                    ListFigure[SecondGladFigure].SaveCurrentShapes();
                    PrepareForTatami(ListFigure[IndexFigure], true);
                    PrepareForTatami(ListFigure[SecondGladFigure], true);
                }
                foreach (Figure sh in LinesForGlad)
                    sh.AddFigure(MainCanvas);
            }
        }
    }
}
