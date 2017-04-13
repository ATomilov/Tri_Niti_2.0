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
    /// Логика взаимодействия для Cepochka.xaml
    /// </summary>
    public partial class Cepochka : Window
    {
        public Cepochka()
        {
            InitializeComponent();
            checkbox1.IsChecked = OptionCepochka.ProkolyVTochkah;
            textbox1.Text = OptionCepochka.LenthStep.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OptionCepochka.ProkolyVTochkah = checkbox1.IsChecked ?? false;
            OptionCepochka.LenthStep = int.Parse(textbox1.Text);
            if ((int.Parse(textbox1.Text) < OptionCepochka.MinLenthStep) || (int.Parse(textbox1.Text) > OptionCepochka.MaxLenthStep))
            {
                System.Windows.MessageBox.Show("Длина шага должна быть от " + OptionCepochka.MinLenthStep.ToString() + " до " + OptionCepochka.MaxLenthStep.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();

        }
    }
}
