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

        public MainWindow()
        {
            InitializeComponent();
            ChosenPts = new List<Point>();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Сохранить последний проект?", "Сообщение", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "Проект Три Нити Дизайн| *.tri";
                op.ShowDialog();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListFigure = new List<Figure>();
            ListFigure.Add(new Figure(MainCanvas));
            IndexFigure = 0;
            tabControl1.Visibility = Visibility.Hidden;
            tabControl2.Visibility = Visibility.Hidden;
            /// Курсоры
            System.Windows.Resources.StreamResourceInfo sri = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Hand.cur", UriKind.Relative));
            HandCursor = new Cursor(sri.Stream);
            System.Windows.Resources.StreamResourceInfo sri1 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Normal.cur", UriKind.Relative));
            NormalCursor = new Cursor(sri1.Stream);
            System.Windows.Resources.StreamResourceInfo sri2 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Sword.cur", UriKind.Relative));
            SwordCursor = new Cursor(sri2.Stream);
            mainGrid.Cursor = NormalCursor;
            MainCanvas.Cursor = NormalCursor;
        }


        private void EditButtonEvent(object sender, RoutedEventArgs e)
        {
            CloseAllTabs();
            RedrawEverything(ListFigure, IndexFigure, false, false, false, MainCanvas);
            OptionRegim.regim = Regim.RegimDraw;
            MainCanvas.Cursor = HandCursor;            
        }

        private void CurcorButtonEvent(object sender, RoutedEventArgs e)
        {
            CloseAllTabs();
            RedrawEverything(ListFigure, IndexFigure, true, true, false, MainCanvas);
            OptionRegim.regim = Regim.RegimSelectFigureToEdit;
            ListFigure[IndexFigure].DrawOutSideRectanglePoints();
            MainCanvas.Cursor = NormalCursor;
        }

    }
}
