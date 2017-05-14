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

            ControlLine = new Figure(MainCanvas);
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
            System.Windows.Resources.StreamResourceInfo sri3 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\ArrowInLineRegim.cur", UriKind.Relative));
            ArrowCursor = new Cursor(sri3.Stream);
            System.Windows.Resources.StreamResourceInfo sri4 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Zoom-in.cur", UriKind.Relative));
            ZoomInCursor = new Cursor(sri4.Stream);
            System.Windows.Resources.StreamResourceInfo sri5 = Application.GetResourceStream(new Uri(@"..\..\..\Cursors\Zoom-out.cur", UriKind.Relative));
            ZoomOutCursor = new Cursor(sri5.Stream);
            mainGrid.Cursor = NormalCursor;
            MainCanvas.Cursor = NormalCursor;


            //ScaleTransform scaleTransform = new ScaleTransform(2, 2);
            //scaleTransform.CenterX = MainCanvas.Width / 2;
            //scaleTransform.CenterY = MainCanvas.Height / 2;
            //MainCanvas.RenderTransform = scaleTransform;
        }


        private void EditButtonEvent(object sender, RoutedEventArgs e)
        {
            Edit_Menu.IsEnabled = false;
            CloseAllTabs();
            ListFigure[IndexFigure].PointsCount.Clear();
            RedrawEverything(ListFigure, IndexFigure, false, false, MainCanvas);
            OptionRegim.regim = Regim.RegimDraw;
            ChangeFiguresColor(ListFigure, MainCanvas);
            MainCanvas.Cursor = HandCursor;            
        }

        private void CurcorButtonEvent(object sender, RoutedEventArgs e)
        {
            Edit_Menu.IsEnabled = true;
            CloseAllTabs();
            ListFigure[IndexFigure].PointsCount.Clear();
            RedrawEverything(ListFigure, IndexFigure, true, true, MainCanvas);
            OptionRegim.regim = Regim.RegimSelectFigureToEdit;
            ChangeFiguresColor(ListFigure, MainCanvas);
            ListFigure[IndexFigure].DrawOutSideRectanglePoints();
            MainCanvas.Cursor = NormalCursor;
        }
        private void ShowSpecialWindow(object sender, RoutedEventArgs e)
        {
            var SpecialWindow = new View.SpecialWindowWhenSelectedFigure();
            SpecialWindow.ShowDialog();
        }

    }
}
