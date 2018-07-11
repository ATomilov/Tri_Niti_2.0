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
            checkBox1.IsChecked = OptionStitches.isZacrepki;
            textBox1.Text = OptionStitches.MaxLenthStechki.ToString();
            textBox2.Text = OptionStitches.MinLenthStechki.ToString();
            textBox3.Text = OptionStitches.CurrentPerehodStechki.ToString();
            textBox1.SelectAll();
            textBox1.Focus();
            button1.IsDefault = true;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            OptionStitches.isZacrepki = checkBox1.IsChecked ?? false;
            OptionStitches.MaxLenthStechki = int.Parse(textBox1.Text);
            OptionStitches.MinLenthStechki = int.Parse( textBox2.Text);
            OptionStitches.CurrentPerehodStechki = int.Parse(textBox3.Text);
            this.Close();
        }
    }
}
