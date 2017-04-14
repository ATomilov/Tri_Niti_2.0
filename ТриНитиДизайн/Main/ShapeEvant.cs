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
            RedrawEverything(ListFigure, IndexFigure, -1, MainCanvas);
            ListFigure[IndexFigure].DrawAllRectangles(8,OptionColor.ColorOpacity);
            ChoosingRectangle = new Figure(MainCanvas);
            TempFigure = new Figure(MainCanvas);
            if (tabControl1.Visibility == Visibility.Hidden)
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
            ListFigure[IndexFigure].Points.Clear();
            Path path = (Path)MainCanvas.Children[MainCanvas.Children.Count - 1];
            PathGeometry myPathGeometry = (PathGeometry)path.Data;
            Point p;
            Point tg;
            var points = new List<Point>();
            double step = 50;
            for (var i = 0; i <= step; i++)
            {
                myPathGeometry.GetPointAtFractionLength(i / step, out p, out tg);
                ListFigure[IndexFigure].Points.Add(p);
            }
            RedrawEverything(ListFigure, IndexFigure, -1, MainCanvas);
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
