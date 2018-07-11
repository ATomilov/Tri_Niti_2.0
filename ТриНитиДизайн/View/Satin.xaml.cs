using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ТриНитиДизайн.View
{
    /// <summary>
    /// Логика взаимодействия для Satin.xaml
    /// </summary>
    public partial class Satin : Window
    {
        public Satin()
        {
            InitializeComponent();
            checkbox1.IsChecked = OptionSatin.PoChisluProkolov;
            checkbox2.IsChecked = OptionSatin.PoLenthStezhka;
            textbox1.Text = OptionSatin.lengthStep.ToString();
            textbox2.Text = OptionSatin.Rasshirenie.ToString();
            textbox3.Text = OptionSatin.NumberOfStezhkov.ToString();
            textbox4.Text = OptionSatin.Otstup.ToString();
            textbox5.Text = OptionSatin.NumberOfProkolov.ToString();
            textbox6.Text = OptionSatin.StartLenthStezhok.ToString();
            textbox7.Text = OptionSatin.EndLenthStezhok.ToString();
            textbox1.SelectAll();
            textbox1.Focus();
            button1.IsDefault = true;

        }
        struct CorrectSatinSetting
        {
            public bool b_lengthStep;
            public bool b_Rasshirenie;
            public bool b_NumberOfStezhkov;
            public bool b_Otstup;
            public bool b_NumberOfProkolov;
            public bool b_StartLenthStezhok;
            public bool b_EndLenthStezhok;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            CorrectSatinSetting CGS;
            OptionSatin.PoChisluProkolov = checkbox1.IsChecked ?? false;
            OptionSatin.PoLenthStezhka = checkbox2.IsChecked ?? false;
            OptionSatin.lengthStep = int.Parse(textbox1.Text);
            if ((int.Parse(textbox1.Text) < OptionSatin.minLengthStep) || (int.Parse(textbox1.Text) > OptionSatin.maxLengthStep))
            {
                System.Windows.MessageBox.Show("Длина шага должна быть от " + OptionSatin.minLengthStep.ToString() + " до " + OptionSatin.maxLengthStep.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                CGS.b_lengthStep = false;
            }
            else CGS.b_lengthStep = true;
            OptionSatin.Rasshirenie = int.Parse(textbox2.Text);
            if ((int.Parse(textbox2.Text) < OptionSatin.MinRasshirenie) || (int.Parse(textbox2.Text) > OptionSatin.MaxRasshirenie))
            {
                System.Windows.MessageBox.Show("Расширение должно быть от " + OptionSatin.MinRasshirenie.ToString() + " до " + OptionSatin.MaxRasshirenie.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                CGS.b_Rasshirenie = false;
            }
            else CGS.b_Rasshirenie = true;
            OptionSatin.NumberOfStezhkov = int.Parse(textbox3.Text);
            if ((int.Parse(textbox3.Text) < OptionSatin.MinNumberOfStezhkov) || (int.Parse(textbox3.Text) > OptionSatin.MaxNumberOfStezhkov))
            {
                System.Windows.MessageBox.Show("Количество опорных стежков должно быть от " + OptionSatin.MinNumberOfStezhkov.ToString() + " до " + OptionSatin.MaxNumberOfStezhkov.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                CGS.b_NumberOfStezhkov = false;
            }
            else CGS.b_NumberOfStezhkov = true;
            OptionSatin.Otstup = int.Parse(textbox4.Text);
            if ((int.Parse(textbox4.Text) < OptionSatin.MinOtstup) || (int.Parse(textbox4.Text) > OptionSatin.MaxOtstup))
            {
                System.Windows.MessageBox.Show("Расстояние от края должно быть от " + OptionSatin.MinOtstup.ToString() + " до " + OptionSatin.MaxOtstup.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                CGS.b_Otstup = false;
            }
            else CGS.b_Otstup = true;
            OptionSatin.NumberOfProkolov = int.Parse(textbox5.Text);
            if ((int.Parse(textbox5.Text) < OptionSatin.MinNumberOfProkolov) || (int.Parse(textbox5.Text) > OptionSatin.MaxNumberOfProkolov))
            {
                System.Windows.MessageBox.Show("Количество проколов должно быть от " + OptionSatin.MinNumberOfProkolov.ToString() + " до " + OptionSatin.MaxNumberOfProkolov.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                CGS.b_NumberOfProkolov = false;
            }
            else CGS.b_NumberOfProkolov = true;
            OptionSatin.StartLenthStezhok = int.Parse(textbox6.Text);
            if ((int.Parse(textbox6.Text) < OptionSatin.MinLehthStezhok) || (int.Parse(textbox6.Text) > OptionSatin.MaxLenthStezhok))
            {
                System.Windows.MessageBox.Show("Длина шага должна быть от " + OptionSatin.MinLehthStezhok.ToString() + " до " + OptionSatin.MaxLenthStezhok.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                CGS.b_StartLenthStezhok = false;
            }
            else CGS.b_StartLenthStezhok = true;
            OptionSatin.EndLenthStezhok = int.Parse(textbox7.Text);
            if ((int.Parse(textbox7.Text) < OptionSatin.MinLehthStezhok) || (int.Parse(textbox7.Text) > OptionSatin.MaxLenthStezhok))
            {
                System.Windows.MessageBox.Show("Длина шага должна быть от " + OptionSatin.MinLehthStezhok.ToString() + " до " + OptionSatin.MaxLenthStezhok.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                CGS.b_EndLenthStezhok = false;
            }
            else CGS.b_EndLenthStezhok = true;
            this.Close();
            
        }
    }
}
