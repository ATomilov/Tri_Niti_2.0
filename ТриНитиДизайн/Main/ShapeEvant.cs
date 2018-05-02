﻿using System;
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
            if (OptionRegim.regim == Regim.RegimCursor)
            {
                IndexFigure = ListFigure.IndexOf(ListFigure[IndexFigure].groupFigures[0]);
            }
            ExitFromRisuiRegim();
            Edit_Menu.IsEnabled = false;
            CloseAllTabs();
            OptionRegim.regim = Regim.RegimEditFigures;
            for (int i = 0; i < ListFigure.Count; i++)
            {
                if (ListFigure[i].PreparedForTatami)
                {
                    ListFigure[i].LoadCurrentShapes();
                }
            }
            ChangeFiguresColor(ListFigure, MainCanvas);
            MainCanvas.Cursor = ArrowCursor;
            RedrawEverything(ListFigure, IndexFigure, true, MainCanvas);
            ChoosingRectangle = new Figure(MainCanvas);
            if (tabControl1.Visibility == Visibility.Hidden)
                tabControl1.Visibility = Visibility.Visible;
            ShowPositionStatus(ListFigure[IndexFigure], false, false);
        }

        private void LomannaiButtonEvent(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.RegimEditFigures;
            MakeLomanaya(ListFigure[IndexFigure], MainCanvas);
            RedrawEverything(ListFigure, IndexFigure, true, MainCanvas);
            ListFigure[IndexFigure].ChangeRectangleColor();
            ShowPositionStatus(ListFigure[IndexFigure], false, false);
        }

        private void DygaButtonEvent(object sender, RoutedEventArgs e)
        {
            MakeLomanaya(ListFigure[IndexFigure], MainCanvas);
            DrawAllChosenLines(ListFigure[IndexFigure], OptionColor.ColorChoosingRec, MainCanvas);
            RedrawEverything(ListFigure, IndexFigure, true, MainCanvas);
            ListFigure[IndexFigure].ChangeRectangleColor();
            ShowPositionStatus(ListFigure[IndexFigure], false, false);
        }

        private void KrivaiaButtonEvent(object sender, RoutedEventArgs e)
        {
            MakeLomanaya(ListFigure[IndexFigure], MainCanvas);
            DrawAllChosenLines(ListFigure[IndexFigure], OptionColor.ColorKrivaya, MainCanvas);
            RedrawEverything(ListFigure, IndexFigure, true, MainCanvas);
            ListFigure[IndexFigure].ChangeRectangleColor();
            ShowPositionStatus(ListFigure[IndexFigure], false, false);
        }

        private void SgladitButtonEvent(object sender, RoutedEventArgs e)
        {
            MakeSpline(ListFigure[IndexFigure], OptionColor.ColorKrivaya, MainCanvas);
            RedrawEverything(ListFigure, IndexFigure, true, MainCanvas);
            ListFigure[IndexFigure].ChangeRectangleColor();
            ShowPositionStatus(ListFigure[IndexFigure], false, false);
        }

        private void SelectPointNextButtonEvent(object sender, RoutedEventArgs e)
        {
            ChooseNextRectangle(ListFigure[IndexFigure], false, MainCanvas);
            ListFigure[IndexFigure].ChangeRectangleColor();
        }

        private void SelectPointPrevButtonEvent(object sender, RoutedEventArgs e)
        {
            ChooseNextRectangle(ListFigure[IndexFigure], true, MainCanvas);
            ListFigure[IndexFigure].ChangeRectangleColor();
        }

        private void PointAddedButtonEvent(object sender, RoutedEventArgs e)
        {
            AddPointToFigure(ListFigure[IndexFigure], MainCanvas);
            RedrawEverything(ListFigure, IndexFigure, true, MainCanvas);
        }

        private void PointDeleteButtonEvent(object sender, RoutedEventArgs e)
        {
            DeletePointFromFigure(ListFigure[IndexFigure], MainCanvas);
            RedrawEverything(ListFigure, IndexFigure, true, MainCanvas);
            ShowPositionStatus(ListFigure[IndexFigure], false, false);
        }

        private void RazrivButtonEvent(object sender, RoutedEventArgs e)
        {
            SplitFigureInTwo(ListFigure[IndexFigure], MainCanvas);
            RedrawEverything(ListFigure, IndexFigure, true, MainCanvas);
            ShowPositionStatus(ListFigure[IndexFigure], false, false);
        }
    }
}
