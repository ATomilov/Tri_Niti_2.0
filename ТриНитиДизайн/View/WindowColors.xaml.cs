using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.VisualBasic;

namespace ТриНитиДизайн
{
    /// <summary>
    /// Логика взаимодействия для WindowColors.xaml
    /// </summary>
    public partial class WindowColors : Window
    {
        Brush brush;
        Polyline lines = new Polyline();

        public WindowColors()
        {
            InitializeComponent();
            Rect_main.Fill = new SolidColorBrush(OptionColor.colorNewBackground);
            lines.Stroke = new SolidColorBrush(OptionColor.colorInactive);
            MakeColorRectangles();
            MakeLine();
            button_accept.Focus();
            button_accept.BorderThickness = new Thickness(1.9);
        }

        private void MakeColorRectangles()                  //отрисовка цветных прямоугольников
        {
            int x = 32;
            int y = 20;
            int currentColumnColor = 0;
            int currentRowColor = 0;
            int[] colorsY = { 0, 98, 180, 236, 255, 0 };            //коэффициенты для перехода на новый цвет
            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    currentColumnColor = currentRowColor + colorsY[j];
                    Rectangle rec = new Rectangle();
                    rec.Height = 17;
                    rec.Width = 17;
                    string color = currentColumnColor.ToString("X");
                    while (color.Length < 6)
                    {
                        color = "0" + color;
                    }

                    var converter = new System.Windows.Media.BrushConverter();
                    brush = (Brush)converter.ConvertFromString("#" + color);
                    rec.Stroke = System.Windows.Media.Brushes.Black;
                    rec.Fill = brush;
                    canvasColors.Children.Add(rec);
                    Canvas.SetTop(rec, y);
                    Canvas.SetLeft(rec, x);
                    y += 19;
                }
                currentRowColor = colorsY[(i + 1) % 5] * 65536 + colorsY[((i + 1) / 5)] * 256;  //переход на новый столбец
                y = 20;
                x += 19;
            }

        }

        private void MakeLine()                 //отрисовка прямых линий в прямоугольнике
        {
            PointCollection pts = new PointCollection();
            int x = 24;
            int y = 158;
            pts.Add(new Point(x, y));

            for (int i = 0; i < 38; i++)
            {
                if (i % 2 == 0)
                {
                    y -= 14;
                    x += 6;
                }
                else
                {
                    y += 14;
                }
                pts.Add(new Point(x, y));
            }
            lines.Points = pts;
            lines.Stroke = System.Windows.Media.Brushes.Black;
            canvasColors.Children.Add(lines);
        }

        private void button_accept_Click(object sender, RoutedEventArgs e)              //если нажимаем создать, то цвет канвы меняется (цвет линий тоже должен меняться, но почему-то не работает)
        {
            OptionColor.colorNewBackground = (Rect_main.Fill as SolidColorBrush).Color;
            OptionColor.colorNewActive = (lines.Stroke as SolidColorBrush).Color;
            this.Close();
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)              //отмена
        {
            this.Close();
        }


        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)          //если по нажатию левой кнопки мыши находим прямоугольник, то заполняем прямоугольник
        {
            if (e.OriginalSource is Rectangle)
            {
                Rectangle clickedRectangle = (Rectangle)e.OriginalSource;
                lines.Stroke = clickedRectangle.Fill;
            }
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)         //заполняем линии
        {
            if (e.OriginalSource is Rectangle)
            {
                Rectangle clickedRectangle = (Rectangle)e.OriginalSource;
                Rect_main.Fill = clickedRectangle.Fill;
            }
        }

        private void button_delete_Click(object sender, RoutedEventArgs e)                      //при нажатии на удалить - возврат к фиолетовому цвету линий
        {
            OptionColor.colorNewActive = (Brushes.Violet as SolidColorBrush).Color;
            this.Close();
        }

        private void button_accept_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            button_accept.BorderThickness = new Thickness(1);
        }
    }
}
