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

        private void MashtabMainButtonEvant(object sender, RoutedEventArgs e)
        {
            ExitFromRisuiRegim();
            if (expander1.Visibility == Visibility.Collapsed)
                expander1.Visibility = Visibility.Visible;
            else
                expander1.Visibility = Visibility.Collapsed;
            expander1.IsExpanded = !expander1.IsExpanded;
        }

        private void PlusButtonEvent(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.ZoomIn;
            MainCanvas.Cursor = ZoomInCursor;
        }
        private void MinusButtonEvent(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.ZoomOut;
            MainCanvas.Cursor = ZoomOutCursor;
        }

        private void PrevVidButtonEvent(object sender, RoutedEventArgs e)
        {

        }

        private void MashtabFigureButtonEvent(object sender, RoutedEventArgs e)
        {

        }

        private void MashtabVidButtonEvent(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.MoveCanvas;
        }

        private void SetCenterButtonEvent(object sender, RoutedEventArgs e)
        {

        }

        private void OneToOneButtonEvent(object sender, RoutedEventArgs e)
        {

        }
    }
}
