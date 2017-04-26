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

        private void FigureMainButtonEvant(object sender, RoutedEventArgs e)
        {
            ListFigure[IndexFigure].PointsCount.Clear();
            RedrawEverything(ListFigure, IndexFigure, false, false, MainCanvas);
            OptionRegim.regim = OptionRegim.oldRegim;
            ChangeFiguresColor(ListFigure, MainCanvas);
            if (OptionRegim.regim == Regim.RegimTatami)
            {
                ControlLine.AddFigure(MainCanvas);
            }
            if (OptionRegim.regim == Regim.RegimGlad)
            {
                foreach (Figure sh in LinesForGlad)
                    sh.AddFigure(MainCanvas);
            }
            ChosenPts = new List<Point>();
            CloseAllTabs();
            MainCanvas.Cursor = SwordCursor;   
            if (tabControl2.Visibility == Visibility.Hidden)
                tabControl2.Visibility = Visibility.Visible;
        }

        private void ChepochkaButtonEvent(object sender, RoutedEventArgs e)
        {
            if (ListFigure[IndexFigure].Points.Count > 0)
            {
                bool accepted;
                accepted = ShowAcceptMessage(0);
                if (accepted)
                {
                    OptionRegim.regim = Regim.RegimCepochka;
                    OptionRegim.oldRegim = Regim.RegimCepochka;
                    var CepochkaSetting = new View.Cepochka();
                    CepochkaSetting.Owner = this;
                    CepochkaSetting.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                    CepochkaSetting.ShowDialog();
                    if (!ListFigure[IndexFigure].PreparedForTatami)
                    {
                        PrepareForTatami(ListFigure[IndexFigure], MainCanvas);
                    }
                }
            }
        }

        private void GladButtonEvent(object sender, RoutedEventArgs e)
        {
            if (LinesForGlad.Count > 0)
            {
                var GladSetting = new View.Glad();
                GladSetting.Owner = this;
                GladSetting.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                GladSetting.ShowDialog();
            }
        }

        private void TatamiButtonEvent(object sender, RoutedEventArgs e)
        {
            if (ListFigure[IndexFigure].Points.Count > 0)
            {
                bool accepted;
                accepted = ShowAcceptMessage(1);
                if (accepted)
                {
                    OptionRegim.regim = Regim.RegimTatami;
                    OptionRegim.oldRegim = Regim.RegimTatami;
                    Tatami TatamiWindow = new Tatami();
                    TatamiWindow.ShowDialog();

                    if (!ListFigure[IndexFigure].PreparedForTatami)
                    {
                        PrepareForTatami(ListFigure[IndexFigure], MainCanvas);
                        ListFigure[IndexFigure].AddPoint(ListFigure[IndexFigure].Points[0], OptionColor.ColorDraw, false, 8);
                    }
                    InsertFirstControlLine(ListFigure[IndexFigure], ControlLine, MainCanvas);
                }
            }
        }
        private void StagkiButtonEvent(object sender, RoutedEventArgs e)
        {
            if (OptionRegim.regim != Regim.RegimFigure)
            {
                bool accepted;
                accepted = ShowAcceptMessage(2);
                if (accepted)
                {
                    if (OptionRegim.regim == Regim.RegimTatami)
                    {
                        Line lineCon = (Line)ControlLine.Shapes[0];
                        Point p1 = new Point(lineCon.X1, lineCon.Y1);
                        Point p2 = new Point(lineCon.X2, lineCon.Y2);
                        CalculateParallelLines(p1, p2, ListFigure[IndexFigure], ControlFigures, TatamiFigures, MainCanvas);
                        TatamiFigures[0].ChangeFigureColor(OptionColor.ColorDraw, false);
                        for (int i = 0; i < TatamiFigures.Count; i++)
                        {
                            ListFigure.Insert(IndexFigure, TatamiFigures[i]);
                        }
                        ListFigure.RemoveAt(IndexFigure + TatamiFigures.Count);
                        IndexFigure = ListFigure.IndexOf(TatamiFigures[0]);
                        RedrawEverything(ListFigure, IndexFigure, false, false, MainCanvas);
                        TatamiFigures.Clear();
                        OptionRegim.regim = Regim.RegimFigure;
                        OptionRegim.oldRegim = Regim.RegimFigure;
                    }
                    if (OptionRegim.regim == Regim.RegimGlad)
                    {
                        CalculateGladLines(ListFigure[IndexFigure], ListFigure[SecondGladFigure], LinesForGlad, ControlFigures, MainCanvas);
                        Figure firstFigure = ListFigure[IndexFigure];
                        Figure secondFigure = ListFigure[SecondGladFigure];
                        SecondGladFigure = -1; 
                        LinesForGlad[0].ChangeFigureColor(OptionColor.ColorDraw, false);
                        for (int i = 0; i < LinesForGlad.Count; i++)
                        {
                            ListFigure.Insert(IndexFigure, LinesForGlad[i]);
                        }
                        ListFigure.Remove(firstFigure);
                        ListFigure.Remove(secondFigure);
                        IndexFigure = ListFigure.IndexOf(LinesForGlad[0]);
                        RedrawEverything(ListFigure, IndexFigure, false, false, MainCanvas);
                        LinesForGlad.Clear();
                        OptionRegim.regim = Regim.RegimFigure;
                        OptionRegim.oldRegim = Regim.RegimFigure;
                    }
                    if (OptionRegim.regim == Regim.RegimCepochka)
                    {
                        ListFigure[IndexFigure] = Cepochka(ListFigure[IndexFigure], OptionCepochka.LenthStep, MainCanvas);
                        OptionRegim.regim = Regim.RegimFigure;
                        OptionRegim.oldRegim = Regim.RegimFigure;
                        for(int i = 0; i < ListFigure[IndexFigure].Points.Count; i++)
                        {
                            DrawRectangle(ListFigure[IndexFigure].Points[i], Brushes.White, MainCanvas);
                        }
                    }
                }
            }
        }

        private void RisuiButtonEvent(object sender, RoutedEventArgs e)
        {
            if (OptionRegim.regim == Regim.RegimTatami)
            {
                if (ControlLine != null)
                {
                    if (ControlLine.Points.Count > 2)
                    {
                        CalculateParallelLines(ControlLine.Points[2], ControlLine.Points[ControlLine.Points.Count - 1], ListFigure[IndexFigure], ControlFigures, TatamiFigures, MainCanvas);
                        TatamiFigures[0].ChangeFigureColor(OptionColor.ColorDraw, false);
                        for (int i = 0; i < TatamiFigures.Count; i++)
                        {
                            ListFigure.Insert(IndexFigure, TatamiFigures[i]);
                        }
                        ListFigure.RemoveAt(IndexFigure + TatamiFigures.Count);
                        IndexFigure = ListFigure.IndexOf(TatamiFigures[0]);
                        RedrawEverything(ListFigure, IndexFigure, false, false, MainCanvas);
                        TatamiFigures.Clear();
                        OptionRegim.regim = Regim.RegimFigure;
                    }
                }
            }
            if (OptionRegim.regim == Regim.RegimGlad)
            {
                CalculateGladLines(ListFigure[IndexFigure], ListFigure[SecondGladFigure], LinesForGlad, ControlFigures, MainCanvas);
                Figure firstFigure = ListFigure[IndexFigure];
                Figure secondFigure = ListFigure[SecondGladFigure];
                LinesForGlad[0].ChangeFigureColor(OptionColor.ColorDraw, false);
                for (int i = 0; i < LinesForGlad.Count; i++)
                {
                    ListFigure.Insert(IndexFigure, LinesForGlad[i]);
                }
                ListFigure.Remove(firstFigure);
                ListFigure.Remove(secondFigure);
                IndexFigure = ListFigure.IndexOf(LinesForGlad[0]);
                RedrawEverything(ListFigure, IndexFigure, false, false, MainCanvas);
                LinesForGlad.Clear();
                OptionRegim.regim = Regim.RegimFigure;
            }
            if (OptionRegim.regim == Regim.RegimCepochka)
            {
                ListFigure[IndexFigure] = Cepochka(ListFigure[IndexFigure], OptionCepochka.LenthStep, MainCanvas);
                OptionRegim.regim = Regim.RegimFigure;
            }
        }
    }
}
