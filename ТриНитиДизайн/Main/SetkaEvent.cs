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
        private void SetCenter(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            DrawCenter(check.IsChecked);
        }

        private void UnsetCenter(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            DrawCenter(check.IsChecked);
        }

        private void SetItemSetka1(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            if (check.IsChecked == true)
            {
                for (int i = 0; i < MenuSetka.Items.Count; i++)
                {
                    if (MenuSetka.Items[i] is MenuItem)
                    {
                        MenuItem item = (MenuItem)MenuSetka.Items[i];
                        if (!ReferenceEquals(item, check))
                        {
                            if (!item.Header.Equals("Рисовать по сетке") && !item.Header.Equals("Показать центр"))
                            {
                                item.IsChecked = false;
                                item.IsHitTestVisible = true;
                            }
                        }
                        else
                            item.IsHitTestVisible = false;
                    }
                }
                OptionSetka.MasshtabSetka = 0;
                SetGrid();
            }
        }

        private void SetItemSetka2(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            if (check.IsChecked == true)
            {
                for (int i = 0; i < MenuSetka.Items.Count; i++)
                {
                    if (MenuSetka.Items[i] is MenuItem)
                    {
                        MenuItem item = (MenuItem)MenuSetka.Items[i];
                        if (!ReferenceEquals(item, check))
                        {
                            if(!item.Header.Equals("Рисовать по сетке") && !item.Header.Equals("Показать центр"))
                            {
                                item.IsChecked = false;
                                item.IsHitTestVisible = true;
                            }
                        }
                        else
                            item.IsHitTestVisible = false;
                    }
                }
                OptionSetka.MasshtabSetka = 0.1;
                SetGrid();
            }
        }

        private void SetItemSetka3(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            if (check.IsChecked == true)
            {
                for (int i = 0; i < MenuSetka.Items.Count; i++)
                {
                    if (MenuSetka.Items[i] is MenuItem)
                    {
                        MenuItem item = (MenuItem)MenuSetka.Items[i];
                        if (!ReferenceEquals(item, check))
                        {
                            if(!item.Header.Equals("Рисовать по сетке") && !item.Header.Equals("Показать центр"))
                            {
                                item.IsChecked = false;
                                item.IsHitTestVisible = true;
                            }
                        }
                        else
                            item.IsHitTestVisible = false;
                    }
                }
                OptionSetka.MasshtabSetka = 0.2;
                SetGrid();
            }
        }

        private void SetItemSetka4(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            if (check.IsChecked == true)
            {
                for (int i = 0; i < MenuSetka.Items.Count; i++)
                {
                    if (MenuSetka.Items[i] is MenuItem)
                    {
                        MenuItem item = (MenuItem)MenuSetka.Items[i];
                        if (!ReferenceEquals(item, check))
                        {
                            if(!item.Header.Equals("Рисовать по сетке") && !item.Header.Equals("Показать центр"))
                            {
                                item.IsChecked = false;
                                item.IsHitTestVisible = true;
                            }
                        }
                        else
                            item.IsHitTestVisible = false;
                    }
                }
                OptionSetka.MasshtabSetka = 0.5;
                SetGrid();
            }
        }

        private void SetItemSetka5(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            if (check.IsChecked == true)
            {
                for (int i = 0; i < MenuSetka.Items.Count; i++)
                {
                    if (MenuSetka.Items[i] is MenuItem)
                    {
                        MenuItem item = (MenuItem)MenuSetka.Items[i];
                        if (!ReferenceEquals(item, check))
                        {
                            if (!item.Header.Equals("Рисовать по сетке") && !item.Header.Equals("Показать центр"))
                            {
                                item.IsChecked = false;
                                item.IsHitTestVisible = true;
                            }
                        }
                        else
                            item.IsHitTestVisible = false;
                    }
                }
                OptionSetka.MasshtabSetka = 1;
                SetGrid();
            }
        }

        private void SetItemSetka6(object sender, RoutedEventArgs e)
        {
             MenuItem check = (MenuItem)sender;
             if (check.IsChecked == true)
             {
                 for (int i = 0; i < MenuSetka.Items.Count; i++)
                 {
                     if (MenuSetka.Items[i] is MenuItem)
                     {
                         MenuItem item = (MenuItem)MenuSetka.Items[i];
                         if (!ReferenceEquals(item, check))
                         {
                             if (!item.Header.Equals("Рисовать по сетке") && !item.Header.Equals("Показать центр"))
                             {
                                 item.IsChecked = false;
                                 item.IsHitTestVisible = true;
                             }
                         }
                         else
                             item.IsHitTestVisible = false;
                     }
                 }
                 OptionSetka.MasshtabSetka = 2;
                 SetGrid();
             }
        }

        private void SetItemSetka7(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            if (check.IsChecked == true)
            {
                for (int i = 0; i < MenuSetka.Items.Count; i++)
                {
                    if (MenuSetka.Items[i] is MenuItem)
                    {
                        MenuItem item = (MenuItem)MenuSetka.Items[i];
                        if (!ReferenceEquals(item, check))
                        {
                            if (!item.Header.Equals("Рисовать по сетке") && !item.Header.Equals("Показать центр"))
                            {
                                item.IsChecked = false;
                                item.IsHitTestVisible = true;
                            }
                        }
                        else
                            item.IsHitTestVisible = false;
                    }
                }
                OptionSetka.MasshtabSetka = 5;
                SetGrid();
            }
        }
        private void SetItemSetka8(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            if (check.IsChecked == true)
            {
                for (int i = 0; i < MenuSetka.Items.Count; i++)
                {
                    if (MenuSetka.Items[i] is MenuItem)
                    {
                        MenuItem item = (MenuItem)MenuSetka.Items[i];
                        if (!ReferenceEquals(item, check))
                        {
                            if (!item.Header.Equals("Рисовать по сетке") && !item.Header.Equals("Показать центр"))
                            {
                                item.IsChecked = false;
                                item.IsHitTestVisible = true;
                            }
                        }
                        else
                            item.IsHitTestVisible = false;
                    }
                }
                OptionSetka.MasshtabSetka = 10;
                SetGrid();
            }
        }
        private void SetItemSetka9(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            if (check.IsChecked == true)
            {
                for (int i = 0; i < MenuSetka.Items.Count; i++)
                {
                    if (MenuSetka.Items[i] is MenuItem)
                    {
                        MenuItem item = (MenuItem)MenuSetka.Items[i];
                        if (!ReferenceEquals(item, check))
                        {
                            if(!item.Header.Equals("Рисовать по сетке") && !item.Header.Equals("Показать центр"))
                            {
                                item.IsChecked = false;
                                item.IsHitTestVisible = true;
                            }
                        }
                        else
                            item.IsHitTestVisible = false;
                    }
                }
                OptionSetka.MasshtabSetka = 20;
                SetGrid();
            }
        }

        private void SetDotOnGrid(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            OptionSetka.isDotOnGrid = check.IsChecked;
        }

        private void UnsetDotOnGrid(object sender, RoutedEventArgs e)
        {
            MenuItem check = (MenuItem)sender;
            OptionSetka.isDotOnGrid = check.IsChecked;
        }
    }
}
