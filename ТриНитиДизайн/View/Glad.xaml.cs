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
    /// Логика взаимодействия для Glad.xaml
    /// </summary>
    public partial class Glad : Window
    {
        public Glad()
        {
            InitializeComponent();
            checkbox1.IsChecked = OptionGlad.PoChisluProkolov;
            checkbox2.IsChecked = OptionGlad.PoLenthStezhka;
            textbox1.Text = OptionGlad.LenthStep.ToString();
            textbox2.Text = OptionGlad.Rasshirenie.ToString();
            textbox3.Text = OptionGlad.NumberOfStezhkov.ToString();
            textbox4.Text = OptionGlad.Otstup.ToString();
            textbox5.Text = OptionGlad.NumberOfProkolov.ToString();
            textbox6.Text = OptionGlad.StartLenthStezhok.ToString();
            textbox7.Text = OptionGlad.EndLenthStezhok.ToString();

        }
        struct CorrectGladSetting
        {
            public bool b_LenthStep;
            public bool b_Rasshirenie;
            public bool b_NumberOfStezhkov;
            public bool b_Otstup;
            public bool b_NumberOfProkolov;
            public bool b_StartLenthStezhok;
            public bool b_EndLenthStezhok;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            CorrectGladSetting CGS;
            OptionGlad.PoChisluProkolov = checkbox1.IsChecked ?? false;
            OptionGlad.PoLenthStezhka = checkbox2.IsChecked ?? false;
            OptionGlad.LenthStep = int.Parse(textbox1.Text);
            if ((int.Parse(textbox1.Text) < OptionGlad.MinLenthStep) || (int.Parse(textbox1.Text) > OptionGlad.MaxLenthStep))
            {
                System.Windows.MessageBox.Show("Длина шага должна быть от " + OptionGlad.MinLenthStep.ToString() + " до " + OptionGlad.MaxLenthStep.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                CGS.b_LenthStep = false;
            }
            else CGS.b_LenthStep = true;
            OptionGlad.Rasshirenie = int.Parse(textbox2.Text);
            if ((int.Parse(textbox2.Text) < OptionGlad.MinRasshirenie) || (int.Parse(textbox2.Text) > OptionGlad.MaxRasshirenie))
            {
                System.Windows.MessageBox.Show("Расширение должно быть от " + OptionGlad.MinRasshirenie.ToString() + " до " + OptionGlad.MaxRasshirenie.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                CGS.b_Rasshirenie = false;
            }
            else CGS.b_Rasshirenie = true;
            OptionGlad.NumberOfStezhkov = int.Parse(textbox3.Text);
            if ((int.Parse(textbox3.Text) < OptionGlad.MinNumberOfStezhkov) || (int.Parse(textbox3.Text) > OptionGlad.MaxNumberOfStezhkov))
            {
                System.Windows.MessageBox.Show("Количество опорных стежков должно быть от " + OptionGlad.MinNumberOfStezhkov.ToString() + " до " + OptionGlad.MaxNumberOfStezhkov.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                CGS.b_NumberOfStezhkov = false;
            }
            else CGS.b_NumberOfStezhkov = true;
            OptionGlad.Otstup = int.Parse(textbox4.Text);
            if ((int.Parse(textbox4.Text) < OptionGlad.MinOtstup) || (int.Parse(textbox4.Text) > OptionGlad.MaxOtstup))
            {
                System.Windows.MessageBox.Show("Расстояние от края должно быть от " + OptionGlad.MinOtstup.ToString() + " до " + OptionGlad.MaxOtstup.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                CGS.b_Otstup = false;
            }
            else CGS.b_Otstup = true;
            OptionGlad.NumberOfProkolov = int.Parse(textbox5.Text);
            if ((int.Parse(textbox5.Text) < OptionGlad.MinNumberOfProkolov) || (int.Parse(textbox5.Text) > OptionGlad.MaxNumberOfProkolov))
            {
                System.Windows.MessageBox.Show("Количество проколов должно быть от " + OptionGlad.MinNumberOfProkolov.ToString() + " до " + OptionGlad.MaxNumberOfProkolov.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                CGS.b_NumberOfProkolov = false;
            }
            else CGS.b_NumberOfProkolov = true;
            OptionGlad.StartLenthStezhok = int.Parse(textbox6.Text);
            if ((int.Parse(textbox6.Text) < OptionGlad.MinLehthStezhok) || (int.Parse(textbox6.Text) > OptionGlad.MaxLenthStezhok))
            {
                System.Windows.MessageBox.Show("Длина шага должна быть от " + OptionGlad.MinLehthStezhok.ToString() + " до " + OptionGlad.MaxLenthStezhok.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                CGS.b_StartLenthStezhok = false;
            }
            else CGS.b_StartLenthStezhok = true;
            OptionGlad.EndLenthStezhok = int.Parse(textbox7.Text);
            if ((int.Parse(textbox7.Text) < OptionGlad.MinLehthStezhok) || (int.Parse(textbox7.Text) > OptionGlad.MaxLenthStezhok))
            {
                System.Windows.MessageBox.Show("Длина шага должна быть от " + OptionGlad.MinLehthStezhok.ToString() + " до " + OptionGlad.MaxLenthStezhok.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                CGS.b_EndLenthStezhok = false;
            }
            else CGS.b_EndLenthStezhok = true;
            this.Close();
            
        }
    }
}
