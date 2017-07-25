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
        public void RedrawEverything(List<Figure> FigureList,int ChosenFigure, bool AllRectanglesOff,bool rectanglesOn, Canvas canvas)
        {
            canvas.Children.Clear();
            SetkaFigure.AddFigure(canvas);
            double size = OptionDrawLine.SizeWidthAndHeightRectangle;
            for(int i = 0; i < FigureList.Count;i++)
            {
                FigureList[i].AddFigure(canvas);                        //можно не перерисовывать каждый раз
                if (!AllRectanglesOff)
                {
                    if (i == ChosenFigure)
                    {
                        if (rectanglesOn)
                        {
                            FigureList[i].DrawAllRectangles(size, OptionColor.ColorOpacity);
                        }
                        else
                        {
                            if (FigureList[i].Points.Count > 0)
                            {
                                DrawRectangle(FigureList[i].Points[0], OptionColor.ColorOpacity, canvas);
                                DrawRectangle(FigureList[i].Points[FigureList[i].Points.Count - 1], OptionColor.ColorOpacity, canvas);
                            }
                        }
                    }
                }
                
            }
            for (int i = 0; i < ListPltFigure.Count; i++)
            {
                ListPltFigure[i].AddFigure(canvas);
            }
        }


        private void DrawRectangle(Point p, Brush brush, Canvas canvas)
        {
            Rectangle rec = new Rectangle();
            rec.Height = OptionDrawLine.SizeWidthAndHeightRectangle;
            rec.Width = OptionDrawLine.SizeWidthAndHeightRectangle;
            Canvas.SetLeft(rec, p.X - OptionDrawLine.SizeWidthAndHeightRectangle/2);
            Canvas.SetTop(rec, p.Y - OptionDrawLine.SizeWidthAndHeightRectangle/2);
            rec.Fill = brush;
            rec.Stroke = OptionColor.ColorSelection;
            rec.StrokeThickness = OptionDrawLine.StrokeThickness;
            canvas.Children.Add(rec);
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
                        RedrawEverything(ListFigure, IndexFigure, false, false, MainCanvas);
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

        public void ChooseFigureByClicking(Point clickedP, List<Figure> FigureList, Object clickedShape, Canvas canvas)
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
                    for(int i = 0; i < FigureList.Count;i++)
                    {
                       var invLine = FigureList[i].DictionaryInvLines.FirstOrDefault(z => z.Value == path);
                       var point = FigureList[i].DictionaryPointLines.FirstOrDefault(z => z.Value == invLine.Key);      //TODO: улучшить
                       if(point.Value != null)
                       {
                           x = point.Key.X;
                           y = point.Key.Y;
                           break;
                       }
                    }
                }
                for (int i = 0; i < FigureList.Count; i++)
                {
                    if (OptionRegim.regim != Regim.RegimEditFigures)
                    {
                        if (FigureList[i].DictionaryPointLines.ContainsKey(new Point(x, y)) == true)
                        {
                            if (IndexFigure == i && OptionRegim.regim != Regim.RegimFigure)
                            {
                                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection,false);
                                IndexFigure = FigureList.Count - 1;
                                RedrawEverything(FigureList, IndexFigure, true, false, canvas);
                            }
                            else
                            {                                
                                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection,false);
                                IndexFigure = i;                                
                                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw, false);
                                RedrawEverything(FigureList, IndexFigure, false, false, canvas);
                                if(OptionRegim.regim == Regim.RegimFigure)
                                    ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].Points,OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
                            }                            
                            break;
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
                                RedrawEverything(FigureList, IndexFigure, false, true, canvas);
                            }
                            else
                            {
                                FigureList[IndexFigure].PointForAddingPoints = new Point(x, y);
                                canvas.Children.Remove(FigureList[IndexFigure].NewPointEllipse);
                                FigureList[IndexFigure].DrawEllipse(clickedP,OptionColor.ColorSelection, OptionDrawLine.SizeEllipseForPoints, canvas,false);
                            }
                            break;
                        }
                    }
                }
            }
            else if (OptionRegim.regim != Regim.RegimEditFigures && OptionRegim.regim != Regim.RegimFigure && OptionRegim.regim != Regim.RegimSelectFigureToEdit)
            {
                FigureList[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection,false);
                FigureList.Add(new Figure(MainCanvas));
                IndexFigure = FigureList.Count - 1;
                RedrawEverything(FigureList, IndexFigure, false, false, canvas);
            }
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
                RedrawEverything(ListFigure, IndexFigure, false, false, MainCanvas);
                LoadPreviousRegim(true);
                ListFigure[IndexFigure].DrawDots(ListFigure[IndexFigure].tempPoints, OptionDrawLine.RisuiRegimDots, OptionColor.ColorSelection, MainCanvas);
            }
        }
    }
}
