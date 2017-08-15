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
        public void CloseAllTabs()
        {
            expander1.IsExpanded = false;
            if (tabControl1.Visibility == Visibility.Visible)
                tabControl1.Visibility = Visibility.Hidden;
            if (tabControl2.Visibility == Visibility.Visible)
                tabControl2.Visibility = Visibility.Hidden;
        }

        public void SetToDefault()
        {
            OptionSetka.MasshtabSetka = 0;
            OptionSetka.isDrawSetka = false;
            OptionSetka.isDotOnGrid = false;
            OptionDrawLine.StrokeThickness = 1;
            OptionDrawLine.SizeWidthAndHeightRectangle = 8;
            OptionDrawLine.InvisibleStrokeThickness = 10;
            OptionDrawLine.SizeRectangleForTransform = 10;
            OptionSetka.Masshtab = 1;
            OptionSetka.Angle = 0;
            ScaleTransform scaleTransform = new ScaleTransform(OptionSetka.Masshtab, OptionSetka.Masshtab);
            MainCanvas.RenderTransform = scaleTransform;

        }

        public string SavingFigures()
        {
            string dots = "";
            for (int i = 0; i < ListFigure.Count; i++)
            {
                if (ListFigure[i].Points.Count > 0)
                {
                    dots += ListFigure[i].Points[0].X;
                    dots += " ";
                    dots += ListFigure[i].Points[0].Y;
                    dots += " ";
                    for (int j = 1; j < ListFigure[i].Points.Count; j++)
                    {
                        Shape sh;
                        ListFigure[i].DictionaryPointLines.TryGetValue(ListFigure[i].Points[j - 1], out sh);
                        if (sh is Path)
                        {
                            Tuple<Point,Point> contP;
                            ListFigure[i].DictionaryShapeControlPoints.TryGetValue(ListFigure[i].Points[j - 1], out contP);
                            if (sh.MinHeight == 5)
                            {
                                dots += "C";
                                dots += " ";
                                dots += contP.Item1.X;
                                dots += " ";
                                dots += contP.Item1.Y;
                                dots += " ";
                                dots += contP.Item2.X;
                                dots += " ";
                                dots += contP.Item2.Y;
                            }
                            else
                            {
                                dots += "A";
                                dots += " ";
                                dots += contP.Item1.X;
                                dots += " ";
                                dots += contP.Item1.Y;
                            }
                        }
                        else
                            dots += "L";
                        dots += " ";
                        dots += ListFigure[i].Points[j].X;
                        dots += " ";
                        dots += ListFigure[i].Points[j].Y;
                        dots += " ";
                    }
                    dots += "!";
                }
            }
            return dots;
        }

        private void ClearEverything(bool isListEmpty)
        {
            ListFigure.Clear();
            if(!isListEmpty)
                ListFigure.Add(new Figure(MainCanvas));
            MainCanvas.Children.Clear();
            ListPltFigure.Clear();
            OptionRegim.regim = Regim.RegimNull;
            OptionColor.ColorNewBackground = Brushes.LightSkyBlue;
            OptionColor.ColorNewDraw = Brushes.Violet;
            TatamiFigures.Clear();
            IndexFigure = 0;
            CopyFigure = new Figure(MainCanvas);
            FirstGladFigure = -1;
            SecondGladFigure = -1;
            CloseAllTabs();
            SetToDefault();
            MainCanvas.Cursor = NormalCursor;
            SetkaFigure.AddFigure(MainCanvas);
        }

        public void CursorMenuDrawInColor()
        {
            TempListFigure = ListFigure.ToList<Figure>();
            MainCanvas.Children.Clear();
            ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorNewDraw, false);
            MainCanvas.Background = OptionColor.ColorNewBackground;
            ListFigure[IndexFigure].AddFigure(MainCanvas);
        }

        public void CursorMenuDrawStegki()
        {
            ListFigure[IndexFigure].ChangeFigureColor(OptionColor.ColorKrivaya, false);
            MainCanvas.Children.Remove(lastRec);
            MainCanvas.Children.Remove(firstRec);
        }

        public bool ShowAcceptMessage(int choice)
        {
            string sMessageBoxText = "Соединить?";
            string sCaption = "Стежки";
            if(choice == 0)
            {
                sMessageBoxText = "Создать цепочку стежков?";
            }
            if(choice == 1)
            {
                sMessageBoxText = "Создать татами?";
            }
            if (choice == 2)
            {
                sMessageBoxText = "Создать стежки?";
                if(OptionRegim.regim == Regim.RegimTatami)
                {
                    sCaption = "Татами";
                }
                if (OptionRegim.regim == Regim.RegimCepochka)
                {
                    sCaption = "Цепочка стежков";
                }
                if (OptionRegim.regim == Regim.RegimGlad)
                {
                    sCaption = "Гладь";
                }

            }
            MessageBoxButton btnMessageBox = MessageBoxButton.OKCancel;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.OK:
                    return true;

                case MessageBoxResult.Cancel:
                    return false;
            }
            return false;
        }
    }
}
