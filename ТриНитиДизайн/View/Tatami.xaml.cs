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
            textbox1.Text = OptionTatami.stepBetweenLines.ToString();
            textbox2.Text = OptionTatami.stitchLength.ToString();
            textbox3.Text = OptionTatami.Smeshcheniye.ToString();
            textbox1.SelectAll();
            textbox1.Focus();
            button1.IsDefault = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OptionTatami.stepBetweenLines = int.Parse(textbox1.Text);
            if ((int.Parse(textbox1.Text) < OptionTatami.MinStepLine) || (int.Parse(textbox1.Text) > OptionTatami.MaxStepLine))
            {
                System.Windows.MessageBox.Show("Длина шага должна быть от " + OptionTatami.MinStepLine.ToString() + " до " + OptionTatami.MaxStepLine.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            OptionTatami.stitchLength = int.Parse(textbox2.Text);
            if ((int.Parse(textbox2.Text) < OptionTatami.MinStepStegok) || (int.Parse(textbox2.Text) > OptionTatami.MaxStepStegok))
            {
                System.Windows.MessageBox.Show("Длина стежка должна быть от " + OptionTatami.MinStepStegok.ToString() + " до " + OptionTatami.MaxStepStegok.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            OptionTatami.Smeshcheniye = int.Parse(textbox3.Text);
            if ((int.Parse(textbox3.Text) < OptionTatami.MinSmeshcheniye) || (int.Parse(textbox3.Text) > OptionTatami.MaxSmeshcheniye))
            {
                System.Windows.MessageBox.Show("Смещение должно быть от " + OptionTatami.MinSmeshcheniye.ToString() + " до " + OptionTatami.MaxSmeshcheniye.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
        }
    }
}