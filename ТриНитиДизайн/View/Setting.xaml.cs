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
    /// Логика взаимодействия для Setting.xaml
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            InitializeComponent();
            checkBox1.IsChecked = OptionStechki.isZacrepki;
            textBox1.Text = OptionStechki.MaxLenthStechki.ToString();
            textBox2.Text = OptionStechki.MinLenthStechki.ToString();
            textBox3.Text = OptionStechki.LenthPerehodStechki.ToString();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            OptionStechki.isZacrepki = checkBox1.IsChecked ?? false;
            OptionStechki.MaxLenthStechki = int.Parse(textBox1.Text);
            OptionStechki.MinLenthStechki = int.Parse( textBox2.Text);
            OptionStechki.LenthPerehodStechki = int.Parse(textBox3.Text);
            this.Close();
        }
    }
}
