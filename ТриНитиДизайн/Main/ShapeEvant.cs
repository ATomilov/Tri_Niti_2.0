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
            if (OptionMode.mode == Mode.modeCursor)
            {
                indexFigure = listFigure.IndexOf(listFigure[indexFigure].groupFigures[0]);
            }
            ExitFromRisuimode();
            Edit_Menu.IsEnabled = false;
            CloseAllTabs();
            OptionMode.mode = Mode.modeEditFigures;
            for (int i = 0; i < listFigure.Count; i++)
            {
                if (listFigure[i].PreparedForTatami)
                {
                    listFigure[i].LoadCurrentShapes();
                }
            }
            ChangeFiguresColor(listFigure, MainCanvas);
            MainCanvas.Cursor = arrowCursor;
            RedrawEverything(listFigure, indexFigure, true, MainCanvas);
            choosingRectangle = new Figure(MainCanvas);
            if (tabControl1.Visibility == Visibility.Hidden)
                tabControl1.Visibility = Visibility.Visible;
            ShowPositionStatus(listFigure[indexFigure], false, false);
        }

        private void LomannaiButtonEvent(object sender, RoutedEventArgs e)
        {
            OptionMode.mode = Mode.modeEditFigures;
            MakeLomanaya(listFigure[indexFigure], MainCanvas);
            RedrawEverything(listFigure, indexFigure, true, MainCanvas);
            listFigure[indexFigure].ChangeRectangleColor();
            ShowPositionStatus(listFigure[indexFigure], false, false);
        }

        private void DygaButtonEvent(object sender, RoutedEventArgs e)
        {
            MakeLomanaya(listFigure[indexFigure], MainCanvas);
            DrawAllChosenLines(listFigure[indexFigure], OptionColor.colorArc, MainCanvas);
            RedrawEverything(listFigure, indexFigure, true, MainCanvas);
            listFigure[indexFigure].ChangeRectangleColor();
            ShowPositionStatus(listFigure[indexFigure], false, false);
        }

        private void KrivaiaButtonEvent(object sender, RoutedEventArgs e)
        {
            MakeLomanaya(listFigure[indexFigure], MainCanvas);
            DrawAllChosenLines(listFigure[indexFigure], OptionColor.colorCurve, MainCanvas);
            RedrawEverything(listFigure, indexFigure, true, MainCanvas);
            listFigure[indexFigure].ChangeRectangleColor();
            ShowPositionStatus(listFigure[indexFigure], false, false);
        }

        private void SsatinitButtonEvent(object sender, RoutedEventArgs e)
        {
            MakeSpline(listFigure[indexFigure], OptionColor.colorCurve, MainCanvas);
            RedrawEverything(listFigure, indexFigure, true, MainCanvas);
            listFigure[indexFigure].ChangeRectangleColor();
            ShowPositionStatus(listFigure[indexFigure], false, false);
        }

        private void SelectPointNextButtonEvent(object sender, RoutedEventArgs e)
        {
            ChooseNextRectangle(listFigure[indexFigure], false, MainCanvas);
            listFigure[indexFigure].ChangeRectangleColor();
        }

        private void SelectPointPrevButtonEvent(object sender, RoutedEventArgs e)
        {
            ChooseNextRectangle(listFigure[indexFigure], true, MainCanvas);
            listFigure[indexFigure].ChangeRectangleColor();
        }

        private void PointAddedButtonEvent(object sender, RoutedEventArgs e)
        {
            AddPointToFigure(listFigure[indexFigure], MainCanvas);
            RedrawEverything(listFigure, indexFigure, true, MainCanvas);
        }

        private void PointDeleteButtonEvent(object sender, RoutedEventArgs e)
        {
            DeletePointFromFigure(listFigure[indexFigure], MainCanvas);
            RedrawEverything(listFigure, indexFigure, true, MainCanvas);
            ShowPositionStatus(listFigure[indexFigure], false, false);
        }

        private void RazrivButtonEvent(object sender, RoutedEventArgs e)
        {
            SplitFigureInTwo(listFigure[indexFigure], MainCanvas);
            RedrawEverything(listFigure, indexFigure, true, MainCanvas);
            ShowPositionStatus(listFigure[indexFigure], false, false);
        }
    }
}
