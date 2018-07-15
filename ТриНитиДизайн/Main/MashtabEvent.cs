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
        private void MashtabMainButtonEvent(object sender, RoutedEventArgs e)
        {
            //ExitFromRisuimode();
            //if (expander1.Visibility == Visibility.Collapsed)
            //    expander1.Visibility = Visibility.Visible;
            //else
            //    expander1.Visibility = Visibility.Collapsed;
            //expander1.IsExpanded = !expander1.IsExpanded;
        }

        private void PlusButtonEvent(object sender, RoutedEventArgs e)
        {
            expander1.IsExpanded = false;
            expander1.Visibility = Visibility.Collapsed;
            if (OptionMode.mode != Mode.zoomIn && OptionMode.mode != Mode.zoomOut && OptionMode.mode != Mode.moveCanvas &&
                OptionMode.mode != Mode.oneToOne)
            {
                prevMode = OptionMode.mode;
                prevCursor = mainCanvas.Cursor;
            }
            OptionMode.mode = Mode.zoomIn;
            mainCanvas.Cursor = zoomInCursor;
        }

        private void MinusButtonEvent(object sender, RoutedEventArgs e)
        {
            expander1.IsExpanded = false;
            expander1.Visibility = Visibility.Collapsed;
            if (OptionMode.mode != Mode.zoomIn && OptionMode.mode != Mode.zoomOut && OptionMode.mode != Mode.moveCanvas &&
                OptionMode.mode != Mode.oneToOne)
            {
                prevMode = OptionMode.mode;
                prevCursor = mainCanvas.Cursor;
            }
            OptionMode.mode = Mode.zoomOut;
            mainCanvas.Cursor = zoomOutCursor;
        }

        private void PrevVidButtonEvent(object sender, RoutedEventArgs e)
        {
            //expander1.IsExpanded = false;
            //expander1.Visibility = Visibility.Collapsed;
            //if(previousViewList.Count !=0)
            //    LoadLastView();
            //SetGrid();
        }

        private void MashtabFigureButtonEvent(object sender, RoutedEventArgs e)
        {
            //expander1.IsExpanded = false;
            //expander1.Visibility = Visibility.Collapsed;
            //SaveLastView();
            ////TODO: add scale to one dot figure
            //if(listFigure[indexFigure].points.Count > 1)
            //    ScaleToFigure(listFigure[indexFigure]);
            //SetGrid();
        }

        private void MashtabVidButtonEvent(object sender, RoutedEventArgs e)
        {
            //expander1.IsExpanded = false;
            //expander1.Visibility = Visibility.Collapsed;
            //SaveLastView();
            //ResetScale();
            //SetGrid();
        }

        private void SetCenterButtonEvent(object sender, RoutedEventArgs e)
        {
            expander1.IsExpanded = false;
            expander1.Visibility = Visibility.Collapsed;
            if (OptionMode.mode != Mode.zoomIn && OptionMode.mode != Mode.zoomOut && OptionMode.mode != Mode.moveCanvas &&
                OptionMode.mode != Mode.oneToOne)
            {
                prevMode = OptionMode.mode;
                prevCursor = mainCanvas.Cursor;
            }
            mainCanvas.Cursor = centerCursor;   
            OptionMode.mode = Mode.moveCanvas;
        }

        private void OneToOneButtonEvent(object sender, RoutedEventArgs e)
        {
            expander1.IsExpanded = false;
            expander1.Visibility = Visibility.Collapsed;
            if (OptionMode.mode != Mode.zoomIn && OptionMode.mode != Mode.zoomOut && OptionMode.mode != Mode.moveCanvas &&
                OptionMode.mode != Mode.oneToOne)
            {
                prevMode = OptionMode.mode;
                prevCursor = mainCanvas.Cursor;
            }
            OptionMode.mode = Mode.oneToOne;
            mainCanvas.Cursor = oneToOneCursor;
        }
    }
}
