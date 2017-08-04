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
using System.Windows.Shapes;

namespace ТриНитиДизайн.View
{
    /// <summary>
    /// Логика взаимодействия для SpecialWindowWhenSelectedFigure.xaml
    /// </summary>
    public partial class SpecialWindowWhenSelectedFigure : Window
    {
        public SpecialWindowWhenSelectedFigure()
        {
            InitializeComponent();
        }

        private void Prorisovat_Stezhki(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.RegimDrawStegki;
            this.Close();
        }

        private void Prorisovat_v_tsvete(object sender, RoutedEventArgs e)
        {
            OptionRegim.regim = Regim.RegimDrawInColor;
            this.Close();
        }

        private void Sokhranit_v_fayl(object sender, RoutedEventArgs e)
        {

        }

        private void Otshit(object sender, RoutedEventArgs e)
        {

        }

        private void Otmenit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
