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
    /// Логика взаимодействия для Razorvat.xaml
    /// </summary>
    public partial class Razorvat : Window
    {
        List<Figure> currentList;
        int index;
        Rectangle firstRec;
        Rectangle lastRec;
        Canvas canvas;

        public Razorvat(List<Figure> _currentList, int _index, Canvas _canvas)
        {
            InitializeComponent();
            currentList = _currentList;
            index = _index;
            canvas = _canvas;
            firstRec = GeometryHelper.DrawRectangle(currentList[index].PointStart, false, true,
                OptionDrawLine.strokeThickness, OptionColor.colorInactive, canvas);
            lastRec = GeometryHelper.DrawRectangle(currentList[index].PointEnd, false, false,
                OptionDrawLine.strokeThickness, OptionColor.colorInactive, canvas);
            yes_button.Focus();
            yes_button.BorderThickness = new Thickness(1.9);
        }

        private void OpenWindowsColor(object sender, RoutedEventArgs e)
        {
            WindowColors window = new WindowColors();
            window.ShowDialog();
        }

        private void yes_button_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Remove(firstRec);
            canvas.Children.Remove(lastRec);
            List<Figure> group = new List<Figure>(currentList);
            Figure brokenFigure = currentList[index];
            brokenFigure.Points.Add(new Point(-31231, 312312));
            for (int i = 0; i < index; i++ )
            {
                Figure fig = group[i];
                for (int j = index; j < group.Count; j++)
                {
                    Figure figureToDelete = group[j];
                    fig.groupFigures.Remove(figureToDelete);
                }
            }
            for (int i = index + 1; i < group.Count; i++)
            {
                Figure fig = group[i];
                for (int j = 0; j <= index; j++)
                {
                    Figure figureToDelete = group[j];
                    fig.groupFigures.Remove(figureToDelete);
                }
            }
            brokenFigure.groupFigures.Clear();
            brokenFigure.groupFigures.Add(brokenFigure);
            
            this.Close();
        }

        private void no_button_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Remove(firstRec);
            canvas.Children.Remove(lastRec);
            this.Close();
        }

        private void previous_button_Click(object sender, RoutedEventArgs e)
        {
            if(index !=0)
            {
                canvas.Children.Remove(firstRec);
                canvas.Children.Remove(lastRec);
                currentList[index].ChangeFigureColor(OptionColor.colorActive, false);
                index--;
                currentList[index].ChangeFigureColor(OptionColor.colorArc, false);
                firstRec = GeometryHelper.DrawRectangle(currentList[index].PointStart, false, true,
                OptionDrawLine.strokeThickness, OptionColor.colorInactive, canvas);
                lastRec = GeometryHelper.DrawRectangle(currentList[index].PointEnd, false, false,
                    OptionDrawLine.strokeThickness, OptionColor.colorInactive, canvas);
            }
        }

        private void next_button_Click(object sender, RoutedEventArgs e)
        {
            if (index != currentList.Count - 1)
            {
                canvas.Children.Remove(firstRec);
                canvas.Children.Remove(lastRec);
                currentList[index].ChangeFigureColor(OptionColor.colorActive, false);
                index++;
                currentList[index].ChangeFigureColor(OptionColor.colorArc, false);
                firstRec = GeometryHelper.DrawRectangle(currentList[index].PointStart, false, true,
                OptionDrawLine.strokeThickness, OptionColor.colorInactive, canvas);
                lastRec = GeometryHelper.DrawRectangle(currentList[index].PointEnd, false, false,
                    OptionDrawLine.strokeThickness, OptionColor.colorInactive, canvas);
            }
        }

        private void yes_button_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            yes_button.BorderThickness = new Thickness(1);
        }
    }
}
