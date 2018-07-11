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
            checkbox1.IsChecked = OptionRunStitch.ProkolyVTochkah;
            textbox1.Text = OptionRunStitch.lengthStep.ToString();
            textbox1.SelectAll();
            textbox1.Focus();
            button1.IsDefault = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OptionRunStitch.ProkolyVTochkah = checkbox1.IsChecked ?? false;
            OptionRunStitch.lengthStep = int.Parse(textbox1.Text);
            if ((int.Parse(textbox1.Text) < OptionRunStitch.minLengthStep) || (int.Parse(textbox1.Text) > OptionRunStitch.maxLengthStep))
            {
                System.Windows.MessageBox.Show("Длина шага должна быть от " + OptionRunStitch.minLengthStep.ToString() + " до " + OptionRunStitch.maxLengthStep.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();

        }

        private void textbox1_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
