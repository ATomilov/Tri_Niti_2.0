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

        private void SetItemSetka1(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            if (check.IsChecked == true)
            {
                foreach (MenuItem item in MenuSetka.Items)
                    if (item is MenuItem && !ReferenceEquals(item, check))
                        item.IsChecked = false;
                OptionSetka.MasshtabSetka = 0;
                SetGrid();
            }
        }

        private void SetItemSetka2(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            if (check.IsChecked == true)
            {
                foreach (MenuItem item in MenuSetka.Items)
                    if (item is MenuItem && !ReferenceEquals(item, check))
                        item.IsChecked = false;
                OptionSetka.MasshtabSetka = 10;
                SetGrid();
            }
        }

        private void SetItemSetka3(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            if (check.IsChecked == true)
            {
                foreach (MenuItem item in MenuSetka.Items)
                    if (item is MenuItem && !ReferenceEquals(item, check))
                        item.IsChecked = false;
                OptionSetka.MasshtabSetka = 20;
                SetGrid();
            }
        }

        private void SetItemSetka4(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            if (check.IsChecked == true)
            {
                foreach (MenuItem item in MenuSetka.Items)
                    if (item is MenuItem && !ReferenceEquals(item, check))
                        item.IsChecked = false;
                OptionSetka.MasshtabSetka = 50;
                SetGrid();
            }
        }

        private void SetItemSetka5(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            if (check.IsChecked == true)
            {
                foreach (MenuItem item in MenuSetka.Items)
                    if (item is MenuItem && !ReferenceEquals(item, check))
                        item.IsChecked = false;
                OptionSetka.MasshtabSetka = 100;
                SetGrid();
            }
        }

        private void SetItemSetka6(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            if (check.IsChecked == true)
            {
                foreach (MenuItem item in MenuSetka.Items)
                    if (item is MenuItem && !ReferenceEquals(item, check))
                        item.IsChecked = false;
                OptionSetka.MasshtabSetka = 200;
                SetGrid();
            }
        }

        private void SetDotOnGrid(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            OptionSetka.isDotOnGrid = check.IsChecked;
        }

    }
}
