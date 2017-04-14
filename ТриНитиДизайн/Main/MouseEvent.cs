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

        private void CanvasTest_MouseRightButtonDown(object sender, MouseButtonEventArgs e)         //при нажатии на праую кнопку мыши
        {
            Mouse.Capture(MainCanvas);
            if (OptionRegim.regim == Regim.RegimTatami)
            {
                if(ControlLine.Points.Count > 2)
                {
                    MainCanvas.Children.RemoveAt(MainCanvas.Children.Count - 1);
                }
                ControlLine.Points.Clear();
                ControlLine.Points.Add(e.GetPosition(MainCanvas));
            }
        }

        private void CanvasTest_MouseMove(object sender, MouseEventArgs e)
        {
            MousePositionX = e.GetPosition(MainCanvas).X;
            MousePositionY = e.GetPosition(MainCanvas).Y;
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (OptionRegim.regim == Regim.RegimDraw)
                {
                    if (ListFigure[IndexFigure].Points.Count > 0)
                    {
                        MainCanvas.Children.RemoveAt(MainCanvas.Children.Count - 1);
                        Line line = ListFigure[IndexFigure].GetLine(ListFigure[IndexFigure].PointEnd, e.GetPosition(MainCanvas));
                        line.StrokeThickness = 1;
                        line.Stroke = OptionColor.ColorChoosingRec;
                        MainCanvas.Children.Add(line);
                    }
                }
                if (OptionRegim.regim == Regim.RegimTatami)
                {
                    if (ControlLine.Points.Count > 1)
                    {
                        MainCanvas.Children.RemoveAt(MainCanvas.Children.Count - 1);
                        ControlLine.Points.RemoveAt(ControlLine.Points.Count - 1);
                    }
                    Line line = ControlLine.GetLine(ControlLine.Points[0], e.GetPosition(MainCanvas));
                    DoubleCollection dashes = new DoubleCollection();
                    dashes.Add(2);
                    dashes.Add(2);
                    line.StrokeDashArray = dashes;
                    line.StrokeThickness = 1;
                    line.Stroke = OptionColor.ColorSelection;
                    MainCanvas.Children.Add(line);
                    MainCanvas.UpdateLayout();
                    ControlLine.Points.Add(e.GetPosition(MainCanvas));
                }
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if(OptionRegim.regim == Regim.RegimKrivaya)
                {
                    if (ChosenPts.Count > 0)
                    {
                        RedrawEverything(ListFigure, IndexFigure,  MainCanvas);
                        ListFigure[IndexFigure].DrawAllRectangles(8, OptionColor.ColorOpacity);
                        ChosenPts.Insert(1, e.GetPosition(MainCanvas));

                        SetSpline(0.75, ChosenPts, MainCanvas);
                        ChosenPts.RemoveAt(1);
                         
                    }
                }

                if (OptionRegim.regim == Regim.RegimEditFigures)
                {
                    if (ChoosingRectangle.Points.Count > 0)
                    {
                        RedrawEverything(ListFigure, IndexFigure, MainCanvas);
                        ListFigure[IndexFigure].DrawAllRectangles(8, OptionColor.ColorOpacity);
                        DrawChoosingRectangle(ChoosingRectangle.Points[0], e.GetPosition(MainCanvas),MainCanvas);
                    }
                }
            }
        }

        private void CanvasTest_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            if (OptionRegim.regim == Regim.RegimKrivaya)
            {
                if (ChosenPts.Count > 1)
                {
                    ListFigure[IndexFigure].AddShape((Shape)MainCanvas.Children[MainCanvas.Children.Count - 1], ChosenPts[0]);
                    ChosenPts.Clear();
                    OptionRegim.regim = Regim.RegimEditFigures;
                }
            }
            if (OptionRegim.regim == Regim.RegimEditFigures)
            {
                if (ChoosingRectangle.Points.Count > 0)
                {
                    RedrawEverything(ListFigure, IndexFigure, MainCanvas);
                    ListFigure[IndexFigure].DrawAllRectangles(8, OptionColor.ColorOpacity);
                    Point UpperLeftCorner = new Point();
                    Point LowerRightCorner = new Point();
                    if(e.GetPosition(MainCanvas).X < ChoosingRectangle.Points[0].X)
                    {
                        UpperLeftCorner.X = e.GetPosition(MainCanvas).X;
                        LowerRightCorner.X = ChoosingRectangle.Points[0].X;
                    }
                    else
                    {
                        UpperLeftCorner.X = ChoosingRectangle.Points[0].X;
                        LowerRightCorner.X = e.GetPosition(MainCanvas).X;
                    }
                    if (e.GetPosition(MainCanvas).Y < ChoosingRectangle.Points[0].Y)
                    {
                        UpperLeftCorner.Y = e.GetPosition(MainCanvas).Y;
                        LowerRightCorner.Y = ChoosingRectangle.Points[0].Y;
                    }
                    else
                    {
                        UpperLeftCorner.Y = ChoosingRectangle.Points[0].Y;
                        LowerRightCorner.Y = e.GetPosition(MainCanvas).Y;
                    }
                    for (int i = 0; i < ListFigure[IndexFigure].Points.Count;i++ )
                    {
                        Point newPoint = ListFigure[IndexFigure].Points[i];
                        if(newPoint.X < LowerRightCorner.X && newPoint.X > UpperLeftCorner.X &&
                            newPoint.Y < LowerRightCorner.Y && newPoint.Y > UpperLeftCorner.Y)
                        {
                            ListFigure[IndexFigure].PointsCount.Add(i);
                        }
                    }
                    ListFigure[IndexFigure].DrawAllRectangles(8, OptionColor.ColorOpacity);
                }
            }
        }

        private void CanvasTest_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            if (OptionRegim.regim == Regim.RegimDraw)
            {
                if (ListFigure[IndexFigure].Points.Count > 1)
                {
                    MainCanvas.Children.RemoveAt(MainCanvas.Children.Count - 1);
                    DrawRectangle(ListFigure[IndexFigure].PointStart,OptionColor.ColorOpacity,MainCanvas);
                }

                Point point = FindClosestDot(e.GetPosition(MainCanvas));
                ListFigure[IndexFigure].AddPoint(point, OptionColor.ColorDraw, true, 8);
            }

            if (OptionRegim.regim == Regim.RegimTatami)
            {
                if(ControlLine.Points.Count == 1)
                {
                    ControlLine.Points.Add(e.GetPosition(MainCanvas));
                }
                FindControlLine(ListFigure[IndexFigure], ControlLine, MainCanvas);
            }
        }

        void CanvasTest_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)          //левая кнопка мыши
        {
            Mouse.Capture(MainCanvas);
            if (OptionRegim.regim == Regim.RegimDraw)
            {
                if (e.OriginalSource is Line)                      //выделение части татами
                {
                    double x;
                    double y;
                    Line clickedLine = (Line)e.OriginalSource;
                    x = clickedLine.X1;
                    y = clickedLine.Y1;
                    for (int i = 0; i < ListFigure.Count; i++)
                    {
                        for (int j = 0; j < ListFigure[i].Points.Count; j++)
                        {
                            if (ListFigure[i].Points[j].X == x && ListFigure[i].Points[j].Y == y)
                            {
                                IndexFigure = i;
                                ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorDraw);
                                RedrawEverything(ListFigure, IndexFigure, MainCanvas);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorSelection);
                    ListFigure.Add(new Figure(MainCanvas));
                    IndexFigure = ListFigure.Count - 1;
                    RedrawEverything(ListFigure, IndexFigure, MainCanvas);
                }
            }
            if (OptionRegim.regim == Regim.RegimStegki)
            {
                if (e.OriginalSource is Line || e.OriginalSource is Path)                      //выделение части татами
                {
                    double x;
                    double y;
                    if (e.OriginalSource is Line)                                       //если мы нажали на линию, то находим одну из точек линии
                    {
                        Line clickedLine = (Line)e.OriginalSource;
                        x = clickedLine.X1;
                        y = clickedLine.Y1;
                    }
                    else
                    {
                        Path clickedPath = (Path)e.OriginalSource;                      //если мы нажали на точку, то находим координаты
                        EllipseGeometry geom = (EllipseGeometry)clickedPath.Data;
                        x = geom.Center.X;
                        y = geom.Center.Y;
                    }
                    for (int i = 0; i < 128; i++)                           //находим номер фигуры, которую хотим выделить
                    {
                        for (int j = 0; j < TatamiFigures[i].Points.Count; j++)
                        {
                            if (TatamiFigures[i].Points[j].X == x && TatamiFigures[i].Points[j].Y == y)
                            {
                                DrawTatami(TatamiFigures, i, MainCanvas);
                                break;
                            }
                        }
                    }
                }
            }

            if (OptionRegim.regim == Regim.RegimEditFigures)
            {
                ChosenPts.Clear();
                ListFigure[IndexFigure].PointsCount.Clear();
                ChoosingRectangle.Points.Clear();
                if (e.OriginalSource is Line)
                {
                    double x;
                    double y;
                    Line clickedLine = (Line)e.OriginalSource;
                    x = clickedLine.X1;
                    y = clickedLine.Y1;
                    Point newPoint = new Point(x, y);
                    Shape sh;
                    if (clickedLine.Stroke == OptionColor.ColorKrivaya)
                    {
                        OptionRegim.regim = Regim.RegimKrivaya;
                        ListFigure[IndexFigure].DictionaryPointLines.TryGetValue(new Point(x, y), out sh);
                        ListFigure[IndexFigure].DeleteShape(sh, new Point(x, y));
                        ChosenPts.Add(newPoint);
                        ChosenPts.Add(new Point(clickedLine.X2, clickedLine.Y2));
                    }
                }
                else
                {
                    ChoosingRectangle.Points.Add(e.GetPosition(MainCanvas));
                }
            }

        }
    }
}
