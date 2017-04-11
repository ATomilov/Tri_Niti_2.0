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
            if (tabControl2.Visibility == Visibility.Visible)
                tabControl2.Visibility = Visibility.Hidden;
            else if (tabControl2.Visibility == Visibility.Hidden)
                tabControl2.Visibility = Visibility.Visible;
        }

        private void ChepochkaButtonEvent(object sender, RoutedEventArgs e)
        {
            var CepochkaSetting = new View.Cepochka();
            CepochkaSetting.Show();
            if (ListFigure[IndexFigure].Points.Count > 0)
            {
                OptionRegim.regim = Regim.RegimCepochka;
                ListFigure[IndexFigure].AddPoint(ListFigure[IndexFigure].Points[0]);
                Cepochka(ListFigure[IndexFigure], OptionCepochka.LenthStep, MainCanvas);
            }
        }

        private void GladButtonEvent(object sender, RoutedEventArgs e)
        {
            var GladSetting = new View.Glad();
            GladSetting.Show();

        }
        private void TatamiButtonEvent(object sender, RoutedEventArgs e)
        {
            if (ListFigure[IndexFigure].Points.Count > 0)
            {
                Tatami TatamiWindow = new Tatami();
                TatamiWindow.ShowDialog();
                OptionRegim.regim = Regim.RegimTatami;
                ListFigure[IndexFigure].AddPoint(ListFigure[IndexFigure].Points[0]);
                ControlLine = new Figure(MainCanvas);
            }
        }
        private void StagkiButtonEvent(object sender, RoutedEventArgs e)
        {
            if (ControlLine != null)
            {
                if (ControlLine.Points.Count > 2)
                {
                    OptionRegim.regim = Regim.RegimStegki;
                    CalculateParallelLines(ControlLine.Points[2], ControlLine.Points[ControlLine.Points.Count - 1], ListFigure[IndexFigure], ControlFigures, TatamiFigures, MainCanvas);
                }
            }
            //Ctezhki(ListFigure[IndexFigure].Shapes, new Point(), new Point(), 1, 0,MainCanvas);
        }

        private void RisuiButtonEvent(object sender, RoutedEventArgs e)
        {


        }



    }
}
