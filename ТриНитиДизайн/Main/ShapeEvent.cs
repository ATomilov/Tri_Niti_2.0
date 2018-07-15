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
        private void ShapeMainButtonEvent(object sender, RoutedEventArgs e)
        {
            //if (OptionMode.mode == Mode.modeCursor)
            //{
            //    indexFigure = listFigure.IndexOf(listFigure[indexFigure].groupFigures[0]);
            //}
            //ExitFromRisuimode();
            Edit_Menu.IsEnabled = false;
            CloseAllTabs();
            OptionMode.mode = Mode.modeEditPoints;
            //for (int i = 0; i < listFigure.Count; i++)
            //{
            //    if (listFigure[i].preparedForTatami)
            //    {
            //        listFigure[i].LoadCurrentShapes();
            //    }
            //}
            //ChangeFiguresColor(listFigure, mainCanvas);
            mainCanvas.Cursor = arrowCursor;
            RedrawScreen(listFigure, indexFigure, mainCanvas);
            //choosingRectangle = new Figure(mainCanvas);
            //if (tabControl1.Visibility == Visibility.Hidden)
            //    tabControl1.Visibility = Visibility.Visible;
            //ShowPositionStatus(listFigure[indexFigure], false, false);
        }

        private void LomannaiButtonEvent(object sender, RoutedEventArgs e)
        {
            //OptionMode.mode = Mode.modeEditPoints;
            //MakeLomanaya(listFigure[indexFigure], mainCanvas);
            //RedrawEverything(listFigure, indexFigure, true, mainCanvas);
            //listFigure[indexFigure].ChangeRectangleColor();
            //ShowPositionStatus(listFigure[indexFigure], false, false);
        }

        private void DygaButtonEvent(object sender, RoutedEventArgs e)
        {
            //MakeLomanaya(listFigure[indexFigure], mainCanvas);
            //DrawAllChosenLines(listFigure[indexFigure], OptionColor.colorArc, mainCanvas);
            //RedrawEverything(listFigure, indexFigure, true, mainCanvas);
            //listFigure[indexFigure].ChangeRectangleColor();
            //ShowPositionStatus(listFigure[indexFigure], false, false);
        }

        private void KrivaiaButtonEvent(object sender, RoutedEventArgs e)
        {
            //MakeLomanaya(listFigure[indexFigure], mainCanvas);
            //DrawAllChosenLines(listFigure[indexFigure], OptionColor.colorCurve, mainCanvas);
            //RedrawEverything(listFigure, indexFigure, true, mainCanvas);
            //listFigure[indexFigure].ChangeRectangleColor();
            //ShowPositionStatus(listFigure[indexFigure], false, false);
        }

        private void SsatinitButtonEvent(object sender, RoutedEventArgs e)
        {
            //MakeSpline(listFigure[indexFigure], OptionColor.colorCurve, mainCanvas);
            //RedrawEverything(listFigure, indexFigure, true, mainCanvas);
            //listFigure[indexFigure].ChangeRectangleColor();
            //ShowPositionStatus(listFigure[indexFigure], false, false);
        }

        private void SelectPointNextButtonEvent(object sender, RoutedEventArgs e)
        {
            //ChooseNextRectangle(listFigure[indexFigure], false, mainCanvas);
            //listFigure[indexFigure].ChangeRectangleColor();
        }

        private void SelectPointPrevButtonEvent(object sender, RoutedEventArgs e)
        {
            //ChooseNextRectangle(listFigure[indexFigure], true, mainCanvas);
            //listFigure[indexFigure].ChangeRectangleColor();
        }

        private void PointAddedButtonEvent(object sender, RoutedEventArgs e)
        {
            //AddPointToFigure(listFigure[indexFigure], mainCanvas);
            //RedrawEverything(listFigure, indexFigure, true, mainCanvas);
        }

        private void PointDeleteButtonEvent(object sender, RoutedEventArgs e)
        {
            //DeletePointFromFigure(listFigure[indexFigure], mainCanvas);
            //RedrawEverything(listFigure, indexFigure, true, mainCanvas);
            //ShowPositionStatus(listFigure[indexFigure], false, false);
        }

        private void RazrivButtonEvent(object sender, RoutedEventArgs e)
        {
            //SplitFigureInTwo(listFigure[indexFigure], mainCanvas);
            //RedrawEverything(listFigure, indexFigure, true, mainCanvas);
            //ShowPositionStatus(listFigure[indexFigure], false, false);
        }
    }
}
