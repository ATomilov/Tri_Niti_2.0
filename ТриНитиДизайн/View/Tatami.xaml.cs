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

namespace ТриНитиДизайн
{
    /// <summary>
    /// Логика взаимодействия для Tatami.xaml
    /// </summary>
    public partial class Tatami : Window
    {
        public Tatami()
        {
            InitializeComponent();
            textbox1.Text = OptionTatami.StepLine.ToString();
            textbox2.Text = OptionTatami.StepStegok.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OptionTatami.StepLine = int.Parse(textbox1.Text);
            OptionTatami.StepStegok = int.Parse(textbox2.Text);
            this.Close();
        }
    }
}