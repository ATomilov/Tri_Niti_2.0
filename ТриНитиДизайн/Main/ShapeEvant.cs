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
            if (tabControl1.Visibility == Visibility.Visible)
                tabControl1.Visibility = Visibility.Hidden;
            else if (tabControl1.Visibility == Visibility.Hidden)
                tabControl1.Visibility = Visibility.Visible;
        }


        private void LomannaiButtonEvent(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.RegimLomanaya;
            MainCanvas.Children.Clear();
            RedrawEverything(ListFigure, IndexFigure, -1, MainCanvas);
        }

        private void DygaButtonEvent(object sender, RoutedEventArgs e)
        {

        }

        private void KrivaiaButtonEvent(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.RegimKrivaya;
        }

        private void SgladitButtonEvent(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();
            SetSpline(1, ListFigure[IndexFigure].Points, MainCanvas);

        }

        private void SelectPointNextButtonEvent(object sender, RoutedEventArgs e)
        {


        }
        private void SelectPointPrevButtonEvent(object sender, RoutedEventArgs e)
        {


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
