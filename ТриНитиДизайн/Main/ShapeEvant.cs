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

        private void ShapeMainButtonEvant(object sender, RoutedEventArgs e)
        {
            CloseAllTabs();
            OptionRegim.regim = Regim.RegimEditFigures;
             if (ListFigure[IndexFigure].PreparedForTatami)
            {
                ListFigure[IndexFigure].LoadCurrentShapes();
            }
            ChangeFiguresColor(ListFigure, MainCanvas);
            MainCanvas.Cursor = ArrowCursor;
            RedrawEverything(ListFigure, IndexFigure, false, true, MainCanvas);
            ChoosingRectangle = new Figure(MainCanvas);
            if (tabControl1.Visibility == Visibility.Hidden)
                tabControl1.Visibility = Visibility.Visible;
        }

        private void LomannaiButtonEvent(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.RegimEditFigures;
            MakeLomanaya(ListFigure[IndexFigure], MainCanvas);
            RedrawEverything(ListFigure, IndexFigure, false, true, MainCanvas);
        }

        private void DygaButtonEvent(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.RegimDuga;
            DrawAllChosenLines(ListFigure[IndexFigure], OptionColor.ColorChoosingRec, MainCanvas);
        }

        private void KrivaiaButtonEvent(object sender, RoutedEventArgs e)
        {
            DrawAllChosenLines(ListFigure[IndexFigure], OptionColor.ColorKrivaya, MainCanvas);
        }

        private void SgladitButtonEvent(object sender, RoutedEventArgs e)
        {
            MakeSpline(ListFigure[IndexFigure], MainCanvas);
            RedrawEverything(ListFigure, IndexFigure, false, true, MainCanvas);
        }

        private void SelectPointNextButtonEvent(object sender, RoutedEventArgs e)
        {
            ChooseNextRectangle(ListFigure[IndexFigure], false, MainCanvas);
            ListFigure[IndexFigure].DrawAllRectangles(OptionDrawLine.SizeWidthAndHeightRectangle, OptionColor.ColorOpacity);
        }
        private void SelectPointPrevButtonEvent(object sender, RoutedEventArgs e)
        {
            ChooseNextRectangle(ListFigure[IndexFigure], true, MainCanvas);
            ListFigure[IndexFigure].DrawAllRectangles(OptionDrawLine.SizeWidthAndHeightRectangle, OptionColor.ColorOpacity);
        }

        private void PointAddedButtonEvent(object sender, RoutedEventArgs e)
        {


        }
        private void PointDeleteButtonEvent(object sender, RoutedEventArgs e)
        {


        }

        private void RazrivButtonEvent(object sender, RoutedEventArgs e)
        {


        }
    }
    }
